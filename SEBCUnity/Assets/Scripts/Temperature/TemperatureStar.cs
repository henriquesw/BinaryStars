using UnityEngine;
using System.Collections;

public class TemperatureStar : GeometryStar  {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public double [,] T, SL, SM, SN, G, R, L;
	//protected double THE[], STH[];
	protected double DTH;
	//protected double THE[], STH[];
	protected double DPH;
	//protected double THE[], STH[];
	protected double TP4;
	public ArrayList TF;
	protected float MAX_TEMP;
	protected float MIN_TEMP;

	public TemperatureStar() : base() { 
	
		//this.setU(57);
		this.TF = new ArrayList();
		this.initializeVectorsandMatrix();
	}

	public TemperatureStar(double u, double ab) : base() {
		this.setU(u);
		this.setAB(ab);
		this.initializeVectorsandMatrix();
	}

	/**
     *
     */
	protected override void initializeVectorsandMatrix() {
		base.initializeVectorsandMatrix();
		T = new double[this.getNPH(),this.getNPH()+1];
		SL = new double[this.getNPH(),this.getNPH()+1];
		SM = new double[this.getNPH(),this.getNPH()+1];
		SN = new double[this.getNPH(),this.getNPH()+1];
		G = new double[this.getNPH(),this.getNPH()+1];
		//THE = new double[this.getNPH()];
		//STH = new double[this.getNPH()];
		R = new double[this.getNPH(),this.getNPH()+1];
		L = new double[this.getNPH(),this.getNPH()+1];
	}

	public void calculateMaxTemp() {
		double aux = 0.0;
		foreach (double x in this.TF) {
			if (x > aux) {
				aux = x;
			}
		}
		this.setMAX_TEMP(aux);
	}

	public void CalculateMinTemp() {
		double aux = 15000.0;
		foreach (double x in this.TF) {
			if (x < aux) {
				aux = x;
			}
		}
		this.setMIN_TEMP(aux);
	}

	public float getNormalizedTemperature(int index) {
		string aux = this.TF[index].ToString();
		return (float.Parse (aux) / this.MAX_TEMP);
	}

	/**
     * @return the DTH
     */
	public double getDTH() {
		return DTH;
	}

	/**
     * @param DTH the DTH to set
     */
	public void setDTH(double DTH) {
		this.DTH = DTH;
	}

	/**
     * @return the DPH
     */
	public double getDPH() {
		return DPH;
	}

	/**
     * @param DPH the DPH to set
     */
	public void setDPH(double DPH) {
		this.DPH = DPH;
	}

	/**
     * @return the TP4
     */
	public double getTP4() {
		return TP4;
	}

	/**
     * @param TP4 the TP4 to set
     */
	public void setTP4(double TP4) {
		this.TP4 = TP4;
	}

	/**
     * @return the MAX_TEMP
     */
	public float getMAX_TEMP() {
		return MAX_TEMP;
	}

	/**
     * @param MAX_TEMP the MAX_TEMP to set
     */
	public void setMAX_TEMP(double MAX_TEMP) {
		string aux = MAX_TEMP.ToString();
		this.MAX_TEMP = float.Parse(aux);
	}

	/**
     * @return the MIN_TEMP
     */
	public float getMIN_TEMP() {
		return MIN_TEMP;
	}

	/**
     * @param MIN_TEMP the MIN_TEMP to set
     */
	public void setMIN_TEMP(double MIN_TEMP) {
		string aux = MIN_TEMP.ToString();
		this.MIN_TEMP = float.Parse(aux);
	}
}
