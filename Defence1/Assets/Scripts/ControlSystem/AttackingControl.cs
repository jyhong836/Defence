using UnityEngine;
using System.Collections;
using System;

public class AttackingControl <T> where T: MonoBehaviour, IAliveable {

//	public HitpointControl currentTarget;
	public T currentTarget;
	Func<Vector2> armPosition;

	public float attackingRadius = 8;
	public float injury;
	public float hitForce;
	public float attackInterval = 2;

	private float nextAttackTime;

	private Func<bool> isTargetOutOfDetecting;
	private Func<bool> isTargetOutOfAttacking;
	Func<Action<T>, bool> detectTarget;

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

	public Func<bool> isAimedAtTarget;
	private Action<float> updateOrientation;

	public void init(
		Func<Vector2> armPosition, 
		Action<bool> fireEffect,
		Action<T, float> attackAction, 
		Func<bool> isTargetOutOfDetecting,
		Func<bool> isTargetOutOfAttacking,
		Func<Action<T>, bool> detectTarget,
		Func<bool> isAimedAtTarget,
		Action<float> updateOrientation
	) 
	{
		this.armPosition = armPosition;
		if (fireEffect == null)
			this.fireEffect = (fire) => { };
		else
			this.fireEffect = fireEffect;
		if (attackAction == null)
			this.attackAction = (T target,float injury)=>{target.hpControl.hp -= injury;};
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
		if (currentTarget == null || currentTarget.hpControl.hp <= 0 || isTargetOutOfDetecting()) {
			if (!detectTarget ((T target) => {currentTarget = target;}))
				currentTarget = null;
//			else if (currentTarget.hpControl.hp>0) {
//				attackTarget();
//			}
		} else if (!isTargetOutOfAttacking() && isAimedAtTarget()) {
			isFiring = true;
			return attackInterval;
		} else if (!isAimedAtTarget())
			updateOrientation (Time.fixedDeltaTime);
		isFiring = false;
		return 0;
	}

	private Action<T, float> attackAction;
	private Action<bool> fireEffect;

	/// <summary>
	/// Draw attacking line in debug window.
	/// </summary>
	public void DrawAttackLine() {
		if (currentTarget != null && currentTarget.alive) {
			var start = Vector3Extension.fromVec2(this.armPosition());
			var end = currentTarget.transform.position;
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

