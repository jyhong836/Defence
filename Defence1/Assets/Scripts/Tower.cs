using UnityEngine;
using System.Collections;

public enum TowerMode {
	Attack, // auto resume from powerless
	Idle,
	AttackOnce // whill attack until out of power, not resume.
}

public class Tower : MonoBehaviour {
	
	public float attackingRadius = 6;
	
	[SerializeField] Enemy currentTarget;
	public bool isAttacking = false;
	public float idlePowerUsage = 0.001f; // per sec
	public float attackPowerUsage = 0.01f; // per sec
	public float injury = 10;
	public float attackInterval = 2;
	private float nextAttackTime;

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
	private bool isOutOfPower = false; // TODO true;
	[SerializeField] float power = 0;
	public float powerLeft {
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

	// life	
	[SerializeField] float life = 100;
	public float lifeLeft {
		get {
			return life;
		}
		set {
			life = value;
			if (life<=0) {
				Destroy(gameObject);
			}
		}
	}

	void FixedUpdate () {
		if (isAttacking) {
			if (power < attackPowerUsage) {
				isAttacking = false;
			} else {
				powerLeft -= attackPowerUsage*Time.fixedDeltaTime;
			}
			Attack();
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
		ChangeCurrentTarget ();
		nextAttackTime = attackInterval;
	}
	
	// Update is called once per frame
	void Update () {
		if (isAttacking && currentTarget != null) {
			var start = transform.position;
			var end = currentTarget.transform.position;

			Debug.DrawLine (start,end,Color.red);
		}
	}

	void StartAttack () {
		if (currentTarget!=null)
			isAttacking = true;
		else {
			ChangeCurrentTarget();
		}
	}

	void Attack () {
		if (currentTarget == null) {
			ChangeCurrentTarget ();
			return;
		}
		if (currentTarget.lifeLeft > 0)
			AttackTarget ();
		else
			ChangeCurrentTarget ();
	}

	void AttackTarget () {
		if (nextAttackTime <= 0) {
			currentTarget.lifeLeft -= injury;
			nextAttackTime += attackInterval;
		} else {
			nextAttackTime -= Time.fixedDeltaTime;
		}
	}
	
	void ChangeCurrentTarget () {
		var colliders = Physics.OverlapSphere (transform.position,attackingRadius,Masks.Enemy);
		if (colliders.Length > 0) {
			//random pick one
			var index = UnityEngine.Random.Range (0, colliders.Length);
			var enemy = colliders [index].gameObject.GetComponent <Enemy>();
			currentTarget = enemy;
			isAttacking = true;
		} else {
			currentTarget = null;
			isAttacking = false;
		}
	}
}
