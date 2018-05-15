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
//
//[AddComponentMenu("Oyedoyin/Engine System/Lift Fan")]
public class SilantroLiftFan : MonoBehaviour {
	//Fan State
	public enum FanState
	{
		Off,
		Clutching,
		Active
	}
	//Control switches
	public bool start;
	public bool stop;
	private bool starting;
	[Header("Power Supplied")]
	public float powerExtract = 29000f;
	public SilantroTurboFan attachedEngine;
	//Fan Properties to calculate Thrust
	[Header("Fan Specifications")]
	/// <summary>
	/// Diameter of the Lift Fan
	/// </summary>
	public float fanDiameter = 1f;
	public float fanAcceleration = 0.2f;
	//Efficiency affected by cross-wind and aircraft speed
	public float fanEfficiency = 76f;
	//
	[HideInInspector]public bool fanOn;
	[HideInInspector]public float fanPower;
	//
	[Header(" ")]//SPACE
	[Header("Fan Performance")]
	public FanState CurrentFanState = FanState.Off;
	[SerializeField] [EfficiencyOnly]public float FanPower;
	private float FanIdleRPM = 10f;
	private float FanMaximumRPM = 100f;
	[SerializeField][RPMOnly]public float currentFanRPM;
	[SerializeField][SilantroShowAttribute]float fanShaftTorque;
	//
	//
	[NonSerialized]
	public bool isAccelerating;
	//
	[Header("Lift System")]
	public LayerMask surfaceMask;
	public float maximumHoverHeight = 30f;
	public float hoverDamper = 9000f;
	public float hoverAngleDrift = 25f;
	public enum LiftForceMode
	{
		Linear,
		Quadratic,
		Quatic
	}
	public LiftForceMode liftForceMode = LiftForceMode.Linear;
	private float exponent;
	//
	[HideInInspector]
	public float DesiredRPM;
	[Header(" ")]
	[Header("Connections")]
	[SerializeField][SilantroShowAttribute]float powerFactor = 1.2f;
	[HideInInspector]public float CurrentRPM;
	//
	public Rigidbody Parent;
	public Transform Thruster;
	public Transform fan;
	[Header("Fan Rotation Settings")]
	//
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
	//
	private AudioSource fanStart;
	private AudioSource fanRun;
	private AudioSource fanShutdown;
	//
	//SOUND SETTINGS
	private float idlePitch = 0.5f;
	private float maximumPitch = 1.2f;
	private float fanSoundVolume = 1.5f;
	//
	[Header("Sound Clips")]
		public AudioClip fanStartClip;
		public AudioClip fanRunningClip;
		public AudioClip fanShutdownClip;

	//
	[Header("Control")]
	[Range(0f,1f)]
	public float control;
	//
	[Header(" ")]//SPACE
	[Header("Fan Display")]

