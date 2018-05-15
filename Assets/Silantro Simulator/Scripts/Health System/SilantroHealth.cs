using System.Collections;
//
//Property of Oyedoyin Dada
//cc dadaoyedoyin@gmail.com
//
//
//using System.Collections.Generic;
using UnityEngine;

public class SilantroHealth : MonoBehaviour {
	//
	[Header("Health Values")]
	//Health Values
	public float startingHealth = 100.0f;		// The amount of health to start with
	[SerializeField][SilantroShowAttribute]public float currentHealth;	
	//
	[Header("Engines")]
	public GameObject[] engines;
	//
	[Header("Attached Parts")]
	public GameObject[] attachments;
	//
	[Header("Attached Aerofoil")]
	public SilantroAerofoilHealth[] aerofoilHealth;
	//
	[Header("Effects")]
	//public GameObject engineFire;
	public GameObject ExplosionPrefab;
	 Transform explosionPoint;
	public GameObject firePrefab;

	private Rigidbody rb;
	private Collider col;
	private Vector3 dropVelocity;
	//
	private bool destroyed;
	[HideInInspector]public bool isDistructible = true;
	[Header("Manual Control")]
	public bool Explode;
	//
	void Start () {
		currentHealth = startingHealth;
		//
		explosionPoint = this.transform;
	}
	//
	void Update()
	{
		if (Explode && !destroyed) {
			currentHealth = 0;
			Disintegrate ();
		}
		if(Input.GetKeyDown(KeyCode.F5))
			{
			Explode = true;
			}
	}
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
	//
	//DESTRUCTION SYSTEM
	public void Disintegrate()
	{
		if (isDistructible) {
			if (transform.root.GetComponent<Rigidbody> ()) {
				dropVelocity = transform.root.GetComponent<Rigidbody> ().velocity;
			}
			destroyed = true;
			//
			//ACTIVATE EXPLOSION AND FIRE
			if (explosionPoint == null) {
				explosionPoint = transform;
			}
			//
			if (ExplosionPrefab != null) {
				GameObject explosion = Instantiate (ExplosionPrefab, explosionPoint.position, Quaternion.identity);
				explosion.SetActive (true);
				explosion.GetComponentInChildren<AudioSource> ().Play ();
			}
			if (firePrefab != null) {
				GameObject fire = Instantiate (firePrefab, explosionPoint.position, Quaternion.identity);
				fire.SetActive (true);fire.transform.parent = gameObject.transform.root;fire.transform.localPosition = new Vector3 (0, 0, 0);
				fire.GetComponentInChildren<AudioSource> ().Play ();
			}
			//
			int a;
			if (aerofoilHealth.Length > 0) {
				for (a = 0; a < aerofoilHealth.Length; a++) {
					if (aerofoilHealth [a] != null) {
						aerofoilHealth [a].Disintegrate ();
					}
				}
			}
			//
			//
			//
			WheelCollider[] wheels = GetComponentsInChildren<WheelCollider> ();
			if (wheels.Length > 0) {
				int y;
				for (y = 0; y < wheels.Length; y++) {
					Destroy (wheels [y].gameObject);
				}
			}
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
			//

			//
			//SHUTDOWN ENGINE
			int l;
			if (engines.Length > 0) {
				for (l = 0; l < engines.Length; l++) {
					engines [l].SendMessage ("DestroyEngine", SendMessageOptions.DontRequireReceiver);
					engines [l].SendMessage ("Disintegrate", SendMessageOptions.DontRequireReceiver);

				}
			}

			//
			//
			//Destroy (GetComponentInChildren<SilantroTargetingSystem> ());
			//if(GetComponent<SilantroPropController>())Destroy(GetComponent<SilantroPropController>());
			//if(GetComponent<SilantroJetController>())Destroy(GetComponent<SilantroJetController>());
			Destroy (GetComponent<SilantroHealth> ());

		}
	}
	//DAMAGE
	void OnCollisionEnter(Collision col)
	{
		//if (col.collider.transform.root.tag != "Ground") {
		if (col.relativeVelocity.magnitude >50f) {
				Disintegrate ();
			}
	///	}
	}
}
