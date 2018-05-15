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
#if UNITY_EDITOR
using UnityEditor;
#endif

//[AddComponentMenu("Oyedoyin/Engine System/Turbo Fan")]
public class SilantroTurboFan : MonoBehaviour {
	
	public enum EngineState
	{
		Off,
		Starting,
		Running
	}
	[Header("Engine Dimensions")]
	public float EngineDiameter = 1f;
	[Range(0f,100f)]
	public float IntakeDiameterPercentage = 90f;
	[Range(0f,100f)]
	public float ExhaustDiameterPercentage = 90f;
	[SerializeField][LengthOnly]public float IntakeDiameter;
	float intakeDiameter;
	[SerializeField][LengthOnly]public float ExhaustDiameter;
	public float weight = 500f;
	public float overallLength = 4f;
	//
	float intakeFactor;
	//
	[Header(" ")]//SPACE
	[Header("Engine Specifications")]
	public float OverallPressureRatio =10f;
	public float bypassRatio = 1f;
	//
	public enum ReheatSystem
	{
		Afterburning,
		noReheat
	}
	public ReheatSystem reheatSystem = ReheatSystem.noReheat;

	[Header("Afterburner Control")]
	public bool AfterburnerOperative;
	[Header("Specific Fuel conumption lb/lbf/hr")]
	public float AfterburnerTSFC =2f;
	bool canUseAfterburner;
	//
	[Header(" ")]//SPACE
	[Header("Engine Configuration")]
	public bool EngineOn;
	public float engineAcceleration = 0.2f;
	//

	[NonSerialized]
	public bool isAccelerating;
	[SerializeField] [EfficiencyOnly]public float EnginePower;
	[SerializeField][TemperatureOnly]public float EGT;
	[HideInInspector]public float enginePower;
	//

	//
	[Header("RPM Settings")]
	public float LowPressureFanRPM = 100f;
	public float HighPressureFanRPM = 1000f;
	public float RPMAcceleration = 0.5f;
	[SerializeField][RPMOnly]public float LPRPM;
	[SerializeField][RPMOnly]public float HPRPM;
	//
	float LPIdleRPM;
	float HPIdleRPM;
	float currentHPRPM;
	float targetHPRPM;
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
	float sfc;
	//
	[Header("Fuel Configuration")]
	public SilantroFuelTank attachedFuelTank;
	//public float StartAmount;
	[Header("Specific Fuel conumption lb/lbf/hr")]
	public float TSFC = 0.1f;
	[SerializeField][WeightOnly]public float currentTankFuel;
	public float criticalFuelLevel = 10f;
	[SerializeField][MassFlowOnly]public float actualConsumptionrate;
	bool InUse;

	public bool LowFuel;
	//
	//

	[HideInInspector]
	public float TargetRPM;

	[HideInInspector]
	public float CurrentRPM;
	//
	[Header(" ")]//SPACE
	[Header("Engine Sounds")]
	public AudioClip EngineStartSound;
	public AudioClip EngineIdleSound;
	public AudioClip EngineShutdownSound;

	[Header("Sounds Settings")]
	public float EngineAfterburnerPitch = 1.75f;
	[HideInInspector]public float EngineIdlePitch = 0.5f;
	[HideInInspector]public float EngineMaximumRPMPitch = 1f;

	[HideInInspector]public float maximumPitch = 2f;
	[HideInInspector]public float engineSoundVolume = 2f;
	//

	[Header(" ")]//SPACE
	[Header("Connections")]
	public Rigidbody Parent;
	public Transform fan;

	//
	[Header("Fan Rotation Axis")]

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
	float num5;
	//
	public Transform Thruster;
	//
	private AudioSource EngineStart;
	private AudioSource EngineRun;
	private AudioSource EngineShutdown;

