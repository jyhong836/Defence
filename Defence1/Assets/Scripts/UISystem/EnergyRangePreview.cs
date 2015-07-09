using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnergyRangePreview : RangePreviewParent {
	public bool isRedirector;

	public override float upOffset {
		get { return 0f; }
	}

	public void init(Transform parent,float radius, bool isRedirector, bool showRange){
		init (parent,radius);
		this.isRedirector = isRedirector;
		var mesh = GetComponent <MeshRenderer>();
		mesh.enabled = showRange;
	}

	public List<EnergyNode> connections() {
		var connections = new List<EnergyNode> ();

		var colliders = Physics.OverlapSphere (transform.position,radius);
		foreach(var c in colliders){
			var node = c.gameObject.GetComponent<EnergyNode> ();
			if(node!=null && node.shouldConnectTo (isRedirector)){
				connections.Add (node);
			}
		}
		return connections;
	}
}
