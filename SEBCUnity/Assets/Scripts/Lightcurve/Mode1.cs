using System;

public class Mode1 : LightCurve {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public Mode1(int kcolor, double angu, double r01, double q2, double tp1, double tp2, double coef1, double coef2, double ab2, double beta, double Base) :
	base(1, kcolor, angu, r01, q2, tp1, tp2, coef1, coef2, ab2, beta, Base) {
	}

	public override void runLightCurve() {
		base.runLightCurve();
		this.calculateStarsGeometricParameters();
		this.computeIntensityDistributions();
		this.findSecondaryVisiblePoints();
		//this.setGraphicData();
	}
		
	protected override void calculateStarsGeometricParameters() {
		base.calculateStarsGeometricParameters();
		base.calculateSecondaryStarParameters();
		this.calculatePrimaryStarParameters();
		this.lastStepStarsGeometricParameters();
	}

	protected override void calculatePrimaryStarParameters() {
		int i, j, k = 0;
		this.primary.setDTH(2.0 / (float) (this.primary.getNTH() - 1));
		//DO 26 I = 1, NTH1
		for (i = 1; i <= this.primary.getNTH(); i++) {
			this.primary.WN[i] = 1.0 - this.primary.getDTH() * (float) (i - 1);
			this.primary.ThE = Math.Acos(this.primary.WN[i]);
			this.primary.STHE[i] = Math.Sin(this.primary.ThE);
			//DO 26 J = 1, NPH1
			for (j = 1; j <= this.primary.getNPH(); j++) {
				this.primary.PhA = this.primary.getDPH() * (float) (j - 1);
				this.primary.WL[i,j] = this.primary.STHE[i] * Math.Cos(this.primary.PhA);
				//double aux = this.primary.STHE[i];
				//double aux1 = Math.Sin(this.primary.PhA);
				//double aux2 = this.primary.STHE[i] * Math.Sin(this.primary.PhA);
				this.primary.WM[i,j] = this.primary.STHE[i] * Math.Sin(this.primary.PhA);

				this.primary.R[i,j] = this.primary.getPontos().R[k];
				this.primary.G[i,j] = this.primary.getPontos().G[k];
				this.primary.SL[i,j] = this.primary.getPontos().x[k];
				this.primary.SM[i,j] = this.primary.getPontos().y[k];
				this.primary.SN[i,j] = this.primary.getPontos().z[k];
				k++;
			}
		}
	}
}
