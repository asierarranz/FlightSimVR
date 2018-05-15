//
//Property of Oyedoyin Dada
//cc dadaoyedoyin@gmail.com
//
//
//using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[AddComponentMenu("Oyedoyin/Utilties/Instrumentation")]
public class SilantroInstrumentation : MonoBehaviour {

	[Header("Connection")]
	public Rigidbody airplane;
	//
	public enum AircraftType
	{
		Jet,
		Propeller
	}
	public AircraftType aircraftType;
	//
	[Header("Ambient Data")]
	[SerializeField][KnotSpeedOnly]public float currentSpeed;
	[SerializeField][SilantroShowAttribute]public float machSpeed;
	[SerializeField][AltitudeOnly]public float currentAltitude;
	[SerializeField][AngleOnly]public float headingDirection;
	[SerializeField][FeetSpeedOnly] public float verticalSpeed;
	[SerializeField][DensityOnly]public float airDensity;
	[SerializeField][TemperatureOnly]public float ambientTemperature;
	[SerializeField][PressureOnly]public float ambientPressure;
	// Use this for initialization
	[Header("Sonic boom Effect")]
	public ParticleSystem condenationEffect;
	public AudioClip sonicBoom;
	[HideInInspector]public AudioSource boom;
	ParticleSystem.EmissionModule condensation;
	bool sonicing;
	bool played = false;
	// Update is called once per frame

	//
	public void Store()
	{
		GameObject soundPoint = new GameObject();
		soundPoint.transform.parent = this.transform;
		soundPoint.transform.localPosition = new Vector3 (0, 0, 0);
		soundPoint.name = this.name +" Sound Point";

		boom = soundPoint.AddComponent<AudioSource> ();
		boom.priority = 0;
		boom.spatialBlend = 1f;
		boom.dopplerLevel = 0f;
		boom.rolloffMode = AudioRolloffMode.Custom;
		boom.maxDistance = 700f;
		boom.volume = 1.5f;
	}
	//

	void Start()
	{
		if (aircraftType == AircraftType.Jet && condenationEffect !=null) {
			condenationEffect.Stop ();
		}

	}
	//
	void FixedUpdate () {
		if (airplane != null) {
			currentAltitude = airplane.gameObject.transform.position.y * 3.28f;
		}
		//
		CalculateDensity (currentAltitude);
		CalculateData (currentAltitude);
		//
		//Calculate Speed
		currentSpeed = airplane.velocity.magnitude * 1.944f;
		//
		verticalSpeed = airplane.velocity.y * 3.28f * 60f;
	}
	//
	void CalculateDensity(float altitude)
	{
		float altiKmeter = altitude / 3280.84f;
		//
		float a =  0.0025f * Mathf.Pow(altiKmeter,2f);
		float b = 0.106f * altiKmeter;
		//
		airDensity = a -b +1.2147f;
		//
	}
	//
	void CalculateData(float altitude)
	{
		//Calculate Temperature
		float a1 = 0.000000003f * altitude * altitude;
		float a2 = 0.0021f * altitude;
		ambientTemperature = a1-a2+15.443f;//
		//Calculate Pressure
		float a = 0.0000004f * altitude * altitude;
		float b = (0.0351f*altitude);
		ambientPressure =( a - b + 1009.6f)/10f;
		//
		headingDirection = airplane.transform.eulerAngles.y;
		//
		float soundSpeed = Mathf.Pow((1.4f*287f*(273.15f+ambientTemperature)),0.5f);
		machSpeed = (currentSpeed / 1.944f) / soundSpeed;
		//
		if (aircraftType == AircraftType.Jet) {
			if (machSpeed >= 0.98f && !played) {
				Boom ();
			}
			//
			if (machSpeed < 0.98f && played) {
				played = false;
			}
		}

	}
	//
	void Boom()
	{
		boom.PlayOneShot (sonicBoom);
		condenationEffect.Emit (250);
		played = true;
	}
	//

}
