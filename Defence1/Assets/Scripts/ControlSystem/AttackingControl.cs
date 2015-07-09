using UnityEngine;
using System.Collections;
using System;

public enum AttackTargetType {
	Enemy,
	Tower
}

[Serializable] public class AttackingControl {

	HitpointControl currentTarget;
	Func<Vector2> _armPosition;
	Vector2 armPosition{get{ return _armPosition();}}
	AttackTargetType targetType;
	public int targetMask{ 
		get{ 
			switch (targetType) {
			case AttackTargetType.Enemy:
				return Masks.Enemy;
			case AttackTargetType.Tower:
				return Masks.Tower;
			default:
				throw new UnityException ("Unknown targetType");
			}
		} }

	[SerializeField] public float attackingRadius = 8;
	[SerializeField] public float injury;
	[SerializeField] public float hitForce;
	[SerializeField] public float attackInterval = 2;

	private float nextAttackTime;

	protected bool isTargetOutOfRange {
		get {
			if (currentTarget==null)
				return true;
			Vector2 dist = armPosition;
			dist -= currentTarget.objectPosition;
			return dist.magnitude > attackingRadius;
		}
	}

	/// <summary>
	/// _fireCallback (bool fire, HitpointControl currentTarget, Vector3 firePoint, float injury)
	/// </summary>
	Action<bool, HitpointControl, Vector3, float> fireCallback;
	protected bool _isFiring;
	public bool isFiring{ 
		get{
			return _isFiring;
		} 
		set{
			if (value!=_isFiring) {
				fireCallback (value, currentTarget, rotationPart.transform.position, injury);
			}
			_isFiring = value;
		} 
	}

	/// <summary>
	/// float _attackTarget ()
	/// return next time interval
	/// </summary>
	Func<float> attackTarget; 

	protected AimingControl aimControl;
	[SerializeField] private Transform rotationPart;
	[SerializeField] private float rotateSpeed = 3;
	[SerializeField] private float fireAngle = 0.01f;

	/// <summary>
	/// Init the specified armPosition, fireCallback and attackTarget.
	/// </summary>
	/// <param name="armPosition">Arm position.</param>
	/// <param name="fireCallback">Fire callback. _fireCallback (HitpointControl 
	/// currentTarget, float injury). You can set this to null for using default.</param>
	/// <param name="attackTarget">Attack target. Should return next time interval 
	/// You can set this to null for using default.</param>
	public void init(
		AttackTargetType targetType,
		Func<Vector2> armPosition, 
		Action<bool, HitpointControl, Vector3, float> fireCallback, 
		Func<float> attackTarget) 
	{
		this.targetType = targetType;
		this._armPosition = armPosition;
		if (fireCallback == null)
			this.fireCallback = _fireCallback;
		else
			this.fireCallback = fireCallback;
		if (attackTarget == null)
			this.attackTarget = _attackTarget;
		else
			this.attackTarget = attackTarget;

		aimControl = 
			new HorizontalRotationAimingControl (
				rotateSpeed: () => rotateSpeed,
				fireAngle: () => fireAngle,
				rotateToDirection: RotationMath.RotatePart (rotationPart, 0f),
				hasTarget: () => currentTarget!=null,
				targetDirection: () => RotationMath.directionOf (currentTarget.objectPosition - this.armPosition)
			);
		nextAttackTime = attackInterval;
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
	private float _attackTarget () {
		if (currentTarget == null || currentTarget.hp <= 0 || isTargetOutOfRange) {
			ChangeCurrentTarget ();
		} else if (aimControl.ready) {
			isFiring = true;
			return attackInterval;
		} else 
			aimControl.updateOrientation (Time.fixedDeltaTime);
		isFiring = false;
		return 0;
	}

	private void _fireCallback (bool fire, HitpointControl currentTarget, Vector3 firePoint, float injury) {
		if (fire)
			currentTarget.hp -= injury;
	}

	/// <summary>
	/// Changes the current target to a new enemy.
	/// </summary>
	/// <returns>Time for rotate to new target or 0.</returns>
	private bool ChangeCurrentTarget () {
		var colliders = Physics.OverlapSphere (
			new Vector3(armPosition.x, 0, armPosition.y),
			attackingRadius,
			targetMask);
		if (colliders.Length > 0) {
			//random pick one
			var index = UnityEngine.Random.Range (0, colliders.Length);
			switch (targetType) {
			case AttackTargetType.Enemy: 
				var enemy = colliders [index].gameObject.GetComponent<Enemy> ();
				currentTarget = enemy.hpControl;
				break;
			case AttackTargetType.Tower:
				var tower = colliders [index].gameObject.GetComponent<Tower> ();
				if (tower.alive)
					currentTarget = tower.hpControl;
				else
					currentTarget = null;
				break;
			default:
				throw new UnityException ("Unknown mask: "+targetMask);
			}
		} else {
			currentTarget = null;
		}
		return currentTarget != null;
	}

	public void DrawAttackLine() {
		if (currentTarget != null && currentTarget.isAlive) {
			var start = this.rotationPart.position;
			var end = Vector3Extension.fromVec2(currentTarget.objectPosition);
			if (aimControl.ready)
				Debug.DrawLine (start, end, Color.red);
			else
				Debug.DrawLine (start, end, Color.green);
		}
	}
}

