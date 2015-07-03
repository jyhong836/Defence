using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Use 'Control' instead of 'Manager' to indecate that this is not a Monobehavior.
/// All the resouce services come here.
/// </summary>
public class ResourceControl{
	public float ore{ get; private set;}

	public ResourceControl(float initOre){
		ore = initOre;
	}

	public bool tryChangeOre(float delta){
		var newValue = ore + delta;
		if(newValue>=0){
			ore = newValue;
			return true;
		}else{
			return false;
		}
	}

}
