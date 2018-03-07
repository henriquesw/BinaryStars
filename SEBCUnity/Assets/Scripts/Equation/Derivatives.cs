using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Derivatives {

	private float mass;

	public Derivatives(float mass)
    {
		this.mass = mass;
	}

	public float derivativeX (float x, float y)
    {	
		float radius1 = Mathf.Sqrt (Mathf.Pow (x - mass, 2) + Mathf.Pow (y, 2));
		float radius2 = Mathf.Sqrt (Mathf.Pow (x + 1 - mass, 2) + Mathf.Pow (y, 2));

		float derivative_x = -(1 - mass) * (x - mass) / Mathf.Pow (radius1, 3) - mass * (x + 1 - mass) / Mathf.Pow (radius2, 3) + x;

		return derivative_x;
	}

	public float derivativeY (float x, float y)
    {	
		float radius1 = Mathf.Sqrt (Mathf.Pow (x - mass, 2) + Mathf.Pow (y, 2));
		float radius2 = Mathf.Sqrt (Mathf.Pow (x + 1 - mass, 2) + Mathf.Pow (y, 2));

		float derivative_y = - (1 - mass) * y / Mathf.Pow (radius1, 3) - mass * y / Mathf.Pow (radius2, 3) + y;

		return derivative_y;
	}

}
