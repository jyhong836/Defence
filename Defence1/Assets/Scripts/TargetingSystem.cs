using UnityEngine;
using System.Collections;
using System;

public class TargetingSystem <T> where T: class{
	public T target;
	public Func<T,Vector2> predictFunc;


	public Vector2 provideTarget(){
		if(target!=null){
			return predictFunc (target);
		}else{
			//should return last targeting position;
			throw new NotImplementedException ();
		}
	}

}
