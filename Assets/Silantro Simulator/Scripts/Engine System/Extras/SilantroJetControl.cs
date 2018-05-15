//
//Property of Oyedoyin Dada
//cc dadaoyedoyin@gmail.com
//
//
//using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class SilantroJetControl : MonoBehaviour {
	//
	[Header("Engine Connection")]
	public GameObject Engine;
	Transform engineThruster;
	//
	Vector3 initialPosition;
	Quaternion initialRotation;
	//
	[Header("Thrust Vectoring Control")]
	[Range(-1,1)]
	public float thrustDirection;
	//
	[Header("Vectoring Values")]
	public float maximumVerticalAngle = 30f;
	//public float rotateRate = 10f;
	float verticalThrustAngle;
	//float horizontalAngle;
	//
	[Header("Engine Nozzle Model")]
	public GameObject engineNozzleModel;
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
	private Quaternion InitialModelRotation = Quaternion.identity;
	//

	//
	//
	void Start () {
		if (Engine.GetComponent<SilantroTurboFan> ()) 
		{
			engineThruster = Engine.GetComponent<SilantroTurboFan> ().Thruster;
		}
		else if (Engine.GetComponent<SilantroTurboJet> ()) 
		{
			engineThruster = Engine.GetComponent<SilantroTurboJet> ().Thruster;
		}
		//
		initialPosition = engineThruster.localPosition;
		initialRotation = engineThruster.localRotation;
		//
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

		if ( null != engineNozzleModel )
		{
			InitialModelRotation = engineNozzleModel.transform.localRotation;
		}
	}
	
	// Update is called once per frame
	void Update () {
		verticalThrustAngle = thrustDirection * maximumVerticalAngle;
		//horizontalAngle = thrustHorizontalDirection * maximumHorizontalAngle;
		//
		engineThruster.localRotation = Quaternion.Euler(verticalThrustAngle,0f,0f);
		//Apply rotation to model.	
		if ( null != engineNozzleModel )
		{
			engineNozzleModel.transform.localRotation = InitialModelRotation;
			engineNozzleModel.transform.Rotate( axisRotation, verticalThrustAngle );
		}
	}
	//
	#if UNITY_EDITOR
	void OnDrawGizmos()
	{

		if (engineThruster != null) {//Handles.DrawLine (initialPosition, (initialPosition * 2f));
			Handles.color = Color.blue;
			Handles.DrawWireArc (initialPosition, engineThruster.transform.forward, initialPosition, verticalThrustAngle, 20f);
		}
	}
	//
	#endif
}
