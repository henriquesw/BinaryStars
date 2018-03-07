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

    private GUIStyle styleLabel = new GUIStyle();

    private double biggestValue = 0;
    private double smallestValue = double.PositiveInfinity;

    private bool isGraphicOn = false;

    private Button mailButton;
    private Button closeButton;

    // Use this for initialization
    void Start()
    {
        UserWindow.lightCurveWindow.SetActive(true);

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
                 UserWindow.lightCurveWindow.GetComponent<RectTransform>(),
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
        lighCurvesRawImage = GameObject.Find("EventSystem").GetComponent<UserWindow>().lightCurvesRawImage;

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

    //void OnGUI()
    //{
    //    if (isGraphicOn)
    //    {
    //        GUI.Window(0, new Rect(Screen.width * 0.18f, Screen.height * 0.105f, Screen.width * 0.635f, Screen.height * 0.78f), doMyWindow, "Light Curves");
    //    }
    //}

    //private void doMyWindow(int id)
    //{
    //    GUI.DrawTexture(new Rect(-10, -5, Screen.width * 0.73f, Screen.height * 0.83f), GameObject.Find("EventSystem").GetComponent<UserWindow>().windowTexture, ScaleMode.StretchToFill);

    //    styleLabel.fontSize = (int)(Screen.width * 0.024f);
    //    styleLabel.normal.textColor = Color.black;
    //    styleLabel.fontStyle = FontStyle.Bold;
    //    GUI.Label(new Rect(Screen.width * 0.002f, Screen.height * 0.002f, Screen.width * 0.3f, Screen.height * 0.1f), "Light Curve", styleLabel);
    //    styleLabel.normal.textColor = Color.white;
    //    GUI.Label(new Rect(Screen.width * 0.0015f, Screen.height * 0.001f, Screen.width * 0.3f, Screen.height * 0.1f), "Light Curve", styleLabel);

    //    if (GUI.Button(new Rect(Screen.width * 0.15f, Screen.height * 0.004f, Screen.height * 0.048f, Screen.height * 0.048f), "",
    //        GameObject.Find("EventSystem").GetComponent<UserWindow>().styleButtonEmail))
    //    {
    //        if (GetComponent<EmailWindow>() == null)
    //        {
    //            gameObject.AddComponent<EmailWindow>();
    //            isGraphicOn = false;
    //        }
    //    }

    //    if (GUI.Button(new Rect(Screen.width * 0.605f, Screen.height * 0.006f, Screen.height * 0.042f, Screen.height * 0.042f), "", 
    //        GameObject.Find("EventSystem").GetComponent<UserWindow>().styleButtonClose))
    //    {
    //        destroyComponnent();
    //    }

    //    //GUI.DrawTexture(new Rect(Screen.width * 0.055f, Screen.height * 0.1f, Screen.width * 0.56f, Screen.height * 0.62f), lighCurvesRawImage.texture);

    //    styleLabel.normal.textColor = Color.black;
    //    styleLabel.fontStyle = FontStyle.Normal;

    //    float minWidth = Screen.width * 0.02f;
    //    float maxWidth = 0f;
    //    float minHeight = Screen.height * 0.1f;
    //    float maxHeight = Screen.height * 0.7f;

    //    styleLabel.fontSize = (int)(Screen.width * 0.012f);
    //    GUI.Label(new Rect(minWidth * 0.75f, minHeight * 0.58f, 100, 50), "NORMALIZED\n    BRIGHT", styleLabel);

    //    /* USE THIS FOR REAL VALUES */
    //    //styleLabel.fontSize = (int)(Screen.width * 0.01f);
    //    //GUI.Label(new Rect(minWidth * 70f, minHeight, 30, 1000), "^\n|\n|\n|\n|\n|\n|\n|\n|\n|\n|\n|\n|\n|\n|\n|\n|\n|\n|\n|\n|\n|\n|\n|\n|\n|\n|\n|\n|\n|\n|\n|\n|\n|\n|\n|\n|\n|\n|\n|\n|\n|\n|\n|\n|\n|\n|\n|\n|\n|\n|\n|\n|\n|\n|\n|\n|\n|\n|\n|\n|", styleLabel);
    //    //GUI.Label(new Rect(minWidth, minHeight, 100, 70), biggestValue.ToString("F", CultureInfo.InvariantCulture), styleLabel);
    //    //GUI.Label(new Rect(minWidth, (minHeight + (minHeight + maxHeight) / 2) / 2, 100, 35), ((((biggestValue + smallestValue) / 2) + biggestValue) / 2).ToString("F", CultureInfo.InvariantCulture), styleLabel);
    //    //GUI.Label(new Rect(minWidth, (minHeight + maxHeight) / 2, 100, 35), ((biggestValue + smallestValue) / 2).ToString("F", CultureInfo.InvariantCulture), styleLabel);
    //    //GUI.Label(new Rect(minWidth, (maxHeight + (minHeight + maxHeight) / 2) / 2, 100, 35), ((((biggestValue + smallestValue) / 2) + smallestValue) / 2).ToString("F", CultureInfo.InvariantCulture), styleLabel);
    //    //GUI.Label(new Rect(minWidth, maxHeight, 100, 35), smallestValue.ToString("F", CultureInfo.InvariantCulture), styleLabel);

    //    /* USE THIS FOR NORMALIZED VALUES*/
    //    styleLabel.fontSize = (int)(Screen.width * 0.01f);
    //    GUI.Label(new Rect(minWidth * 2.5f, minHeight, 30, 1000), "^\n|\n|\n|\n|\n|\n|\n|\n|\n|\n|\n|\n|\n|\n|\n|\n|\n|\n|\n|\n|\n|\n|\n|\n|\n|\n|\n|\n|\n|\n|\n|\n|\n|\n|\n|\n|\n|\n|\n|\n|\n|\n|\n|\n|\n|\n|\n|\n|\n|\n|\n|\n|\n|\n|\n|\n|\n|\n|\n|\n|", styleLabel);
    //    GUI.Label(new Rect(minWidth, minHeight, 100, 70), "2.00", styleLabel);
    //    GUI.Label(new Rect(minWidth, (minHeight + (minHeight + maxHeight) / 2) / 2, 100, 35), "1.75", styleLabel);
    //    GUI.Label(new Rect(minWidth, (minHeight + maxHeight) / 2, 100, 35), "1.50", styleLabel);
    //    GUI.Label(new Rect(minWidth, (maxHeight + (minHeight + maxHeight) / 2) / 2, 100, 35), "1.25", styleLabel);
    //    GUI.Label(new Rect(minWidth, maxHeight, 100, 35), "1.00", styleLabel);

    //    minWidth = Screen.width * 0.055f;
    //    maxWidth = Screen.width * 0.6f;
    //    maxHeight = Screen.height * 0.73f;

    //    styleLabel.fontSize = (int)(Screen.width * 0.012f);
    //    GUI.Label(new Rect(maxWidth * 0.95f, maxHeight * 0.95f, 100, 35), "PHASE", styleLabel);

    //    styleLabel.fontSize = (int)(Screen.width * 0.01f);
    //    GUI.Label(new Rect(0, maxHeight * 0.98f, 1000, 35), "------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------->", styleLabel);
    //    //GUI.Label(new Rect(minWidth, maxHeight, 100, 35), lightCurveGenerator.lc.getVectorPhasp()[0].ToString("F", CultureInfo.InvariantCulture)+ "\u03A0", styleLabel);
    //    //GUI.Label(new Rect((minWidth + (minWidth + maxWidth) / 2) / 2, maxHeight, 100, 35), lightCurveGenerator.lc.getVectorPhasp()[lightCurveGenerator.lc.getVectorPhasp().Length / 2].ToString("F", CultureInfo.InvariantCulture), styleLabel);
    //    //GUI.Label(new Rect((minWidth + maxWidth) / 2, maxHeight, 100, 35), (lightCurveGenerator.lc.getVectorPhasp()[lightCurveGenerator.lc.getVectorPhasp().Length - 1] + 0.01).ToString("F", CultureInfo.InvariantCulture), styleLabel);
    //    //GUI.Label(new Rect((maxWidth + (minWidth + maxWidth) / 2) / 2, maxHeight, 100, 35), (lightCurveGenerator.lc.getVectorPhasp()[lightCurveGenerator.lc.getVectorPhasp().Length / 2] + 1).ToString("F", CultureInfo.InvariantCulture), styleLabel);
    //    //GUI.Label(new Rect(maxWidth, maxHeight, 100, 35), (lightCurveGenerator.lc.getVectorPhasp()[lightCurveGenerator.lc.getVectorPhasp().Length - 1] + 1).ToString("F", CultureInfo.InvariantCulture), styleLabel);

    //    GUI.Label(new Rect(minWidth, maxHeight, 100, 35), "0°", styleLabel);
    //    GUI.Label(new Rect((minWidth + (minWidth + maxWidth) / 2) / 2, maxHeight, 100, 35), "180°", styleLabel);
    //    GUI.Label(new Rect((minWidth + maxWidth) / 2, maxHeight, 100, 35), "360°", styleLabel);
    //    GUI.Label(new Rect((maxWidth + (minWidth + maxWidth) / 2) / 2, maxHeight, 100, 35), "420°", styleLabel);
    //    GUI.Label(new Rect(maxWidth, maxHeight, 100, 35), "720°", styleLabel);

    //    GUI.DragWindow();
    //}

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
        UserWindow.lightCurveWindow.SetActive(false);
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
