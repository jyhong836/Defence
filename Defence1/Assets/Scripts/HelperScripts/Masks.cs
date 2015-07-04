using UnityEngine;
using System.Collections;

public static class Masks {

	public static readonly int Ore = LayerMask.GetMask ("Ore");
	public static readonly int Enemy = LayerMask.GetMask ("Enemy");
	public static readonly int Tower = LayerMask.GetMask ("Tower");
}
