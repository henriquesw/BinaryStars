using System.Collections;
using System;

public class LightCurve
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    protected static int NRDK = 31, NADK = 51, NRSP = NRDK + 1, kControle;
    protected double PI2, UWD3, URL3, UDK3, UDK, FS, ANGU, BETA, BASE, CONSTANTE,
    SINIA, SN0, IA, RMAXR, RMAXW, C, D1TP, SL0, SM0, COSB, DDTHETA, SR0,
    DSR0, DSL0, ROCHE, SINP, COSP, X, Y, Z, S, DWS, SWP, XX, YY, XP, YP,
    maxER2, minER2, DRDK, DADK, BRDK, NASP1 = 31, NASP2 = 30, BRSP, BRIT;
    protected int NTH12, NTH22, NPH12, NPH22, NPH21, LL;
    protected LightCurveStar primary, secondary;
    protected ArrayList PLAMDA, CLAMDA, SCOLOR, CPS;
    public ArrayList PHASP, Brit, swp, roche, brdk, brsp;
    protected double[,] D, BDK;
    protected double[] ER2, EA2, RDK, ADK;
    protected int KFLAG, KCOLOR, ICOLOR;
    //protected Disk disk;
    // FILTER RESPONSE FUNCTIONS /
    protected static double[] SU = { .025, .250, .680, 1.137, 1.650, 2.006, 2.250, 2.337, 1.925, .650, .197, .070 },
    SB = { .006, .080, .337, 1.425, 2.253, 2.806, 2.950, 3.000, 2.937, 2.780, 2.52, 2.23, 1.881, 1.550, 1.275, .975, .695, .4400, .210, .055 },
    SV = { .02, .175, .90, 1.88, 2.1002, 2.85, 2.82, 2.625, 2.370, 2.05, 1.72, 1.143, 1.068, .759, .567, .387, .25, .160, .11, .081, .061, .045, .028, .017, .007 },
    SR = { .06, .28, .50, .69, .79, .88, .94, .98, 1.0, .94, .85, .7400, .57, .42, .31, .17, .11, .06, .04, .02, .01 },
    SJ = { .0001, .02, .11, .32, .42, .47, .63, .73, .77, .81, .83, .88, .94, .91, .79, .68, .04, .11, .07, .03, .0001 },
    SH = { .0001, .15, .44, .86, .94, .98, .95, .99, .99, .99, .99, .99, .99, .95, .87, .84, .71, .52, .02, .0001 },
    SK = { .0001, .12, .2, .3, .55, .74, .55, .77, .85, .9, .94, .94, .95, .94, .96, .98, .97, .96, .91, .88, .84, .75, .64, .1, .0001 };

    public LightCurve()
    {
        this.primary = new LightCurveStar();
        this.secondary = new LightCurveStar();
    }

    public LightCurve(int kflag, int kcolor, double angu, double r01, double q2, double tp1, double tp2, double coef1, double coef2, double ab2, double beta, double Base)
    {
        /*
         Inicializa as variáveis a partir dos valores fornecidos
         */
        primary = new LightCurveStar();
        secondary = new LightCurveStar();
        //this.disk = new Disk();
        KFLAG = kflag;
        KCOLOR = kcolor;
        ANGU = angu;
        //IA = Math.toRadians(ANGU);
        IA = ANGU;
        this.primary.setR0(r01);
        this.secondary.setQ(q2);
        this.primary.setTP(tp1);
        this.secondary.setTP(tp2);
        this.primary.setCOEF(coef1);
        this.secondary.setCOEF(coef2);
        this.secondary.setAB(ab2);
        BETA = beta;
        BASE = Base;
    }

    public virtual void runLightCurve()
    {
        this.initializeVectorandMatrix();
        this.loadConstants();
        this.calculateValuesForEachColor();

    }

    protected void loadConstants()
    {
        this.CONSTANTE = this.ANGU;

        this.primary.C = 3.74185E-5;
        this.secondary.C = 1.43883;
        PI2 = 1.0 / Math.PI;
        this.NTH12 = (this.primary.getNTH() + 1) / 2;
        this.NPH12 = (this.primary.getNPH() + 1) / 2;
        this.NTH22 = (this.secondary.getNTH() + 1) / 2;
        this.NPH22 = (this.secondary.getNPH() + 1) / 2;
        UWD3 = 3.0 / (3.0 - this.primary.getU());
        URL3 = 3.0 / (3.0 - this.secondary.getU());
        UDK3 = 3.0 / (3.0 - UDK);
    }

    protected virtual void initializeVectorandMatrix()
    {
        this.PLAMDA = new ArrayList();
        this.CLAMDA = new ArrayList();
        this.SCOLOR = new ArrayList();
        this.PHASP = new ArrayList();
        this.CPS = new ArrayList();
        this.D = new double[51, 52];
        this.ER2 = new double[100];
        this.EA2 = new double[100];
        this.RDK = new double[NRDK + 1];
        this.ADK = new double[NADK + 1];
        this.BDK = new double[NRDK + 1, NADK + 1];
        this.Brit = new ArrayList();
        this.swp = new ArrayList();
        this.roche = new ArrayList();
        this.brdk = new ArrayList();
        this.brsp = new ArrayList();
    }

    protected void calculateValuesForEachColor()
    {
        /*
         FOR EACH COLOR CALCULATE THE VALUES OF PLAMDA, SLAMDA AND
         SCOLOR, WHICH ARE USED TO CALCULATE THE RADIATION 
         */
        int i;
        switch (this.KCOLOR)
        {
            case 1:
                {
                    ICOLOR = 12;
                    FS = 1.0;
                    for (i = 0; i < this.ICOLOR; i++)
                    {
                        this.PLAMDA.Add(1.0 / (0.0029 + 0.000001 * (float)(i + 1)));
                        this.CLAMDA.Add(this.primary.C * Math.Pow((double)this.PLAMDA[i], 5));
                        this.SCOLOR.Add(LightCurve.SU[i]);
                    }
                    break;
                }
            case 2:
                {
                    ICOLOR = 20;
                    FS = 1.0;
                    for (i = 0; i < this.ICOLOR; i++)
                    {
                        this.PLAMDA.Add(1.0 / (0.000035 + 0.000001 * (float)(i + 1)));
                        this.CLAMDA.Add(this.primary.C * Math.Pow((double)this.PLAMDA[i], 5));//CLAMDA (I) = C1*PLAMDA(I)**5
                        this.SCOLOR.Add(LightCurve.SB[i]);//SCOLOR (I) = SB(I)
                    }
                    break;
                }
            case 3:
                {
                    ICOLOR = 25;
                    FS = 1.0;
                    for (i = 0; i < this.ICOLOR; i++)
                    {
                        this.PLAMDA.Add(1.0 / (0.000047 + 0.000001 * (float)(i + 1)));//PLAMDA (I) = 1.0 / (0.000047 + 0.000001*FLOAT(I))
                        this.CLAMDA.Add(this.primary.C * Math.Pow((double)this.PLAMDA[i], 5));//CLAMDA (I) = C1*PLAMDA(I)**5
                        this.SCOLOR.Add(LightCurve.SV[i]);//SCOLOR (I) = SV(I)
                    }
                    break;
                }
            case 4:
                {
                    ICOLOR = 21;
                    FS = 2.0;
                    for (i = 0; i < this.ICOLOR; i++)
                    {
                        this.PLAMDA.Add(1.0 / (0.000052 + 0.000002 * (float)(i + 1)));//PLAMDA (I) = 1.0/(0.000052 + 0.000002*FLOAT(I))
                        this.CLAMDA.Add(this.primary.C * Math.Pow((double)this.PLAMDA[i], 5));//CLAMDA (I) = C1*PLAMDA(I)**5
                        this.SCOLOR.Add(LightCurve.SR[i]);//SCOLOR (I) = SR(I)
                    }
                    break;
                }
            case 5:
                {
                    ICOLOR = 21;
                    FS = 2.0;
                    for (i = 0; i < this.ICOLOR; i++)
                    {
                        this.PLAMDA.Add(1.0 / (0.000104 + 0.000002 * (float)(i)));//PLAMDA (I) = 1.0/(0.000104 + 0.000002*FLOAT(I-1))
                        this.CLAMDA.Add(this.primary.C * Math.Pow((double)this.PLAMDA[i], 5));//CLAMDA (I) = C1*PLAMDA(I)**5
                        this.SCOLOR.Add(LightCurve.SJ[i]);//SCOLOR (I) = SJ(I)
                    }
                    break;
                }
            case 6:
                {
                    ICOLOR = 20;
                    FS = 2.0;
                    for (i = 0; i < this.ICOLOR; i++)
                    {
                        this.PLAMDA.Add(1.0 / (0.000146 + 0.000002 * (float)(i)));//PLAMDA (I) = 1.0/(0.000146 + 0.000002*FLOAT(I-1))
                        this.CLAMDA.Add(this.primary.C * Math.Pow((double)this.PLAMDA[i], 5));//CLAMDA (I) = C1*PLAMDA(I)**5
                        this.SCOLOR.Add(LightCurve.SH[i]);//SCOLOR (I) = SH(I)
                    }
                    break;
                }
            default:
                {
                    ICOLOR = 25;
                    FS = 2.0;
                    for (i = 0; i < this.ICOLOR; i++)
                    {
                        this.PLAMDA.Add(1.0 / (0.000194 + 0.000002 * (float)(i + 1)));//PLAMDA (I) = 1.0/(0.000194 + 0.000002*FLOAT(I))
                        this.CLAMDA.Add(this.primary.C * Math.Pow((double)this.PLAMDA[i], 5));//CLAMDA (I) = C1*PLAMDA(I)**5
                        this.SCOLOR.Add(LightCurve.SK[i]);//SCOLOR (I) = SK(I)
                    }
                    break;
                }
        }
    }

    public void setPrimaryPoints(PointsCollection p)
    {
        this.primary.setPontos(p);
    }

    public void setSecondaryPoints(PointsCollection p)
    {
        this.secondary.setPontos(p);
    }

    public void setPrimaryTemperature(ArrayList T)
    {
        this.primary.TF = T;
    }

    public void setSecondaryTemperature(ArrayList T)
    {
        this.secondary.TF = T;
    }

    protected void calculateSecondaryStarParameters()
    {
        int i, j, k = 0;
        this.secondary.setDTH(2.0 / (float)(this.secondary.getNTH() - 1));
        this.secondary.setDPH(2.0 * Math.PI / (float)(this.secondary.getNPH() - 1));
        //DO 10 I =1, NTH2
        for (i = 1; i <= this.secondary.getNTH(); i++)
        {
            this.secondary.N = 1.0 - this.secondary.getDTH() * (float)(i - 1);
            if (this.secondary.N <= -1.0)
            {
                this.secondary.N = -1.0;
            }
            this.secondary.THE[i] = Math.Acos(this.secondary.N);
            this.secondary.CTHE[i] = this.secondary.N;
            this.secondary.STHE[i] = Math.Sin(this.secondary.THE[i]);
            //DO 10 J=1, NPH2
            for (j = 1; j <= this.secondary.getNPH(); j++)
            {
                this.secondary.PHA[j] = this.secondary.getDPH() * (float)(j - 1);
                this.secondary.CPHA[j] = Math.Cos(this.secondary.PHA[j]);
                this.secondary.SPHA[j] = Math.Sin(this.secondary.PHA[j]);

                this.secondary.R[i, j] = this.secondary.getPontos().R[k];
                this.secondary.G[i, j] = this.secondary.getPontos().G[k];
                this.secondary.SL[i, j] = this.secondary.getPontos().x[k];
                this.secondary.SM[i, j] = this.secondary.getPontos().y[k];
                this.secondary.SN[i, j] = this.secondary.getPontos().z[k];
                k++;
            }
        }
    }

    protected virtual void calculatePrimaryStarParameters()
    {
        int i, j;
        //DO 20 I=1, NTH1
        for (i = 1; i <= primary.getNTH(); i++)
        {
            primary.ThE = primary.getDTH() * (float)(i - 1);
            primary.STHE[i] = Math.Sin(primary.ThE);
            primary.WN[i] = Math.Cos(primary.ThE);
            //DO 20 J=1, NPH1
            for (j = 1; j <= this.primary.getNPH(); j++)
            {
                this.primary.PhA = this.primary.getDPH() * (float)(j - 1);
                this.primary.WL[i, j] = this.primary.STHE[i] * Math.Cos(this.primary.PhA);
                this.primary.WM[i, j] = this.primary.STHE[i] * Math.Sin(this.primary.PhA);

                this.primary.R[i, j] = this.primary.getR0();
                this.primary.SL[i, j] = this.primary.WL[i, j];
                this.primary.SM[i, j] = this.primary.WM[i, j];
                this.primary.SN[i, j] = this.primary.WN[i];

            }
        }
    }

    protected virtual void calculateStarsGeometricParameters()
    {
        this.primary.setDTH(Math.PI / (float)(this.primary.getNTH() - 1));
        this.primary.setDPH(2.0 * Math.PI / (float)(this.primary.getNPH() - 1));
    }

    protected void lastStepStarsGeometricParameters()
    {
        SINIA = Math.Sin(this.IA);
        SN0 = Math.Cos(IA);
        this.RMAXR = this.max(this.secondary.R, this.secondary.getNTH(), this.secondary.getNPH());//CALL MAXIM (R2, NTH2, NPH2, RMAXR)
        this.RMAXW = this.max(this.primary.R, this.primary.getNTH(), this.primary.getNPH());//CALL MAXIM (R1, NTH1, NPH1, RMAXW)
    }

    protected void computeIntensityDistributions()
    {
        //        COMPUTE THE INTENSITY DISTRIBUITIONS ON THE TWO STAR AND THE  DISK
        this.computeSecondaryIntensityDistribution();
        this.computePrimayIntensityDistribution();
    }

    protected void computeSecondaryIntensityDistribution()
    {
        int i, j, II, JJ, k, ind = 0;
        double T2P, B2;
        //DO 400 I = 1, NTH22
        for (i = 1; i <= this.NTH22; i++)
        {
            II = this.secondary.getNTH() + 1 - i;
            //DO 400 J = 1, NPH22
            for (j = 1; j <= this.NPH22; j++)
            {
                JJ = this.secondary.getNPH() + 1 - j;//JJ = this.secondary.getNPH() +1  - j;
                T2P = (double)this.secondary.TF[ind];
                ind++;
                this.secondary.BR[i, j] = 0.0;
                //DO 31 K = 1, ICOLOR
                for (k = 1; k <= this.ICOLOR; k++)
                {
                    B2 = this.black((double)this.PLAMDA[k - 1], (double)this.CLAMDA[k - 1], T2P);
                    this.secondary.BR[i, j] = (double)this.secondary.BR[i, j] + this.PI2 * B2 * URL3 * FS * (double)this.SCOLOR[k - 1];
                }
                this.secondary.BR[i, JJ] = this.secondary.BR[i, j];
                this.secondary.BR[II, j] = this.secondary.BR[i, j];
                this.secondary.BR[II, JJ] = this.secondary.BR[i, j];
            }
        }
    }

    protected void computePrimayIntensityDistribution()
    {
        int i, j, k, II, JJ, ind = 0;
        double T1P = 0.0, B1;
        //DO 40 I = 1, NTH12
        for (i = 1; i <= this.NTH12; i++)
        {
            II = this.primary.getNTH() + 1 - i;
            //DO 40 J =1, NPH12
            for (j = 1; j <= this.NPH12; j++)
            {
                JJ = this.primary.getNPH() + 1 - j;//JJ = this.primary.getNPH() + 1 - i;
                if (this.KFLAG == 1)
                {
                    T1P = (double)this.primary.TF[ind];
                    ind++;
                }
                if (this.KFLAG != 1)
                {
                    T1P = this.primary.getTP();
                }
                this.primary.BR[i, j] = 0.0;
                //DO 41 K =1, ICOLOR
                for (k = 1; k <= this.ICOLOR; k++)
                {
                    B1 = this.black((double)this.PLAMDA[k - 1], (double)this.CLAMDA[k - 1], T1P);
                    this.primary.BR[i, j] = (double)this.primary.BR[i, j] + PI2 * B1 * UWD3 * FS * (double)this.SCOLOR[k - 1];
                }
                this.primary.BR[i, JJ] = this.primary.BR[i, j];
                this.primary.BR[II, j] = this.primary.BR[i, j];
                this.primary.BR[II, JJ] = this.primary.BR[i, j];
            }
        }
    }

    protected virtual void calculateTheDisk()
    {
    }

    protected virtual void calculateTheBrightSpot()
    {
    }

    protected void findSecondaryVisiblePoints()
    {
        int i, j;
        double p;
        this.loadPhaspData();
        this.C = 0.5;
        this.NPH21 = this.secondary.getNPH() - 1;
        this.D1TP = this.primary.getDPH() * this.primary.getDTH();
        //DO 100 K=1, quantos
        for (kControle = 1; kControle <= this.PHASP.Count; kControle++)
        {
            C = C + 1.0;
            p = (double)PHASP[kControle - 1] * 2.0 * Math.PI;
            //System.out.println(p);
            SINP = Math.Sin(p);//provavelmente pode retirar essa variavel do código
            COSP = Math.Cos(p);//provavelmente pode retirar essa variavel do código
                               //System.out.println(SINP +" "+COSP);
            SL0 = COSP * SINIA;
            SM0 = -SINP * SINIA;
            SR0 = 0.0;
            //System.out.println("X: "+SL0+"    Y:"+SM0);
            //DO 60 I =1,NTH2
            for (i = 1; i <= this.secondary.getNTH(); i++)
            {
                //DO 60 J=1,NPH21
                for (j = 1; j <= this.NPH21; j++)
                {
                    D[i, j] = this.secondary.SL[i, j] * SL0 + this.secondary.SM[i, j] * SM0 + this.secondary.SN[i, j] * SN0;
                    if (D[i, j] <= 0.0)
                    {
                        COSB = this.secondary.SL[i, j] * this.secondary.STHE[i] * this.secondary.CPHA[j] + this.secondary.SN[i, j] * this.secondary.CTHE[i];
                        COSB = COSB + this.secondary.STHE[i] * this.secondary.SPHA[j] * this.secondary.SM[i, j];
                        if (i == 1 || i == this.secondary.getNTH())
                        {
                            DDTHETA = 0.5 * this.secondary.THE[2];
                        }
                        else
                        {
                            DDTHETA = 0.5 * (this.secondary.THE[i + 1] - this.secondary.THE[i - 1]);
                        }
                        DSR0 = this.secondary.R[i, j] * this.secondary.R[i, j] * this.secondary.STHE[i] * DDTHETA * this.secondary.getDPH() / COSB;

                        this.obscuringEdge(i, j);
                    }
                }
            }
            ROCHE = SR0;

            this.projectSecondaryPointsAndCalculatePrimaryLight();
            this.calculateTheLightFromDiskAndTheSpot();
        }
        //System.out.println(this.disk.X.Count);
    }

    /*
     Coleção de metodos extraidos do fluxo do metodo  findSecondaryVisiblePoints()
     */
    private void obscuringEdge(int i, int j)
    {
        /*  AQUI ENTRA  A LEI DE OBSCURECIMENTO POR BORDA. NESSA VERSAO USO A LEI
         * DE RAIZ QUADRADA.
         */
        double cu, cu1, cu2, cu3;
        cu1 = 1.0 - this.primary.getCOEF() - this.secondary.getCOEF();
        cu2 = -D[i, j];
        cu3 = Math.Pow(cu2, 0.5);
        cu = cu1 + this.primary.getCOEF() * D[i, j] + this.secondary.getCOEF() * cu3;
        DSL0 = -this.secondary.BR[i, j] * cu * D[i, j];
        SR0 = SR0 + DSR0 * DSL0;
    }

    protected void loadPhaspData()
    {
        //double i = 0.0;
        int value = 0;
        while (0.010 * value < 1.00)
        {
            //this.PHASP.Add(i);
            //i = i + 0.1*(value);
            this.PHASP.Add(0.010 * value);
            value++;
        }
    }

    protected void projectSecondaryPointsAndCalculatePrimaryLight()
    {
        /* 
         PROJECT ALL THE POINTS ON THE EDGE OF SECONDARY STAR'S
         SURFACE ONTO THE PLANE OF THE SKY AND CALCULATE THE LIGHT
         FROM THE PRIMARY STAR
         */
        this.projectPointsOnSkyPlane();
        this.orderPolygonVertices();
        this.processPrimaryProjectedArea();
    }

    protected void calculateTheLightFromDiskAndTheSpot()
    {
        /*
         CALCULATE THE LIGHT FROM THE DISK AND THE SPOT
         */

        /* Aqui deveria existir a seguinte condição: */
        if (KFLAG != 3)
        {
            BRDK = 0.0;
            BRSP = 0.0;
        }
        else
        {
            this.calculateTheLightFromTheDisk();
            this.calculateTheLightFromTheSpot();
        }
        /*
         porém o Artur solicitou que eu forcasse a entrada no bloco do else.
         Futuramente o projeto deverá ser alterado nesse ponto.
         
         this.calculateTheLightFromTheDisk();
         this.calculateTheLightFromTheSpot();
         */
        this.calculateTheLightFromAllComponents();
    }

    /*
     coleção de metodos referentes aos subfluxos do metodo projectSecondaryPointsAndCalculatePrimaryLight()
     */
    private void projectPointsOnSkyPlane()
    {
        /*
         1, FOR EACH VALUE OF ANGLE THETA ON THE SURFACE OF THE
         SECUNDARY THERE ARE TWO PLACES WHERE PARAMETER D CHANGES ITS
         SING AS ANGLE PHAI INCREASES. LOCATE THESES TWO POINTS FOR
         EACH THETA AND PROJECT THE ONTO THE PLANE OF THE SKY. EACH
         PROJECTED POINT WOUD BE A VERTEX OF A POLYGON ON THE PLANE 
         OF THE SKY.
         */
        int i, j, MM;
        double DD = 0.0, DIJ, PHA0, RR0, SINPHA0, COSPHA0;
        LL = 0;
        //DO 70 I=1, NTH2
        for (i = 1; i <= this.secondary.getNTH(); i++)
        {
            if (i == 1 || i == this.secondary.getNTH())
            {
                MM = NPH21;
            }
            else
            {
                MM = 1;
            }
            // DO 70 J=1, NPH21, MM
            for (j = 1; j <= this.NPH21; j = j + MM)
            {
                if (MM == NPH21)
                {
                    //bloco linha 73
                    if (D[i, j] != 0.0)
                    {
                        //break;
                        continue;
                    }
                    SINPHA0 = this.secondary.SPHA[j];
                    COSPHA0 = this.secondary.CPHA[j];
                    RR0 = this.secondary.R[i, j];

                }
                else
                {
                    DD = D[i, j] * D[i, j + 1];
                    if (DD > 0.0)
                    {
                        //break;
                        continue;
                    }
                    DIJ = 1.0 / (D[i, j + 1] - D[i, j]);
                    PHA0 = this.secondary.PHA[j] - D[i, j] * this.secondary.getDPH() * DIJ;
                    RR0 = this.secondary.R[i, j] - D[i, j] * (this.secondary.R[i, j + 1] - this.secondary.R[i, j]) * DIJ;
                    SINPHA0 = Math.Sin(PHA0);
                    COSPHA0 = Math.Cos(PHA0);

                }
                //bloco linha 74
                LL = LL + 1;
                X = -RR0 * this.secondary.STHE[i] * COSPHA0 * SINP;
                X = X - RR0 * this.secondary.STHE[i] * SINPHA0 * COSP;
                Z = -RR0 * this.secondary.STHE[i] * SINPHA0 * SN0 * SINP - RR0 * this.secondary.CTHE[i] * SINIA;
                Z = Z + RR0 * this.secondary.STHE[i] * COSPHA0 * SN0 * COSP;
                ER2[LL] = Math.Sqrt(X * X + Z * Z);
                //CALL ANGLE (X, Z, EA2(LL))
                EA2[LL] = this.angle(X, Z);
                //System.out.println("X: "+X+"    Y:"+Z);
            }
            //            if (DD > 0.0 || D[i,j] != 0.0) {
            //                break;
            //            }
        }
    }

    private void orderPolygonVertices()
    {
        /* 2. NOW DO A BUBBLE SORT TO PUT THE VERTICES OF THE POLYGON
         INTO THE ORDER OF INCRESING ANGLES.*/

        //this.bubbleSort(ER2);
        //this.bubbleSort(EA2);
        //Aqui eu substitui o Buble-Sort pelo Quick-Sorte devido a complexidade algoritmica alta do primeiro!
        //
        this.quickSort(ER2, 1, ER2.Length - 1);
        this.quickSort(EA2, 1, EA2.Length - 1);
    }

    protected void processPrimaryProjectedArea()
    {
        /*
         3.DIVIDE THE PROJECTED AREA OF THE PRIMARY INTO MESH AND FOR
         EACH POINT CHECK WHETHER IT IS THE SHADON OF THE
         SECUNDARY STAR. IF NOT CALCULATE THE AREA AND CONTRIBUTE ITS	
         RADIATION TO THE TOTAL BRIGHTNESS OF THE PRIMARY OUTSIDE
         ECLIPSE.
         */
        double SWS = 0.0, PQ = 0.0, XW, YW, RWW, AWW, cu5, DIONI1, DW0;
        int NTHH, i, j;

        maxER2 = this.max(ER2, LL);
        minER2 = this.min(ER2, LL);

        NTHH = this.primary.getNTH();
        if (KFLAG == 3)
        {
            NTHH = NTH12;
        }
        //DO 80 I = 1, NTHH
        for (i = 1; i <= NTHH; i++)
        {
            //DO 80 J = 2, NPH1
            for (j = 2; j <= this.primary.getNPH(); j++)
            {
                PQ = -this.primary.SL[i, j] * SL0 - this.primary.SM[i, j] * SM0 + this.primary.SN[i, j] * SN0;
                if (PQ > 0.0)
                {
                    //break;
                    continue;
                }
                Z = this.primary.R[i, j] * this.primary.WN[i];
                Y = this.primary.R[i, j] * this.primary.WM[i, j];
                X = this.primary.R[i, j] * this.primary.WL[i, j];

                XW = X * SINP + Y * COSP - SINP;
                YW = Y * SINP * SN0 - X * COSP * SN0 - Z * SINIA + SN0 * COSP;
                //System.out.println("X: "+XW+"    Y:"+YW);
                RWW = Math.Sqrt(YW * YW + XW * XW);
                AWW = this.angle(XW, YW);
                S = this.shadow(maxER2, minER2, ER2, EA2, LL, RWW, AWW);
                if (S < 0.0)
                {
                    //break;
                    continue;
                }
                COSB = this.primary.WL[i, j] * this.primary.SL[i, j] + this.primary.WM[i, j] * this.primary.SM[i, j] + this.primary.WN[i] * this.primary.SN[i, j];
                if (i == 1 || i == this.primary.getNTH())
                {
                    DDTHETA = 0.5 * this.primary.STHE[2];
                }
                else
                {
                    DDTHETA = 0.5 * (this.primary.STHE[i + 1] - this.primary.STHE[i - 1]);
                }
                D1TP = DDTHETA * this.primary.getDPH();
                DWS = this.primary.R[i, j] * this.primary.R[i, j] * this.primary.STHE[i] * D1TP / COSB;
                /*
                 AQUI ENTRA  A LEI DE OBSCURECIMENTO POR BORDA. NESSA VERSAO USO A LEI
                 DE RAIZ QUADRADA.
                 */
                cu5 = -PQ;
                DIONI1 = (1.0 - this.primary.getCOEF() - this.primary.getCOEF() * PQ - this.secondary.getCOEF() + this.secondary.getCOEF() * (Math.Pow(cu5, 0.5)));
                DW0 = this.primary.BR[i, j] * DIONI1 * PQ;
                //DWS = this.primary.R[i,j] * this.primary.R[i,j] * this.primary.STHE[i] * D1TP / COSB;
                //DW0 = -this.primary.BR[i,j] * (1.0 - this.primary.getU() - this.primary.getU() * PQ) * PQ;
                SWS = SWS + DWS * DW0;
            }
            //            if (PQ > 0.0 || S < 0.0) {
            //                break;
            //            }
        }
        SWP = SWS;
    }
    /*
     FIM coleção de metodos referentes aos subfluxos do metodo projectSecondaryPointsAndCalculatePrimaryLight()
     */
    /*
	Coleção de metodos referentes aos subfluxos do metodo calculateTheLightFromDiskAndTheSpot()
		*/

    private void calculateTheLightFromTheDisk()
    {
        /*
         CALCULATE THE LIGHT FROM THE DISK 
         */
        int i, j;
        double DAREA = 0.0, RRD, AADK, DS;
        //DO 90 I = 1, NRDK
        for (i = 1; i <= NRDK; i++)
        {
            //DO 90 J = 1, NADK
            for (j = 1; j <= NADK; j++)
            {
                XX = this.RDK[i] * Math.Cos(this.ADK[j]);
                YY = this.RDK[i] * Math.Sin(this.ADK[j]);
                XP = XX * SINP + YY * COSP;
                YP = -XX * COSP + YY * SINP;
                X = XP - SINP;
                Y = (YP + COSP) * SN0;
                //adiciona os pontos gerados no objeto disco!
                //this.getDisk().addPoint(XP, YP);
                //if (j % 2 == 0) this.getDisk().addPoint(XP, YP);
                //System.out.println("X: "+X+"    Y:"+Y);

                RRD = Math.Sqrt(X * X + Y * Y);
                AADK = this.angle(X, Y);
                S = this.shadow(this.maxER2, this.minER2, this.ER2, this.EA2, LL, RRD, AADK);
                if (S < 0.0)
                {
                    //break;
                    continue;
                }
                DS = RDK[i] * DRDK * DADK * BDK[i, j] * (1.0 - UDK + UDK * SN0) * SN0;
                DAREA = DAREA + DS;
            }
            //            if (S < 0.0) {
            //                break;
            //            }
        }
        //this.disk.addPoint(X, Y);
        BRDK = DAREA;
    }

    private void calculateTheLightFromTheSpot()
    {
        /*
         CALCULATE THE LIGHT FROM THE  SPOT
         */
        int i, j;
        double RRD, AADK, DS;
        double SAREA;
        if (NASP1 == NASP2)
        {
            BRSP = 0.0;
        }
        else
        {
            SAREA = 0.0;
            //DO 95 I = NRSP, NRDK
            for (i = NRSP; i <= NRDK; i++)
            {
                //DO 95 J= NASP1, NASP2
                for (j = (int)NASP1; j <= NASP2; j++)
                {
                    if ((double)PHASP[kControle - 1] > 0.0)
                    {
                        //break;
                        continue;
                    }
                    XX = RDK[i] * Math.Cos(ADK[j]);
                    YY = RDK[i] * Math.Sin(ADK[j]);
                    XP = XX * SINP + YY * COSP;
                    YP = -XX * COSP + YY * SINP;
                    X = XP - SINP;
                    Y = (YP + COSP) * SN0;
                    //System.out.println("X: "+X+"    Y:"+Y);
                    RRD = Math.Sqrt(X * X + Y * Y);
                    AADK = this.angle(X, Y);
                    S = this.shadow(this.maxER2, this.minER2, ER2, EA2, LL, RRD, AADK);
                    if (S < 0.0)
                    {
                        //break;
                        continue;
                    }
                    DS = RDK[i] * DRDK * DADK * BDK[i, j] * (1.0 - UDK + UDK * SN0) * SN0;
                    SAREA = SAREA + DS;
                }
                //                if (PHASP.get(kControle - 1) > 0.0 || S < 0.0) {
                //                    break;
                //                }
            }
            BRSP = SAREA;
        }
    }

    protected void calculateTheLightFromAllComponents()
    {
        /*
         AND THE LIGHT FROM ALL COMPONENTS TOGETHER
         */

        BRIT = SWP + ROCHE + BRDK + BRSP;

        this.swp.Add(SWP);
        this.roche.Add(ROCHE);
        this.brdk.Add(BRDK);
        this.brsp.Add(BRSP);

        this.Brit.Add(BRIT);
        //this.Brit.Add(Math.abs(BRIT / 1000000));
        //System.out.println(this.PHASP.get(kControle - 1) + "            " + BRIT);
    }

    private double maxBritValue()
    {
        double saida = (double)this.Brit[0];
        int i;
        for (i = 0; i < this.Brit.Count; i++)
        {
            if ((double)this.Brit[i] > saida)
            {
                saida = (double)this.Brit[i];
            }
        }
        return saida;
    }
    /*
     Fim Coleção de metodos referentes aos subfluxos do metodo calculateTheLightFromDiskAndTheSpot()
     */

    //**************************************************************************
    /*
    
     Biblioteca de subrotinas do código original
     */
    protected double max(double[,] x, int dim1, int dim2)
    {
        int i, j;
        double big = 0.0;
        for (i = 1; i <= dim1; i++)
        {
            for (j = 1; j <= dim2; j++)
            {
                if (big < x[i, j])
                {
                    big = x[i, j];
                }
            }
        }
        return big;
    }

    protected double max(double[] x, int dim)
    {
        int i;
        double big = 0.0;
        for (i = 1; i <= dim; i++)
        {
            if (big < x[i])
            {
                big = x[i];
            }
        }
        return big;
    }

    protected double min(double[] x, int dim)
    {
        int i;
        double small = 3.0;
        for (i = 1; i <= dim; i++)
        {
            if (small > x[i])
            {
                small = x[i];
            }
        }
        return small;
    }

    protected double black(double plamda, double clamda, double t)
    {
        double B = 0.0, C2, C2LT;
        C2 = 1.43883;
        C2LT = C2 * plamda / t;
        //B = C/ (EXP(C2LT) -1.0) * 0.000001
        B = clamda / (Math.Exp(C2LT) - 1.0) * 0.000001;
        return B;
    }

    protected double angle(double x, double y)
    {
        double A = 0, B;
        if (x == 0.0)
        {
            if (y > 0.0)
            {
                A = Math.PI / 2.0;
            }
            else
            {
                A = 3.0 * Math.PI / 2.0;
            }
        }
        else
        {
            B = Math.Atan(y / x);
            if (x < 0.0)
            {
                A = Math.PI + B;
            }
            else if (y < 0.0)
            {
                A = 2.0 * Math.PI + B;
            }
            else
            {
                A = B;
            }
        }
        return A;
    }

    protected void bubbleSort(double[] v)
    {
        /*Algoritmo de ordenação de vetores
         Complexidade algoritmica:N^2
         */
        for (int i = v.Length; i >= 1; i--)
        {
            for (int j = 1; j < i; j++)
            {
                if (v[j - 1] > v[j])
                {
                    double aux = v[j];
                    v[j] = v[j - 1];
                    v[j - 1] = aux;
                }
            }
        }
    }

    /*
     *Algoritmo de ordenação Quick Sort
     */
    protected int partition(double[] arr, int left, int right)
    {
        int i = left, j = right;
        double tmp;
        double pivot = arr[(left + right) / 2];

        while (i <= j)
        {
            while (arr[i] < pivot)
            {
                i++;
            }
            while (arr[j] > pivot)
            {
                j--;
            }
            if (i <= j)
            {
                tmp = arr[i];
                arr[i] = arr[j];
                arr[j] = tmp;
                i++;
                j--;
            }
        }

        return i;
    }

    protected void quickSort(double[] arr, int left, int right)
    {
        /*Algoritmo de Ordenação QuickSort
         Complexidade algoritmica: N(log(N))
         */

        int index = partition(arr, left, right);
        if (left < index - 1)
        {
            quickSort(arr, left, index - 1);
        }
        if (index < right)
        {
            quickSort(arr, index, right);
        }
    }
    /*
     FIM Quick Sort
     ****************************************************************************
     */

    protected double shadow(double rMax, double rMin, double[] rv, double[] av, int nv, double r, double a)
    {
        /*
         THIS SUBROUTINE TESTS WHETHER A POINT (R,A) IS INSIDE THE 
         CONVEX POLYGON GIVEN BY THE ORDERED POINTS (RV, AV). INPUT
         DATA ARE RV, AV = ARRAYS CONTAINING THE POINTS (R, THETA) OF
         THE VERTICES OF THE CONVEX POLYGON WITH NV VERTICES.
         RMAX, RMIN = MAXIMUM AND MINIMUM VALUES OF THE RADII
         CONTAINED IN ARRAY RV.
         R, A = THE COORDINATE (R,THETA) OF THE POINT TO BE TESTED.
         OUTPUT DATA:
         S = +1.0 : POINT UNSHADOWED; S=-1.0 : POINT SHADOWE;
         */
        int i, k;
        double S = 0, X, X1, X2, Y1, Y2, Q, nv1;
        double TWOPI = 2.0 * 3.1415926;
        //nv1 = nv - 1;
        if (r > rMax)
        {
            S = 1.0;
            return S;
        }
        if (r < rMin)
        {
            S = -1.0;
            return S;
        }
        if (a >= av[1])
        {
            if (a <= av[nv])
            {
                nv1 = nv - 1;
                //DO 30 I =1, NV1
                for (i = 1; i <= nv1; i++)
                {
                    k = i + 1;
                    if ((a >= av[i]) && (a <= av[k]))
                    {
                        //bloco label 40
                        X1 = av[i];
                        X2 = av[k];
                        X = a;
                        Y1 = rv[i];
                        Y2 = rv[k];
                        //bloco label 100
                        if ((r > Y1) && (r > Y2))
                        {
                            S = 1.0;
                            return S;
                        }
                        if ((r <= Y1) && (r <= Y2))
                        {
                            S = -1.0;
                            return S;
                        }
                        Q = ((X - X1) / (X2 - X1)) * (Y2 - Y1) + Y1;
                        if (r <= Q)
                        {
                            S = -1.0;
                            return S;
                        }
                        S = 1.0;
                        return S;
                    }
                }
            }
            X1 = av[nv];
            X2 = av[1] + TWOPI;
            X = a;
            Y1 = rv[nv];
            Y2 = rv[1];
            //bloco label 100
            if ((r > Y1) && (r > Y2))
            {
                S = 1.0;
                return S;
            }
            if ((r <= Y1) && (r <= Y2))
            {
                S = -1.0;
                return S;
            }
            Q = ((X - X1) / (X2 - X1)) * (Y2 - Y1) + Y1;
            if (r <= Q)
            {
                S = -1.0;
                return S;
            }
            S = 1.0;
            return S;
        }
        X1 = av[nv];
        X2 = av[1] + TWOPI;
        X = a + TWOPI;
        Y1 = rv[nv];
        Y2 = rv[1];
        //bloco do label 100
        if ((r > Y1) && (r > Y2))
        {
            S = 1.0;
            return S;
        }
        if ((r <= Y1) && (r <= Y2))
        {
            S = -1.0;
            return S;
        }
        Q = ((X - X1) / (X2 - X1)) * (Y2 - Y1) + Y1;
        if (r <= Q)
        {
            S = -1.0;
            return S;
        }
        S = 1.0;
        return S;
    }

    public double[] getVectorPhasp()
    {
        double[] saida = new double[this.PHASP.Count];
        int i;
        for (i = 0; i < this.PHASP.Count; i++)
        {
            saida[i] = (double)this.PHASP[i];
        }
        return saida;
    }

    public double[] getVectorBrit()
    {
        double[] saida = new double[this.Brit.Count];
        int i;
        for (i = 0; i < this.Brit.Count; i++)
        {
            saida[i] = (double)this.Brit[i];
        }
        return saida;
    }

    /**
     * @return the disk
     */
    //	public Disk getDisk() {
    //		return disk;
    //	}

    //	public void gravarPhaspBrit() throws IOException {
    //		File file = new File("PhaspAndBrit.csv");
    //		if (!file.exists()) {
    //			file.createNewFile();
    //		}
    //
    //		FileWriter fw = new FileWriter(file);
    //		BufferedWriter bw = new BufferedWriter(fw);
    //
    //		bw.write("SWP,ROCHE,BRDK,BRSP,BRIT,PHASP");
    //		bw.newLine();
    //		for (int i = 0; i < this.Brit.Count; i++) {
    //			bw.write(this.swp[i] + "," + this.roche[i] + "," + this.brdk[i] + "," + this.brsp[i] + "," + this.Brit[i] + "," + this.PHASP[i]);
    //			bw.newLine();
    //		}
    //
    //		bw.close();
    //		fw.close();
    //
    //	}
    //
    //	public void showLightCurveGraphic() throws IOException {
    //		double[] brit = new double[this.getVectorBrit().length * 2];
    //		double[] b = this.getVectorBrit();
    //		for (int i = 0; i < b.length; i++) {
    //			brit[i] = b[i];
    //			brit[i + b.length] = b[i];
    //		}
    //
    //		double[] phasp = new double[this.getVectorPhasp().length * 2];
    //		double[] p = this.getVectorPhasp();
    //		for (int i = 0; i < p.length; i++) {
    //			phasp[i] = p[i];
    //			phasp[i + p.length] = p[i] + 1.0;
    //		}
    //
    //		Plot2DPanel plot = new Plot2DPanel();
    //		plot.addLegend("SOUTH");
    //		plot.setAxisLabel(0, "Phase");
    //		plot.setAxisLabel(1, "Bright");
    //		plot.addLinePlot("Curve", phasp, brit);
    //		JFrame frame = new JFrame("Light Curves");
    //		frame.setSize(600, 600);
    //		frame.setContentPane(plot);
    //		frame.setLocationRelativeTo(null);
    //		frame.setVisible(true);
    //		this.gravarPhaspBrit();
    //		//System.out.println(frame.getDefaultCloseOperation());
    //		//frame.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
    //	}

    public void showLightCurveGraphic()
    {
        Console.WriteLine("Implementar");
    }
}
