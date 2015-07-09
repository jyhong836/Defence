using UnityEngine;
using System.Collections;
using System;


public enum TowerType{
	Miner,
	LaserTower,
	CannonTower,
	FireTower,
	Generator,
	Redirector
}

public static class TowerTypeService  {
	public static TowerType towerTypeOfName(string name){
		if (name == "Redirector")
			return TowerType.Redirector;
		else if (name == "Generator")
			return TowerType.Generator;
		else if (name == "Miner")
			return TowerType.Miner;
		else if (name == "FireTower")
			return TowerType.FireTower;
		else if (name == "CannonTower")
			return TowerType.CannonTower;
		else if (name == "LaserTower")
			return TowerType.LaserTower;


		throw new ArgumentException ("No such name exists!");
	}
}