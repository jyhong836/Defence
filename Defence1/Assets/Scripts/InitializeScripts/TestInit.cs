using UnityEngine;
using System.Collections;

public class TestInit : InitializeScript {

	public override void initialize (GameManager manager){
		manager.createGenerator (Vector2.zero);
		manager.createPowerRedirector (new Vector2(6,0));
		manager.createPowerRedirector (new Vector2(-6,0));

	}

	public override int initOre(){
		return 10000;
	}

}
