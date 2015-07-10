using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Miner : Tower {
	
	public static readonly float oreCollectSpeed = 2; 
	public static readonly float oreUpdateInterval = 0.5f;
	public static readonly float workingRadius = 6;
	public static readonly float energyConsumingSpeed = 10;

	public LaserEffect miningLaser;
	public Transform rotationPart;
	[SerializeField] bool finishedWorking = false;
	[SerializeField] bool hasEnoughEnergy = false;

	bool _isFiring;
	public bool isFiring {
		get {return isFiring;} 
		private set {
			if(_isFiring != value){
				_isFiring = value;
				miningLaser.showEffect = value;
				if(value){
					miningLaser.setEndpoints (firePoint, currentTarget.transform.position);
				}
			}
		}
	}
	public Ore currentTarget { get; private set;}
	public Vector3 firePoint{
		get{return transform.position;}
	}

	float oreTemp;
	Action<int> collectCallback;
	AimingControl aimControl;


	protected override void init(Vector2 pos){
		this.collectCallback = change => GameManager.Get.resourceControl.tryChangeOre (change);
		aimControl = 
			new HorizontalRotationAimingControl (
			rotateSpeed: () => 1f,
			fireAngle: () => 0.01f,
			rotateToDirection: RotationMath.RotatePart (rotationPart, 0f),
			hasTarget: () => !finishedWorking,
			targetDirection: () => RotationMath.directionOf (currentTarget.getPos () - this.getPos ())
		);
	}
		
	void Start () {
		changeCurrentTarget ();
	}
	
	// Game Mechanisms should be included into FixedUpdate.
	void FixedUpdate () {
		if(! finishedWorking){
			if(currentTarget.oreLeft > 0 && targetInRange() ) 
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

	bool targetInRange(){
		var laserLength = (currentTarget.getPos () - this.getPos ()).magnitude - currentTarget.radius;
		return laserLength < workingRadius;
	}

	void collectFromTarget(){
		aimControl.updateOrientation (Time.fixedDeltaTime);
		if (aimControl.ready) {
			var de = energyConsumingSpeed * Time.fixedDeltaTime;
			hasEnoughEnergy = powerLeft > de;
			if (hasEnoughEnergy) {
				powerLeft -= de;
				isFiring = true;
			
				var amount = Mathf.Min (currentTarget.oreLeft, oreCollectSpeed * Time.fixedDeltaTime);
				currentTarget.oreLeft -= (amount + 1e-5f);
				oreTemp += amount;

				if (oreTemp >= oreCollectSpeed * oreUpdateInterval) {
					var send = (int)oreTemp;
					collectCallback (send);
					oreTemp -= send;
				}
			}else{
				isFiring = false;
			}
		}else{
			isFiring = false;
		}
	}

	void changeCurrentTarget(){
		isFiring = false;

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
