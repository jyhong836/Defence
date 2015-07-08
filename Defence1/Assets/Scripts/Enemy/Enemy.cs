using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	[SerializeField] float searchingRadius = 10;
	[SerializeField] float attackingRadus = 5; // TODO add attacking code
	public float injury = 10;
	
	public float attackInterval = 2;
	private float nextAttackTime;

	[SerializeField] float life = 100;
	public float lifeLeft {
		get {
			return life;
		}
		set {
			if (value < life)
				HurtedBack();
			life = value;
			if (life<=0) {
				Destroy(gameObject);
			}
		}
	}

	public float stepForce = 10;
	public float hurtedForce = 5;
	private Vector3 direction;
	private Rigidbody _rigidbody;

	private WeaponTower currentTarget;

	public void init(Vector2 pos) {
		transform.position = Vector3Extension.fromVec2 (pos);
	}

	// Use this for initialization
	void Start () {
		_rigidbody = GetComponent<Rigidbody> ();
		Vector3 start = this.transform.position;
		Vector3 end = Vector3.zero;
		direction = end - start;
		direction.Normalize();
		ChangeCurrentTarget ();
		nextAttackTime = attackInterval;
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
	
	void ChangeCurrentTarget () {
		var colliders = Physics.OverlapSphere (transform.position,searchingRadius,Masks.Tower);
		if (colliders.Length > 0) {
			//random pick one
			var index = UnityEngine.Random.Range (0, colliders.Length);
			var tower = colliders [index].gameObject.GetComponent <WeaponTower>();
			currentTarget = tower;
		} else {
			currentTarget = null;
		}
	}

	void MoveStep() {
		_rigidbody.AddForce (direction * this.stepForce);
	}

	void HurtedBack() {
		_rigidbody.AddForce (-hurtedForce * direction);
	}

	void Attack() {
		Vector3 end = currentTarget.transform.position;
		Vector3 start = transform.position;
		Vector3 dist = end - start;
		if (dist.magnitude > attackingRadus) {
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
}
