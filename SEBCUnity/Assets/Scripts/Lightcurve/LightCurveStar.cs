public class LightCurveStar : TemperatureStar {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public double C;
	public double N;
	public double [] STHE, CTHE, PHA, CPHA, SPHA, WN;
	public double [,] WL, WM, BR;
	public double ThE, PhA;

	public LightCurveStar() :
		base() {
		this.initializeVectorsandMatrix();
	}
		
	protected override void initializeVectorsandMatrix() {
		base.initializeVectorsandMatrix();
		this.STHE = new double[this.NPH + 1];
		this.CTHE = new double[this.NPH + 1];
		this.PHA = new double[this.NPH + 1];
		this.CPHA = new double[this.NPH + 1];
		this.SPHA = new double[this.NPH + 1];
		this.WN = new double[this.NPH + 1];
		this.WL = new double[this.NPH,this.NPH + 1];
		this.WM = new double[this.NPH,this.NPH + 1];
		this.BR = new double[this.NPH,this.NPH + 1];

	}
}
