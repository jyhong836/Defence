using UnityEngine;
using System.Collections;

public static class ColorExtension {

	/// <summary>
	/// return a new Color with the same color but diffent alpha.
	/// </summary>
	/// <returns>The alpha.</returns>
	/// <param name="c">Original.</param>
	/// <param name="alpha">Alpha.</param>
	public static Color withAlpha(this Color c, float alpha){
		return new Color (c.r, c.g, c.b, alpha);
	}

}
