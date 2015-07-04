using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	public void init(Vector2 pos) {
		transform.position = Vector3Extension.fromVec2 (pos);
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
