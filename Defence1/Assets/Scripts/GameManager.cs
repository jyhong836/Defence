using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public Text oreText;

	[SerializeField] private float mapSize = 10f;

	[SerializeField] private GameObject orePrefab;

	ResourceControl resourceControl;

	// Use this for initialization
	void Start () {
		resourceControl = new ResourceControl (initOre: 200, updateOre: v=> oreText.text = string.Format ("Ore: {0}",v) );
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
