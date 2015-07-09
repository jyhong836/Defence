﻿using UnityEngine;
using System.Collections;

public enum TowerMode {
	Attack, // auto resume from powerless
	Idle,
	AttackOnce // whill attack until out of power, not resume.
}

public class WeaponTower : Tower {
	
//	[SerializeField] protected Enemy currentTarget;
	protected bool isAttacking = false;

	public AttackingControl attackControl;

//	protected bool _isFiring;
//	protected virtual bool isFiring{ get{
//			return _isFiring;
//		} 
//		set{
//			_isFiring = value;
//			if (value) {
//				
//			}
//		} 
//	}

	[SerializeField] protected float idlePowerUsage = 0.001f; // per sec
	[SerializeField] protected float attackPowerUsage = 0.01f; // per sec

	// mode
	[SerializeField] TowerMode mode = TowerMode.Attack;
	public TowerMode currentMode {
		get {return mode;}
		set {
			mode = value;
			switch (mode) 
			{
			case TowerMode.Attack: isAttacking = true; break;
			case TowerMode.AttackOnce: isAttacking = true; break;
			case TowerMode.Idle: break;
//				default: Debug.Log("ERROR in Tower mode switch: Unknown Mode");
			}
		}
	}

	// power
	[SerializeField] bool isOutOfPower = true;
	[SerializeField] float power = 0;
	public override float powerLeft {
		get { return power; }
		set {
			power = value;
			if (power>0) {
				isOutOfPower = false;
				if (mode==TowerMode.Attack) {
					isAttacking = true;
				}
			} else 
				isOutOfPower = true;
		}
	}
	
	protected override void init(Vector2 pos){
		initAttackingControl ();
	}

	protected virtual void initAttackingControl() {
		attackControl.init (AttackTargetType.Enemy, ()=>transform.position.toVec2(), null, null);
	}

	void FixedUpdate () {
		if (isAttacking) {
			if (power < attackPowerUsage) {
				isAttacking = false;
			} else {
				powerLeft -= attackPowerUsage*Time.fixedDeltaTime;
//				Attack();
				attackControl.Attack();
			}
		} else if (!isOutOfPower) {
			if (powerLeft < idlePowerUsage) {
				isOutOfPower = true;
			} else {
				powerLeft -= idlePowerUsage*Time.fixedDeltaTime;
			}
		}
	}

	// Use this for initialization
	void Start () {
		isAttacking = true; // FIXME attack start at begining
//		ChangeCurrentTarget ();
//		
	}
	
	// Update is called once per frame
	void Update () {
		if (isAttacking) {
			attackControl.DrawAttackLine ();
		}
	}

//	void StartAttack () {
//		if (currentTarget!=null)
//			isAttacking = true;
//		else {
//			ChangeCurrentTarget();
//		}
//	}

//	// control the attack time
//	protected void Attack () {
//		if (nextAttackTime <= 0) {
//			nextAttackTime += AttackTarget();
//		} else {
//			nextAttackTime -= Time.fixedDeltaTime;
//			isFiring = false;
//		}
//	}

//	/// <summary>
//	/// Attacks the target.
//	/// </summary>
//	/// <returns>Time interval.</returns>
//	protected virtual float AttackTarget () {
//		if (currentTarget == null || currentTarget.hpControl.hp <= 0 || isTargetOutOfRange) {
//			ChangeCurrentTarget ();
//		} else if (aimControl.ready) {
//			isFiring = true;
//			return attackInterval;
//		} else 
//			aimControl.updateOrientation (Time.fixedDeltaTime);
//		isFiring = false;
//		return 0;
//	}

//	/// <summary>
//	/// Changes the current target to a new enemy.
//	/// </summary>
//	/// <returns>Time for rotate to new target or 0.</returns>
//	protected virtual bool ChangeCurrentTarget () {
//		var colliders = Physics.OverlapSphere (transform.position,attackingRadius,Masks.Enemy);
//		if (colliders.Length > 0) {
//			//random pick one
//			var index = UnityEngine.Random.Range (0, colliders.Length);
//			var enemy = colliders [index].gameObject.GetComponent <Enemy>();
//			currentTarget = enemy;
////			isAttacking = true;
//
//			return true;
//		} else {
//			currentTarget = null;
////			isAttacking = false;
//			return false;
//		}
//	}

	#region implemented abstract members of TowerParent
	public override float maxPower ()
	{
		return 60;
	}
	#endregion
}
