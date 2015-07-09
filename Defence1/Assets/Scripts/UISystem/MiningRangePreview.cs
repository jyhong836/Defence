using UnityEngine;
using System.Collections.Generic;

public class MiningRangePreview : RangePreviewParent {

	public override float upOffset {
		get { return 0f; }
	}

	public new void init(Transform parent,float radius){
		base.init (parent,radius);
	}

	public List<Ore> oresInRange() {
		var ores = new List<Ore>();

		var colliders = Physics.OverlapSphere (transform.position,radius,Masks.Ore);
		foreach(var c in colliders){
			var ore = c.gameObject.GetComponent<Ore> ();
			ores.Add (ore);
		}
		return ores;
	}
}
