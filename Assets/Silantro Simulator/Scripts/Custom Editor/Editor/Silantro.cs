using UnityEditor;
using UnityEngine;using System;using System.Linq;

public class Silantro : MonoBehaviour {

	public class SilantroMenu
	{
		[MenuItem("Oyedoyin/Propulsion System/Engines/TurboJet Engine")]
		private static void AddTurboJetEngine()
		{
			if (Selection.activeGameObject != null) {
				GameObject thruster = new GameObject ();
				thruster.name = "Thruster";
				thruster.transform.parent = Selection.activeGameObject.transform;
				thruster.transform.localPosition = new Vector3 (0, 0, -2);
				//
				GameObject fan = new GameObject ();
				fan.name = "Fan";
				fan.transform.parent = Selection.activeGameObject.transform;
				fan.transform.localPosition = new Vector3 (0, 0, 2);	Selection.activeGameObject.name = "Default TurboJet Engine";
				//
				GameObject effects = new GameObject("Engine Effects");
				effects.transform.parent = Selection.activeGameObject.transform;
				effects.transform.localPosition = new Vector3 (0, 0, -2);
				//
				GameObject smoke = (GameObject)AssetDatabase.LoadAssetAtPath ("Assets/Prefabs/Default/Componets/Exhaust Smoke.prefab",typeof(GameObject));
				GameObject smokeEffect = GameObject.Instantiate (smoke, effects.transform.position, Quaternion.Euler(0,-180,0),effects.transform);
				SilantroEngineEffect engineEffects = effects.AddComponent<SilantroEngineEffect> ();
				engineEffects.exhaustSmoke = smokeEffect.GetComponent<ParticleSystem> ();
				//
				Rigidbody parent = Selection.activeGameObject.transform.root.gameObject.GetComponent<Rigidbody> ();
				if (parent == null) {
					Debug.Log ("Engine is not parented to an Aircraft!! Create a Kinematic Rigidbody is you're just testing the Engine");
				}
				SilantroTurboJet jet = Selection.activeGameObject.AddComponent<SilantroTurboJet> ();engineEffects.engine = jet.gameObject;engineEffects.engineType = SilantroEngineEffect.EngineType.TurboJet;
				jet.Thruster = thruster.transform;
				jet.intakeFanPoint = fan.transform;
				if (parent != null) {
					jet.Parent = parent;
				}//
				AudioClip start = (AudioClip)AssetDatabase.LoadAssetAtPath ("Assets/Sounds/Default/Engines/Default Start.wav",typeof(AudioClip));
				AudioClip stop = (AudioClip)AssetDatabase.LoadAssetAtPath ("Assets/Sounds/Default/Engines/Default Shutdown.wav",typeof(AudioClip));
			//	AudioClip alert = (AudioClip)AssetDatabase.LoadAssetAtPath ("Assets/Sounds/Default/Engines/Default Fuel Alert.wav",typeof(AudioClip));
				AudioClip run = (AudioClip)AssetDatabase.LoadAssetAtPath ("Assets/Sounds/Default/Engines/Jet/Jet Running.wav",typeof(AudioClip));
				jet.EngineIdleSound = run;
				jet.EngineStartSound = start;jet.EngineShutdownSound = stop;

			} else {
				Debug.Log ("Please Select GameObject to add Engine to..");
			}
		}
		//

