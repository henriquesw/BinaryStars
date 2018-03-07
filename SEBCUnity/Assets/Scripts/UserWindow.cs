using UnityEngine;
using System.Globalization;
using UnityEngine.UI;
using System.Collections;
//using UnityEditor;
//using System.Net;
//using System.Net.Mail;
//using System.Net.Security;
//using System.Security.Cryptography.X509Certificates;

public class UserWindow : MonoBehaviour
{
    //public Texture progressBarTexture0;
    //public Texture progressBarTexture1;
    //public Texture progressBarTexture2;
    //public Texture progressBarTexture3;
    //public Texture progressBarTexture4;
    //public Texture progressBarTexture5;
    //public Texture progressBarTexture6;
    //public Texture progressBarTexture7;

    public Texture windowTexture;
    public Texture optionsWindow;
    public RawImage lightCurvesRawImage;

    public Shader shader;

    private CloseBinarySimulator screen;

    public static GameObject lightCurveWindow;
    public static GameObject aboutWindow;
    public static GameObject helpWindow;
    public static GameObject emailWindow;
    public static GameObject progressDialogWindow;

    // Use this for initialization
    void Start()
    {
        //gameObject.AddComponent<ProgressDialog>();
        screen = new CloseBinarySimulator();

        int indice = (int)(mass - 10) / 2;
        //screen.setSystem (new BinarySystemData (3.57027f, 0.10f, 0.71751f, 1.22831f, -0.61414f));
        screen.setSystem(indice,
            countParticles,
            sliderPrimaryTemp,
            sliderSecondaryTemp);

        gameObject.AddComponent<RotateObject>();

        lightCurveWindow = GameObject.Find("LightCurveWindow");
        lightCurveWindow.SetActive(false);
        aboutWindow = GameObject.Find("AboutWindow");
        aboutWindow.SetActive(false);
        helpWindow = GameObject.Find("HelpWindow");
        helpWindow.SetActive(false);
        emailWindow = GameObject.Find("EmailWindow");
        emailWindow.SetActive(false);
        progressDialogWindow = GameObject.Find("ProgressDialogWindow");
        progressDialogWindow.SetActive(false);

        styleLabel.fontSize = (int)(Screen.width * 0.02f);
        styleLabel.normal.textColor = Color.white;

        styleSpeechBubbleHorizontal.fontSize = (int)(Screen.width * 0.015f);
        styleSpeechBubbleVertical.fontSize = (int)(Screen.width * 0.015f);

        buttonSize = new Vector2(Screen.width * 0.095f, Screen.height * 0.085f);
        buttonPosition = new Vector2(Screen.width * 0.0248f, Screen.height * 0.15f);

        sliderVerticalSize = new Vector2(37, Screen.height * 0.25f);
    }

    // Update is called once per frame
    void Update()
    {
        if (screen.getOrbit())
        {
            GameObject.Find("White Dwarf").transform.RotateAround(Vector3.zero, Vector3.down, 40 * Time.deltaTime);
            GameObject.Find("Red Giant").transform.RotateAround(Vector3.zero, Vector3.down, 40 * Time.deltaTime);

            //if (!screen.getClockWise())
            //{
            //    GameObject.Find("White Dwarf").transform.RotateAround(Vector3.zero, Vector3.down, 40 * Time.deltaTime);
            //    GameObject.Find("Red Giant").transform.RotateAround(Vector3.zero, Vector3.down, 40 * Time.deltaTime);
            //}
            //else
            //{
            //    GameObject.Find("White Dwarf").transform.RotateAround(Vector3.zero, Vector3.up, 40 * Time.deltaTime);
            //    GameObject.Find("Red Giant").transform.RotateAround(Vector3.zero, Vector3.up, 40 * Time.deltaTime);
            //}
        }

        if ((sliderMass % 2) == 0)
        {
            mass = sliderMass;
        }
        else
        {
            mass = sliderMass + 1;
        }

        if (screen.getParticles())
        {
            screen.drawParticles(countParticles);
            countParticles += 8;
        }
    }

