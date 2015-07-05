using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

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
				switch(value){
				case Towers.None:
					previewTower = null;
					break;
				case Towers.Miner:
					previewTower = makePreview (gManager.minerPrefab);
					break;
				case Towers.Tower:
					previewTower = makePreview (gManager.towerPrefab);
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
		

	void Update () {
		handleMousePoint ();
		handleCancelation ();
		handleTowerPlacement ();
	}
	
	public void TowerButtonClicked(){
		previewState = Towers.Tower;
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
		
	Preview makePreview<T> (T prefab) where T: TowerParent{
		var obj = Instantiate (prefab.gameObject);
		destroyOptionally (obj.GetComponent<T> ());
		obj.name = "Preview Model";

		var r = obj.AddComponent <Rigidbody>();
		r.isKinematic = true;
		var preview = obj.AddComponent <Preview>();

		return preview;
	}

	void destroyOptionally(UnityEngine.Object b){
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
		if(Input.GetButtonUp ("LeftClick") && previewTower != null && previewTower.valid){
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
		
}


public enum Towers{
	None,
	Miner,
	Tower,
	Generator,
	Redirector
}