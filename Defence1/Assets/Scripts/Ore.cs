using UnityEngine;
using System.Collections;

public class Ore : MonoBehaviour {
	
	[SerializeField] float ore;
	public float radius;

	public static readonly float radiusFactor = 0.5f;
	
	public float oreLeft{
		get{ return ore; }
		set{ 
			ore = value;
			if (ore <= 0) {
				destroySelf ();
			} else {
				ajustRadiusAccordingToOre ();
			}
		}
	}
	
	public void init(Vector2 pos, float ore){
		this.ore = ore;
		transform.position = new Vector3(pos.x,0,pos.y);
	}
	
	void destroySelf(){
		Destroy (gameObject);
	}
	

	// Use this for initialization
	void Start () {
		ajustRadiusAccordingToOre ();
	}

	void ajustRadiusAccordingToOre(){
		radius = radiusOfAmount (ore);
		transform.localScale = Vector3Extension.vectorOf(radius);
	}

	public static float radiusOfAmount(float amount){
		return radiusFactor * Mathf.Pow (amount, 0.33333f);
	}
}
