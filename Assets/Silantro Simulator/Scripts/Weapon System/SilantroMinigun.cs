//
//Property of Oyedoyin Dada
//cc dadaoyedoyin@gmail.com
//
//
//using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SilantroMinigun : MonoBehaviour {

	[Header("Weapon Properties")]
	public float damage=10f;
	//
	[Header("Fire Settings")]
	public float rateOfFire = 5;
	[SerializeField][SilantroShowAttribute]private float actualRate;
	[SerializeField][SilantroShowAttribute]private float fireTimer;
	//float actualFireTimer;
	//
	[Header("Weapon Accuracy")]
	public float accuracy = 80f;
	[SerializeField][SilantroShowAttribute]public float currentAccuracy;
	private float accuracyDrop = 0.2f;
	private float accuracyRecover = 0.5f;
	float acc;
	//
	public float range = 500f;
	public float rangeRatio = 1f;
	//
	[Header("Ammo Settings")]
	public int ammoCapacity = 100;
	[SerializeField][SilantroShowAttribute]public int currentAmmo;
	public bool unlimitedAmmo;
	//
	[Header("Point Attachments")]
	public List<Transform> muzzles = new List<Transform>();
	public Transform shellEjectPoint;
	//
	[Header("Effects")]
	public GameObject muzzleFlash;
	public GameObject bulletCase;
	private float shellSpitForce = 1.5f;					
	private float shellForceRandom = 1.5f;
	private float shellSpitTorqueX = 0.5f;
	private float shellSpitTorqueY = 0.5f;
	private float shellTorqueRandom = 1.0f;
	//
	[Header("Impact Effects")]
	public GameObject groundHit;
	public GameObject metalHit;
	public GameObject woodHit;
	//
	private int muzzle = 0;
	private Transform currentMuzzle;
	//
	Vector3 planeVelocity;
	//
	[Header("Sounds")]
	public AudioClip fireSound;
	//public AudioClip dryFireSound;
	//
	bool canFire = true;
	//
	// Use this for initialization
	void Start () {
		//
		if (rateOfFire != 0) {
			actualRate = 1.0f / rateOfFire;
		} else {
			actualRate = 0.01f;
		}
		fireTimer = 0.0f;
		//
		currentMuzzle = muzzles [muzzle];
		//Add Audio Source
		if (GetComponent<AudioSource> () == null) {
			gameObject.AddComponent (typeof(AudioSource));
		}
		currentAmmo = ammoCapacity;
	}
	
	// Update is called once per frame
	void Update () {
		//
		currentAccuracy = Mathf.Lerp(currentAccuracy, accuracy, accuracyRecover * Time.deltaTime);
		// Update the fireTimer
		fireTimer += Time.deltaTime;
		//
		//actualFireTimer = fireTimer;
		//
		if (currentAmmo < 0)
		{ 
			currentAmmo = 0;
		}
		if (currentAmmo == 0) {
			canFire = false;
		}
		//
		if (Input.GetKey (KeyCode.LeftControl) && (fireTimer >= actualRate) && canFire) {
			Fire ();
		}
	
	}

	void Fire()
	{
		muzzle += 1;
		if (muzzle > (muzzles.Count -1)) {
			muzzle = 0;
		}
		currentMuzzle = muzzles [muzzle];
		//
		//Reset
		fireTimer = 0.0f;
		//
		if (!unlimitedAmmo) {
			currentAmmo--;
		}
		//
		//Actually Shoot weapon
		Vector3 direction = currentMuzzle.forward;
		//
		Ray rayout = new Ray (currentMuzzle.position, direction);
		RaycastHit hitout;
		//
		if (Physics.Raycast (rayout, out hitout, range / rangeRatio)) {
			//Debug.Log (hitout.distance);
			acc = 1 - ((hitout.distance) / (range / rangeRatio));
		}
		//Calculate accuracy for current shot
		float accuracyVary = (100 - currentAccuracy) / 1000;

		direction.x += UnityEngine.Random.Range (-accuracyVary, accuracyVary);
		direction.y += UnityEngine.Random.Range (-accuracyVary, accuracyVary);
		direction.z += UnityEngine.Random.Range (-accuracyVary, accuracyVary);
		currentAccuracy -= accuracyDrop;
		if (currentAccuracy <= 0.0f)
			currentAccuracy = 0.0f;
		//
		Ray ray = new Ray (currentMuzzle.position, direction);
		RaycastHit hit;
		//
		//IMPACT ON HIT
		if (Physics.Raycast (ray, out hit, range / rangeRatio)) {
			// Warmup heat
			float damageeffect = damage * acc;//
			//
			if (hit.collider.tag == "Ground") {
				Instantiate(groundHit, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
				//Instantiate(groundHole, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
			}

		}
		///
		//

		if (transform.root.GetComponent<Rigidbody> ()) {
			planeVelocity = transform.root.GetComponent<Rigidbody> ().velocity;
		}
		if (muzzleFlash != null) {
			GameObject flash = Instantiate (muzzleFlash, currentMuzzle.position, currentMuzzle.rotation);
			flash.transform.position = currentMuzzle.position;
			flash.transform.parent = currentMuzzle.transform;
		}
		//
		GameObject shellGO = Instantiate(bulletCase, shellEjectPoint.position, shellEjectPoint.rotation) as GameObject;
		shellGO.GetComponent<Rigidbody> ().velocity = planeVelocity;
		shellGO.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(shellSpitForce + UnityEngine.Random.Range(0, shellForceRandom), 0, 0), ForceMode.Impulse);
		shellGO.GetComponent<Rigidbody>().AddRelativeTorque(new Vector3(shellSpitTorqueX + UnityEngine.Random.Range(-shellTorqueRandom, shellTorqueRandom), shellSpitTorqueY + UnityEngine.Random.Range(-shellTorqueRandom, shellTorqueRandom), 0), ForceMode.Impulse);
		//

		//
		GetComponent<AudioSource>().PlayOneShot(fireSound);
	}


}
