using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equation : MonoBehaviour {

    public float xStart;
    public float yStart;
    public float mass;
    private Derivatives derivatives;
    private Potential potential;

	// Use this for initialization
	void Start () {
        derivatives = new Derivatives(mass);
        potential = new Potential();
		pointsMath();
	}
	
	// Update is called once per frame
	void Update () {

	}

	private void pointsMath ()
    {
        float k = potential.getPotential(mass, xStart, yStart);

        float x = xStart;
        float y = yStart;
        int curve = 1;
        int control1 = 1;
		int control2;
		int control3;
		int count = 0;
		float dx = 0, dy = 0;
        float x12;
        float y12;
        float V;

        while (control1 == 1 && count < 10)
        {
            if (curve == 1)
            {
                dx = -0.05f;
            }
            else
            {
                dy = 0.05f;
            }

            if (curve == 1)
            {
                control2 = 2;
                while (control2 == 2)
                {

                    x12 = x + 0.5f * dx;
                    y12 = y - 0.5f * dx * derivatives.derivativeX(x, y) / derivatives.derivativeY(x, y);
                    x = x + dx;

                    dy = Mathf.Abs(-dx * derivatives.derivativeX(x12, y12) / derivatives.derivativeY(x12, y12));

                    V = potential.getPotential(mass, x, y);

                    if (Mathf.Abs(dx) >= dy)
                    {
                        control2 = 2;
                    }
                    else
                    {
                        control2 = 3;
                        break;
                    }

                    y = y - dx * derivatives.derivativeX(x12, y12) / derivatives.derivativeY(x12, y12);

                    y = y - (potential.getPotential(mass, x, y) - k) / derivatives.derivativeY(x, y);

                    dy = Mathf.Abs(-dx * derivatives.derivativeX(x12, y12) / derivatives.derivativeY(x12, y12));

					Debug.Log(x + " " + y);
                }
            }
            else
            {
                control3 = 3;
                while (control3 == 3)
                {
                    y12 = y + 0.5f * dy;
                    x12 = x - 0.5f * dy * derivatives.derivativeX(x, y) / derivatives.derivativeY(x, y);

                    dx = Mathf.Abs(-dy * derivatives.derivativeX(x12, y12) / derivatives.derivativeY(x12, y12));

                    V = potential.getPotential(mass, x, y);

                    if (Mathf.Abs(dy) >= dx)
                    {
                        control3 = 3;
                    }
                    else
                    {
                        control3 = 2;
                        break;
                    }

                    y = y + dy;
                    x = x - dy * derivatives.derivativeX(x12, y12) / derivatives.derivativeY(x12, y12);

                    x = x - (potential.getPotential(mass, x, y) - k) / derivatives.derivativeX(x, y);

					Debug.Log(x + " " + y);
                }
            }

            control1 = 1;

            if (control1 == 1)
            {
                if (curve == 1)
                {
                    curve = 2;
                }
                else
                {
                    curve = 1;
                }
            }
            count++;
        }
    }
}
