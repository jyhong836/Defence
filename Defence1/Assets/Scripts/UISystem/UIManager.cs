using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour {
	public GameManager gManager;
	public Text oreText;
	public Text warningText;

	[SerializeField] float fadeOutTime = 2;

	Preview previewTower;

	public bool inPreviewModel {get; private set;}
	[SerializeField] TowerType _placementState;
	public TowerType previewState{
		get{ return _placementState;}
		set{
			_placementState = value;
			if(inPreviewModel && previewTower == null){
				var prefab = gManager.getPrefabOfType (value);
				previewTower = Preview.makePreview (prefab);
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
		

	void clearPreviewTower(){
		if (previewTower != null) {
			Destroy (previewTower.gameObject);
			previewTower = null;
		}
	}

	public void towerButtonClicked(string name){
		inPreviewModel = true;
		clearPreviewTower ();
		previewState = TowerTypeService.towerTypeOfName (name);
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
		if (Input.GetButtonUp ("LeftClick")) {
			if (previewTower != null && previewTower.valid && !EventSystem.current.IsPointerOverGameObject ()) {
				if (gManager.resourceControl.tryCostOre (previewState)) {
					var pos = previewTower.transform.position;
					var v2 = new Vector2 (pos.x, pos.z);

					previewTower.gameObject.SetActive (false);

					gManager.createConstructingTower (v2, previewState, previewTower.copyAModel ());

					previewTower.gameObject.SetActive (true);
				} else {
					var price = gManager.resourceControl.priceOf (previewState);
					warning (string.Format ("You need at least {0} ore to place this tower.", price));
				}
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

