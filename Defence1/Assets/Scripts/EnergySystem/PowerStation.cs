﻿using UnityEngine;
using System.Collections;

public class PowerStation : TowerParent {

	public readonly static float outputInterval = 2;
	public readonly static float outputAmount = 100;


	public EnergyPoint energyPointPrefab;

	float outputTimer = 0;
	
	public void init(Vector2 pos){
		initParent ();

		transform.position = Vector3Extension.fromVec2 (pos);
	}

	void FixedUpdate() {
		if(outputTimer >= outputInterval){
			outputPower ();
		}
		outputTimer += Time.fixedDeltaTime;
	}

	void outputPower() {
		var point = Instantiate (energyPointPrefab);
		point.init (outputAmount,energyNode);
		outputTimer -= outputInterval;
	}


	public override float maxPower (){
		throw new UnityException ("This method shouldn't be called!");
	}

	protected override float energyArrive (float amount) {
		return amount; // PowerStation doesn't consume power.
	}
}
