using UnityEngine;

public class AboutWindow : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        UserWindow.aboutWindow.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        click();
    }

    private void click()
    {
        //Debug.Log(Input.mousePosition + "\t" + Screen.width * 0.9f + "\t" + Screen.height * 0.9f);

        if (Input.GetMouseButton(0) && GUIUtility.hotControl == 0 &&
            !RectTransformUtility.RectangleContainsScreenPoint(
                 UserWindow.aboutWindow.GetComponent<RectTransform>(),
                 Input.mousePosition) &&
                 !RectTransformUtility.RectangleContainsScreenPoint(
                 GameObject.Find("ActionBar").GetComponent<RectTransform>(),
                 Input.mousePosition))
        {
            destroy();
        }
    }

    public void destroy()
    {
        UserWindow.aboutWindow.SetActive(false);
        Destroy(this);
    }

    //void OnGUI()
    //{
    //    GUI.Window(2, new Rect(Screen.width * 0.18f, Screen.height * 0.105f, Screen.width * 0.635f, Screen.height * 0.78f),
    //    doMyWindow, "CBSS: Grafic Simulation of a Compact Binary Star System");
    //}

    //private void doMyWindow(int id)
    //{
    //    if (GUI.Button(new Rect(Screen.width * 0.605f, Screen.height * 0.006f, Screen.height * 0.042f, Screen.height * 0.042f), "",
    //        GameObject.Find("EventSystem").GetComponent<UserWindow>().styleButtonClose))
    //    {
    //        Destroy(this);
    //    }

    //    GUI.Label(new Rect(Screen.width * 0.01f, Screen.height * 0.05f, 1000, 1000),
    //        "The program graphically simulates a compact binary star system\n" +
    //        "and allows interaction by users, in input information and in the\n" +
    //        "generated virtual envoiriment.\n\n" +
    //        "Authors:\n" +
    //        "Gustavo Carvalho Souza, Adriano Luis da Silva, Pedro Pereira,\n" +
    //        "Hugo Luiz Camargo Pinto, Paulo Alexandre Bressan and \n" +
    //        "Artur Justiniano Roberto Junior\n\n" +
    //        "Federal University of Alfenas - Educational Technology Laboratory\n\n" +
    //        "Version: 1.0.1");

    //    GUI.DragWindow();
    //}

}