		[MenuItem("Oyedoyin/Propulsion System/Engines/TurboFan Engine")]
		private static void AddTurboFanEngine()
		{
			if (Selection.activeGameObject != null) {
			GameObject thruster = new GameObject ();
			thruster.name = "Thruster";
			thruster.transform.parent = Selection.activeGameObject.transform;
			thruster.transform.localPosition = new Vector3 (0, 0, -2);
			//
			GameObject fan = new GameObject ();
			fan.name= "Fan";
			fan.transform.parent = Selection.activeGameObject.transform;
				fan.transform.localPosition = new Vector3 (0, 0, 2);	Selection.activeGameObject.name = "Default TurboFan Engine";
				//
				//
				GameObject effects = new GameObject("Engine Effects");
				effects.transform.parent = Selection.activeGameObject.transform;
				effects.transform.localPosition = new Vector3 (0, 0, -2);
				//
				GameObject smoke = (GameObject)AssetDatabase.LoadAssetAtPath ("Assets/Prefabs/Default/Componets/Exhaust Smoke.prefab",typeof(GameObject));
				GameObject smokeEffect = GameObject.Instantiate (smoke, effects.transform.position, Quaternion.Euler(0,-180,0),effects.transform);
				SilantroEngineEffect engineEffects = effects.AddComponent<SilantroEngineEffect> ();
				engineEffects.exhaustSmoke = smokeEffect.GetComponent<ParticleSystem> ();
			//
			Rigidbody parent = Selection.activeGameObject.transform.root.gameObject.GetComponent<Rigidbody>();
				if (parent == null) {
					Debug.Log ("Engine is not parented to an Aircraft!! Create a default Rigidbody is you're just testing the Engine");
				}
				SilantroTurboFan jet =  Selection.activeGameObject.AddComponent<SilantroTurboFan> ();engineEffects.engine = jet.gameObject;engineEffects.engineType = SilantroEngineEffect.EngineType.TurboFan;
			jet.Thruster = thruster.transform;
			jet.fan = fan.transform;
			if (parent != null) {
				jet.Parent = parent;
			}
				AudioClip start = (AudioClip)AssetDatabase.LoadAssetAtPath ("Assets/Sounds/Default/Engines/Default Start.wav",typeof(AudioClip));
				AudioClip stop = (AudioClip)AssetDatabase.LoadAssetAtPath ("Assets/Sounds/Default/Engines/Default Shutdown.wav",typeof(AudioClip));
				//AudioClip alert = (AudioClip)AssetDatabase.LoadAssetAtPath ("Assets/Sounds/Default/Engines/Default Fuel Alert.wav",typeof(AudioClip));
				AudioClip run = (AudioClip)AssetDatabase.LoadAssetAtPath ("Assets/Sounds/Default/Engines/Jet/Jet Running.wav",typeof(AudioClip));
				jet.EngineIdleSound = run;jet.EngineStartSound = start;jet.EngineShutdownSound = stop;
			} 
			else {
				Debug.Log ("Please Select GameObject to add Engine to..");
			}
		}
		//
		//
		//SETUP TURBO PROP ENGINE
		[MenuItem("Oyedoyin/Propulsion System/Engines/TurboProp Engine")]
		private static void AddTurboPropEngine()
		{
			if (Selection.activeGameObject != null) {
			GameObject thruster = new GameObject ();
			thruster.name = "Thruster";
			thruster.transform.parent = Selection.activeGameObject.transform;
			thruster.transform.localPosition = new Vector3 (0, 0, -1);
			//
			Rigidbody parent = Selection.activeGameObject.transform.root.gameObject.GetComponent<Rigidbody>();
				if (parent == null) {
					Debug.Log ("Engine is not parented to an Aircraft!! Create a default Rigidbody is you're just testing the Engine");
				}
				SilantroTurboProp prop = Selection.activeGameObject.AddComponent<SilantroTurboProp> ();
			prop.Thruster = thruster.transform;
				prop.Parent = parent;	Selection.activeGameObject.name = "Default TurboProp Engine";
			//
			GameObject normalPropeller = new GameObject("Slow Propeller");
			GameObject fastPropller = new GameObject ("Fast Propeller");
			GameObject Props = new GameObject ("Propellers");
		//
			Props.transform.parent = Selection.activeGameObject.transform;
			fastPropller.transform.parent = Props.transform;normalPropeller.transform.parent = Props.transform;
			//
			prop.fastPropeller = fastPropller.transform;prop.Propeller = normalPropeller.transform;
				AudioClip start = (AudioClip)AssetDatabase.LoadAssetAtPath ("Assets/Sounds/Default/Engines/Default Start.wav",typeof(AudioClip));
				AudioClip stop = (AudioClip)AssetDatabase.LoadAssetAtPath ("Assets/Sounds/Default/Engines/Default Shutdown.wav",typeof(AudioClip));
			//	AudioClip alert = (AudioClip)AssetDatabase.LoadAssetAtPath ("Assets/Sounds/Default/Engines/Default Fuel Alert.wav",typeof(AudioClip));
				AudioClip run = (AudioClip)AssetDatabase.LoadAssetAtPath ("Assets/Sounds/Default/Engines/Propeller/Propeller Running.wav",typeof(AudioClip));
				prop.EngineStartSound = start;prop.EngineShutdownSound = stop;prop.EngineIdleSound = run;
			} else {
				Debug.Log ("Please Select GameObject to add Engine to..");
			}
		}
		//
		//SETUP TURBOFAN ENGINE
		[MenuItem("Oyedoyin/Propulsion System/Engines/LiftFan Engine")]
		private static void AddLiftFanEngine()
		{
			if (Selection.activeGameObject != null) {
			GameObject thruster = new GameObject ();
			thruster.name = "Thruster";
			thruster.transform.parent = Selection.activeGameObject.transform;
			thruster.transform.localPosition = new Vector3 (0, -1, 0);
			//
			GameObject fan = new GameObject ();
			fan.name= "Fan";
			fan.transform.parent = Selection.activeGameObject.transform;
			fan.transform.localPosition = new Vector3 (0, 1, 0);
			//
				Selection.activeGameObject.name = "Default LiftFan Engine";
			Rigidbody parent = Selection.activeGameObject.transform.root.gameObject.GetComponent<Rigidbody>();
				if (parent == null) {
					Debug.Log ("Engine is not parented to an Aircraft!! Create a default Rigidbody is you're just testing the Engine");
				}SilantroLiftFan liftfan =  Selection.activeGameObject.AddComponent<SilantroLiftFan> ();
			//
			liftfan.Thruster = thruster.transform;
			liftfan.fan = fan.transform;
			//
			if (parent != null) {
				liftfan.Parent = parent;
			}
				AudioClip start = (AudioClip)AssetDatabase.LoadAssetAtPath ("Assets/Sounds/Default/Engines/Default Start.wav",typeof(AudioClip));
				AudioClip stop = (AudioClip)AssetDatabase.LoadAssetAtPath ("Assets/Sounds/Default/Engines/Default Shutdown.wav",typeof(AudioClip));
				AudioClip run = (AudioClip)AssetDatabase.LoadAssetAtPath ("Assets/Sounds/Default/Engines/LiftFan/Fan Running.wav",typeof(AudioClip));
				liftfan.fanStartClip = start;liftfan.fanShutdownClip = stop;liftfan.fanRunningClip = run;
		} else {
			Debug.Log ("Please Select GameObject to add Engine to..");
			}
		}
		//
		[MenuItem("Oyedoyin/Propulsion System/Fuel System/Fuel Distributor")]
		private static void AddFuelControl()
		{
			GameObject tank = new GameObject ();
			tank.name = "Fuel Distributor";
			SilantroFuelDistributor distributor = tank.AddComponent<SilantroFuelDistributor> ();
			AudioClip alert = (AudioClip)AssetDatabase.LoadAssetAtPath ("Assets/Sounds/Default/Engines/Default Fuel Alert.wav",typeof(AudioClip));
			distributor.fuelAlert = alert;
		}
		[MenuItem("Oyedoyin/Propulsion System/Fuel System/Fuel Tanks/Internal")]
		private static void AddInternalTank()
		{
			GameObject tank;
			if (Selection.activeGameObject != null && Selection.activeGameObject.GetComponent<SilantroFuelDistributor>())
			{
				tank = new GameObject ();
				tank.transform.parent = Selection.activeGameObject.transform;tank.transform.localPosition = new Vector3 (0, 0, 0);
			} else {
				tank = new GameObject ();
			}
			tank.name = "Internal Fuel Tank";
			SilantroFuelTank fuelTank = tank.AddComponent<SilantroFuelTank> ();fuelTank.Capacity = 1000f;fuelTank.tankType = SilantroFuelTank.TankType.Internal;
			GameObject smoke = (GameObject)AssetDatabase.LoadAssetAtPath ("Assets/Prefabs/Default/Effects/Explosion/Component Explosion.prefab",typeof(GameObject));
			fuelTank.ExplosionPrefab = smoke;
		}
		//
		[MenuItem("Oyedoyin/Propulsion System/Fuel System/Fuel Tanks/External")]
		private static void AddExternalTank()
		{
			GameObject tank;
			if (Selection.activeGameObject != null && Selection.activeGameObject.GetComponent<SilantroFuelDistributor>())
			{
				tank = new GameObject ();
				tank.transform.parent = Selection.activeGameObject.transform;tank.transform.localPosition = new Vector3 (0, 0, 0);
			} else {
				tank = new GameObject ();
			}
			tank.name = "External Fuel Tank";
			SilantroFuelTank fuelTank = tank.AddComponent<SilantroFuelTank> ();fuelTank.Capacity = 400f;fuelTank.tankType = SilantroFuelTank.TankType.External;
			GameObject smoke = (GameObject)AssetDatabase.LoadAssetAtPath ("Assets/Prefabs/Default/Effects/Explosion/Component Explosion.prefab",typeof(GameObject));
			fuelTank.ExplosionPrefab = smoke;
		}
		//
		//
		[MenuItem("Oyedoyin/Aerofoil System/Structures/Controllable/Wing")]
		private static void AddWing()
		{
			GameObject wing;
			if (Selection.activeGameObject != null )
			{
				wing = new GameObject ();
				wing.transform.parent = Selection.activeGameObject.transform;wing.transform.localPosition = new Vector3 (0, 0, 0);
			} else {
				wing = new GameObject ();GameObject parent = new GameObject ("Aerodynamics");wing.transform.parent = parent.transform;
			}
			wing.name = "Default Right Wing";
			SilantroAerofoil wingAerofoil = wing.AddComponent<SilantroAerofoil> ();wingAerofoil.AerofoilSubdivisions = 5;wingAerofoil.aerofoilType = SilantroAerofoil.AerofoilType.Wing;
			SilantroControlSurface wingControl = wing.AddComponent<SilantroControlSurface> ();wingControl.surfaceType = SilantroControlSurface.SurfaceType.Aileron;wingAerofoil.controlSurface = wingControl;wingAerofoil.canBeControlled = true;
			SilantroAirfoil wng = (SilantroAirfoil)AssetDatabase.LoadAssetAtPath ("Assets/Prefabs/Default/Airfoils/Default Wing 23016.prefab",typeof(SilantroAirfoil));
			wingAerofoil.airfoil = wng;
		
		}
		//
		[MenuItem("Oyedoyin/Aerofoil System/Structures/Controllable/Tail")]
		private static void AddTail()
		{
			GameObject wing;
			if (Selection.activeGameObject != null )
			{
				wing = new GameObject ();
				wing.transform.parent = Selection.activeGameObject.transform;wing.transform.localPosition = new Vector3 (0, 0, 0);
			} else {
				wing = new GameObject ();GameObject parent = new GameObject ("Aerodynamics");wing.transform.parent = parent.transform;
			}
			wing.name = "Default Right Tail";//wing.transform.parent = parent.transform;
			SilantroAerofoil wingAerofoil = wing.AddComponent<SilantroAerofoil> ();wingAerofoil.AerofoilSubdivisions = 4;wingAerofoil.aerofoilType = SilantroAerofoil.AerofoilType.Tail;
			SilantroControlSurface wingControl = wing.AddComponent<SilantroControlSurface> ();wingControl.surfaceType = SilantroControlSurface.SurfaceType.Elevator;wingAerofoil.controlSurface = wingControl;wingAerofoil.canBeControlled = true;
			SilantroAirfoil start = (SilantroAirfoil)AssetDatabase.LoadAssetAtPath ("Assets/Prefabs/Default/Airfoils/Default Control 0009.prefab",typeof(SilantroAirfoil));
			wingAerofoil.airfoil = start;}
		//
		[MenuItem("Oyedoyin/Aerofoil System/Structures/Controllable/Rudder")]
		private static void AddRudder()
		{
			GameObject wing;
			if (Selection.activeGameObject != null )
			{
				wing = new GameObject ();
				wing.transform.parent = Selection.activeGameObject.transform;wing.transform.localPosition = new Vector3 (0, 0, 0);
			} else {
				wing = new GameObject ();GameObject parent = new GameObject ("Aerodynamics");wing.transform.parent = parent.transform;
			}
			wing.name = "Default Rudder";//wing.transform.parent = parent.transform;
			SilantroAerofoil wingAerofoil = wing.AddComponent<SilantroAerofoil> ();wingAerofoil.AerofoilSubdivisions = 4;wingAerofoil.aerofoilType = SilantroAerofoil.AerofoilType.Tail;
			SilantroControlSurface wingControl = wing.AddComponent<SilantroControlSurface> ();wingControl.surfaceType = SilantroControlSurface.SurfaceType.Rudder;wingAerofoil.controlSurface = wingControl;wingAerofoil.canBeControlled = true;
			GameObject start = (GameObject)AssetDatabase.LoadAssetAtPath ("Assets/Prefabs/Default/Airfoils/Default Control 0009.prefab",typeof(GameObject));
			wingAerofoil.airfoil = start.GetComponent<SilantroAirfoil> ();}
		//
		[MenuItem("Oyedoyin/Aerofoil System/Structures/Stationary/Wing")]
		private static void AddStationaryWing()
		{
			GameObject wing;
			if (Selection.activeGameObject != null )
			{
				wing = new GameObject ();
				wing.transform.parent = Selection.activeGameObject.transform;wing.transform.localPosition = new Vector3 (0, 0, 0);
			} else {
				wing = new GameObject ();GameObject parent = new GameObject ("Aerodynamics");wing.transform.parent = parent.transform;
			}
			wing.name = "Default Right Wing";//wing.transform.parent = parent.transform;
			SilantroAerofoil wingAerofoil = wing.AddComponent<SilantroAerofoil> ();wingAerofoil.AerofoilSubdivisions = 5;wingAerofoil.aerofoilType = SilantroAerofoil.AerofoilType.Wing;wingAerofoil.canBeControlled = false;
			GameObject start = (GameObject)AssetDatabase.LoadAssetAtPath ("Assets/Prefabs/Default/Airfoils/Default Wing 23016.prefab",typeof(GameObject));
			wingAerofoil.airfoil = start.GetComponent<SilantroAirfoil> ();wingAerofoil.canBeControlled = false;
		
		}

