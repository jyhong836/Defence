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

	[SerializeField] float fadeOutTime = 2;

	Preview previewTower;

	public bool inPreviewModel {get; private set;}
	[SerializeField] TowerType _placementState;
	public TowerType previewState{
		get{ return _placementState;}
		set{
			_placementState = value;
			if(inPreviewModel && previewTower == null){
				switch(value){
				case TowerType.Miner:
					previewTower = makeMinerPreview (gManager.minerPrefab);
					break;
				case TowerType.LaserTower:
					previewTower = makeWeaponPreview (gManager.laserTowerPrefab);
					break;
				case TowerType.CannonTower:
					previewTower = makeWeaponPreview (gManager.cannonTowerPrefab);
					break;
				case TowerType.FireTower:
					previewTower = makeWeaponPreview (gManager.fireTowerPrefab);
					break;
				case TowerType.Generator:
					previewTower = makePreview (gManager.generatorPrefab);
					break;
				case TowerType.Redirector:
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

	public TowerType towerTypeOfName(string name){
		if (name == "Redirector")
			return TowerType.Redirector;
		else if (name == "Generator")
			return TowerType.Generator;
		else if (name == "Miner")
			return TowerType.Miner;
		else if (name == "FireTower")
			return TowerType.FireTower;
		else if (name == "CannonTower")
			return TowerType.CannonTower;
		else if (name == "LaserTower")
			return TowerType.LaserTower;

		throw new ArgumentException ("No such name exists!");
	}

	void clearPreviewTower(){
		if (previewTower != null) {
			Destroy (previewTower.gameObject);
			previewTower = null;
		}
	}

	public void towerButtonClicked(string name){
		inPreviewModel = true;
		clearPreviewTower ();
		previewState = towerTypeOfName (name);
	}
		
	Preview makePreview<T> (T prefab) where T: Tower{
		var obj = Instantiate (prefab.gameObject);
		var tower = obj.GetComponent<T> ();
		destroyOptionally (tower);
		obj.name = "Preview Model";
		obj.tag = "Preview";

		var r = obj.AddComponent <Rigidbody> ();
		r.isKinematic = true;

		var preview = obj.AddComponent <Preview> ();

		var showRange = prefab is Generator || prefab is PowerRedirector;
		var energyRange = Instantiate (energyRangePrefab);
		energyRange.init (obj.transform, EnergyNode.transmissionRadius, tower.isRedirector, showRange);
		preview.addRange (energyRange);

		return preview;
	}

	Preview makeWeaponPreview<U> (U prefab) where U: WeaponTower{
		var preview = makePreview (prefab);

		var attackRange = Instantiate (attackRangePrefab);
		attackRange.init (preview.transform, prefab.attackingRadius);
		preview.addRange (attackRange);
		return preview;
	}

	Preview makeMinerPreview (Miner prefab){
		var preview = makePreview (prefab);

		var miningRange = Instantiate (miningRangePrefab);
		miningRange.init (preview.transform, Miner.workingRadius);
		preview.addRange (miningRange);
		return preview;
	}

	public static void destroyOptionally(UnityEngine.Object b){
		if(b!=null){
			Destroy (b);
		}
	}

	void handleMousePoint(){
		if (inPreviewModel) {
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
			inPreviewModel = false;
			clearPreviewTower ();
		}
	}

	void handleTowerPlacement() {
		if(Input.GetButtonUp ("LeftClick") && previewTower != null && previewTower.valid && !EventSystem.current.IsPointerOverGameObject()) {
			if (gManager.resourceControl.tryCostOre (previewState)) {
				var pos = previewTower.transform.position;
				var v2 = new Vector2 (pos.x, pos.z);

				previewTower.gameObject.SetActive (false);

				gManager.createConstructingTower (v2, previewState, previewTower.copyAModel());

				previewTower.gameObject.SetActive (true);
			}else{
				var price = gManager.resourceControl.priceOf (previewState);
				warning (string.Format ("You need at least {0} ore to place this tower.", price));
			}

		}
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


public enum TowerType{
	Miner,
	LaserTower,
	CannonTower,
	FireTower,
	Generator,
	Redirector
}