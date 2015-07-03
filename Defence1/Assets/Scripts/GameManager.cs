using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using System;

public class GameManager : MonoBehaviour {

	public Text oreText;

	[SerializeField] float mapSize = 10f;

	[SerializeField] GameObject orePrefab;
	[SerializeField] Miner minerPrefab;


	ResourceControl resourceControl;
	[SerializeField] PreviewState _placementState = PreviewState.None;
	Preview previewTower;

	public PreviewState previewState{
		get{ return _placementState;}
		set{
			if(value != _placementState){
				_placementState = value;
				switch(value){
				case PreviewState.None:
					previewTower = null;
					break;
				case PreviewState.Miner:
					previewTower = makePreview (minerPrefab.gameObject);
					break;
				}
			}
		}
	}

	// Use this for initialization
	void Start () {
		resourceControl = new ResourceControl (initOre: 200, updateOre: v=> oreText.text = string.Format ("Ore: {0}",v) );
		generateMap ();
	}

	// Update is called once per frame
	void Update () {
		handleMousePoint ();
		handleTowerPlacement ();
	}

	void generateMap(){
		var mapGen = new MapGenerator(oreNum: 50);
		mapGen.generateOres (
			genFunc: (pos, ore) => {
				var oreObject = Instantiate (orePrefab);
				oreObject.GetComponent <Ore>().init(pos,ore);
			},
			randomPosInScene: this.randomPosInScene
		);
	}

	Preview makePreview(GameObject prefab){
		var obj = Instantiate (prefab);
		destroyOptionally (obj.GetComponent<Miner> ());

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

	public Vector2 randomPosInScene(){
		var x = mapSize * (UnityEngine.Random.value - 0.5f);
		var y = mapSize * (UnityEngine.Random.value - 0.5f);
		return new Vector2 (x, y);
	}

	public void MinerButtonClicked(){
		previewState = PreviewState.Miner;
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

	void handleTowerPlacement() {
		if(Input.GetButtonUp ("LeftClick") && previewTower != null && previewTower.valid){
			
			var pos = previewTower.transform.position;

			switch(previewState){
			case PreviewState.Miner:
				createMiner (new Vector2(pos.x,pos.z));
				break;
			default:
				throw new UnityException ("Don't know what to create!");
			}

		}
	}

	public Miner createMiner(Vector2 pos){
		var miner = Instantiate (minerPrefab);
		miner.init (pos: pos, oreCollected: delta => resourceControl.tryChangeOre (delta));
		return miner;
	}

	/// <summary>
	/// Represent which tower will be added next.
	/// </summary>
	public enum PreviewState{
		None,
		Miner
	}
}

