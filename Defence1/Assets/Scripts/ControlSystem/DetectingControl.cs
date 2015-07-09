using UnityEngine;
using System.Collections;
using System;

public enum TargetType {
	Enemy,
	Tower
}

public class DetectingControl {

	private float detectingRadius;

	private TargetType targetType;
	public int targetMask{ 
		get{ 
			switch (targetType) {
			case TargetType.Enemy:
				return Masks.Enemy;
			case TargetType.Tower:
				return Masks.Tower;
			default:
				throw new UnityException ("Unknown DetectiongTargetType: "+targetType);
			}
		} }
	
	Func<Vector2> armPosition;

	public DetectingControl(
		TargetType targetType,
		Func<Vector2> armPosition,
		float detectingRadius = 10
	) 
	{
		this.targetType = targetType;
		this.armPosition = armPosition;
		this.detectingRadius = detectingRadius;
	}

	/// <summary>
	/// Is the target null or out of range.
	/// </summary>
	/// <returns><c>true</c>, if target out of range, <c>false</c> otherwise.</returns>
	/// <param name="target">HitpointControl target.</param>
	/// <param name="radius">Radius.</param>
	public bool isOutOfRange(HitpointControl target, float radius) {
		return target == null || (armPosition () - target.objectPosition).magnitude > radius;
	}
	public bool isOutOfRange(HitpointControl target) {
		return isOutOfRange (target, detectingRadius);
	}

	/// <summary>
	/// Detects the single target from random choice.
	/// </summary>
	/// <returns><c>true</c>, if target was detected, <c>false</c> otherwise.</returns>
	/// <param name="detectedCallback">Detected callback.</param>
	public bool DetectSingleRandom(Action<HitpointControl> detectedCallback) {
		var colliders = Physics.OverlapSphere (
			Vector3Extension.fromVec2(armPosition()),
			detectingRadius,
			targetMask); // FIXME the preview model will detect too. use tag ?
		HitpointControl currentTarget = null;
		if (colliders.Length > 0) {
			//random pick one
			var index = UnityEngine.Random.Range (0, colliders.Length);

			currentTarget = getTarget (colliders [index].gameObject);
			if (currentTarget != null) {
				detectedCallback (currentTarget);
				return true;
			}
		}
		return currentTarget != null;
	}

	/// <summary>
	/// Detects the multiple targets.
	/// </summary>
	/// <returns><c>true</c>, if multiple was detected, <c>false</c> otherwise.</returns>
	/// <param name="detectedCallback">Detected callback.</param>
	/// <param name="radius">Radius.</param>
	public bool DetectMultiple(Action<HitpointControl> detectedCallback, 
		float radius) {
		var colliders = Physics.OverlapSphere (
			Vector3Extension.fromVec2(armPosition()), 
			radius, 
			targetMask);
		if (colliders.Length > 0) {
			foreach (var c in colliders) {
				var target = getTarget (c.gameObject);
				detectedCallback(target);
			}
			return true;
		}
		return false;
	}
	public bool DetectMultiple(Action<HitpointControl> detectedCallback) {
		return DetectMultiple (detectedCallback, detectingRadius);
	}

	public bool DetectSingleNearest(Action<HitpointControl> detectedCallback) {
		var colliders = Physics.OverlapSphere (
			Vector3Extension.fromVec2(armPosition()),
			detectingRadius,
			targetMask); // FIXME the preview model will detect too. use tag ?
		HitpointControl currentTarget = null;
		if (colliders.Length > 0) {
			// pick nearest one
			int minIdx = 1;
			float minDist = (colliders [minIdx].gameObject.getPos () - armPosition()).magnitude;
			for (int idx = 2; idx < colliders.Length; idx++) {
				float dist = (colliders [idx].gameObject.getPos () - armPosition()).magnitude;
				if (dist < minDist) {
					minDist = dist;
					minIdx = idx;
				}
			}

			currentTarget = getTarget (colliders [minIdx].gameObject);
			if (currentTarget != null) {
				detectedCallback (currentTarget);
				return true;
			}
		}
		return false;
	}

	private HitpointControl getTarget(GameObject obj) {
		switch (targetType) {
		case TargetType.Enemy: 
			var enemy = obj.GetComponent<Enemy> ();
			if (enemy.alive)
				return enemy.hpControl;
			break;
		case TargetType.Tower:
			var tower = obj.GetComponent<Tower> ();
			if (tower.alive)
				return tower.hpControl;
			break;
		default:
			throw new UnityException ("Unknown mask: "+targetMask);
		}
		return null;
	}
	
}
