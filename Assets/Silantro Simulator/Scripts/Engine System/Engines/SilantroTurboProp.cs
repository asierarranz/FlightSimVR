//
//Property of Oyedoyin Dada
//cc dadaoyedoyin@gmail.com
//
//
//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
//
//[AddComponentMenu("Oyedoyin/Engine System/Turbo Prop")]
public class SilantroTurboProp : MonoBehaviour {

	public enum EngineState
	{
		Off,
		Starting,
		Running
	}
	[Header("Properties (HP,m,86%)")]
	public float ShaftPower = 1000f;
	public float PropellerDiameter = 1f;
	public float PropellerEfficiency = 70f;
	[SerializeField] [EfficiencyOnly]public float currentEfficiency;
	//
	//
	[Header(" ")]//SPACE
	[Header("Engine Configuration")]
	public bool EngineOn;
	public float engineAcceleration = 0.2f;
	[SerializeField] [EfficiencyOnly]float EnginePower;
	float enginePower;
	//
	[SerializeField][TemperatureOnly]public float EGT;
	float engineInverseEfficiency;
	[NonSerialized]
	public bool isAccelerating;
	//

	//
	[Header("RPM Settings")]
	public float EngineIdleRPM = 100f;
	public float EngineMaximumRPM = 1000f;
	public float RPMAcceleration = 0.5f;
	[SerializeField][RPMOnly]public float EngineRPM;
	//
	[Header(" ")]//SPACE
	[Header("Fuel Type and Combustion System")]
	[SerializeField][SilantroShowAttribute]float massFactor;
	public enum FuelType
	{
		JetB,
		JetA1,
		JP6,
		JP8
	}
	public FuelType fuelType = FuelType.JetB;
	[SerializeField][SilantroShowAttribute]float combustionEnergy;
	//

	[Header("Fuel Configuration")]
	public SilantroFuelTank attachedFuelTank;
	//public float StartAmount;
	[Header("Fuel Consumption (Kg/s)")]
	public float FuelConsumption = 1.5f;
	[SerializeField][WeightOnly]public float currentTankFuel;
	public float criticalFuelLevel = 10f;
	[SerializeField][MassFlowOnly]public float actualConsumptionrate;
	 bool InUse;

	public bool LowFuel;
	//
	[HideInInspector]public bool captured;


	//
	[HideInInspector]
	public float DesiredRPM;

	[HideInInspector]
	public float CurrentRPM;
	//
	[Header(" ")]//SPACE
	[Header("Engine Sounds")]
	public AudioClip EngineStartSound;
	public AudioClip EngineIdleSound;
	public AudioClip EngineShutdownSound;


	[HideInInspector]public float EngineIdlePitch = 0.5f;
	[HideInInspector]public float EngineMaximumRPMPitch = 1f;
	[HideInInspector]public float maximumPitch = 2f;
	[HideInInspector]public float engineSoundVolume = 2f;
	//
	[Header(" ")]//SPACE
	[Header("Connections")]
	public Rigidbody Parent;
	public Transform Propeller;
	[Header("Use Fast Propeller")]
	public bool useFastPropeller = true;
	public Transform fastPropeller;
	[Header("Propeller Rotation Axis")]

	public bool X;
	public bool Y;
	public bool Z;
	//
	public enum RotationDirection
	{
		CW,
		CCW
	}
	public RotationDirection rotationDirection = RotationDirection.CCW;
	//

	public Transform Thruster;
	//
	private AudioSource EngineStart;
	private AudioSource EngineRun;
	private AudioSource EngineShutdown;

	//
	[Header(" ")]//SPACE
	[Header("Control")]
	[HideInInspector]
	public bool start;
	[HideInInspector]
	public bool stop;
	private bool starting;
	private float velocityMe;
	private bool lowFuel;
	//
	[Header(" ")]//SPACE
	[Header("Throttle Control")]
	[Range(0.2f, 1f)]
	public float FuelInput = 0.2f;
	public float throttleSpeed = 0.15f;
	//

	KeyCode startEngine;
	KeyCode stopEngine;
	KeyCode throttleUp;
	KeyCode throttleDown;
	//
	[Header(" ")]//SPACE
	[Header("Engine Display")]
	public EngineState CurrentEngineState;
	//
	[SerializeField][DensityOnly]public float airDensity = 1.225f;

