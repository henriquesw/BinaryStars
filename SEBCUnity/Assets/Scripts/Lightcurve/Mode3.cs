using System;

public class Mode3 : LightCurve {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	protected double  TDOUT, TSPOT = 15000, BSPOT;

	protected static double FACTOR = -0.95,
	RIN = 0.135,
	ROUT = 0.39;
	//protected double RDK[], ADK[], BDK[][];

	public Mode3(int kcolor, double angu, double r01, double q2, double tp1, double tp2, double coef1, double coef2, double ab2, double beta, double Base, double tdout) :
		base(3, kcolor, angu, r01, q2, tp1, tp2, coef1, coef2, ab2, beta, Base) {
		this.TDOUT = tdout;//TDOUT=1000 por padrão até o momento
	}

	public override void runLightCurve() {
		this.initializeVectorandMatrix();
		base.runLightCurve();
		this.calculateStarsGeometricParameters();
		this.computeIntensityDistributions();

		this.calculateTheDisk();
		this.calculateTheBrightSpot();

		this.findSecondaryVisiblePoints();
	}

	protected override void calculateStarsGeometricParameters() {
		base.calculateStarsGeometricParameters();
		base.calculateSecondaryStarParameters();
		base.calculatePrimaryStarParameters();
		this.lastStepStarsGeometricParameters();
	}

	protected override void calculateTheDisk() {
		int i, j, k;
		double DRDK2, tdisk, bd;
		//System.out.println("Implementar este metodo!!!\nClasse Mode3 CalculateTheDisk()");
		DRDK = (ROUT - RIN) / (float) (NRDK);
		DRDK2 = DRDK / 2.0;
		DADK = 2.0 * Math.PI / (float) (NADK);
		//DO 50 I =1, NRDK
		for (i = 1; i <= NRDK; i++) {
			RDK[i] = RIN + DRDK * (float) (i) - DRDK2;
			tdisk = this.diskt(RDK[i]);
			//DO 50 J=1, NADK
			for (j = 1; j <= NADK; j++) {
				ADK[j] = DADK * (float) (j);
				BDK[i,j] = 0.0;
				//DO 51 K =1, ICOLOR
				for (k = 1; k <= ICOLOR; k++) {
					bd = this.black((double)this.PLAMDA[k - 1], (double)this.CLAMDA[k - 1], tdisk);
					BDK[i,j] = BDK[i,j] + PI2 * bd * UDK3 * FS * (double)SCOLOR[k - 1];
				}
			}
		}
	}

	protected override void calculateTheBrightSpot() {
		//precisa resolver os valores de entrada para NRSP, NASP1,NASP2,TSPOT, NP1,NP2!!! (resolvido)
		int k;
		double bst;
		if (NASP1 != NASP2) {
			BSPOT = 0.0;
			//DO 52 K =1, ICOLOR
			for (k = 1; k <= ICOLOR; k++) {
				bst = this.black((double)this.PLAMDA[k - 1], (double)this.CLAMDA[k - 1], TSPOT);
				BSPOT = BSPOT + PI2 * bst * UDK3 * FS * (double)SCOLOR[k - 1];
			}
		}
	}

	protected override void initializeVectorandMatrix() {
		base.initializeVectorandMatrix();
	}

	protected double diskt(double rdk) {
		double t, rrout;
		rrout = rdk / ROUT;
		t = TDOUT * (Math.Pow(rrout, FACTOR));
		return t;
	}
}
