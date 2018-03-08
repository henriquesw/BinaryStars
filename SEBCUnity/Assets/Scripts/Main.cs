using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour {

	public static float mass = 50f;
	public static float lastPrimaryTemp = 15f;
	public static float lastSecondaryTemp = 1f;

	private CloseBinarySimulator screen;
	private int countParticles;
	private float sliderPrimaryTemp = lastPrimaryTemp;
	private float sliderSecondaryTemp = lastSecondaryTemp;

	// Use this for initialization
	void Start () {
		screen = new CloseBinarySimulator();
		int indice = (int)(mass - 10) / 2;
		screen.setSystem(indice, countParticles, sliderPrimaryTemp, sliderSecondaryTemp);
	}
	
	// Update is called once per frame
	void Update () {
		if (screen.getOrbit())
		{
			GameObject.Find("White Dwarf").transform.RotateAround(Vector3.zero, Vector3.down, 40 * Time.deltaTime);
			GameObject.Find("Red Giant").transform.RotateAround(Vector3.zero, Vector3.down, 40 * Time.deltaTime);
		}

		if (screen.getParticles())
		{
			screen.drawParticles(countParticles);
			countParticles += 8;
		}
	}

	void OnGUI()
	{
		if (Input.GetMouseButtonUp(0)) // get mouse click up
		{
			onMouseClickUp();
		}
	}

	private void onMouseClickUp()
	{
		isHandlingClick = false;

		if (lastMass != mass || lastSecondaryTemp != sliderSecondaryTemp || lastPrimaryTemp != sliderPrimaryTemp) // verify changes in mass ratio or temperatures
		{
			lastMass = mass;
			lastSecondaryTemp = sliderSecondaryTemp;
			lastPrimaryTemp = sliderPrimaryTemp;

			if (GetComponent<LightCurveWindow>() != null)
			{
				StartCoroutine(showLightCurves());
			}

			updateScreen();
		}

		if (lastRotationY != rotationY) // verify changes in the angle of rotation
		{
			lastRotationY = rotationY;

			if (GetComponent<LightCurveWindow>() != null)
			{
				StartCoroutine(showLightCurves());
			}
		}
	}

	private IEnumerator showLightCurves()
	{
		gameObject.AddComponent<ProgressDialog>();

		yield return null;

		//screen.setLabels(false);

		float m = mass / 100;
		double t1p = (double)sliderPrimaryTemp * 1000;
		double t2p = (double)sliderSecondaryTemp * 1000;
		double angle = rotationY;

		LightCurveGenerator lightCurveGenerator;

		if (screen.getParticles())
		{
			lightCurveGenerator = new LightCurveGenerator(3, m, t1p, t2p, angle);
		}
		else
		{
			if (screen.getRocheLobule())
			{
				lightCurveGenerator = new LightCurveGenerator(1, m, t1p, t2p, angle);
			}
			else
			{
				lightCurveGenerator = new LightCurveGenerator(2, m, t1p, t2p, angle);
			}
		}
		//Debug.Log(lightCurveGenerator.KFLAG + "\t" + m + "\t" + t1p + "\t" + t2p + "\t" + angle);

		StartCoroutine(lightCurveGenerator.generateLightCurves());

		GameObject.Find("EventSystem").GetComponent<ProgressDialog>().progress = 4;
		yield return null;

		if (GetComponent<LightCurveWindow>() != null)
		{
			GetComponent<LightCurveWindow>().destroy();
			DestroyImmediate(GetComponent<LightCurveWindow>());

			GameObject.Find("EventSystem").GetComponent<ProgressDialog>().progress = 5;
			yield return null;
		}

		gameObject.AddComponent<LightCurveWindow>();

		GameObject.Find("EventSystem").GetComponent<ProgressDialog>().progress = 6;
		yield return null;

		GetComponent<LightCurveWindow>().setLighCurveGenerator(lightCurveGenerator);

		GameObject.Find("EventSystem").GetComponent<ProgressDialog>().progress = 0;

		DestroyImmediate(GetComponent<ProgressDialog>());
		progressDialogWindow.SetActive(false);
		yield return null;
	}

	private void updateScreen()
	{
		int indice = (int)(mass - 10) / 2;
		screen.setSystem(indice, countParticles, sliderPrimaryTemp, sliderSecondaryTemp);
	}

	public void openUrl(string url)
	{
		Application.OpenURL("http://www.bcc.unifal-mg.edu.br/lte/");
	}
}
