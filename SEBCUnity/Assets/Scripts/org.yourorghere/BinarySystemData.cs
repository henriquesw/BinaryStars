using System;
using System.Collections;

public class BinarySystemData
{

	private float C1;
	private float m;
	private float L1;
	private float L2;
    private float L3;
	private int slices;
	private int stacks;
	private int size;

	private ThreeBodyStart threeBodyStart;

	private ArrayList equipotentialRocheRedGiant;
	private ArrayList equipotentialRocheDwarf;
	private ArrayList caminho;

    public BinarySystemData(float C1, float m, float L1, float L2, float L3)
    {
        this.C1 = C1;
        this.m = m;
        this.L1 = L1;
        this.L2 = L2;
        this.L3 = L3;
        this.slices = 20;
        this.stacks = 15;
        this.size = this.slices * this.stacks + 2;

        this.threeBodyStart = new ThreeBodyStart(0.1 / 100000, this.L1, 0.0, 0.0, 0.0, 0.1, 9000);
        this.equipotentialRocheRedGiant = new ArrayList();
        this.equipotentialRocheDwarf = new ArrayList();
        this.caminho = new ArrayList();
    }

    public void generateRedGiant()
    {
        //Starting points being inserted for rendering lobe
        Point point;
        point = new Point(L1, 0, 0);
        this.equipotentialRocheRedGiant.Add(point);

        int ang = (int)(360 / slices);
        int pos = 1;
        float step1 = (L2 - L1) / (stacks + 1);
        float x = L1;
        float y = 0.0f;
        float r1, r2;
        double C1Temp;

        for (int cont1 = 1; cont1 <= stacks; cont1++)
        {
            y = 0.0f;
            x += step1;
            C1Temp = 0.0f;
            //Values ??for the points generated must be close to the value of specific equipotential
            while ((C1Temp > C1 + 0.001) || (C1Temp < C1 - 0.001))
            {
                y = y + 0.0001f;
                r1 = (float)Math.Sqrt(x * x + y * y);
                r2 = (float)Math.Sqrt((x - 1) * (x - 1) + (y * y));
                C1Temp = (2 / (1 + m)) * (1 / r1) + (2 * m) / (1 + m) * ((1 / r2) - x) + (x * x + y * y) + (m * m) / ((1 + m) * (1 + m));
            }

            for (int cont2 = 0; cont2 < slices; cont2++, pos++)
            {
                point = new Point(x, y * (float)Math.Cos(toRadians(ang * cont2)), y * (float)Math.Sin(toRadians(ang * cont2)));
                //Points being inserted for rendering lobe
                this.equipotentialRocheRedGiant.Add(point);
            }
        }
        //Last point in the list of values for the rendering of the lobe.
        point = new Point(L2, 0, 0);
        this.equipotentialRocheRedGiant.Add(point);
    }

    public void generateWhiteDwarf()
    {
        //Starting points being inserted for rendering RocheDwarf
        Point point;
        point = new Point(L3, 0, 0);
        this.equipotentialRocheDwarf.Add(point);

        int pos = 1;
        int ang = (int)(360 / slices);
        float step1 = (L1 - L3) / (stacks + 1);
        float x = L3;
        float y = 0.0f;
        float r1, r2;
        double C1Temp;

        for (int cont = 1; cont <= stacks; cont++)
        {
            y = 0.0f;
            x += step1;
            C1Temp = 0.0f;

            while ((C1Temp > C1 + 0.001) || (C1Temp < C1 - 0.001))
            {
                y = y + 0.0001f;
                r1 = (float)Math.Sqrt(x * x + y * y);
                r2 = (float)Math.Sqrt((x - 1) * (x - 1) + (y * y));
                C1Temp = (2 / (1 + m)) * (1 / r1) + (2 * m) / (1 + m) * ((1 / r2) - x) + (x * x + y * y) + (m * m) / ((1 + m) * (1 + m));

            }
            for (int contt = 0; contt < slices; contt++, pos++)
            {
                point = new Point(x, y * (float)Math.Cos(toRadians(ang * contt)), y * (float)Math.Sin(toRadians(ang * contt)));
                this.equipotentialRocheDwarf.Add(point);
            }
        }
        point = new Point(L1, 0, 0);
        equipotentialRocheDwarf.Add(point);
    }

    public void generateAnimatedParticles ()
	{
		this.threeBodyStart.generateParticlesPathRK ();
		setCaminho (this.threeBodyStart.getMaterialEnergetico ());
	}

	public double toRadians (double angle)
	{
		return (Math.PI / 180) * angle;
	}

	public float getC1 ()
	{
		return C1;
	}

	public float getL1 ()
	{
		return L1;
	}

	public float getL2 ()
	{
		return L2;
	}

	public float getL3 ()
	{
		return L3;
	}

	public float getM ()
	{
		return m;
	}

	public int getSlices ()
	{
		return slices;
	}

	public int getStacks ()
	{
		return stacks;
	}

	public int getSize ()
	{
		return size;
	}

	public void setSlices (int slices)
	{
		this.slices = slices;
	}

	public void setStacks (int stacks)
	{
		this.stacks = stacks;
	}

	public ArrayList getEquipotentialRocheDwarf ()
	{
		return equipotentialRocheDwarf;
	}

	public ArrayList getEquipotentialRocheRedGiant ()
	{
		return equipotentialRocheRedGiant;
	}

	public ArrayList getCaminho ()
	{
		return caminho;
	}

	public void setCaminho (ArrayList caminho)
	{
		this.caminho = caminho;
	}

	//	public LightCurveGenerator getGenerator() {
	//		return generator;
	//	}
	//
	//	public void setGenerator(LightCurveGenerator generator) {
	//		this.generator = generator;
	//	}
}
