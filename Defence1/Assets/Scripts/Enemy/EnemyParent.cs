using UnityEngine;
using System.Collections;

public abstract class EnemyParent : MonoBehaviour, IAliveable {

	#region IAliveable implementation

	public HitpointControl _hpControl;
	public HitpointControl hpControl { 
		get{ return _hpControl; }
		protected set{ _hpControl = value; }
	}

	public bool destroyed { get{ return !alive;}}
	public bool alive { get; private set;}

	#endregion

	public void create(Vector2 pos) {
		initParent (pos);
		init (pos);
	}

	private void initParent(Vector2 pos) {
		alive = true;
		this.setPos (pos.x,pos.y);
		initHpControl ();
	}

	public virtual void init(Vector2 pos) { }

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

	protected virtual void initHpControl(){
//		hpControl = new HitpointControl ();
		hpControl.init (
			outOfHp: (value) => destroySelf (), 
			hurtedCallback: (value) => {},
			objectPosition: ()=>transform.position.toVec2()
		);
	}
}
