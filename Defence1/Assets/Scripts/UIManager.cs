using UnityEngine;
using System.Collections;

public class UIManager : MonoBehaviour {
	public GameManager gManager;

	[SerializeField] PreviewState _placementState = PreviewState.None;
	Preview previewTower;

	public PreviewState previewState{
		get{ return _placementState;}
		set{
			if(value != _placementState){
				_placementState = value;
				switch(value){
				case PreviewState.None:
					Destroy (previewTower.gameObject);
					previewTower = null;
					break;
				case PreviewState.Miner:
					previewTower = makePreview (gManager.minerPrefab.gameObject);
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


	public void MinerButtonClicked(){
		previewState = PreviewState.Miner;
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
			previewState = PreviewState.None;
		}
	}

	void handleTowerPlacement() {
		if(Input.GetButtonUp ("LeftClick") && previewTower != null && previewTower.valid){

			var pos = previewTower.transform.position;

			switch(previewState){
			case PreviewState.Miner:
				gManager.createMiner (new Vector2(pos.x,pos.z));
				break;
			default:
				throw new UnityException ("Don't know what to create!");
			}

		}
	}

	/// <summary>
	/// Represent which tower will be added next.
	/// </summary>
	public enum PreviewState{
		None,
		Miner
	}
}
