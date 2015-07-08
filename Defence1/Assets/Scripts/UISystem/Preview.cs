using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class Preview : MonoBehaviour {

	static Color validColor = Color.green;
	static Color invalideColor = Color.red;

	public List<RangePreviewParent> ranges = new List<RangePreviewParent> ();

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
		preview.ranges.ForEach (r=>Destroy(r.gameObject));
		obj.SetActive (true);
		Destroy (preview);

		return obj;
	}
}
