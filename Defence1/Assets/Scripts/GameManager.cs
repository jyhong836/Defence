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
	public ConstructingTower constructingTowerPrefab;
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

	public Tower createTowerOfType(Vector2 pos, Towers towerType){
		switch (towerType) {
		case Towers.Redirector:
			return createPowerRedirector (pos);
		case Towers.Miner:
			return createMiner (pos);
		case Towers.Tower:
			return createTower (pos);
		case Towers.LaserTower:
			return createLaserTower (pos);
		case Towers.CannonTower:
			return createCannonTower (pos);
		case Towers.FireTower:
			return createFireTower (pos);
		case Towers.Generator:
			return createGenerator (pos);
		default:
			throw new UnityException ("Don't know what to create!");
		}
	}

	public Tower getPrefabOfType(Towers t){
		switch(t){
		case Towers.Miner:
			return minerPrefab;
		case Towers.LaserTower:
			return laserTowerPrefab;
		case Towers.CannonTower:
			return cannonTowerPrefab;
		case Towers.FireTower:
			return fireTowerPrefab;
		case Towers.Generator:
			return generatorPrefab;
		case Towers.Redirector:
			return redirectorPrefab;
		default:
			throw new UnityException ("Can't find prefab of this type!");
		}
	}

	public ConstructingTower createConstructingTower(Vector2 pos, Towers towerType, GameObject model){
		model.transform.parent = towerParent.transform;
		var t = model.AddComponent<ConstructingTower> ();
		t.init (pos, ConstructingTower.powerNeedToConstruct (towerType),
			createTower: () => createTowerOfType (pos, towerType));

		return t;
	}
		
}

