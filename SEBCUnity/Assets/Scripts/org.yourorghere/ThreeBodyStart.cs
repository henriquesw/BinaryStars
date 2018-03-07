using System.Collections;
using System;

public class ThreeBodyStart {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private double parMass;
    private double posicaox, posicaoy, velocidadeU, velocidadeV;
	private double posicaox0, posicaoy0, dt, fu, fv;
	private double L1;
	private int numeroRepeticoes;
	private ArrayList materialEnergetico;

    public ThreeBodyStart(double parMass, double posicaox0, double posicaoy0, double velocidadeU0, double velocidadeV0, double dt, int numeroRepeticoes) {
        this.parMass = parMass;
        this.L1 = posicaox0;
        this.posicaox0 = 0.71751;
        this.posicaoy0 = posicaoy0;
        this.dt = dt;
        this.numeroRepeticoes = numeroRepeticoes;
        this.posicaox = this.posicaox0;
        this.posicaoy = this.posicaoy0;
        this.velocidadeU = velocidadeU0;
        this.velocidadeV = velocidadeV0;
        this.materialEnergetico = new ArrayList();
    }

	public void generateParticlesPath() {
        Point point;
        point = new Point((float) this.posicaox0, (float) this.posicaoy0);
		materialEnergetico.Add(point);

		for (int i = 1; i <= this.numeroRepeticoes; i++) {

			this.dt = Math.Sqrt(posicaox * posicaox + posicaoy * posicaoy) * 0.1 / posicaox0 / 10;

            double posicaox1 = (this.posicaox + 0.5 * this.dt * this.velocidadeU);
            double posicaoy1 = (this.posicaoy + 0.5 * this.dt * this.velocidadeV);
            this.effes(this.posicaox, this.posicaoy, this.velocidadeU, this.velocidadeV, this.parMass);
            double velocidadeu1 = (this.velocidadeU + 0.5 * this.dt * this.fu);
            double velocidadev1 = (this.velocidadeV + 0.5 * this.dt * this.fv);
            this.posicaox = (this.posicaox + this.dt * velocidadeu1);
            this.posicaoy = (this.posicaoy + this.dt * velocidadev1);
            this.effes(posicaox1, posicaoy1, velocidadeu1, velocidadev1, this.parMass);
            this.velocidadeU = (this.velocidadeU + this.dt * this.fu);
            this.velocidadeV = (this.velocidadeV + this.dt * this.fv);

            point = new Point((float) (this.posicaox * this.L1 / this.posicaox0), (float) (this.posicaoy * this.L1 / this.posicaox0));
			materialEnergetico.Add(point);
		}
	}

    public void generateParticlesPathRK()
    {
        Point point;
        point = new Point((float)this.posicaox0, (float)this.posicaoy0);
        materialEnergetico.Add(point);

        for (int i = 1; i <= this.numeroRepeticoes; i++)
        {
            this.dt = Math.Sqrt(this.posicaox * this.posicaox + this.posicaoy * this.posicaoy)
                    * 0.1 / this.posicaox0 / 10;

            double posicaox1 = (this.posicaox + 0.5 * this.dt * this.velocidadeU);
            double posicaoy1 = (this.posicaoy + 0.5 * this.dt * this.velocidadeV);
            this.effes(this.posicaox, this.posicaoy, this.velocidadeU, this.velocidadeV, this.parMass);
            double ku1 = dt * fu;
            double kv1 = dt * fv;
            this.effes(posicaox1, posicaoy1, this.velocidadeU + ku1 / 2, this.velocidadeV + kv1 / 2, this.parMass);
            double ku2 = dt * fu;
            double kv2 = dt * fv;
            this.effes(posicaox1, posicaoy1, this.velocidadeU + ku2 / 2, this.velocidadeV + kv2 / 2, this.parMass);
            double ku3 = dt * fu;
            double kv3 = dt * fv;
            this.effes(this.posicaox + dt * this.velocidadeU, this.posicaoy + dt * velocidadeV, this.velocidadeU + ku3, this.velocidadeV + kv3 / 2, this.parMass);
            double ku4 = dt * fu;
            double kv4 = dt * fv;

            double velocidadeu1 = this.velocidadeU + (ku1 + 2 * ku2 + 2 * ku3 + ku4) / 6;
            double velocidadev1 = this.velocidadeV + (kv1 + 2 * kv2 + 2 * kv3 + kv4) / 6;

            this.posicaox = this.posicaox + dt * velocidadeu1;
            this.posicaoy = this.posicaoy + dt * velocidadev1;

            this.effes(posicaox1, posicaoy1, velocidadeu1, velocidadev1, this.parMass);

            this.velocidadeU = this.velocidadeU + dt * fu;
            this.velocidadeV = this.velocidadeV + dt * fv;

            point = new Point((float)(this.posicaox * this.L1 / this.posicaox0), (float)(this.posicaoy * this.L1 / this.posicaox0));
            materialEnergetico.Add(point);
        }
    }

    public void effes(double x, double y, double u, double v, double mu) {
		double raio1, raio2;

		raio1 = Math.Sqrt((x - mu) * (x - mu) + y * y);
		raio2 = Math.Sqrt((x + 1 - mu) * (x + 1 - mu) + y * y);
		this.fu = -(1 - mu) * (x - mu) / Math.Pow(raio1, 3) - mu * (x + 1 - mu) / Math.Pow(raio2, 3) + x + 2 * v;
		this.fv = -(1 - mu) * y / Math.Pow(raio1, 3) - mu * y / Math.Pow(raio2, 3) + y - 2 * u;

	}

	public double getDt() {
		return dt;
	}

	public void setDt(double dt) {
		this.dt = dt;
	}

	public double getFu() {
		return fu;
	}

	public void setFu(double fu) {
		this.fu = fu;
	}

	public double getFv() {
		return fv;
	}

	public void setFv(double fv) {
		this.fv = fv;
	}

	public int getNumeroRepeticoes() {
		return numeroRepeticoes;
	}

	public void setNumeroRepeticoes(int numeroRepeticoes) {
		this.numeroRepeticoes = numeroRepeticoes;
	}

	public double getParMass() {
		return parMass;
	}

	public void setParMass(double parMass) {
		this.parMass = parMass;
	}

	public double getPosicaox() {
		return posicaox;
	}

	public void setPosicaox(double posicaox) {
		this.posicaox = posicaox;
	}

	public double getPosicaox0() {
		return posicaox0;
	}

	public void setPosicaox0(double posicaox0) {
		this.posicaox0 = posicaox0;
	}

	public double getPosicaoy() {
		return posicaoy;
	}

	public void setPosicaoy(double posicaoy) {
		this.posicaoy = posicaoy;
	}

	public double getPosicaoy0() {
		return posicaoy0;
	}

	public void setPosicaoy0(double posicaoy0) {
		this.posicaoy0 = posicaoy0;
	}

	public double getVelocidadeU() {
		return velocidadeU;
	}

	public void setVelocidadeU(double velocidadeU) {
		this.velocidadeU = velocidadeU;
	}

	public double getVelocidadeV() {
		return velocidadeV;
	}

	public void setVelocidadeV(double velocidadeV) {
		this.velocidadeV = velocidadeV;
	}

	public ArrayList getMaterialEnergetico() {
		return materialEnergetico;
	}

	public void setMaterialEnergetico(ArrayList materialEnergetico) {
		this.materialEnergetico = materialEnergetico;
	}
}
