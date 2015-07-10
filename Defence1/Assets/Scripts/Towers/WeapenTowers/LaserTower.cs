using UnityEngine;
using System.Collections;

public class LaserTower : WeaponTower {
	public LaserEffect attackingaLaser;

	protected override void initAttackingControl() {
		attackControl.init (TargetType.Enemy, ()=>transform.position.toVec2 (),
			(fire, currentTarget, firePoint, injury) => {
				attackingaLaser.showEffect = fire;
				if (fire) {
					attackingaLaser.setEndpoints (firePoint, 
						Vector3Extension.fromVec2(currentTarget.objectPosition));
					currentTarget.hp -= injury;
				}
			},
			null
		);
	}
}
