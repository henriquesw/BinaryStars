using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class LagrangePoints {

	private Point[] points;
	private float mass;

	public LagrangePoints (float mass)
    {
		this.mass = mass;
		this.L1 ();
		this.L2 ();
		this.L3 ();
		this.L4 ();
		this.L5 ();
	}

	private void L1 ()
    {	
		float l1 = 0;
		float delta;
		float g;
		int h1;
		int h2;

		if (l1 < mass && l1 > (mass-1))
        {
			h1 = -1;
			h2 = 1;
		}
        else
        {
			h1 = 1;
			h2 = -1;
		}
			
		delta = (float) 1e-15;
		while (delta >= 1e-15)
        {
			g = l1;
			l1 = (float) ((g) - ((-h1 * ((1 - mass) / (Mathf.Pow(g - mass, 2))) - h2 * ((mass) / (Mathf.Pow(g + 1 - mass, 2))) + g) / (2 * h1 * ((1 - mass) / (Mathf.Pow(g - mass, 3))) + 2 * h2 * ((mass) / (Mathf.Pow(g + 1 - mass, 3))) + 1)));
			delta = Mathf.Abs (g - l1);
		}
		
		points [0] = new Point ((float) l1, 0);

    }

	private void L2 ()
    {
		float l2 = -1;
		float delta;
		float g;
		int h1;
		int h2;

		if (l2 < mass - 1)
        {
			h1 = -1;
			h2 = -1;
		}
        else
        {
			h1 = 1;
			h2 = 1;
		}

		delta = (float) 1e-15;
		while (delta >= 1e-15)
        {
			g = l2;
			l2 = (float) ((g) - (( -h1*((1-mass)/(Mathf.Pow((g-mass),2)))-h2*((mass)/(Mathf.Pow((g+1-mass),2)))+g)/(2*h1*((1-mass)/(Mathf.Pow((g-mass),3)))+2*h2*((mass)/(Mathf.Pow((g+1-mass),3)))+1)));
			delta = Mathf.Abs (g - l2);
		}

		points [1] = new Point ((float) l2, 0);
	}

	private void L3 ()
    {
		float l3 = 1;
		float delta;
		float g;
		int h1;
		int h2;

		if (l3 > mass)
        {
			h1 = 1;
			h2 = 1;
		}
        else
        {
			h1 = -1;
			h2 = -1;
		}

		delta = (float) 1e-15;
		while (delta >= 1e-15)
        {
			g = l3;
			l3 = (float) ((g) - ((-h1 * ((1 - mass) / (Mathf.Pow((g - mass), 2))) - h2 * (mass / (Mathf.Pow((g + 1 - mass), 2))) + g) / (2 * h1 * ((1 - mass) / (Mathf.Pow((g - mass), 3))) + 2 * h2 * (mass / (Mathf.Pow((g + 1 - mass), 3))) + 1)));
			delta = Mathf.Abs (g - l3);
		}

		points [2] = new Point ((float) l3, 0);
	}

	private void L4 ()
    {
		float l4_x = (float) (mass - 0.5);
		float l4_y = (float) Mathf.Sin (60);

		points [3] = new Point (l4_x, l4_y);
	}

	private void L5 ()
    {
		float l5_x = (float) (mass - 0.5);
		float l5_y = (float) -Mathf.Sin (60);

		points [4] = new Point (l5_x, l5_y);
	}

	public Point getLagrangePoint (int position)
    {
		return points [position - 1];
	}
}