	[SerializeField][ForceOnly]public float PropellerThrust;
	float EngineLinearSpeed;
	// float maxPossibleThrust;
	//
	SilantroInstrumentation instrumentation;
	//
	float fuelMassFlow;
	bool fuelAlertActivated;
	float fuelFactor = 1f;
	float combusionFactor;
	//
	SilantroControls controlBoard;
	void Awake () {
		controlBoard = GameObject.FindGameObjectWithTag ("GameController").GetComponent<SilantroControls> ();
		if (controlBoard == null) {
			Debug.LogError ("Control Board is missing, Place Control Board in scene and restart!");
		}
		//
		startEngine= controlBoard.engineStart;
		stopEngine = controlBoard.engineShutdown;
		throttleUp = controlBoard.engineThrottleUp;
		throttleDown = controlBoard.engineThrottleDown;
		//
		GameObject brain =GameObject.FindGameObjectWithTag ("Brain");
		if (brain != null && brain.transform.root == gameObject.transform.root) {
			instrumentation = brain.GetComponent<SilantroInstrumentation> ();
		}

		if (instrumentation == null) {
			//instrumentation.Store ();
			Debug.LogError ("Instrumentation is missing!! If Engine is just for Test, add Instrumentation Prefab to the Scene");
		} 
	}
	//
	// Use this for initialization
	void Start () {
		//
		if (Parent == null) {
			Debug.Log ("Engine cannot be Operated without a Rigidbody parent!!, add Rigidbody to an empty gameobject if engine is for Test");
		}
		engineInverseEfficiency = UnityEngine.Random.Range(38f,44f);
		massFactor = UnityEngine.Random.Range(1.6f,2.2f);
		//
		//SET UP ENGINE FUEL COMBUSTION VALUES
		if (fuelType == FuelType.JetB)
		{
			combustionEnergy = 42.8f;
		}
		else if (fuelType == FuelType.JetA1) 
		{
			combustionEnergy = 43.5f;
		}
		else if (fuelType == FuelType.JP6) 
		{
			combustionEnergy = 49.6f;
		} 
		else if (fuelType == FuelType.JP8) 
		{
			combustionEnergy = 43.28f;
		}
		//
		combusionFactor = combustionEnergy/42f;
		//
		if (null != EngineStartSound)
		{
			EngineStart = Thruster.gameObject.AddComponent<AudioSource>();
			EngineStart.clip = EngineStartSound;
			EngineStart.loop = false;
			EngineStart.dopplerLevel = 0f;
			EngineStart.spatialBlend = 1f;
			EngineStart.rolloffMode = AudioRolloffMode.Custom;
			EngineStart.maxDistance = 650f;
		}
		if (null != EngineIdleSound)
		{
			EngineRun = Thruster.gameObject.AddComponent<AudioSource>();
			EngineRun.clip = EngineIdleSound;
			EngineRun.loop = true;
			EngineRun.Play();
			engineSoundVolume = EngineRun.volume * 2f;
			EngineRun.spatialBlend = 1f;
			EngineRun.dopplerLevel = 0f;
			EngineRun.rolloffMode = AudioRolloffMode.Custom;
			EngineRun.maxDistance = 600f;
		}
		if (null != EngineShutdownSound)
		{
			EngineShutdown = Thruster.gameObject.AddComponent<AudioSource>();
			EngineShutdown.clip = EngineShutdownSound;
			EngineShutdown.loop = false;
			EngineShutdown.dopplerLevel = 0f;
			EngineShutdown.spatialBlend = 1f;
			EngineShutdown.rolloffMode = AudioRolloffMode.Custom;
			EngineShutdown.maxDistance = 650f;
		}

		EngineOn = false;
		start = false;
		starting = false;
		stop = false;
	}

