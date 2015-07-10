using UnityEngine;
using System.Collections;

public class LaserTower : WeaponTower {
	public LaserEffect attackingaLaser;

	protected override void initAttackingControl() {
		attackControl.init (TargetType.Enemy, ()=>transform.position.toVec2 (),
			(bool fire) => {
				attackingaLaser.showEffect = fire;
//				if (fire) {
//					attackingaLaser.setEndpoints (attackControl.rotationPart.position, 
//						Vector3Extension.fromVec2(currentTarget.objectPosition));
//				}
			},
			(HitpointControl target, float injury) => {
				attackingaLaser.setEndpoints (attackControl.rotationPart.position, 
					Vector3Extension.fromVec2(target.objectPosition));
				target.hp -= injury;
			},
			()=>detectControl.isOutOfRange(attackControl.currentTarget),
			()=>detectControl.isOutOfRange(attackControl.currentTarget, attackControl.attackingRadius),
			(detectedCallback)=>detectControl.DetectSingleNearest(detectedCallback)
		);
	}
}
