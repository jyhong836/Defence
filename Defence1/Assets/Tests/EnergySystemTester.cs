using UnityEngine;
using System.Collections;

public class EnergySystemTester : MonoBehaviour {

	public GameManager gameManager;

	Generator station;

	// Use this for initialization
	void Start () {
		gameManager.createPowerRedirector (new Vector2(-10,0));
		gameManager.createPowerRedirector (new Vector2(-10,5));

		station = gameManager.createGenerator (new Vector2(-15f, 2));
//		StartCoroutine (destroyStationLater ());
	}

	IEnumerator destroyStationLater(){
		yield return new WaitForSeconds (4);
		station.destroySelf (gameManager);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
