using UnityEngine;
using System.Collections;

/// <summary>
/// Fire tower, a range attacking tower.
/// </summary>
public class FireTower : Tower {
	public ParticleSystem fireSystem;
	
	private bool _isFiring;
	protected override bool isFiring {
		get {return isFiring;} 
		set {
			if(_isFiring != value){
				_isFiring = value;
				if (value) {
					if (!fireSystem.isPlaying) 
						fireSystem.Play();
				} else {
					if (!fireSystem.isStopped)
						fireSystem.Stop();
				}
			}
		}
	}

	protected override float AttackTarget () {
		var colliders = Physics.OverlapSphere (transform.position, attackingRadius, Masks.Enemy);
		if (colliders.Length > 0) {
			foreach (var collider in colliders) {
				var enemy = collider.gameObject.GetComponent<Enemy> ();
				enemy.lifeLeft -= injury;
			}
			isFiring = true;

			return 0;
		} else {
			isFiring = false;
			return 0;
		}
	}

	protected override bool ChangeCurrentTarget () {
		isAttacking = true;
		isFiring = false;
		return true;
	}
}
