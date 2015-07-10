using UnityEngine;
using System.Collections;
using System;

public enum TowerMode {
	Attack, // auto resume from powerless
	Idle,
	AttackOnce // whill attack until out of power, not resume.
}

public class WeaponTower : Tower {
	
	protected bool isAttacking = false;

	public AttackingControl attackControl;
	public DetectingControl<Enemy> detectControl;
	public AimingControl aimControl;
	public float detectingRadius = 10;

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

	[SerializeField] public Transform rotationPart;
	[SerializeField] protected float rotateSpeed = 3;
	[SerializeField] protected float fireAngle = 0.01f;

	#region init functions

	protected override void init(Vector2 pos) {
		detectControl = new DetectingControl<Enemy>(TargetType.Enemy,(o)=>o.hpControl,
			()=>transform.position.toVec2(),
			detectingRadius
		);
		aimControl = 
			new HorizontalRotationAimingControl (
				rotateSpeed: () => rotateSpeed,
				fireAngle: () => fireAngle,
				rotateToDirection: RotationMath.RotatePart (rotationPart, 0f),
				hasTarget: () => attackControl.currentTarget!=null,
				targetDirection: () => RotationMath.directionOf (attackControl.currentTarget.objectPosition - 
					transform.position.toVec2())
			);
		initAttackingControl ();
	}

	protected virtual void initAttackingControl() {
		initAttackingControl (null, null);
	}

	protected void initAttackingControl(
		Action<bool> fireEffect,
		Action<HitpointControl, float> attackAction
	) {
		attackControl.init (()=>transform.position.toVec2(), 
			fireEffect, attackAction,
			()=>detectControl.isOutOfRange(attackControl.currentTarget),
			()=>detectControl.isOutOfRange(attackControl.currentTarget, attackControl.attackingRadius),
			(detectedCallback)=>detectControl.DetectSingleNearest(detectedCallback),
			()=>aimControl.ready,
			aimControl.updateOrientation
		);
	}

	#endregion

	void FixedUpdate () {
		if (isAttacking) {
			if (power < attackPowerUsage) {
				isAttacking = false;
			} else {
				powerLeft -= attackPowerUsage*Time.fixedDeltaTime;
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
