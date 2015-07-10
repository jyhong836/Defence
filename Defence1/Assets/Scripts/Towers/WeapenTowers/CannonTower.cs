using UnityEngine;
using System.Collections;

public class CannonTower : WeaponTower {
	public Bullet bulletPrefab;
	public float bulletSpeed;
	public float bulletAttackingRadius;

	protected override void initAttackingControl() {
		initAttackingControl (
			(bool fire)=>{},
			(HitpointControl target, float injury)=>{
				var bullet = Instantiate(bulletPrefab);
				bullet.init (rotationPart.position,
					bulletSpeed, 
					Vector3Extension.fromVec2(target.objectPosition), injury,
					bulletAttackingRadius); 
			}
		);
	}
}
