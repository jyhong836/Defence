using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class Preview : MonoBehaviour {

	static Color validColor = Color.green;
	static Color invalideColor = Color.red;

	public Tower towerPrefab;

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

	public GameObject copyAModel(){
		var obj = Instantiate (this.gameObject);

		var preview = obj.GetComponent <Preview> ();
		obj.SetActive (true);
		Destroy (preview);

		return obj;
	}

	public static Preview makePreview<T> (T prefab) where T: Tower{
		var obj = Instantiate (prefab.gameObject);
		var tower = obj.GetComponent<T> ();
		destroyOptionally (tower);
		obj.name = "Preview Model";
		obj.tag = "Preview";

		var r = obj.AddComponent <Rigidbody> ();
		r.isKinematic = true;

		var preview = obj.AddComponent <Preview> ();
		preview.towerPrefab = prefab;

		return preview;
	}

	public static void destroyOptionally(UnityEngine.Object b){
		if(b!=null)
			Destroy (b);
	}
}
