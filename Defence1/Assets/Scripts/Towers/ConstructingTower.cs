﻿using UnityEngine;
using System.Collections;
using System;

public class ConstructingTower : Tower {

	public float constructionPower;
	public Func<Tower> createTower;
	public Tower successor { get; private set;} // The new tower created should be set to this.

	Renderer render;

	public void init(Vector2 pos ,float powerNeed, Func<Tower> createTower){
		initParent (pos);

		render = GetComponent <Renderer>();
		this.constructionPower = powerNeed;
		this.createTower = createTower;
		powerLeft = 0; //This is needed.
		changeColorAccordingEnergy ();
	}


	public override float maxPower () {
		return constructionPower;
	}

	void changeColorAccordingEnergy(){
		var ratio = powerLeft / maxPower ();
		render.material.color = Color.Lerp (EnergyPoint.lowColor, EnergyPoint.highColor, ratio);
	}

	protected override float energyArrive (float amount){
		var energyReturned = base.energyArrive (amount);
		changeColorAccordingEnergy ();

		if(energyReturned > 1e-4f){
			energyFulled ();
		}

		return energyReturned;
	}

	bool construtionCompleted = false;
	public void energyFulled(){
		if(!construtionCompleted){
			destroySelf ();

			successor = createTower ();

			construtionCompleted = true;
		}
	}

	public static float powerNeedToConstruct(Towers t){
		switch(t){
		case Towers.Redirector:
			return 10;
		case Towers.Generator:
			return 400;
		case Towers.Miner:
			return 250;
		case Towers.LaserTower:
			return 300;
		case Towers.FireTower:
			return 300;
		case Towers.CannonTower:
			return 500;
		default:
			throw new  NotImplementedException ();
		}
	}
}