	// Update is called once per frame
	void Update () {
		//

		if (attachedFuelTank != null) {
			currentTankFuel = attachedFuelTank.CurrentAmount;
		}
		//
		//
		if (Input.GetKeyDown (startEngine) &&attachedFuelTank != null&& attachedFuelTank.CurrentAmount > 0) {
			start = true;
		}
		if (Input.GetKeyDown (stopEngine)) {
			stop = true;
		}
		if(Input.GetKey(throttleUp) && CurrentEngineState == EngineState.Running)
		{
			FuelInput = Mathf.Lerp (FuelInput, 1.0f, throttleSpeed);
		}
		if(Input.GetKey(throttleDown))
		{
			FuelInput = Mathf.Lerp (FuelInput, 0.0f, throttleSpeed);
		}
		//
		EngineActive ();
		if (enginePower > 0f) {
			EngineCalculation ();
		}
		//
		if (InUse && EngineOn) {
			UseFuel ();
		}
		//
		//performanceConfiguration.EngineLinearSpeed = num * 1.94384444f;
		if (Parent) {
			switch (CurrentEngineState) {
			case EngineState.Off:
				UpdateOff ();
				break;
			case EngineState.Starting:
				UpdateStarting ();
				break;
			case EngineState.Running:
				UpdateRunning ();
				break;
			}

		}
		//INTERPOLATE ENGINE RPM
		if (EngineOn) {
			CurrentRPM =   Mathf.Lerp (CurrentRPM, DesiredRPM,RPMAcceleration * Time.deltaTime * (enginePower* fuelFactor* fuelFactor));
		} else {
			CurrentRPM =  Mathf.Lerp (CurrentRPM, 0.0f, RPMAcceleration * Time.deltaTime);
		}
		//

		if (null != EngineRun)
		{
			float magnitude = Parent.velocity.magnitude;
			float num2 = magnitude * 1.94384444f;
			float num3 = CurrentRPM + num2 * 10f;
			float num4 = (num3 - EngineIdleRPM) / (EngineMaximumRPM - EngineIdleRPM);
			float num5 = EngineIdlePitch + (EngineMaximumRPMPitch - EngineIdlePitch) * num4;
			num5 = Mathf.Clamp(num5, 0f, maximumPitch);
			//
			if (attachedFuelTank != null && attachedFuelTank.CurrentAmount <= 0)
			{
				stop = true;
			}
			//
			if (attachedFuelTank != null && attachedFuelTank.CurrentAmount <= criticalFuelLevel ) {
				if (EngineOn) {
					float startRange = 0.6f;
					float endRange = 1.0f;
					//
					float cycleRange = (endRange - startRange) / 2f;
					float offset = cycleRange + startRange;
					//
					fuelFactor = offset + Mathf.Sin (Time.time * 3f) * cycleRange;
					//
					EngineRun.pitch = fuelFactor;
				}
			}
			//
			else
			{
				EngineRun.pitch = num5 * enginePower;
			}
			//
			//
			EngineRun.volume = engineSoundVolume;
			if (CurrentRPM < EngineIdleRPM)
			{
				EngineRun.volume = engineSoundVolume * num5;
				if (CurrentRPM < EngineIdleRPM * 0.1f)
				{
					EngineRun.volume = 0f;
				}
			}
			else
			{
				EngineRun.volume = engineSoundVolume * num5;
			}

		}
		//

	}
	//
	//
	public IEnumerator ReturnIgnition()
	{
		yield return new WaitForSeconds (0.5f);
		start = false;
		stop = false;
	}
	//
	private void UpdateOff()
	{
		if (EngineStart.isPlaying)
		{
			EngineStart.Stop();
			start = false;
		}
		if (start && attachedFuelTank!= null && attachedFuelTank.CurrentAmount >0)
		{
			EngineOn = true;
			EngineStart.Play();
			CurrentEngineState = EngineState.Starting;
			starting = true;
			StartCoroutine(ReturnIgnition());
		}
		DesiredRPM = 0f;
	}
	//
	private void UpdateStarting()
	{
		if (starting)
		{
			if (!EngineStart.isPlaying)
			{
				CurrentEngineState = EngineState.Running;
				starting = false;
				UpdateRunning();
			}
		}
		else
		{
			EngineStart.Stop();
			CurrentEngineState = EngineState.Off;
		}
		DesiredRPM = EngineIdleRPM;
	}
	//
	//
	private void UpdateRunning()
	{
		if (EngineStart.isPlaying)
		{
			EngineStart.Stop();
		}
		FuelInput = Mathf.Clamp(FuelInput, 0f, 1f);
		DesiredRPM = EngineIdleRPM + (EngineMaximumRPM - EngineIdleRPM) * FuelInput;
		InUse = true;

		if (stop)
		{
			CurrentEngineState = EngineState.Off;
			EngineOn = false;
			//Stop Fuel Alert
			EngineShutdown.Play();PropellerThrust=0;
			FuelInput = 0f;
			StartCoroutine(ReturnIgnition());
		}
	}
	//
	//
	private void EngineActive()
	{
		if (EngineOn)
		{
			if (enginePower < 1f && !isAccelerating)
			{
				enginePower += Time.deltaTime * engineAcceleration;

			}
		}
		else if (enginePower > 0f)
		{
			enginePower -= Time.deltaTime * engineAcceleration;
		
		}
		else
		{
			enginePower = 0f;
			EGT = 0f;
		}
		//
		EnginePower = enginePower * 100f;
	}
	///
	public void DestroyEngine()
	{
		
		EngineOn = false;
		PropellerThrust = 0f;
	}
	//
	public void UseFuel()
	{
		{
			actualConsumptionrate = combusionFactor*FuelConsumption * (FuelInput +0.1f)* EngineRun.pitch ;
			//
			if (attachedFuelTank != null) {
				attachedFuelTank.CurrentAmount -= actualConsumptionrate * Time.deltaTime;
			}
		}

		if (attachedFuelTank != null && attachedFuelTank.CurrentAmount == 0f)
		{
			EngineRun.volume = 0f;
			EngineRun.pitch = 0f;
			stop = true;
			PropellerThrust = 0f;
		}
	


	}
	//
	//

