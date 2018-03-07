using System;

public class Temperature1 : Temperature
{

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	private double CP;
	protected double[] T22 = new double[100];
	//int II, JJ;

	public Temperature1 (double angu, double t1p, double t2p, double u1, double u2, double ab1, double ab2, double beta, int nth1, int nph1, int nth2, int nph2) :
		base ()	{
		this.ANGU = angu;
		this.primary.setTP (t1p);
		this.secondary.setTP (t2p);
		this.primary.setU (u1);
		this.secondary.setU (u2);
		this.primary.setAB (ab1);
		this.secondary.setAB (ab2);
		this.BETA = beta;
		this.primary.setNTH (nth1);
		this.primary.setNPH (nph1);
		this.secondary.setNTH (nth2);
		this.secondary.setNPH (nph2);

	}

	public override void runTemperature ()
	{
		this.loadConstants ();
		this.meshTheSecondaryStar ();
		this.meshThePrimaryStar ();
		this.calculateTemperatureDistribuion ();
		this.calculateFinalTemeperature ();
	}

	protected override void loadConstants ()
	{
		this.primary.setDTH (2.0 / (float)(this.primary.getNTH () - 1));
		base.loadConstants ();
	}

	protected override void calculateTemperatureDistribuion ()
	{
		//OBs.: Precisa verificar a inicialização da variável U!!!
		CP = 1.5;
		U13 = 3.0 / (3.0 - this.primary.getU ());
		U23 = 3.0 / (3.0 - this.secondary.getU ());
		while (CP < 3.0) {
			this.secondaryTemperatureDistribution ();
			this.primaryTemperatureDistribution ();
			CP = CP + 1.0;
		}
	}

	protected override void secondaryTemperatureDistribution ()
	{
		/*
         Precisa refatorar esse metodo
         Verificar os indices
         */
		int i, j, IK, JK;
		int II, JJ;
		for (i = 1; i <= this.secondary.getNTH (); i++) {
			DTRL = 0.0;
			for (IK = 1; IK <= this.primary.getNTH (); IK++) {
				D1TP = 0.5 * (this.primary.THE [IK + 1] - this.primary.THE [IK - 1]) * this.primary.getDPH ();
				for (JK = 1; JK <= this.primary.getNPH (); JK++) {
					XWX = 1.0 - XW [IK, JK] - XRL [i, 1];
					YWY = -YW [IK, JK] - YRL [i, 1];
					ZWZ = ZW [IK, JK] - ZRL [i, 1];
					this.check (XWX, YWY, ZWZ);
					PP = this.secondary.SL [i, 1] * WRL + this.secondary.SM [i, 1] * WRM + this.secondary.SN [i, 1] * WRN;
					if (PP < 0.0) {
						//break;
						continue;
					}
					QQ = this.primary.SL [IK, JK] * WRL + this.primary.SM [IK, JK] * WRM - this.primary.SN [IK, JK] * WRN;
					if (QQ < 0.0) {
						//break;
						continue;
					}
					DTR = (1.0 - this.primary.getU () + this.primary.getU () * QQ) * QQ * PP * this.primary.STH [IK] * this.primary.R [IK, JK] * this.primary.R [IK, JK];
					DTR1 = DTR * RWR * RWR * D1TP * this.primary.T [IK, JK];
					DTRL = DTRL + DTR1;
				}
				//                if (QQ < 0.0 || PP < 0.0) {
				//                    break;
				//                }
			}
			T22 [i] = this.secondary.getAB () * DTRL / Math.PI * U13;
		}

		for (i = 1; i <= this.NTH22; i++) {
			II = (int)(this.secondary.getNTH () + 1 - i);//(int) (this.secondary.getNTH() + 1 - i);
			//DO 45 J = 1, NPH22
			for (j = 1; j <= this.NPH22; j++) {
				JJ = (int)(this.secondary.getNPH () + 1 - j);//(int) (this.secondary.getNPH() + 1 - j);
				if (this.secondary.L [i, j] < 0.0) {
					this.secondary.T [i, j] = this.secondary.T [i, j] + 0.0;
				} else {
					ALPHA = Math.Asin (this.secondary.L [i, j]);
					MN = (int)Math.Round (ALPHA / this.secondary.getDTH ());
					this.secondary.T [i, j] = this.secondary.T [i, j] + T22 [MN];
				}
				this.secondary.T [II, j] = this.secondary.T [i, j];
				this.secondary.T [i, JJ] = this.secondary.T [i, j];
				this.secondary.T [II, JJ] = this.secondary.T [i, j];
			}

		}

	}

