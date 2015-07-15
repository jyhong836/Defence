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

	#region Attackable

	public AttackingControl<Enemy> attackControl;
	public float attackingRadius = 8;
	public float injury;
	public float hitForce;
	public float attackInterval = 2;

	#endregion

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
		attackControl = new AttackingControl<Enemy> ();
		attackControl.attackingRadius = attackingRadius;
		attackControl.injury = injury;
		attackControl.hitForce = hitForce;
		attackControl.attackInterval = attackInterval;
		detectControl = new DetectingControl<Enemy>(TargetType.Enemy,
			()=>transform.position.toVec2(),
			detectingRadius
		);

		aimControl = 
			new HorizontalRotationAimingControl (
				rotateSpeed: () => rotateSpeed,
				fireAngle: () => fireAngle,
				rotateToDirection: RotationMath.RotatePart (rotationPart, 0f),
				hasTarget: () => attackControl.currentTarget!=null,
				targetDirection: () => RotationMath.directionOf (
					attackControl.currentTarget.transform.position.toVec2() - 
					transform.position.toVec2())
			);
		initAttackingControl ();
	}

	protected virtual void initAttackingControl() {
		initAttackingControl (null, null);
	}

	protected void initAttackingControl(
		Action<bool> fireEffect,
		Action<Enemy, float> attackAction
	) {
		attackControl.init (
			armPosition: () => transform.position.toVec2 (), 
			fireEffect: fireEffect,
			attackAction: attackAction, 
			isTargetOutOfDetecting: () => detectControl.isOutOfRange (attackControl.currentTarget),
			isTargetOutOfAttacking: () => detectControl.isOutOfRange (attackControl.currentTarget, attackControl.attackingRadius),
			detectTarget: (detectedCallback) => detectControl.DetectSingleNearest (detectedCallback),
			isAimedAtTarget: () => aimControl.ready,
			updateOrientation: aimControl.updateOrientation
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
		isAttacking = true; // FIXME attack start at begining?
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