		//
		[MenuItem("Oyedoyin/Aerofoil System/Structures/Stationary/Tail")]
		private static void AddStationaryTail()
		{
			GameObject wing;
			if (Selection.activeGameObject != null )
			{
				wing = new GameObject ();
				wing.transform.parent = Selection.activeGameObject.transform;wing.transform.localPosition = new Vector3 (0, 0, 0);
			} else {
				wing = new GameObject ();GameObject parent = new GameObject ("Aerodynamics");wing.transform.parent = parent.transform;
			}
			wing.name = "Default Rudder";//wing.transform.parent = parent.transform;
			SilantroAerofoil wingAerofoil = wing.AddComponent<SilantroAerofoil> ();wingAerofoil.AerofoilSubdivisions = 4;wingAerofoil.aerofoilType = SilantroAerofoil.AerofoilType.Tail;wingAerofoil.canBeControlled = false;
			GameObject start = (GameObject)AssetDatabase.LoadAssetAtPath ("Assets/Prefabs/Default/Airfoils/Default Control 0009.prefab",typeof(GameObject));
			wingAerofoil.airfoil = start.GetComponent<SilantroAirfoil> ();wingAerofoil.canBeControlled = false;
		}
		//
		//
		[MenuItem("Oyedoyin/Aerofoil System/Structures/Left Structure/Bound")]
		private static void AddLeftWingMoving()
		{
			//
			//
			GameObject wingInstance;GameObject wing;
			if (Selection.activeGameObject != null)
			{
				wing = Selection.activeGameObject;
			} else {
				return;
			}
			SilantroAerofoil wingAerofoil = wing.GetComponent<SilantroAerofoil> ();
			if (wingAerofoil != null && wingAerofoil.gameObject.GetComponent<SilantroMirrorTransform> () == null) {
				wingInstance = new GameObject ();
				wingInstance.transform.parent = Selection.activeGameObject.transform.parent;
				wingInstance.name = "Default Left " + wingAerofoil.aerofoilType.ToString ();
				SilantroAerofoil newwingAerofoil = wingInstance.AddComponent<SilantroAerofoil> ();
				newwingAerofoil.AerofoilSubdivisions = wingAerofoil.AerofoilSubdivisions;
				newwingAerofoil.aerofoilType = wingAerofoil.aerofoilType;newwingAerofoil.airfoil = wingAerofoil.airfoil;
				newwingAerofoil.canBeControlled = wingAerofoil.canBeControlled;
				//
				SilantroControlSurface wingControl = newwingAerofoil.gameObject.AddComponent<SilantroControlSurface> ();
				wingControl.surfaceType = wingAerofoil.controlSurface.surfaceType;
				//
				SilantroMirrorTransform mirror = wingInstance.AddComponent<SilantroMirrorTransform> ();
				mirror.Instance = wing.transform;

			} 
			else if (wingAerofoil != null && wingAerofoil.gameObject.GetComponent<SilantroMirrorTransform> () == null) {
				Debug.Log ("Selected GameObject has an attached Mirror Transform");
			}
			else if (wingAerofoil == null) {
				Debug.Log ("Selected GameObject is not an Aerofoil! Create an Aerofoil and try again");
			} 

		}
		//
		//
		//
		[MenuItem("Oyedoyin/Aerofoil System/Structures/Left Structure/UnBound")]
		private static void AddLeftWing()
		{
			//
			//
			GameObject wingInstance;GameObject wing;
			if (Selection.activeGameObject != null)
			{
				wing = Selection.activeGameObject;
			} else {
				return;
			}
			SilantroAerofoil wingAerofoil = wing.GetComponent<SilantroAerofoil> ();
			if (wingAerofoil != null && wingAerofoil.gameObject.GetComponent<SilantroMirrorTransform> () == null) {
				wingInstance = new GameObject ();
				wingInstance.transform.parent = Selection.activeGameObject.transform.parent;
				wingInstance.name = "Default Left " + wingAerofoil.aerofoilType.ToString ();	float x = wingAerofoil.transform.localPosition.x;
				float y = wingAerofoil.transform.localPosition.y;
				float z = wingAerofoil.transform.localPosition.z;
				wingInstance.transform.localPosition = new Vector3 (x-2, y, z);
				SilantroAerofoil newwingAerofoil = wingInstance.AddComponent<SilantroAerofoil> ();
				newwingAerofoil.AerofoilSubdivisions = wingAerofoil.AerofoilSubdivisions;
				newwingAerofoil.aerofoilType = wingAerofoil.aerofoilType;newwingAerofoil.airfoil = wingAerofoil.airfoil;
				newwingAerofoil.canBeControlled = wingAerofoil.canBeControlled;
				//
				SilantroControlSurface wingControl = newwingAerofoil.gameObject.AddComponent<SilantroControlSurface> ();
				wingControl.surfaceType = wingAerofoil.controlSurface.surfaceType;
				//
				newwingAerofoil.transform.localScale = new Vector3 (-1, 1, 1);

			} 
			else if (wingAerofoil != null && wingAerofoil.gameObject.GetComponent<SilantroMirrorTransform> () == null) {
				Debug.Log ("Selected GameObject has an attached Mirror Transform");
			}
			else if (wingAerofoil == null) {
				Debug.Log ("Selected GameObject is not an Aerofoil! Create an Aerofoil and try again");
			} 

		}
		//
		[MenuItem("Oyedoyin/Aerofoil System/Controls/Aileron")]
		private static void AddAileron()
		{
			GameObject wing;
			if (Selection.activeGameObject != null)
			{
				wing = Selection.activeGameObject;
			} else {
				return;
			}
			SilantroAerofoil wingAerofoil = wing.GetComponent<SilantroAerofoil> ();

			if (wingAerofoil != null && wingAerofoil.gameObject.GetComponent<SilantroControlSurface> () == null && wingAerofoil.aerofoilType == SilantroAerofoil.AerofoilType.Wing) {
				SilantroControlSurface wingControl = wingAerofoil.gameObject.AddComponent<SilantroControlSurface> ();
				wingAerofoil.controlSurface = wingControl;
				wingAerofoil.canBeControlled = true;
				wingControl.surfaceType = SilantroControlSurface.SurfaceType.Aileron;
			} else if (wingAerofoil == null) {
				Debug.Log ("Selected GameObject is not an Aerofoil! Create an Aerofoil and try again");
			} else if (wingAerofoil.gameObject.GetComponent<SilantroControlSurface> () != null) {
				Debug.Log ("Selected Aerofoil already contains a Control Surface, add Flap maybe!?");
			} else if (wingAerofoil.aerofoilType == SilantroAerofoil.AerofoilType.Tail) {
				Debug.Log ("Aileron can only be used to control the Wing!!!, Add Elevator or Rudder to the Tail");
			}
		}
		//
		[MenuItem("Oyedoyin/Aerofoil System/Controls/Elevator")]
		private static void AddElevator()
		{
			GameObject wing;
			if (Selection.activeGameObject != null)
			{
				wing = Selection.activeGameObject;
			} else {
				return;
			}
			SilantroAerofoil wingAerofoil = wing.GetComponent<SilantroAerofoil> ();
			if (wingAerofoil != null && wingAerofoil.gameObject.GetComponent<SilantroControlSurface> () == null && wingAerofoil.aerofoilType == SilantroAerofoil.AerofoilType.Tail) {
				SilantroControlSurface wingControl = wingAerofoil.gameObject.AddComponent<SilantroControlSurface> ();
				wingAerofoil.controlSurface = wingControl;
				wingAerofoil.canBeControlled = true;
				wingControl.surfaceType = SilantroControlSurface.SurfaceType.Elevator;
			} else if (wingAerofoil == null) {Debug.Log ("Selected GameObject is not an Aerofoil! Create an Aerofoil and try again");
			} else if (wingAerofoil.gameObject.GetComponent<SilantroControlSurface> () != null) {Debug.Log("Selected Aerofoil already contains a Control Surface, add Flap maybe!?");
			}
			else if (wingAerofoil.aerofoilType == SilantroAerofoil.AerofoilType.Wing) {
			Debug.Log ("Elevator can only be used to control the Tail!!!, Add Aileron or Flap to the Wing");
			}
		}
		//
		[MenuItem("Oyedoyin/Aerofoil System/Controls/Rudder")]
		private static void AddRudderControl()
		{
			GameObject wing;
			if (Selection.activeGameObject != null)
			{
				wing = Selection.activeGameObject;
			} else {
				return;
			}
			SilantroAerofoil wingAerofoil = wing.GetComponent<SilantroAerofoil> ();
			if (wingAerofoil != null && wingAerofoil.gameObject.GetComponent<SilantroControlSurface> () == null) {
				SilantroControlSurface wingControl = wingAerofoil.gameObject.AddComponent<SilantroControlSurface> ();
				wingAerofoil.controlSurface = wingControl;
				wingAerofoil.canBeControlled = true;
				wingControl.surfaceType = SilantroControlSurface.SurfaceType.Rudder;
			} else if (wingAerofoil == null) {Debug.Log ("Selected GameObject is not an Aerofoil! Create an Aerofoil and try again");
			} else if (wingAerofoil.gameObject.GetComponent<SilantroControlSurface> () != null) {Debug.Log("Selected Aerofoil already contains a Control Surface, add Flap maybe!?");
			}
		}
		//
		//
		[MenuItem("Oyedoyin/Aerofoil System/Controls/Flap")]
		private static void AddFlap()
		{
			GameObject wing;
			if (Selection.activeGameObject != null)
			{
				wing = Selection.activeGameObject;
			} else {
				return;
			}
			SilantroAerofoil wingAerofoil = wing.GetComponent<SilantroAerofoil> ();
			if (wingAerofoil != null && wingAerofoil.gameObject.GetComponent<SilantroFlap> () == null && wingAerofoil.aerofoilType == SilantroAerofoil.AerofoilType.Wing) {
				SilantroFlap wingControl = wingAerofoil.gameObject.AddComponent<SilantroFlap> ();
				wingAerofoil.flap = wingControl;
				wingAerofoil.usesFlap = true;wingAerofoil.canBeControlled = true;
			} else if (wingAerofoil == null) {
				Debug.Log ("Selected GameObject is not an Aerofoil! Create an Aerofoil and try again");
			} else if (wingAerofoil.gameObject.GetComponent<SilantroFlap> () != null) {
				Debug.Log ("Selected Aerofoil already contains Flap Control!!");
			} else if (wingAerofoil.aerofoilType == SilantroAerofoil.AerofoilType.Tail) {
				Debug.Log ("Flap can only be used to control the Wing!!!");
			}	
		}
		//
		[MenuItem("Oyedoyin/Aerofoil System/Controls/Slat")]
		private static void AddSlat()
		{
			Debug.Log ("Feature Coming Soon in the Next Update");
		}
		//
		[MenuItem("Oyedoyin/Weapon System/Component/Minigun")]
		private static void AddMinigun()
		{
			GameObject gun; 
			gun = new GameObject ();
			gun.name = "Default Gatling Gun";
			//
			GameObject shootSpot = new GameObject ("Shoot Spot");GameObject shellPoint = new GameObject ("Shell Ejection Point");shootSpot.transform.parent = gun.transform;shellPoint.transform.parent = gun.transform;
			SilantroMinigun minigun = gun.AddComponent<SilantroMinigun> ();minigun.shellEjectPoint = shellPoint.transform;minigun.muzzles.Add (shootSpot.transform);
			AudioClip shoot = (AudioClip)AssetDatabase.LoadAssetAtPath ("Assets/Sounds/Default/Weapons/Minigun/minigun_Fire.wav",typeof(AudioClip));
			minigun.fireSound = shoot;
			//
			GameObject flash = (GameObject)AssetDatabase.LoadAssetAtPath ("Assets/Prefabs/Default/Weapons/Effects/Flash/Default Muzzle Flash.prefab",typeof(GameObject));
			GameObject impact = (GameObject)AssetDatabase.LoadAssetAtPath ("Assets/Prefabs/Default/Weapons/Effects/Impacts/Ground Impact.prefab",typeof(GameObject));
			GameObject bcase = (GameObject)AssetDatabase.LoadAssetAtPath ("Assets/Prefabs/Default/Weapons/Cases/7.62x36mm.prefab",typeof(GameObject));
			//
			minigun.muzzleFlash = flash;minigun.groundHit = impact;minigun.metalHit = impact;minigun.woodHit = impact;minigun.bulletCase = bcase;
		}
		//
		[MenuItem("Oyedoyin/Weapon System/Component/Missile")]
		private static void AddMissile()
		{
			Debug.Log ("Feature Coming Soon in the Next Update");
		}
		//
		[MenuItem("Oyedoyin/Weapon System/Component/Bomb")]
		private static void Addbomb()
		{
			Debug.Log ("Feature Coming Soon in the Next Update");
		}
		//
		[MenuItem("Oyedoyin/Health System/Aircraft Health")]
		private static void AddmainHealth()
		{
			GameObject plane;SilantroHealth health;
			if (Selection.activeGameObject != null) {plane = Selection.activeGameObject;
				health = plane.AddComponent<SilantroHealth> ();//
				GameObject planeFire = (GameObject)AssetDatabase.LoadAssetAtPath ("Assets/Prefabs/Default/Effects/Fire/Airplane Fire.prefab",typeof(GameObject));
				GameObject plosion = (GameObject)AssetDatabase.LoadAssetAtPath ("Assets/Prefabs/Default/Effects/Explosion/Aircraft Explosion.prefab",typeof(GameObject));
				GameObject engineFire = (GameObject)AssetDatabase.LoadAssetAtPath ("Assets/Prefabs/Default/Effects/Fire/Engine Fire.prefab",typeof(GameObject));
				//
				health.firePrefab = planeFire;health.ExplosionPrefab = plosion;
			} else {Debug.Log ("Please Select an Aircraft to add Health Component to!!");}

		}
		//
		[MenuItem("Oyedoyin/Health System/Aerofoil Health")]
		private static void AddAerofoilHealth()
		{
			GameObject aerofoil;SilantroAerofoilHealth health;
			if (Selection.activeGameObject != null) {aerofoil = Selection.activeGameObject;
				if (aerofoil.GetComponent<SilantroAerofoil> ()) {
					health = aerofoil.AddComponent<SilantroAerofoilHealth> ();
					//
					GameObject planeFire = (GameObject)AssetDatabase.LoadAssetAtPath ("Assets/Prefabs/Default/Effects/Fire/Component Fire.prefab",typeof(GameObject));
					GameObject plosion = (GameObject)AssetDatabase.LoadAssetAtPath ("Assets/Prefabs/Default/Effects/Explosion/Component Explosion.prefab",typeof(GameObject));
					//
					health.firePrefab = planeFire;health.ExplosionPrefab = plosion;
				} else {Debug.Log ("Selected GameObject is not an Aerofoil");}
			} else {Debug.Log ("Please Select an Aerofoil to add Health Component to!!");}
			//
		}
		//
		[MenuItem("Oyedoyin/Health System/Engine Health")]
		private static void AddEngineHealth()
		{
			GameObject aerofoil;
			SilantroEngineHealth health;
			if (Selection.activeGameObject != null) {aerofoil = Selection.activeGameObject;
				if (aerofoil.GetComponent<SilantroTurboFan> ()||aerofoil.GetComponent<SilantroTurboJet> ()||aerofoil.GetComponent<SilantroTurboProp> ()) {
					health = aerofoil.AddComponent<SilantroEngineHealth> ();
					//
					GameObject engineFire = (GameObject)AssetDatabase.LoadAssetAtPath ("Assets/Prefabs/Default/Effects/Fire/Engine Fire.prefab",typeof(GameObject));
					GameObject plosion = (GameObject)AssetDatabase.LoadAssetAtPath ("Assets/Prefabs/Default/Effects/Explosion/Component Explosion.prefab",typeof(GameObject));
					//
					health.engineFire = engineFire;health.ExplosionPrefab = plosion;
				} else {Debug.Log ("Selected GameObject is not an Engine");}
			} else {Debug.Log ("Please Select an Engine to add Health Component to!!");}
			//
		}
		//
		[MenuItem("Oyedoyin/Health System/Hit")]
		private static void AddHit()
		{GameObject component;
			if (Selection.activeGameObject != null) {component = Selection.activeGameObject;
				if (component.GetComponent<Collider> ()) {
					component.AddComponent<SilantroHit> ();
				} else {
					Debug.Log ("No Collider is attached to the selected GameObject, Hit requires collider to function");
				}
			} else {Debug.Log ("Select GameObject to add Hit Component to!!");
			};
		}


