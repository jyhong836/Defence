using UnityEngine;
using System.Collections;
using System;

public class HitpointControl {
	public float maxHitpoint;
	public bool isAlive{get{ return _hp > 0; }}
	public Func<Vector2> _objectPosition;
	public Vector2 objectPosition{ get{ return _objectPosition (); } private set{ } }

	float _hp;
	public float hp { 
		get{return _hp;} 
		set {
			if (value < _hp)
				hurtedCallback (_hp - value);
			_hp = value;
//			Debug.Log ("hp:" + _hp+" "+value);
			if(value <= 1e-4){
				outOfHp (value);
			}
		}
	}

	Action<float> outOfHp; // outOfHp(minusHp);
	Action<float> hurtedCallback; // hurtCallbakc(hurtHp);

	private HitpointControl(float maxHp, float initHp, 
		Action<float> outOfHp,
		Action<float> hurtedCallback,
		Func<Vector2> objectPosition
	){
		this.outOfHp = outOfHp;
		this.hurtedCallback = hurtedCallback;
		this._objectPosition = objectPosition;
		this.maxHitpoint = maxHp;
		hp = initHp;
	}

	public static HitpointControl createFor(Tower t){
		return new HitpointControl (maxHp: 200, initHp: 200,
			outOfHp: h => t.destroySelf (),
			hurtedCallback: x => { },
			objectPosition: () => t.transform.position.toVec2 ()
		);
	}

	public static HitpointControl createFor(EnemyParent t){
		return new HitpointControl (maxHp: 200, initHp: 200,
			outOfHp: h => t.destroySelf (),
			hurtedCallback: x => { },
			objectPosition: () => t.transform.position.toVec2 ()
		);
	}
}