	[SerializeField][DensityOnly]public float airDensity = 1.225f;
	[SerializeField][ForceOnly]public float FanThrust;
	//
	//
	public IEnumerator ReturnIgnition()
	{
		yield return new WaitForSeconds (0.5f);
		start = false;
		stop = false;
	}
	//
	void Start()
	{
		if (Parent == null) {
			Debug.Log ("Engine cannot be Operated without a Rigidbody parent!!, add Rigidbody to an empty gameobject if engine is for Test");
		}
		if (attachedEngine == null) {
			Debug.Log ("Engine requires an active Turbofan engine to function");
		}
		//CALCULATE MAXIMUM RPM POSSIBLE
		float kq = (10f / fanEfficiency);
		float squareRPM = (550f * powerExtract) / (2f * 3.142f * kq * airDensity * Mathf.Pow (fanDiameter, 5f));
		FanMaximumRPM = (Mathf.Pow (squareRPM,0.33333f) *60f);
		FanIdleRPM = FanMaximumRPM * 0.1f;
		//
		if (!Thruster) {
			GameObject thruter = new GameObject ();
			Thruster = thruter.transform;
		}
		//DETERMINE FORCE MODE
		if (liftForceMode == LiftForceMode.Linear)
		{
			exponent = 1f;
		} 
		else if (liftForceMode == LiftForceMode.Quadratic) 
		{
			exponent = 2f;
		} 
		else if (liftForceMode == LiftForceMode.Quatic) 
		{
			exponent = 3f;
		}
		//
		//SETUP SOUND PARTS
		SetupSoundSystem();
	}
	//
	void SetupSoundSystem()
	{
		if (null != fanStartClip)
		{
			fanStart = Thruster.gameObject.AddComponent<AudioSource>();
			fanStart.clip = fanStartClip;
			fanStart.loop = false;
			fanStart.dopplerLevel = 0f;
			fanStart.spatialBlend = 1f;
			fanStart.rolloffMode = AudioRolloffMode.Custom;
			fanStart.
			maxDistance = 650f;
		}
		if (null != fanRunningClip)
		{
			fanRun = Thruster.gameObject.AddComponent<AudioSource>();
			fanRun.clip =  fanRunningClip;
			fanRun.loop = true;
			fanRun.Play();
			fanSoundVolume = fanRun.volume * 1.3f;
			fanRun.spatialBlend = 1f;
			fanRun.dopplerLevel = 0f;
			fanRun.rolloffMode = AudioRolloffMode.Custom;
			fanRun.maxDistance = 600f;
		}
		if (null != fanShutdownClip)
		{
			fanShutdown = Thruster.gameObject.AddComponent<AudioSource>();
			fanShutdown.clip = fanShutdownClip;
			fanShutdown.loop = false;
			fanShutdown.dopplerLevel = 0f;
			fanShutdown.spatialBlend = 1f;
			fanShutdown.rolloffMode = AudioRolloffMode.Custom;
			fanShutdown.maxDistance = 650f;
		}
	}
	//
	public void Update()
	{
		//
		if (fanRun != null) 
		{
			float rpmControl = (currentFanRPM - FanIdleRPM) / (FanMaximumRPM - FanIdleRPM);
			float pitchControl = idlePitch + (maximumPitch - idlePitch) * rpmControl;
			//
			pitchControl = Mathf.Clamp(pitchControl, 0f, maximumPitch);
			//
			//CONTROL FAN RUN PITCH
			fanRun.pitch = pitchControl * fanPower;
			fanRun.volume = fanSoundVolume;
			//
			if (currentFanRPM < FanIdleRPM) 
			{
				fanRun.volume = fanSoundVolume * pitchControl;
				if (currentFanRPM < FanIdleRPM * 0.1f) {
					fanRun.volume = 0f;
				}
			} 
			else
			{
				fanRun.volume = fanSoundVolume * pitchControl;
			}
		}
		//
		FanPowering();
		//
		if (fanPower > 0f) {
			FanCalculation ();
		}
		//
		if (Parent) {
			switch (CurrentFanState) {
			case FanState.Off:
				ShutDown ();
				break;
			case FanState.Clutching:
				Clutching ();
				break;
			case FanState.Active:
				Running ();
				break;
			}

		}
		//RPM REV
		if (fanOn) {
			CurrentRPM =   Mathf.Lerp (CurrentRPM, DesiredRPM,(fanAcceleration*4f) * Time.deltaTime * fanPower);
		} else {
			CurrentRPM =  Mathf.Lerp (CurrentRPM, DesiredRPM, (fanAcceleration*4f) * Time.deltaTime);
		}
	}
	//
	//SHUTDOWN
	private void ShutDown()
	{
		if (fanStart.isPlaying) {
			fanStart.Stop ();
			start = false;
		}
		if (start && attachedEngine != null) {
			fanOn = true;
			fanStart.Play ();
			CurrentFanState = FanState.Clutching;
			starting = true;
			StartCoroutine (ReturnIgnition ());
		}
		DesiredRPM = 0f;
	}
	//START FAN
	private void Clutching()
	{
		if (starting) {
			if (!fanStart.isPlaying) {
				CurrentFanState = FanState.Active;
				starting = false;
				Running();
			}
		}
		else
		{
			fanStart.Stop();
			CurrentFanState = FanState.Off;
		}
		DesiredRPM = FanIdleRPM;
	}
	//RUN FAN
	//
	private void Running()
	{
		if (fanStart.isPlaying) {
			fanStart.Stop ();
		}
		DesiredRPM = FanIdleRPM + (FanMaximumRPM - FanIdleRPM) * control;
		if (stop)
		{
			CurrentFanState = FanState.Off;
			fanOn = false;
			fanShutdown.Play();
			StartCoroutine(ReturnIgnition());
		}
	}
	//
	//POWER UP FAN
	private void FanPowering()
	{
		if (fanOn) {
			if (fanPower < 1f && !isAccelerating) {
				fanPower += Time.deltaTime * fanAcceleration;
			}

		} else if (fanPower > 0f) {
			fanPower -= Time.deltaTime * fanAcceleration;

		} else {
			fanPower = 0f;
		}
	}
	//
	//CALCULATE FAN THRUST
	public void FanCalculation()
	{
		FanPower = fanPower * 100f;
		//
		//SIMULATE TORQUE EXTRACTION FROM ENGINE
		//
		fanShaftTorque = (10f/fanEfficiency)* airDensity* ((currentFanRPM/60f)*(currentFanRPM/60f)) * Mathf.Pow(fanDiameter,5f);
		//
		//Calculate FAN INTAKE AREA
		float fanIntakeArea = (3.142f * fanDiameter *fanDiameter)/4f;
		//
		float fanAirVelocity = (3.142f * fanDiameter * currentFanRPM * 0.4f)/60f;
		//
		float intakeAirmassFlow = airDensity * fanIntakeArea * fanAirVelocity;
		//CALCULATE FAN THRUST
		FanThrust = fanAirVelocity * intakeAirmassFlow;
	}
	//
	//
	#if UNITY_EDITOR
	void OnDrawGizmos()
	{
		//

		Handles.color = Color.red;
		if(Thruster != null){
			Handles.DrawWireDisc (Thruster.transform.position, Thruster.transform.up,  0.2f);
		}

		Handles.color = Color.blue;
		if(fan != null && Parent!=null){
			Handles.DrawWireDisc (fan.transform.position, Parent.transform.up,(fanDiameter/2f));
		}
		//
		Handles.color = Color.cyan;
		if(Thruster != null && fan != null ){
			Handles.DrawLine (fan.transform.position, Thruster.position);
		}
	}
	//
	#endif
	//
	private void FixedUpdate()
	{
		if (fan) {
			currentFanRPM = CurrentRPM;
			//

			if (rotationDirection ==RotationDirection.CCW) {
				if (X) {
					fan.Rotate (new Vector3 (currentFanRPM *0.1f* Time.deltaTime, 0, 0));
				}
				if (Y) {
					fan.Rotate (new Vector3 (0, currentFanRPM  *0.1f* Time.deltaTime, 0));
				}
				if (Z) {
					fan.Rotate (new Vector3 (0, 0, currentFanRPM  *0.1f* Time.deltaTime));
				}
			}
			//
			if (rotationDirection == RotationDirection.CW) {
				if (X) {
					fan.Rotate (new Vector3 (-1f *currentFanRPM  *0.1f* Time.deltaTime, 0, 0));
				}
				if (Y) {
					fan.Rotate (new Vector3 (0, -1f *currentFanRPM  *0.1f* Time.deltaTime, 0));
				}
				if (Z) {
					fan.Rotate (new Vector3 (0, 0, -1f *currentFanRPM  *0.1f* Time.deltaTime));
				}
			}
		}
		///
		//
		if (CurrentRPM <= 0f)
		{
			CurrentRPM = 0f;
		}
		//
		if (fanPower > 0f && CurrentFanState == FanState.Active) {
			ApplyLiftForce ();
		}
	}
	//
	//APPLY LIFT FORCE
	private	void ApplyLiftForce()
	{
		RaycastHit groundHit;
		//CALCULATE DIRECTION OF FORCE
		var up = Thruster.up;
		var gravity = Physics.gravity.normalized;
		//
		up = Vector3.RotateTowards(up, - gravity,hoverAngleDrift*Mathf.Deg2Rad,1);
		powerFactor = 0;
		if(!Physics.Raycast(Thruster.position,-up, out groundHit,maximumHoverHeight,surfaceMask))
		{
			return;
		}
		//
		//CALCULATE POWER FALLOFF
		powerFactor = Mathf.Pow((maximumHoverHeight - groundHit.distance)/maximumHoverHeight,exponent);
		var liftForce = powerFactor * FanThrust;
		//
		//CALCULATE DAMPING
		var velocity = Vector3.Dot(Parent.GetPointVelocity(Thruster.position),up);
		var drag = -velocity * Mathf.Abs (velocity) * hoverDamper;
		//
		//APPLY FORCE AT POSITION
		Parent.AddForceAtPosition(up * (liftForce+drag),Thruster.position,ForceMode.Force);

	}
}
