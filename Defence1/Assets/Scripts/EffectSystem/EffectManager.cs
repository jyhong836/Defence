using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Linq;
using System;

public class EffectManager : MonoBehaviour {
	public Color energyConnectionColor;// = new Color(52f/255,244f/255,236f/255);
	public Color miningConnectionColor;// = Color.yellow;
	public Color attackingConnectionColor;// = new Color (1f, 0.4f, 0.5f);
	public static float connectionWidth = 0.35f;

	public LineRenderer connectionLinePrefab;
	public GameObject emptyPrefab;
	public GameObject rangePrefab;

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
			var preview = obj.GetComponent <Preview> ();
			if(preview != null){
				clearPointOverObject ();
				//Draw preview.
				var v2 = preview.getPos ();
				drawRangeOfTower (v2, preview.towerPrefab);
				drawEnergyConnectionsOfTower (v2, preview.towerPrefab);
				drawOtherConnectionsOfTower (v2, preview.towerPrefab);
			} else if (obj != lastPointOver) { 
				clearPointOverObject ();

				var tower = obj.GetComponent <Tower> ();
				if (tower != null && tower.alive) {
					var pos = tower.getPos ();
					drawRangeOfTower (pos, tower);
					drawEnergyConnectionsOfTower (pos, tower);
					drawOtherConnectionsOfTower (pos, tower);
				}
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

	void drawRange(Vector3 position, float radius, Color color){
		var r = Instantiate (rangePrefab);
		r.transform.position = position;
		r.transform.localScale = 2 * new Vector3 (radius, 0.1f, radius);
		r.transform.parent = pointOverEffectObject.transform;
		r.GetComponent <Renderer>().material.color = color;
	}
		
	void drawRangeOfTower(Vector2 pos, Tower t){
		var center = Vector3Extension.fromVec2 (pos);
		if(t is Generator || t is PowerRedirector){
			drawRange (center, EnergyNode.transmissionRadius, energyConnectionColor);
		}else if (t is Miner){
			drawRange (center, Miner.workingRadius, miningConnectionColor);
		}else if (t is WeaponTower){
			var weap = t as WeaponTower;
			drawRange (center, weap.attackControl.attackingRadius, attackingConnectionColor);
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

			Instantiate (connectionLinePrefab).makeConnection (start, end, 
				energyConnectionColor, connectionWidth, mode, pointOverEffectObject, "Energy Flow");
		}
	}

	void drawEnergyConnectionsOfTower(Vector2 pos, Tower t){
		var start = Vector3Extension.fromVec2 (pos);
		var connections = new List<EnergyNode> ();
		var colliders = Physics.OverlapSphere (start, EnergyNode.transmissionRadius);
		foreach(var c in colliders){
			var node = c.gameObject.GetComponent<EnergyNode> ();
			if(node!=null && node.shouldConnectTo (t.isRedirector)){
				connections.Add (node);
			}
		}
		drawEnergyConnections (start, t.isRedirector, connections);
	}

	void drawOtherConnectionsOfTower(Vector2 pos, Tower t){
		var start = Vector3Extension.fromVec2 (pos);
		if (t is Miner) {
			var colliders = Physics.OverlapSphere (start, Miner.workingRadius);
			foreach (var c in colliders) {
				var ore = c.gameObject.GetComponent<Ore> ();
				if (ore != null) {
					var line = Instantiate (connectionLinePrefab);
					line.makeConnection (start, ore.transform.position, miningConnectionColor, 
						connectionWidth, ConnectionMode.Receieve, pointOverEffectObject, "Ore Flow");
				}
			}
		}
	}
}
