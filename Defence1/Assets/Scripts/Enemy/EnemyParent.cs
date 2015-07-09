using UnityEngine;
using System.Collections;

public class EnemyParent : MonoBehaviour {

	[SerializeField] protected float searchingRadius = 10;
	[SerializeField] protected float attackingRadus = 5; // TODO add attacking code

	public HitpointControl hpControl;
	protected Rigidbody _rigidbody;

	public float attackInterval = 2;
	protected float nextAttackTime;

	protected WeaponTower currentTarget;

	public void initParent(Vector2 pos) {
		alive = true;
		this.setPos (pos.x,pos.y);
		hpControl.init ((value) => destroySelf (), (value) => {}, ()=>transform.position.toVec2());
	}

	public bool destroyed { get{ return !alive;}}
	public bool alive { get; private set;}

	public void destroySelf (GameManager manager){
		if (alive) {
			cleanUp (manager);

			alive = false;
			Destroy (gameObject);
		}
	}

	public void destroySelf(){
		destroySelf (GameManager.Get);
	}

	protected virtual void cleanUp(GameManager manager) {} 

	protected virtual void hitBack(Vector3 force) {
		_rigidbody.AddForce (force);
	}

	// Use this for initialization
	void Start () {
		_rigidbody = GetComponent<Rigidbody> ();
		initMovement ();
		ChangeCurrentTarget ();
		nextAttackTime = attackInterval;
	}

	protected virtual void initMovement() {}

	protected void ChangeCurrentTarget () {
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
}
