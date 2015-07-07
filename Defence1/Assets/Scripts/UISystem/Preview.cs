using UnityEngine;
using System.Collections;

public class Preview : MonoBehaviour {

	static Color validColor = Color.green;
	static Color invalideColor = Color.red;


	Renderer render;

	void Start() {
		render = GetComponent <Renderer> ();
	}
	
	// Update is called once per frame
	void Update () {
		valid = collisionNum == 0;
	}

	private int collisionNum = 0;

	bool _valid = false;
	public bool valid{
		get{ return _valid; }
		set{
			if(value != _valid){
				_valid = value;
				render.material.color = value ? validColor : invalideColor;
			}
		}
	}

	void OnTriggerEnter(Collider other) {
		collisionNum += 1;	
	}
		

	void OnTriggerExit(Collider other) {
		collisionNum -= 1;
	}
}
