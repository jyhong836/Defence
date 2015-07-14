using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public enum TargetType {
	Enemy,
	Tower
}

public class DetectingControl <T> where T: MonoBehaviour, IAliveable {

	private float detectingRadius;

	private TargetType targetType;
//	private Func<T, HitpointControl> getTarget;
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
//		Func<T, HitpointControl> getTarget,
		Func<Vector2> armPosition,
		float detectingRadius = 10
	) 
	{
//		this.getTarget = getTarget;
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
	public bool isOutOfRange(T target, float radius) {
		return target == null || (armPosition () - target.transform.position.toVec2()).magnitude > radius;
	}
	public bool isOutOfRange(T target) {
		return isOutOfRange (target, detectingRadius);
	}

	/// <summary>
	/// Detects the single target from random choice.
	/// </summary>
	/// <returns><c>true</c>, if target was detected, <c>false</c> otherwise.</returns>
	/// <param name="detectedCallback">Detected callback.</param>
	public bool DetectSingleRandom(Action<T> detectedCallback) {
		var colliders = Physics.OverlapSphere (
			Vector3Extension.fromVec2(armPosition()),
			detectingRadius,
			targetMask); // FIXME the preview model will detect too. use tag ?
		T currentTarget = null;
		if (colliders.Length > 0) {
			var targetColliders = pickTargetColliders (colliders);
			if (targetColliders.Count > 0) {
				//random pick one
				int index = UnityEngine.Random.Range (0, targetColliders.Count);

				currentTarget = colliders [index].gameObject.GetComponent<T> ();
				if (currentTarget != null) {
					detectedCallback (currentTarget);
					return true;
				}
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
	public bool DetectMultiple(Action<T> detectedCallback, 
		float radius) {
		var colliders = Physics.OverlapSphere (
			Vector3Extension.fromVec2(armPosition()), 
			radius, 
			targetMask);
		if (colliders.Length > 0) {
			var targetColliders = pickTargetColliders (colliders);
			if (targetColliders.Count > 0) {
				foreach (var c in targetColliders) {
					var target = c.gameObject.GetComponent<T> ();
					detectedCallback (target);
				}
				return true;
			}
		}
		return false;
	}
	public bool DetectMultiple(Action<T> detectedCallback) {
		return DetectMultiple (detectedCallback, detectingRadius);
	}

	public bool DetectSingleNearest(Action<T> detectedCallback) {
		var colliders = Physics.OverlapSphere (
			Vector3Extension.fromVec2(armPosition()),
			detectingRadius,
			targetMask); // FIXME the preview model will detect too. use tag ?
		T currentTarget = null;

		if (colliders.Length > 0) {
			var targetColliders = pickTargetColliders (colliders);
			if (targetColliders.Count > 0) {
				// pick nearest one
				int minIdx = 0;
				float minDist = (colliders [minIdx].gameObject.getPos () - armPosition ()).magnitude;
				for (int idx = 1; idx < targetColliders.Count; idx++) {
					float dist = (colliders [idx].gameObject.getPos () - armPosition ()).magnitude;
					if (dist < minDist) {
						minDist = dist;
						minIdx = idx;
					}
				}

				currentTarget = colliders [minIdx].gameObject.GetComponent<T> ();
				if (currentTarget != null) {
					detectedCallback (currentTarget);
					return true;
				}
			}
		}
		return false;
	}

	List<Collider> pickTargetColliders(Collider[] colliders) {
		List<Collider> targetColliders = new List<Collider>();
		foreach (var c in colliders) {
			if (c.gameObject.tag != "Preview") {
				targetColliders.Add (c);
			}
		}
		return targetColliders;
	}
}