	//
	[HideInInspector]public bool captured;

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
	//Controls
	KeyCode startEngine ;
	KeyCode stopEngine ;
	KeyCode throttleUp ;
	KeyCode throttleDown;
	KeyCode afterburnerControl;
	//
	[Header(" ")]//SPACE
	[Header("Engine Display")]
	public EngineState CurrentEngineState;
	//
	[SerializeField][DensityOnly]public float airDensity = 1.225f;

	[SerializeField][ForceOnly]public float EngineThrust;
	 float EngineLinearSpeed;
	float maxPossibleThrust;
	//
	bool fuelAlertActivated;
	float fuelFactor = 1f;
	float combusionFactor;
	//
	//ParticleSystem.EmissionModule exhaustModule;//NEXT UPDATE
	//
	//CALCULATION VALUES
	[HideInInspector]public float fuelMassFlow;
	[HideInInspector]public float intakeArea;
	[HideInInspector]public float exhaustArea ;
	//
	[HideInInspector]public float intakeAirVelocity ;
	[HideInInspector]public float fanAirVelocity ;
	[HideInInspector]public float exhaustAirVelocity ;
	//
	[HideInInspector]public float intakeAirMassFlow ;
	//
	[HideInInspector]public float bypassfactor ;
	[HideInInspector]	public float coreAirMassFlow;
	[HideInInspector]public float fanAirMassFlow;
	//
	SilantroInstrumentation instrumentation;
	//
	float ambientPressure;
	//Thrust Values
	[HideInInspector]public float fanThrust ;
	[HideInInspector]public float coreThrust;

	SilantroControls controlBoard;
	PanelControls panelControls;

