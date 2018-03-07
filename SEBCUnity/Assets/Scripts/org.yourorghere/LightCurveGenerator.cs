using UnityEngine;
using System.Collections;

public class LightCurveGenerator
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private Geometry geometry;
    private Temperature temperature;
    private LightCurve lc;

    private int KFLAG;
    private float m;
    private double t1p;
    private double t2p;
    private double angle;

    public LightCurveGenerator(int KFLAG, float m, double t1p, double t2p, double angle)
    {
        this.KFLAG = KFLAG;
        this.m = m;
        this.t1p = t1p;
        this.t2p = t2p;
        this.angle = angle;
    }

    public IEnumerator generateLightCurves()
    {
        //Thread.Sleep(1000);
        geometry = new Geometry(KFLAG, 5, angle, 0.00001, m, t1p, t2p, -0.237, 1.016, 0.676, 0.08, 14.07);
        geometry.calculateGeometry();

        GameObject.Find("EventSystem").GetComponent<ProgressDialog>().progress = 1;
        yield return null;

        //System.out.println(t1p +" " + t2p);
        if (KFLAG == 1)
        {
            temperature = new Temperature1(angle, t1p, t2p, 0.87, 0.87, 0.7, 0.676, 0.08, 31, 51, 31, 51);
        }
        else if (KFLAG == 2)
        {
            temperature = new Temperature2(0.00001, t1p, t2p, 0.87, 0.87, 0.7, 0.676, 0.08, 31, 51, 31, 51);
        }
        else
        {
            temperature = new Temperature3(0.00001, t1p, t2p, 0.87, 0.87, 0.7, 0.676, 1000, 0.5, 0.08, 31, 51, 31, 51, 31, 51, 0.135, 0.39, -0.95);
        }
        temperature.setPrimaryPoints(geometry.getPrimary().getPontos());
        temperature.setSecondaryPoints(geometry.getSecondary().getPontos());
        temperature.runTemperature();

        GameObject.Find("EventSystem").GetComponent<ProgressDialog>().progress = 2;
        yield return null;

        if (KFLAG == 1)
        {
            lc = new Mode1(5, angle, geometry.getPrimary().getR0(), m, t1p, t2p, -0.237, 1.016, 0.676, 0.08, 14.07);
        }
        else if (KFLAG == 2)
        {
            lc = new Mode2(5, angle, 0.00001, m, t1p, t2p, -0.237, 1.016, 0.676, 0.08, 14.07);
        }
        else
        {
            lc = new Mode3(5, angle, 0.00001, m, t1p, t2p, -0.237, 1.016, 0.676, 0.08, 14.07, 1000);
        }
        //lc = new Mode3(5, 57, 0.00001, 0.5, 15000, 3200, -0.237, 1.016, 0.676, 0.08, 14.07, 1000);
        lc.setPrimaryPoints(geometry.getPrimary().getPontos());
        lc.setSecondaryPoints(geometry.getSecondary().getPontos());
        lc.setPrimaryTemperature(temperature.primary.TF);
        lc.setSecondaryTemperature(temperature.secondary.TF);
        lc.runLightCurve();

        GameObject.Find("EventSystem").GetComponent<ProgressDialog>().progress = 3;
        yield return null;
    }

    public Geometry getGEO()
    {
        return geometry;
    }

    public Temperature getTEMP()
    {
        return temperature;
    }

    public LightCurve getLC()
    {
        return lc;
    }
}
