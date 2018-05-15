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
//[AddComponentMenu("Oyedoyin/Aerofoil System/Aerofoil")]
//[MenuItem("Oyedoyin")]
[RequireComponent(typeof(BoxCollider))]
public class SilantroAerofoil : MonoBehaviour {
	//
	//Aerofoil Type 
	//[Header("Aerofoil Type")]
	public enum AerofoilType
	{
		Wing,
		Tail
	}
	public AerofoilType aerofoilType = AerofoilType.Wing;
	//
	[Header(" ")]//SPACE
	//Base Airfoil to calculate CD and CL
	[Header("Airfoil Component")]
	public SilantroAirfoil airfoil;
	[SerializeField][SilantroShowAttribute]public float liftCoefficent;
	[SerializeField][SilantroShowAttribute]public float dragCoefficient;
	//

	// Aerofoil Sweep
	[Header(" ")]//SPACE
	[Header("Aerofoil Sweep")]
	public SweepDirection sweepDirection = SweepDirection.Unswept;
	[Range(0,90)]
	public float aerofoilSweepAngle;float sweepangle;
	//
	//SWEP DIRECTION OF WING
	public enum SweepDirection
	{
		Unswept,
		Forward,
		Backward
	}

	//

	// Aerofoil Twist
	[Header("Aerofoil Twist")]
	public TwistDirection twistDirection = TwistDirection.Untwisted;
	//
	[Range(0,90)]
	public float twistAngle;
	//
	//
	public enum TwistDirection
	{
		Untwisted,
		Upwards,
		Downwards
	}
	//
	//
	[Header("Aerofoil Tip Width %")]
	[Range(0,100)]
	public float AerofoilTipWidth = 0.0f;
	//
	[Header("Aerofoil Structural Subdivisions")]
	[Range(0,15)]
	public int AerofoilSubdivisions;
	//
	[Header(" ")]//SPACE
	[Header("Aerofoil Dimensions")]
	[SerializeField][LengthOnly]public float rootChord;
	[SerializeField][LengthOnly]public float tipChord;
	[SerializeField][LengthOnly]float leadingEdgeLength;
	[SerializeField][LengthOnly]float trailingEdgeLength;
	//

	public bool canBeControlled = true;
	//
	private Vector3 aerofoilRootLeadingEdge = Vector3.zero;
	private Vector3 aerofoilRootTrailingEdge = Vector3.zero;
	private Vector3 aerofoilTipLeadingEdge = Vector3.zero;
	private Vector3 aerofoilTipTrailingEdge = Vector3.zero;
	//
	[HideInInspector]public SilantroControlSurface controlSurface;
	[HideInInspector]public bool usesFlap;
	[HideInInspector]public SilantroFlap flap;
	Rigidbody AirplaneBody;

	//
	Vector3 aerodynamicCenter;
	//
	[Header(" ")]//SPACE
	[Header("Display Properties")]
	[SerializeField][AreaOnly]public float aerofoilArea;
	[SerializeField][AngleOnly]public float angleOfAttack;
	[SerializeField][DensityOnly]public float airDensity = 1.225f;
	[SerializeField][SpeedOnly]public float trueAirSpeed;
	//[SerializeField][SilantroShowAttribute]public float AircraftVelocity;
	//
	[Header(" ")]//SPACE
	[Header("Forces Generated")]
	[SerializeField][ForceOnly]public float TotalDrag;
	[SerializeField][ForceOnly]public float TotalLift;
	//
	SilantroInstrumentation instrumentation;
	//
	float localscale;
	float actualTwist;
	Vector3 relativeWind;
	//
	private BoxCollider aerofoilCollider;

