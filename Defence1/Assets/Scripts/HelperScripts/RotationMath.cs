using UnityEngine;
using System.Collections;
using System;

public static class RotationMath {

	public static float circle = 2 * Mathf.PI;
	public static float rand2Deg = 180f / Mathf.PI;

	public static float stayIn2Pi(float x){
		var x1 = x % circle;
		if (x1 < 0)
			x1 += circle;
		return x1;
	}

	/// <summary>
	/// A1， A2 should be between 0 and 2Pi
	/// </summary>
	/// <returns>The angle.</returns>
	/// <param name="a1">A1.</param>
	/// <param name="a2">A2.</param>
	public static float approachingAngle(float start, float to){
		var raw = to - start;
		var abs = Mathf.Abs (raw);
		if (abs < Mathf.PI)
			return raw;
		else{
			return (abs-circle) * Mathf.Sign (raw);
		}
	}

	public static float directionOf(Vector2 v2){
		return stayIn2Pi (Mathf.Atan2 (v2.x,v2.y));
	}

	public static Action<float> RotatePart(Transform part, float frontDirection){
		return d => {
			part.eulerAngles = new Vector3 (0, rand2Deg*(frontDirection + d), 0);
		};
	}
}
