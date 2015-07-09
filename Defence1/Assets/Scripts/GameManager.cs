﻿using UnityEngine;
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
	public InitializeScript initScript;

	//Prefabs-----------
	public GameObject emptyPrefab;
	public GameObject orePrefab;
	public Miner minerPrefab;
	public WeaponTower towerPrefab;
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
	public GameObject towerParent {get; private set;}

	// Use this for initialization
	void Start () {
		Get = this; //setup singleton.

		setupParents ();

		initScript.initialize (this);
		resourceControl = new ResourceControl (initOre: initScript.initOre(), updateOre: v=> UIManager.Get.oreText.text = string.Format ("Ore: {0}",v) );
		if(shouldGenerateMap)
			generateMap ();
	}

	void setupParents(){
		energyPointParent = Instantiate (emptyPrefab);
		energyPointParent.name = "Energy Points";
		towerParent = Instantiate (emptyPrefab);
		towerParent.name = "Towers";
	}

	void generateMap(){
		var oreParent = Instantiate (emptyPrefab);
		oreParent.name = "Ores";

		var mapGen = new MapGenerator(
			oreNum: 100, mapSize: mapSize,
			putOre: (pos, ore) => {
				var oreObject = Instantiate (orePrefab);
				oreObject.transform.parent = oreParent.transform;
				oreObject.GetComponent <Ore>().init(pos,ore);
			},
			checkAvailable: (pos, ore) => Physics.OverlapSphere (Vector3Extension.fromVec2 (pos), Ore.radiusOfAmount (ore)).Length == 0 
		);
		mapGen.generateOres ();
	}

	T instantiateUnderParent<T> (T prefab) where T: Tower{
		var t = Instantiate (prefab);
		t.transform.parent = towerParent.transform;
		return t;
	}

	public Miner createMiner(Vector2 pos){
		var miner = instantiateUnderParent (minerPrefab);
		miner.init (pos: pos, oreCollected: delta => resourceControl.tryChangeOre (delta));
		return miner;
	}
	#region These Tower methods can be merged.
	public WeaponTower createTower(Vector2 pos){
		var tower = instantiateUnderParent (towerPrefab);
		tower.init (pos);
		return tower;
	}
	
	public WeaponTower createLaserTower(Vector2 pos){
		var tower = instantiateUnderParent (laserTowerPrefab);
		tower.init (pos);
		return tower;
	}
	
	public WeaponTower createCannonTower(Vector2 pos){
		var tower = instantiateUnderParent (cannonTowerPrefab);
		tower.init (pos);
		return tower;
	}
	
	public WeaponTower createFireTower(Vector2 pos){
		var tower = instantiateUnderParent (fireTowerPrefab);
		tower.init (pos);
		return tower;
	}
	#endregion

	public Generator createGenerator(Vector2 pos){
		var station = instantiateUnderParent (generatorPrefab);
		station.init (pos);
		return station;
	}

	public PowerRedirector createPowerRedirector(Vector2 pos){
		var r = instantiateUnderParent (redirectorPrefab);
		r.init (pos);
		return r;
	}

	public Tower createTowerOfType(Vector2 pos, TowerType towerType){
		switch (towerType) {
		case TowerType.Redirector:
			return createPowerRedirector (pos);
		case TowerType.Miner:
			return createMiner (pos);
		case TowerType.LaserTower:
			return createLaserTower (pos);
		case TowerType.CannonTower:
			return createCannonTower (pos);
		case TowerType.FireTower:
			return createFireTower (pos);
		case TowerType.Generator:
			return createGenerator (pos);
		default:
			throw new UnityException ("Don't know what to create!");
		}
	}

	public Tower getPrefabOfType(TowerType t){
		switch(t){
		case TowerType.Miner:
			return minerPrefab;
		case TowerType.LaserTower:
			return laserTowerPrefab;
		case TowerType.CannonTower:
			return cannonTowerPrefab;
		case TowerType.FireTower:
			return fireTowerPrefab;
		case TowerType.Generator:
			return generatorPrefab;
		case TowerType.Redirector:
			return redirectorPrefab;
		default:
			throw new UnityException ("Can't find prefab of this type!");
		}
	}

	public ConstructingTower createConstructingTower(Vector2 pos, TowerType towerType, GameObject model){
		model.transform.parent = towerParent.transform;
		var t = model.AddComponent<ConstructingTower> ();
		var maxHp = 200f; //TODO need to be improved.
		t.init (pos, maxHp, ConstructingTower.powerNeedToConstruct (towerType),
			createTower: () => createTowerOfType (pos, towerType));

		return t;
	}
		
}

