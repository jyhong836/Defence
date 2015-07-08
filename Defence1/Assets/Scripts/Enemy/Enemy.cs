using UnityEngine;
using System.Collections;

public class Enemy : EnemyParent {

	public float injury = 10;

	public float stepForce = 10;
	public float hurtedForce = 5;
	private Vector3 direction;

	public void init(Vector2 pos) {
		initParent (pos);
	}

	protected override void initMovement() {
		Vector3 start = this.transform.position;
		Vector3 end = Vector3.zero;
		direction = end - start;
		direction.Normalize();
	}
	
	// Update is called once per frame
	void Update () {
		if (currentTarget == null) {
			ChangeCurrentTarget ();
		} else {
			Attack();
		}
		UpdateDirection ();
		MoveStep ();
	}

	void UpdateDirection() {
		Vector3 end;
		if (currentTarget != null) {
			end = currentTarget.transform.position;
		} else {
			end = Vector3.zero;
		}
		Vector3 start = transform.position;
		direction = end - start;
		direction.Normalize();
	}

	void MoveStep() {
		_rigidbody.AddForce (direction * this.stepForce);
	}

	//region this should be applied in Attacking
//	void HurtedBack() {
//		_rigidbody.AddForce (-hurtedForce * direction);
//	}
	//endregion

	void Attack() {
		Vector3 end = currentTarget.transform.position;
		Vector3 start = transform.position;
		Vector3 dist = end - start;
		if (dist.magnitude > attackingRadus) {
			return;
		}
		if (currentTarget.hpControl.hp > 0)
			AttackTarget ();
		else
			ChangeCurrentTarget ();
	}

	void AttackTarget () {
		if (nextAttackTime <= 0) {
			currentTarget.hpControl.hp -= injury;
			nextAttackTime += attackInterval;
		} else {
			nextAttackTime -= Time.fixedDeltaTime;
		}
	}
}