    private int countParticles;
    public static float mass = 50f;
    private float sliderMass = mass;
    private float lastMass = mass;
    public static float lastPrimaryTemp = 15f;
    private float sliderPrimaryTemp = lastPrimaryTemp;
    public static float lastSecondaryTemp = 1f;
    private float sliderSecondaryTemp = lastSecondaryTemp;
    public static float sliderRotationY = 0f;
    public static int rotationY = 0;
    private int lastRotationY = rotationY;

    private bool dropdown = false;
    private bool isHandlingClick = false;
    private bool hideUI = false;

    public GUIStyle backgroundHorizontalSlider;
    public GUIStyle backgroundVerticalSliderDwarf;
    public GUIStyle backgroundVerticalSliderGiant;
    public GUIStyle backgroundVerticalSliderRatio;
    public GUIStyle handleAreaWhiteDwarf;
    public GUIStyle handleAreaRedGiant;
    public GUIStyle handleAreaMassRatio;
    public GUIStyle handleAreaRotationY;

    public Texture textureButtonOrbit;
    public Texture textureButtonOrbitClicked;
    public Texture textureButtonRocheLobule;
    public Texture textureButtonRocheLobuleClicked;
    public Texture textureButtonAxes;
    public Texture textureButtonAxesClicked;
    public Texture textureButtonLabels;
    public Texture textureButtonLabelsClicked;
    public Texture textureButtonParticles;
    public Texture textureButtonParticlesClicked;

    public GUIStyle styleButtonLightCurves;
    public GUIStyle styleButtonClose;
    public GUIStyle styleButtonEmail;
    public GUIStyle styleSpeechBubbleHorizontal;
    public GUIStyle styleSpeechBubbleVertical;
    private GUIStyle styleLabel = new GUIStyle();
    private GUIStyle styleButtonAction = new GUIStyle();

    private Vector2 sliderVerticalSize;
    private Vector2 buttonSize;
    private Vector2 buttonPosition;


    void OnGUI()
    {
        optionsButton();

        if (!hideUI)
        {
            renderButtons();

            renderSliders();

            renderSpeechBubbles();
        }

        renderLabels();

        //renderTmperatureBarLabels();

        if (Input.GetMouseButtonUp(0)) // get mouse click up
        {
            onMouseClickUp();
        }

        if (Input.GetMouseButtonDown(0)) // get mouse click down
        {
            onMouseClickDown();
        }
    }

