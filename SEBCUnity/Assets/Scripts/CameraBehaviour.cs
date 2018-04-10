using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CameraBehaviour : MonoBehaviour {

	public Slider slider;

    private float currentAngle;
    private Vector2 rotation;

    private float horizontalMove = 1f;

    // Use this for initialization
    void Start () {
        currentAngle = slider.value;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            if (EventSystem.current.currentSelectedGameObject == GameObject.Find("TouchDetector"))
                TouchMove();
        }
    }

	private void TouchMove() {
		rotation = Input.GetTouch (0).deltaPosition;

        if (rotation.x > 0)
        {
            if (rotation.y > 0)
            {
                MoveHorizontal(1);
            }
            else
            {
                MoveHorizontal(1);
            }
        }
        else
        {
            if (rotation.y > 0)
            {
                MoveHorizontal(-1);
            }
            else
            {
                MoveHorizontal(-1);
            }
        }

        slider.value += rotation.y;
	}

    private void MoveHorizontal (float direction)
    {
        transform.RotateAround(Vector3.zero, Vector3.up, direction * horizontalMove);
    }

    public void OnSliderChange ()
    {
        float angle = slider.value - currentAngle;
        transform.RotateAround(Vector3.zero, transform.right, angle);
        currentAngle = slider.value;
    }
}
