using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBehaviour : MonoBehaviour {

	public GameObject lightCurveWindow;
	public GameObject aboutWindow;
	public GameObject helpWindow;
	public GameObject emailWindow;
	public GameObject progressDialogWindow;
	public GameObject optionWindow;

	private bool optionWindowStats = false;
	private bool optionHelpStats = false;
	private bool optionAboutStats = false;
	private bool optionHideStats = false;

	// Use this for initialization
	void Start () {
		lightCurveWindow.SetActive (false);
		aboutWindow.SetActive (optionAboutStats);
		helpWindow.SetActive (optionHelpStats);
		emailWindow.SetActive (false);
		progressDialogWindow.SetActive (false);
		optionWindow.SetActive (optionWindowStats);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SetOptionWindow () {
		optionWindowStats = !optionWindowStats;
		optionWindow.SetActive (optionWindowStats);
	}

	public void SetHelpWindow () {
		optionHelpStats = !optionHelpStats;
		helpWindow.SetActive (optionHelpStats);
		SetOptionWindow ();
	}

	public void SetAboutWindow () {
		optionAboutStats = !optionAboutStats;
		aboutWindow.SetActive (optionAboutStats);
		SetOptionWindow ();
	}

	public void CloseWindows () {
		optionWindowStats = false;
		optionWindow.SetActive (optionWindowStats);

		optionHelpStats = false;
		helpWindow.SetActive (optionHelpStats);

		optionAboutStats = false;
		aboutWindow.SetActive (optionAboutStats);
	}
}
