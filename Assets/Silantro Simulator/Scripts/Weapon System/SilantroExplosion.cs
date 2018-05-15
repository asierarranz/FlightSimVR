//
//Property of Oyedoyin Dada
//cc dadaoyedoyin@gmail.com
//
//
//using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SilantroExplosion : MonoBehaviour {
	[Header("Explosion Properties")]
	public float damage = 200f;
	public float explosionForce = 4000f;
	public float explosionRadius = 45f;
	float fractionalDistance;
	// Use this for initialization
	//
	void Start()
	{
		gameObject.SetActive (true);
		Explode ();
	}
	//
	public void Explode()
	{
		Collider[] hitColliders = Physics.OverlapSphere(transform.position, explosionRadius);
		for (int i = 0; i < hitColliders.Length; i++)
		{
			Collider hit = hitColliders[i];
			if (!hit)
				continue;
				//
				//Calculate Distance to Object
			float distanceToObject = Vector3.Distance(transform.position,hit.gameObject.transform.position);
			fractionalDistance = (1 - (distanceToObject / explosionRadius));
			//
			//
			//
			Vector3 exploionPosition = transform.position;
			//If within Explosion Radius
				if(fractionalDistance <= explosionRadius) {
				//Apply force to Object directly
				hit.gameObject.SendMessageUpwards("SilantroDamage",(-damage * fractionalDistance),SendMessageOptions.DontRequireReceiver);
				if (hit.GetComponent<Rigidbody> ())
				{
					hit.GetComponent<Rigidbody> ().AddExplosionForce ((explosionForce * fractionalDistance), transform.position, explosionRadius, (3.0f ), ForceMode.Impulse);

				}
				else if(hit.transform.root.gameObject.GetComponent<Rigidbody>())
				{
					hit.transform.root.gameObject.GetComponent<Rigidbody> ().AddExplosionForce ((explosionForce * fractionalDistance), transform.position, explosionRadius, (3.0f ), ForceMode.Impulse);
				}
				}
		}
	}
}