	protected override void primaryTemperatureDistribution ()
	{
		int i, j, IK, JK;
		int II, JJ;
		double DTWD, DWT, DWT1;
		//DO 50 I = 1, NTH12
		for (i = 1; i <= NTH12; i++) {
			DTWD = 0.0;
			//DO 51 IK = 1, NTH2
			for (IK = 1; IK <= this.secondary.getNTH (); IK++) {
				D2TP = 0.5 * (this.secondary.THE [IK + 1] - this.secondary.THE [IK - 1]) * this.secondary.getDPH ();
				//DO 51 JK = 1, NPH2
				for (JK = 1; JK <= this.secondary.getNPH (); JK++) {
					XWX = 1.0 - XRL [IK, JK] - XW [i, 1];
					YWY = -YRL [IK, JK] - YW [i, 1];
					ZWZ = ZRL [IK, JK] - ZW [i, 1];
					this.check (XWX, YWY, ZWZ);
					PP = this.primary.SL [i, 1] * WRL + this.primary.SM [i, 1] * WRM + this.primary.SN [i, 1] * WRN;
					if (PP < 0.0) {
						//break;
						continue;
					}
					QQ = this.secondary.SL [IK, JK] * WRL + this.secondary.SM [IK, JK] * WRM - this.secondary.SN [IK, JK] * WRN;
					if (QQ < 0.0) {
						//break;
						continue;
					}
					DWT = (1.0 - this.secondary.getU () + this.secondary.getU () * QQ) * QQ * PP * this.secondary.STH [IK] * RWR * RWR;
					DWT1 = DWT * this.secondary.T [IK, JK] * this.secondary.R [IK, JK] * this.secondary.R [IK, JK] * D2TP;
					//DTWD = DTWD + DTW1;  A expressão original da linha abaixo é essa, mas a váriavel DTW1 não existe em lugar nenhum, então eu supus que
					//fosse a DWT1!!!
					DTWD = DTWD + DWT1;
				}
				//                if (QQ < 0.0 || PP < 0.0) {
				//                    break;
				//                }
			}
			T11 [i] = this.primary.getAB () * DTWD / Math.PI * U23;
		}
		//DO 55 I = 1, NTH12 
		for (i = 1; i <= NTH12; i++) {
			II = (int)(this.primary.getNTH () + 1 - i);
			//DO 55 J = 1, NPH12
			for (j = 1; j <= NPH12; j++) {
				JJ = (int)(this.primary.getNPH () + 1 - j);
				if (this.primary.L [i, j] < 0.0) {
					this.primary.T [i, j] = this.primary.T [i, j] + 0.0;
				} else {
					ALPHA = Math.Asin (this.primary.L [i, j]);
					MN = (int)Math.Round (ALPHA / this.primary.getDTH ());
					this.primary.T [i, j] = this.primary.T [i, j] + T11 [MN];
				}
				this.primary.T [II, j] = this.primary.T [i, j];
				this.primary.T [i, JJ] = this.primary.T [i, j];
				this.primary.T [II, JJ] = this.primary.T [i, j];
			}
		}

	}

	protected override void printResults ()
	{
		//this.printSecondaryResults();
		//this.printPrimaryResults();
	}

	protected override void printSecondaryResults ()
	{
		int i, j;
		double T2F;
		Console.WriteLine ("Temperature1: Secundaria \n");
		//DO 60 I = 1, NTH22
		for (i = 1; i <= NTH22; i++) {
			//DO 60 J = 1, NPH22
			for (j = 1; j <= NPH22; j++) {
				T2F = Math.Pow (this.secondary.T [i, j], 0.25);
				Console.WriteLine (T2F);
			}
		}
	}

	protected override void printPrimaryResults ()
	{
		int i, j;
		double T1F;
		Console.WriteLine ("Temperature1: Primaria \n");
		//DO 70 I = 1, NTH12
		for (i = 1; i <= NTH12; i++) {
			//DO 70 J = 1, NPH12
			for (j = 1; j <= NTH12; j++) {
				T1F = Math.Pow (this.primary.T [i, j], 0.25);
				Console.WriteLine (T1F);
			}
		}
	}

	//    @Override
	//    protected void calculateSecondaryFinalTemperature() {
	//        int i, j;
	//        double T2F;
	//        //DO 60 I = 1, NTH22
	//        for (i = 1; i <= 51; i++) {
	//            //DO 60 J = 1, NPH22
	//            for (j = 1; j <= 51; j++) {
	//                T2F = Math.Pow(this.secondary.T[i,j], 0.25);
	//                //Console.WriteLine(T2F);
	//                this.secondary.TF.Add(T2F);
	//            }
	//        }
	//    }

	protected override void calculateSecondaryFinalTemperature ()
	{
		int i, j;
		double T2F;
		//DO 60 I = 1, NTH22
		for (i = 1; i <= NTH22; i++) {
			//DO 60 J = 1, NPH22
			for (j = 1; j <= NPH22; j++) {
				T2F = Math.Pow (this.secondary.T [i, j], 0.25);
				//Console.WriteLine(T2F);
				this.secondary.TF.Add (T2F);
			}
		}
	}

	//    @Override
	//    protected void calculatePrimaryFinalTemperature() {
	//        int i, j;
	//        double T1F;
	//        //DO 70 I = 1, NTH12
	//        for (i = 1; i <= NTH12; i++) {
	//            //DO 70 J = 1, NPH12
	//            for (j = 1; j <= NTH12; j++) {
	//                T1F = Math.Pow(this.primary.T[i,j], 0.25);
	//                //Console.WriteLine(T1F);
	//                this.primary.TF.Add(T1F);
	//            }
	//        }
	//    }

	protected override void calculatePrimaryFinalTemperature ()
	{
		int i, j;
		double T1F;
		//DO 70 I = 1, NTH12
		for (i = 1; i <= NTH12; i++) {
			//DO 70 J = 1, NPH12
			for (j = 1; j <= NPH12; j++) {
				T1F = Math.Pow (this.primary.T [i, j], 0.25);
				//Console.WriteLine(T1F);
				this.primary.TF.Add (T1F);
			}
		}
	}
}
