//
//Property of Oyedoyin Dada
//cc dadaoyedoyin@gmail.com
//
//
//using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SilantroEngineEffect : MonoBehaviour {
	[Header("Engine Connection")]
	public GameObject engine;
	public enum EngineType
	{
		TurboJet,
		TurboFan
	}
	public EngineType engineType = EngineType.TurboFan;
	SilantroTurboFan turbofan;
	SilantroTurboJet turbojet;
	//
	[Header("Exhaust Emission Settings")]
	public Material engineMaterial;
	//
	Color baseColor;
	Color finalColor;
	//
	[Header("Extremes")]
	public float maximumNormalEmission;
	public float maximumAfterburnerEmission;
	float value;
	//
	[Header("")]
	[Header("Exhaust Smoke Settings")]
	public ParticleSystem exhaustSmoke;
	ParticleSystem.EmissionModule smokeModule;
	//
	[Header("Extremes")]
	public float maximumEmissionValue = 50f;
	//
	[Range(0f,1f)]
	public float controlValue;
	//
	[Header("Afterburner State")]
	public bool Afterburner;
	// Use this for initialization
	void Start () {
		baseColor = Color.white;
		//
		if (exhaustSmoke != null) {
			smokeModule = exhaustSmoke.emission;
		}
		if (engine != null) {
			if (engineType == EngineType.TurboFan) {
				turbofan = engine.GetComponent<SilantroTurboFan> ();
			} else {
				turbojet = engine.GetComponent<SilantroTurboJet> ();
			}
		}
	}
	
	// Update is called once per frame
	void Update () {

		if (exhaustSmoke) {
			//Recieve Afterburner switch from engine
			if (turbofan) {
				Afterburner = turbofan.AfterburnerOperative;
				//
				//Recieve Control from Engine
				if (turbofan.EngineOn == true) {
					controlValue = turbofan.enginePower * (turbofan.FuelInput);
				} else {
					controlValue = Mathf.Lerp (controlValue, 0f, 0.04f);
				}
				//
				if (Afterburner) {
					value = maximumAfterburnerEmission;
					smokeModule.rate = 1.5f * maximumEmissionValue * turbofan.enginePower * turbofan.FuelInput;

				} else {
					value = Mathf.Lerp (value, maximumNormalEmission, 0.02f);
					smokeModule.rate = maximumEmissionValue * turbofan.enginePower * turbofan.FuelInput;
				}

			} else if (turbojet) {
				Afterburner = turbojet.AfterburnerOperative;
				//
				//Recieve Control from Engine
				if (turbojet.EngineOn == true) {
					controlValue = turbojet.enginePower * (turbojet.FuelInput);
				} else {
					controlValue = Mathf.Lerp (controlValue, 0f, 0.04f);
				}
				//
				if (Afterburner) {
					value = maximumAfterburnerEmission;
					smokeModule.rate = 1.5f * maximumEmissionValue * turbojet.enginePower * turbojet.FuelInput;

				} else {
					value = Mathf.Lerp (value, maximumNormalEmission, 0.02f);
					smokeModule.rate = maximumEmissionValue * turbojet.enginePower * turbojet.FuelInput;
				}

			}
		}

		float actualValue = (controlValue) * value;
		//
		if (engineMaterial != null) {
			finalColor = baseColor * Mathf.LinearToGammaSpace (actualValue);
			engineMaterial.SetColor ("_EmissionColor", finalColor);
		}
		//
		//SMOKE SETTINGS
		}
}
