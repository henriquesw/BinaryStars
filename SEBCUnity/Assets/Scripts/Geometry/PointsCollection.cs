using System.Collections.Generic;
using System;

public class PointsCollection
{
	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

	public List<Double> R;
	public List<Double> G;
	public List<Double> x;
	public List<Double> y;
	public List<Double> z;
	private int indexBiggestX;
	private int indexSmallestX;

	public PointsCollection ()
	{
		this.R = new List<Double> ();
		this.G = new List<Double> ();
		this.x = new List<Double> ();
		this.y = new List<Double> ();
		this.z = new List<Double> ();
	}

	public double[] getVextorR ()
	{
		double[] saida = new double[this.R.Count];
		int i;
		for (i = 0; i < this.R.Count; i++) {
			saida [i] = this.R [i];
		}
		return saida;
	}

	public double[] getVextorG ()
	{
		double[] saida = new double[this.G.Count];
		int i;
		for (i = 0; i < this.G.Count; i++) {
			saida [i] = this.G [i];
		}
		return saida;
	}

	public double[] getVectorX ()
	{
		double[] saida = new double[this.x.Count];
		int i;
		for (i = 0; i < this.x.Count; i++) {
			saida [i] = this.x [i];
		}
		return saida;
	}

	public double[] getVectorY ()
	{
		double[] saida = new double[this.y.Count];
		int i;
		for (i = 0; i < this.y.Count; i++) {
			saida [i] = this.y [i];
		}
		return saida;
	}

	public double[] getVectorZ ()
	{
		double[] saida = new double[this.z.Count];
		int i;
		for (i = 0; i < this.z.Count; i++) {
			saida [i] = this.z [i];
		}
		return saida;
	}

	public void printPoints ()
	{
		int i;
		for (i = 0; i < this.x.Count; i++) {
			Console.WriteLine ("R: " + this.R [i] + "    G: " + this.G [i] + "    X: " + this.x [i] + "    Y: " + this.y [i] + "    Z: " + this.z [i]);
		}
		Console.WriteLine ("Size:" + i);
	}

	public void displacePoints ()
	{
		double displacement = 1.0;
		int i;
		double aux;
		for (i = 0; i < this.x.Count; i++) {
			aux = this.x [i];
			this.x [i] = displacement + aux;
		}
		for (i = 0; i < this.y.Count; i++) {
			aux = this.y [i];
			this.y [i] = displacement + aux;
		}
		//        for (i=0;i<this.z.Count;i++) {
		//            aux=this.z[i];
		//            this.z[i] = value+aux;
		//        }
	}

	public void displacePoints (double xDisplacement, double yDisplacement, double zDisplacement)
	{

		int i;
		double aux;
		for (i = 0; i < this.x.Count; i++) {
			aux = this.x [i];
			this.x [i] = xDisplacement + aux;
		}
		for (i = 0; i < this.y.Count; i++) {
			aux = this.y [i];
			this.y [i] = yDisplacement + aux;
		}
		for (i = 0; i < this.z.Count; i++) {
			aux = this.z [i];
			this.z [i] = zDisplacement + aux;
		}
	}

	public double toRadians (double val)
	{
		return (Math.PI / 180) * val;
	}

	public void rotateZ (double degreeAngle)
	{
		double radiansAngle = toRadians (degreeAngle);
		int i;
		double aux;
		for (i = 0; i < this.x.Count; i++) {
			aux = (this.x [i] * Math.Cos (radiansAngle)) - (this.y [i] * Math.Sin (radiansAngle));
			this.x [i] = aux;
			aux = (this.x [i] * Math.Sin (radiansAngle)) + (this.y [i] * Math.Cos (radiansAngle));
			this.y [i] = aux;
		}
	}

	public void rotateX (double degreeAngle)
	{
		double radiansAngle = toRadians (degreeAngle);
		int i;
		double aux;
		for (i = 0; i < this.x.Count; i++) {
			//            x’= x
			//            y’= y*cos(?) - z*sen(?)
			//            z’= y*sen(?) + z*cos(?) 

			aux = (this.y [i] * Math.Cos (radiansAngle)) - (this.z [i] * Math.Sin (radiansAngle));
			this.y [i] = aux;
			aux = (this.y [i] * Math.Sin (radiansAngle)) + (this.z [i] * Math.Cos (radiansAngle));
			this.z [i] = aux;
		}
	}

	public void changeCoordenates ()
	{
		List<Double> aux;
		aux = this.z;
		this.z = this.y;
		//this.x = this.y;
		this.y = aux;
	}

	public void findBiggestXIndex ()
	{
		int saida = 0, i;
		double aux = 0.0;
		for (i = 1; i < this.x.Count; i++) {
			if (this.x [i] > aux) {
				aux = this.x [i];
				saida = i;
			}
		}
		this.indexBiggestX = saida;
	}

	public void findSmallestXIndex ()
	{
		int saida = 0, i;
		double aux = this.x [this.indexBiggestX];
		for (i = 1; i < this.x.Count; i++) {
			if (this.x [i] < aux) {
				aux = this.x [i];
				saida = i;
			}
		}
		this.indexSmallestX = saida;
	}

	public float getX (int index)
	{
		String parser = this.x [index].ToString ();
		float saida = Single.Parse (parser);
		return saida;
	}

	public void print ()
	{
		int i;
		for (i = 0; i < this.x.Count; i++) {
			Console.WriteLine (this.R [i] + "     " + this.G [i] + "     " + this.x [i] + "     " + this.y [i] + "     " + this.z [i] + "     ");
		}
	}

	public int getIndexindexBiggestX ()
	{
		return indexBiggestX;
	}

	public void setindexBiggestX (int indexLagrangePoint)
	{
		this.indexBiggestX = indexLagrangePoint;
	}

	public int getIndexSmallestX ()
	{
		return indexSmallestX;
	}

	public void setIndexSmallestX (int indexSmallestX)
	{
		this.indexSmallestX = indexSmallestX;
	}

}
