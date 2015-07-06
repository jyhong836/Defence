using UnityEngine;
using System.Collections;
using System;

public interface AimingControl {

	bool ready { get; }
	void updateOrientation(float dt);

}

public class HorizontalRotationAimingControl: AimingControl{
	float _direction;
	public float direction{ 
		get{ return _direction; } 
		set{
			_direction = RotationMath.stayIn2Pi (value);
			rotateToDirection (_direction);
		}
	}
	public Func<float> rotateSpeed;
	public Func<float> fireAngle;

	Action<float> rotateToDirection;
	Func<bool> hasTarget;
	Func<float> targetDirection;

	public HorizontalRotationAimingControl(Func<float> rotateSpeed, Func<float> fireAngle, 
		Action<float> rotateToDirection, Func<bool> hasTarget, Func<float> targetDirection,
		float initDirection = 0)
	{
		this.targetDirection = targetDirection;
		this.hasTarget = hasTarget;
		this.rotateToDirection = rotateToDirection;
		this.fireAngle = fireAngle;
		this.rotateSpeed = rotateSpeed;

		direction = initDirection;
	}

	float targetRelativeToThis(){
		return RotationMath.approachingAngle (direction, targetDirection ());
	}
		

	#region AimingControl implementation
	public void updateOrientation (float dt) {
		if(hasTarget()){
			var needRotate = targetRelativeToThis ();
			var maxCanRotate = dt * rotateSpeed();
			float delta;
			if(Mathf.Abs (needRotate) < maxCanRotate){
				delta = needRotate;
			}else{
				delta = Mathf.Sign (needRotate) * maxCanRotate;
			}
			direction += delta;
		}
	}
	public bool ready {
		get {
			if (!hasTarget ())
				return true;
			else
				return Mathf.Abs (targetRelativeToThis()) < fireAngle();
		}
	}
	#endregion

}
	
