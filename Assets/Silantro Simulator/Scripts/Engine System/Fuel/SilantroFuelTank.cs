//
//Property of Oyedoyin Dada
//cc dadaoyedoyin@gmail.com
//
//
//using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SilantroFuelTank : MonoBehaviour {
	//
	public enum TankType
	{
		Internal,
		External
	}
	public TankType tankType = TankType.Internal;

	[Header("Tank Fuel Capacity (kg)")]
	public float Capacity;
	[SerializeField][WeightOnly]public float CurrentAmount;
	public bool attached = true;
	//

	[Header("Health Values")]
	//Health Values
	public float startingHealth = 100.0f;		// The amount of health to start with
	[SerializeField][SilantroShowAttribute]public float currentHealth;
	//
	private bool destroyed;
	[Header("Effects")]
	public GameObject tankGameobject;
	public GameObject ExplosionPrefab;
	SilantroFuelDistributor attachedDitributor;
	//
	void Start () {
		currentHealth = startingHealth;CurrentAmount = Capacity;
		//
		if (ExplosionPrefab != null)
			ExplosionPrefab.SetActive (false);
	}
	//
	public void Detach()
	{
		CurrentAmount = 0;
		if (attachedDitributor != null) {
			if (this.GetComponent<SilantroFuelTank>().tankType == TankType.External && attachedDitributor.externalTanks.Contains (this.GetComponent<SilantroFuelTank> ())) {
				attachedDitributor.externalTanks.Remove (this.GetComponent<SilantroFuelTank> ());
			}
		}
		attached = false;if(tankGameobject){tankGameobject.AddComponent<CapsuleCollider>();tankGameobject.AddComponent<Rigidbody>().mass = Capacity;}tankGameobject.transform.parent = null;
	//Remove fuel from total amount
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
		//Die Procedure
		if (currentHealth == 0 && !destroyed)
			CurrentAmount = 0;Disintegrate();
	}
	//
	//DESTRUCTION SYSTEM
	public void Disintegrate()
	{
		destroyed = true;
		//ACTIVATE EXPLOSION AND FIRE
		if (ExplosionPrefab != null)
			Instantiate (ExplosionPrefab, transform.position, Quaternion.identity);
		ExplosionPrefab.GetComponentInChildren<AudioSource> ().Play ();
		//
		//DESTROY GAMEOBJECT
		Destroy(gameObject);
	}
	//
	//DAMAGE
	void OnCollisionEnter(Collision col)
	{
		if (col.relativeVelocity.magnitude > 50f) {
			Disintegrate ();
		}
	}
	void Update()
	{
		if (CurrentAmount < 0f)
		{
			CurrentAmount = 0f;
		}
	}
}
