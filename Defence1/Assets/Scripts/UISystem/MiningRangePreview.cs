using UnityEngine;
using System.Collections.Generic;

public class MiningRangePreview : RangePreviewParent {

	public List<Ore> ores = new List<Ore>();

	public override float upOffset {
		get { return 0f; }
	}

	public new void init(Transform parent,float radius){
		base.init (parent,radius);
	}

	void Update() {
		ores.Clear ();

		var colliders = Physics.OverlapSphere (transform.position,radius,Masks.Ore);
		foreach(var c in colliders){
			var ore = c.gameObject.GetComponent<Ore> ();
			ores.Add (ore);
		}
	}
}
