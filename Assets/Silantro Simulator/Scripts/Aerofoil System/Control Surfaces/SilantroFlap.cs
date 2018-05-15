//
//Property of Oyedoyin Dada
//cc dadaoyedoyin@gmail.com
//
//
//using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//
//[AddComponentMenu("Oyedoyin/Aerofoil System/Flap Control")]
[RequireComponent (typeof(SilantroAerofoil))]
public class SilantroFlap : MonoBehaviour {
	//
	//Aerofoil Type 
	//[Header("Aerofoil Type")]
	//public SurfaceType surfaceType = SurfaceType.Flap;
	//
	//AFFECTED SECTIONS
	[Header("Surface Properties")]
	public bool[] AffectedAerofoilSubdivisions;

	[Header(" ")]//SPACE
	[Header("Flap Model")]
	public GameObject flapModel;
	//
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
	//
	//
	[Header(" ")]//SPACE
	[Header("Flap Deflection Settings")]//SPACE
	public float maximumDeflection = 30;
	[SerializeField][AngleOnly] public float CurrentDeflection = 0.0f;
	public bool negativeDeflection = false;
	//
	//
	[Header(" ")]//SPACE
	[Header("Geometry Settings (% of Aerofoil Width)")]//SPACE
	[Range(0,100)]
	public float BottomHingeDistance = 2.5f;
	[SerializeField][LengthOnly]public float FlapRootHeight;
	[Range(0,100)]
	public float TopHingeDistance = 2.5f;
	[SerializeField][LengthOnly]public float FlapTipHeight;
	//
	[HideInInspector]public float rootChord;
	[HideInInspector]public float trailChord;
	//
	Vector3 axisRotation;
	//
	string ControlInput;
	AnimationCurve controlCurve;

	//
	[Header(" ")]//SPACE
	[Header("Manual Control Settings")]
	public bool ManualControl = false;
	[Range(-1f,0f)]
	public float controlInput = 0f;
	//
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
	[Range(-1f,0f)]
	float input;
	//
	SilantroControls controlBoard;
	void Awake () {
		controlBoard = GameObject.FindGameObjectWithTag ("GameController").GetComponent<SilantroControls> ();
		if (controlBoard == null) {
			Debug.LogError ("Control Board is missing, Place Control Board in scene and restart!");
		}
		//
		ControlInput = controlBoard.FlapControl;
		controlCurve = controlBoard.controlCurve;
	}
	public void Start () 
	{
		if (negativeRotation) {
			if (rotationAxis == RotationAxis.X) {
				axisRotation = new Vector3 (-1, 0, 0);
			} else if (rotationAxis == RotationAxis.X) {
				axisRotation = new Vector3 (0, -1, 0);
			} else if (rotationAxis == RotationAxis.X) {
				axisRotation = new Vector3 (0, 0, -1);
			}
		} else {
			if (rotationAxis == RotationAxis.X) {
				axisRotation = new Vector3 (1, 0, 0);
			} else if (rotationAxis == RotationAxis.X) {
				axisRotation = new Vector3 (0, 1, 0);
			} else if (rotationAxis == RotationAxis.X) {
				axisRotation = new Vector3 (0, 0, 1);
			}
		}

		axisRotation.Normalize();

		if ( null != flapModel )
		{
			InitialModelRotation = flapModel.transform.localRotation;
		}
	}
	//
	//
	// Update is called once per frame
	public void Update () 
	{
		if (!ManualControl)
		{ 
			input = Input.GetAxis (ControlInput);
			input = Mathf.Clamp (input, -1f, 0f);
			if (negativeDeflection) {
				input *= -1f;
			}
		} 
		else
		{
			input = controlInput;

		}
		//CurrentDeflection = Mathf.Clamp (CurrentDeflection, 0f, maximumDeflection);
		float curveValue = controlCurve.Evaluate( Mathf.Abs(input) );
		curveValue *= Mathf.Sign( input );
		CurrentDeflection = curveValue * maximumDeflection;

		//Apply rotation to model.	
		if ( null != flapModel )
		{
			flapModel.transform.localRotation = InitialModelRotation;
			flapModel.transform.Rotate( axisRotation, CurrentDeflection );
		}
	}
	//
	//
	//
	//
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

				Quaternion hingeRotation = Quaternion.AngleAxis( CurrentDeflection, aileronHinge.normalized);
				rootAileronAngle = hingeRotation * rootAileronAngle;
				tipAileronAngle = hingeRotation * tipAileronAngle;
	
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
		maximumDeflection = Mathf.Clamp( maximumDeflection, 0.0f, 90.0f );
		BottomHingeDistance = Mathf.Clamp( BottomHingeDistance, 0.0f, 100.0f );
		TopHingeDistance = Mathf.Clamp( TopHingeDistance, 0.0f, 100.0f );

		SilantroAerofoil aerofoil = gameObject.GetComponent<SilantroAerofoil>();
		if (null!= aerofoil )
		{
			FlapTipHeight = trailChord * TopHingeDistance / 100f;
			FlapRootHeight = rootChord * BottomHingeDistance / 100f;
			if ( (null==AffectedAerofoilSubdivisions) || (aerofoil.AerofoilSubdivisions != AffectedAerofoilSubdivisions.Length) )
			{
				AffectedAerofoilSubdivisions = new bool[aerofoil.AerofoilSubdivisions];
			}
		}
	}

}
