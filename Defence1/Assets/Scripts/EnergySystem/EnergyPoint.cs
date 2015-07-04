using UnityEngine;
using System.Collections;

public class EnergyPoint : MonoBehaviour {

	public float energy;
	public EnergyNode destination;

	public static readonly float movingSpeed = 3f;

	public void init(float energy, EnergyNode destination){
		this.energy = energy;
		this.destination = destination;
	}

	// Use this for initialization
	void Start () {
	
	}

	void FixedUpdate () {
		var delta = destination.transform.position - transform.position;
		var disLeft = delta.magnitude;
		var disCanMove = movingSpeed * Time.fixedDeltaTime;
		if(disLeft <= disCanMove){
			destination.energyPointArrived (this);
		}else{
			transform.position += delta * (disCanMove / disLeft);
		}
	}
		
	public void destroySelf(){
		Destroy (gameObject);
	}
}