	//
	public void EngineCalculation()
	{
		if (instrumentation != null) {
			airDensity=instrumentation.airDensity;
		}

		//CALCULATE EGT// TO BE USED BY INFRARED RADAR SYSTEM
		EGT  = ((combustionEnergy*actualConsumptionrate * (engineInverseEfficiency/100f) *100000f) + (massFactor * 500f * 32f))/(massFactor * 500f);

		float velocity = Parent.velocity.magnitude;
		EngineLinearSpeed = velocity;
		//
		//
		//Calculate Current Propeller Efficiency
		//EngineThrust 
		float dynamicShaftPower = Mathf.Pow((ShaftPower * 550f),2/3f);
		//Calculate Propeller Area
		float PropArea = (3.142f * Mathf.Pow((3.28084f *PropellerDiameter),2f))/4f;
		//
		if (instrumentation != null) {
			airDensity = instrumentation.airDensity;
		}
		//
		float dynamicArea = Mathf.Pow((2f * airDensity * 0.0624f * PropArea),1/3f);
		//
		//\
		///Calculate Propeller efficiency at current Speed
		float a = -0.0042f * Mathf.Pow((EngineLinearSpeed ),2f);
		float b = 0.3429f * (EngineLinearSpeed );
		//
		currentEfficiency = a+b+PropellerEfficiency;
		currentEfficiency = Mathf.Clamp (currentEfficiency, 30f, 90f);
		//Calculate Thrust
		PropellerThrust = combusionFactor * FuelInput * fuelFactor* dynamicShaftPower * dynamicArea * currentEfficiency/100f;
	}
	//
	public void FixedUpdate()
	{
		if (Parent)
		{
			//FAST PROPELLER SETTINGS
			if (CurrentRPM > ((1f / 4f) * EngineMaximumRPM)) {
				if (useFastPropeller && fastPropeller != null) {
					Propeller.gameObject.GetComponent<Renderer> ().enabled = false;
					fastPropeller.gameObject.GetComponent<Renderer> ().enabled = true;
				}
			} else {
				Propeller.gameObject.GetComponent<Renderer> ().enabled = true;
				if (fastPropeller != null) { 
					fastPropeller.gameObject.GetComponent<Renderer> ().enabled = false;
				}
			}
			//
			EngineRPM = CurrentRPM;
			if (Propeller)
			{
				if (rotationDirection == RotationDirection.CCW) {
					if (X) {
						Propeller.Rotate (new Vector3 (CurrentRPM * Time.deltaTime, 0, 0));
					}
					if (Y) {
						Propeller.Rotate (new Vector3 (0, CurrentRPM * Time.deltaTime, 0));
					}
					if (Z) {
						Propeller.Rotate (new Vector3 (0, 0, CurrentRPM * Time.deltaTime));
					}
				}
				//
				if (rotationDirection == RotationDirection.CW) {
					if (X) {
						Propeller.Rotate (new Vector3 (-1f *CurrentRPM * Time.deltaTime, 0, 0));
					}
					if (Y) {
						Propeller.Rotate (new Vector3 (0, -1f *CurrentRPM * Time.deltaTime, 0));
					}
					if (Z) {
						Propeller.Rotate (new Vector3 (0, 0, -1f *CurrentRPM * Time.deltaTime));
					}
				}
			}
			if (PropellerThrust > 0f)
			{
				Vector3 force = Thruster.forward * PropellerThrust;
				Parent.AddForceAtPosition(force, Thruster.position, ForceMode.Force);
			}
		}
		if (CurrentRPM <= 0f)
		{
			CurrentRPM = 0f;
		}
	}
	//
	//

}

