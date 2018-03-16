using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraBehaviour : MonoBehaviour {

	public Slider slider;

	private Vector3 startPosition;
	private Quaternion startRotation;


	// Use this for initialization
	void Start () {
		startPosition = transform.position;
		startRotation = transform.rotation;
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void rotateCamera (Slider slider)
	{
		transform.position = startPosition;
		transform.rotation = startRotation;
		transform.RotateAround (Vector3.zero , Vector3.right, 90 - slider.value);
	}

}
