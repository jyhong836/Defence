using UnityEngine;
using System.Collections;

public class TestInit : InitializeScript {

	public override void initialize (GameManager manager){
		manager.createTowerOfType (Vector2.zero, TowerType.Generator);
		manager.createTowerOfType (new Vector2(6,0), TowerType.Redirector);
		manager.createTowerOfType (new Vector2(-6,0), TowerType.Redirector);
		manager.createTowerOfType (new Vector2(0,-5),TowerType.LaserTower);
	}

	public override int initOre(){
		return 10000;
	}

}
