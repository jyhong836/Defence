using UnityEngine;
using System.Collections;

public class CannonTower : WeaponTower {
	public Bullet bulletPrefab;
	public float bulletSpeed;
	public float bulletAttackingRadius;

	protected override bool isFiring {
		get {return isFiring;} 
		set {
			if(_isFiring != value){
				var bullet = Instantiate(bulletPrefab);
				bullet.init (this.gameObject.transform.position, bulletSpeed, 
				             currentTarget.gameObject.transform.position, injury,
				             bulletAttackingRadius);
			}
		}
	}

	protected override float AttackTarget () {
		if (currentTarget == null || currentTarget.lifeLeft <= 0 || isTargetOutOfRange) {
			ChangeCurrentTarget ();
		} else if (aimControl.ready) {
			isFiring = true;
			return attackInterval;
		} else 
			aimControl.updateOrientation (Time.fixedDeltaTime);
		isFiring = false;
		return 0;
	}
}
