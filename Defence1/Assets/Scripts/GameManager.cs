using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class GameManager : MonoBehaviour {

	[SerializeField] private float mapSize = 10f;

	[SerializeField] private GameObject orePrefab;



	// Use this for initialization
	void Start () {
		generateMap ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void generateMap(){
		var mapGen = new MapGenerator(oreNum: 50);
		mapGen.generateOres (
			genFunc: (pos, ore) => {
				var oreObject = Instantiate (orePrefab);
				oreObject.GetComponent <Ore>().init(pos,ore);
			},
			randomPosInScene: this.randomPosInScene
		);
	}

	public Vector2 randomPosInScene(){
		var x = mapSize * (Random.value - 0.5f);
		var y = mapSize * (Random.value - 0.5f);
		return new Vector2 (x, y);
	}

}
	