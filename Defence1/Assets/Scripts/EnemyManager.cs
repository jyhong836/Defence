using UnityEngine;
using System.Collections;

public class EnemyManager : MonoBehaviour {

	public GameManager gameManager;
	public Enemy enemyPrefab;
	public float enemyUpdateInterval = 1f; // sec
	private float nextEnemyTime = 0;
	private bool startGenerate = true; // false;

	void FixedUpdate() {
		if (startGenerate)
			if (nextEnemyTime <= 0) {
				createEnemy (gameManager.randomPosAtBound ());
				nextEnemyTime = Random.value * enemyUpdateInterval;
			} else {
				nextEnemyTime -= Time.fixedDeltaTime;
			}
	}

	public void startGenerateEnemy (bool start) {
		startGenerate = start;
	}

	public Enemy createEnemy(Vector2 pos) {
		var enemy = Instantiate (enemyPrefab);
		enemy.init(gameManager.randomPosAtBound());
		return enemy;
	}
}
