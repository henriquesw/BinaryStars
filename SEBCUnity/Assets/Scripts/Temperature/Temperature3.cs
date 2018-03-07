using System;

public class Temperature3 : Temperature {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	protected double TDOUT, UDK, NRDK, NADK, RIN, ROUT, FACTOR, DRDK, DRDK2, DADK, R1, UDK3;
	protected double [] RDK, TDK, SN1, ZW;
	protected double [,] XDK, YDK, DT22;

	public Temperature3(double r01, double T1P, double T2P, double U1, double U2, double AB1, double AB2, double tdout, double udk, double BETA, int NTH1, int NPH1, int NTH2, int NPH2, double NRDK, double NADK, double RIN, double ROUT, double FACTOR) :
		base() {

		this.primary.setR0(r01);
		this.primary.setTP(T1P);
		this.secondary.setTP(T2P);
		this.TDOUT = tdout;
		this.primary.setU(U1);
		this.secondary.setU(U2);
		this.primary.setAB(AB1);
		this.secondary.setAB(AB2);
		this.UDK = udk;
		this.BETA = BETA;
		this.primary.setNTH(NTH1);
		this.primary.setNPH(NPH1);
		this.secondary.setNTH(NTH2);
		this.secondary.setNPH(NPH2);
		this.NRDK = NRDK;
		this.NADK = NADK;
		this.RIN = RIN;
		this.ROUT = ROUT;
		this.FACTOR = FACTOR;

		this.initializeVectorandMatrix();
	}

	public Temperature3(TemperatureStar star1, TemperatureStar star2, double tdout, double udk, double BETA, double NRDK, double NADK, double RIN, double ROUT, double FACTOR) :
		base() {

		this.primary = star1;
		this.secondary = star2;
		this.UDK = udk;
		this.BETA = BETA;
		this.NRDK = NRDK;
		this.NADK = NADK;
		this.RIN = RIN;
		this.ROUT = ROUT;
		this.FACTOR = FACTOR;

		this.initializeVectorandMatrix();
	}

	public override void runTemperature() {
		this.loadConstants();
		this.meshTheDisk();
		this.meshThePrimaryStar();
		this.meshTheSecondaryStar();
		this.calculateTemperatureDistribuion();
		this.calculateFinalTemeperature();
	}
		
	protected override void loadConstants() {
		DRDK = (ROUT - RIN) / (float) (NRDK);
		DRDK2 = DRDK / 2.0;
		DADK = 2.0 * Math.PI / (float) (NADK);
		this.primary.setDTH(Math.PI / (float) (this.primary.getNTH() - 1));
		base.loadConstants();
	}

	protected void meshTheDisk() {
		int i, j;
		double AADK;
		//DO 10 I = 1, NRDK
		for (i = 1; i <= this.NRDK; i++) {
			RDK[i] = RIN + DRDK * (float) (i - 1) - DRDK2;
			this.TDK[i] = this.diskt(this.RDK[i]);
			//DO 10 J = 1, NADK
			for (j = 1; j <= this.NADK; j++) {
				AADK = DADK * (float) (j - 1);
				XDK[i,j] = RDK[i] * Math.Cos(AADK);
				YDK[i,j] = RDK[i] * Math.Sin(AADK);
			}
		}
	}

	protected override void meshTheSecondaryStar() {
		int i, j;
		double THE2, STH2;
		this.loadSecondaryStarValues();//primeira bloco da malha da secundaria extraido em refatoração!
		//DO 30 I = 1, NTH22
		for (i = 1; i <= this.NTH22; i++) {
			N = 1.0 - this.secondary.getDTH() * (float) (i - 1);
			THE2 = Math.Acos(N);
			STH2 = Math.Sin(THE2);
			//DO 30 J = 1, NPH2
			for (j = 1; j <= this.secondary.getNPH(); j++) {
				PHA = this.secondary.getDPH() * (float) (j - 1);
				this.secondary.L[i,j] = STH2 * Math.Cos(PHA);
				M = STH2 * Math.Sin(PHA);
				XRL[i,j] = this.secondary.R[i,j] * this.secondary.L[i,j];
				YRL[i,j] = this.secondary.R[i,j] * M;
				ZRL[i,j] = this.secondary.R[i,j] * N;
				this.secondary.T[i,j] = this.secondary.getTP4() * Math.Pow((this.secondary.G[i,j] / this.secondary.G[1,1]), BETA4);

			}
		}
	}

