﻿using UnityEngine;
using System.Collections;

public class LaserTower : Tower {
	public LaserEffect attackingaLaser;

	protected override bool isFiring {
		get {return isFiring;} 
		set {
			if(_isFiring != value){
				_isFiring = value;
				attackingaLaser.showEffect = value;
				if(value){
					attackingaLaser.setEndpoints (firePoint, currentTarget.transform.position);
					currentTarget.lifeLeft -= injury;
				}
			}
		}
	}

	public Vector3 firePoint{
		get{return transform.position;}
	}
}
