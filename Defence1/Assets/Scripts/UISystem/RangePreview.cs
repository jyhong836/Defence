using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RangePreview : MonoBehaviour {
	public float radius;
	public List<EnergyNode> connections = new List<EnergyNode>();

	public void init(Transform parent,float radius){
		this.radius = radius;
		var scale = 2 * radius;
		transform.localScale = new Vector3(scale,0.2f,scale);
		transform.position = parent.position;
		transform.parent = parent;
	}

	void Update() {
		connections.Clear ();

		var colliders = Physics.OverlapSphere (transform.position,radius);
		foreach(var c in colliders){
			var node = c.gameObject.GetComponent<EnergyNode> ();
			if(node!=null){
				connections.Add (node);
			}
		}
	}
}
