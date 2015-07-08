using UnityEngine;
using System.Collections;

public class LaserTower : WeaponTower {
	public LaserEffect attackingaLaser;

	protected override void initAttackingControl() {
		attackControl.init (AttackTargetType.Enemy, transform.position.toVec2 (),
			(fire, currentTarget, injury) => {
				attackingaLaser.showEffect = fire;
				if (fire) {
					attackingaLaser.setEndpoints (firePoint, currentTarget.objectPosition);
					currentTarget.hp -= injury;
				}
			},
			null
		);
	}

	public Vector3 firePoint{
		get{return transform.position;}
	}
}
