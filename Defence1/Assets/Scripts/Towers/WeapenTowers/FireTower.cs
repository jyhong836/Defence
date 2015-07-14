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
			(Enemy target, float injury) => {
				detectControl.DetectMultiple ((Enemy obj) => {
					obj.hpControl.hp -= injury;
				}, attackControl.attackingRadius);
			}
		);
	}
}
