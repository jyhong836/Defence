using UnityEngine;
using System.Collections;
using System;

[Serializable] public class AttackingControl {

	public HitpointControl currentTarget;
	Func<Vector2> armPosition;

	[SerializeField] public float attackingRadius = 8;
	[SerializeField] public float injury;
	[SerializeField] public float hitForce;
	[SerializeField] public float attackInterval = 2;

	private float nextAttackTime;

	private Func<bool> isTargetOutOfDetecting;
	private Func<bool> isTargetOutOfAttacking;
	Func<Action<HitpointControl>, bool> detectTarget;

	protected bool _isFiring;
	public bool isFiring{ 
		get{
			return _isFiring;
		} 
		set{
			if (value!=_isFiring) {
//				fireCallback (value, currentTarget, rotationPart.transform.position, injury);
				if (value)
					attackAction(currentTarget, injury);
				fireEffect (value);
			}
			_isFiring = value;
		} 
	}

//	protected AimingControl aimControl;
	public Func<bool> isAimedAtTarget;
	private Action<float> updateOrientation;

	/// <summary>
	/// Init the specified armPosition, fireCallback and attackTarget.
	/// </summary>
	/// <param name="armPosition">Arm position.</param>
	/// <param name="fireCallback">Fire callback. _fireCallback (HitpointControl 
	/// currentTarget, float injury). You can set this to null for using default.</param>
	/// <param name="attackTarget">Attack target. Should return next time interval 
	/// You can set this to null for using default.</param>
	public void init(
		TargetType targetType,
		Func<Vector2> armPosition, 
		Action<bool> fireEffect,
		Action<HitpointControl, float> attackAction, 
		Func<bool> isTargetOutOfDetecting,
		Func<bool> isTargetOutOfAttacking,
		Func<Action<HitpointControl>, bool> detectTarget,
		Func<bool> isAimedAtTarget,
		Action<float> updateOrientation
	) 
	{
//		this.targetType = targetType;
		this.armPosition = armPosition;
//		if (fireCallback == null)
//			this.fireCallback = _fireCallback;
//		else
//			this.fireCallback = fireCallback;
		if (fireEffect == null)
			this.fireEffect = (fire) => { };
		else
			this.fireEffect = fireEffect;
		if (attackAction == null)
			this.attackAction = (HitpointControl target,float injury)=>{target.hp -= injury;};
		else
			this.attackAction = attackAction;
		
		this.isAimedAtTarget = isAimedAtTarget;
		this.updateOrientation = updateOrientation;
		nextAttackTime = attackInterval;

		this.isTargetOutOfAttacking = isTargetOutOfAttacking;
		this.isTargetOutOfDetecting = isTargetOutOfDetecting;
		this.detectTarget = detectTarget;
	}

	public void Attack () {
		if (nextAttackTime <= 0) {
			nextAttackTime += attackTarget();
		} else {
			nextAttackTime -= Time.fixedDeltaTime;
			isFiring = false;
		}
	}

	/// <summary>
	/// Attacks the target.
	/// </summary>
	/// <returns>Time interval.</returns>
	private float attackTarget () {
		if (currentTarget == null || currentTarget.hp <= 0 || isTargetOutOfDetecting()) {
			if (!detectTarget ((HitpointControl hpc) => {currentTarget = hpc;}))
				currentTarget = null;
		} else if (isAimedAtTarget() && !isTargetOutOfAttacking()) {
			isFiring = true;
			return attackInterval;
		} else if (!isAimedAtTarget())
			updateOrientation (Time.fixedDeltaTime);
		isFiring = false;
		return 0;
	}

	private Action<HitpointControl, float> attackAction;
	private Action<bool> fireEffect;

	public void DrawAttackLine() {
		if (currentTarget != null && currentTarget.isAlive) {
			var start = Vector3Extension.fromVec2(this.armPosition());
			var end = Vector3Extension.fromVec2(currentTarget.objectPosition);
			if (isAimedAtTarget()) {
				if (isFiring)
					Debug.DrawLine (start, end, Color.red);
				else 
					Debug.DrawLine (start, end, Color.yellow);
			} else
				Debug.DrawLine (start, end, Color.green);
		}
	}
}

