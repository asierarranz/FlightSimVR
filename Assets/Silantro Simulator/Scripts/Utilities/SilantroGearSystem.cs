using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//
//Property of Oyedoyin Dada
//cc dadaoyedoyin@gmail.com
//
//
//
public class SilantroGearSystem : MonoBehaviour {
	//
	[Header("Wheel Settings")]
	public List<WheelSystem> wheelSystem = new List<WheelSystem>();
	//
	//
	[Header("Wheel Data Configuration")]
	public float maximumSteerAngle =20f;
	float speed;
	public Transform frontWheelAxle;
	public enum RotationAxis
	{
		X,
		Y,
		Z
	}
	public RotationAxis rotationAxis = RotationAxis.X;
	Vector3 axisRotation;
	private Quaternion InitialModelRotation = Quaternion.identity;

	[Header("Brake Settings")]
	public float brakeTorque= 5000f;
	public bool brakeActivated = true;
	KeyCode brake ;
	[Header("Push Settings")]
	[SerializeField][ForceOnly]public float availablePushForce = 0f;
	public enum PushDirection
	{
		forward,
		Backward
	}
	public PushDirection pushDirection;
	float direction;
	public bool pushBack;
	//
	float steerAngle;
	[Header("")]
	[Header("Gear Elements")]
	public List<Gear> gears = new List<Gear>() ;

	[System.Serializable]
	public class Gear
	{
		public string Identifier;
		public Transform gearElement;
		[Header("Rotaion Amount")]
		public float xAxis;
		public float yAxis;
		public float zAxis;
		[HideInInspector]public Quaternion initialPosition;
		[HideInInspector]public Quaternion finalPosition;
	}
	public float gearOpenDragCoefficient = 0.016f;
	[HideInInspector]public SilantroController control;
	[HideInInspector]public float baseDragCoefficient;
	//
	[Header("Switches")]
	public bool open;
	[HideInInspector]public bool gearOpened = true;
	bool activated;

