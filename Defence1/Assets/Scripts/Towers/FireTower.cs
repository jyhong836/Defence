using UnityEngine;
using System.Collections;

/// <summary>
/// Fire tower, a range attacking tower.
/// </summary>
public class FireTower : Tower {


	protected override float AttackTarget () {
		var colliders = Physics.OverlapSphere (transform.position, attackingRadius, Masks.Enemy);
		if (colliders.Length > 0) {
			foreach (var collider in colliders) {
				var enemy = collider.gameObject.GetComponent<Enemy> ();
				this.isAttacking = true;
				enemy.lifeLeft -= injury;
			}
			
			// TODO caculate rotate angle for range attacking
//			float angle = 1;
			return 0;
		} else {
			isAttacking = false;
			return 0;
		}
	}
}
