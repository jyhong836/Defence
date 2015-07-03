using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public float moveSpeed = 5;
	public float shiftFactor = 3;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		moveCamera ();
	}

	void moveCamera(){
		var h = Input.GetAxis ("Horizontal");
		var v = Input.GetAxis ("Vertical");
		var shift = Input.GetButton ("Shift");

		var speed = shift ? shiftFactor * moveSpeed : moveSpeed;
		var delta = new Vector3 (h, 0f, v) * (speed * Time.deltaTime);

		transform.position += delta;
	}
}
