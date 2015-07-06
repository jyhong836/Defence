using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class EnergyNode : MonoBehaviour {

	public static readonly float transmissionRadius = 10;
	static readonly Color connectionColor = new Color(0.3f,0.3f,1.0f);

	public TowerParent tower;

	public List<EnergyNode> targetNodes = new List<EnergyNode>();
	Func<float,float> energyArrived;

	public void init(Func<float,float> energyArriveCallback, TowerParent tower){
		if (tower == null)
			throw new ArgumentNullException ("tower");
		this.energyArrived = energyArriveCallback;
		this.tower = tower;
		setupConnections ();
	}

	void Update(){
		var start = transform.position;
		targetNodes.ForEach (n=>{
			var end = n.transform.position;
			Debug.DrawLine (start,end,connectionColor);
		});
	}
		

	void setupConnections() {
		var colliders = Physics.OverlapSphere (transform.position, transmissionRadius, Masks.EnergySystem);
		foreach(var c in colliders){
			if (c.gameObject != gameObject) {
				var node = c.gameObject.GetComponent <EnergyNode> ();
				connectTo (node);
				node.connectTo (this);
			}
		}
	}

	public void clearAllConnections() {
		targetNodes.ForEach (n => n.dropConnectionTo (this));
	}

	public void connectTo(EnergyNode node){
		targetNodes.Add (node);
	}

	public void dropConnectionTo(EnergyNode node){
		var removed = targetNodes.Remove (node);
		if (!removed)
			Debug.Log ("removing error in EnergyNode!");
	}

	public void energyPointArrived(EnergyPoint point){
		var left = energyArrived (point.energy);
		if(left < 1e-5f){
			point.destroySelf ();
		}else{
			point.energy = left;
			redirectPoint (point);
		}
	}

	void redirectPoint(EnergyPoint point){
		if(targetNodes.Count > 0){
			var index = UnityEngine.Random.Range (0, targetNodes.Count);
			point.destination = targetNodes [index];
		} else{
			point.destroySelf ();
		}
	}
		
}
