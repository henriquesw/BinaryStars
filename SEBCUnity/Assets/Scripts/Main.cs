using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Main : MonoBehaviour {

	public float mass;
	public GameObject canvas;
    public GameObject worldCanvas;

	private CloseBinarySimulator screen;
	private int countParticles;
	private float sliderPrimaryTemp = 10f;
	private float sliderSecondaryTemp = 3f;

	// Use this for initialization
	void Start () 
	{
		screen = new CloseBinarySimulator();
		int indice = (int)(mass - 10) / 2;
		screen.setSystem(indice, countParticles, sliderPrimaryTemp, sliderSecondaryTemp);
	}
	
	// Update is called once per frame
	void Update () 
	{

		if (screen.getParticles())
		{
			screen.drawParticles(countParticles);
			countParticles += 8;
		}

		if (screen.getLabels ())
		{
			canvas.GetComponent<UIBehaviour> ().setLabel (screen.getRocheLobule());
		}

        if (screen.getOrbit())
        {
            rotateObjects();
        }

    }

	private void updateScreen()
	{
		int indice = (int)(mass - 10) / 2;
		screen.setSystem(indice, countParticles, sliderPrimaryTemp, sliderSecondaryTemp);
	}

    private void rotateObjects ()
    {
        float time = Time.deltaTime;
        GameObject.Find("White Dwarf").transform.RotateAround(Vector3.zero, Vector3.down, 40 * time);
        GameObject.Find("Red Giant").transform.RotateAround(Vector3.zero, Vector3.down, 40 * time);
        worldCanvas.transform.RotateAround(Vector3.zero, Vector3.down, 40 * time);
    }

	private IEnumerator showLightCurves()
	{
		float m = mass / 100;
		double t1p = (double)sliderPrimaryTemp * 1000;
		double t2p = (double)sliderSecondaryTemp * 1000;
		double angle = GameObject.Find ("Slider (3)").GetComponent<Slider> ().value;

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

		StartCoroutine(lightCurveGenerator.generateLightCurves());
		yield return null;

		if (canvas.GetComponent<LightCurveWindow>() != null)
		{
			canvas.GetComponent<LightCurveWindow>().destroy();
			DestroyImmediate(canvas.GetComponent<LightCurveWindow>());
			yield return null;
		}

		canvas.AddComponent<LightCurveWindow>();
		yield return null;

		canvas.GetComponent<LightCurveWindow>().setLighCurveGenerator(lightCurveGenerator);
		yield return null;
	}

	public void setMass (Slider slider)
	{
		this.mass = slider.value;
		updateScreen ();
	}

	public void setPrimaryTemperature (Slider slider)
	{
		this.sliderPrimaryTemp = slider.value;
		updateScreen ();
	}

	public void setSecondaryTemperature (Slider slider)
	{
		this.sliderSecondaryTemp = slider.value;
		updateScreen ();
	}

	public void setRocheLobule (bool value)
	{
		screen.setRocheLobule (value);
	}

	public void setOrbit (bool value)
	{
		screen.setOrbit (value);
	}

	public void setAxes (bool value)
	{
		screen.setAxes (value);
	}

	public void setLabels (bool value)
	{
		screen.setLabels (value);
		canvas.GetComponent<UIBehaviour> ().setWorldCanvas (value);
	}

	public void setParticles (bool value)
	{
		screen.setParticles (value);
	}

	public void setLightCurves (bool value)
	{
		if (value) 
		{
			StartCoroutine (showLightCurves ());
		}
	}
}
