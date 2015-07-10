using UnityEngine;
using System.Collections;

public enum TowerMode {
	Attack, // auto resume from powerless
	Idle,
	AttackOnce // whill attack until out of power, not resume.
}

public class WeaponTower : Tower {
	
	protected bool isAttacking = false;

	public AttackingControl attackControl;
	public DetectingControl<Enemy> detectControl;
	public float detectingRadius = 10;

	[SerializeField] protected float idlePowerUsage = 0.001f; // per sec
	[SerializeField] protected float attackPowerUsage = 0.01f; // per sec

	// mode
	[SerializeField] TowerMode mode = TowerMode.Attack;
	public TowerMode currentMode {
		get {return mode;}
		set {
			mode = value;
			switch (mode) 
			{
			case TowerMode.Attack: isAttacking = true; break;
			case TowerMode.AttackOnce: isAttacking = true; break;
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
					isAttacking = true;
				}
			} else 
				isOutOfPower = true;
		}
	}
	
	protected override void init(Vector2 pos){
		initAttackingControl ();
		detectControl = new DetectingControl<Enemy>(TargetType.Enemy,(o)=>o.hpControl,
			()=>transform.position.toVec2(),
			detectingRadius
		);
	}

	protected virtual void initAttackingControl() {
		attackControl.init (TargetType.Enemy, ()=>transform.position.toVec2(), null, null,
			()=>detectControl.isOutOfRange(attackControl.currentTarget),
			()=>detectControl.isOutOfRange(attackControl.currentTarget, attackControl.attackingRadius),
			(detectedCallback)=>detectControl.DetectSingleNearest(detectedCallback)
		);
	}

	void FixedUpdate () {
		if (isAttacking) {
			if (power < attackPowerUsage) {
				isAttacking = false;
			} else {
				powerLeft -= attackPowerUsage*Time.fixedDeltaTime;
//				Attack();
				attackControl.Attack();
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
//		ChangeCurrentTarget ();
//		
	}
	
	// Update is called once per frame
	void Update () {
		if (isAttacking) {
			attackControl.DrawAttackLine ();
		}
	}

	#region implemented abstract members of TowerParent
	public override float maxPower ()
	{
		return 60;
	}
	#endregion
}
