//
//Property of Oyedoyin Dada
//cc dadaoyedoyin@gmail.com
//
//
//using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//
//[AddComponentMenu("Oyedoyin/Aerofoil System/Aerofoil Control")]
[RequireComponent (typeof(SilantroAerofoil))]
public class SilantroControlSurface : MonoBehaviour {
	//


	//Aerofoil Type 
	//[Header("Aerofoil Type")]
	public enum SurfaceType
	{
		Elevator,
		Rudder,
		Aileron
	}
	public SurfaceType surfaceType = SurfaceType.Aileron;

	[Header(" ")]//SPACE
	//
	[Header("Control Surface Model")]
	public GameObject SurfaceModel;
	//
	[Header("Rotation Axis")]
	public bool negativeRotation = false;
	public enum RotationAxis
	{
		X,
		Y,
		Z
	}
	public RotationAxis rotationAxis = RotationAxis.X;
	Vector3 axisRotation;
	//

	[Header(" ")]//SPACE
	[Header("Surface Deflection Settings")]
	public float maximumDeflection = 30;
	[SerializeField][AngleOnly] public float CurrentDeflection = 0.0f;
	public bool negativeDeflection = false;
	//

	[Header(" ")]//SPACE
	[Header("Surface Size Settings")]
	[Range(0,100)]
	public float BottomHingeDistance = 2.5f;
	[SerializeField][LengthOnly]public float rootHeight;
	[Range(0,100)]
	public float TopHingeDistance = 2.5f;
	[SerializeField][LengthOnly]public float tipHeight;
	//
	[Header(" ")]//SPACE
	[Header("Surface Properties")]
	public bool[] AffectedAerofoilSubdivisions;
	//
	 string ControlInput;
	AnimationCurve controlCurve;

	//
	[Header("Manual Control Settings")]
	public bool ManualControl = false;
	[Range(-1f,1f)]
	public float controlInput = 0f;
	//
	//
	[HideInInspector]public float rootChord;
	[HideInInspector]public float trailChord;
	//

	private Vector3 surfaceRootHingePos = Vector3.zero;
	private Vector3 SurfaceTipHingePos = Vector3.zero;
	//
	private Quaternion InitialModelRotation = Quaternion.identity;
	//


	//
	float bthingeDistance ;
	float tphingeDistance;
	//
	float input;
	//
	SilantroControls controlBoard;
	void Awake () {
		controlBoard = GameObject.FindGameObjectWithTag ("GameController").GetComponent<SilantroControls> ();
		if (controlBoard == null) {
			Debug.LogError ("Control Board is missing, Place Control Board in scene and restart!");
		}
		//
		if (surfaceType == SurfaceType.Aileron) 
		{
			ControlInput = controlBoard.Aileron;
		} 
		else if (surfaceType == SurfaceType.Elevator) 
		{
			ControlInput = controlBoard.Elevator;
		} 
		else if (surfaceType == SurfaceType.Rudder) 
		{
			ControlInput = controlBoard.Rudder;
		}
	}
	public void Start () 
	{
		if (negativeRotation) {
			if (rotationAxis == RotationAxis.X) {
				axisRotation = new Vector3 (-1, 0, 0);
			} else if (rotationAxis == RotationAxis.Y) {
				axisRotation = new Vector3 (0, -1, 0);
			} else if (rotationAxis == RotationAxis.Z) {
				axisRotation = new Vector3 (0, 0, -1);
			}
		} else {
			if (rotationAxis == RotationAxis.X) {
				axisRotation = new Vector3 (1, 0, 0);
			} else if (rotationAxis == RotationAxis.Y) {
				axisRotation = new Vector3 (0, 1, 0);
			} else if (rotationAxis == RotationAxis.Z) {
				axisRotation = new Vector3 (0, 0, 1);
			}
		}

		axisRotation.Normalize();

		if ( null != SurfaceModel )
		{
			InitialModelRotation = SurfaceModel.transform.localRotation;
		}
	}
	//
	// Update is called once per frame
	public void Update () 
	{
		if (!ManualControl)
		{ 
			 input = Input.GetAxis (ControlInput);
			if (negativeDeflection) {
				input *= -1f;
			}
		} 
		else
		{
			input = controlInput;

		}
			
		float curveValue = controlBoard.controlCurve.Evaluate( Mathf.Abs(input) );
			curveValue *= Mathf.Sign( input );
			CurrentDeflection = curveValue * maximumDeflection;

		//Apply rotation to model.	
		if ( null != SurfaceModel )
		{
			SurfaceModel.transform.localRotation = InitialModelRotation;
			SurfaceModel.transform.Rotate( axisRotation, CurrentDeflection );
		}
	}
	//
	public void CalculateAerofoilStructure( int SectionIndex, ref Vector3 PointA, ref Vector3 PointB, ref Vector3 PointC, ref Vector3 PointD)
	{
		 bthingeDistance = BottomHingeDistance / 100f;
		 tphingeDistance = TopHingeDistance / 100f;


		if ( SectionIndex < AffectedAerofoilSubdivisions.Length )
		{
			if ( AffectedAerofoilSubdivisions[SectionIndex]==true)
			{
				
				surfaceRootHingePos = PointD + ( ( PointA - PointD ) * bthingeDistance );
				SurfaceTipHingePos = PointC + ( ( PointB - PointC ) * tphingeDistance );
				Vector3 aileronHinge = SurfaceTipHingePos - surfaceRootHingePos;

				Vector3 rootAileronAngle = PointD - surfaceRootHingePos;
				Vector3 tipAileronAngle = PointC - SurfaceTipHingePos;

				//Deflect surface.
				Quaternion hingeRotation = Quaternion.AngleAxis( CurrentDeflection, aileronHinge.normalized);
				rootAileronAngle = hingeRotation * rootAileronAngle;
				tipAileronAngle = hingeRotation * tipAileronAngle;

			//wing chord line.
				PointD = surfaceRootHingePos + rootAileronAngle;
				PointC = SurfaceTipHingePos + tipAileronAngle;
			}
		}
	}

	//
	//
	public void OnDrawGizmos() 
	{
		ClampEditorValues();

	}


	private void ClampEditorValues()
	{
		//

		maximumDeflection = Mathf.Clamp( maximumDeflection, 0.0f, 90.0f );
		BottomHingeDistance = Mathf.Clamp( BottomHingeDistance, 0.0f, 100.0f );
		TopHingeDistance = Mathf.Clamp( TopHingeDistance, 0.0f, 100.0f );
	
		SilantroAerofoil aerofoil = gameObject.GetComponent<SilantroAerofoil>();
		if (null!= aerofoil )
		{
			tipHeight = trailChord * TopHingeDistance / 100f;
			rootHeight = rootChord * BottomHingeDistance / 100f;
			if ( (null==AffectedAerofoilSubdivisions) || (aerofoil.AerofoilSubdivisions != AffectedAerofoilSubdivisions.Length) )
			{
				AffectedAerofoilSubdivisions = new bool[aerofoil.AerofoilSubdivisions];
			}
		}
	}

}
