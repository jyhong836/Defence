using UnityEngine;
using System.Collections;

public static class ProbabilityMath {

	/// <summary>
	/// This function return random value x, x is in [-1, 1],
	///  with the center have a probability desentiy of 1 and the edge 0.135.
	/// </summary>
	/// <returns>The distribute.</returns>
	public static float normalDistribute(){
		while(true){
			var x = 2 * Random.value - 1f;
			var y = Mathf.Exp (-2 * x * x);
			if (Random.value < y)
				return x;
		}
	}

	/// <summary>
	/// return x in [-halfWidth, halfWidth] and y in [-halfHeight, halfHeight].
	/// </summary>
	/// <returns>The vec2 in box.</returns>
	/// <param name="width">Half width.</param>
	/// <param name="height">Half height.</param>
	public static Vector2 randomVec2InBox(float width, float height){
		var x = width * UnityEngine.Random.value;
		var y = height * UnityEngine.Random.value;
		return new Vector2 (x, y);
	}

	public static float oneOrZero(){
		return Random.value < 0.5f ? 0f : 1f;
	}

	public static bool halfChance(){
		return Random.value < 0.5f;
	}

	public static float value{
		get{return Random.value;}
	}

}
