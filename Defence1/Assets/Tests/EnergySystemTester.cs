using UnityEngine;
using System.Collections;

public class EnergySystemTester : MonoBehaviour {

	public GameManager gameManager;
	public EnergyNode nodePrefab;

	PowerStation station;

	// Use this for initialization
	void Start () {
		var n1 = Instantiate (nodePrefab);
		n1.setPos (-10, 0);
		n1.init (e => {
			Debug.Log ("Node 1 arrived");
			return e - 1f;
		});
		var n2 = Instantiate (nodePrefab);
		n2.setPos (-10,5);
		n2.init ( e=>{
			Debug.Log ("Node 2 arrived");
			return e - 1f;
		});

		station = gameManager.createPowerStation (new Vector2(-15f, 2));

	}

	IEnumerator destroyStationLayer(){
		yield return new WaitForSeconds (4);

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
