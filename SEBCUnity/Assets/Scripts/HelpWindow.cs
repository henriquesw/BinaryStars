using UnityEngine;

public class HelpWindow : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        UserWindow.helpWindow.SetActive(true);
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
                 UserWindow.helpWindow.GetComponent<RectTransform>(),
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
        UserWindow.helpWindow.SetActive(false);
        Destroy(this);
    }

    //void OnGUI()
    //{
    //    GUI.Window(1, new Rect(Screen.width * 0.18f, Screen.height * 0.105f, Screen.width * 0.635f, Screen.height * 0.78f), doMyWindow, "Help");
    //}

    //private void doMyWindow(int id)
    //{

    //    if (GUI.Button(new Rect(Screen.width * 0.605f, Screen.height * 0.006f, Screen.height * 0.042f, Screen.height * 0.042f), "X"))
    //    {
    //        Destroy(this);
    //    }

    //    GUI.DragWindow();

    //}

}
