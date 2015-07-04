using UnityEngine;
using System.Collections;

public class EnergySystemTester : MonoBehaviour {

	public EnergyNode nodePrefab;
	public EnergyPoint pointPrefab;

	// Use this for initialization
	void Start () {
		var n1 = Instantiate (nodePrefab);
		n1.init (new Vector2(-10,0) ,e => {
			Debug.Log ("Node 1 arrived");
			return e - 1f;
		});
		var n2 = Instantiate (nodePrefab);
		n2.init (new Vector2(-10,5), e=>{
			Debug.Log ("Node 2 arrived");
			return e - 1f;
		});
		var point = Instantiate (pointPrefab);
		point.init (5,n1);

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
