//
//Property of Oyedoyin Dada
//cc dadaoyedoyin@gmail.com
//
//
//using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SilantroHit : MonoBehaviour {

	[Header("Central Body")]
	public GameObject aircraftBody;
	//
	[Header("Damage Multiplier")]
	public float multplier = 1f;
	//

	//
	//HIT SYSTEM
	public void SilantroDamage(float amount)
	{
		SilantroHealth health = aircraftBody.GetComponent<SilantroHealth> ();
		if (health != null) {
			health.SilantroDamage (amount * multplier);
		}
	}

}
