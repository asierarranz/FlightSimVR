//
//Property of Oyedoyin Dada
//cc dadaoyedoyin@gmail.com
//
//
//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
//
public class SilantroController : MonoBehaviour {

	public string aircraftName = "Default";
	[Header("Aircraft Engine Type")]
	public AircraftType engineType = AircraftType.TurboFan;
	public enum AircraftType
	{
		TurboProp,
		TurboFan,
		TurboJet
	}
	//
	[Header("Basic Drag System")]
	public DragSystem dragSystem;
	[System.Serializable]
	public class DragSystem
	{
		public bool useDrag;
		[Header("Enter values in m,m,knots")]
		public float averagefuselargeLength = 5f;
		public float averagefuselargeDiameter = 1f;
		public float maximumSpeed = 100f;
		public bool showBounds = true;
	}

	float dragForce;float fuselargeArea;
	[HideInInspector]public float dragCoefficient;
	float extraDrag = 0;
	SilantroDragMultiplier[] extraDrags;
	//
	[HideInInspector]public bool captured;	
	[HideInInspector]public float maximumPossibleEngineThrust;
	[Header("Aircraft Data")]

	[SerializeField][AreaOnly]public float wingArea = 10f;
	//[SerializeField][WeightOnly]public float loadedWeight = 1000f;
	[SerializeField][SilantroShowAttribute]public int AvailableEngines = 1;
	[SerializeField][ForceOnly]public float totalThrustGenerated = 1f;
	[HideInInspector]public float totalConsumptionRate;
	//

	[SerializeField][SilantroShowAttribute]public float wingLoading;
	[SerializeField][SilantroShowAttribute]public float thrustToWeightRatio;
	//
	[HideInInspector]public SilantroFuelTank currentTank;
	//

	[Header("Master Switch")]
	public bool isDestructible;
	//
	[Header("Aircraft Components")]
	//[SerializeField][NameOnly]public string CenterOfGravity = "Missing";
	[SerializeField][NameOnly]public string InstrumentBoard = "Missing";
	[SerializeField][NameOnly]public string AerodynamicSurfaces = "1";
	[SerializeField][NameOnly]public string Engines = "1";
	[SerializeField][NameOnly]public string WheelSystem = "Missing";
	[SerializeField][NameOnly]public string WeaponSystem = "Missing";
	[SerializeField][NameOnly]public string FuelSystem = "Missing";
	//
	[Header("")]
	[Header("Aircraft Weight System")]
	public float emptyWeight = 1000f;
	[SerializeField][WeightOnly]public float currentWeight;
	public float maximumWeight = 5000f;
	//
	[HideInInspector]public SilantroFuelDistributor fuelsystem;
	SilantroInstrumentation datalog;
	[HideInInspector]public SilantroGearSystem gearHelper;
	[HideInInspector]public Rigidbody aircraft;
	//
	//Engine
	[HideInInspector]public SilantroTurboFan[] turbofans;
	[HideInInspector]public SilantroTurboJet[] turboJet;
	[HideInInspector]public SilantroTurboProp[] turboprop;
	SilantroAerofoil[] wings;
	//
	Vector3 centerPosition;
	Transform frontMark;
	Transform rearMark;