	private void loadSecondaryStarValues() {
		/*
         Metodo extraido do meshTheSecondaryStar()
         Esse metodo corresponde ao primeiro bloco do trecho que 
         gera a malha da secundária.
         */
		int i, j, k = 0;
		//DO 25 I = 1, NTH2
		for (i = 1; i <= this.secondary.getNTH(); i++) {
			//DO 25 J =1, NPH2
			for (j = 1; j <= this.secondary.getNPH(); j++) {
				this.secondary.R[i,j] = this.secondary.getPontos().R[k];
				this.secondary.G[i,j] = this.secondary.getPontos().G[k];
				this.secondary.SL[i,j] = this.secondary.getPontos().x[k];
				this.secondary.SM[i,j] = this.secondary.getPontos().y[k];
				this.secondary.SN[i,j] = this.secondary.getPontos().z[k];
				k++;
			}
		}
		//Console.WriteLine(k + "---" +this.secondary.getPontos().x.size());
	}

	protected override void meshThePrimaryStar() {
		int i, j;
		double ATH;

		R1 = this.primary.getR0();
		//DO 20 I = 1, NTH12
		for (i = 1; i <= this.NTH12; i++) {
			ATH = this.primary.getDTH() * (float) (i - 1);
			this.primary.STH[i] = Math.Sin(ATH);
			this.SN1[i] = Math.Cos(ATH);
			this.ZW[i] = R1 * SN1[i];
			//DO 20 J = 1, NPH1
			for (j = 1; j <= this.primary.getNPH(); j++) {
				APH = this.primary.getDPH() * (float) (j - 1);
				this.primary.SL[i,j] = this.primary.STH[i] * Math.Cos(APH);
				this.primary.SM[i,j] = this.primary.STH[i] * Math.Sin(APH);
				XW[i,j] = R1 * this.primary.SL[i,j];
				YW[i,j] = R1 * this.primary.SM[i,j];
			}
		}
	}

	protected override void secondaryTemperatureDistribution() {
		int i, j, ik, jk;
		double DTRLW, DTRLD, XDX, YDY;
		double WRLaux;
		//DO 50 I = 1, NTH22
		for (i = 1; i <= this.NTH22; i++) {
			//DO 50 J =1, NPH22
			for (j = 1; j <= this.NPH22; j++) {
				DTRLW = 0.0;
				//DO 51 IK = 1, NTH12
				for (ik = 1; ik <= this.NTH12; ik++) {
					ZWZ = ZW[ik] - ZRL[i,j];
					//DO 51 JK= 1, NPH1
					for (jk = 1; jk <= this.primary.getNPH(); jk++) {
						XWX = 1.0 - XW[ik,jk] - XRL[i,j];
						YWY = -YW[ik,jk] - YRL[i,j];
						this.check(XWX, YWY, ZWZ);
						PP = this.secondary.SL[i,j] * WRL + this.secondary.SM[i,j] * WRM + this.secondary.SN[i,j] * WRN;
						if (PP < 0.0) {
							//break;
							continue;
						}
						QQ = this.primary.SL[ik,jk] * WRL + this.primary.SM[ik,jk] * WRM - SN1[ik] * WRN;
						if (QQ < 0.0) {
							//break;
							continue;
						}
						DTR = (1.0 - this.primary.getU() + this.primary.getU() * QQ) * QQ * PP * this.primary.STH[ik] * R1 * R1 * D1TP * RWR * RWR * this.primary.getTP4();
						DTRLW = DTRLW + this.secondary.getAB() * DTR;
					}
					//                    if (PP < 0.0 || QQ < 0.0) {
					//                        break;
					//                    }
				}

				WRLaux = WRL;
				DTRLD = 0.0;
				//DO 52 IK = 1, NRDK
				for (ik = 1; ik <= this.NRDK; ik++) {
					//Do 52 JK= 1, NADK
					for (jk = 1; jk <= this.NADK; jk++) {
						XDX = 1.0 - XDK[ik,jk] - XRL[i,j];
						YDY = -YDK[ik,jk] - YRL[i,j];
						//Nesse trecho eu substitui as váriaveis originais da coluna da esquerda pelas ja existentes na coluna da direita.
						//Portanto, a partir daqui, onde for encontrado os nomes da direita, no código original será encontrado os nomes da esquerda
						//RDR = RWR
						//DRL = WRL
						//DRM = WRM
						//DRN = WRN
						this.check(XDX, YDY, -ZRL[i,j]);
						PP = this.secondary.SL[i,j] * WRL + this.secondary.SM[i,j] * WRM + this.secondary.SN[i,j] * WRN;
						if (PP < 0.0) {
							//break;
							continue;
						}
						DTR = -(1.0 - UDK - UDK * WRN) * WRN * PP * RDK[ik] * RWR * RWR * DRDK * DADK * Math.Pow(TDK[ik], 4);//linha 153
						DTRLD = DTRLD + this.secondary.getAB() * DTR;
					}
				}
				DT22[i,j] = WRLaux * DTRLW / Math.PI * U13 + WRL * DTRLD / Math.PI * UDK3;
			}
		}
	}

