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

	public void init(Vector3 position, float speed, Vector3 target, float injury,
	                 float attackingRadius) {
		originPos = new Vector2 (position.x, position.z);;

		this.transform.position = position;
		this.direction = target - position;
		distance = direction.magnitude;
		direction.Normalize ();
		direction *= speed;
		this.transform.Rotate(new Vector3(0, RotationMath.directionOf(new Vector2(direction.x, direction.z)),0));

		this.injury = injury;
		this.attackingRadius = attackingRadius;
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
				enemy.lifeLeft -= injury;
			}
		}

		Destroy (gameObject);
	}
}
