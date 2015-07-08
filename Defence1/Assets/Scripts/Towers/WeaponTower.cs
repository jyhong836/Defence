using UnityEngine;
using System.Collections;

public enum TowerMode {
	Attack, // auto resume from powerless
	Idle,
	AttackOnce // whill attack until out of power, not resume.
}

public class WeaponTower : Tower {
	
	[SerializeField] protected Enemy currentTarget;
	protected bool isTargetOutOfRange {
		get {
			if (currentTarget==null)
				return true;
			Vector2 start = this.gameObject.getPos();
			start -= currentTarget.gameObject.getPos();
//			Debug.Log(start.sqrMagnitude+" " + attackingRadius);
			return start.magnitude > attackingRadius;
		}
	}
	protected bool isAttacking = false;
	
	protected bool _isFiring;
	protected virtual bool isFiring{ get{
			return _isFiring;
		} 
		set{
			_isFiring = value;
			if (value) {
				currentTarget.hpControl.hp -= injury;
			}
		} 
	}
	
	[SerializeField] protected float attackingRadius = 8;
	[SerializeField] protected float idlePowerUsage = 0.001f; // per sec
	[SerializeField] protected float attackPowerUsage = 0.01f; // per sec
	[SerializeField] protected float injury = 30;
	[SerializeField] protected float attackInterval = 2;
	[SerializeField] protected float rotateSpeed = 3;
	[SerializeField] protected float fireAngle = 0.01f;

	private float nextAttackTime;
	
	protected AimingControl aimControl;
	public Transform rotationPart;

	// mode
	[SerializeField] TowerMode mode = TowerMode.Attack;
	public TowerMode currentMode {
		get {return mode;}
		set {
			mode = value;
			switch (mode) 
			{
				case TowerMode.Attack: StartAttack(); break;
				case TowerMode.AttackOnce: StartAttack(); break;
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
					StartAttack ();
				}
			} else 
				isOutOfPower = true;
		}
	}

//	// life	
//	[SerializeField] float life = 100;
//	public float lifeLeft {
//		get {
//			return life;
//		}
//		set {
//			life = value;
//			if (life<=0) {
//				destroySelf ();
//			}
//		}
//	}
	
	public void init(Vector2 pos){
		initParent (pos);
		
		aimControl = 
			new HorizontalRotationAimingControl (
				rotateSpeed: () => rotateSpeed,
				fireAngle: () => fireAngle,
				rotateToDirection: RotationMath.RotatePart (rotationPart, 0f),
				hasTarget: () => currentTarget!=null,
				targetDirection: () => RotationMath.directionOf (currentTarget.getPos () - this.getPos ())
				);
	}

	void FixedUpdate () {
		if (isAttacking) {
			if (power < attackPowerUsage) {
				isAttacking = false;
			} else {
				powerLeft -= attackPowerUsage*Time.fixedDeltaTime;
				Attack();
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
		ChangeCurrentTarget ();
		nextAttackTime = attackInterval;
	}
	
	// Update is called once per frame
	void Update () {
		if (isAttacking && currentTarget != null) {
			var start = transform.position;
			var end = currentTarget.transform.position;
			if (aimControl.ready)
				Debug.DrawLine (start,end,Color.red);
			else
				Debug.DrawLine (start,end,Color.green);
		}
	}

	void StartAttack () {
		if (currentTarget!=null)
			isAttacking = true;
		else {
			ChangeCurrentTarget();
		}
	}

	// control the attack time
	protected void Attack () {
		if (nextAttackTime <= 0) {
			nextAttackTime += AttackTarget();
		} else {
			nextAttackTime -= Time.fixedDeltaTime;
			isFiring = false;
		}
	}

	/// <summary>
	/// Attacks the target.
	/// </summary>
	/// <returns>Time interval.</returns>
	protected virtual float AttackTarget () {
		if (currentTarget == null || currentTarget.hpControl.hp <= 0 || isTargetOutOfRange) {
			ChangeCurrentTarget ();
		} else if (aimControl.ready) {
			isFiring = true;
			return attackInterval;
		} else 
			aimControl.updateOrientation (Time.fixedDeltaTime);
		isFiring = false;
		return 0;
	}

	/// <summary>
	/// Changes the current target to a new enemy.
	/// </summary>
	/// <returns>Time for rotate to new target or 0.</returns>
	protected virtual bool ChangeCurrentTarget () {
		var colliders = Physics.OverlapSphere (transform.position,attackingRadius,Masks.Enemy);
		if (colliders.Length > 0) {
			//random pick one
			var index = UnityEngine.Random.Range (0, colliders.Length);
			var enemy = colliders [index].gameObject.GetComponent <Enemy>();
			currentTarget = enemy;
//			isAttacking = true;

			return true;
		} else {
			currentTarget = null;
//			isAttacking = false;
			return false;
		}
	}

	#region implemented abstract members of TowerParent
	public override float maxPower ()
	{
		return 60;
	}
	#endregion
}
