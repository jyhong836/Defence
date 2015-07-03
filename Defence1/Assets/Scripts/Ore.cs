using UnityEngine;
using System.Collections;

public class Ore : MonoBehaviour {
	
	[SerializeField] int ore;
	
	const float radiusFactor = 0.5f;
	
	public int oreLeft{
		get{ return ore; }
		set{ 
			ore = value;
			if(ore<=0){
				destroySelf ();
			}
			ajustRadiusAccordingToOre ();
		}
	}
	
	public void init(Vector2 pos, int ore){
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
		var radius = radiusFactor * Mathf.Pow (ore, 0.33333f);
		transform.localScale = Vector3Extension.vectorOf(radius);
	}

	
	// Update is called once per frame
	void Update () {
	}
}
