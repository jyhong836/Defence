using UnityEngine;
using System.Collections;

public class CannonTower : WeaponTower {
	public Bullet bulletPrefab;
	public float bulletSpeed;
	public float bulletAttackingRadius;

	protected override void initAttackingControl() {
		initAttackingControl (
			(bool fire)=>{},
			(Enemy target, float injury)=>{
				var bullet = Instantiate(bulletPrefab);
				bullet.init (rotationPart.position,
					bulletSpeed, 
					target.transform.position, injury,
					bulletAttackingRadius); 
			}
		);
	}
}
