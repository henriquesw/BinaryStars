using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equation : MonoBehaviour {

    public float xStart;
    public float yStart;
    public float mass;
    public float dxStart;
    public float dyStart;

    public bool curve;

    private Derivatives derivatives;
    private Potential potential;
    private bool points = true;
    private bool controle1;

    // Use this for initialization
    void Start() {
        derivatives = new Derivatives();
        potential = new Potential();
    }

    // Update is called once per frame
    void Update() {
        if (points)
        {
            //pointsMath(xStart, yStart, dxStart, dyStart, mass);
        }
    }

    public List<Vector3> pointsMath(float x, float y, float dx, float dy, float mass)
    {
        List<Vector3> vertices = new List<Vector3>();
        curve = true;
        int count = 0;
        controle1 = true;
        while (controle1)
        {
            if (curve)
            {
                List<Vector3> list = pointsMathCurveDy(x, y, dy, mass);
                for (int i = 0; i < list.Count; i++)
                {
                    vertices.Add(list[i]);
                }
                Vector3 point2 = vertices[vertices.Count - 1];
                Vector3 point1 = vertices[vertices.Count - 2];
                x = point2.x;
                y = point2.y;
                if(point1.x > point2.x && dx > 0)
                    dx = -dx;
                else if (point1.x < point2.x && dx < 0)
                    dx = -dx;
            }
            else
            {
                List<Vector3> list = pointsMathCurveDx(x, y, dx, mass);
                for (int i = 0; i < list.Count; i++)
                {
                    vertices.Add(list[i]);
                }
                Vector3 point2 = vertices[vertices.Count - 1];
                Vector3 point1 = vertices[vertices.Count - 2];
                x = point2.x;
                y = point2.y;
                if (point1.y > point2.y && dy > 0)
                    dy = -dy;
                else if (point1.y < point2.y && dy < 0)
                    dy = -dy;
            }
            count++;
            curve = !curve;
        }
        return vertices;
    }

    private List<Vector3> pointsMathCurveDx(float x, float y, float dx, float mass)
    {
        List<Vector3> vertices = new List<Vector3>();

        float dy = 0;
        float V = 0;
        float x12 = 0;
        float y12 = 0;
        float lastDistance = 0;
        bool near = false;
        Vector3 startPoint = new Vector3(x, y);

        float k = potential.getPotential(x, y, mass);

        bool controle2 = true;
        while (controle2)
        {
            x12 = x + 0.5f * dx;
            y12 = y - 0.5f * dx * derivatives.derivativeX(x, y, mass) / derivatives.derivativeY(x, y, mass);
            x = x + dx;

            dy = Mathf.Abs(-dx * derivatives.derivativeX(x12, y12, mass) / derivatives.derivativeY(x12, y12, mass));

            V = potential.getPotential(x, y, mass);

            //Debug.Log("(" + x + ", " + y + ", " + V + ", " + dx + ", " + dy + ")");

            if (Mathf.Abs(dx) >= dy)
                controle2 = true;
            else
            {
                controle2 = false;
                break;
            }

            y = y - dx * derivatives.derivativeX(x12, y12, mass) / derivatives.derivativeY(x12, y12, mass);

            y = y - (potential.getPotential(x, y, mass) - k) / derivatives.derivativeY(x, y, mass);

            dy = Mathf.Abs(-dx * derivatives.derivativeX(x12, y12, mass) / derivatives.derivativeY(x12, y12, mass));

            //Stop the calculo if y minor than zero
            if (y < 0)
            {
                controle1 = false;
                break;
            }

            //For points that don't get negative this looks if the point is near the initial point;
            //Begin
            float distance = Vector3.Distance(new Vector3(x, y), startPoint);
            if (distance < lastDistance)
            {
                near = true;
            }
            if (distance > lastDistance && near)
            {
                controle1 = false;
                break;
            }
            lastDistance = distance;
            //End

            vertices.Add(new Vector3(x, y));
            Debug.Log("(x,y)=(" + x + "," + y + ")");
        }
        points = false;
        Debug.Log(vertices.Count);
        return vertices;
    }

    private List<Vector3> pointsMathCurveDy(float x, float y, float dy, float mass)
    {
        List<Vector3> vertices = new List<Vector3>();

        float dx = 0;
        float V = 0;
        float x12 = 0;
        float y12 = 0;
        float lastDistance = 0;
        bool near = false;
        Vector3 startPoint = new Vector3(x, y);

        float k = potential.getPotential(x, y, mass);

        bool controle3 = true;
        while (controle3)
        {
            y12 = y + 0.5f * dy;
            x12 = x - 0.5f * dy * derivatives.derivativeY(x, y, mass) / derivatives.derivativeX(x, y, mass);

            dx = Mathf.Abs(-dy * derivatives.derivativeY(x12, y12, mass) / derivatives.derivativeX(x12, y12, mass));

            V = potential.getPotential(x, y, mass);

            //Debug.Log("(x,y,V,dx,dy)=(" + x + ", " + y + ", " + V + ", " + dx + ", " + dy + ")");

            if (Mathf.Abs(dy) >= dx)
                controle3 = true;
            else
            {
                controle3 = false;
                break;
            }
            y = y + dy;
            x = x - dy * derivatives.derivativeY(x12, y12, mass) / derivatives.derivativeX(x12, y12, mass);

            x = x - ((potential.getPotential(x, y, mass) - k) / derivatives.derivativeX(x, y, mass));

            //Stop the calculo if y minor than zero
            if (y < 0)
            {
                controle1 = false;
                break;
            }

            //For points that don't get negative this looks if the point is near the initial point;
            //Begin
            float distance = Vector3.Distance(new Vector3(x, y), startPoint);
            if (distance < lastDistance)
            {
                near = true;
            }
            if (distance > lastDistance && near)
            {
                controle1 = false;
                break;
            }
            lastDistance = distance;
            //End

            vertices.Add(new Vector3(x, y));
            Debug.Log("(x,y)=(" + x + "," + y + ")");
        }
        points = false;
        Debug.Log(vertices.Count);
        return vertices;
    }
}
