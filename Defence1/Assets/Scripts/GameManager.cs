using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

public class GameManager : MonoBehaviour {
	static GameManager singleton;
	static bool singletonSet;
	public static GameManager Get{
		get{ return singleton; }
		set{
			if (singletonSet)
				throw new UnityException ("Singleton already set!!");
			else{
				singletonSet = true;
				singleton = value;
			}
		}
	}
		
	public float mapSize = 10f;

	//Prefabs-----------
	public GameObject emptyPrefab;
	public GameObject orePrefab;
	public Miner minerPrefab;
	public Tower towerPrefab;
	public LaserTower laserTowerPrefab;
	public CannonTower cannonTowerPrefab;
	public FireTower fireTowerPrefab;
	public Generator generatorPrefab;
	public PowerRedirector redirectorPrefab;
	public EnergyPoint energyPointPrefab;
	//Prefabs----------------

	public bool shouldGenerateMap = true;

	public ResourceControl resourceControl { get; private set;}
	public GameObject energyPointParent { get; private set;}

	// Use this for initialization
	void Start () {
		Get = this; //setup singleton.

		resourceControl = new ResourceControl (initOre: 200, updateOre: v=> UIManager.Get.oreText.text = string.Format ("Ore: {0}",v) );
		setupParents ();
		if(shouldGenerateMap)
			generateMap ();
	}

	void setupParents(){
		energyPointParent = Instantiate (emptyPrefab);
		energyPointParent.name = "Energy Points";
	}

	void generateMap(){
		var mapGen = new MapGenerator(oreNum: 100);
		var oreParent = Instantiate (emptyPrefab);
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

	System.Random random = new System.Random (1234); //Use the same seed for debug sake.
	public Vector2 randomPosInScene(float oreAmount){
		var r = Ore.radiusOfAmount (oreAmount);
		var x = mapSize * (float)(random.NextDouble () - 0.5);
		var y = mapSize * (float)(random.NextDouble () - 0.5);

		var colliders = Physics.OverlapSphere (Vector3Extension.fromVec2 (x, y),r);
		if(colliders.Length>0){
			return randomPosInScene (oreAmount);
		}else
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
	
	public Tower createLaserTower(Vector2 pos){
		var tower = Instantiate (laserTowerPrefab);
		tower.init (pos);
		return tower;
	}
	
	public Tower createCannonTower(Vector2 pos){
		var tower = Instantiate (cannonTowerPrefab);
		tower.init (pos);
		return tower;
	}
	
	public Tower createFireTower(Vector2 pos){
		var tower = Instantiate (fireTowerPrefab);
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

