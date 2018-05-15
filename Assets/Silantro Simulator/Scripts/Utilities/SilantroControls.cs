//
//Property of Oyedoyin Dada
//cc dadaoyedoyin@gmail.com
//
//
//using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SilantroControls : MonoBehaviour {
	//
	[Header("Aircraft Control Keys")]
	[Header("")]
	[Header("Buttons")]
	public KeyCode engineStart = KeyCode.F1;
	public KeyCode engineShutdown = KeyCode.F2;
	public KeyCode engineThrottleUp = KeyCode.Alpha1;
	public KeyCode engineThrottleDown = KeyCode.Alpha2;
	public KeyCode LandingGear = KeyCode.Alpha0;
	public KeyCode BrakeHoldRelease = KeyCode.X;
	public KeyCode AfterburnerControl = KeyCode.F12;
	public KeyCode dumpFuelControl = KeyCode.Alpha5;
	public KeyCode refuelControl = KeyCode.Alpha6;

	//
	[Header("Levers")]
	public string Aileron;
	public string Elevator;
	public string Rudder;
	public string FlapControl;
	public string SlatControl;
	[Header("Control Smooth Curve")]
	public AnimationCurve controlCurve;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
