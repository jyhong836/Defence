using UnityEngine;
using System.Collections;

/// <summary>
/// Fire tower, a range attacking tower.
/// </summary>
public class FireTower : WeaponTower {
	public ParticleSystem fireSystem;

	protected override void initAttackingControl() {
		initAttackingControl (
			(bool fire) => {
				if (fire) {
					if (!fireSystem.isPlaying)
						fireSystem.Play ();
				} else {
					if (!fireSystem.isStopped)
						fireSystem.Stop ();
				}
			}, 
			(HitpointControl target, float injury) => {
				detectControl.DetectMultiple ((HitpointControl obj) => {
					obj.hp -= injury;
				}, attackControl.attackingRadius);
			}
		);
	}
}
