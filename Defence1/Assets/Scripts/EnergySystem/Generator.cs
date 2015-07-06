using UnityEngine;
using System.Collections;

public class Generator : TowerParent {

	public readonly static float outputInterval = 2;
	public readonly static float outputAmount = 100;

	float outputTimer = 0;
	
	public void init(Vector2 pos){
		initParent (pos);

	}

	void FixedUpdate() {
		if(outputTimer >= outputInterval){
			outputPower ();
		}
		outputTimer += Time.fixedDeltaTime;
	}

	void outputPower() {
		var manager = GameManager.Get;
		var point = Instantiate (manager.energyPointPrefab);
		point.init (outputAmount,energyNode);
		point.transform.position = transform.position;
		point.transform.parent = manager.energyPointParent.transform;
		outputTimer -= outputInterval;
	}


	public override float maxPower (){
		throw new UnityException ("This method shouldn't be called!");
	}

	protected override float energyArrive (float amount) {
		return amount; // PowerStation doesn't consume power.
	}
}
