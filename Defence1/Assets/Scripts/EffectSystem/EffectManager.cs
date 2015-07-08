using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Linq;

public class EffectManager : MonoBehaviour {
	public static Color connectionColor = new Color (0.3f, 0.3f, 1f);
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

				var rangePreview = obj.transform.GetComponentInChildren <RangePreview> ();
				if (rangePreview != null)
					drawPreivewConnections (rangePreview);

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
		
	void drawConnections(Vector3 start, IEnumerable<Vector3> ends){
		foreach(var p in ends){
			makeLine (connectionLinePrefab, start, p, connectionColor, connectionColor,
				connectionWidth, 0, pointOverEffectObject, "Connection");
		}
	}

	void mouseOverTower(Tower tower){
		var node = tower.energyNode;
		var start = node.transform.position;
		var ends = node.targetNodes.Select (n => n.transform.position);
		drawConnections (start,ends);
	}

	void drawPreivewConnections(RangePreview preview){
		var start = preview.transform.position;
		var ends = preview.connections.Select (n => n.transform.position);
		drawConnections (start,ends);
	}

	public static LineRenderer makeLine(LineRenderer prefab ,Vector3 start, Vector3 end, Color colorStart, Color colorEnd,
		float widthStart, float widthEnd, GameObject parent, string lineName){

		var render = Instantiate (prefab);
		var lineObj = render.gameObject;
		lineObj.transform.parent = parent.transform;
		lineObj.name = lineName;

		render.SetVertexCount (2);
		render.SetPosition (0, start);
		render.SetPosition (1, end);
		render.SetColors (colorStart, colorEnd);
		render.SetWidth (widthStart,widthEnd);

		return render;
	}
}
