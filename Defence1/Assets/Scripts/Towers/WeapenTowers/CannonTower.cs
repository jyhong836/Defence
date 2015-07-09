using UnityEngine;
using System.Collections;

public class CannonTower : WeaponTower {
	public Bullet bulletPrefab;
	public float bulletSpeed;
	public float bulletAttackingRadius;

	protected override void initAttackingControl() {
		attackControl.init (TargetType.Enemy,
			()=>transform.position.toVec2(), 
			(fire, currentTarget, firePoint, injury) => {
				if (fire) {
					var bullet = Instantiate(bulletPrefab);
					bullet.init (firePoint, bulletSpeed, 
						Vector3Extension.fromVec2(currentTarget.objectPosition), injury,
						bulletAttackingRadius);
				}
			}, null);
	}
}
