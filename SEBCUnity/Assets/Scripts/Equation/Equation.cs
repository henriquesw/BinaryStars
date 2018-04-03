using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equation : MonoBehaviour {

    public float xStart;
    public float yStart;
    public float mass;
    public int curve;
    public float dxStart;
    public float dyStart;

    private Derivatives derivatives;
    private Potential potential;
    private bool points = true;

    // Use this for initialization
    void Start() {
        derivatives = new Derivatives(mass);
        potential = new Potential();
        
    }

    // Update is called once per frame
    void Update() {
        if (points)
            pointsMath();
    }

    private void pointsMath()
    {
        int controle0 = 0;
        while (controle0 == 0)
        {
            float x = 0;
            float y = 0;
            float dx = 0;
            float dy = 0;
            float k = 0;
            float V = 0;
            float x12 = 0;
            float y12 = 0;

            x = xStart;
            y = yStart;
            k = potential.getPotential(x, y, mass);
            //k = 1.716751166f;
            Debug.Log(k);

            int controle1 = 1;
            while (controle1 == 1)
            {
                if (curve == 1)
                    dx = dxStart;
                else
                    dy = dyStart;

                if (curve == 1)
                {
                    int controle2 = 2;
                    while (controle2 == 2)
                    {
                        x12 = x + 0.5f * dx;
                        y12 = y - 0.5f * dx * derivatives.derivativeX(x, y) / derivatives.derivativeY(x, y);
                        x = x + dx;

                        dy = Mathf.Abs(-dx * derivatives.derivativeX(x12, y12) / derivatives.derivativeY(x12, y12));

                        V = potential.getPotential(x, y, mass);

                        Debug.Log("(" + x + ", " + y + ", " + V + ", " + dx + ", " + dy + ")");

                        if (Mathf.Abs(dx) >= dy)
                            controle2 = 2;
                        else
                        {
                            controle2 = 3;
                            break;
                        }

                        y = y - dx * derivatives.derivativeX(x12, y12) / derivatives.derivativeY(x12, y12);

                        y = y - (potential.getPotential(x, y, mass) - k) / derivatives.derivativeY(x, y);

                        dy = Mathf.Abs(-dx * derivatives.derivativeX(x12, y12) / derivatives.derivativeY(x12, y12));
                        Debug.Log("(x,y)=(" + x + "," + y + ")");
                    }
                }
                else
                {
                    int controle3 = 3;
                    while (controle3 == 3)
                    {
                        y12 = y + 0.5f * dy;
                        x12 = x - 0.5f * dy* derivatives.derivativeY(x, y) / derivatives.derivativeX(x, y);

                        dx = Mathf.Abs(-dy* derivatives.derivativeY(x12, y12) / derivatives.derivativeX(x12, y12));

                        V = potential.getPotential(x, y, mass);

                        Debug.Log("(x,y,V,dx,dy)=(" + x + ", " + y + ", " + V + ", " + dx + ", " + dy + ")");

                        if (Mathf.Abs(dy) >= dx)
                            controle3 = 3;
                        else
                        {
                            controle3 = 2;
                            break;
                        }
                        y = y + dy;
                        x = x - dy* derivatives.derivativeY(x12, y12) / derivatives.derivativeX(x12, y12);

                        x = x - ((potential.getPotential(x, y, mass) - k) / derivatives.derivativeX(x, y));
                        Debug.Log("(x,y)=(" + x + "," + y + ")");
                    }
                }
                controle1 = 0;
                if (controle1 == 1)
                {
                    if (curve == 1)
                        curve = 2;
                    else
                        curve = 1;
                }
                if (controle1 == 1)
                    controle0 = 1;
                else
                    controle0 =  2;
                points = false;
            }
        }
    }

    /*private void pointsMath ()
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
    }*/
}
