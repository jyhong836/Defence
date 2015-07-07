using System;
using UnityEngine;

public static class Vector3Extension{
	
	public static Vector3 vectorOf(float x){
		return new Vector3 (x, x, x);
	}

	public static Vector3 fromVec2(Vector2 v2){
		return new Vector3 (v2.x, 0, v2.y);
	}

	public static Vector3 fromVec2(float x, float y){
		return new Vector3 (x, 0, y);
	}

	public static void setPos(this GameObject obj, float x, float y){
		obj.transform.position = new Vector3 (x, 0, y);
	}

	public static void setPos(this MonoBehaviour b, float x, float y){
		b.transform.position = new Vector3 (x, 0, y);
	}

	public static Vector2 getPos(this GameObject obj){
		var p = obj.transform.position;
		return new Vector2 (p.x, p.z);
	}

	public static Vector2 getPos(this MonoBehaviour obj){
		var p = obj.transform.position;
		return new Vector2 (p.x, p.z);
	}
}


