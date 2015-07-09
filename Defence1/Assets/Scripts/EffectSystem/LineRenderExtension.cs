using UnityEngine;
using System.Collections;
using System;

public static class LineRenderExtension {

	public static void makeConnection(this LineRenderer render,Vector3 start, Vector3 end, Color color,
		float lineWidth, ConnectionMode mode, GameObject parent, string lineName){
	
		render.material.color = color;
		var lineObj = render.gameObject;
		lineObj.transform.parent = parent.transform;
		lineObj.name = lineName;

		var smallWidth = lineWidth / 5;
		float startWidth, endWidth;
		switch(mode){
		case ConnectionMode.Standard:
			startWidth = lineWidth;
			endWidth = lineWidth;
			break;
		case ConnectionMode.Send:
			startWidth = lineWidth;
			endWidth = smallWidth;
			break;
		case ConnectionMode.Receieve:
			startWidth = smallWidth;
			endWidth = lineWidth;
			break;
		default:
			throw new NotImplementedException ();
		}

		render.SetVertexCount (2);
		render.SetPosition (0, start);
		render.SetPosition (1, end);
		render.SetWidth (startWidth,endWidth);
	}
}

public enum ConnectionMode{
	Standard,
	Send,
	Receieve
}