using UnityEngine;
using System.Collections;

public interface IAliveable {

	HitpointControl hpControl { get; }

	bool destroyed { get; }
	bool alive { get; }

}