    private void renderButtons()
    {
        int n = 0;
        styleButtonAction.stretchHeight = true;

        /********************************************************************************************************************************************************/
        if (screen.getOrbit())
        {
            if (GUI.Button(new Rect(buttonPosition.x, buttonPosition.y + buttonSize.y * n++, buttonSize.x, buttonSize.y), textureButtonOrbit, styleButtonAction))
            {
                //MailMessage mail = new MailMessage();

                //mail.From = new MailAddress("tablet.lte@bcc.unifal-mg.edu.br");
                //mail.To.Add("hugo_camargo10@hotmail.com");
                //mail.Subject = "Test Mail";
                //mail.Body = "This is for testing SMTP mail from GMAIL";

                //SmtpClient smtpServer = new SmtpClient("smtp.gmail.com");
                //smtpServer.Port = 587;
                //smtpServer.Credentials = new System.Net.NetworkCredential("tablet.lte@bcc.unifal-mg.edu.br", "-Asdqma?0") as ICredentialsByHost;
                //smtpServer.EnableSsl = true;
                //ServicePointManager.ServerCertificateValidationCallback =
                //delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
                //{ return true; };
                //smtpServer.Send(mail);
                //Debug.Log("success");

                screen.setOrbit(!screen.getOrbit());
            }
        }
        else
        {
            if (GUI.Button(new Rect(buttonPosition.x, buttonPosition.y + buttonSize.y * n++, buttonSize.x, buttonSize.y), textureButtonOrbitClicked, styleButtonAction))
            {
                screen.setOrbit(!screen.getOrbit());
            }
        }
        /********************************************************************************************************************************************************/
        //bool clockWiseButton = GUI.Button(new Rect(buttonPosition.x, buttonPosition.y  + buttonSize.y * n++, buttonSize.x, buttonSize.y), "", styleButtonClockWise);
        //if (clockWiseButton)
        //{
        //    screen.setClockWise(!screen.getClockWise());
        //}
        /********************************************************************************************************************************************************/
        n++;
        if (screen.getRocheLobule())
        {
            if (GUI.Button(new Rect(buttonPosition.x, buttonPosition.y + buttonSize.y * n++, buttonSize.x, buttonSize.y), textureButtonRocheLobule, styleButtonAction))
            {
                screen.setRocheLobule(!screen.getRocheLobule());
            }
        }
        else
        {
            if (GUI.Button(new Rect(buttonPosition.x, buttonPosition.y + buttonSize.y * n++, buttonSize.x, buttonSize.y), textureButtonRocheLobuleClicked, styleButtonAction))
            {
                screen.setRocheLobule(!screen.getRocheLobule());
            }
        }
        /********************************************************************************************************************************************************/
        if (screen.getAxes())
        {
            if (GUI.Button(new Rect(buttonPosition.x, buttonPosition.y + buttonSize.y * n++, buttonSize.x, buttonSize.y), textureButtonAxes, styleButtonAction))
            {
                screen.setAxes(!screen.getAxes());
            }
        }
        else
        {
            if (GUI.Button(new Rect(buttonPosition.x, buttonPosition.y + buttonSize.y * n++, buttonSize.x, buttonSize.y), textureButtonAxesClicked, styleButtonAction))
            {
                screen.setAxes(!screen.getAxes());
            }
        }
        /********************************************************************************************************************************************************/
        if (screen.getLabels())
        {
            if (GUI.Button(new Rect(buttonPosition.x, buttonPosition.y + buttonSize.y * n++, buttonSize.x, buttonSize.y), textureButtonLabels, styleButtonAction))
            {
                screen.setLabels(!screen.getLabels());
            }
        }
        else
        {
            if (GUI.Button(new Rect(buttonPosition.x, buttonPosition.y + buttonSize.y * n++, buttonSize.x, buttonSize.y), textureButtonLabelsClicked, styleButtonAction))
            {
                screen.setLabels(!screen.getLabels());
            }
        }
        /********************************************************************************************************************************************************/
        if (!screen.getParticles())
        {
            if (GUI.Button(new Rect(buttonPosition.x, buttonPosition.y + buttonSize.y * n++, buttonSize.x, buttonSize.y), textureButtonParticlesClicked, styleButtonAction))
            {
                screen.setParticles(true);
                countParticles = 0;
            }
        }
        else
        {
            if (GUI.Button(new Rect(buttonPosition.x, buttonPosition.y + buttonSize.y * n++, buttonSize.x, buttonSize.y), textureButtonParticles, styleButtonAction))
            {
                screen.setParticles(false);
            }
        }
        /********************************************************************************************************************************************************/
        n++;
        if (GUI.Button(new Rect(buttonPosition.x, buttonPosition.y + buttonSize.y * n++, buttonSize.y * 2, buttonSize.y), "", styleButtonLightCurves))
        {
            StartCoroutine(showLightCurves());
        }
    }

