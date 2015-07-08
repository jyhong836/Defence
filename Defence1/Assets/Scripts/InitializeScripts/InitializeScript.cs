using UnityEngine;
using System.Collections;

//Sadly, interfaces can't be displayed in Unity Editor, so I have to use abstract Monobehaviour...
public abstract class InitializeScript: MonoBehaviour{ 

	/// <summary>
	/// This method is called just before generating ores.
	/// </summary>
	/// <param name="manager">Manager.</param>
	public abstract void initialize (GameManager manager);

	public abstract int initOre();
}
