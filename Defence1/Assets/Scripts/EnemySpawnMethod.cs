using UnityEngine;
using System.Collections;
using System;

public class EnemySpawnMethod {
	public static float extendBoundSize = 10;

	public float spawnDecayTime;
	public float maxStrengthTime;
	public float arrivalUncertainty;
	public float mapSize;

	IEnemySpawner spawner;
	float extendedSize;
	float initSpawnInterval;

	public EnemySpawnMethod(IEnemySpawner spawner, float mapSize, float initSpawnInterval = 20, float spawnIntervalDecayTime = 100, 
		float arrivalUncertainty = 0.5f, float maxStrengthTime = 200){
		this.initSpawnInterval = initSpawnInterval;
		this.mapSize = mapSize;
		extendedSize = mapSize + extendBoundSize;
		this.arrivalUncertainty = arrivalUncertainty;
		this.spawner = spawner;
		this.spawnDecayTime = spawnIntervalDecayTime;
		this.maxStrengthTime = maxStrengthTime;
	}

	float time = 0;
	float nextSpawnTime = 0;

	public void timePassed(float dt){
		time += dt;

		if(time >= nextSpawnTime){
			//Spawn code...
			spawnOne (); // need to improve in the future

			nextSpawnTime += calcTimeBeforeNextSpawn ();
		}
	}
		
	float randomStrength(){
		var maxStrength = Mathf.Min (1f, time / maxStrengthTime);
		return maxStrength * ProbabilityMath.value;
	}

	float calcTimeBeforeNextSpawn(){
		var interval = initSpawnInterval * Mathf.Exp (-time / spawnDecayTime);
		var offset = interval * arrivalUncertainty * ProbabilityMath.normalDistribute ();
		return interval + offset;
	}

	Vector2 randomPosAtBound(){
		float x = -0.5f, y = -0.5f;
		if(ProbabilityMath.halfChance ()){
			x += ProbabilityMath.oneOrZero ();
			y += ProbabilityMath.value;
		}else{
			x += ProbabilityMath.value;
			y += ProbabilityMath.oneOrZero ();
		}

		return new Vector2 (x, y) * extendedSize;
	}

	static int maxSpawnTry = 20;
	void spawnOne(){
		for (int i = 0; i < maxSpawnTry; i++) {
			var strength = randomStrength ();
			var pos = randomPosAtBound ();
			if(spawner.spawnEnemyOfStrength (strength, pos)){
				return;
			}
		}
		Debug.Log ("Max Spawn Try Reached!"); 
	}

}
	
public interface IEnemySpawner {

	/// <summary>
	/// Spawn the enemy of specific strength.
	/// </summary>
	/// <returns><c>true</c>, if enemy was created, <c>false</c> the enemy can't be created at this position.</returns>
	/// <param name="strength">Strength.</param>
	/// <param name="pos">Position.</param>
	bool spawnEnemyOfStrength (float strength, Vector2 pos);

}
