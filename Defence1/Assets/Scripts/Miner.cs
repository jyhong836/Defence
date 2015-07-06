using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Miner : TowerParent {
	
	public static readonly float oreCollectSpeed = 2; 
	public static readonly float oreUpdateInterval = 0.5f;
	public static readonly float workingRadius = 6;
	public static readonly float energyConsumingSpeed = 10;

	[SerializeField] Ore currentTarget;
	[SerializeField] bool finishedWorking = false;
	[SerializeField] bool hasEnoughEnergy = false;

	float oreTemp;
	Action<int> collectCallback;
	AimingControl aimControl;


	public void init(Vector2 pos, Action<int> oreCollected){
		initParent (pos);

		transform.position = Vector3Extension.fromVec2 (pos);
		this.collectCallback = oreCollected;

	}
		
	void Start () {
		changeCurrentTarget ();
	}
	
	// Game Mechanisms should be included into FixedUpdate.
	void FixedUpdate () {
		if(! finishedWorking){
			if(currentTarget.oreLeft >0 ) 
				collectFromTarget ();
			else 
				changeCurrentTarget ();
		}
	}

	void Update(){
		if (currentTarget != null && hasEnoughEnergy) {
			var start = transform.position;
			var end = currentTarget.transform.position;

			Debug.DrawLine (start,end,Color.yellow);
		}
	}

	void collectFromTarget(){
		var de = energyConsumingSpeed * Time.fixedDeltaTime;
		hasEnoughEnergy = powerLeft > de;

		if (hasEnoughEnergy) {
			powerLeft -= de;
			
			var amount = Mathf.Min (currentTarget.oreLeft, oreCollectSpeed * Time.fixedDeltaTime);
			currentTarget.oreLeft -= (amount + 1e-5f);
			oreTemp += amount;

			if (oreTemp >= oreCollectSpeed * oreUpdateInterval) {
				var send = (int)oreTemp;
				collectCallback (send);
				oreTemp -= send;
			}
		}
	}

	void changeCurrentTarget(){
		var colliders = Physics.OverlapSphere (transform.position,workingRadius,Masks.Ore);
		if(colliders.Length > 0){
			//random pick one
			var index = UnityEngine.Random.Range (0, colliders.Length);
			var ore = colliders [index].gameObject.GetComponent <Ore>();
			currentTarget = ore;
		}else{
			currentTarget = null;
			finishedWorking = true;
		}
	}

	#region implemented abstract members of TowerParent
	public override float maxPower () {
		return 100;
	}
	#endregion
}