	void Awake()
	{
		//
		//ADD AUDIOSOURCES
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
			GameObject soundPoint = new GameObject();
			soundPoint.transform.parent = this.transform;
			soundPoint.transform.localPosition = new Vector3 (0, 0, 0);
			soundPoint.name = this.name +" Sound Point";
			//

			EngineRun = soundPoint.gameObject.AddComponent<AudioSource>();
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

		//
		GameObject brain = GameObject.FindGameObjectWithTag ("Brain");
		if (brain.transform.root == gameObject.transform.root) {
			instrumentation = brain.GetComponent<SilantroInstrumentation> ();
		}
		if (instrumentation == null) {
			Debug.LogError ("Instrumentation System is Missing!! Add COG to aircraft");
		} else {
			instrumentation.boom = EngineRun;
		}
		//
			controlBoard = GameObject.FindGameObjectWithTag ("GameController").GetComponent<SilantroControls> ();
			panelControls=GameObject.FindGameObjectWithTag ("PanelControls").GetComponent<PanelControls> ();
			if (controlBoard == null) {
				Debug.LogError ("Control Board is missing, Place Control Board in scene and restart!");
			}
		startEngine= controlBoard.engineStart;
		stopEngine = controlBoard.engineShutdown;
		throttleUp = controlBoard.engineThrottleUp;
		throttleDown = controlBoard.engineThrottleDown;
		afterburnerControl = controlBoard.AfterburnerControl;
			//
		LPIdleRPM = LowPressureFanRPM * 0.1f;
		HPIdleRPM = HighPressureFanRPM * 0.09f;
		//
		if (reheatSystem == ReheatSystem.Afterburning) {
			canUseAfterburner = true;
		} else if (reheatSystem == ReheatSystem.noReheat) {
			canUseAfterburner = false;
		}
		//
		AfterburnerOperative = false;
	}
// Use this for initialization
	void Start () {if (Parent == null) {
			Debug.Log ("Engine cannot be Operated without a Rigidbody parent!!, add Rigidbody to an empty gameobject if engine is for Test");
		}if (attachedFuelTank == null) {
			Debug.Log ("No fuel tank is attached to this Engine!!, Note: Engine will not function correctly");
		}
		//RECIEVE DIAMETERS
		intakeDiameter = IntakeDiameter;
		//
		//SET UP MASS FACTOR FOR EGT CALCULATION
		massFactor = UnityEngine.Random.Range(1.6f,2.2f);
		fuelMassFlow = TSFC / 1000f;
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
		intakeFactor = UnityEngine.Random.Range(0.38f,0.45f);//FACTOR OF TEMPERATURE IN FUTURE UPDATES
		combusionFactor = combustionEnergy/42f;
		//
		//


		EngineOn = false;
		start = false;
		starting = false;
		stop = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (attachedFuelTank != null) {
			currentTankFuel = attachedFuelTank.CurrentAmount;
		}
		//
		//
		if ((panelControls.engineStart || Input.GetKeyDown (startEngine)) && attachedFuelTank != null&& attachedFuelTank.CurrentAmount > 0) {
			start = true;
			panelControls.engineStart = false;
		}
		if (Input.GetKeyDown (stopEngine)) {
			stop = true;
		}
		if((panelControls.engineThrottleUp || Input.GetKey(throttleUp)) && CurrentEngineState == EngineState.Running)
		{
			FuelInput = Mathf.Lerp (FuelInput, 1.0f, throttleSpeed);
		}
		if(Input.GetKey(throttleDown))
		{
			FuelInput = Mathf.Lerp (FuelInput, 0.0f, throttleSpeed);
		}
		//
		//AfterburnerControl
		if (Input.GetKeyDown (afterburnerControl) && canUseAfterburner && enginePower > 0.5f && FuelInput > 0.5f) {
			AfterburnerOperative = !AfterburnerOperative;
		}
		//
		EngineActive ();
		if (enginePower > 0f) {
			EngineCalculation ();
		}
		//
		if (InUse) {
			UseFuel ();
		}
		//
		//performanceConfiguration.EngineLinearSpeed = num * 1.94384444f;
		if (Parent) {
			switch (CurrentEngineState) {
			case EngineState.Off:
					ShutdownEngine ();
				break;
			case EngineState.Starting:
				StartEngine ();
				break;
			case EngineState.Running:
				RunEngine ();
				break;
			}
		}
		//INTERPOLATE ENGINE RPM
		if (EngineOn) {
			CurrentRPM =   Mathf.Lerp (CurrentRPM, TargetRPM,RPMAcceleration * Time.deltaTime * (enginePower* fuelFactor* fuelFactor));
			currentHPRPM =Mathf.Lerp(currentHPRPM,targetHPRPM,RPMAcceleration* Time.deltaTime * (enginePower* fuelFactor* fuelFactor));
			//exhaustModule.rateOverTime = (enginePower * maximumEmmisionValue);
		} else {
			CurrentRPM =  Mathf.Lerp (CurrentRPM, 0.0f, RPMAcceleration * Time.deltaTime);
			currentHPRPM =Mathf.Lerp(currentHPRPM,0.0f,RPMAcceleration* Time.deltaTime);
		}
		//

		if (null != EngineRun)
		{
			if (Parent != null) {
				//PERFORM MINOR ENGINE CALCULATIONS TO CONTROL SOUND PITCH
				float magnitude = Parent.velocity.magnitude;
				float num2 = magnitude * 1.94384444f;
				float num3 = CurrentRPM + num2 * 10f;
				float num4 = (num3 - LPIdleRPM) / (LowPressureFanRPM - LPIdleRPM);
			
			if (AfterburnerOperative) {
				num5 = EngineIdlePitch + (EngineAfterburnerPitch - EngineIdlePitch) * num4;
			} else if (!AfterburnerOperative) {
				num5 = EngineIdlePitch + (EngineMaximumRPMPitch - EngineIdlePitch) * num4;
			}
			num5 = Mathf.Clamp(num5, 0f, maximumPitch);
			//
			}
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
			if (CurrentRPM < LPIdleRPM)
			{
				EngineRun.volume = engineSoundVolume * num5;
				if (CurrentRPM < LPIdleRPM * 0.1f)
				{
					EngineRun.volume = 0f;
				}
			}
			else
			{
				EngineRun.volume = engineSoundVolume * num5;
			}

		}

	}
	//
	//
	//
	public IEnumerator ReturnIgnition()
	{
		yield return new WaitForSeconds (0.5f);
		start = false;
		stop = false;
	}
	//
	private void ShutdownEngine()
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
		TargetRPM = 0f;
		targetHPRPM = 0f;
	}
	//
	private void StartEngine()
	{
		if (starting)
		{
			if (!EngineStart.isPlaying)
			{
				CurrentEngineState = EngineState.Running;
				starting = false;
				//exhaustModule.enabled = true;
				RunEngine();
			}
		}
		else
		{
			EngineStart.Stop();
			CurrentEngineState = EngineState.Off;
		}
		TargetRPM = LPIdleRPM;
		targetHPRPM = HPIdleRPM;
	}
	//
	//
	private void RunEngine()
	{
		if (EngineStart.isPlaying)
		{
			EngineStart.Stop();
		}
		FuelInput = Mathf.Clamp(FuelInput, 0f, 1f);
		TargetRPM = LPIdleRPM + (LowPressureFanRPM - LPIdleRPM) * FuelInput;
		targetHPRPM = HPIdleRPM + (HighPressureFanRPM - HPIdleRPM) * FuelInput;
		InUse = true;

		if (stop)
		{
			
			CurrentEngineState = EngineState.Off;
			EngineOn = false;
			//exhaustEffect.emission.enabled = false;
			//Stop Fuel Alert
			EngineThrust = 0;
			EngineShutdown.Play();
			FuelInput = 0f;
			StartCoroutine(ReturnIgnition());
		}
	}
	//
	//ACCELERATE AND DECELERATE ENGINE
	private void EngineActive()
	{
		if (EngineOn)
		{
			if (enginePower < 1f && !isAccelerating)
			{
				enginePower += Time.deltaTime * engineAcceleration;
				//Calculate EGT
			}
		}
		else if (enginePower > 0f)
		{
			enginePower -= Time.deltaTime * engineAcceleration;
		}
		else
		{
			enginePower = 0f;EngineThrust = 0;
			EGT = 0f;
		}
	}
	///
	///STOP ENGINE IF DESTROYED
	public void DestroyEngine()
	{

		EngineOn = false;
		EngineThrust = 0f;
	}
	//

	//DEPLETE FUEL LEVEL WITH USAGE
	public void UseFuel()
	{
		{
			actualConsumptionrate = combusionFactor*fuelMassFlow * (FuelInput +0.000001f)* EngineRun.pitch ;
			//
			if (attachedFuelTank != null) {
				attachedFuelTank.CurrentAmount -= actualConsumptionrate * Time.deltaTime;
			}
		}

		if (attachedFuelTank.CurrentAmount == 0f)
		{
			EngineRun.volume = 0f;
			EngineRun.pitch = 0f;
			stop = true;
			EngineThrust = 0f;
		}


	}
	//


	//CALCULATE ENGINE THRUST
	public void EngineCalculation()
	{
		if (instrumentation != null) {
			airDensity=instrumentation.airDensity;
			ambientPressure = instrumentation.ambientPressure;
		}
		//
		EnginePower = enginePower *100f;
		//
		if (Parent != null) {
			float velocity = Parent.velocity.magnitude;
			EngineLinearSpeed = velocity;
		}
		//
		intakeArea = (3.142f * intakeDiameter *intakeDiameter)/4f;
		exhaustArea = (3.142f * ExhaustDiameter * ExhaustDiameter) / 4f;
		//
		intakeAirVelocity = (3.142f * intakeDiameter * LPRPM)/60f;
		fanAirVelocity = intakeAirVelocity * intakeFactor;
		 exhaustAirVelocity = (3.142f * ExhaustDiameter * HPRPM) / 60f;
		//
		intakeAirMassFlow = airDensity * intakeArea * fanAirVelocity;
		//
		bypassfactor = (1+bypassRatio);
		coreAirMassFlow = (1 / bypassfactor) * intakeAirMassFlow;
		fanAirMassFlow = (bypassRatio / bypassfactor) * intakeAirMassFlow;
		//
		//Thrust Values
		fanThrust = fanAirMassFlow *(intakeAirVelocity - EngineLinearSpeed);
		//
		//Afterburner Calculations
		if (AfterburnerOperative) {
			coreThrust = (((coreAirMassFlow + fuelMassFlow) * (exhaustAirVelocity*1.5f)) - (coreAirMassFlow * EngineLinearSpeed) + (exhaustArea * ((OverallPressureRatio * ambientPressure) - ambientPressure)));
		} else {
			coreThrust = (((coreAirMassFlow + fuelMassFlow) * (exhaustAirVelocity)) - (coreAirMassFlow * EngineLinearSpeed) + (exhaustArea * ((OverallPressureRatio * ambientPressure) - ambientPressure)));
		}
		//
		EngineThrust = (coreThrust + fanThrust);//TOTAL THRUST GENERATED
		if (coreThrust > 0) {
			CalculateFuelFlow (EngineThrust);
		}
	}
	//
	void CalculateFuelFlow(float currentThrust)
	{
		float poundThrust = currentThrust / 4.448f;
		if (AfterburnerOperative) {
			sfc = (poundThrust * AfterburnerTSFC) / 3600f;
		} else {
			sfc = (poundThrust * TSFC) / 3600f;
		}

		//
		fuelMassFlow = sfc*0.4536f;
	}
	//
	public void FixedUpdate()
	{
		if (Parent)
		{


			LPRPM = CurrentRPM;
			HPRPM = currentHPRPM;
			if (fan)
			{
				if (rotationDirection == RotationDirection.CCW) {
					if (X) {
						fan.Rotate (new Vector3 (CurrentRPM * Time.deltaTime, 0, 0));
					}
					if (Y) {
						fan.Rotate (new Vector3 (0, CurrentRPM * Time.deltaTime, 0));
					}
					if (Z) {
						fan.Rotate (new Vector3 (0, 0, CurrentRPM * Time.deltaTime));
					}
				}
				//
				if (rotationDirection == RotationDirection.CW) {
					if (X) {
						fan.Rotate (new Vector3 (-1f *CurrentRPM * Time.deltaTime, 0, 0));
					}
					if (Y) {
						fan.Rotate (new Vector3 (0, -1f *CurrentRPM * Time.deltaTime, 0));
					}
					if (Z) {
						fan.Rotate (new Vector3 (0, 0, -1f *CurrentRPM * Time.deltaTime));
					}
				}
			}
			if (EngineThrust > 0f)
			{
				Vector3 force = Thruster.forward * EngineThrust;
				Parent.AddForce(force, ForceMode.Force);
			}
		}
		if (CurrentRPM <= 0f)
		{
			CurrentRPM = 0f;
		}
	}
	//
	#if UNITY_EDITOR
	void OnDrawGizmos()
	{
		//

		ExhaustDiameter = EngineDiameter * ExhaustDiameterPercentage/100f;
		Handles.color = Color.red;
	if(Thruster != null){
			Handles.DrawWireDisc (Thruster.position, Thruster.transform.forward, (ExhaustDiameter/2f));
	}
		IntakeDiameter = EngineDiameter * IntakeDiameterPercentage / 100f;
		Handles.color = Color.blue;
		if(fan != null && Parent!=null){
			Handles.DrawWireDisc (fan.transform.position, Parent.transform.forward, (IntakeDiameter / 2f));
	}
		//
		Handles.color = Color.cyan;
	if(Thruster != null && fan != null ){
		Handles.DrawLine (fan.transform.position, Thruster.position);
	}
	}
	//
	#endif


}
