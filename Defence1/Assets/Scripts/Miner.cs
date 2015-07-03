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
		List<Ore> oreToRemoveList = new List<Ore> ();
		foreach(Ore ore in oreList) {
			if (ore.oreLeft<=0) {
				oreToRemoveList.Add(ore);
			} else {
				oreTemp += (oreCollectSpeed*Time.deltaTime);
//				Debug.Log("temp"+oreTemp);
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
		foreach (Ore ore in oreToRemoveList) {
			oreList.Remove(ore);
			Debug.Log("remove one ore, "+oreCollect);
		}
	}

	void OnTriggerEnter(Collider other) {
		if (other.tag == "Ore") {
			Vector3 OrePosition = other.gameObject.transform.position;
			Ore o1 = other.gameObject.GetComponent<Ore>();
			Debug.Log ("find one ore in "+OrePosition+" with "+o1.oreLeft+" ores");
			oreList.Add(o1);
		}
	}
}
