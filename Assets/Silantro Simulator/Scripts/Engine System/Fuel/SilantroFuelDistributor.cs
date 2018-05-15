//
//Property of Oyedoyin Dada
//cc dadaoyedoyin@gmail.com
//
//
//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SilantroFuelDistributor : MonoBehaviour {

	[Header("Fuel Tanks")]
	public SilantroFuelTank internalFuelTank;//[Header("")]
	public List<SilantroFuelTank> externalTanks = new List<SilantroFuelTank> ();
	[SerializeField][WeightOnly]public float TotalFuelRemaining;
	//
	[Header("")]
	[SerializeField][NameOnly]public string currentTank;
	[HideInInspector]
	public SilantroFuelTank CurrentTank;
	[SerializeField][NameOnly]public string tankType;
	[SerializeField][WeightOnly]public float currentTankFuel;
	[SerializeField][NameOnly]string timeLeft;
	[HideInInspector]public float totalConsumptionRate =1f;
	//
	[Header("")]//SPACE
	[Header("Fuel Dump System")]
	public bool dumpFuel = false;
	[Header("Fuel Dump Rate kg/s")]
	public float fuelDumpRate = 1f;
	[SerializeField][MassFlowOnly]public float actualFlowRate;
	//
	[Header("Tank Refill System")]
	public bool refillTank = false;
	[Header("Fuel Refuel Rate kg/s")]
	public float refuelRate = 1f;
	[SerializeField][MassFlowOnly]public float actualrefuelRate;

	//
	[Header("")]//SPACE
	[Header("Warning System")]
	public bool lowFuel;bool fuelAlertActivated;
	public float minimumFuelAmount = 50f;
	public AudioClip fuelAlert;
	AudioSource FuelAlert;SilantroControls controlBoard;KeyCode stopEngine ;KeyCode dumpFuelKey;KeyCode refuelTankKey;
	// Use this for initialization
	//
	void Awake () {
		//
		controlBoard = GameObject.FindGameObjectWithTag ("GameController").GetComponent<SilantroControls> ();
		if (controlBoard == null) {
			Debug.LogError ("Control Board is missing, Place Control Board in scene and restart!");
		}
		stopEngine = controlBoard.engineShutdown;dumpFuelKey = controlBoard.dumpFuelControl;refuelTankKey = controlBoard.refuelControl;
		//FUEL SETUP
		if (internalFuelTank != null) {
			TotalFuelRemaining = 0;
			if (externalTanks.Count > 0) {
				foreach (SilantroFuelTank external in externalTanks) {
					TotalFuelRemaining += external.Capacity;
				}
			}
			TotalFuelRemaining += internalFuelTank.Capacity;
			CurrentTank = internalFuelTank;
			//
		} else {
			Debug.Log ("No internal fuel tank is assigned to distributor!!");
		}
		//
	}
	//
	void Start () {
		
		GameObject soundPoint = new GameObject("Warning Horn");
		soundPoint.transform.parent = this.transform;
		//
		if (null != fuelAlert) {
			FuelAlert = soundPoint.gameObject.AddComponent<AudioSource> ();
			FuelAlert.clip = fuelAlert;
			FuelAlert.loop = true;
			FuelAlert.volume = 1f;
			FuelAlert.dopplerLevel = 0f;
			FuelAlert.spatialBlend = 1f;
			FuelAlert.rolloffMode = AudioRolloffMode.Custom;
			FuelAlert.maxDistance = 650f;
		}

	}
	
	// Update is called once per frame
	void FixedUpdate () {
		//
		if (internalFuelTank.CurrentAmount < minimumFuelAmount) {
			if (externalTanks.Count > 0) {
				CurrentTank = externalTanks [0];
				if (CurrentTank.tankType == SilantroFuelTank.TankType.External && CurrentTank.CurrentAmount<0.1f) {
					if (externalTanks.Contains (CurrentTank)) {
						externalTanks.Remove (CurrentTank);
						CurrentTank = externalTanks [0];
					}
				}
			} 
			else {
				CurrentTank = internalFuelTank;
				//Activate fuel Alert
				if (CurrentTank.CurrentAmount <= minimumFuelAmount)
				{
					lowFuel = true;
					if (!fuelAlertActivated) {
						//
						StartCoroutine(LowFuelAction());
						fuelAlertActivated = true;
					}
				}
				Debug.Log("Fuel Low");
			}
		}
		//
		if (CurrentTank != null) {
			currentTank = CurrentTank.gameObject.name;//
			currentTankFuel = CurrentTank.CurrentAmount;
			tankType = CurrentTank.tankType.ToString ();
			if (totalConsumptionRate > 0) {
				float flightTime = (CurrentTank.CurrentAmount) / totalConsumptionRate;
				timeLeft = string.Format ("{0}:{1:00}", (int)flightTime / 60, (int)flightTime % 60);
			}
		}
		TotalFuelRemaining = 0;
		if (externalTanks.Count > 0) {
			foreach (SilantroFuelTank external in externalTanks) {
				TotalFuelRemaining += external.Capacity;
			}
		}
		TotalFuelRemaining += internalFuelTank.CurrentAmount;
	}
	//
	void Update()
	{
		if (Input.GetKeyDown (stopEngine) && fuelAlertActivated) {
			FuelAlert.Stop();lowFuel = false;
			//fuelAlertActivated = false;
		}
		if (CurrentTank.CurrentAmount <= 0) {
			CurrentTank.CurrentAmount = 0;
		}
		if (Input.GetKeyDown (dumpFuelKey)) {
			if (!refillTank) {
				dumpFuel = !dumpFuel;
			}
		}if (Input.GetKeyDown (refuelTankKey)) {
			if (!dumpFuel) {
				refillTank = !refillTank;
			}
		}
		if (dumpFuel) {DumpFuel ();}
		if (refillTank) {RefuelTank ();}
	}
	////REFUEL TANKS
	 void RefuelTank()
	{
		
		actualrefuelRate = refuelRate * Time.deltaTime;
		if (internalFuelTank != null) {
			internalFuelTank.CurrentAmount += actualrefuelRate;
		}
		//CONTROL AMOUNT
		if (internalFuelTank.CurrentAmount > CurrentTank.Capacity) {
			internalFuelTank.CurrentAmount = CurrentTank.Capacity;
			refillTank = false;
		}
	}
	///
	////REFUEL TANKS
	 void DumpFuel()
	{
		actualFlowRate = fuelDumpRate * Time.deltaTime;
		if (CurrentTank != null) {
			CurrentTank.CurrentAmount -= actualFlowRate;
		}
		if (CurrentTank.CurrentAmount <= 0) {
			CurrentTank.CurrentAmount = 0;
			dumpFuel = false;
		}
	}
	///
	//ACTIVATE FUEL ALERT SOUND
	IEnumerator LowFuelAction()
	{
		yield return new WaitForSeconds (0.5f);
		FuelAlert.Play ();
	}
}
