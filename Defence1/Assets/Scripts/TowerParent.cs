﻿using UnityEngine;
using System.Collections;

public abstract class TowerParent : MonoBehaviour {

	public bool destroyed { get; private set;}
	public EnergyNode energyNode { get; private set;} //The embeded enrgyNode for every building.

	public virtual float powerLeft { get; set;}

	public abstract float maxPower ();

	public void destroySelf (GameManager manager){
		if (!destroyed) {
			cleanUp (manager);

			energyNode.clearAllConnections ();

			destroyed = true;
			Destroy (gameObject);
		}
	}

	protected virtual void cleanUp(GameManager manager) {} 

	/// <summary>
	/// Remember to call this method first in every child's init.
	/// </summary>
	protected void initParent(Vector2 pos){
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
