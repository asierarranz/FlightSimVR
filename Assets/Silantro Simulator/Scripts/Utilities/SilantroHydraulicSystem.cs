//
//Property of Oyedoyin Dada
//cc dadaoyedoyin@gmail.com
//
//
//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[AddComponentMenu("Oyedoyin/Components/Hydraulics")]
public class SilantroHydraulicSystem : MonoBehaviour {
	//

	[Header("Door Elements")]
	public List<Door> doors = new List<Door>() ;

	[System.Serializable]
	public class Door
	{
		public string Identifier;
		public Transform doorElement;
		[Header("Rotaion Amount")]
		public float xAxis;
		public float yAxis;
		public float zAxis;
		[HideInInspector]public Quaternion initialPosition;
		[HideInInspector]public Quaternion finalPosition;
	

	}

	[Header("Switches")]
	public bool open;
	[HideInInspector]public bool opened = false;
	bool activated;

	public bool close ;
	[HideInInspector]public bool closed= true;
	//
	[Header("Control Values")]
	public float openTime = 5f;
	public float closeTime = 5f;
	public float rotateSpeed = 10f;
	[Header("Sound Effect")]
	public AudioClip openSound;
	public AudioClip closeSound;
	AudioSource doorSound;
// Use this for initialization
	void Start () {
		//
		GameObject soundPoint = new GameObject();
		soundPoint.transform.parent = this.transform;
		soundPoint.transform.localPosition = new Vector3 (0, 0, 0);
		soundPoint.name = this.name +" Sound Point";
		//
		doorSound = soundPoint.AddComponent<AudioSource>();
	
		//
		foreach (Door door in doors) {
			door.initialPosition = door.doorElement.localRotation;
			door.finalPosition = Quaternion.Euler (door.xAxis, door.yAxis, door.zAxis);
			}
		//
	}
	
	// Update is called once per frame
	void Update () {
		if (open && !opened) {
			
			foreach (Door door in doors) {
				door.doorElement.localRotation = Quaternion.RotateTowards (door.doorElement.localRotation, door.finalPosition, Time.deltaTime * rotateSpeed);
			}
			if (!activated) {
				StartCoroutine (Open ());
				activated = true;
				if (openSound != null) {
					doorSound.PlayOneShot (openSound);
				}
			}
		}
		if (close && !closed ) {

			foreach (Door door in doors) {
				door.doorElement.localRotation = Quaternion.RotateTowards (door.doorElement.localRotation, door.initialPosition, Time.deltaTime * rotateSpeed);
			}
			if (!activated) {
				StartCoroutine (Close ());
				activated = true;
				if (closeSound != null) {
					doorSound.PlayOneShot (closeSound);
				}
			}
		}
	}

	//
	IEnumerator Open()
	{
		yield return new WaitForSeconds (openTime);
		opened = true;closed =false;CloseSwitches ();
		activated = false;

	}
	IEnumerator Close()
	{
		yield return new WaitForSeconds (closeTime);
		closed = true;opened = false;CloseSwitches ();
		activated = false;

	}
	void CloseSwitches()
	{
		open =  false;
		close = false;
	}
}
