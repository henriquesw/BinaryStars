using UnityEngine;
using UnityEngine.UI;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Globalization;
using System.Collections;

public class EmailWindow : MonoBehaviour
{

    private Button submiteEmailButton;
    private string email;
    private double [] phase;
    private double [] bright;

    // Use this for initialization
    void Start()
    {
        UserWindow.emailWindow.SetActive(true);

        submiteEmailButton = GameObject.Find("EmailWindow/Panel/Send").GetComponent<Button>();
        submiteEmailButton.onClick.AddListener(() =>  StartCoroutine(sendEmail()));
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
                 UserWindow.emailWindow.GetComponent<RectTransform>(),
                 Input.mousePosition) &&
                 !RectTransformUtility.RectangleContainsScreenPoint(
                 GameObject.Find("ActionBar").GetComponent<RectTransform>(),
                 Input.mousePosition))
        {
            destroy();
        }
    }

    private IEnumerator sendEmail()
    {
        email = GameObject.Find("EmailWindow/Panel/InputField/Text").GetComponent<Text>().text;

        if (!email.Equals(""))
        {
            gameObject.AddComponent<ProgressDialog>();
            yield return null;

            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("tablet.lte@bcc.unifal-mg.edu.br");
            mail.To.Add(email);
            mail.Subject = "Binary Stars 3D - Curva de Luz";
            mail.Body = printLightCurveTable();

            GameObject.Find("EventSystem").GetComponent<ProgressDialog>().progress = 2;
            yield return null;

            SmtpClient smtpServer = new SmtpClient("smtp.gmail.com");
            smtpServer.Port = 587;
            smtpServer.Credentials = new NetworkCredential("tablet.lte@bcc.unifal-mg.edu.br", "-Asdqma?0") as ICredentialsByHost;
            smtpServer.EnableSsl = true;

            GameObject.Find("EventSystem").GetComponent<ProgressDialog>().progress = 4;
            yield return null;

            ServicePointManager.ServerCertificateValidationCallback =
            delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
            { return true; };

            GameObject.Find("EventSystem").GetComponent<ProgressDialog>().progress = 6;
            yield return null;

            try
            {
                smtpServer.Send(mail);
            } catch (SmtpException se)
            {
                // AVISAR USUÁRIO QUE DEU ERRADO E PQ DEU ERRADO
                // PROBLEMAS DE AUTENTICAÇÃO
            }
            //GameObject.Find("EventSystem").GetComponent<ProgressDialog>().progress = 10;
            //yield return null;
            GameObject.Find("EventSystem").GetComponent<ProgressDialog>().progress = 0;

            DestroyImmediate(GetComponent<ProgressDialog>());
            UserWindow.progressDialogWindow.SetActive(false);
            yield return null;
        }
        else
        {
            // AVISAR USUÁRIO QUE DEU ERRADO E PQ DEU ERRADO
            // PROBLEMAS COM CAMPOS VAZIOS
        }
    }

    private string printLightCurveTable()
    {
        StringBuilder sb = new StringBuilder();

        for(int i = 0; i < bright.Length; i++)
        {
            sb.Append(phase[i].ToString("F", CultureInfo.InvariantCulture));
            sb.Append("\t");
            sb.AppendLine(bright[i].ToString());
        }

        return sb.ToString();
    }

    public void setBrit(double [] bright)
    {
        this.bright = bright;
    }

    public void setPhase(double [] phase)
    {
        this.phase = phase;
    }

    public void destroy()
    {
        UserWindow.emailWindow.SetActive(false);
        Destroy(this);
    }
}
