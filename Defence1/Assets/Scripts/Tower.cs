using UnityEngine;
using System.Collections;

public class Tower : MonoBehaviour {
	
	public float workingRadius = 6;
	
	[SerializeField] Enemy currentTarget;
//	[SerializeField] bool isWorking = true;
	private bool isAttacking = false;
	private bool isOutOfPower = true;
	private float power = 0;
	public float idlePowerUsage = 1;
	public float attackPowerUsage = 10;
	public float injury = 10;
	public float attackInterval = 2;
	private float nextAttackTime = attackInterval;

	void FixedUpdate () {
		if (isAttacking) {
			if (power < attackPowerUsage) {
				isAttacking = false;
			} else {
				power -= attackPowerUsage;
			}
			Attack();
		} else if (!isOutOfPower) {
			if (power < idlePowerUsage) {
				isOutOfPower = true;
			} else {
				power -= idlePowerUsage;
			}
		}
	}

	// Use this for initialization
	void Start () {
		ChangeCurrentTarget ();
	}
	
	// Update is called once per frame
	void Update () {
		if (currentTarget != null) {
			var start = transform.position;
			var end = currentTarget.transform.position;
			
			Debug.DrawLine (start,end,Color.red);
		}
	}

	void Attack () {
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
		var colliders = Physics.OverlapSphere (transform.position,workingRadius,Masks.Enemy);
		if (colliders.Length > 0) {
			//random pick one
			var index = UnityEngine.Random.Range (0, colliders.Length);
			var enemy = colliders [index].gameObject.GetComponent <Enemy>();
			currentTarget = enemy;
		} else {
			currentTarget = null;
		}
	}
}