	public bool close ;
	[HideInInspector]public bool gearClosed = false;
	//
	[Header("Control Values")]
	public float openTime = 7f;
	public float closeTime = 7f;
	public float rotateSpeed = 10f;
	//
	[Header("Mechanical Sounds")]
	public AudioClip gearUp;
	public AudioClip gearDown;
	//
	[HideInInspector]public AudioSource mechanicalSounds;
	//
	KeyCode gearActivate;
	//
	SilantroControls controlBoard;
	PanelControls panelControls;
	[System.Serializable]
	public class WheelSystem
	{
		[Header("Connections")]
		public string Identifier;
		public WheelCollider collider;
		public Transform wheelModel;
		//
		[Header("Axis Rotation")]
		public bool AxisX = true;
		public bool AxisY;
		public bool AxisZ;
		//
		[Header("Controls")]
		public bool steerable;
		public bool attachedMotor;
	}
	//
	void Awake () {
		//
		if (pushDirection == PushDirection.Backward) {
			direction = -1;
		} else if (pushDirection == PushDirection.forward) {
			direction = 1;
		}
		controlBoard = GameObject.FindGameObjectWithTag ("GameController").GetComponent<SilantroControls> ();
		panelControls=GameObject.FindGameObjectWithTag ("PanelControls").GetComponent<PanelControls> ();

		if (controlBoard == null) {
			Debug.LogError ("Control Board is missing, Place Control Board in scene and restart!");
		}
		brake = controlBoard.BrakeHoldRelease;
		gearActivate = controlBoard.LandingGear;
		GameObject attachee = new GameObject ();
		attachee.transform.parent = this.transform;
		attachee.transform.localPosition = new Vector3 (0, 0, 0);
		attachee.transform.name = "Mechanical Sounds";
		mechanicalSounds = attachee.AddComponent<AudioSource>();
		brakeActivated = true;
		//
	
	}
	void Start()
	{
		brakeActivated = false;
		foreach (WheelSystem system in wheelSystem) {
			if (system.attachedMotor) {
				system.collider.motorTorque = direction *0; // Bug, it was 1000f AA
			}
		}
		if (rotationAxis == RotationAxis.X) {
			axisRotation = new Vector3 (1, 0, 0);
		} else if (rotationAxis == RotationAxis.Y) {
			axisRotation = new Vector3 (0, 1, 0);
		} else if (rotationAxis == RotationAxis.Z) {
			axisRotation = new Vector3 (0, 0, 1);
		}
		axisRotation.Normalize();

		if ( null != frontWheelAxle )
		{
			InitialModelRotation = frontWheelAxle.transform.localRotation;
		}
		brakeActivated = true;

		foreach (Gear gear in gears) {
			gear.initialPosition = gear.gearElement.localRotation;
			gear.finalPosition = Quaternion.Euler (gear.xAxis, gear.yAxis, gear.zAxis);
		}
		//
	}
	void Update()
	{
		
		if(Input.GetKeyDown(gearActivate) && !gearOpened && !activated && control.aircraft.transform.position.y > 5f)
		{
			open = true;
		}
		if(Input.GetKeyDown(gearActivate) && !gearClosed && !activated && control.aircraft.transform.position.y > 5f)
		{
			close = true;
		}
		//
		if (open && !gearOpened) {

			foreach (Gear gear in gears) {
				gear.gearElement.localRotation = Quaternion.RotateTowards (gear.gearElement.localRotation, gear.initialPosition, Time.deltaTime * rotateSpeed);
			}
			if (!activated) {
				StartCoroutine (Open ());
				activated = true;
				if (gearDown != null) {
					mechanicalSounds.PlayOneShot (gearDown);
				}
			}
		}
		if (close && !gearClosed ) {

			foreach (Gear gear in gears) {
				gear.gearElement.localRotation = Quaternion.RotateTowards (gear.gearElement.localRotation, gear.finalPosition, Time.deltaTime * rotateSpeed);
			}
			if (!activated) {
				StartCoroutine (Close ());
				activated = true;
				if (gearUp != null) {
					mechanicalSounds.PlayOneShot (gearUp);
				}
			}
		}
	}
	//
	//
	IEnumerator Open()
	{
		yield return new WaitForSeconds (openTime);
		gearOpened = true;gearClosed =false;CloseSwitches ();control.dragCoefficient = baseDragCoefficient + gearOpenDragCoefficient;
		activated = false;

	}
	IEnumerator Close()
	{
		yield return new WaitForSeconds (closeTime);
		gearClosed = true;control.dragCoefficient = baseDragCoefficient;
		gearOpened = false;CloseSwitches ();
		activated = false;

	}
	void CloseSwitches()
	{
		open =  false;
		close = false;
	}
	// Update is called once per frame
	void FixedUpdate () {
		//
		//SEND BRAKING DATA
		foreach (WheelSystem system in wheelSystem) {
			BrakingSystem (system);
		}
		//

		//
		steerAngle = -1 *maximumSteerAngle * Input.GetAxis (controlBoard.Rudder);
		//
		foreach (WheelSystem system in wheelSystem) {
			//SEND ROTATION DATA
			RotateWheel(system.wheelModel,system);
			//SEND ALIGNMENT DATA
			WheelAllignment(system,system.wheelModel);
			//SEND STEER DATA

			if (system.steerable) {
				//ROTATE FRONT AXLE
				if ( null != frontWheelAxle )
				{
					frontWheelAxle.transform.localRotation = InitialModelRotation;
					frontWheelAxle.transform.Rotate( axisRotation, steerAngle );
				}
				if (transform.parent.gameObject.GetComponent<Rigidbody> ().velocity.magnitude > 1f) {
					if (system.collider != null) {
						system.collider.steerAngle = steerAngle;
					}
				}
			}
		}
	}
	//
	//ROTATE WHEEL
	void RotateWheel(Transform wheel,WheelSystem system)
	{
		if (system.collider != null) {
			speed = system.collider.rpm;
		}
		if (wheel != null) {
			//
			if (system.AxisX) {
				wheel.Rotate (new Vector3 (speed * Time.deltaTime, 0, 0));
			}
			if (system.AxisY) {
				wheel.Rotate (new Vector3 (0, speed * Time.deltaTime, 0));
			}
			if (system.AxisZ) {
				wheel.Rotate (new Vector3 (0, 0, speed * Time.deltaTime));
			}
		}
	}
	//
	//ALLIGN WHEEL TO COLLIDER
	void WheelAllignment(WheelSystem system,Transform wheel)
	{
		if (wheel != null) {
			RaycastHit hit;
			WheelHit CorrespondingGroundHit;
			if (system.collider != null) {
				Vector3 ColliderCenterPoint = system.collider.transform.TransformPoint (system.collider.center);
				system.collider.GetGroundHit (out CorrespondingGroundHit);

				if (Physics.Raycast (ColliderCenterPoint, -system.collider.transform.up, out hit, (system.collider.suspensionDistance + system.collider.radius) * transform.localScale.y)) {
					wheel.position = hit.point + (system.collider.transform.up * system.collider.radius) * transform.localScale.y;
					float extension = (-system.collider.transform.InverseTransformPoint (CorrespondingGroundHit.point).y - system.collider.radius) / system.collider.suspensionDistance;
					Debug.DrawLine (CorrespondingGroundHit.point, CorrespondingGroundHit.point + system.collider.transform.up, extension <= 0.0 ? Color.magenta : Color.white);
					Debug.DrawLine (CorrespondingGroundHit.point, CorrespondingGroundHit.point - system.collider.transform.forward * CorrespondingGroundHit.forwardSlip * 2f, Color.green);
					Debug.DrawLine (CorrespondingGroundHit.point, CorrespondingGroundHit.point - system.collider.transform.right * CorrespondingGroundHit.sidewaysSlip * 2f, Color.red);
				} else {
					wheel.transform.position = Vector3.Lerp (wheel.transform.position, ColliderCenterPoint - (system.collider.transform.up * system.collider.suspensionDistance) * transform.localScale.y, Time.deltaTime * 10f);
				}
			}
		}
	}
	//
	// BRAKE
	void BrakingSystem(WheelSystem wheel)
	{
		//PEG DOWN WHEEL WITH BRAKE
		//
		if (wheel.collider != null) {
			wheel.collider.brakeTorque = 0f;
		}
		
		if (Input.GetKeyDown (brake) )
		{
			if (brakeActivated)
			{
                
				if (wheel.attachedMotor) {
					wheel.collider.motorTorque =direction* (availablePushForce / 10f);
				}
				brakeActivated = false;
			}
			else if (!brakeActivated)
			{
				brakeActivated = true;
			}
			//panelControls.brakesButton = false;
		} 
        

		//
		//ACTIVATE BRAKE
		if (brakeActivated && wheel.attachedMotor) {
			wheel.collider.brakeTorque = brakeTorque;
		}
	}

}
