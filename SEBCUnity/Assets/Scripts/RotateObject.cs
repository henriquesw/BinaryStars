using UnityEngine;

public class RotateObject : MonoBehaviour
{

    private float rotationX;
    //velocidade que terá a rotação
    private float rotationSpeedX = 50.0f;
    private float rotationSpeedY = 100.0f;
    //Se movimento é com suavização
    private bool smooth = true;
    //limita a rotação no eixo X
    //private float rotationMinX = -360.0f;
    //private float rotationMaxX = 360.0f;
    //limita a rotação no eixo Y
    private float rotationMinY = 0f;
    private float rotationMaxY = 90f;
    //tempo para dar o smooth
    private float smoothTime = 0.2f;
    //variaveis referencia para velocidade
    private float xVelocity = 0.0f;
    private float yVelocity = 0.0f;
    //guarda o valor de x e y enquanto está interpolando
    private float xSmooth = 0.0f;
    private float ySmooth = 0.0f;

    private Vector2 clickDeltaPosition;
    private Vector2 clickPosition;

    private bool isTablet = true;

    void Start()
    {
        //inicia já na posição setada
        ySmooth = UserWindow.sliderRotationY = 0;
        //inicializa nas posicões passadas
        updateRotation();
    }

    void LateUpdate()
    {

        //Debug.Log (GUIUtility.hotControl == 0); Ignores mouse click when click on GUI elements
        if (isTablet)
        {
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved && GUIUtility.hotControl == 0)
            {
                touchMove();
            }
        }
        else
        {
            if (Input.GetMouseButton(0) && GUIUtility.hotControl == 0)
            {
                mouseClickMove();
            }
        }

        if (smooth)
        {
            //trava a rotação smooth nos limites
            UserWindow.sliderRotationY = Mathf.Clamp(UserWindow.sliderRotationY, rotationMinY, rotationMaxY);

            xSmooth = Mathf.SmoothDamp(xSmooth, rotationX, ref xVelocity, smoothTime);
            ySmooth = Mathf.SmoothDamp(ySmooth, UserWindow.sliderRotationY, ref yVelocity, smoothTime);
        }
        updateRotation();

    }

    private void touchMove()
    {
        clickDeltaPosition = Input.GetTouch(0).deltaPosition;
        clickPosition = Input.GetTouch(0).position;

        checkClickPositionAndMove(1);
    }

    private void mouseClickMove()
    {
        clickPosition = Input.mousePosition;
        clickDeltaPosition = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        checkClickPositionAndMove(5);
    }

    private void checkClickPositionAndMove(float speedMultiplier)
    {
        if (clickPosition.x > Screen.width * 0.15 && clickPosition.x < Screen.width * 0.85 &&
                clickPosition.y > Screen.height * 0.15 && clickPosition.y < Screen.height * 0.90)
        {

            if (UserWindow.helpWindow.activeSelf || UserWindow.aboutWindow.activeSelf ||
                UserWindow.lightCurveWindow.activeSelf || UserWindow.emailWindow.activeSelf)
            {

                if (!RectTransformUtility.RectangleContainsScreenPoint(
                    UserWindow.helpWindow.GetComponent<RectTransform>(),
                    clickPosition) ||
                !RectTransformUtility.RectangleContainsScreenPoint(
                    UserWindow.aboutWindow.GetComponent<RectTransform>(),
                    clickPosition) ||
                !RectTransformUtility.RectangleContainsScreenPoint(
                    UserWindow.lightCurveWindow.GetComponent<RectTransform>(),
                    clickPosition) ||
                !RectTransformUtility.RectangleContainsScreenPoint(
                    UserWindow.emailWindow.GetComponent<RectTransform>(),
                    clickPosition))
                {
                    setMoviment(clickDeltaPosition, speedMultiplier);
                }
            }
            else
            {
                setMoviment(clickDeltaPosition, speedMultiplier);
            }
        }
    }

    private void setMoviment(Vector2 position, float speedMultiplier)
    {
        rotationX += position.x * rotationSpeedX * speedMultiplier * Time.deltaTime;
        UserWindow.sliderRotationY -= position.y * rotationSpeedY * speedMultiplier * Time.deltaTime;
    }

    private void updateRotation()
    {
        Quaternion rotation;

        if (smooth)
        {
            rotation = Quaternion.Euler(ySmooth, xSmooth, 0);
        }
        else
        {
            //acerta os angulos para nao passarem de 0 ou 90
            //rotationX = ClampAngleX(rotationX, rotationMinX, rotationMaxX);
            UserWindow.sliderRotationY = ClampAngleY(UserWindow.sliderRotationY, rotationMinY, rotationMaxY);
            rotation = Quaternion.Euler(UserWindow.sliderRotationY, rotationX, 0);
        }
        transform.rotation = rotation;
    }

    private float ClampAngleY(float angle, float min, float max)
    {
        //acerta os angulos para nao passarem de 0 ou 90
        if (angle < 0)
            angle = 0;
        if (angle > 90)
            angle = 90;
        //garante que o angulo esta no intervalor setado
        return Mathf.Clamp(angle, min, max);
    }

    private float ClampAngleX(float angle, float min, float max)
    {
        //acerta os angulos para nao passarem de -360 ou 360
        if (angle < -360)
            angle += 360;
        if (angle > 360)
            angle -= 360;
        //garante que o angulo esta no intervalor setado
        return Mathf.Clamp(angle, min, max);
    }

}