using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public abstract class Tower : MonoBehaviour {

	public bool destroyed { get{ return !alive;}}
	public bool alive { get; private set;}
	public EnergyNode energyNode { get; private set;} //The embeded enrgyNode for every building.

	public virtual float powerLeft { get; set;}
	public virtual bool isRedirector{ get{ return false; }}
	public abstract float maxPower ();

	public void destroySelf (GameManager manager){
		if (alive) {
			cleanUp (manager);

			energyNode.clearAllConnections ();

			alive = false;
			gameObject.SetActive (false);
			Destroy (gameObject);
		}
	}

	public void destroySelf(){
		destroySelf (GameManager.Get);
	}

	protected virtual void cleanUp(GameManager manager) {} 

	/// <summary>
	/// Remember to call this method first in every child's init.
	/// </summary>
	protected void initParent(Vector2 pos){
		alive = true;
		this.setPos (pos.x,pos.y);
		energyNode = gameObject.AddComponent <EnergyNode>();
		energyNode.init (energyArrive,this);
	}

	/// <summary>
	/// This method can be overrided by some tower, for example, 
	/// The Power Station doesn't consume any Power.
	/// </summary>
	/// <returns> energy left. </returns>
	/// <param name="amount">energy arrived.</param>
	protected virtual float energyArrive(float amount){
		var powerNeed = maxPower () - powerLeft;
		var powerGet = Mathf.Min (powerNeed, amount);
		powerLeft += powerGet;
		return amount - powerGet;
	}

}
