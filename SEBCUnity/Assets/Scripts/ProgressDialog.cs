using UnityEngine;
using UnityEngine.UI;

public class ProgressDialog : MonoBehaviour
{

    public float progress = 1;

    // Use this for initialization
    void Start()
    {
        UserWindow.progressDialogWindow.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        GameObject.Find("Canvas/ProgressDialogWindow/Slider").GetComponent<Slider>().value = this.progress / 10;
    }

    //private Rect windowRect = new Rect(Screen.width / 2 - Screen.width * 0.15f, Screen.height / 2 - Screen.height * 0.075f, Screen.width * 0.3f, Screen.height * 0.15f);

    //void OnGUI()
    //{
    //    GUI.depth = -999;

    //    if (progress == 0)
    //    {
    //        GUI.DrawTexture(windowRect, GameObject.Find("EventSystem").GetComponent<UserWindow>().progressBarTexture0, ScaleMode.StretchToFill);
    //    }
    //    if (progress == 1)
    //    {
    //        GUI.DrawTexture(windowRect, GameObject.Find("EventSystem").GetComponent<UserWindow>().progressBarTexture1, ScaleMode.StretchToFill);
    //    }
    //    if (progress == 2)
    //    {
    //        GUI.DrawTexture(windowRect, GameObject.Find("EventSystem").GetComponent<UserWindow>().progressBarTexture2, ScaleMode.StretchToFill);
    //    }
    //    if (progress == 3)
    //    {
    //        GUI.DrawTexture(windowRect, GameObject.Find("EventSystem").GetComponent<UserWindow>().progressBarTexture3, ScaleMode.StretchToFill);
    //    }
    //    if (progress == 4)
    //    {
    //        GUI.DrawTexture(windowRect, GameObject.Find("EventSystem").GetComponent<UserWindow>().progressBarTexture4, ScaleMode.StretchToFill);
    //    }
    //    if (progress == 5)
    //    {
    //        GUI.DrawTexture(windowRect, GameObject.Find("EventSystem").GetComponent<UserWindow>().progressBarTexture5, ScaleMode.StretchToFill);
    //    }
    //    if (progress == 6)
    //    {
    //        GUI.DrawTexture(windowRect, GameObject.Find("EventSystem").GetComponent<UserWindow>().progressBarTexture6, ScaleMode.StretchToFill);
    //    }
    //    if (progress == 7)
    //    {
    //        GUI.DrawTexture(windowRect, GameObject.Find("EventSystem").GetComponent<UserWindow>().progressBarTexture7, ScaleMode.StretchToFill);
    //    }
    //}
}
