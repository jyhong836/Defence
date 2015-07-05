using UnityEngine;
using System.Collections;

public class PowerStation : TowerParent {

	public void init(Vector2 pos){
		initParent ();

		transform.position = Vector3Extension.fromVec2 (pos);
	}



}
