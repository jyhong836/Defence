using UnityEngine;
using System.Collections;

/// <summary>
/// Fire tower, a range attacking tower.
/// </summary>
public class FireTower : WeaponTower {
	public ParticleSystem fireSystem;

	protected override void initAttackingControl() {
		attackControl.init (AttackTargetType.Enemy, 
			transform.position.toVec2(), 
			(fire, currentTarget, injury) => {
				if (fire) {
					if (!fireSystem.isPlaying) 
						fireSystem.Play();
				} else {
					if (!fireSystem.isStopped) 
						fireSystem.Stop();
				}
			}, AttackTarget);
	}

//	protected override bool isFiring {
//		get {return isFiring;} 
//		set {
//			if(_isFiring != value){
//				_isFiring = value;
//				if (value) {
//					if (!fireSystem.isPlaying) 
//						fireSystem.Play();
//				} else {
//					if (!fireSystem.isStopped) 
//						fireSystem.Stop();
//				}
//			}
//		}
//	}

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

			return 0;
		} else {
			attackControl.isFiring = false;
			return 0;
		}
	}
//
//	protected override bool ChangeCurrentTarget () {
//		isAttacking = true;
//		isFiring = false;
//		return true;
//	}
}
