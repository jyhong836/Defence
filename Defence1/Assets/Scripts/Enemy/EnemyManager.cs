using UnityEngine;
using System.Collections;

public class EnemyManager : MonoBehaviour, IEnemySpawner {

	public GameManager gameManager;
	public FighterEnemy fighterEnemyPrefab;

	EnemySpawnMethod method;
	Transform enemyParent;

	void Start(){
		var obj = Instantiate (gameManager.emptyPrefab);
		obj.name = "Enemies";
		enemyParent = obj.transform;

		method = new EnemySpawnMethod (this, gameManager.mapSize, initSpawnInterval: 10, spawnIntervalDecayTime: 100);
	}

	void FixedUpdate() {
		method.timePassed (Time.fixedDeltaTime);
	}

	Enemy createEnemy(Vector2 pos) {
		var enemy = Instantiate (fighterEnemyPrefab);
		enemy.create(pos);

		enemy.transform.parent = enemyParent;
		return enemy;
	}

	#region IEnemySpawner implementation

	public bool spawnEnemyOfStrength (float strength, Vector2 pos){
		var enemyRadius = 1f;
		if (Physics.OverlapSphere (pos, enemyRadius).Length > 0)
			return false;
		else{
//			Debug.Log ("spawn with strength: " + strength);
			createEnemy (pos); // need enhancements.
			return true;
		}
	}

	#endregion
}
