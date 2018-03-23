using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBehaviour : MonoBehaviour {

	public GameObject lightCurveWindow;
	public GameObject aboutWindow;
	public GameObject helpWindow;
	public GameObject emailWindow;
	public GameObject progressDialogWindow;
	public GameObject optionWindow;

	public GameObject worldCanvas;
	public GameObject rocheLobuleText;

	private bool optionWindowStats = false;
	private bool optionHelpStats = false;
	private bool optionAboutStats = false;
	private bool optionHideStats = false;

	public GameObject massText;
	public GameObject angleText;
	public GameObject primaryTemperatureText;
	public GameObject secondaryTemperatureText;

	public GameObject curvesButton;

	// Use this for initialization
	void Start () {
		lightCurveWindow.SetActive (false);
		aboutWindow.SetActive (optionAboutStats);
		helpWindow.SetActive (optionHelpStats);
		emailWindow.SetActive (false);
		progressDialogWindow.SetActive (false);
		optionWindow.SetActive (optionWindowStats);
		worldCanvas.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SetOptionWindow () 
	{
		optionWindowStats = !optionWindowStats;
		optionWindow.SetActive (optionWindowStats);
    }

	public void SetHelpWindow () 
	{
		optionHelpStats = !optionHelpStats;
		helpWindow.SetActive (optionHelpStats);
		SetOptionWindow ();
	}

	public void SetAboutWindow () 
	{
		optionAboutStats = !optionAboutStats;
		aboutWindow.SetActive (optionAboutStats);
		SetOptionWindow ();
	}

	public void CloseWindows () 
	{
		optionWindowStats = false;
		optionWindow.SetActive (optionWindowStats);

		optionHelpStats = false;
		helpWindow.SetActive (optionHelpStats);

		optionAboutStats = false;
		aboutWindow.SetActive (optionAboutStats);
	}

	public void setMass (Slider slider)
	{
		massText.GetComponent<Text> ().text = "" + slider.value / 100;
	}

	public void setAngle (Slider slider)
	{
		angleText.GetComponent<Text> ().text = slider.value + "º";
	}

	public void setPrimaryTemperature (Slider slider)
	{
		primaryTemperatureText.GetComponent<Text> ().text = slider.value + "000";
	}

	public void setSecondaryTemperature (Slider slider)
	{
		secondaryTemperatureText.GetComponent<Text> ().text = slider.value + "000";
	}

	public void setWorldCanvas (bool value) 
	{
		worldCanvas.SetActive (value);
	}

	public void setLightCurve (bool value) 
	{
		lightCurveWindow.SetActive (value);
	}

	public void openUrl(string url)
	{
		Application.OpenURL("http://www.bcc.unifal-mg.edu.br/lte/");
	}

	public void setLabel (bool rocheLobule) 
	{
		GameObject text;

		text = GameObject.Find ("L1Text");
		text.transform.localPosition = -GameObject.Find("L1").transform.position;
		text.transform.rotation = Camera.main.transform.rotation;

		text = GameObject.Find ("L2Text");
		text.transform.localPosition = -GameObject.Find("L2").transform.position;
		text.transform.rotation = Camera.main.transform.rotation;

		text = GameObject.Find ("L3Text");
		text.transform.localPosition = -GameObject.Find("L3").transform.position;
		text.transform.rotation = Camera.main.transform.rotation;

		text = GameObject.Find ("PrimaryText");
		text.transform.position = GameObject.Find("White Dwarf").transform.position;
		text.transform.rotation = Camera.main.transform.rotation;

		text = GameObject.Find ("SecondaryText");
		text.transform.position = GameObject.Find("Red Giant").transform.position;
		text.transform.rotation = Camera.main.transform.rotation;

		rocheLobuleText.SetActive (rocheLobule);
		if (rocheLobule) 
		{
			rocheLobuleText.transform.position = GameObject.Find ("White Dwarf").transform.position + new Vector3 (0, -0.65f, 0);
			rocheLobuleText.transform.rotation = Camera.main.transform.rotation;
		}
	}

	public void closeLightCurve () 
	{
		curvesButton.GetComponent<Toggle> ().isOn = false;
	}
		
}
