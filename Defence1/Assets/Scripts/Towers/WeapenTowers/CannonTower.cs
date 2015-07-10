using UnityEngine;
using System.Collections;

public class CannonTower : WeaponTower {
	public Bullet bulletPrefab;
	public float bulletSpeed;
	public float bulletAttackingRadius;

	protected override void initAttackingControl() {
		attackControl.init (TargetType.Enemy,
			()=>transform.position.toVec2(), 
			(bool fire)=>{},
			(HitpointControl target, float injury)=>{
				var bullet = Instantiate(bulletPrefab);
				bullet.init (attackControl.rotationPart.position,
					bulletSpeed, 
					Vector3Extension.fromVec2(target.objectPosition), injury,
					bulletAttackingRadius); 
			},
			()=>detectControl.isOutOfRange(attackControl.currentTarget),
			()=>detectControl.isOutOfRange(attackControl.currentTarget, attackControl.attackingRadius),
			(detectedCallback)=>detectControl.DetectSingleNearest(detectedCallback)
		);
	}
}
