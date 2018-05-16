using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OculusJoystick : MonoBehaviour {
    Quaternion rot;
    OVRInput.Controller activeController;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        activeController = OVRInput.GetActiveController();
        rot = OVRInput.GetLocalControllerRotation(activeController);
        //GameObject.Find("debug").GetComponent<UnityEngine.UI.Text>().text = rot.ToString();

        transform.rotation = Quaternion.Inverse(new Quaternion(rot.x,-rot.y,rot.z,rot.w));
    }
}
