using UnityEngine;
using System.Collections;
using System;

public class Temperature
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    protected double[,] XRL, YRL, ZRL, XW, YW, ZW; //L1[][], L2[][];
    protected double[] T11;
    protected double M, N, IA, NTH12, NTH22, NPH12, NPH22, BETA4, PHA, APH, U13, U23, DTRL,
    D1TP, D2TP, XWX, YWY, ZWZ, RWR, WRL, WRM, WRN, AL, AM, AN, PP, QQ, DTR, DTR1, DTR2, ALPHA;
    protected double ANGU, BETA, BASE;
    protected int MN;
    public TemperatureStar primary, secondary;

    public Temperature(double angu, double r01, double q2, double tp1, double tp2, double coef1, double coef2, double ab2, double beta, double Base)
    {

        this.primary = new TemperatureStar(0.87, 0.7);
        this.secondary = new TemperatureStar(0.87, 0.676);
        this.initializeVectorandMatrix();
        this.ANGU = angu;
        this.primary.setR0(r01);
        this.secondary.setQ(q2);
        this.primary.setTP(tp1);
        this.secondary.setTP(tp2);
        this.primary.setCOEF(coef1);
        this.secondary.setCOEF(coef2);
        this.secondary.setAB(ab2);
        this.BETA = beta;
        this.BASE = Base;
    }

    public Temperature()
    {
        this.primary = new TemperatureStar();
        this.secondary = new TemperatureStar();
        this.initializeVectorandMatrix();
    }

    public virtual void initializeVectorandMatrix()
    {
        XRL = new double[51, 52];
        YRL = new double[51, 52];
        ZRL = new double[51, 52];
        //T22 = new double[100];
        T11 = new double[51];
        XW = new double[51, 52];
        YW = new double[51, 52];
        ZW = new double[51, 52];
    }

    public void setPrimaryPoints(PointsCollection p)
    {
        this.primary.setPontos(p);
    }

    public void setSecondaryPoints(PointsCollection p)
    {
        this.secondary.setPontos(p);
    }

    public virtual void runTemperature()
    {
    }

    protected virtual void loadConstants()
    {
        this.primary.setDPH(2.0 * Math.PI / (float)(this.primary.getNPH() - 1));
        this.secondary.setDTH(2.0 / (float)(this.secondary.getNTH() - 1));
        this.secondary.setDPH(2.0 * Math.PI / (float)(this.secondary.getNPH() - 1));
        NTH12 = (this.primary.getNTH() + 1) / 2;
        NTH22 = (this.secondary.getNTH() + 1) / 2;
        NPH12 = (this.primary.getNPH() + 1) / 2;
        NPH22 = (this.secondary.getNPH() + 1) / 2;
        this.secondary.setTP4(Math.Pow(this.secondary.getTP(), 4));
        this.primary.setTP4(Math.Pow(this.primary.getTP(), 4));
        BETA4 = 4.0 * BETA;
    }

    protected virtual void meshTheSecondaryStar()
    {
        int i, j, k = 0;
        for (i = 1; i <= this.secondary.getNTH(); i++)
        {
            N = 1.0 - this.secondary.getDTH() * (float)(i - 1);
            this.secondary.THE[i] = Math.Acos(N);
            this.secondary.STH[i] = Math.Sin(this.secondary.THE[i]);
            for (j = 1; j <= this.secondary.getNPH(); j++)
            {
                PHA = this.secondary.getDPH() * (float)(j - 1);
                this.secondary.L[i, j] = this.secondary.STH[i] * Math.Cos(PHA);
                M = this.secondary.STH[i] * Math.Sin(PHA);

                //READ(4, *) R2(I, J), G2(I, -J), SL2(I, J), SM2(I, J), SN2(I, J)
                this.secondary.R[i, j] = this.secondary.getPontos().R[k];
                this.secondary.G[i, j] = this.secondary.getPontos().G[k];
                this.secondary.SL[i, j] = this.secondary.getPontos().x[k];
                this.secondary.SM[i, j] = this.secondary.getPontos().y[k];
                this.secondary.SN[i, j] = this.secondary.getPontos().z[k];
                k++;

                XRL[i, j] = this.secondary.R[i, j] * this.secondary.L[i, j];
                YRL[i, j] = this.secondary.R[i, j] * M;
                ZRL[i, j] = this.secondary.R[i, j] * N;
                this.secondary.T[i, j] = this.secondary.getTP4() * Math.Pow((this.secondary.G[i, j] / this.secondary.G[1, 1]), BETA4);

            }
        }
    }

    protected virtual void meshThePrimaryStar()
    {
        int i, j, k = 0;
        for (i = 1; i <= this.primary.getNTH(); i++)
        {
            N = 1.0 - this.primary.getDTH() * (float)(i - 1);
            this.primary.THE[i] = Math.Acos(N);
            this.primary.STH[i] = Math.Sin(this.primary.THE[i]);
            for (j = 1; j <= this.primary.getNPH(); j++)
            {
                APH = this.primary.getDPH() * (float)(j - 1);
                this.primary.L[i, j] = this.primary.STH[i] * Math.Cos(APH);
                M = this.primary.STH[i] * Math.Sin(APH);

                //READ(4, *) R1(I, J), G1(I, J), SL1(I, J), SM1(I, J), SN1(I, J)
                this.primary.R[i, j] = this.primary.getPontos().R[k];
                this.primary.G[i, j] = this.primary.getPontos().G[k];
                this.primary.SL[i, j] = this.primary.getPontos().x[k];
                this.primary.SM[i, j] = this.primary.getPontos().y[k];
                this.primary.SN[i, j] = this.primary.getPontos().x[k];
                k++;

                XW[i, j] = this.primary.R[i, j] * this.primary.L[i, j];
                YW[i, j] = this.primary.R[i, j] * M;
                ZW[i, j] = this.primary.R[i, j] * N;
                this.primary.T[i, j] = this.primary.getTP4() * Math.Pow((this.primary.G[i, j] / this.primary.G[1, 1]), BETA4);
            }
        }
    }

    protected virtual void secondaryTemperatureDistribution()
    {
    }

    protected virtual void primaryTemperatureDistribution()
    {
    }

    protected virtual void calculateTemperatureDistribuion()
    {
    }

    //    protected void check(double x, double y, double z) {
    //        RWR = 1.0 / Math.Sqrt(x * x + y * y + z * z);
    //        AL = x * RWR;
    //        AM = y * RWR;
    //        AN = z * RWR;
    //    }

    protected void check(double x, double y, double z)
    {
        this.RWR = 1.0 / Math.Sqrt(x * x + y * y + z * z);
        this.WRL = x * this.RWR;
        this.WRM = y * this.RWR;
        this.WRN = z * this.RWR;
    }

    protected void calculateFinalTemeperature()
    {
        this.calculateSecondaryFinalTemperature();
        this.calculatePrimaryFinalTemperature();
    }

    protected virtual void calculateSecondaryFinalTemperature() { }
    protected virtual void calculatePrimaryFinalTemperature() { }

    protected virtual void printResults()
    {
        this.printSecondaryResults();
        this.printPrimaryResults();
    }

    protected virtual void printPrimaryResults() { }
    protected virtual void printSecondaryResults() { }

}
