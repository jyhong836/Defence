using UnityEngine;
using System.Collections;

public abstract class RangePreviewParent : MonoBehaviour {

	public float radius;
	public abstract float upOffset {get;}

	protected void init(Transform parent,float radius){
		this.radius = radius;
		var scale = 2 * radius;
		transform.localScale = new Vector3(scale,0.2f,scale);
		transform.position = parent.position + Vector3.up * upOffset;
		transform.parent = parent;
	}
}
