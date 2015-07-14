using UnityEngine;
using System.Collections;

public interface IAttackable<T> where T: MonoBehaviour, IAliveable {
	AttackingControl<T> attackControl { get; }
//	float attackingRadius { get; }
//	float injury { get; }
//	float hitForce { get; }
//	float attackInterval  { get; }
}
