using UnityEngine;
using System.Collections;

public class PowerRedirector : TowerParent {

	public override float maxPower () {
		throw new UnityException ("This method shouldn't be called!");
	}

	protected override float energyArrive (float amount) {
		return amount; // Redirector doesn't consume power.
	}

	public void init(Vector2 pos){
		initParent (pos);
	}

	public override bool isRedirector {
		get {
			return true;
		}
	}
		
}