	private Vector3 aerofoilRootLiftPosition = Vector3.zero;
	private Vector3 aerofoilTipLiftPosition = Vector3.zero;
	private float aerofoilLiftLineChordPosition = 0.75f;
	// Use this for initialization
	void Start () {
		aerofoilCollider = (BoxCollider)gameObject.GetComponent<Collider>();
		AirplaneBody = transform.root.gameObject.GetComponent<Rigidbody> ();
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
	//
	void FixedUpdate () 
	{
		//
		//float debugLineScale = 1.0f / 30.0f;
		//
		CalculateAerofoilStructure ();
		//

		for (int i = 0; i < AerofoilSubdivisions; i++) {		
			//
			//Trailing Edge of Aerofoil
			Vector3 trailingForwardChord = aerofoilRootLeadingEdge + ((aerofoilTipLeadingEdge -aerofoilRootLeadingEdge) * (float)i / (float)AerofoilSubdivisions);
			Vector3 trailingBackChord = aerofoilRootLeadingEdge + ((aerofoilTipLeadingEdge - aerofoilRootLeadingEdge) * (float)(i + 1) / (float)AerofoilSubdivisions);
			//Root Edge of Aerofoil
			Vector3 rootForwardChord = aerofoilRootTrailingEdge + ((aerofoilTipTrailingEdge - aerofoilRootTrailingEdge) * (float)(i + 1) / (float)AerofoilSubdivisions);
			Vector3 rootBackChord = aerofoilRootTrailingEdge + ((aerofoilTipTrailingEdge - aerofoilRootTrailingEdge) * (float)i / (float)AerofoilSubdivisions);
			//
			//
			//SEND DATA TO CONTROL SURFACE AND FLAP// SLAT IN FULL VERSION
			if ( null != controlSurface )
			{
				controlSurface.CalculateAerofoilStructure( i, ref trailingForwardChord, ref trailingBackChord, ref rootForwardChord, ref rootBackChord);

			}
			if (flap != null) {
				flap.CalculateAerofoilStructure (i, ref trailingForwardChord, ref trailingBackChord, ref rootForwardChord, ref rootBackChord);
				
			}
			//
			//
			Vector3 sectionRootLiftPosition = rootBackChord + ( ( trailingForwardChord - rootBackChord ) * aerofoilLiftLineChordPosition );
			Vector3 sectionTipLiftPosition = rootForwardChord + ( ( trailingBackChord - rootForwardChord ) * aerofoilLiftLineChordPosition );

			//
			aerodynamicCenter = sectionRootLiftPosition + ( ( sectionTipLiftPosition - sectionRootLiftPosition ) * 0.5f );
			//
			//
			Vector3 chordLine = ( trailingForwardChord + ((trailingBackChord-trailingForwardChord) * 0.5f )) -  ( rootBackChord + ((rootForwardChord-rootBackChord) * 0.5f) );
			float chordLength = chordLine.magnitude;
			chordLine.Normalize();
			//
			//
			if (AirplaneBody != null) {
				relativeWind = -AirplaneBody.velocity;
			
			//
			//
			Vector3 fromCOMToAerodynamicCenter = aerodynamicCenter -  AirplaneBody.worldCenterOfMass;
			Vector3 angularVelocity = AirplaneBody.angularVelocity;

			Vector3 localRelativeWind = Vector3.Cross( angularVelocity.normalized, fromCOMToAerodynamicCenter.normalized );
			localRelativeWind *= -((angularVelocity.magnitude) * fromCOMToAerodynamicCenter.magnitude);
			//Apply
			relativeWind += localRelativeWind;
			Vector3 correction = gameObject.transform.right;
			float perpChordDotRelativeWind = Vector3.Dot( correction, relativeWind );
			correction *= perpChordDotRelativeWind;
			relativeWind -= correction;
			//
			//Find the angle of attack.	
			Vector3 relativeWindNormalized = relativeWind.normalized;
			angleOfAttack = Vector3.Dot(chordLine, -relativeWindNormalized);
			angleOfAttack = Mathf.Clamp( angleOfAttack, -1.0f, 1.0f );
			angleOfAttack = Mathf.Acos(angleOfAttack);
			angleOfAttack *= Mathf.Rad2Deg;
			//
			Vector3 up = Vector3.Cross( chordLine, (sectionTipLiftPosition-sectionRootLiftPosition).normalized );
			up.Normalize();

			if ( transform.localScale.x < 0.0f )
			{
				up=-up;
			}

			float yAxisDotRelativeWind = Vector3.Dot( up, relativeWindNormalized );		
			if ( yAxisDotRelativeWind < 0.0f )
			{
				angleOfAttack = -angleOfAttack;
			}
			//
			float totalLift = 0.0f;
			float totalDrag = 0.0f;
			float cM = 0.0f;
			//
			if (instrumentation != null) {
				airDensity=instrumentation.airDensity;
			}
			//
				if ( null != airfoil )
			{
				
					liftCoefficent = airfoil.CL.Evaluate(angleOfAttack);	

				float AerofoilArea = CalculateWingArea( trailingForwardChord, trailingBackChord, rootForwardChord, rootBackChord );
				float v = relativeWind.magnitude;	
				totalLift = liftCoefficent * AerofoilArea * 0.5f * airDensity * (v*v);

					dragCoefficient	 = airfoil.CD.Evaluate(angleOfAttack);
				totalDrag = 0.5f * dragCoefficient * airDensity * (v*v) * AerofoilArea;
					cM = airfoil.CM.Evaluate(angleOfAttack);
				//

			}
			//AircraftVelocity = connections.AirplaneBody.velocity.magnitude;
			trueAirSpeed = relativeWind.magnitude;
			TotalLift = totalLift;
			TotalDrag = totalDrag;
			//
			Vector3 liftForce = Vector3.Cross( gameObject.transform.right, relativeWind );
			liftForce.Normalize();
			liftForce *= totalLift;
			Vector3 dragForce = relativeWind;
			dragForce.Normalize();
			dragForce *= totalDrag;
			Vector3 liftDragPoint = aerodynamicCenter;

			//Find wing pitching moment...
			float wingPitchingMoment = cM * chordLength * ( 0.5f *  airDensity * (relativeWind.magnitude*relativeWind.magnitude) ) * CalculateWingArea( trailingForwardChord, trailingBackChord, rootForwardChord, rootBackChord );
			Vector3 pitchAxis = Vector3.Cross( chordLine, liftForce.normalized );
			pitchAxis.Normalize();
			pitchAxis *= wingPitchingMoment;

			//Apply forces.
			
				AirplaneBody.AddForceAtPosition (liftForce, liftDragPoint, ForceMode.Force);
				AirplaneBody.AddForceAtPosition (dragForce, liftDragPoint, ForceMode.Force);		
				AirplaneBody.AddTorque (pitchAxis, ForceMode.Force);
			} else {
				Debug.Log ("Parent rigidbody is missing!!!!");
			}
			//
	}
}

	public void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawSphere (aerodynamicCenter, 0.15f);

		aerofoilCollider = (BoxCollider)gameObject.GetComponent<Collider> ();
		if (null != aerofoilCollider) {
			
			aerofoilCollider.size = new Vector3 (1.0f, 0.1f, 1.0f);
			//
			//CALCULATE AEROFOIL STRUCTURE
			CalculateAerofoilStructure ();

			Gizmos.color = Color.yellow;
			Gizmos.DrawLine (aerofoilRootLeadingEdge, aerofoilTipLeadingEdge);

			Gizmos.color = Color.red;
			Gizmos.DrawLine (aerofoilTipTrailingEdge, aerofoilRootTrailingEdge);

			Gizmos.color = Color.yellow;
			Gizmos.DrawLine (aerofoilRootTrailingEdge, aerofoilRootLeadingEdge);
			Gizmos.DrawLine (aerofoilTipLeadingEdge, aerofoilTipTrailingEdge);
			//
			Gizmos.color = Color.red;
			Gizmos.DrawSphere(aerofoilRootLeadingEdge,0.07f);
			Gizmos.color = Color.blue;
			Gizmos.DrawSphere(aerofoilRootTrailingEdge,0.07f);
			Gizmos.color = Color.grey;
			Gizmos.DrawSphere(aerofoilTipLeadingEdge,0.07f);
			Gizmos.color = Color.green;
			Gizmos.DrawSphere(aerofoilTipTrailingEdge,0.07f);

			rootChord = Vector3.Distance(aerofoilRootLeadingEdge,aerofoilRootTrailingEdge);
			tipChord = Vector3.Distance (aerofoilTipLeadingEdge, aerofoilTipTrailingEdge);
			leadingEdgeLength = Vector3.Distance (aerofoilTipLeadingEdge, aerofoilRootLeadingEdge);
			trailingEdgeLength = Vector3.Distance (aerofoilTipTrailingEdge, aerofoilRootTrailingEdge);
			//

			//Sections.
			Gizmos.color = Color.yellow;
			for (int i = 0; i < AerofoilSubdivisions; i++) {
				Vector3 sectionStart = aerofoilRootTrailingEdge + ((aerofoilTipTrailingEdge - aerofoilRootTrailingEdge) * (float)i / (float)AerofoilSubdivisions);
				Vector3 sectionEnd = aerofoilRootLeadingEdge + ((aerofoilTipLeadingEdge - aerofoilRootLeadingEdge) * (float)i / (float)AerofoilSubdivisions);
				Gizmos.DrawLine (sectionStart, sectionEnd);

			}
			//
			//Lift line.
			Gizmos.color = Color.yellow;
			Gizmos.DrawLine (aerofoilRootLiftPosition, aerofoilTipLiftPosition);

			//
			//Control Hinge'
			if (canBeControlled && controlSurface != null) {
				float rootHingeOffset = controlSurface.BottomHingeDistance / 100f;
				float tipHingeOffset = controlSurface.TopHingeDistance / 100f; 

				Vector3 aerofoilRootAileronHingePos = aerofoilRootTrailingEdge + ((aerofoilRootLeadingEdge - aerofoilRootTrailingEdge) * rootHingeOffset);
				Vector3 aerofoilTipAileronHingePos = aerofoilTipTrailingEdge + ((aerofoilTipLeadingEdge - aerofoilTipTrailingEdge) * tipHingeOffset);

				if (null != controlSurface.AffectedAerofoilSubdivisions) {
					for (int i = 0; i < controlSurface.AffectedAerofoilSubdivisions.Length; i++) {
						if (controlSurface.AffectedAerofoilSubdivisions [i] == true) {
							Vector3 hingeLeft = aerofoilRootAileronHingePos + ((aerofoilTipAileronHingePos - aerofoilRootAileronHingePos) * ((float)i / (float)controlSurface.AffectedAerofoilSubdivisions.Length));
							Vector3 hingeRight = aerofoilRootAileronHingePos + ((aerofoilTipAileronHingePos - aerofoilRootAileronHingePos) * ((float)(i + 1) / (float)controlSurface.AffectedAerofoilSubdivisions.Length));

							Vector3 backLeft = aerofoilRootTrailingEdge + ((aerofoilTipTrailingEdge - aerofoilRootTrailingEdge) * ((float)i / (float)controlSurface.AffectedAerofoilSubdivisions.Length));
							Vector3 backRight = aerofoilRootTrailingEdge + ((aerofoilTipTrailingEdge - aerofoilRootTrailingEdge) * ((float)(i + 1) / (float)controlSurface.AffectedAerofoilSubdivisions.Length));
							//
							controlSurface.rootChord = rootChord;
							controlSurface.trailChord = tipChord;
							//
							Gizmos.color = Color.gray;
							Gizmos.DrawLine( hingeLeft, backRight );
							Vector3[] vert = new Vector3[4];
							vert [0] = hingeLeft;
							vert [1] = hingeRight;
							vert [2] = backLeft;
							vert [3] = backRight;
							#if UNITY_EDITOR
							DrawControlHandles (vert,Color.red);
							#endif
						}
					}
				}
			}
				//
				//
				//Flap Control
			if (canBeControlled && flap != null) {
				float rootHingeOffset = flap.BottomHingeDistance/100f;
				float tipHingeOffset = flap.TopHingeDistance/100f; 

					Vector3 aerofoilRootAileronHingePos = aerofoilRootTrailingEdge + ( ( aerofoilRootLeadingEdge - aerofoilRootTrailingEdge ) * rootHingeOffset );
					Vector3 aerofoilTipAileronHingePos = aerofoilTipTrailingEdge + ( ( aerofoilTipLeadingEdge - aerofoilTipTrailingEdge ) * tipHingeOffset );

				if ( null != flap.AffectedAerofoilSubdivisions ) 
					{
					for ( int i=0; i<flap.AffectedAerofoilSubdivisions.Length; i++ )
						{
						if ( flap.AffectedAerofoilSubdivisions[i] == true )
							{
							Vector3 hingeLeft = aerofoilRootAileronHingePos + ( (aerofoilTipAileronHingePos - aerofoilRootAileronHingePos ) * ((float)i / (float)flap.AffectedAerofoilSubdivisions.Length) );
							Vector3 hingeRight = aerofoilRootAileronHingePos + ( (aerofoilTipAileronHingePos - aerofoilRootAileronHingePos ) * ((float)(i+1) / (float)flap.AffectedAerofoilSubdivisions.Length) );

							Vector3 backLeft = aerofoilRootTrailingEdge + ( (aerofoilTipTrailingEdge - aerofoilRootTrailingEdge ) * ((float)i / (float)flap.AffectedAerofoilSubdivisions.Length) );
							Vector3 backRight = aerofoilRootTrailingEdge + ( (aerofoilTipTrailingEdge - aerofoilRootTrailingEdge ) * ((float)(i+1) / (float)flap.AffectedAerofoilSubdivisions.Length) );
							//
							flap.rootChord = rootChord;
							flap.trailChord = tipChord;
							//connections.controlSurface.rootHeight = connections.controlSurface.BottomHingeDistance * rootChord;
							//Draw Lines to denote affected area
							Gizmos.color = Color.gray;
								Gizmos.DrawLine( hingeLeft, backRight );
							//
							Vector3[] vert = new Vector3[4];
							vert [0] = hingeLeft;
							vert [1] = hingeRight;
							vert [2] = backLeft;
							vert [3] = backRight;
							#if UNITY_EDITOR
							DrawControlHandles (vert,Color.blue);
							#endif
							}
						}
					}
			}
			//
			//
			//Slat Control
		}
	}

