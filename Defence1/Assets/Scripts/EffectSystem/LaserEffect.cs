using UnityEngine;
using System.Collections;

public class LaserEffect : MonoBehaviour {

	public float lightTravelSpeed = 1f;

	LineRenderer lineRender;
	float phase;

	bool _showEffect;
	public bool showEffect {
		get { return _showEffect;}
		set {
			_showEffect = value;
			lineRender.enabled = value;
		}
	}

	/// <summary>
	/// You should call this whenever laser position changed.
	/// </summary>
	/// <param name="start">Start.</param>
	/// <param name="end">End.</param>
	public void setEndpoints(Vector3 start, Vector3 end){
		lineRender.SetPosition (0, start);
		lineRender.SetPosition (1, end);
	}
		
	void Start(){
		lineRender = GetComponent <LineRenderer>();
		showEffect = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(lineRender.isVisible && showEffect){
			lineRender.material.mainTextureOffset = new Vector2 (phase,0);
			phase += lightTravelSpeed * Time.deltaTime;
		}
	}
}
