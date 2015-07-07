using UnityEngine;
using System.Collections;

public class LaserTower : Tower {
	public LaserEffect attackingaLaser;

	private bool _isFiring;
	protected override bool isFiring {
		get {return isFiring;} 
		set {
			if(_isFiring != value){
				_isFiring = value;
				attackingaLaser.showEffect = value;
				if(value){
					attackingaLaser.setEndpoints (firePoint, currentTarget.transform.position);
				}
			}
		}
	}

	public Vector3 firePoint{
		get{return transform.position;}
	}
}
