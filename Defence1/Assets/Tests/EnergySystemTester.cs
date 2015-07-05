using UnityEngine;
using System.Collections;

public class EnergySystemTester : MonoBehaviour {

	public EnergyNode nodePrefab;

	public EnergyPoint pointPrefab;

	// Use this for initialization
	void Start () {
		var n1 = Instantiate (nodePrefab);
		n1.init (e => {
			Debug.Log ("Node 1 arrived");
			return e - 1f;
		});
		n1.setPos (-10, 0);
		var n2 = Instantiate (nodePrefab);
		n2.init ( e=>{
			Debug.Log ("Node 2 arrived");
			return e - 1f;
		});
		n2.setPos (-10,5);
		var point = Instantiate (pointPrefab);
		point.init (5,n1);

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
