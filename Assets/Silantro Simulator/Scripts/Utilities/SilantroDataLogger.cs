//
//Property of Oyedoyin Dada
//cc dadaoyedoyin@gmail.com
//
//
//using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Text;

//[AddComponentMenu("Oyedoyin/Utilties/Data Logger")]
public class SilantroDataLogger : MonoBehaviour {
	[Header("Savefile")]
	public string savefileName;
	public enum FileExtension
	{
		txt,
		csv
	}
	public FileExtension dataExtension = FileExtension.txt;
	//
	[Header("Save Location")]
	public string saveLocation = "C:/Users/";
	//
	[Header("Logger Settings")]
	public float logRate = 5f;
	public enum DataType
	{
		//EngineData, //COMING SOON
		FlightData
		//WingData// COMING SOON
	}
	public DataType dataType = DataType.FlightData;
	//
	[Header("Aircraft Connection")]
	public GameObject Aircraft;
	//
	bool isTurbojet;
	bool isTurboFan;
	bool isTurboProp;
	//
	SilantroInstrumentation datalog;
	//Engine Data
	SilantroTurboJet[] turboengines;
	SilantroTurboFan[] jetengines;
	SilantroTurboProp[] propengines;
	SilantroController control;
	//STATIC DATA
	string engineName;
	string fuelType;
	float massFactor;
	float fuelTankCapacity;
	//DYNAMIC DATA
	float enginePower;
	float engineRPM;
	float propEfficiency;
	float EGT;
	float currentFuel;
	float cumbustionRate;
	float engineThrust;
	float HealthValue;
	//
	bool csv;
	bool txt;
	//
	//FLIGHT DATA
	//STATIC
	string aircraftName;
	float takeoffweight;
	int noOfEngines;
	float startFuel;
	//DYNAMIC DATA
	float currentSpeed;
	float climbRate;
	int headingDirection;
	float currentAltitude;
	float fuelRemaining;
	float totalThrutGenerated;
	float currentHealth;
	//
	float flightTIme;
	float timer;
	float actualLogRate;
	//
	private List<string[]> dataRow = new List<string[]>();
	//StreamWriter streamWriter;
	//INDIVIDUAL PART HEALTHS.....COMING SOON
	// Use this for initialization
	void Start () {
		//
		if (Aircraft == null) {
			Debug.Log ("No Aircraft is connected to the Black Box");
		}
		datalog = Aircraft.GetComponentInChildren<SilantroInstrumentation>();//
		//Property of Oyedoyin Dada
		//cc dadaoyedoyin@gmail.com
		//
		//
		//
		if (datalog == null) {
			Debug.Log ("No instrumentation is connected to the Black Box");
		}

		control = Aircraft.GetComponent<SilantroController> ();
		if (control == null) {
			Debug.Log ("Controller has not been added to the Airplane");
		}
		if (control.engineType == SilantroController.AircraftType.TurboFan) {
			isTurboFan = true;
			jetengines = control.turbofans;
		}
		//
		if (control.engineType == SilantroController.AircraftType.TurboJet) {
			isTurbojet = true;
			turboengines = control.turboJet;
		}
		//
		if (control.engineType == SilantroController.AircraftType.TurboProp) {
			isTurboProp = true;
			propengines = control.turboprop;
		}
		//
		if (dataExtension == FileExtension.csv)
		{
			csv = true;
			txt = false;
		}

		else if (dataExtension == FileExtension.txt)
		{
			txt = true;
			csv = false;
		}
		//

		timer = 0.0f;
		//
		if (logRate != 0)
		{
			actualLogRate = 1.0f / logRate;
		} else 
		{
			actualLogRate = 0.10f;
		}
		//

		aircraftName = control.gameObject.name;
		takeoffweight = control.currentWeight;
		noOfEngines = control.AvailableEngines;
		startFuel = control.fuelsystem.TotalFuelRemaining;


	//	engines = aircraft.Engines;
		//
		if (dataType == DataType.FlightData) {

			//WRITE INITIAL TXT FILE
			if (txt) {
				File.WriteAllText (saveLocation + "" + savefileName + ".txt", "Flight Data");
				//
				using (System.IO.StreamWriter writeText = System.IO.File.AppendText (saveLocation + "" + savefileName + ".txt")) {
					writeText.WriteLine ("<<>>");
					writeText.WriteLine ("<<>>");
					writeText.WriteLine ("Aircraft Name: " + aircraftName);
					writeText.WriteLine ("Gross Weight: " + takeoffweight.ToString () + " kg");
					writeText.WriteLine ("No of Engines: " + noOfEngines.ToString ());
					writeText.WriteLine ("Available Fuel: " + startFuel.ToString () + " kg");
					writeText.WriteLine ("Date: " + DateTime.Now.ToString ("f"));
					//writeText.WriteLine ("Current Time: " + DateTime.Now.ToString ("));
					writeText.WriteLine ("<<>>");
					writeText.WriteLine ("Flight Time  " + "Current Speed  " + " Climb Rate  " + "    Heading  " + " Current Altitude  " + " Fuel Remaining  " + " Thrust Generated  " + " Current Health");
				}
			} else if (csv) {
				//WRITE INITIAL CSV FILE
				string[] dataRowTemp = new string[8];
				dataRowTemp [0] = "Flight Time";
				dataRowTemp [1] = "Current Speed";
				dataRowTemp [2] = "Climb Rate";
				dataRowTemp [3] = "Heading";
				dataRowTemp [4] = "Current Altitude";
				dataRowTemp [5] = "Fuel Remaining";
				dataRowTemp [6] = "Total Thrust Generated";
				dataRowTemp [7] = "Current Health";
				dataRow.Add (dataRowTemp);
				//
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;
		//
		if (isTurboFan) 
		{
			if(jetengines[0].EngineOn)
			{
				flightTIme += Time.deltaTime;
			}
			//
			if (timer > actualLogRate && jetengines[0].EngineOn) {
				if (txt) 
				{
					WriteLogTxt ();
				}
				else if (csv)
				{
					GetCSVData ();
				}

			}
		} 
		//
		if (isTurbojet) 
		{
			if(turboengines[0].EngineOn)
			{
				flightTIme += Time.deltaTime;
			}
			//
			if (timer > actualLogRate && turboengines[0].EngineOn) {
				if (txt) 
				{
					WriteLogTxt ();
				}
				else if (csv)
				{
					GetCSVData ();
				}

			}
		} 

		//
		if (isTurboProp) 
		{
			if(propengines[0].EngineOn)
			{
				flightTIme += Time.deltaTime;
			}
			//
			if (timer > actualLogRate && propengines[0].EngineOn) {
				if (txt) 
				{
					WriteLogTxt ();
				}
				else if (csv)
				{
					GetCSVData ();
				}

			}
		} 

	}
	//WRITES TXT LOG FILE
	void WriteLogTxt()
	{
		timer = 0.0f;
		//
		//string minSec = string.Format ("{0}:{1:00}", (int)controller.timeLeft / 60, (int)controller.timeLeft % 60);
		//
		if(datalog != null)
		{
			currentSpeed = datalog.currentSpeed;
			climbRate = datalog.verticalSpeed;
			currentAltitude = datalog.currentAltitude;
			totalThrutGenerated = control.totalThrustGenerated;
			headingDirection = (int)datalog.headingDirection;
			currentFuel = control.fuelsystem.TotalFuelRemaining;
		}


		if (Aircraft.gameObject.GetComponent<SilantroHealth> ()) {
			HealthValue = Aircraft.gameObject.GetComponent<SilantroHealth> ().currentHealth;
		}
		string minSec = string.Format ("{0}:{1:00}", (int)flightTIme / 60, (int)flightTIme % 60);

		//WRITE TXT DATA

		using (System.IO.StreamWriter writeText = System.IO.File.AppendText (saveLocation + "" + savefileName + ".txt")) {
			writeText.Write (minSec + " mins     " + currentSpeed.ToString ("0000.0") + " knots    " + climbRate.ToString ("  0000.0") + " ft/min    " + headingDirection.ToString ("000.0 ") + "°      " + currentAltitude.ToString ("000000.0") + " ft          " + currentFuel.ToString ("00000.0") + " kg            " + totalThrutGenerated.ToString ("000000.0") + " N             " + HealthValue.ToString ("0000.00") + Environment.NewLine);
		}
	}
	//
	void GetCSVData()
	{

		if(datalog != null)
		{
			currentSpeed = datalog.currentSpeed;
			climbRate = datalog.verticalSpeed;
			currentAltitude = datalog.currentAltitude;
			totalThrutGenerated = control.totalThrustGenerated;
			headingDirection = (int)datalog.headingDirection;
			currentFuel = control.fuelsystem.TotalFuelRemaining;
		}

		if (Aircraft.gameObject.GetComponent<SilantroHealth> ()) {
			HealthValue = Aircraft.gameObject.GetComponent<SilantroHealth> ().currentHealth;
		}
		string minSec = string.Format ("{0}:{1:00}", (int)flightTIme / 60, (int)flightTIme % 60);

		WriteLogCsv(minSec + " mins",currentSpeed.ToString ("0.0") + " knots",climbRate.ToString ("0.0") + " ft/min",headingDirection.ToString ("0.0 ") + " °",currentAltitude.ToString ("0.0") + " ft",currentFuel.ToString ("0.0") + " kg",totalThrutGenerated.ToString ("0.0") + " N",HealthValue.ToString ("0.00"));

	}
	//WRITES CSV LOG FILE
	void WriteLogCsv(string flightTIme, string currentSpeed, string climbRate, string Heading, string currentAltitude, string fuelRemaining, string totalThrust, string currentHealth)
	{
		timer = 0.0f;
		//
		string[] dataAdd = new string[8];
		//
		dataAdd[0] = flightTIme;
		dataAdd [1] = currentSpeed;
		dataAdd [2] = climbRate;
		dataAdd [3] = Heading;
		dataAdd [4] = currentAltitude;
		dataAdd [5] = fuelRemaining;
		dataAdd [6] = totalThrust;
		dataAdd [7] = currentHealth;
		dataRow.Add (dataAdd);

	}

	void OnApplicationQuit()
	{
		//
		if (csv) {
			string[][] output = new string[dataRow.Count][];
			//
			for (int i = 0; i < output.Length; i++) {
				output [i] = dataRow [i];
			}
			//
			int length = output.GetLength (0);
			string delimiter = ",";
			//
			StringBuilder builder = new StringBuilder ();
			for (int index = 0; index < length; index++)
				builder.AppendLine (string.Join (delimiter, output [index]));

			//StreamWriter streamWriter = System.IO.File.CreateText (saveLocation + "" + savefileName + ".csv");
			//streamWriter.WriteLine (builder);
			//streamWriter.Close ();
		}
	}

}
