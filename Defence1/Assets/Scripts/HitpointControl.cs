using UnityEngine;
using System.Collections;
using System;

[Serializable] public class HitpointControl {
	[SerializeField] public float maxHitpoint;
	[SerializeField] float _hp;
	public Vector2 objectPosition{ get; private set; }
	public float hp { 
		get{return _hp;} 
		set {
			if (value < _hp)
				hurtedCallback (_hp - value);
			_hp = value;
			if(value <= 0){
				outOfHp (value);
			}
		}
	}

	Action<float> outOfHp; // outOfHp(remainHp);
	Action<float> hurtedCallback; // hurtCallbakc(hurtHp);

	public void init(//float maxHp, float initHp, 
		Action<float> outOfHp,
		Action<float> hurtedCallback,
		Vector2 objectPosition
	){
		this.outOfHp = outOfHp;
		this.hurtedCallback = hurtedCallback;
		this.objectPosition = objectPosition;
//		this.maxHitpoint = maxHp;
//		hp = initHp;
	}

}
