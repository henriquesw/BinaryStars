using System;

public class Temperature2 : Temperature
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    protected double R1, T1;
    protected double[,] T22 = new double[100, 100];

    public Temperature2(double r01, double t1p, double t2p, double u1, double u2, double ab1, double ab2, double beta, int nth1, int nph1, int nth2, int nph2) :
        base()
    {
        this.primary.setR0(r01);
        this.primary.setTP(t1p);
        this.secondary.setTP(t2p);
        this.primary.setU(u1);
        this.secondary.setU(u2);
        this.primary.setAB(ab1);
        this.secondary.setAB(ab2);
        this.BETA = beta;
        this.primary.setNTH(nth1);
        this.primary.setNPH(nph1);
        this.secondary.setNTH(nth2);
        this.secondary.setNPH(nph2);

    }

    public Temperature2() :
        base(57, 0.00001, 0.5, 15000, 3200, -0.237, 1.016, 0.676, 0.08, 14.07)
    {
        //Temperature2 constructor
    }

    public Temperature2(double angu, double r01, double q2, double tp1, double tp2, double coef1, double coef2, double ab2, double beta, double Base) :
        base(angu, r01, q2, tp1, tp2, coef1, coef2, ab2, beta, Base)
    {
        //this.loadConstants();
    }

    protected override void loadConstants()
    {
        this.primary.setDTH(Math.PI / (float)(this.primary.getNTH() - 1));
        base.loadConstants();
    }


    public override void runTemperature()
    {
        this.loadConstants();
        base.meshTheSecondaryStar();
        this.meshThePrimaryStar();
        this.calculateTemperatureDistribuion();
        this.calculateFinalTemeperature();
    }


    protected override void meshThePrimaryStar()
    {
        int i, j;
        double ath, wn, wm;
        R1 = this.primary.getR0();
        T1 = this.primary.getTP4();
        //DO 20 I = 1, NTH1
        for (i = 1; i <= this.primary.getNTH(); i++)
        {
            ath = (this.primary.getDTH() * (float)(i - 1));
            this.primary.STH[i] = Math.Sin(ath);
            wn = Math.Cos(ath);
            //DO 20 J = 1, NPH1
            for (j = 1; j <= this.primary.getNPH(); j++)
            {
                APH = this.primary.getDPH() * (float)(j - 1);
                this.primary.L[i, j] = this.primary.STH[i] * Math.Cos(APH);
                wm = this.primary.STH[i] * Math.Sin(APH);
                this.primary.SL[i, j] = this.primary.L[i, j];
                this.primary.SM[i, j] = wm;
                this.primary.SN[i, j] = wn;
                this.XW[i, j] = R1 * this.primary.L[i, j];
                this.YW[i, j] = R1 * wm;
                this.ZW[i, j] = R1 * wn;
            }
        }
    }

    protected override void secondaryTemperatureDistribution()
    {
        int i, j, ik, jk;
        //DO 40 I = 1, NTH22
        for (i = 1; i <= this.NTH22; i++)
        {
            //DO 40 J =1, NPH22
            for (j = 1; j <= this.NPH22; j++)
            {
                DTRL = 0.0;
                //DO 41 IK = 1, NTH1
                for (ik = 1; ik <= this.primary.getNTH(); ik++)
                {
                    //DO 41 JK= 1, NPH1 
                    for (jk = 1; jk <= this.primary.getNPH(); jk++)
                    {
                        XWX = 1.0 - XW[ik, jk] - XRL[i, j];
                        YWY = -YW[ik, jk] - YRL[i, j];
                        ZWZ = ZW[ik, jk] - ZRL[i, j];
                        this.check(XWX, YWY, ZWZ);
                        PP = this.secondary.SL[i, j] * WRL + this.secondary.SM[i, j] * WRM + this.secondary.SN[i, j] * WRN;
                        if (PP < 0.0)
                        {
                            //break;
                            continue;
                        }
                        QQ = this.primary.SL[ik, jk] * WRL + this.primary.SM[ik, jk] * WRM - this.primary.SN[ik, jk] * WRN;
                        if (QQ < 0.0)
                        {
                            //break;
                            continue;
                        }
                        DTR = (1.0 - this.primary.getU() + this.primary.getU() * QQ) * QQ * PP * this.primary.STH[ik] * R1 * R1 * RWR * RWR * D1TP * T1;
                        DTRL = DTRL + DTR;
                    }
                    //                    if (PP < 0.0 || QQ < 0.0) {
                    //                        break;
                    //                    }
                }
                T22[i, j] = this.secondary.getAB() * DTRL / Math.PI * U13;
            }
        }
    }

    protected override void calculateTemperatureDistribuion()
    {
        U13 = 3.0 / (3.0 - this.primary.getU());
        U23 = 3.0 / (3.0 - this.secondary.getU());
        D1TP = this.primary.getDTH() * this.primary.getDPH();
        this.secondaryTemperatureDistribution();
    }

    protected override void printSecondaryResults()
    {
        int i, j;
        double T2F;
        Console.WriteLine("Temperature2: Secundaria \n");
        //DO 45 I = 1, NTH22
        for (i = 1; i < this.NTH22; i++)
        {
            //DO 45 J = 1, NPH22
            for (j = 1; j < this.NPH22; j++)
            {
                if (this.secondary.L[i, j] < 0.0)
                {
                    this.secondary.T[i, j] = this.secondary.T[i, j] + 0.0;
                }
                else
                {
                    ALPHA = Math.Asin(this.secondary.L[i, j]);
                    MN = (int)Math.Round(ALPHA / this.secondary.getDTH());
                    this.secondary.T[i, j] = this.secondary.T[i, j] + T22[MN, j];
                }
                T2F = Math.Pow(this.secondary.T[i, j], 0.25);
                Console.WriteLine(T2F);
            }
        }
    }

    protected override void calculateSecondaryFinalTemperature()
    {
        int i, j;
        double T2F;
        //Console.WriteLine("Temperature2: Secundaria \n");
        //DO 45 I = 1, NTH22
        for (i = 1; i <= this.NTH22; i++)
        {
            //DO 45 J = 1, NPH22
            for (j = 1; j <= this.NPH22; j++)
            {
                if (this.secondary.L[i, j] < 0.0)
                {
                    this.secondary.T[i, j] = this.secondary.T[i, j] + 0.0;
                }
                else
                {
                    ALPHA = Math.Asin(this.secondary.L[i, j]);
                    MN = (int)Math.Round(ALPHA / this.secondary.getDTH());
                    this.secondary.T[i, j] = this.secondary.T[i, j] + T22[MN, j];
                }
                T2F = Math.Pow(this.secondary.T[i, j], 0.25);
                //Console.WriteLine(T2F);
                this.secondary.TF.Add(T2F);
            }
        }
    }
}
