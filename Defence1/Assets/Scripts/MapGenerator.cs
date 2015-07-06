using UnityEngine;
using System.Collections;
using System;

public class MapGenerator {

	public readonly float oreNum;
	public readonly float minOreValue = 50;
	public readonly float maxOreValue = 1000;

	public MapGenerator (float oreNum){
		this.oreNum = oreNum;
	}

	float randomOreNum(){
		var x = UnityEngine.Random.value;
		return (minOreValue + x * x * (maxOreValue - minOreValue));
	}

	public void generateOres(Action<Vector2,float> genFunc,Func<Vector2> randomPosInScene){

		for (int i = 0; i < oreNum; i++) {
			var pos = randomPosInScene ();
			var ore = randomOreNum ();
			genFunc (pos, ore);
		}
	}

}
