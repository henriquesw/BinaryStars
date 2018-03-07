public class Mode2 : LightCurve {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public Mode2(int kcolor, double angu, double r01, double q2, double tp1, double tp2, double coef1, double coef2, double ab2, double beta, double Base) :
		base(2, kcolor, angu, r01, q2, tp1, tp2, coef1, coef2, ab2, beta, Base) {
	}

	public override void runLightCurve() {
		base.runLightCurve();
		this.calculateStarsGeometricParameters();
		this.computeIntensityDistributions();
		this.findSecondaryVisiblePoints();
	}

	protected override void calculateStarsGeometricParameters() {
		base.calculateStarsGeometricParameters();
		base.calculateSecondaryStarParameters();
		base.calculatePrimaryStarParameters();
		this.lastStepStarsGeometricParameters();
	}

}
