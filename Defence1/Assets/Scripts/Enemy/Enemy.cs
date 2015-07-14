using UnityEngine;
using System.Collections;
using System;

public class Enemy : EnemyParent {
	
	protected Rigidbody _rigidbody;

	#region Attackable
	public AttackingControl<Tower> attackControl;
	public float attackingRadius = 8;
	public float injury;
	public float hitForce;
	public float attackInterval = 2;
	#endregion

	public DetectingControl<Tower> detectControl;
	public AimingControl aimControl;
	public float detectingRadius = 10;

	[SerializeField] public Transform rotationPart;
	[SerializeField] protected float rotateSpeed = 3;
	[SerializeField] protected float fireAngle = 0.01f;

	#region init functions
	public override void init(Vector2 pos) { 
		detectControl = new DetectingControl<Tower>(
			targetType: TargetType.Tower,
			armPosition: ()=>transform.position.toVec2(),
			detectingRadius: detectingRadius
		);

		aimControl = 
			new HorizontalRotationAimingControl (
				rotateSpeed: () => rotateSpeed,
				fireAngle: () => fireAngle,
				rotateToDirection: RotationMath.RotatePart (rotationPart, 0f),
				hasTarget: () => attackControl.currentTarget!=null,
				targetDirection: () => RotationMath.directionOf (
					attackControl.currentTarget.transform.position.toVec2() - 
					transform.position.toVec2()),
				initDirection: RotationMath.directionOf (Vector2.zero - 
					transform.position.toVec2())
			);

		attackControl = new AttackingControl<Tower> ();
		attackControl.attackingRadius = attackingRadius;
		attackControl.injury = injury;
		attackControl.hitForce = hitForce;
		attackControl.attackInterval = attackInterval;
		initAttackingControl ();
	}

	protected virtual void initAttackingControl() {
		initAttackingControl (null, null);
	}

	protected void initAttackingControl(
		Action<bool> fireEffect,
		Action<Tower, float> attackAction
	) {
		attackControl.init (
			armPosition: ()=>transform.position.toVec2(), 
			fireEffect: fireEffect, 
			attackAction: attackAction,
			isTargetOutOfDetecting: ()=>detectControl.isOutOfRange(attackControl.currentTarget),
			isTargetOutOfAttacking: ()=>detectControl.isOutOfRange(attackControl.currentTarget, attackControl.attackingRadius),
			detectTarget: (detectedCallback)=>detectControl.DetectSingleNearest(detectedCallback),
			isAimedAtTarget: ()=>aimControl.ready,
			updateOrientation: aimControl.updateOrientation
		);
	}
	#endregion


	void FixedUpdate () {
		if (attackControl != null)
			attackControl.Attack ();
		else
			Debug.Log ("attackControl is null");
		MoveStep ();
	}


	public virtual void hitBack(Vector3 force) {
		_rigidbody.AddForce (force);
	}

	// Use this for initialization
	void Start () {
		_rigidbody = GetComponent<Rigidbody> ();
	}

	public float stepForce = 10;

	protected virtual void MoveStep() {
		_rigidbody.AddForce (transform.forward * this.stepForce);
	}
}
