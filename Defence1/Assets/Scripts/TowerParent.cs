using UnityEngine;
using System.Collections;

abstract class TowerParent : MonoBehaviour {

	public EnergyNode energyNode;

	/// <summary>
	/// Remember to call this method first in every child's init.
	/// </summary>
	protected void initParent(){
		energyNode = gameObject.AddComponent <EnergyNode>();
	}
}
