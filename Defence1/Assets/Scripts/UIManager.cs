using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

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
				switch(value){
				case Towers.None:
					Destroy (previewTower.gameObject);
					previewTower = null;
					break;
				case Towers.Miner:
					previewTower = makePreview (gManager.minerPrefab.gameObject);
					break;
				case Towers.Tower:
					previewTower = makePreviewTower (gManager.towerPrefab.gameObject);
					break;
				}
			}
		}
	}


	void Start () { }

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

	Preview makePreview(GameObject prefab){
		var obj = Instantiate (prefab);
		destroyOptionally (obj.GetComponent<Miner> ());

		var r = obj.AddComponent <Rigidbody>();
		r.isKinematic = true;
		var preview = obj.AddComponent <Preview>();

		return preview;
	}
	
	Preview makePreviewTower(GameObject prefab){
		var obj = Instantiate (prefab);
		destroyOptionally (obj.GetComponent<Tower> ());
		
		var r = obj.AddComponent <Rigidbody>();
		r.isKinematic = true;
		var preview = obj.AddComponent <Preview>();
		
		return preview;
	}

	void destroyOptionally(MonoBehaviour b){
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

				switch (previewState) {
				case Towers.Miner:
					gManager.createMiner (new Vector2 (pos.x, pos.z));
					break;
				case Towers.Tower:
					gManager.createTower (new Vector2 (pos.x, pos.z));
					break;
				default:
					throw new UnityException ("Don't know what to create!");
				}
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
		
}


public enum Towers{
	None,
	Miner,
	Tower
}