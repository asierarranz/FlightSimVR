//
//Property of Oyedoyin Dada
//cc dadaoyedoyin@gmail.com
//
//
//using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SilantroCaseSounds : MonoBehaviour {

	public AudioClip[] sounds;
	public float soundRange = 300f;
	private AudioSource audio;
	// Use this for initialization
	void OnCollisionEnter (Collision col) {
		if (col.collider.tag == "Ground") {
			AudioSource audio = gameObject.AddComponent<AudioSource> ();
			audio.dopplerLevel = 0f;
			audio.spatialBlend = 1f;
			audio.rolloffMode = AudioRolloffMode.Custom;
			audio.maxDistance = soundRange;
			audio.PlayOneShot (sounds [Random.Range (0, sounds.Length)]);
		}
	}

}
