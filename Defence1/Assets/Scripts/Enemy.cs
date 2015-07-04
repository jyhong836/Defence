using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	[SerializeField] float life = 100;

	public float lifeLeft {
		get {
			return life;
		}
		set {
			life = value;
			if (life<=0) {
				Destroy(gameObject);
			}
		}
	}

	public void init(Vector2 pos) {
		transform.position = Vector3Extension.fromVec2 (pos);
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		MoveStep ();
	}

	// Add enemy motion robot here
	void MoveStep() {

	}
}
