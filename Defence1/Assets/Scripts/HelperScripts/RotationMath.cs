using UnityEngine;
using System.Collections;

public static class RotationMath {

	public static float circle = 2 * Mathf.PI;

	public static float stayIn2Pi(float x){
		var x1 = x % circle;
		if (x1 < 0)
			x1 += circle;
		return x1;
	}
}
