using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potential {
    
    // Calcula e retorna o potencial no ponto (x,y) com a massa relativa
	public float getPotential (float mass, float x, float y)
    {
        // Calcula os raios
		float radius1 = Mathf.Sqrt (Mathf.Pow (x - mass, 2) + Mathf.Pow (y, 2));
		float radius2 = Mathf.Sqrt (Mathf.Pow (x + 1 - mass, 2) + Mathf.Pow (y, 2));

        // Calcula o potencial
		float potential = (1 - mass) / radius1 + mass / radius2 + (Mathf.Pow (x, 2) + Mathf.Pow (y, 2)) / 2;

		return potential;
	}

}
