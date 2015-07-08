using UnityEngine;
using System.Collections;

public abstract class RangePreviewParent : MonoBehaviour {

	public float radius;
	public float upOffset = 0f;

	public void init(Vector2 pos, float radius){
		this.radius = radius;
		var scale = 2 * radius;
		transform.localScale = new Vector3(scale,0.2f,scale);
		transform.position = Vector3Extension.fromVec2 (pos) + Vector3.up * upOffset;
	}
}