    private void doOptionsDropdown(int id)
    {
        GUI.DrawTexture(new Rect(0, 0, Screen.width * 0.14f, Screen.height * 0.26f), optionsWindow, ScaleMode.StretchToFill);

        styleLabel.fontSize = (int)(Screen.width * 0.02f);
        styleLabel.normal.textColor = Color.black;

        if (GUI.Button(new Rect(Screen.width * 0.005f, Screen.height * 0.01f, Screen.width * 0.13f, Screen.height * 0.075f), ""))
        {
            if (GetComponent<AboutWindow>() != null)
            {
                GetComponent<AboutWindow>().destroy();
            }
            if (GetComponent<HelpWindow>() == null)
            {
                gameObject.AddComponent<HelpWindow>();
            }
            else
            {
                GetComponent<HelpWindow>().destroy();
            }

            dropdown = false;
        }
        GUI.Label(new Rect(Screen.width * 0.052f, Screen.height * 0.057f / 2, 100, 100), "Help", styleLabel);
        styleLabel.normal.textColor = Color.white;
        GUI.Label(new Rect(Screen.width * 0.05f, Screen.height * 0.055f / 2, 100, 100), "Help", styleLabel);
        styleLabel.normal.textColor = Color.black;

        /**********************************************************************************************************************************************/
        if (GUI.Button(new Rect(Screen.width * 0.005f, Screen.height * 0.01f + Screen.height * 0.08f, Screen.width * 0.13f, Screen.height * 0.075f), ""))
        {
            if (GetComponent<HelpWindow>() != null)
            {
                GetComponent<HelpWindow>().destroy();
            }
            if (GetComponent<AboutWindow>() == null)
            {
                gameObject.AddComponent<AboutWindow>();
            }
            else
            {
                GetComponent<AboutWindow>().destroy();
            }

            dropdown = false;
        }
        GUI.Label(new Rect(Screen.width * 0.047f, Screen.height * 0.01f + Screen.height * 0.08f + Screen.height * 0.037f / 2, 100, 100), "About", styleLabel);
        styleLabel.normal.textColor = Color.white;
        GUI.Label(new Rect(Screen.width * 0.045f, Screen.height * 0.01f + Screen.height * 0.08f + Screen.height * 0.035f / 2, 100, 100), "About", styleLabel);
        styleLabel.normal.textColor = Color.black;

        /**********************************************************************************************************************************************/
        if (GUI.Button(new Rect(Screen.width * 0.005f, Screen.height * 0.01f + Screen.height * 0.08f * 2, Screen.width * 0.13f, Screen.height * 0.075f), ""))
        {
            //GameObject.Find("Gradient Color Bar").GetComponent<Image>().enabled = !GameObject.Find("Gradient Color Bar").GetComponent<Image>().enabled;

            hideUI = !hideUI;
            dropdown = false;
        }
        if (hideUI)
        {
            GUI.Label(new Rect(Screen.width * 0.04f, Screen.height * 0.01f + Screen.height * 0.08f * 2 + Screen.height * 0.037f / 2, 100, 100), "Show UI", styleLabel);
            styleLabel.normal.textColor = Color.white;
            GUI.Label(new Rect(Screen.width * 0.038f, Screen.height * 0.01f + Screen.height * 0.08f * 2 + Screen.height * 0.035f / 2, 100, 100), "Show UI", styleLabel);
        }
        else
        {
            GUI.Label(new Rect(Screen.width * 0.04f, Screen.height * 0.01f + Screen.height * 0.08f * 2 + Screen.height * 0.037f / 2, 100, 100), "Hide UI", styleLabel);
            styleLabel.normal.textColor = Color.white;
            GUI.Label(new Rect(Screen.width * 0.038f, Screen.height * 0.01f + Screen.height * 0.08f * 2 + Screen.height * 0.035f / 2, 100, 100), "Hide UI", styleLabel);
        }

        /**********************************************************************************************************************************************/
        GUI.DragWindow();
    }

