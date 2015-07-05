using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

public class GameManager : MonoBehaviour {


	public UIManager uiManager;
	public float mapSize = 10f;
	public GameObject emptyPrefab;
	public GameObject orePrefab;
	public Miner minerPrefab;
	public Tower towerPrefab;
	public Generator generatorPrefab;
	public PowerRedirector redirectorPrefab;

	public ResourceControl resourceControl { get; private set;}
	public bool shouldGenerateMap = true;

	// Use this for initialization
	void Start () {
		resourceControl = new ResourceControl (initOre: 200, updateOre: v=> uiManager.oreText.text = string.Format ("Ore: {0}",v) );
		if(shouldGenerateMap)
			generateMap ();
	}

	// Update is called once per frame
	void Update () { }

	void generateMap(){
		var mapGen = new MapGenerator(oreNum: 50);
		var oreParent = Instantiate<GameObject> (emptyPrefab);
		oreParent.name = "Ores";
		mapGen.generateOres (
			genFunc: (pos, ore) => {
				var oreObject = Instantiate (orePrefab);
				oreObject.transform.parent = oreParent.transform;
				oreObject.GetComponent <Ore>().init(pos,ore);
			},
			randomPosInScene: this.randomPosInScene
		);
	}

	public Vector2 randomPosInScene(){
		var x = mapSize * (UnityEngine.Random.value - 0.5f);
		var y = mapSize * (UnityEngine.Random.value - 0.5f);
		return new Vector2 (x, y);
	}

	public Vector2 randomPosAtBound(){
		var x = mapSize * (UnityEngine.Random.value - 0.5f);
		var y = mapSize * (UnityEngine.Random.value - 0.5f);
		if (UnityEngine.Random.value > 0.5)
			x = mapSize*(0.5f - (float)Math.Round(UnityEngine.Random.value));
		else 
			y = mapSize*(0.5f - (float)Math.Round(UnityEngine.Random.value));
		return new Vector2 (x, y);
	}

	public Miner createMiner(Vector2 pos){
		var miner = Instantiate (minerPrefab);
		miner.init (pos: pos, oreCollected: delta => resourceControl.tryChangeOre (delta));
		return miner;
	}
	
	public Tower createTower(Vector2 pos){
		var tower = Instantiate (towerPrefab);
		tower.init (pos);
		return tower;
	}

	public Generator createGenerator(Vector2 pos){
		var station = Instantiate (generatorPrefab);
		station.init (pos);
		return station;
	}

	public PowerRedirector createPowerRedirector(Vector2 pos){
		var r = Instantiate (redirectorPrefab);
		r.init (pos);
		return r;
	}

}

