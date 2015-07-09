using UnityEngine;
using System.Collections;
using System;

public enum TowerMode {
	Attack, // auto resume from powerless
	Idle,
	AttackOnce // whill attack until out of power, not resume.
}

public class WeaponTower : Tower {
	
//	[SerializeField] protected Enemy currentTarget;
	protected bool isAttacking = false;

	public AttackingControl attackControl;
	public Transform rotationPart;

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

	protected virtual Action<bool, HitpointControl, Vector3, float> fireCallback{
		get{return null;}
	}
	
	public override void init(Vector2 pos){
		initParent (pos);
		attackControl = initAttackingControl ();
		attackControl.rotationPart = rotationPart;
		AttackingControl.setupAttackingControlFor (this);
	}
	protected virtual AttackingControl initAttackingControl() {
		return new AttackingControl (AttackTargetType.Enemy, ()=>transform.position.toVec2(), fireCallback, null);
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


	#region implemented abstract members of TowerParent
	public override float maxPower ()
	{
		return 60;
	}
	#endregion
}
