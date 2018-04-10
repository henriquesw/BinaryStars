using UnityEngine;
using System.Globalization;
using UnityEngine.UI;


public class LightCurveWindow : MonoBehaviour
{

    private FreePlotter lightCurvePlotter;
    private LightCurveGenerator lightCurveGenerator;

    private double[] brit;
    private double[] phasp;

    public static RawImage lighCurvesRawImage;

    private double biggestValue = 0;
    private double smallestValue = double.PositiveInfinity;

    private Button mailButton;
    private Button closeButton;

    // Use this for initialization
    void Start()
    {
        mailButton = GameObject.Find("LightCurveWindow/ActionBar/Email").GetComponent<Button>();
        mailButton.onClick.AddListener(() => emailWindow());

        closeButton = GameObject.Find("LightCurveWindow/ActionBar/Close").GetComponent<Button>();
        closeButton.onClick.AddListener(() => destroyComponnent());
    }

    // Update is called once per frame
    void Update()
    {
        click();
    }

    private void click()
    {
        //Debug.Log(Input.mousePosition + "\t" + Screen.width * 0.9f + "\t" + Screen.height * 0.9f);

        //if (Input.GetMouseButton(0) && GUIUtility.hotControl == 0 &&
        //        (Input.mousePosition.x > Screen.width * 0.125 && Input.mousePosition.x < Screen.width * 0.875 &&
        //     Input.mousePosition.y > Screen.height * 0.15 && Input.mousePosition.y < Screen.height * 0.90))
        //{
        //    destroyComponnent();
        //}

        if (Input.GetMouseButton(0) && GUIUtility.hotControl == 0 &&
            !RectTransformUtility.RectangleContainsScreenPoint(
                 GetComponent<RectTransform>(),
                 Input.mousePosition) &&
                 !RectTransformUtility.RectangleContainsScreenPoint(
                 GameObject.Find("ActionBar").GetComponent<RectTransform>(),
                 Input.mousePosition))
        {
            destroyComponnent();
        }
    }

    public void showLightCurveGraphic()
    {
		lighCurvesRawImage = GameObject.Find("Light Curve Raw Image").GetComponent<RawImage>();

        biggestValue = 0;
        smallestValue = double.PositiveInfinity;

        double[] b = lightCurveGenerator.getLC().getVectorBrit();
        brit = new double[b.Length * 2];
        double[] p = lightCurveGenerator.getLC().getVectorPhasp();
        phasp = new double[p.Length * 2];

        for (int i = 0; i < b.Length; i++)
        {
            brit[i] = b[i];
            brit[i + b.Length] = b[i];

            phasp[i] = p[i];
            phasp[i + p.Length] = p[i] + 1.0;

            if (b[i] > biggestValue)
            {
                biggestValue = b[i];
            }

            if (b[i] < smallestValue)
            {
                smallestValue = b[i];
            }
        }

        //Debug.Log(smallestValue + "\t\t" + biggestValue + "\t\t" + (biggestValue >= smallestValue));

        /* Normalizing values to fit in the plotter */

        if (smallestValue == biggestValue)
        {
            //Debug.Log(smallestValue / biggestValue + "\t\t" + biggestValue / biggestValue);
            /* Put 0.5 as min and 1.5 as max, because the graphic is a horizontal line (smallest == biggest)
             * So smallest / biggest == 1, always. In other words, always in the middles of 0.5 and 1.5.
             */
            lightCurvePlotter = new FreePlotter(0.5, 1.5);
        }
        else
        {
            if (smallestValue > 0)
            {
                //Debug.Log((smallestValue / (biggestValue * 1.01d)) + "\t\t" + (biggestValue / (biggestValue * 0.99d)));
                lightCurvePlotter = new FreePlotter((smallestValue / (biggestValue * 1.001d)), (biggestValue / (biggestValue * 0.999d)));
            }
            else
            {
                //Debug.Log((smallestValue / (biggestValue * 0.99d)) + "\t\t" + (biggestValue / (biggestValue * 0.99d)));
                lightCurvePlotter = new FreePlotter((smallestValue / (biggestValue * 0.999d)), (biggestValue / (biggestValue * 0.999d)));
            }
        }
        lightCurvePlotter.NewPlot("LightCurve", Color.blue);

        for (int i = 0; i < brit.Length; i++)
        {
            //lightCurvePlotter.AddPoint("LightCurve", i, (float)brit[i]);
            lightCurvePlotter.AddPoint("LightCurve", i, (float)(brit[i] / biggestValue));
        }

        lightCurvePlotter.updateGraphic();
    }

    public void setLighCurveGenerator(LightCurveGenerator lightCurveGenerator)
    {
        this.lightCurveGenerator = lightCurveGenerator;
        //isGraphicOn = false;
        showLightCurveGraphic();
        //isGraphicOn = true;
    }

    public void destroy()
    {
        //UserWindow.lightCurveWindow.SetActive(false);
        lightCurveGenerator = null;
        lightCurvePlotter = null;
        Destroy(this);
    }

    public void destroyComponnent()
    {
        //isGraphicOn = false;
        lightCurveGenerator = null;
        lightCurvePlotter = null;
        Destroy(this);
    }

    public void emailWindow()
    {
        gameObject.AddComponent<EmailWindow>();
        gameObject.GetComponent<EmailWindow>().setBrit(this.brit);
        gameObject.GetComponent<EmailWindow>().setPhase(this.phasp);
    }

}
