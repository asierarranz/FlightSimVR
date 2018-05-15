//
//Property of Oyedoyin Dada
//cc dadaoyedoyin@gmail.com
//
//
//using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SilantroEngineHealth : MonoBehaviour {
	//
	[Header("Health Values")]
	//Health Values
	public float startingHealth = 100.0f;		// The amount of health to start with
	[SerializeField][SilantroShowAttribute]public float currentHealth;	
	//
	[Header("Attached Parts")]
	public GameObject[] attachments;
	//
	[Header("Effects")]
	public GameObject engineFire;
	GameObject actualFire;
	public GameObject ExplosionPrefab;
	//
	private bool destroyed;
	private bool engineOnFire;
	private Vector3 dropVelocity;

	//
	[Header("Manual Control")]
	public bool Explode;
	// Use this for initialization
	void Start () {
		currentHealth = startingHealth;engineOnFire = false;
		if (engineFire != null) {
			actualFire = Instantiate (engineFire, this.transform.position, Quaternion.identity);
			actualFire.transform.parent = this.transform;
		}
		if(actualFire != null)
			actualFire.SetActive (false);
	}
	//MANUAL CONTROL
	void Update()
	{
		if (Explode && !destroyed) {
			currentHealth = 0;
			Disintegrate ();
		}
	}
	//HIT SYSTEM
	public void SilantroDamage(float amount)
	{
		currentHealth += amount;

		// If the health runs out, then Die.
		if (currentHealth < 0)
		{
			currentHealth = 0;
		}
		if (currentHealth < 50f && !engineOnFire) {
			engineFire.SetActive (true);
		}
		//Die Procedure
		if (currentHealth == 0 && !destroyed)
			Disintegrate();
	}
	//
	//DESTRUCTION SYSTEM
	public void Disintegrate()
	{
		if (transform.root.GetComponent<Rigidbody> ()) {
			dropVelocity = transform.root.GetComponent<Rigidbody> ().velocity;
		}
		destroyed = true;
		gameObject.SendMessage ("DestroyEngine", SendMessageOptions.DontRequireReceiver);
		//
		//ADD COLLIDERS TO ATTACHED PARTS
		int j;
		if (attachments.Length > 0) {
			for (j = 0; j < attachments.Length; j++) {
				attachments [j].transform.parent = null;
				//Attach Box collider
				if (!attachments [j].GetComponent<BoxCollider> ()) {
					attachments [j].AddComponent<BoxCollider> ();
				}
				//Attach Rigibbody
				if (!attachments [j].GetComponent<Rigidbody> ()) {
					attachments [j].AddComponent<Rigidbody> ();
				}
				attachments [j].GetComponent<Rigidbody> ().mass = 300.0f;
				attachments [j].GetComponent<Rigidbody> ().velocity = dropVelocity;
			}
		}
		gameObject.transform.parent = null;
		//
		if (GetComponent<Collider> () == null) {
			gameObject.AddComponent<CapsuleCollider> ();
		}
		if (!gameObject.GetComponent<Rigidbody> ()) {
			gameObject.AddComponent<Rigidbody> ();
		}
		gameObject.GetComponent<Rigidbody> ().isKinematic = false;
		gameObject.GetComponent<Rigidbody> ().mass = 200f;
		gameObject.GetComponent<Rigidbody> ().velocity = dropVelocity;
		//
		if (actualFire ) {
			actualFire.SetActive (true);

		}
		if (ExplosionPrefab != null) {
			GameObject explosion = Instantiate (ExplosionPrefab, this.transform.position, Quaternion.identity);
			explosion.GetComponentInChildren<AudioSource> ().Play ();
		}	
		if (gameObject.GetComponent<SilantroTurboProp> ()) {
			//Destroy (gameObject.GetComponent<SilantroTurboProp> ());
		}
		if (gameObject.GetComponent<SilantroTurboFan> ()) {
			//Destroy (gameObject.GetComponent<SilantroTurbofan> ());
		}
		Destroy (this.GetComponent<SilantroEngineHealth> ());
	}
}
