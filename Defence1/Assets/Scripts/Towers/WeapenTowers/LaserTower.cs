using UnityEngine;
using System.Collections;

public class LaserTower : WeaponTower {
	public LaserEffect attackingaLaser;

	protected override void initAttackingControl() {
		initAttackingControl (
			(bool fire) => {
				attackingaLaser.showEffect = fire;
			},
			(HitpointControl target, float injury) => {
				attackingaLaser.setEndpoints (rotationPart.position, 
					Vector3Extension.fromVec2(target.objectPosition));
				target.hp -= injury;
			}
		);
	}
}
