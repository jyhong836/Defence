using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour {
	public GameManager gManager;
	public Text oreText;
	public Text warningText;

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
				if (value == Towers.None)
					previewTower = null;
				else{
					var prefab = gManager.getPrefabOfType (value);
					previewTower = Preview.makePreview (prefab);
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
		Debug.Log ("Tower is deprecated now. Please use its children.");
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