	//
	GameObject cog;
	//
	// Use this for initialization
	void Start () {
		aircraft = GetComponent<Rigidbody> ();
		if (aircraft == null) {
			Debug.Log ("Add Rigidbody component to airplane body");
		}
		//
		datalog = aircraft.gameObject.GetComponentInChildren<SilantroInstrumentation>();
		currentTank = fuelsystem.internalFuelTank;
		//AIRCRAFT SETUP
		if (engineType == AircraftType.TurboFan) {
			turbofans = GetComponentsInChildren<SilantroTurboFan> ();
			foreach (SilantroTurboFan turbofan in turbofans) {
				turbofan.attachedFuelTank = currentTank;
			}
			AvailableEngines = turbofans.Length;
			//
		} else if (engineType == AircraftType.TurboJet) {
			turboJet = GetComponentsInChildren<SilantroTurboJet> ();
			AvailableEngines = turboJet.Length;
			foreach (SilantroTurboJet jet in turboJet) {
				jet.attachedFuelTank = currentTank;
			}
		} else if (engineType == AircraftType.TurboProp) {
			turboprop = GetComponentsInChildren<SilantroTurboProp> ();
			AvailableEngines = turboprop.Length;
			foreach (SilantroTurboProp turboProp in turboprop) {
				turboProp.attachedFuelTank = currentTank;
			}
		}
		//
		wings = GetComponentsInChildren<SilantroAerofoil>();
		wingArea = 0;
		if (wings.Length > 0) {
			foreach (SilantroAerofoil wing in wings) {
				wingArea += wing.aerofoilArea;
			}
		}
		//
		GameObject brain = GameObject.FindGameObjectWithTag ("Brain");
		if (brain.transform.root == gameObject.transform.root) {
			cog = brain;
		}
		//
		Collider col = gameObject.GetComponent<CapsuleCollider>();
		if (col == null) {
			Debug.Log ("Attach a capsule collider to the airplane and restart!!");
		} 
		float r = dragSystem.averagefuselargeDiameter/2f;
		fuselargeArea = (3.142f * r * r) + (2 * 3.142f * r * dragSystem.averagefuselargeLength);
	//
		//
		if (isDestructible && GetComponent<SilantroHealth> () != null) {
			GetComponent<SilantroHealth> ().isDistructible = true;
			SilantroAerofoilHealth[]	healths = GetComponentsInChildren<SilantroAerofoilHealth> ();
			foreach (SilantroAerofoilHealth hlt in healths) {
				hlt.isDistructible = true;
			}
	
		} else if (!isDestructible && GetComponent<SilantroHealth> () != null) {
			GetComponent<SilantroHealth> ().isDistructible = false;
			SilantroAerofoilHealth[]	healths = GetComponentsInChildren<SilantroAerofoilHealth> ();
			foreach (SilantroAerofoilHealth hlt in healths) {
				hlt.isDistructible = false;
			}
		}
		//
		//CALCULATE EXTRA DRAG OBJECTS
		extraDrags = GetComponentsInChildren<SilantroDragMultiplier>();
		extraDrag = 0;
		foreach (SilantroDragMultiplier drag in extraDrags) {
			if (drag.dragActive) {
				extraDrag += drag.dragCoefficient;
			}
		}
		//
		//
		//
		AerodynamicSurfaces = wings.Length.ToString();
		Engines = AvailableEngines.ToString ();
		if (datalog != null) {
			InstrumentBoard = "Active";
		} 
		if(fuelsystem != null) {
			FuelSystem = "Active";
		}
		if (gearHelper != null) {
			WheelSystem = "Active";
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		
		totalConsumptionRate = 0;

		if (engineType == AircraftType.TurboFan) {
			totalThrustGenerated = 0;totalConsumptionRate = 0;
			foreach (SilantroTurboFan turbofan in turbofans) {
				//CAPTURE ENGINE THRUST
				totalThrustGenerated += turbofan.EngineThrust;
				totalConsumptionRate += turbofan.actualConsumptionrate;
				//
				//
			}
		}
		if (engineType == AircraftType.TurboJet) {
			totalThrustGenerated = 0;totalConsumptionRate = 0;
			foreach (SilantroTurboJet turbojet in turboJet) {
				//CAPTURE ENGINE THRUST
				//	
				totalThrustGenerated += turbojet.EngineThrust;
				totalConsumptionRate += turbojet.actualConsumptionrate;
			}
		}
		if (engineType == AircraftType.TurboProp) {
			totalThrustGenerated = 0;totalConsumptionRate = 0;
			foreach (SilantroTurboProp turboProp in turboprop) {
				//CAPTURE ENGINE THRUST
				totalThrustGenerated += turboProp.PropellerThrust;
				//
				totalConsumptionRate += turboProp.actualConsumptionrate;
			}
		}
		//
		currentWeight = emptyWeight + fuelsystem.TotalFuelRemaining;
		if(aircraft != null)aircraft.mass = currentWeight;
		if (currentWeight > maximumWeight) {
			Debug.Log ("Aircraft is too Heavy for takeoff, Dump some Fuel...");
		}
		//
		//
		wingLoading = currentWeight/wingArea;
		thrustToWeightRatio = totalThrustGenerated / currentWeight;
		gearHelper.availablePushForce = totalThrustGenerated;
		//
		if (!captured && dragSystem.useDrag) {
			if (aircraft.velocity.magnitude > 10f ) {
				//CALCULATE CD
				maximumPossibleEngineThrust = totalThrustGenerated;
				dragCoefficient = ((2*maximumPossibleEngineThrust)/(fuselargeArea * (dragSystem.maximumSpeed/1.944f)  * (dragSystem.maximumSpeed/1.944f) * datalog.airDensity));
				gearHelper.baseDragCoefficient = dragCoefficient;
				dragCoefficient += gearHelper.gearOpenDragCoefficient;
				dragCoefficient += extraDrag;
				captured = true;
			}
		}
		//

		//
		//

		fuelsystem.totalConsumptionRate = totalConsumptionRate;
	}

	void Update()
	{
		float v = aircraft.velocity.sqrMagnitude;
		dragForce = (0.5f* -dragCoefficient * fuselargeArea* v * datalog.airDensity);
		Vector3 pint = transform.position + transform.forward;
		Vector3 drag = aircraft.velocity.normalized* dragForce;//Debug.Log (drag);
		aircraft.AddForceAtPosition (drag,pint,ForceMode.Force);
		//
	}
	//
	//
	//
	#if UNITY_EDITOR
	void OnDrawGizmos()
	{
		//
		if (aircraft == null && gameObject.GetComponent<Rigidbody> () ) {
			 aircraft = GetComponent<Rigidbody> ();
			//

		}
		Collider col = gameObject.GetComponent<CapsuleCollider>();
		if (col != null) {
			centerPosition = col.bounds.center;
			//
			if (dragSystem != null) {
				float halfLength = dragSystem.averagefuselargeLength / 2f;
				float radius = dragSystem.averagefuselargeDiameter / 2f;
			
			//
			Vector3 front = centerPosition + new Vector3(0,0,-halfLength);
			Vector3 back = centerPosition + new Vector3 (0, 0, halfLength);
			//
			if (dragSystem.showBounds&& this.transform.eulerAngles.x < 5f && this.transform.eulerAngles.x > -5f) {
				Handles.color = Color.white;
				Handles.DrawLine (front,back);
				//
				Handles.color = Color.yellow;
				Handles.DrawSolidDisc (front,  aircraft.transform.forward, radius);
				Handles.DrawSolidDisc (back, aircraft.transform.forward, radius);
			}
			//
			}

		}
	}
	//
	#endif
}
