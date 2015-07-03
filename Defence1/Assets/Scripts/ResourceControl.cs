using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// Use 'Control' instead of 'Manager' to indecate that this is not a Monobehavior.
/// All the resouce services come here.
/// </summary>
public class ResourceControl{
	int _ore;

	public int ore{ 
		get{ return _ore;}
		private set{ 
			_ore = value;
			updateOre (value);
		}
	}

	Action<int> updateOre;

	public ResourceControl(int initOre,Action<int> updateOre){
		this.updateOre = updateOre;

		ore = initOre;
	}

	public bool tryChangeOre(int delta){
		var newValue = ore + delta;
		if(newValue>=0){
			ore = newValue;
			return true;
		}else{
			return false;
		}
	}

}
