//
//Property of Oyedoyin Dada
//cc dadaoyedoyin@gmail.com
//
//
//using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SilantroAerofoilHealth : MonoBehaviour {
	//
	[Header("Health Values")]
	//Health Values
	public float startingHealth = 100.0f;		// The amount of health to start with
	[SerializeField][SilantroShowAttribute]public float currentHealth;
	//
	[Header("Attached Parts")]
	public SilantroAerofoil extraAerofoil;
	//
	[Header("Attached Parts")]
	public GameObject[] attachments;
	//
	[Header("Attached Weapons")]
	public GameObject[] weapons;
	//
	[Header("Attached Engines")]
	public GameObject[] engines;
	//
	private bool destroyed;
	[Header("Manual Control")]
	public bool Explode;
	//
	[Header("Effects")]
	public GameObject ExplosionPrefab;
	public Transform explosionPoint;
	public GameObject firePrefab;
	//
	//public string type;
	//
	[HideInInspector]public bool isDistructible = true;

	private Rigidbody rb;
	private Collider col;
	private Vector3 dropVelocity;
	//
	// Use this for initialization
	void Start () {
		currentHealth = startingHealth;
		//
		//explosionPoint =th.transform;
	}
	//
	void Update()
	{
		if (Explode && !destroyed) {
			currentHealth = 0;
			Disintegrate ();
		}
		//

	}
	//
	//
	//HIT SYSTEM
	public void SilantroDamage(float amount)
	{
		currentHealth += amount;

		// If the health runs out, then Die.
		if (currentHealth < 0)
		{
			currentHealth = 0;
		}
		//Die Procedure
		if (currentHealth == 0 && !destroyed)
			Disintegrate();
	}
	//
	//DESTRUCTION SYSTEM
	public void Disintegrate()
	{
		if(isDistructible){
		//
			if (transform.root.gameObject.GetComponent<Rigidbody> ()) {
				dropVelocity = transform.root.gameObject.GetComponent<Rigidbody> ().velocity;
			}
		//
		destroyed = true;
		//
		if (explosionPoint == null) {
			explosionPoint = this.transform;
		}
		//ACTIVATE EXPLOSION AND FIRE
		if (ExplosionPrefab != null) {
			GameObject explosion = Instantiate (ExplosionPrefab, explosionPoint.position, Quaternion.identity);
				explosion.SetActive (true);
			explosion.GetComponentInChildren<AudioSource> ().Play ();
		}	
		if (firePrefab != null) {
				GameObject fire = Instantiate (firePrefab, explosionPoint.position, Quaternion.identity);
				fire.SetActive (true);fire.transform.parent = gameObject.transform;fire.transform.localPosition = new Vector3 (0, 0, 0);
				fire.GetComponentInChildren<AudioSource> ().Play ();
		}
		//
		//ADD COLLIDERS TO ATTACHED PARTS
		int a;
		if (attachments.Length > 0) {
			for (a = 0; a < attachments.Length; a++) {
				attachments [a].transform.parent = null;
				//Attach Box collider
				if (!attachments [a].GetComponent<BoxCollider> ()) {
					attachments [a].AddComponent<BoxCollider> ();
				}
				//Attach Rigibbody
				if (!attachments [a].GetComponent<Rigidbody> ()) {
					attachments [a].AddComponent<Rigidbody> ();
				}
				attachments [a].GetComponent<Rigidbody> ().mass = 300.0f;
				attachments [a].GetComponent<Rigidbody> ().velocity = dropVelocity;
			}
		}
		//
		//DEACTIVATE AEROFOILS
		//if(this.GetComponent<SilantroSlats> ())
		//Destroy(this.GetComponent<SilantroSlats> ());
		//
		if(this.GetComponent<SilantroFlap> ())
		Destroy(this.GetComponent<SilantroFlap> ());
		//
		if(this.GetComponent<SilantroControlSurface> ())
		Destroy(this.GetComponent<SilantroControlSurface> ());
		//
		if(this.GetComponent<SilantroAerofoil> ())
		Destroy(this.GetComponent<SilantroAerofoil> ());
		//Detroy Extra Aerofoils
		if (extraAerofoil != null) {
			
			Destroy (extraAerofoil.gameObject);
		}

		//
		//DROP ANY ATTACHED WEAPON
		int d;
		if (weapons.Length > 0) {
			for (d = 0; d < weapons.Length; d++) {
				if (weapons [d] != null && weapons [d].GetComponent<Rigidbody> ()) {
					weapons [d].GetComponent<Rigidbody> ().isKinematic = false;
					weapons [d].GetComponent<Rigidbody> ().velocity = dropVelocity;//weapons [d].transform.root.gameObject.GetComponent<Rigidbody> ().velocity;
					weapons [d].transform.parent = null;
				}
			}
		}
		//
		//DESTROY ATTACHED ENGINES
		int e;
		if (engines.Length > 0) {
			for (e = 0; e < engines.Length; e++) {
				if(engines [e].GetComponent<SilantroEngineHealth> ())
				engines [e].GetComponent<SilantroEngineHealth> ().Disintegrate ();
			}
		}
		//DESTROY GAMEOBJECT
		Destroy(gameObject);
	}
}
	//
	void OnCollisionEnter(Collision col)
	{
		if (col.relativeVelocity.magnitude >100f) {
			Disintegrate ();
		}
	}
}
