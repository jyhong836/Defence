using UnityEngine;
using System.Collections;
using System;

public enum DetectiongTargetType {
	Enemy,
	Tower
}

public class DetectingControl {

	private float detectingRadius;

	private DetectiongTargetType targetType;
	public int targetMask{ 
		get{ 
			switch (targetType) {
			case DetectiongTargetType.Enemy:
				return Masks.Enemy;
			case DetectiongTargetType.Tower:
				return Masks.Tower;
			default:
				throw new UnityException ("Unknown DetectiongTargetType: "+targetType);
			}
		} }
	
	Func<Vector2> _armPosition;
	Vector2 armPosition{get{ return _armPosition();}}

	public DetectingControl(
		DetectiongTargetType targetType,
		Func<Vector2> armPosition,
		float detectingRadius = 10
	) 
	{
		this.targetType = targetType;
		this._armPosition = armPosition;
		this.detectingRadius = detectingRadius;
	}

	public bool DetectSingleRandom(Action<HitpointControl> detectedCallback) {
		var colliders = Physics.OverlapSphere (
			Vector3Extension.fromVec2(armPosition),
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

	public bool DetectMultiple(Action<HitpointControl> detectedCallback, 
		float radius = detectingRadius) {
		var colliders = Physics.OverlapSphere (
			Vector3Extension.fromVec2(armPosition), 
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

	public bool DetectSingleNearest(Action<HitpointControl> detectedCallback) {
		var colliders = Physics.OverlapSphere (
			Vector3Extension.fromVec2(armPosition),
			detectingRadius,
			targetMask); // FIXME the preview model will detect too. use tag ?
		HitpointControl currentTarget = null;
		if (colliders.Length > 0) {
			// pick nearest one
			int minIdx = 1;
			float minDist = (colliders [minIdx].gameObject.getPos () - armPosition).magnitude;
			for (int idx = 2; idx < colliders.Length; idx++) {
				float dist = (colliders [idx].gameObject.getPos () - armPosition).magnitude;
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
		case AttackTargetType.Enemy: 
			var enemy = obj.GetComponent<Enemy> ();
			if (enemy.alive)
				return enemy.hpControl;
			break;
		case AttackTargetType.Tower:
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
