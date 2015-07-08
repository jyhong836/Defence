using UnityEngine;
using System.Collections;

public class StandardInit : InitializeScript {

	public override void initialize (GameManager manager){
		manager.createGenerator (Vector2.zero);
		manager.createPowerRedirector (new Vector2(6,0));
		manager.createPowerRedirector (new Vector2(-6,0));
//		manager.createTower (new Vector2(0, -4));

	}

	public override int initOre(){
		return 120;
	}

}