	protected override void calculateTemperatureDistribuion() {
		U13 = 3.0 / (3.0 - this.primary.getU());
		UDK3 = 3.0 / (3.0 - UDK);
		D1TP = this.primary.getDTH() * this.primary.getDPH();
		this.secondaryTemperatureDistribution();

	}

	private double diskt(double r) {
		double t, rrout;
		rrout = r / this.ROUT;
		t = Math.Pow(this.TDOUT * rrout, this.FACTOR);
		return t;
	}

	private void initializeVectorandMatrix() {
		this.RDK = new double[52];
		this.TDK = new double[52];
		this.XDK = new double[51,52];
		this.YDK = new double[51,52];
		this.SN1 = new double[52];
		this.ZW = new double[52];
		this.DT22 = new double[51,52];
	}

	protected override void printSecondaryResults() {
		int i, j;
		double T2F;
		Console.WriteLine("Temperature3: Secundaria \n");
		//DO 60 I = 1, NTH22
		for (i = 1; i <= this.NTH22; i++) {
			//DO 60 J =1, NPH22
			for (j = 1; j <= this.NPH22; j++) {
				if (this.secondary.L[i,j] < 0.0) {
					T2F = Math.Pow(this.secondary.T[i,j], 0.25);
				} else {
					ALPHA = Math.Asin(this.secondary.L[i,j]);
					MN = (int) Math.Round(ALPHA / this.secondary.getDTH());
					T2F = Math.Pow((this.secondary.T[i,j] + DT22[i,j]), 0.25);
				}
				Console.WriteLine(T2F);
			}
		}
		//Console.WriteLine("Total de valores: "+n);
	}

	protected override void calculateSecondaryFinalTemperature() {
		int i, j;
		double T2F;
		Console.WriteLine("Temperature3: Secundaria \n");
		//DO 60 I = 1, NTH22
		for (i = 1; i <= this.NTH22; i++) {
			//DO 60 J =1, NPH22
			for (j = 1; j <= this.NPH22; j++) {
				if (this.secondary.L[i,j] < 0.0) {
					T2F = Math.Pow(this.secondary.T[i,j], 0.25);
				} else {
					ALPHA = Math.Asin(this.secondary.L[i,j]);
					MN = (int) Math.Round(ALPHA / this.secondary.getDTH());
					T2F = Math.Pow((this.secondary.T[i,j] + DT22[i,j]), 0.25);
				}
				//Console.WriteLine(T2F);
				this.secondary.TF.Add(T2F);
			}
		}
	}
}