    public void optionsButton()
    {
        if (dropdown)
        {
            Rect rect = new Rect(Screen.width * 0.86f, Screen.height * 0.085f, Screen.width * 0.14f, Screen.height * 0.26f);
            GUI.Window(3, rect, doOptionsDropdown, "optionsDropdown");

            if (Input.GetMouseButton(0) &&
                GUIUtility.hotControl == 0 &&
                !rect.Contains(new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y)))
            {
                dropdown = false;
            }
        }
    }

    private void renderSliders()
    {
        GUI.depth = 10;

        GUI.Label(new Rect(Screen.width * 0.87f, Screen.height * 0.11f, 120, 35), "Temperatures", styleLabel);

        styleLabel.fontSize = (int)(Screen.width * 0.013f);
        GUI.Label(new Rect(Screen.width * 0.88f, Screen.height * 0.16f, 120, 35), "Primary", styleLabel);
        GUI.Label(new Rect(Screen.width * 0.925f, Screen.height * 0.16f, 120, 35), "Secondary", styleLabel);

        styleLabel.fontSize = (int)(Screen.width * 0.02f);

        sliderPrimaryTemp = (int)GUI.VerticalSlider(new Rect(Screen.width * 0.89f, Screen.height * 0.20f, sliderVerticalSize.x, sliderVerticalSize.y), sliderPrimaryTemp, 15, 5, backgroundVerticalSliderDwarf, handleAreaWhiteDwarf);
        GUI.Label(new Rect(Screen.width * 0.87f, Screen.height * 0.20f + sliderVerticalSize.y, 40, 35), (sliderPrimaryTemp * 1000).ToString(), styleLabel);

        sliderSecondaryTemp = (int)GUI.VerticalSlider(new Rect(Screen.width * 0.94f, Screen.height * 0.20f, sliderVerticalSize.x, sliderVerticalSize.y), sliderSecondaryTemp, 5, 1, backgroundVerticalSliderGiant, handleAreaRedGiant);
        GUI.Label(new Rect(Screen.width * 0.93f, Screen.height * 0.20f + sliderVerticalSize.y, 40, 35), (sliderSecondaryTemp * 1000).ToString(), styleLabel);

        GUI.Label(new Rect(Screen.width * 0.88f, Screen.height * 0.54f, 120, 35), "Mass Ratio", styleLabel);
        sliderMass = (int)GUI.VerticalSlider(new Rect(Screen.width * 0.915f, Screen.height * 0.6f, sliderVerticalSize.x, sliderVerticalSize.y), sliderMass, 10f, 100f, backgroundVerticalSliderRatio, handleAreaMassRatio);
        GUI.Label(new Rect(Screen.width * 0.91f, Screen.height * 0.6f + sliderVerticalSize.y, 25, 23), (mass / 100).ToString("F", CultureInfo.InvariantCulture), styleLabel);

        sliderRotationY = (int)GUI.HorizontalSlider(new Rect((Screen.width * 0.1f), (Screen.height * 0.93f), (Screen.width * 0.8f), sliderVerticalSize.x), sliderRotationY, 0, 90, backgroundHorizontalSlider, handleAreaRotationY);
        //rotationY = (int)(sliderRotationY - 90) * (-1);
        rotationY = (int)(sliderRotationY);
        GUI.Label(new Rect((Screen.width * 0.91f), (Screen.height * 0.9265f), 25, 35), "90°", styleLabel);
        GUI.Label(new Rect((Screen.width * 0.07f), (Screen.height * 0.9265f), 25, 35), "0°", styleLabel);
        GUI.Label(new Rect((Screen.width * 0.38f), (Screen.height * 0.895f), 250, 50), "Relative's Observer Angle: " + rotationY.ToString() + "°", styleLabel);
        //GUI.Label(new Rect((Screen.width * 0.5f), (Screen.height * 0.95f - 25), 25, 35), rotationY.ToString() + "°");

    }

    private void renderSpeechBubbles()
    {

        if (isHandlingClick)
        {
            //white dwarf
            Rect rect = new Rect(Screen.width * 0.89f, Screen.height * 0.20f, sliderVerticalSize.x, sliderVerticalSize.y);
            if (rect.Contains(new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y)))
            {
                GUI.Label(new Rect(Screen.width * 0.83f, Screen.height * 0.38f - (sliderVerticalSize.y * 0.7f / 10 * (sliderPrimaryTemp - 5)), Screen.width * 0.06f, Screen.height * 0.05f), (sliderPrimaryTemp * 1000).ToString(), styleSpeechBubbleVertical);
            }

            //reg giant
            rect = new Rect(Screen.width * 0.94f, Screen.height * 0.20f, sliderVerticalSize.x, sliderVerticalSize.y);
            if (rect.Contains(new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y)))
            {
                GUI.Label(new Rect(Screen.width * 0.89f, Screen.height * 0.38f - (sliderVerticalSize.y * 0.9f / 5 * (sliderSecondaryTemp - 1)), Screen.width * 0.05f, Screen.height * 0.05f), (sliderSecondaryTemp * 1000).ToString(), styleSpeechBubbleVertical);
            }

            //mass ratio
            rect = new Rect(Screen.width * 0.915f, Screen.height * 0.6f, sliderVerticalSize.x, sliderVerticalSize.y);
            if (rect.Contains(new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y)))
            {
                GUI.Label(new Rect(Screen.width * 0.865f, Screen.height * 0.6f + (sliderVerticalSize.y * 0.7f / 80 * (sliderMass - 10)), Screen.width * 0.045f, Screen.height * 0.05f), (mass / 100).ToString("F", CultureInfo.InvariantCulture), styleSpeechBubbleVertical);
            }

            //rotation angle
            rect = new Rect((Screen.width * 0.1f), (Screen.height * 0.93f), (Screen.width * 0.8f), sliderVerticalSize.x);
            if (rect.Contains(new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y)))
            {
                //GUI.Label(new Rect(Screen.width * 0.1f - (Screen.width * 0.78f / 90 * (rotationY - 90)), Screen.height * 0.86f, Screen.width * 0.03f, Screen.height * 0.07f), (rotationY).ToString() + "°", styleSpeechBubbleHorizontal);
                GUI.Label(new Rect(Screen.width * 0.11f + (Screen.width * 0.77f / 90 * (rotationY)), Screen.height * 0.86f, Screen.width * 0.03f, Screen.height * 0.07f), (rotationY).ToString() + "°", styleSpeechBubbleHorizontal);
            }
        }
    }

    private void renderLabels()
    {
        GUI.depth = 10;

        styleLabel.fontSize = (int)(Screen.width * 0.013f);
        styleLabel.fontStyle = FontStyle.Bold;

        if (screen.getAxes())
        {
            GUI.color = new Color(0, 1, 0, 1);
            Vector3 objPosition = Camera.main.WorldToScreenPoint(GameObject.Find("ConeGreen").transform.position);
            GUI.Label(new Rect(objPosition.x - Screen.width * 0.015f, Screen.height - objPosition.y - Screen.height * 0.015f, 20, 20), "y", styleLabel);

            GUI.color = new Color(1, 0.3f, 0, 1);
            objPosition = Camera.main.WorldToScreenPoint(GameObject.Find("ConeRed").transform.position);
            GUI.Label(new Rect(objPosition.x + Screen.width * 0.002f, Screen.height - objPosition.y - Screen.height * 0.035f, 30, 30), "x", styleLabel);

            GUI.color = new Color(0, 1, 1, 1);
            objPosition = Camera.main.WorldToScreenPoint(GameObject.Find("ConeBlue").transform.position);
            GUI.Label(new Rect(objPosition.x + Screen.width * 0.002f, Screen.height - objPosition.y - Screen.height * 0.035f, 30, 30), "z", styleLabel);
        }

        if (screen.getLabels())
        {
            GUI.color = Color.white;

            Vector3 objPosition = Camera.main.WorldToScreenPoint(GameObject.Find("Red Giant").transform.position);
            GUI.Label(new Rect(objPosition.x - Screen.width * 0.025f, Screen.height - objPosition.y, 75, 30), "Secondary", styleLabel);

            objPosition = Camera.main.WorldToScreenPoint(GameObject.Find("White Dwarf").transform.position);
            GUI.Label(new Rect(objPosition.x - Screen.width * 0.02f, objPosition.y + Screen.height * 0.015f, 75, 30), "Primary", styleLabel);

            objPosition = Camera.main.WorldToScreenPoint(GameObject.Find("L1").transform.position);
            GUI.Label(new Rect(objPosition.x - Screen.width * 0.005f, Screen.height - objPosition.y, 50, 30), "L1", styleLabel);

            objPosition = Camera.main.WorldToScreenPoint(GameObject.Find("L2").transform.position);
            GUI.Label(new Rect(objPosition.x - Screen.width * 0.001f, Screen.height - objPosition.y, 50, 30), "L2", styleLabel);

            objPosition = Camera.main.WorldToScreenPoint(GameObject.Find("L3").transform.position);
            GUI.Label(new Rect(objPosition.x - Screen.width * 0.01f, Screen.height - objPosition.y, 50, 30), "L3", styleLabel);

            if (screen.getRocheLobule())
            {
                objPosition = Camera.main.WorldToScreenPoint(GameObject.Find("White Dwarf").transform.position);
                GUI.Label(new Rect(objPosition.x - Screen.width * 0.03f, objPosition.y + screen.data.getL1() * 210, 100, 30), "Roche Lobule", styleLabel);
            }
        }

        styleLabel.fontSize = (int)(Screen.width * 0.02f);
        styleLabel.fontStyle = FontStyle.Normal;

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

    private void onMouseClickDown()
    {
        isHandlingClick = true;
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
        //BinarySystemData data = new BinarySystemData((float)table.getC1()[indice],
        //                            (float)table.getM()[indice],
        //                            (float)table.getL1()[indice],
        //                            (float)table.getL2()[indice],
        //                            (float)table.getL3()[indice]);
        screen.setSystem(indice, countParticles, sliderPrimaryTemp, sliderSecondaryTemp);
    }

    public void openUrl(string url)
    {
        Application.OpenURL("http://www.bcc.unifal-mg.edu.br/lte/");
    }

    public void setDropdown()
    {
        dropdown = !dropdown;
    }

    public void renderTmperatureBarLabels()
    {
        styleLabel.fontSize = (int)(Screen.width * 0.013f);

        styleLabel.normal.textColor = Color.red;
        GUI.Label(new Rect(Screen.width * 0.175f, Screen.height * 0.097f, 120, 35), "1000", styleLabel);
        //styleLabel.normal.textColor = new Color(0, 0.5f, 1, 1);
        styleLabel.normal.textColor = Color.blue;
        GUI.Label(new Rect(Screen.width * 0.8f, Screen.height * 0.097f, 120, 35), "15000", styleLabel);
        styleLabel.normal.textColor = Color.white;

        GUI.Label(new Rect(Screen.width * 0.21f + (sliderSecondaryTemp - 1) * 59, Screen.height * 0.125f, 120, 35), "^", styleLabel);
        styleLabel.fontStyle = FontStyle.Bold;
        styleLabel.normal.textColor = new Color(1 - (sliderSecondaryTemp - 1) / 4, 0, (sliderSecondaryTemp - 1) / 4 - 0.5f, 1);
        GUI.Label(new Rect(Screen.width * 0.185f + (sliderSecondaryTemp - 1) * 59, Screen.height * 0.135f, 120, 35), "Secondary", styleLabel);

        styleLabel.fontStyle = FontStyle.Normal;
        styleLabel.normal.textColor = Color.white;

        GUI.Label(new Rect(Screen.width * 0.395f + (sliderPrimaryTemp - 5) * 50, Screen.height * 0.125f, 120, 35), "^", styleLabel);
        styleLabel.normal.textColor = new Color(0.3f - (sliderPrimaryTemp - 5) / 10, 0, (sliderPrimaryTemp - 5) / 10 + 0.5f, 1);
        styleLabel.fontStyle = FontStyle.Bold;
        GUI.Label(new Rect(Screen.width * 0.375f + (sliderPrimaryTemp - 5) * 50, Screen.height * 0.135f, 120, 35), "Primary", styleLabel);

        styleLabel.normal.textColor = Color.white;
        styleLabel.fontSize = (int)(Screen.width * 0.02f);
        styleLabel.fontStyle = FontStyle.Normal;
    }

}