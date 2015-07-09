using UnityEngine;
using System.Collections;
using System;

[Serializable] public class HitpointControl {
	[SerializeField] public float maxHitpoint;
	[SerializeField] float _hp;
	public bool isAlive{get{ return _hp > 0; }}
	public Func<Vector2> _objectPosition;
	public Vector2 objectPosition{ get{ return _objectPosition (); } private set{ } }
	public float hp { 
		get{return _hp;} 
		set {
			if (value < _hp)
				hurtedCallback (_hp - value);
			_hp = value;
//			Debug.Log ("hp:" + _hp+" "+value);
			if(value <= 0){
				outOfHp (value);
			}
		}
	}

	Action<float> outOfHp; // outOfHp(minusHp);
	Action<float> hurtedCallback; // hurtCallbakc(hurtHp);

	public void init(//float maxHp, float initHp, 
		Action<float> outOfHp,
		Action<float> hurtedCallback,
		Func<Vector2> objectPosition
	){
		this.outOfHp = outOfHp;
		this.hurtedCallback = hurtedCallback;
		this._objectPosition = objectPosition;
//		this.maxHitpoint = maxHp;
//		hp = initHp;
	}

}
