using UnityEngine;
using System.Collections;
using System;

public class Enemy : EnemyParent {
	
//	[SerializeField] protected float searchingRadius = 10;
//	[SerializeField] protected float attackingRadus = 5; // TODO add attacking code

//	public HitpointControl hpControl;
	protected Rigidbody _rigidbody;

	public AttackingControl attackControl;
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
			getTarget: (o)=>o.hpControl,
			armPosition: ()=>transform.position.toVec2(),
			detectingRadius: detectingRadius
		);
		aimControl = 
			new HorizontalRotationAimingControl (
				rotateSpeed: () => rotateSpeed,
				fireAngle: () => fireAngle,
				rotateToDirection: RotationMath.RotatePart (rotationPart, 0f),
				hasTarget: () => attackControl.currentTarget!=null,
				targetDirection: () => RotationMath.directionOf (attackControl.currentTarget.objectPosition - 
					transform.position.toVec2()),
				initDirection: RotationMath.directionOf (Vector2.zero - 
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
		attackControl.Attack();
		MoveStep ();
	}


	protected virtual void hitBack(Vector3 force) {
		_rigidbody.AddForce (force);
	}

	// Use this for initialization
	void Start () {
		_rigidbody = GetComponent<Rigidbody> ();
//		initMovement ();
//		ChangeCurrentTarget ();
	}

//	protected virtual void initMovement() {}

//	protected void ChangeCurrentTarget () {
//		var colliders = Physics.OverlapSphere (transform.position,searchingRadius,Masks.Tower);
//		if (colliders.Length > 0) {
//			//random pick one
//			var index = UnityEngine.Random.Range (0, colliders.Length);
//			var tower = colliders [index].gameObject.GetComponent <WeaponTower>();
//			currentTarget = tower;
//		} else {
//			currentTarget = null;
//		}
//	}

	// ---------------------------

//	public float injury = 10;

	public float stepForce = 10;
//	public float hurtedForce = 5;
//	private Vector3 direction;
//
//	public override void init(Vector2 pos) {
//		
//	}

//	protected override void initMovement() {
//		Vector3 start = this.transform.position;
//		Vector3 end = Vector3.zero;
//		direction = end - start;
//		direction.Normalize();
//	}
	
//	// Update is called once per frame
//	void Update () {
//		if (currentTarget == null) {
//			ChangeCurrentTarget ();
//		} else {
//			Attack();
//		}
//		UpdateDirection ();
//		MoveStep ();
//	}

//	void UpdateDirection() {
//		Vector3 end;
//		if (currentTarget != null) {
//			end = currentTarget.transform.position;
//		} else {
//			end = Vector3.zero;
//		}
//		Vector3 start = transform.position;
//		direction = end - start;
//		direction.Normalize();
//	}

	protected virtual void MoveStep() {
		_rigidbody.AddForce (transform.forward * this.stepForce);
	}

	//region this should be applied in Attacking
//	void HurtedBack() {
//		_rigidbody.AddForce (-hurtedForce * direction);
//	}
	//endregion

//	void Attack() {
//		Vector3 end = currentTarget.transform.position;
//		Vector3 start = transform.position;
//		Vector3 dist = end - start;
//		if (dist.magnitude > attackingRadus) {
//			return;
//		}
//		if (currentTarget.hpControl.hp > 0)
//			AttackTarget ();
//		else
//			ChangeCurrentTarget ();
//	}
//
//	void AttackTarget () {
//		if (nextAttackTime <= 0) {
//			currentTarget.hpControl.hp -= injury;
//			nextAttackTime += attackInterval;
//		} else {
//			nextAttackTime -= Time.fixedDeltaTime;
//		}
//	}
}
