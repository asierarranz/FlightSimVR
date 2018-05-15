using UnityEngine;
using UnityEngine.UI;

public class PanelControls : MonoBehaviour
{
	//Make sure to attach these Buttons in the Inspector
	public Button m_StartEngine, m_ThrottleUp,m_Brakes;
	public bool engineStart = false;
	public bool engineShutdown = false;
	public bool engineThrottleUp = false;
	public bool engineThrottleDown = false;
	public bool brakesButton = false;



	void Start()
	{
		Button btn = m_StartEngine.GetComponent<Button>();
		Button btn2 = m_ThrottleUp.GetComponent<Button>();
		Button btn3 = m_Brakes.GetComponent<Button>();
		//Calls the TaskOnClick method when you click the Button
		btn.onClick.AddListener(StartEngine);
		btn2.onClick.AddListener(ThrottleUp);
		btn3.onClick.AddListener(Brakes);

		//m_ThrottleUp.onClick.AddListener(delegate {TaskWithParameters("Hello"); });
	}

	void StartEngine()
	{
		engineStart = true;
	}

	void ThrottleUp()
	{
		engineThrottleUp = true;
	}
	void Brakes()
	{
		brakesButton = true;
	}

	void Update()
	{
		
	}

}