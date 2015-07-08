using UnityEngine;
using System.Collections;
using System;

public class MapGenerator {

	public readonly float oreNum;
	public readonly float mapSize;
	public readonly float minOreValue;
	public readonly float maxOreValue;

	Action<Vector2, float> putOre;
	Func<Vector2, float, bool> checkAvailable;

	public MapGenerator (float oreNum, float mapSize, Action<Vector2,float> putOre,Func<Vector2,float,bool> checkAvailable,
		float minOreValue = 100f, float maxOreValue = 2000){

		this.checkAvailable = checkAvailable;
		this.putOre = putOre;
		this.oreNum = oreNum;
		this.mapSize = mapSize;
		this.minOreValue = minOreValue;
		this.maxOreValue = maxOreValue;
	}

	public void generateOres(){
		for (int i = 0; i < oreNum; i++) {
			generateAnOre ();
		}
	}

	static int maxTry = 30;
	void generateAnOre(){
		for (int i = 0; i < maxTry; i++) {
			var pos = randomPosInScene ();
			var ore = randomOreNum (pos.magnitude);
			if(checkAvailable(pos, ore)){
				putOre (pos, ore);
				return;
			}
		}
		Debug.Log ("Max try reached while generating ore! Maybe it's no where to put the new ores.");
	}

	System.Random random = new System.Random (1234); //Use the same seed for debug sake.
	public Vector2 randomPosInScene(){
		var x = mapSize * (float)(random.NextDouble () - 0.5);
		var y = mapSize * (float)(random.NextDouble () - 0.5);
		return new Vector2 (x, y);
	}

	float randomOreNum(float disToOrigin){
		var disModifier = Mathf.Sqrt (disToOrigin / (Mathf.Sqrt (2) * mapSize));
		var x = UnityEngine.Random.value;
		return (minOreValue + x * x * (maxOreValue - minOreValue)) * disModifier;
	}
}
