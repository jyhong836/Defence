using UnityEngine;
using System.Collections;

public class LaserTower : WeaponTower {
	public LaserEffect attackingaLaser;

	protected override void initAttackingControl() {
		initAttackingControl (
			(bool fire) => {
				attackingaLaser.showEffect = fire;
			},
			(Enemy target, float injury) => {
				attackingaLaser.setEndpoints (rotationPart.position, 
					target.transform.position);
				target.hpControl.hp -= injury;
			}
		);
	}
}
