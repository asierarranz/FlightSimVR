//
//Property of Oyedoyin Dada
//cc dadaoyedoyin@gmail.com
//
//
//using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SilantroTimeDestroy : MonoBehaviour {
	public float destroyTime = 5f;
	// Use this for initialization
	void Start () {
		Destroy (gameObject, destroyTime);	
	}
}
