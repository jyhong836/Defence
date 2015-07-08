using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour {
	public GameManager gManager;
	public Text oreText;
	public Text warningText;
	public EnergyRangePreview energyRangePrefab;
	public AttackingRangePreview attackRangePrefab;
	public MiningRangePreview miningRangePrefab;

	[SerializeField] Towers _placementState = Towers.None;
	[SerializeField] float fadeOutTime = 2;

	Preview previewTower;

	public Towers previewState{
		get{ return _placementState;}
		set{
			if(value != _placementState){
				_placementState = value;
				if (previewTower != null) 
					Destroy (previewTower.gameObject);
				switch(value){
				case Towers.None:
					previewTower = null;
					break;
				case Towers.Miner:
					previewTower = makeMinerPreview (gManager.minerPrefab);
					break;
				case Towers.Tower: //TODO This should be removed in the future.
					previewTower = makeWeaponPreview (gManager.towerPrefab);
					break;
				case Towers.LaserTower:
					previewTower = makeWeaponPreview (gManager.laserTowerPrefab);
					break;
				case Towers.CannonTower:
					previewTower = makeWeaponPreview (gManager.cannonTowerPrefab);
					break;
				case Towers.FireTower:
					previewTower = makeWeaponPreview (gManager.fireTowerPrefab);
					break;
				case Towers.Generator:
					previewTower = makePreview (gManager.generatorPrefab);
					break;
				case Towers.Redirector:
					previewTower = makePreview (gManager.redirectorPrefab);
					break;
				default:
					throw new UnityException ("Unknow Preview State.");
				}
			}
		}
	}
		
	void Start () {
		Get = this;
	}

	void Update () {
		handleMousePoint ();
		handleCancelation ();
		handleTowerPlacement ();
	}
	
	public void TowerButtonClicked(){
		previewState = Towers.Tower;
	}
	
	public void LaserTowerButtonClicked(){
		previewState = Towers.LaserTower;
	}
	public void CannonTowerButtonClicked(){
		previewState = Towers.CannonTower;
	}
	public void FireTowerButtonClicked(){
		previewState = Towers.FireTower;
	}

	public void MinerButtonClicked(){
		previewState = Towers.Miner;
	}

	public void GeneratorButtonClicked(){
		previewState = Towers.Generator;
	}

	public void RedirectorButtonClicked(){
		previewState = Towers.Redirector;
	}
		
	Preview makePreview<T> (T prefab) where T: Tower{
		var obj = Instantiate (prefab.gameObject);
		var tower = obj.GetComponent<T> ();
		destroyOptionally (tower);
		obj.name = "Preview Model";
		obj.tag = "Preview";

		var r = obj.AddComponent <Rigidbody> ();
		r.isKinematic = true;

		var showRange = prefab is Generator || prefab is PowerRedirector;
		var energyRange = Instantiate (energyRangePrefab);
		energyRange.init (obj.transform, EnergyNode.transmissionRadius, tower.isRedirector, showRange);


		var preview = obj.AddComponent <Preview> ();
		return preview;
	}

	Preview makeWeaponPreview<U> (U prefab) where U: WeaponTower{
		var preview = makePreview (prefab);

		var attackRange = Instantiate (attackRangePrefab);
		attackRange.init (preview.transform, prefab.attackingRadius);
		return preview;
	}

	Preview makeMinerPreview (Miner prefab){
		var preview = makePreview (prefab);

		var miningRange = Instantiate (miningRangePrefab);
		miningRange.init (preview.transform, Miner.workingRadius);

		return preview;
	}

	public static void destroyOptionally(UnityEngine.Object b){
		if(b!=null){
			Destroy (b);
		}
	}

	void handleMousePoint(){
		if (previewTower != null) {
			var ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			var plane = new Plane (Vector3.up, 0f);

			float dis;
			if (plane.Raycast (ray, out dis)) {
				var pos = ray.GetPoint (dis);
				previewTower.transform.position = pos;
			}
		}
	}

	void handleCancelation(){
		if(Input.GetButtonDown ("Cancel")){
			previewState = Towers.None;
		}
	}

	void handleTowerPlacement() {
		if(Input.GetButtonUp ("LeftClick") && previewTower != null && previewTower.valid && !EventSystem.current.IsPointerOverGameObject()) {
			if (gManager.resourceControl.tryCostOre (previewState)) {
				var pos = previewTower.transform.position;
				var v2 = new Vector2 (pos.x, pos.z);

				hidePreviewTowerWhileDoingThisAction (() => { //must hide the preview, or its layer will become a trouble.
					switch (previewState) {
					case Towers.Redirector:
						gManager.createPowerRedirector (v2);
						break;
					case Towers.Miner:
						gManager.createMiner (v2);
						break;
					case Towers.Tower:
						gManager.createTower (v2);
						break;
					case Towers.LaserTower:
						gManager.createLaserTower (v2);
						break;
					case Towers.CannonTower:
						gManager.createCannonTower (v2);
						break;
					case Towers.FireTower:
						gManager.createFireTower (v2);
						break;
					case Towers.Generator:
						gManager.createGenerator (v2);
						break;
					default:
						throw new UnityException ("Don't know what to create!");
					}
				});
		
			}else{
				var price = gManager.resourceControl.priceOf (previewState);
				warning (string.Format ("You need at least {0} ore to place this tower.", price));
			}

		}
	}

	void hidePreviewTowerWhileDoingThisAction(Action doSomething){
		previewTower.gameObject.SetActive (false);
		doSomething ();
		previewTower.gameObject.SetActive (true);
	}

	Coroutine fadeCoroutine;
	void warning(string info){
		warningText.text = info;
		warningText.color = warningText.color.withAlpha (1f);
		if(fadeCoroutine != null) {
			StopCoroutine (fadeCoroutine);
		}
		fadeCoroutine = StartCoroutine (fadeOutWarningText());
	}

	IEnumerator fadeOutWarningText(){
		var timePast = 0f;
		while(timePast<fadeOutTime){
			warningText.color = warningText.color.withAlpha (1f - timePast/fadeOutTime);
			yield return null;
			timePast += Time.deltaTime;
		}
	
	}

	static UIManager singleton;
	static bool singletonSet;
	public static UIManager Get{
		get{ return singleton; }
		set{
			if (singletonSet)
				throw new UnityException ("UI Singleton already set!!");
			else{
				singletonSet = true;
				singleton = value;
			}
		}
	}
		
}


public enum Towers{
	None,
	Miner,
	Tower, //TODO remove this
	LaserTower,
	CannonTower,
	FireTower,
	Generator,
	Redirector
}