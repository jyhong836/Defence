using UnityEngine;
using System.Collections;

/// <summary>
/// Fire tower, a range attacking tower.
/// </summary>
public class FireTower : WeaponTower {
	public ParticleSystem fireSystem;

	protected override void initAttackingControl() {
		attackControl.init (AttackTargetType.Enemy, 
			()=>transform.position.toVec2(), 
			(fire, currentTarget, firePoint, injury) => {
				if (fire) {
					if (!fireSystem.isPlaying) 
						fireSystem.Play();
				} else {
					if (!fireSystem.isStopped) 
						fireSystem.Stop();
				}
			}, AttackTarget);
	}

	float AttackTarget () {
		var colliders = Physics.OverlapSphere (transform.position, 
			attackControl.attackingRadius, 
			attackControl.targetMask);
		if (colliders.Length > 0) {
			foreach (var collider in colliders) {
				var enemy = collider.gameObject.GetComponent<Enemy> ();
				enemy.hpControl.hp -= attackControl.injury;
			}
			attackControl.isFiring = true;

			return attackControl.attackInterval;
		} else {
			attackControl.isFiring = false;
			return 0;
		}
	}
}
