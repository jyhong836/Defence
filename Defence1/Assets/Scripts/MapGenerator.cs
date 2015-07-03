using UnityEngine;
using System.Collections;
using System;

public class MapGenerator {

	public readonly float oreNum;
	public readonly float minOreValue = 50.0f;
	public readonly float maxOreValue = 1000.0f;

	public MapGenerator (float oreNum){
		this.oreNum = oreNum;
	}

	int randomOreNum(){
		var x = UnityEngine.Random.value;
		return (int)(minOreValue + x * x * (maxOreValue - minOreValue));
	}

	public void generateOres(Action<Vector2,int> genFunc,Func<Vector2> randomPosInScene){

		for (int i = 0; i < oreNum; i++) {
			var pos = randomPosInScene ();
			var ore = randomOreNum ();
			genFunc (pos, ore);
		}
	}

}