		//
		[MenuItem("Oyedoyin/Hydraulic System/Gear System")]
		private static void AddGearSystem()
		{
			GameObject wheelSystem;
			wheelSystem = new GameObject ("Wheel System");
	
			SilantroGearSystem gearSystem = wheelSystem.AddComponent<SilantroGearSystem> ();
			//SETUP WHEELS
			GameObject frontWheel = new GameObject("Front Wheel");frontWheel.transform.parent = wheelSystem.transform;frontWheel.transform.localPosition = new Vector3(0,-0.5f,0);frontWheel.AddComponent<WheelCollider>();frontWheel.GetComponent<WheelCollider>().radius = 0.2f;
			GameObject leftWheel = new GameObject ("Left Wheel");leftWheel.transform.parent = wheelSystem.transform;leftWheel.transform.localPosition = new Vector3 (-1, -0.5f, -2);leftWheel.AddComponent<WheelCollider>();leftWheel.GetComponent<WheelCollider>().radius = 0.2f;
			GameObject rightWheel = new GameObject ("Right Wheel");rightWheel.transform.parent = wheelSystem.transform;rightWheel.transform.localPosition = new Vector3 (1, -0.5f, -2);rightWheel.AddComponent<WheelCollider>();rightWheel.GetComponent<WheelCollider>().radius = 0.2f;
			//
			SilantroGearSystem.WheelSystem frontGearSystem = new SilantroGearSystem.WheelSystem ();frontGearSystem.collider = frontWheel.GetComponent<WheelCollider>();frontGearSystem.Identifier = "Front Gear";frontGearSystem.steerable = true;
			SilantroGearSystem.WheelSystem leftGearSystem = new SilantroGearSystem.WheelSystem ();leftGearSystem.collider = leftWheel.GetComponent<WheelCollider>();leftGearSystem.Identifier = "Left Gear";leftGearSystem.attachedMotor = true;
			SilantroGearSystem.WheelSystem rightGearSystem = new SilantroGearSystem.WheelSystem ();rightGearSystem.collider = rightWheel.GetComponent<WheelCollider>();rightGearSystem.Identifier = "Right Gear";rightGearSystem.attachedMotor = true;
			//
			gearSystem.wheelSystem.Add(frontGearSystem);gearSystem.wheelSystem.Add(leftGearSystem);gearSystem.wheelSystem.Add(rightGearSystem);
			AudioClip open = (AudioClip)AssetDatabase.LoadAssetAtPath ("Assets/Sounds/Default/Gear/Gear Open.wav",typeof(AudioClip));
			AudioClip close = (AudioClip)AssetDatabase.LoadAssetAtPath ("Assets/Sounds/Default/Gear/Gear Close.wav",typeof(AudioClip));
			gearSystem.gearUp = close;gearSystem.gearDown = open;
		}
		//
		//
		[MenuItem("Oyedoyin/Hydraulic System/Door System")]
		private static void AddDoorSystem()
		{
			GameObject door;
			door = new GameObject ("Door Hydraulics");
			//
			SilantroHydraulicSystem doorSystem = door.AddComponent<SilantroHydraulicSystem> ();
			AudioClip open = (AudioClip)AssetDatabase.LoadAssetAtPath ("Assets/Sounds/Default/Door/Door Open.wav",typeof(AudioClip));
			AudioClip close = (AudioClip)AssetDatabase.LoadAssetAtPath ("Assets/Sounds/Default/Door/Door Close.wav",typeof(AudioClip));
			doorSystem.openSound = open;doorSystem.closeSound = close;
		}
		//
		[MenuItem("Oyedoyin/Camera System/Orbit Camera")]
		private static void Camera()
		{
			GameObject box;
			if (Selection.activeGameObject != null)
			{
				box = new GameObject ();
				box.transform.parent = Selection.activeGameObject.transform;
			} else {
				box = new GameObject ();
			}
			box.name = "Camera System";
			GameObject focalPoint = new GameObject ("Camera Focus Point");focalPoint.transform.parent = box.transform;
			GameObject normalCam = new GameObject("Default Camera");
			normalCam.gameObject.transform.parent = box.transform;normalCam.AddComponent<Camera> ();
			normalCam.gameObject.AddComponent<SilantroCamera> ();normalCam.GetComponent<SilantroCamera> ().FocusPoint = focalPoint;
		}
		//
		[MenuItem("Oyedoyin/Utilities/Black Box")]
		private static void AddBlackBox()
		{
			GameObject box;
			box = new GameObject ();
			//
			box.name = "Black Box";box.AddComponent<SilantroDataLogger> ();
			if (box.transform.parent != null) {
			//	box.GetComponent<SilantroDataLogger> ().Aircraft = box.transform.root.gameObject;
			}
		}
		//
		//
		[MenuItem("Oyedoyin/Utilities/Gravity Center")]
		private static void COG()
		{
			GameObject box;
			if (Selection.activeGameObject != null)
			{
				box = new GameObject ();
				box.transform.parent = Selection.activeGameObject.transform;
			} else {
				box = new GameObject ();
			}
			box.name = "COG";box.AddComponent<SilantroGravityCenter> ();box.tag = "Brain";box.AddComponent<SilantroInstrumentation> ();
			Rigidbody boxParent = box.transform.root.gameObject.GetComponent<Rigidbody> ();
			if (boxParent != null) {
				box.GetComponent<SilantroInstrumentation> ().airplane = boxParent;
			} else {
				Debug.Log ("Please parent Center of Gravity to Rigidbody Airplane");
			}
			AudioClip boom = (AudioClip)AssetDatabase.LoadAssetAtPath ("Assets/Sounds/Default/Extras/Sonic Boom.wav",typeof(AudioClip));
			GameObject flash = (GameObject)AssetDatabase.LoadAssetAtPath ("Assets/Prefabs/Default/Componets/Sonic Boom.prefab",typeof(GameObject));
			box.GetComponent<SilantroInstrumentation> ().sonicBoom = boom;box.GetComponent<SilantroInstrumentation> ().condenationEffect = flash.GetComponent<ParticleSystem>();
		}

