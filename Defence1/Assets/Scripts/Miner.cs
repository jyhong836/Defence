using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Miner : MonoBehaviour {

	List<Ore> oreList = new List<Ore>();
//	int oreCollect = 0;
	public float oreCollectSpeed = 2; 
	public float oreUpdateSpeed = 2;
	public float workingRadius = 6;

	[SerializeField] Ore currentTarget;

	float oreTemp = 0;
	Action<int> oreCollected;

	public void init(Action<int> oreCollected){
		this.oreCollected = oreCollected;
	}
		
	void Start () {
		detectOres (); // only detect ores once here. Be careful, this method should be called after ores have shown to take effect.
	}
	
	// Game Mechanisms should be included into FixedUpdate.
	void FixedUpdate () {
		if (oreList.Count==0)
			return;
		Ore ore = oreList[0];
		if (ore.oreLeft<=0) {
//			Debug.Log("remove one ore, "+oreCollect+" idx: "+oreList.IndexOf(ore));
			oreList.Remove(ore);
			oreTemp = 0;
			currentTarget = null;
		} else {
			currentTarget = ore;
			oreTemp += (oreCollectSpeed*Time.deltaTime);
			if (oreTemp>=oreUpdateSpeed) {
				int oreAdd = (int)oreTemp;
				oreTemp -= oreAdd;
				oreAdd = (ore.oreLeft<oreAdd)?ore.oreLeft:oreAdd;
				ore.oreLeft -= oreAdd;

				if (oreCollected != null) {
					oreCollected (oreAdd); // we allow it to be null for prefab testing.
				}
//				Debug.Log(ore.oreLeft);
			}
		}
	}

	void Update(){
		if (currentTarget != null) {
			var start = transform.position;
			var end = currentTarget.transform.position;
			Debug.DrawLine (start,end,Color.yellow);
		}
	}

	void detectOres(){
		var colliders = Physics.OverlapSphere (transform.position,workingRadius,Masks.Ore);
		foreach(var c in colliders){
			var ore = c.gameObject.GetComponent <Ore>();
			oreList.Add (ore);
		}
	}

}
