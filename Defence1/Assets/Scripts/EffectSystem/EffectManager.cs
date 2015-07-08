using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Linq;
using System;

public class EffectManager : MonoBehaviour {
	public static Color energyConnectionColor = new Color(52f/255,244f/255,236f/255);
	public static Color miningConnectionColor = Color.yellow;
	public static float connectionWidth = 0.35f;

	public LineRenderer connectionLinePrefab;
	public GameObject emptyPrefab;

	void Start(){
		pointOverEffectObject = Instantiate(emptyPrefab);
		pointOverEffectObject.name = "Effects";
	}

	void Update(){
		handleMousePoint ();
	}

	GameObject lastPointOver;
	GameObject pointOverEffectObject;

	void handleMousePoint(){
		var ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hitInfo;
		if(Physics.Raycast (ray, out hitInfo)){
			var obj = hitInfo.collider.gameObject;
			if (obj != lastPointOver || obj.tag == "Preview") { //Previews need to be refresh every frame.
				clearPointOverObject ();

				var tower = obj.GetComponent <Tower> ();
				if (tower != null && tower.alive) {
					mouseOverTower (tower);
				}

				var energyRangePreview = obj.transform.GetComponentInChildren <EnergyRangePreview> ();
				if (energyRangePreview != null) {
					var start = energyRangePreview.transform.position;
					var isRedirector = energyRangePreview.isRedirector;
					drawEnergyConnections (start, isRedirector, energyRangePreview.connections);
				}

				var minningRangePreview = obj.transform.GetComponentInChildren <MiningRangePreview> ();
				if (minningRangePreview != null)
					drawMiningConnections (minningRangePreview);

				lastPointOver = obj;
			}
		}else{
			clearPointOverObject ();
			lastPointOver = null;
		}
	}

	void clearPointOverObject() {
		if(pointOverEffectObject.transform.childCount != 0){
			Destroy(pointOverEffectObject.gameObject);
			pointOverEffectObject = Instantiate(emptyPrefab);
			pointOverEffectObject.name = "Effects";
		}
	}
		
	void drawConnections(Vector3 start, IEnumerable<Vector3> ends, Color connectionColor, ConnectionMode mode, string lineName){
		foreach(var p in ends){
			makeLine (start, p, connectionColor, connectionWidth, mode, pointOverEffectObject, lineName);
		}
	}

	void drawEnergyConnections(Vector3 start, bool isThisRedirector, IEnumerable<EnergyNode> targets){
		foreach (var node in targets) {
			var end = node.transform.position;
			ConnectionMode mode;
			if (isThisRedirector == node.isRedirector)
				mode = ConnectionMode.Standard;
			else if (isThisRedirector)
				mode = ConnectionMode.Send;
			else
				mode = ConnectionMode.Receieve;
			
			makeLine (start, end, energyConnectionColor, connectionWidth, mode, pointOverEffectObject, "Energy Flow");
		}
	}

	void mouseOverTower(Tower tower){
		var node = tower.energyNode;
		var start = node.transform.position;
		drawEnergyConnections (start, tower.isRedirector, node.targetNodes);
	}

	void drawMiningConnections(MiningRangePreview preview){
		var start = preview.transform.position;
		var ends = preview.ores.Select (n => n.transform.position);
		drawConnections (start,ends, miningConnectionColor, ConnectionMode.Receieve, "Ore Flow");
	}

	public LineRenderer makeLine(Vector3 start, Vector3 end, Color color,
		float lineWidth, ConnectionMode mode, GameObject parent, string lineName){

		var render = Instantiate (connectionLinePrefab);
		render.material.color = color;
		var lineObj = render.gameObject;
		lineObj.transform.parent = parent.transform;
		lineObj.name = lineName;

		var smallWidth = lineWidth / 5;
		float startWidth, endWidth;
		switch(mode){
		case ConnectionMode.Standard:
			startWidth = lineWidth;
			endWidth = lineWidth;
			break;
		case ConnectionMode.Send:
			startWidth = lineWidth;
			endWidth = smallWidth;
			break;
		case ConnectionMode.Receieve:
			startWidth = smallWidth;
			endWidth = lineWidth;
			break;
		default:
			throw new NotImplementedException ();
		}

		render.SetVertexCount (2);
		render.SetPosition (0, start);
		render.SetPosition (1, end);
		render.SetWidth (startWidth,endWidth);

		return render;
	}

	public enum ConnectionMode{
		Standard,
		Send,
		Receieve
	}
}