		[MenuItem("Oyedoyin/Components/Controller")]
		private static void AddController()
		{
			GameObject plane;
			if (Selection.activeGameObject != null) {
				plane = Selection.activeGameObject;
				if (plane.GetComponentInChildren<SilantroFuelDistributor> () && plane.GetComponentInChildren<SilantroGearSystem> ()) {
					SilantroController control = plane.AddComponent<SilantroController> ();
					control.gearHelper = plane.GetComponentInChildren<SilantroGearSystem> ();
					control.fuelsystem = plane.GetComponentInChildren<SilantroFuelDistributor> ();
					control.gearHelper.control = control;
				} else {
					Debug.Log ("Controller can't be added to this Object!!>..Vital Components are missing");
				}
			} else {
				Debug.Log ("Please Select an Aircraft to add Controller to!!");
			}
			//	//

		}
		//
		[MenuItem("Oyedoyin/Components/Drag Multiplier")]
		private static void AddDrag()
		{
			GameObject plane;
			if (Selection.activeGameObject != null) {
				plane = Selection.activeGameObject;
				plane.AddComponent<SilantroDragMultiplier> ();
			}
			else {
				Debug.Log ("Please Select a Component to add Drag to!!");
			}
		}
		//
		[MenuItem("GameObject/Oyedoyin/Aircraft/Propeller Powered",false,0)]
		static void CreateNewPropllerPlane(MenuCommand command)
		{
			GameObject plane = new GameObject ("Propeller Plane");
			GameObjectUtility.SetParentAndAlign (plane, command.context as GameObject);
			//
			Undo.RegisterCreatedObjectUndo(plane,"Create " + plane.name);
			Selection.activeObject = plane;
			//
			//SETUP PLANE
			plane.AddComponent<Rigidbody>().mass = 1000f;
			GameObject cog = new GameObject ("COG");cog.transform.parent = plane.transform;cog.AddComponent<SilantroGravityCenter> ();cog.AddComponent<SilantroInstrumentation> ();cog.transform.localPosition = new Vector3 (0, 0, -1f);  cog.GetComponent<SilantroInstrumentation> ().aircraftType = SilantroInstrumentation.AircraftType.Propeller;cog.GetComponent<SilantroInstrumentation> ().airplane = plane.GetComponent<Rigidbody> ();cog.tag = "Brain";
			GameObject aerodynamics = new GameObject ("Aerodynamics");aerodynamics.transform.parent = plane.transform;
			GameObject body = new GameObject ("Body");body.transform.parent = plane.transform;
			GameObject engine = new GameObject ("TurboProp Engine");engine.transform.parent = plane.transform;
			GameObject wheelSystem = new GameObject ("Wheel System");wheelSystem.transform.parent = plane.transform;
			SilantroGearSystem wheelSys = wheelSystem.AddComponent<SilantroGearSystem> ();
			GameObject weapons = new GameObject ("Weapon System");weapons.transform.parent = plane.transform;
			GameObject cameras = new GameObject ("Camera System");cameras.transform.parent = plane.transform;


			//
		
			//ADD NECESSARY COMPONENTS
			SilantroTurboProp propEngine = engine.AddComponent<SilantroTurboProp> ();
			GameObject thruster = new GameObject ();
			thruster.name = "Thruster";
			thruster.transform.parent = engine.transform;
			thruster.transform.localPosition = new Vector3 (0, 0, -2);
			//
			//
			GameObject normalPropeller = new GameObject("Slow Propeller");
			GameObject fastPropller = new GameObject ("Fast Propeller");
			GameObject Props = new GameObject ("Propellers");
			//
			Props.transform.parent = engine.transform;
			fastPropller.transform.parent = Props.transform;normalPropeller.transform.parent = Props.transform;
			//
			propEngine.fastPropeller = fastPropller.transform;propEngine.Propeller = normalPropeller.transform;
			propEngine.Thruster = thruster.transform;propEngine.Parent = plane.GetComponent<Rigidbody>();
			//Sounds
			AudioClip start = (AudioClip)AssetDatabase.LoadAssetAtPath ("Assets/Sounds/Default/Engines/Default Start.wav",typeof(AudioClip));
			AudioClip stop = (AudioClip)AssetDatabase.LoadAssetAtPath ("Assets/Sounds/Default/Engines/Default Shutdown.wav",typeof(AudioClip));
			//AudioClip alert = (AudioClip)AssetDatabase.LoadAssetAtPath ("Assets/Sounds/Default/Engines/Default Fuel Alert.wav",typeof(AudioClip));
			AudioClip run = (AudioClip)AssetDatabase.LoadAssetAtPath ("Assets/Sounds/Default/Engines/Propeller/Propeller Running.wav",typeof(AudioClip));
			propEngine.EngineIdleSound = run;
			propEngine.EngineShutdownSound = stop;propEngine.EngineStartSound = start;
			//
			GameObject fuelSystem = new GameObject ("Fuel System");fuelSystem.transform.parent = plane.transform;fuelSystem.AddComponent<SilantroFuelDistributor>();
			GameObject tank = new GameObject ("Main Tank");tank.transform.parent = fuelSystem.transform; SilantroFuelTank tnk = tank.AddComponent<SilantroFuelTank> ();tnk.Capacity = 1000f;tnk.tankType = SilantroFuelTank.TankType.Internal;
			fuelSystem.GetComponent<SilantroFuelDistributor> ().internalFuelTank = tnk;
			//
			//SETUP WINGS
			GameObject leftWing = new GameObject("Left Wing");leftWing.transform.parent = aerodynamics.transform;leftWing.transform.localPosition = new Vector3(-1,0,0);leftWing.transform.localScale = new Vector3(-1,1,1);
			GameObject rightWing = new GameObject("Right Wing");rightWing.transform.parent = aerodynamics.transform;rightWing.transform.localPosition = new Vector3(1,0,0);
			GameObject leftTail = new GameObject("Left Tail");leftTail.transform.parent = aerodynamics.transform;leftTail.transform.localPosition = new Vector3(-1,0,-2);leftTail.transform.localScale = new Vector3(-1,1,1);
			GameObject rightTail = new GameObject("Right Tail");rightTail.transform.parent = aerodynamics.transform;rightTail.transform.localPosition = new Vector3(1,0,-2);
			GameObject rudder = new GameObject("Rudder");rudder.transform.parent = aerodynamics.transform;rudder.transform.localPosition = new Vector3(0,0.5f,-2);rudder.transform.localRotation = Quaternion.Euler (0, 0, 90);
			//ADD WING COMPONENTS
			SilantroAerofoil rightWingAerofoil = rightWing.AddComponent<SilantroAerofoil>();rightWingAerofoil.AerofoilSubdivisions = 5;
			rightWingAerofoil.controlSurface =rightWing.AddComponent<SilantroControlSurface> (); rightWingAerofoil.canBeControlled = true;rightWing.GetComponent<SilantroControlSurface> ().surfaceType = SilantroControlSurface.SurfaceType.Aileron;
			//
			SilantroAerofoil leftWingAerofoil = leftWing.AddComponent<SilantroAerofoil>();leftWingAerofoil.AerofoilSubdivisions = 5;
			leftWingAerofoil.controlSurface =leftWing.AddComponent<SilantroControlSurface> (); leftWingAerofoil.canBeControlled = true;leftWing.GetComponent<SilantroControlSurface> ().surfaceType = SilantroControlSurface.SurfaceType.Aileron;leftWing.GetComponent<SilantroControlSurface> ().negativeRotation = true;
			//
			SilantroAerofoil leftTailAerofoil = leftTail.AddComponent<SilantroAerofoil>();leftTailAerofoil.AerofoilSubdivisions = 3;leftTailAerofoil.aerofoilType = SilantroAerofoil.AerofoilType.Tail;
			leftTailAerofoil.controlSurface =leftTail.AddComponent<SilantroControlSurface> (); leftTailAerofoil.canBeControlled = true;leftTail.GetComponent<SilantroControlSurface> ().surfaceType = SilantroControlSurface.SurfaceType.Elevator;leftTail.GetComponent<SilantroControlSurface> ().negativeDeflection = true;
			//
			SilantroAerofoil rightTailAerofoil = rightTail.AddComponent<SilantroAerofoil>();rightTailAerofoil.AerofoilSubdivisions = 5;rightTailAerofoil.aerofoilType = SilantroAerofoil.AerofoilType.Tail;
			rightTailAerofoil.controlSurface =rightTail.AddComponent<SilantroControlSurface> (); rightTailAerofoil.canBeControlled = true;rightTail.GetComponent<SilantroControlSurface> ().surfaceType = SilantroControlSurface.SurfaceType.Elevator;
			//
			SilantroAerofoil rudderAerofoil = rudder.AddComponent<SilantroAerofoil>();rudderAerofoil.AerofoilSubdivisions = 3;rudderAerofoil.aerofoilType = SilantroAerofoil.AerofoilType.Tail;
			rudderAerofoil.controlSurface =rudder.AddComponent<SilantroControlSurface> (); rudderAerofoil.canBeControlled = true;rudder.GetComponent<SilantroControlSurface> ().surfaceType = SilantroControlSurface.SurfaceType.Rudder;
		//
			SilantroAirfoil wng = (SilantroAirfoil)AssetDatabase.LoadAssetAtPath ("Assets/Prefabs/Default/Airfoils/Default Wing 23016.prefab",typeof(SilantroAirfoil));
			SilantroAirfoil ctl = (SilantroAirfoil)AssetDatabase.LoadAssetAtPath ("Assets/Prefabs/Default/Airfoils/Default Control 0009.prefab",typeof(SilantroAirfoil));
			rightTailAerofoil.airfoil = ctl;rudderAerofoil.airfoil = ctl;leftTailAerofoil.airfoil = ctl;leftWingAerofoil.airfoil = wng;rightWingAerofoil.airfoil = wng;
			//
			//SETUP WHEELS
			GameObject frontWheel = new GameObject("Front Wheel");frontWheel.transform.parent = wheelSystem.transform;frontWheel.transform.localPosition = new Vector3(0,-0.5f,0);frontWheel.AddComponent<WheelCollider>();frontWheel.GetComponent<WheelCollider>().radius = 0.2f;
			GameObject leftWheel = new GameObject ("Left Wheel");leftWheel.transform.parent = wheelSystem.transform;leftWheel.transform.localPosition = new Vector3 (-1, -0.5f, -2);leftWheel.AddComponent<WheelCollider>();leftWheel.GetComponent<WheelCollider>().radius = 0.2f;
			GameObject rightWheel = new GameObject ("Right Wheel");rightWheel.transform.parent = wheelSystem.transform;rightWheel.transform.localPosition = new Vector3 (1, -0.5f, -2);rightWheel.AddComponent<WheelCollider>();rightWheel.GetComponent<WheelCollider>().radius = 0.2f;
		//
			//
			SilantroGearSystem.WheelSystem frontGearSystem = new SilantroGearSystem.WheelSystem ();frontGearSystem.collider = frontWheel.GetComponent<WheelCollider>();frontGearSystem.Identifier = "Front Gear";frontGearSystem.steerable = true;
			SilantroGearSystem.WheelSystem leftGearSystem = new SilantroGearSystem.WheelSystem ();leftGearSystem.collider = leftWheel.GetComponent<WheelCollider>();leftGearSystem.Identifier = "Left Gear";leftGearSystem.attachedMotor = true;
			SilantroGearSystem.WheelSystem rightGearSystem = new SilantroGearSystem.WheelSystem ();rightGearSystem.collider = rightWheel.GetComponent<WheelCollider>();rightGearSystem.Identifier = "Right Gear";rightGearSystem.attachedMotor = true;
			//
			wheelSys.wheelSystem.Add(frontGearSystem);wheelSys.wheelSystem.Add(leftGearSystem);wheelSys.wheelSystem.Add(rightGearSystem);
			AudioClip open = (AudioClip)AssetDatabase.LoadAssetAtPath ("Assets/Sounds/Default/Gear/Gear Open.wav",typeof(AudioClip));
			AudioClip close = (AudioClip)AssetDatabase.LoadAssetAtPath ("Assets/Sounds/Default/Gear/Gear Close.wav",typeof(AudioClip));
			wheelSys.gearUp = close;wheelSys.gearDown = open;
			//
			SilantroController control = plane.AddComponent<SilantroController> ();
			control.gearHelper = wheelSys;
			control.fuelsystem = plane.GetComponentInChildren<SilantroFuelDistributor> ();
			control.gearHelper.control = control;control.engineType = SilantroController.AircraftType.TurboProp;
			//
			CapsuleCollider col =plane.AddComponent<CapsuleCollider>();
			col.height = 5f;
			col.radius = 0.5f;
			col.direction = 2;
			//
			//
			AudioClip boom = (AudioClip)AssetDatabase.LoadAssetAtPath ("Assets/Sounds/Default/Extras/Sonic Boom.wav",typeof(AudioClip));
			GameObject flash = (GameObject)AssetDatabase.LoadAssetAtPath ("Assets/Prefabs/Default/Componets/Sonic Boom.prefab",typeof(GameObject));
			cog.GetComponent<SilantroInstrumentation> ().sonicBoom = boom;cog.GetComponent<SilantroInstrumentation> ().condenationEffect = flash.GetComponent<ParticleSystem>();

			//
			GameObject minigunSystem = new GameObject("Minigun System");minigunSystem.transform.parent = weapons.transform;
			GameObject missileSystem = new GameObject("Missile System");missileSystem.transform.parent = weapons.transform;
			GameObject bombSystem = new GameObject("Bomb System");bombSystem.transform.parent = weapons.transform;
			//
			GameObject focalPoint = new GameObject ("Camera Focus Point");focalPoint.transform.parent = plane.transform;
			GameObject normalCam = new GameObject("Default Camera");
			normalCam.gameObject.transform.parent = cameras.transform;normalCam.AddComponent<Camera> ();
			normalCam.gameObject.AddComponent<SilantroCamera> ();normalCam.GetComponent<SilantroCamera> ().FocusPoint = focalPoint;
		}
		//
		//
		//
		//
		[MenuItem("GameObject/Oyedoyin/Aircraft/Jet/TurboFan Powered",false,0)]
		static void CreateNewTurbofanPlane(MenuCommand command)
		{
			GameObject plane = new GameObject ("Jet Plane");
			GameObjectUtility.SetParentAndAlign (plane, command.context as GameObject);
			//
			Undo.RegisterCreatedObjectUndo(plane,"Create " + plane.name);
			Selection.activeObject = plane;
			//
			//SETUP PLANE
			plane.AddComponent<Rigidbody>().mass = 1000f;
			GameObject cog = new GameObject ("COG");cog.transform.parent = plane.transform;cog.AddComponent<SilantroGravityCenter> ();cog.AddComponent<SilantroInstrumentation> ();cog.transform.localPosition = new Vector3 (0, 0, -1f);  cog.GetComponent<SilantroInstrumentation> ().aircraftType = SilantroInstrumentation.AircraftType.Jet;cog.GetComponent<SilantroInstrumentation> ().airplane = plane.GetComponent<Rigidbody> ();cog.tag = "Brain";
			GameObject aerodynamics = new GameObject ("Aerodynamics");aerodynamics.transform.parent = plane.transform;
			GameObject body = new GameObject ("Body");body.transform.parent = plane.transform;
			GameObject engine = new GameObject ("TurboFan Engine");engine.transform.parent = plane.transform;
			GameObject wheelSystem = new GameObject ("Wheel System");wheelSystem.transform.parent = plane.transform;
			SilantroGearSystem wheelSys = wheelSystem.AddComponent<SilantroGearSystem> ();
			GameObject weapons = new GameObject ("Weapon System");weapons.transform.parent = plane.transform;
			GameObject cameras = new GameObject ("Camera System");cameras.transform.parent = plane.transform;

			//
			//ADD NECESSARY COMPONENTS
			SilantroTurboFan fanEngine = engine.AddComponent<SilantroTurboFan> ();
			GameObject thruster = new GameObject ();
			thruster.name = "Thruster";
			thruster.transform.parent = engine.transform;
			thruster.transform.localPosition = new Vector3 (0, 0, -2);
			//
			GameObject fan = new GameObject ();
			fan.name= "Fan";
			fan.transform.parent = engine.transform;
			fan.transform.localPosition = new Vector3 (0, 0, 2);
			//
			//
			GameObject effects = new GameObject("Engine Effects");
			effects.transform.parent = engine.transform;
			effects.transform.localPosition = new Vector3 (0, 0, -2);
			//
			GameObject smoke = (GameObject)AssetDatabase.LoadAssetAtPath ("Assets/Prefabs/Default/Componets/Exhaust Smoke.prefab",typeof(GameObject));
			GameObject smokeEffect = GameObject.Instantiate (smoke, effects.transform.position, Quaternion.Euler(0,-180,0),effects.transform);
			SilantroEngineEffect engineEffects = effects.AddComponent<SilantroEngineEffect> ();
			engineEffects.exhaustSmoke = smokeEffect.GetComponent<ParticleSystem> ();engineEffects.engine = engine;engineEffects.engineType = SilantroEngineEffect.EngineType.TurboFan;
			//
			fanEngine.Thruster = thruster.transform;fanEngine.fan = fan.transform;fanEngine.Parent = plane.GetComponent<Rigidbody>();
			AudioClip start = (AudioClip)AssetDatabase.LoadAssetAtPath ("Assets/Sounds/Default/Engines/Default Start.wav",typeof(AudioClip));
			AudioClip stop = (AudioClip)AssetDatabase.LoadAssetAtPath ("Assets/Sounds/Default/Engines/Default Shutdown.wav",typeof(AudioClip));
		//	AudioClip alert = (AudioClip)AssetDatabase.LoadAssetAtPath ("Assets/Sounds/Default/Engines/Default Fuel Alert.wav",typeof(AudioClip));
			AudioClip run = (AudioClip)AssetDatabase.LoadAssetAtPath ("Assets/Sounds/Default/Engines/Jet/Jet Running.wav",typeof(AudioClip));
			fanEngine.EngineIdleSound = run;
			fanEngine.EngineShutdownSound = stop;fanEngine.EngineStartSound = start;
			//
			//
			AudioClip boom = (AudioClip)AssetDatabase.LoadAssetAtPath ("Assets/Sounds/Default/Extras/Sonic Boom.wav",typeof(AudioClip));
			GameObject flash = (GameObject)AssetDatabase.LoadAssetAtPath ("Assets/Prefabs/Default/Componets/Sonic Boom.prefab",typeof(GameObject));
			cog.GetComponent<SilantroInstrumentation> ().sonicBoom = boom;cog.GetComponent<SilantroInstrumentation> ().condenationEffect = flash.GetComponent<ParticleSystem>();

			//
			//
			//SETUP WINGS
			GameObject leftWing = new GameObject("Left Wing");leftWing.transform.parent = aerodynamics.transform;leftWing.transform.localPosition = new Vector3(-1,0,0);leftWing.transform.localScale = new Vector3(-1,1,1);
			GameObject rightWing = new GameObject("Right Wing");rightWing.transform.parent = aerodynamics.transform;rightWing.transform.localPosition = new Vector3(1,0,0);
			GameObject leftTail = new GameObject("Left Tail");leftTail.transform.parent = aerodynamics.transform;leftTail.transform.localPosition = new Vector3(-1,0,-2);leftTail.transform.localScale = new Vector3(-1,1,1);
			GameObject rightTail = new GameObject("Right Tail");rightTail.transform.parent = aerodynamics.transform;rightTail.transform.localPosition = new Vector3(1,0,-2);
			GameObject rudder = new GameObject("Rudder");rudder.transform.parent = aerodynamics.transform;rudder.transform.localPosition = new Vector3(0,0.5f,-2);rudder.transform.localRotation = Quaternion.Euler (0, 0, 90);
			//ADD WING COMPONENTS
			SilantroAerofoil rightWingAerofoil = rightWing.AddComponent<SilantroAerofoil>();rightWingAerofoil.AerofoilSubdivisions = 5;
			rightWingAerofoil.controlSurface =rightWing.AddComponent<SilantroControlSurface> (); rightWingAerofoil.canBeControlled = true;rightWing.GetComponent<SilantroControlSurface> ().surfaceType = SilantroControlSurface.SurfaceType.Aileron;
			//
			SilantroAerofoil leftWingAerofoil = leftWing.AddComponent<SilantroAerofoil>();leftWingAerofoil.AerofoilSubdivisions = 5;
			leftWingAerofoil.controlSurface =leftWing.AddComponent<SilantroControlSurface> (); leftWingAerofoil.canBeControlled = true;leftWing.GetComponent<SilantroControlSurface> ().surfaceType = SilantroControlSurface.SurfaceType.Aileron;leftWing.GetComponent<SilantroControlSurface> ().negativeRotation = true;
			//
			SilantroAerofoil leftTailAerofoil = leftTail.AddComponent<SilantroAerofoil>();leftTailAerofoil.AerofoilSubdivisions = 3;leftTailAerofoil.aerofoilType = SilantroAerofoil.AerofoilType.Tail;
			leftTailAerofoil.controlSurface =leftTail.AddComponent<SilantroControlSurface> (); leftTailAerofoil.canBeControlled = true;leftTail.GetComponent<SilantroControlSurface> ().surfaceType = SilantroControlSurface.SurfaceType.Elevator;leftTail.GetComponent<SilantroControlSurface> ().negativeDeflection = true;
			//
			SilantroAerofoil rightTailAerofoil = rightTail.AddComponent<SilantroAerofoil>();rightTailAerofoil.AerofoilSubdivisions = 5;rightTailAerofoil.aerofoilType = SilantroAerofoil.AerofoilType.Tail;
			rightTailAerofoil.controlSurface =rightTail.AddComponent<SilantroControlSurface> (); rightTailAerofoil.canBeControlled = true;rightTail.GetComponent<SilantroControlSurface> ().surfaceType = SilantroControlSurface.SurfaceType.Elevator;
			//
			SilantroAerofoil rudderAerofoil = rudder.AddComponent<SilantroAerofoil>();rudderAerofoil.AerofoilSubdivisions = 3;rudderAerofoil.aerofoilType = SilantroAerofoil.AerofoilType.Tail;
			rudderAerofoil.controlSurface =rudder.AddComponent<SilantroControlSurface> (); rudderAerofoil.canBeControlled = true;rudder.GetComponent<SilantroControlSurface> ().surfaceType = SilantroControlSurface.SurfaceType.Rudder;
			//
			SilantroAirfoil wng = (SilantroAirfoil)AssetDatabase.LoadAssetAtPath ("Assets/Prefabs/Default/Airfoils/Default Wing 23016.prefab",typeof(SilantroAirfoil));
			SilantroAirfoil ctl = (SilantroAirfoil)AssetDatabase.LoadAssetAtPath ("Assets/Prefabs/Default/Airfoils/Default Control 0009.prefab",typeof(SilantroAirfoil));
			rightTailAerofoil.airfoil = ctl;rudderAerofoil.airfoil = ctl;leftTailAerofoil.airfoil = ctl;leftWingAerofoil.airfoil = wng;rightWingAerofoil.airfoil = wng;
			//
			GameObject fuelSystem = new GameObject ("Fuel System");fuelSystem.transform.parent = plane.transform;fuelSystem.AddComponent<SilantroFuelDistributor>();
			GameObject tank = new GameObject ("Main Tank");tank.transform.parent = fuelSystem.transform; SilantroFuelTank tnk = tank.AddComponent<SilantroFuelTank> ();tnk.Capacity = 1000f;tnk.tankType = SilantroFuelTank.TankType.Internal;
			fuelSystem.GetComponent<SilantroFuelDistributor> ().internalFuelTank = tnk;
			//
			//SETUP WHEELS
			GameObject frontWheel = new GameObject("Front Wheel");frontWheel.transform.parent = wheelSystem.transform;frontWheel.transform.localPosition = new Vector3(0,-0.5f,0);frontWheel.AddComponent<WheelCollider>();frontWheel.GetComponent<WheelCollider>().radius = 0.2f;
			GameObject leftWheel = new GameObject ("Left Wheel");leftWheel.transform.parent = wheelSystem.transform;leftWheel.transform.localPosition = new Vector3 (-1, -0.5f, -2);leftWheel.AddComponent<WheelCollider>();leftWheel.GetComponent<WheelCollider>().radius = 0.2f;
			GameObject rightWheel = new GameObject ("Right Wheel");rightWheel.transform.parent = wheelSystem.transform;rightWheel.transform.localPosition = new Vector3 (1, -0.5f, -2);rightWheel.AddComponent<WheelCollider>();rightWheel.GetComponent<WheelCollider>().radius = 0.2f;
			//
			//
			SilantroGearSystem.WheelSystem frontGearSystem = new SilantroGearSystem.WheelSystem ();frontGearSystem.collider = frontWheel.GetComponent<WheelCollider>();frontGearSystem.Identifier = "Front Gear";frontGearSystem.steerable = true;
			SilantroGearSystem.WheelSystem leftGearSystem = new SilantroGearSystem.WheelSystem ();leftGearSystem.collider = leftWheel.GetComponent<WheelCollider>();leftGearSystem.Identifier = "Left Gear";leftGearSystem.attachedMotor = true;
			SilantroGearSystem.WheelSystem rightGearSystem = new SilantroGearSystem.WheelSystem ();rightGearSystem.collider = rightWheel.GetComponent<WheelCollider>();rightGearSystem.Identifier = "Right Gear";rightGearSystem.attachedMotor = true;
			//
			wheelSys.wheelSystem.Add(frontGearSystem);wheelSys.wheelSystem.Add(leftGearSystem);wheelSys.wheelSystem.Add(rightGearSystem);
			AudioClip open = (AudioClip)AssetDatabase.LoadAssetAtPath ("Assets/Sounds/Default/Gear/Gear Open.wav",typeof(AudioClip));
			AudioClip close = (AudioClip)AssetDatabase.LoadAssetAtPath ("Assets/Sounds/Default/Gear/Gear Close.wav",typeof(AudioClip));
			wheelSys.gearUp = close;wheelSys.gearDown = open;
			//
			GameObject minigunSystem = new GameObject("Minigun System");minigunSystem.transform.parent = weapons.transform;
			GameObject missileSystem = new GameObject("Missile System");missileSystem.transform.parent = weapons.transform;
			GameObject bombSystem = new GameObject("Bomb System");bombSystem.transform.parent = weapons.transform;
			//
			//
			SilantroController control = plane.AddComponent<SilantroController> ();
			control.gearHelper = wheelSys;
			control.fuelsystem = plane.GetComponentInChildren<SilantroFuelDistributor> ();
			control.gearHelper.control = control;control.engineType = SilantroController.AircraftType.TurboFan;
			//
			CapsuleCollider col = plane.AddComponent<CapsuleCollider>();
			col.height = 5f;
			col.radius = 0.5f;
			col.direction = 2;
			//
			GameObject normalCam = new GameObject("Default Camera");GameObject focalPoint = new GameObject ("Camera Focus Point");focalPoint.transform.parent = plane.transform;
			normalCam.gameObject.transform.parent = cameras.transform;normalCam.AddComponent<Camera> ();
			normalCam.gameObject.AddComponent<SilantroCamera> ();normalCam.GetComponent<SilantroCamera> ().FocusPoint = focalPoint;
		}
		//
		//
		//
		//
		//
		[MenuItem("GameObject/Oyedoyin/Aircraft/Jet/TurboJet Powered",false,0)]
		static void CreateNewTurboJetPlane(MenuCommand command)
		{
			GameObject plane = new GameObject ("Jet Plane");
			GameObjectUtility.SetParentAndAlign (plane, command.context as GameObject);
			//
			Undo.RegisterCreatedObjectUndo(plane,"Create " + plane.name);
			Selection.activeObject = plane;
			//
			//SETUP PLANE
			plane.AddComponent<Rigidbody>().mass = 1000f;
			GameObject cog = new GameObject ("COG");cog.transform.parent = plane.transform;cog.AddComponent<SilantroGravityCenter> ();cog.AddComponent<SilantroInstrumentation> ();cog.transform.localPosition = new Vector3 (0, 0, -1f);  cog.GetComponent<SilantroInstrumentation> ().aircraftType = SilantroInstrumentation.AircraftType.Jet;cog.GetComponent<SilantroInstrumentation> ().airplane = plane.GetComponent<Rigidbody> ();cog.tag = "Brain";
			GameObject aerodynamics = new GameObject ("Aerodynamics");aerodynamics.transform.parent = plane.transform;
			GameObject body = new GameObject ("Body");body.transform.parent = plane.transform;
			GameObject engine = new GameObject ("TurboJet Engine");engine.transform.parent = plane.transform;
			GameObject wheelSystem = new GameObject ("Wheel System");wheelSystem.transform.parent = plane.transform;
			SilantroGearSystem wheelSys =  wheelSystem.AddComponent<SilantroGearSystem> ();
			GameObject weapons = new GameObject ("Weapon System");weapons.transform.parent = plane.transform;
			GameObject cameras = new GameObject ("Camera System");cameras.transform.parent = plane.transform;

			//
			GameObject fuelSystem = new GameObject ("Fuel System");fuelSystem.transform.parent = plane.transform;fuelSystem.AddComponent<SilantroFuelDistributor>();
			GameObject tank = new GameObject ("Main Tank");tank.transform.parent = fuelSystem.transform; SilantroFuelTank tnk = tank.AddComponent<SilantroFuelTank> ();tnk.Capacity = 1000f;tnk.tankType = SilantroFuelTank.TankType.Internal;
			fuelSystem.GetComponent<SilantroFuelDistributor> ().internalFuelTank = tnk;
			//
			//ADD NECESSARY COMPONENTS
			SilantroTurboJet fanEngine = engine.AddComponent<SilantroTurboJet> ();
			GameObject thruster = new GameObject ();
			thruster.name = "Thruster";
			thruster.transform.parent = engine.transform;
			thruster.transform.localPosition = new Vector3 (0, 0, -2);
			//
			GameObject fan = new GameObject ();
			fan.name= "Fan";
			fan.transform.parent = engine.transform;
			fan.transform.localPosition = new Vector3 (0, 0, 2);
			//
			//
			AudioClip boom = (AudioClip)AssetDatabase.LoadAssetAtPath ("Assets/Sounds/Default/Extras/Sonic Boom.wav",typeof(AudioClip));
			GameObject flash = (GameObject)AssetDatabase.LoadAssetAtPath ("Assets/Prefabs/Default/Componets/Sonic Boom.prefab",typeof(GameObject));
			cog.GetComponent<SilantroInstrumentation> ().sonicBoom = boom;cog.GetComponent<SilantroInstrumentation> ().condenationEffect = flash.GetComponent<ParticleSystem>();

			//
			//
			GameObject effects = new GameObject("Engine Effects");
			effects.transform.parent = engine.transform;
			effects.transform.localPosition = new Vector3 (0, 0, -2);
			//
			GameObject smoke = (GameObject)AssetDatabase.LoadAssetAtPath ("Assets/Prefabs/Default/Componets/Exhaust Smoke.prefab",typeof(GameObject));
			GameObject smokeEffect = GameObject.Instantiate (smoke, effects.transform.position, Quaternion.Euler(0,-180,0),effects.transform);
			SilantroEngineEffect engineEffects = effects.AddComponent<SilantroEngineEffect> ();
			engineEffects.exhaustSmoke = smokeEffect.GetComponent<ParticleSystem> ();engineEffects.engine = engine;engineEffects.engineType = SilantroEngineEffect.EngineType.TurboJet;
			//
			fanEngine.Thruster = thruster.transform;fanEngine.intakeFanPoint = fan.transform;fanEngine.Parent = plane.GetComponent<Rigidbody>();
			//Sounds
			AudioClip start = (AudioClip)AssetDatabase.LoadAssetAtPath ("Assets/Sounds/Default/Engines/Default Start.wav",typeof(AudioClip));
			AudioClip stop = (AudioClip)AssetDatabase.LoadAssetAtPath ("Assets/Sounds/Default/Engines/Default Shutdown.wav",typeof(AudioClip));
			AudioClip alert = (AudioClip)AssetDatabase.LoadAssetAtPath ("Assets/Sounds/Default/Engines/Default Fuel Alert.wav",typeof(AudioClip));
			AudioClip run = (AudioClip)AssetDatabase.LoadAssetAtPath ("Assets/Sounds/Default/Engines/Jet/Jet Running.wav",typeof(AudioClip));
			fanEngine.EngineIdleSound = run;
			fanEngine.EngineShutdownSound = stop;fanEngine.EngineStartSound = start;//
			//
			//SETUP WINGS
			GameObject leftWing = new GameObject("Left Wing");leftWing.transform.parent = aerodynamics.transform;leftWing.transform.localPosition = new Vector3(-1,0,0);leftWing.transform.localScale = new Vector3(-1,1,1);
			GameObject rightWing = new GameObject("Right Wing");rightWing.transform.parent = aerodynamics.transform;rightWing.transform.localPosition = new Vector3(1,0,0);
			GameObject leftTail = new GameObject("Left Tail");leftTail.transform.parent = aerodynamics.transform;leftTail.transform.localPosition = new Vector3(-1,0,-2);leftTail.transform.localScale = new Vector3(-1,1,1);
			GameObject rightTail = new GameObject("Right Tail");rightTail.transform.parent = aerodynamics.transform;rightTail.transform.localPosition = new Vector3(1,0,-2);
			GameObject rudder = new GameObject("Rudder");rudder.transform.parent = aerodynamics.transform;rudder.transform.localPosition = new Vector3(0,0.5f,-2);rudder.transform.localRotation = Quaternion.Euler (0, 0, 90);
			//ADD WING COMPONENTS
			SilantroAerofoil rightWingAerofoil = rightWing.AddComponent<SilantroAerofoil>();rightWingAerofoil.AerofoilSubdivisions = 5;
			rightWingAerofoil.controlSurface =rightWing.AddComponent<SilantroControlSurface> (); rightWingAerofoil.canBeControlled = true;rightWing.GetComponent<SilantroControlSurface> ().surfaceType = SilantroControlSurface.SurfaceType.Aileron;
			//
			SilantroAerofoil leftWingAerofoil = leftWing.AddComponent<SilantroAerofoil>();leftWingAerofoil.AerofoilSubdivisions = 5;
			leftWingAerofoil.controlSurface =leftWing.AddComponent<SilantroControlSurface> (); leftWingAerofoil.canBeControlled = true;leftWing.GetComponent<SilantroControlSurface> ().surfaceType = SilantroControlSurface.SurfaceType.Aileron;leftWing.GetComponent<SilantroControlSurface> ().negativeRotation = true;
			//
			SilantroAerofoil leftTailAerofoil = leftTail.AddComponent<SilantroAerofoil>();leftTailAerofoil.AerofoilSubdivisions = 3;leftTailAerofoil.aerofoilType = SilantroAerofoil.AerofoilType.Tail;
			leftTailAerofoil.controlSurface =leftTail.AddComponent<SilantroControlSurface> (); leftTailAerofoil.canBeControlled = true;leftTail.GetComponent<SilantroControlSurface> ().surfaceType = SilantroControlSurface.SurfaceType.Elevator;leftTail.GetComponent<SilantroControlSurface> ().negativeDeflection = true;
			//
			SilantroAerofoil rightTailAerofoil = rightTail.AddComponent<SilantroAerofoil>();rightTailAerofoil.AerofoilSubdivisions = 5;rightTailAerofoil.aerofoilType = SilantroAerofoil.AerofoilType.Tail;
			rightTailAerofoil.controlSurface =rightTail.AddComponent<SilantroControlSurface> (); rightTailAerofoil.canBeControlled = true;rightTail.GetComponent<SilantroControlSurface> ().surfaceType = SilantroControlSurface.SurfaceType.Elevator;
			//
			SilantroAerofoil rudderAerofoil = rudder.AddComponent<SilantroAerofoil>();rudderAerofoil.AerofoilSubdivisions = 3;rudderAerofoil.aerofoilType = SilantroAerofoil.AerofoilType.Tail;
			rudderAerofoil.controlSurface =rudder.AddComponent<SilantroControlSurface> (); rudderAerofoil.canBeControlled = true;rudder.GetComponent<SilantroControlSurface> ().surfaceType = SilantroControlSurface.SurfaceType.Rudder;
			//
			SilantroAirfoil wng = (SilantroAirfoil)AssetDatabase.LoadAssetAtPath ("Assets/Prefabs/Default/Airfoils/Default Wing 23016.prefab",typeof(SilantroAirfoil));
			SilantroAirfoil ctl = (SilantroAirfoil)AssetDatabase.LoadAssetAtPath ("Assets/Prefabs/Default/Airfoils/Default Control 0009.prefab",typeof(SilantroAirfoil));
			rightTailAerofoil.airfoil = ctl;rudderAerofoil.airfoil = ctl;leftTailAerofoil.airfoil = ctl;leftWingAerofoil.airfoil = wng;rightWingAerofoil.airfoil = wng;
			//
			//SETUP WHEELS
			GameObject frontWheel = new GameObject("Front Wheel");frontWheel.transform.parent = wheelSystem.transform;frontWheel.transform.localPosition = new Vector3(0,-0.5f,0);frontWheel.AddComponent<WheelCollider>();frontWheel.GetComponent<WheelCollider>().radius = 0.2f;
			GameObject leftWheel = new GameObject ("Left Wheel");leftWheel.transform.parent = wheelSystem.transform;leftWheel.transform.localPosition = new Vector3 (-1, -0.5f, -2);leftWheel.AddComponent<WheelCollider>();leftWheel.GetComponent<WheelCollider>().radius = 0.2f;
			GameObject rightWheel = new GameObject ("Right Wheel");rightWheel.transform.parent = wheelSystem.transform;rightWheel.transform.localPosition = new Vector3 (1, -0.5f, -2);rightWheel.AddComponent<WheelCollider>();rightWheel.GetComponent<WheelCollider>().radius = 0.2f;
			//
			//
			SilantroGearSystem.WheelSystem frontGearSystem = new SilantroGearSystem.WheelSystem ();frontGearSystem.collider = frontWheel.GetComponent<WheelCollider>();frontGearSystem.Identifier = "Front Gear";frontGearSystem.steerable = true;
			SilantroGearSystem.WheelSystem leftGearSystem = new SilantroGearSystem.WheelSystem ();leftGearSystem.collider = leftWheel.GetComponent<WheelCollider>();leftGearSystem.Identifier = "Left Gear";leftGearSystem.attachedMotor = true;
			SilantroGearSystem.WheelSystem rightGearSystem = new SilantroGearSystem.WheelSystem ();rightGearSystem.collider = rightWheel.GetComponent<WheelCollider>();rightGearSystem.Identifier = "Right Gear";rightGearSystem.attachedMotor = true;
			//
			wheelSys.wheelSystem.Add(frontGearSystem);wheelSys.wheelSystem.Add(leftGearSystem);wheelSys.wheelSystem.Add(rightGearSystem);
			AudioClip open = (AudioClip)AssetDatabase.LoadAssetAtPath ("Assets/Sounds/Default/Gear/Gear Open.wav",typeof(AudioClip));
			AudioClip close = (AudioClip)AssetDatabase.LoadAssetAtPath ("Assets/Sounds/Default/Gear/Gear Close.wav",typeof(AudioClip));
			wheelSys.gearUp = close;wheelSys.gearDown = open;
			//
			GameObject minigunSystem = new GameObject("Minigun System");minigunSystem.transform.parent = weapons.transform;
			GameObject missileSystem = new GameObject("Missile System");missileSystem.transform.parent = weapons.transform;
			GameObject bombSystem = new GameObject("Bomb System");bombSystem.transform.parent = weapons.transform;
			//
			//
			SilantroController control = plane.AddComponent<SilantroController> ();
			control.gearHelper = wheelSys;
			control.fuelsystem = plane.GetComponentInChildren<SilantroFuelDistributor> ();
			control.gearHelper.control = control;control.engineType = SilantroController.AircraftType.TurboJet;
			//
			CapsuleCollider col =plane.AddComponent<CapsuleCollider>();
			col.height = 5f;
			col.radius = 0.5f;
			col.direction = 2;
			//
			GameObject normalCam = new GameObject("Default Camera");GameObject focalPoint = new GameObject ("Camera Focus Point");focalPoint.transform.parent = plane.transform;
			normalCam.gameObject.transform.parent = cameras.transform;normalCam.AddComponent<Camera> ();
			normalCam.gameObject.AddComponent<SilantroCamera> ();normalCam.GetComponent<SilantroCamera> ().FocusPoint = focalPoint;
		}
	}
}
