using UnityEngine;
using System.Collections;

public class AttackingRangePreview : RangePreviewParent {
	#region implemented abstract members of RangePreviewParent
	public override float upOffset {
		get {
			return 0.3f;
		}
	}
	#endregion

	public new void init(Transform parent, float radius) {
		base.init (parent, radius);
	}
}