	//
	void CalculateAerofoilStructure()
	{
		//Calculate root and tip center points.
			Vector3 wingRootCenter = transform.position - ( transform.right * (transform.localScale.x * 0.5f) );
			Vector3 wingTipCenter = transform.position + ( transform.right * (transform.localScale.x * 0.5f) );
	

		//
		if (sweepDirection == SweepDirection.Unswept) {
			sweepangle = aerofoilSweepAngle = 0;
		}
		else if (sweepDirection == SweepDirection.Forward)
		{
			sweepangle = aerofoilSweepAngle;
		} else if (sweepDirection == SweepDirection.Backward)
		{
			sweepangle = -aerofoilSweepAngle;
		}
		//
		localscale = transform.localScale.magnitude;
		//Debug.Log (transform.localScale.magnitude);
		//
		wingTipCenter += transform.forward * (sweepangle/90) *localscale;

			//Calculate corners.
			aerofoilRootLeadingEdge = wingRootCenter + ( transform.forward * (transform.localScale.z * 0.5f) );
		aerofoilRootTrailingEdge = wingRootCenter - ( transform.forward * (transform.localScale.z * 0.5f) );
		aerofoilTipLeadingEdge = wingTipCenter + ( transform.forward * ((transform.localScale.z * 0.5f) *AerofoilTipWidth/100f) );
		aerofoilTipTrailingEdge = wingTipCenter - ( transform.forward * ((transform.localScale.z * 0.5f) * AerofoilTipWidth /100f) );


			//Tweak tip corners based on the angle between them.
		Vector3 tipTrailingEdgeToTipLeadingEdge = aerofoilTipLeadingEdge - aerofoilTipTrailingEdge;
		//
	
		if (twistDirection == TwistDirection.Untwisted) {
			actualTwist = twistAngle = 0;
		} else if (twistDirection == TwistDirection.Downwards) {
			actualTwist = twistAngle;
		} else if (twistDirection == TwistDirection.Upwards) {
			actualTwist = -twistAngle;
		}
		//
		Quaternion rotation = Quaternion.AngleAxis( actualTwist, transform.rotation * new Vector3( 1.0f, 0.0f, 0.0f ));
			tipTrailingEdgeToTipLeadingEdge = rotation * tipTrailingEdgeToTipLeadingEdge;
		aerofoilTipTrailingEdge = wingTipCenter - (tipTrailingEdgeToTipLeadingEdge * 0.5f);
		aerofoilTipLeadingEdge = wingTipCenter + (tipTrailingEdgeToTipLeadingEdge * 0.5f);

		aerofoilRootLiftPosition = aerofoilRootTrailingEdge + ( ( aerofoilRootLeadingEdge - aerofoilRootTrailingEdge ) * aerofoilLiftLineChordPosition );
		aerofoilTipLiftPosition = aerofoilTipTrailingEdge + ( ( aerofoilTipLeadingEdge - aerofoilTipTrailingEdge ) * aerofoilLiftLineChordPosition );

			//Calculate wing area.
		aerofoilArea = CalculateWingArea( aerofoilRootLeadingEdge, aerofoilTipLeadingEdge, aerofoilTipTrailingEdge, aerofoilRootTrailingEdge );

	}
	//
	//CALCULATE RECTANGULAR AREA
	private float CalculateWingArea( Vector3 pointA, Vector3 pointB, Vector3 pointC, Vector3 pointD )
	{
		float ab = (pointB - pointA).magnitude;
		float bc = (pointC - pointB).magnitude;
		float cd = (pointD - pointC).magnitude;
		float da = (pointA - pointD).magnitude;

		float s = ( ab + bc + cd + da ) * 0.5f;
		float squareArea = (s-ab) * (s-bc) * (s-cd) * (s-da);
		float area = Mathf.Sqrt( squareArea );
		//
		aerofoilArea = area;
		return area;

	}
	//
	#if UNITY_EDITOR
	void DrawControlHandles(Vector3[] vectar,Color colar)
	{
		Handles.DrawSolidRectangleWithOutline (vectar,colar,colar);
	}
	#endif
}

