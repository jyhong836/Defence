using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

	public GameObject explosionEffectPrefab;
	private Vector3 direction;
	public float maxDistance = 200;
//	private Vector2 target;
	private Vector2 originPos;

	public DetectingControl<Enemy> detectControl;
	public float detectingRadius = 0;

	#region Attackable

	public AttackingControl<Enemy> attackControl;
	private float injury;
	private float attackingRadius;
	float hitForce;

	#endregion

	public void init(Vector3 position, float speed, Vector3 targetPos, float injury,
		float attackingRadius, float hitForce, float maxDistance) {
		originPos = new Vector2 (position.x, position.z);;

		this.transform.position = position;
		this.direction = targetPos - position;
		direction.Normalize ();
		this.transform.Rotate(new Vector3(90, RotationMath.directionOf(new Vector2(direction.x, direction.z))*RotationMath.rand2Deg,0));
		direction *= speed;

		this.injury = injury;
		this.attackingRadius = attackingRadius;
		this.hitForce = hitForce;
		this.maxDistance = maxDistance;

		detectControl = new DetectingControl<Enemy>(TargetType.Enemy,
			()=>transform.position.toVec2(),
			detectingRadius
		);

		attackControl = new AttackingControl<Enemy> ();
		attackControl.attackingRadius = attackingRadius;
		attackControl.injury = injury;
		attackControl.hitForce = hitForce;
		attackControl.attackInterval = 0;
		attackControl.init (
			armPosition: () => transform.position.toVec2 (), 
			fireEffect: (b) => Instantiate (explosionEffectPrefab, transform.position, Quaternion.identity),
			attackAction: (v1, v2) => { explode(v2); },
			isTargetOutOfDetecting: () => detectControl.isOutOfRange (attackControl.currentTarget),
			isTargetOutOfAttacking: () => detectControl.isOutOfRange (attackControl.currentTarget, attackControl.attackingRadius),
			detectTarget: (detectedCallback) => detectControl.DetectSingleNearest (detectedCallback),
			isAimedAtTarget: () => true,
			updateOrientation: (t) => {}
		);
	}

	void FixedUpdate() {
		Vector2 dist = new Vector2 (this.transform.position.x, transform.position.z);
		dist -= originPos;
		if (dist.magnitude < maxDistance) {
			moveStep ();
			attackControl.Attack ();
		} else
			explode (injury);
	}

	void moveStep() {
		this.transform.position += direction * Time.fixedDeltaTime;
	}

	void explode(float injury) {
		detectControl.DetectMultiple ((Enemy obj) => {
			obj.hpControl.hp -= injury;
			obj.hitBack (
				(obj.transform.position - transform.position).normalized*hitForce
			);
		}, attackingRadius);

		Destroy (gameObject);
	}
}
