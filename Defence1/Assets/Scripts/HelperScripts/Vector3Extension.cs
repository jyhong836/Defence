using System;
using UnityEngine;

public static class Vector3Extension{
	
	public static Vector3 vectorOf(float x){
		return new Vector3 (x, x, x);
	}

	public static Vector3 fromVec2(Vector2 v2){
		return new Vector3 (v2.x, 0, v2.y);
	}
}


