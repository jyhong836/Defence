using UnityEngine;
using System.Collections;

public class EnergyPoint : MonoBehaviour {

	public static readonly float movingSpeed = 3f;
	public static readonly float maxEnergy = 100;
	public static readonly Color highColor = Color.yellow;
	public static readonly Color lowColor = Color.red;

	float _energy;
	public float energy {
		get{ return _energy;}
		set{
			_energy = value;
			changeColor ();
		}
	}
	public EnergyNode destination;

	Renderer render;


	public void init(float energy, EnergyNode destination){
		render = GetComponent <Renderer> ();
		this.energy = energy;
		this.destination = destination;
	}

	void FixedUpdate () {
		if (destination.tower.destroyed) {
			var cons = destination.tower as ConstructingTower;
			if(cons != null){
				destination = cons.energyNode;
			}else
				destroySelf ();
		}
		else {
			var delta = destination.transform.position - transform.position;
			var disLeft = delta.magnitude;
			var disCanMove = movingSpeed * Time.fixedDeltaTime;
			if (disLeft <= disCanMove) {
				destination.energyPointArrived (this);
			} else {
				transform.position += delta * (disCanMove / disLeft);
			}
		}
	}


	void changeColor() {
		var ratio = energy / maxEnergy;
		var c = Color.Lerp (lowColor,highColor,ratio);
		render.material.color = c;
	}

		
	public void destroySelf(){
		Destroy (gameObject);
	}
}
