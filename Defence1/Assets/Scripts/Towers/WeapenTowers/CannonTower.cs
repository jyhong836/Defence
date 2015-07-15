using UnityEngine;
using System.Collections;

public class CannonTower : WeaponTower {
	public Bullet bulletPrefab;
	public float bulletSpeed;
	public float bulletAttackingRadius;
	public float bulletMaxDistance = 200;

	protected override void initAttackingControl() {
		initAttackingControl (
			(bool fire)=>{},
			(Enemy target, float injury)=>{
				var bullet = Instantiate(bulletPrefab);
				bullet.init (
					transform.position,
					bulletSpeed, 
					target.transform.position, 
					injury,
					bulletAttackingRadius,
					hitForce,
					bulletMaxDistance
				); 
			}
		);
	}
}
