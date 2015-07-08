using UnityEngine;
using System.Collections;
using System;

public class HitpointControl {
	public float maxHitpoint;
	float _hp;
	public float hp { 
		get{return _hp;} 
		private set {
			_hp = value;
			if(value < 0){
				outOfHp (value);
			}
		}
	}

	Action<float> outOfHp;

	public HitpointControl(float maxHp, float initHp, Action<float> outOfHp){
		this.outOfHp = outOfHp;
		this.maxHitpoint = maxHp;
		hp = initHp;
	}

}
