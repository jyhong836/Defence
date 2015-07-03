using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Miner : MonoBehaviour {

	List<Ore> oreList = new List<Ore>();
	int oreCollect = 0;
	public float oreCollectSpeed = 2; 
	public float oreUpdateSpeed = 2;
	float oreTemp = 0;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (oreList.Count==0)
			return;
		Ore ore = oreList[0];
		if (ore.oreLeft<=0) {
			Debug.Log("remove one ore, "+oreCollect+" idx: "+oreList.IndexOf(ore));
			oreList.Remove(ore);
			oreTemp = 0;
		} else {
			oreTemp += (oreCollectSpeed*Time.deltaTime);
			if (oreTemp>=oreUpdateSpeed) {
				int oreAdd = (int)oreTemp;
				oreTemp -= oreAdd;
				oreAdd = (ore.oreLeft<oreAdd)?ore.oreLeft:oreAdd;
				ore.oreLeft -= oreAdd;
				oreCollect += oreAdd;
				Debug.Log(ore.oreLeft);
			}
		}
	}

	void OnTriggerEnter(Collider other) {
		if (other.tag == "Ore") {
			Vector3 OrePosition = other.gameObject.transform.position;
			Ore o1 = other.gameObject.GetComponent<Ore>();
//			Debug.Log ("find one ore in "+OrePosition+" with "+o1.oreLeft+" ores");
			oreList.Add(o1);
		}
	}
}
