using UnityEngine;
using System.Collections;

/// <summary>
/// Fire tower, a range attacking tower.
/// </summary>
public class FireTower : WeaponTower {
	public ParticleSystem fireSystem;

	protected override void initAttackingControl() {
		attackControl.init (TargetType.Enemy, 
			()=>transform.position.toVec2(), 
			(bool fire)=>{
				if (fire) {
					if (!fireSystem.isPlaying) 
						fireSystem.Play();
				} else {
					if (!fireSystem.isStopped) 
						fireSystem.Stop();
				}
			}, 
			(HitpointControl target, float injury)=>{
				detectControl.DetectMultiple ((HitpointControl obj) => {
					obj.hp -= injury;
				}, attackControl.attackingRadius);
			},
			()=>detectControl.isOutOfRange(attackControl.currentTarget),
			()=>detectControl.isOutOfRange(attackControl.currentTarget, attackControl.attackingRadius),
			(detectedCallback)=>detectControl.DetectSingleNearest(detectedCallback)
		);
	}

//	float AttackTarget (DetectingControl detectControl) {
//		if (detectControl.DetectMultiple ((HitpointControl obj) => {
//			obj.hp -= attackControl.injury;
//		}, attackControl.attackingRadius)) {
//			attackControl.isFiring = true;
//			return attackControl.attackInterval;
//		} else {
//			attackControl.isFiring = false;
//			return 0;
//		}
//	}


//
//
//		var colliders = Physics.OverlapSphere (transform.position, 
//			attackControl.attackingRadius, 
//			attackControl.targetMask);
//		if (colliders.Length > 0) {
//			foreach (var collider in colliders) {
//				var enemy = collider.gameObject.GetComponent<Enemy> ();
//				enemy.hpControl.hp -= attackControl.injury;
//			}
//			attackControl.isFiring = true;
//
//			return attackControl.attackInterval;
//		} else {
//			attackControl.isFiring = false;
//			return 0;
//		}
//	}
}
