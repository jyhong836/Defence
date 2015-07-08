using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public float moveSpeed = 5;
	public float shiftFactor = 3;
	public float basicHeight = 10;
	public float scaleSpeed = 0.1f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		moveCamera ();
	}

	float scale = 0.5f;
	void moveCamera(){
		var h = Input.GetAxis ("Horizontal");
		var v = Input.GetAxis ("Vertical");
		var shift = Input.GetButton ("Shift");
		var scroll = Input.mouseScrollDelta.y;

		var speed = shift ? shiftFactor * moveSpeed : moveSpeed;
		var delta = new Vector3 (h, scroll, v) * (speed * Time.deltaTime);
		transform.position += delta;

		scale = Mathf.Clamp01 (scale + scroll * scaleSpeed);
		transform.position = new Vector3 (transform.position.x, heightOfScale (scale), transform.position.z);
	}

	float heightOfScale(float scale){
		return basicHeight * 2f / (2f - scale);
	}
}
