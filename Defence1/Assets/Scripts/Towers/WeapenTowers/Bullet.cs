using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

	public GameObject explosionEffectPrefab;
	private Vector3 direction;
	private float distance;
//	private Vector2 target;
	private Vector2 originPos;
	private float injury;
	private float attackingRadius;
	float hitForce;

	public void init(Vector3 position, float speed, Vector3 targetPos, float injury,
		float attackingRadius, float hitForce) {
		originPos = new Vector2 (position.x, position.z);;

		this.transform.position = position;
		this.direction = targetPos - position;
		distance = direction.magnitude;
		direction.Normalize ();
		this.transform.Rotate(new Vector3(90, RotationMath.directionOf(new Vector2(direction.x, direction.z))*RotationMath.rand2Deg,0));
		direction *= speed;

		this.injury = injury;
		this.attackingRadius = attackingRadius;
		this.hitForce = hitForce;
	}

	void FixedUpdate() {
		Vector2 dist = new Vector2 (this.transform.position.x, transform.position.z);
		dist -= originPos;
		if (dist.magnitude < distance)
			this.transform.position += direction * Time.fixedDeltaTime;
		else
			explode ();
	}

	void explode() {
		Instantiate (explosionEffectPrefab, transform.position, Quaternion.identity);

		var colliders = Physics.OverlapSphere (transform.position, attackingRadius, Masks.Enemy);
		if (colliders.Length > 0) {
			foreach (var collider in colliders) {
				var enemy = collider.gameObject.GetComponent<Enemy> ();
//				enemy.lifeLeft -= injury;
				enemy.hpControl.hp -= injury;
				enemy.hitBack (
					(enemy.transform.position - transform.position).normalized*hitForce
				);
//				Debug.Log ("hurt enemy"+enemy.hpControl.hp);
			}
		}

		Destroy (gameObject);
	}
}
