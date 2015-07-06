using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class EffectManager : MonoBehaviour {
	public static Color connectionColor = new Color (0.3f, 0.3f, 1f);
	public static float connectionWidth = 0.35f;

	public LineRenderer connectionLinePrefab;
	public GameObject emptyPrefab;

	void Start(){
		pointOverEffectObject = Instantiate(emptyPrefab);
		pointOverEffectObject.name = "Effects";
	}

	void Update(){
			handleMousePoint ();
	}

	GameObject lastPointOver;
	GameObject pointOverEffectObject;

	void handleMousePoint(){
		var ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hitInfo;
		if(Physics.Raycast (ray, out hitInfo)){
			var obj = hitInfo.collider.gameObject;
			if (obj != lastPointOver) {
				clearPointOverObject ();

				var tower = obj.GetComponent <TowerParent> ();
				if (tower != null) {
					mouseOverTower (tower);
				}

				lastPointOver = obj;
			}
		}else{
			clearPointOverObject ();
			lastPointOver = null;
		}
	}

	// Don't know where the bug is. GetChild will trigger errors.
//	void clearPointOverObject(){
//		var n = pointOverEffectObject.transform.childCount;
//		Debug.Log ("Child Count: "+n );
//		var allChilden = new List<Transform> ();
//		for (int i = 0; i < n; i++) {
//			allChilden.Add (pointOverEffectObject.transform.GetChild (n));
//		}
//
//		allChilden.ForEach (Destroy);
//		pointOverEffectObject.transform.DetachChildren ();
//	}

	void clearPointOverObject() {
		if(pointOverEffectObject.transform.childCount != 0){
			Destroy(pointOverEffectObject.gameObject);
			pointOverEffectObject = Instantiate(emptyPrefab);
			pointOverEffectObject.name = "Effects";
		}
	}

	void mouseOverTower(TowerParent tower){
		var node = tower.energyNode;
		var startPos = node.transform.position;
		foreach (var n in node.targetNodes) {
			var endPos = n.transform.position;
			makeLine (connectionLinePrefab, startPos, endPos, connectionColor, connectionColor,
				connectionWidth, 0, pointOverEffectObject, "Connection");
		}
	}

	LineRenderer makeLine(LineRenderer prefab ,Vector3 start, Vector3 end, Color colorStart, Color colorEnd,
		float widthStart, float widthEnd, GameObject parent, string lineName){

		var render = Instantiate (prefab);
		var lineObj = render.gameObject;
		lineObj.transform.parent = parent.transform;
		lineObj.name = lineName;

		render.SetVertexCount (2);
		render.SetPosition (0, start);
		render.SetPosition (1, end);
		render.SetColors (colorStart, colorEnd);
		render.SetWidth (widthStart,widthEnd);

		return render;
	}
}
