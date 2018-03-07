using System;

public class Geometry
{
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private GeometryStar primary, secondary;
    private double ANGU, L, M, N, LS, MS, NS, QSS, FI, RS,
    //NRDK = 31,
    //NADK = 51;
    //FACTOR = -0.95;
    //RIN = 0.135;
    //ROUT = 0.39;
    R = 0,//Se der problema no metodo Normal() essa variavel deve ser a primeira causa investigada!!!
    G,
    KFLAG, //mode-selector;
    KCOLOR,//Color-selector
    BETA,//Gravity-darkening coefficient
    BASE,//number to normalize the light curve
    QCONT;

    public Geometry(double kflag, double kcolor, double angu, double r01, double q2, double tp1, double tp2, double coef1, double coef2, double ab2, double beta, double Base)
    {
        /*
         Inicializa as variáveis a partir dos valores fornecidos
         */
        this.primary = new GeometryStar();
        this.secondary = new GeometryStar();
        this.KFLAG = kflag;
        this.KCOLOR = kcolor;
        this.ANGU = angu;
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

    public void calculateGeometry()
    {
        /*
         Fluxo principal!
         Váriável KFLAG define desvios no código original,
         aqui ela foi utilizada para conduzir o fluxo de execução!
         */
        if (this.KFLAG == 3)
        {
            this.mode3();
        }
        else if (this.KFLAG == 2)
        {
            this.mode2();
        }
        else if (this.KFLAG == 1)
        {
            this.mode1();
        }
    }
    /*
     Os métodos abaixo representam subfluxos do programa
     */

    private void calculateSecondaryGravitationalPotential()
    {
        //Calcula o potencial gravitacional da secundaria, bem como o seu raio
        QCONT = this.getSecondary().getQ();
        QSS = QCONT;
        this.lagranjeano();
        this.getSecondary().setW(FI);
        this.getSecondary().setR0(RS);
        this.getSecondary().setQ(QSS);

    }

    private void calculatePrimaryGravitationalPotential()
    {
        //Calcula o potencial gravitacional da secundaria, bem como o seu raio
        QCONT = 1 / this.getSecondary().getQ();
        QSS = QCONT;
        this.lagranjeano();
        this.getPrimary().setW(FI);
        this.getPrimary().setR0(RS);
        this.getPrimary().setQ(QSS);
    }

    private void mode1()
    {
        /*
         Roche geometry for both component stars;
         */

        //The secondary
        this.secondary.cleanPontos();
        this.calculateSecondaryGravitationalPotential();
        this.calculatePrimaryGravitationalPotential();
        this.meshTheSecondaryStar();
        //The primary
        this.primary.cleanPontos();
        this.meshThePrimaryStar();
    }

    private void mode2()
    {
        /*
         *Roche lobe for one component star and the other is assumed to be a sphere(the primary)
         */
        this.secondary.cleanPontos();
        this.calculateSecondaryGravitationalPotential();
        this.meshTheSecondaryStar();
    }

    private void mode3()
    {
        /*
         The same as in mode 2 except a disk is surrounding the spherical star
         */
        this.mode2();
    }

    private void meshTheSecondaryStar()
    {
        /*
         Verificar repetição:
         Esta executando 52 vezes a menos do que o código em Fortran!!!
         Verificar valores das variaveis LS, MS, NS
         */
        double DTHETA = 2.0 / (float)(this.getSecondary().getNTH() - 1);
        double DPHAI = 2.0 * Math.PI / (float)(this.getSecondary().getNPH() - 1);
        double THETA, SINTHETA;
        double PHAI, COSPHAI, SINPHAI;
        int i, j;
        for (i = 1; i <= this.getSecondary().getNTH(); i++)
        {
            N = (1.0 - DTHETA * (i - 1));
            if (N <= (-1))
            {
                N = (-1);
            }
            THETA = Math.Acos(N);
            SINTHETA = Math.Sin(THETA);
            for (j = 1; j <= this.getSecondary().getNPH(); j++)
            {
                PHAI = DPHAI * (float)(j - 1);
                COSPHAI = Math.Cos(PHAI); //returning 0 or 1 aproximated
                SINPHAI = Math.Sin(PHAI);
                L = (SINTHETA * COSPHAI);
                M = (SINTHETA * SINPHAI);
                if (Math.Abs(N) < 0.00001 && L > 0.99999)
                {
                    this.R = this.lagrange(this.getSecondary().getQ(), this.getSecondary().getW(), this.getSecondary().getR0());
                    this.normal(this.getSecondary().getQ());
                }
                else
                {
                    this.R = this.newton(this.getSecondary().getQ(), this.getSecondary().getW(), this.getSecondary().getR0());
                    this.normal(this.getSecondary().getQ());
                }
                this.secondary.getPontos().R.Add(R);
                this.secondary.getPontos().G.Add(G);
                this.secondary.getPontos().x.Add(LS);
                this.secondary.getPontos().y.Add(MS);
                this.secondary.getPontos().z.Add(NS);
            }
        }
    }

    private void meshThePrimaryStar()
    {
        /*
         Verificar repetição:
         Esta executando 52 vezes a menos do que o código em Fortran!!!
         Verificar valores das variaveis LS, MS, NS
         */
        double DTHETA = Math.PI / (float)(this.getPrimary().getNTH() - 1);
        double DPHAI = 2.0 * Math.PI / (float)(this.getPrimary().getNPH() - 1);
        double THETA, SINTHETA;
        double PHAI, COSPHAI, SINPHAI;
        int i, j;
        for (i = 1; i <= this.getPrimary().getNTH(); i++)
        {
            THETA = DTHETA * (float)(i - 1);
            N = (Math.Cos(THETA));
            SINTHETA = Math.Sin(THETA);
            for (j = 1; j <= this.getPrimary().getNPH(); j++)
            {
                PHAI = DPHAI * (float)(j - 1);
                COSPHAI = Math.Cos(PHAI);
                SINPHAI = Math.Sin(PHAI);
                L = (SINTHETA * COSPHAI);
                M = (SINTHETA * SINPHAI);
                if (Math.Abs(N) < 0.00001 && L > 0.99999)
                {
                    this.R = this.lagrange(this.getPrimary().getQ(), this.getPrimary().getW(), this.getPrimary().getR0());
                    this.normal(this.getPrimary().getQ());
                }
                else
                {
                    this.R = this.newton(this.getPrimary().getQ(), this.getPrimary().getW(), this.getPrimary().getR0());
                    this.normal(this.getPrimary().getQ());
                }
                this.primary.getPontos().R.Add(R);
                this.primary.getPontos().G.Add(G);
                this.primary.getPontos().x.Add(LS);
                this.primary.getPontos().y.Add(MS);
                this.primary.getPontos().z.Add(NS);
            }
        }
    }

    /*
     ############################################################################
     Coleção de metodos equivalente a coleção de procedimentos 
     no código original em Fortran. Todos metodos abaixo correspondem aos procedimentos
     com mesmo nome no código Fortran utilizado para iniciar este projeto.
     Obs.: Código traduzido para java linha a linha.
     Precisa analisar se é possível refatorar!!!
     ############################################################################    
     */
    private double newton(double Q, double W, double R0)
    {
        /*
         Metodo numérico de Newton para calcular a derivada
         */
        double df, a, b, y, d, c, z, f, x;
        a = 0.5d * (1.0d + Q);
        b = 1.0d - N * N;
        y = R0;
        d = a * b;
        do
        {
            c = y * y;
            z = 1.0d / Math.Sqrt(1.0d - 2.0d * L * y + c);
            f = W - 0.5d * Q * Q / (1.0d + Q) - 1.0d / y - Q * (z - L * y) - d * c;
            df = 1.0d / c - Q * ((L - y) * z * z * z - L) - 2.0d * d * y;
            x = y - f / df;
            if (!(Math.Abs(x - y) < 0.00001d && Math.Abs(f) < 0.00001d))
            {
                y = x;
            }
        } while (!(Math.Abs(x - y) < 0.00001d && Math.Abs(f) < 0.00001d));

        return x;
    }

    private double lagrange(double Q, double W, double R0)
    {
        double df, x, a, y, c, z, f;
        a = 0.5 * (1.0 + Q);
        y = R0;
        do
        {
            c = y * y;
            z = 1.0 / (1.0 - y);
            f = W - 0.5 * Q * Q / (1.0 + Q) - 1.0 / y - Q * (z - y) - a * c;
            df = 1.0 / c - Q * (z * z - 1.0) - 2.0 * a * y;
            x = y - f / df;
            if (!(Math.Abs(x - y) < 0.00001 && Math.Abs(f) < 0.00001))
            {
                y = x;
            }
        } while (!(Math.Abs(x - y) < 0.00001 && Math.Abs(f) < 0.00001));

        return x;
    }

    private void normal(double Q)
    {
        /*
         Correspondente a subrotina Normal no código original em Fortran
         Obs.: Precisa validar o metodo! Principalmente a Variável R.
         */
        double pbx, pby, pbz, gg,
        a, b, c, d, e, f, p, s, t;

        a = (1.0 + Q) * 0.5;
        b = 2.0 * Q / (1.0 + Q);
        c = 1.0 - L * L;
        d = 1.0 - N * N;
        e = Math.Sqrt(1.0 - 2.0 * R * L + R * R);
        //System.out.println(String.format("%.19f", e));
        f = 1.0 / (e * e * e);
        p = -1.0 / (R * R) - Q * (R - L) * f + a * (2.0 * R * d - b * L);
        s = Q * (f - 1.0);
        t = -2.0 * a * R * N;
        pbx = L * p + c * s - L * N * t;
        pby = M * p - L * M * s - M * N * t;
        pbz = N * p - L * N * s + d * t;
        G = -Math.Sqrt(pbx * pbx + pby * pby + pbz * pbz);
        gg = 1.0 / G;
        LS = (pbx * gg);
        MS = (pby * gg);
        NS = (pbz * gg);
        //LS, MS, NS
    }

    private void lagranjeano()
    {
        /*
         Correspondente a subrotina Lagrangeano no código original em Fortran
         */
        int i, j, s = 0;
        double Y1, Y2, Y21, Y11, R1, R2, U, V, XS = 0, DD, cu, AS, BS, CS, IQ, XIS;
        double[] f = new double[1000000];
        double[] g = new double[1000000];
        double dp;
        cu = 0.1;
        for (i = 1; i < 999999; i++)
        {           
            XIS = 0.000001 * i;
            f[i] = (1.0 / (Math.Pow(XIS, 2))) - XIS;
            double a = (1.0 / (Math.Pow(XIS, 2))) - XIS;
            DD = 1 / ((1 - XIS) * (1 - XIS));
            g[i] = QSS * (DD - (1 - XIS));
            double b= QSS * (DD - (1 - XIS));
            dp = Math.Sqrt(Math.Pow((f[i] - g[i]), 2));
            if (dp < cu)
            {
                cu = Math.Abs(f[i] - g[i]);
                s = i;
                XS = XIS;
            }
        }
        for (j = 1; j < 100000; j++)
        {
            XIS = (s) * 0.000001;
            f[j] = (1.0 / Math.Pow(XIS, 2)) - XIS;
            DD = 1 / ((1 - XIS) * (1 - XIS));
            g[j] = QSS * (DD - (1 - XIS));
            dp = Math.Sqrt(Math.Pow((f[j] - g[j]), 2));
            if (dp < cu)
            {
                cu = Math.Abs(f[j] - g[j]);
                s = j;
                XS = XIS;
            }
        }
        XIS = XS;
        U = QSS / (1 + QSS);
        Y1 = XIS - U;
        Y2 = 0.0;
        Y11 = -U;
        Y21 = (1.0d) - U;
        R1 = Math.Sqrt((Y1 - Y11) * (Y1 - Y11) + (Y2 * Y2));
        R2 = Math.Sqrt((Y1 - Y21) * (Y1 - Y21) + (Y2 * Y2));
        V = 2.0 * (1 - U) / R1 + 2.0 * U / R2 + (Math.Pow(Y1, 2) + Math.Pow(Y2, 2));
        FI = (V * ((1 + QSS) / 2.0));
        IQ = 1.0 / QSS;
        AS = 0.49 * Math.Pow(IQ, (2.0 / 3.0));
        BS = 0.6 * Math.Pow(IQ, (2.0 / 3.0));
        CS = Math.Log((1 + Math.Pow(IQ, (1.0 / 3.0))));
        RS = (AS / (BS + CS));
    }
    /*
     ################################################################################
    
     Coleção de metodos auxiliáres
    
     ################################################################################
     */

    //	public void printGeometria() {
    //		System.out.println(String.format("%.19f", getKFLAG()) + "\n" + String.format("%.19f", getKCOLOR()) + "\n" + String.format("%.19f", getANGU()) + "\n" + String.format("%.19f", this.getPrimary().getR0()) + "\n"
    //			+ String.format("%.19f", this.getSecondary().getQ()) + "\n" + String.format("%.19f", this.getPrimary().getTP()) + "\n" + String.format("%.19f", this.getSecondary().getTP()) + "\n" + String.format("%.19f", this.getPrimary().getCOEF()) + "\n"
    //			+ String.format("%.19f", this.getSecondary().getCOEF()) + "\n" + String.format("%.19f", this.getSecondary().getAB()) + "\n" + String.format("%.19f", getBETA()) + "\n" + String.format("%.19f", getBASE()));
    //	}

    /**
     * @return the primary
     */
    public GeometryStar getPrimary()
    {
        return primary;
    }

    /**
     * @param primary the primary to set
     */
    public void setPrimary(GeometryStar primary)
    {
        this.primary = primary;
    }

    public static double getPI()
    {
        return Math.PI;
    }

    /**
     * @return the secondary
     */
    public GeometryStar getSecondary()
    {
        return secondary;
    }

    /**
     * @param secondary the secondary to set
     */
    public void setSecondary(GeometryStar secondary)
    {
        this.secondary = secondary;
    }

    /**
     * @return the ANGU
     */
    public double getANGU()
    {
        return ANGU;
    }

    /**
     * @param ANGU the ANGU to set
     */
    public void setANGU(double ANGU)
    {
        this.ANGU = ANGU;
    }

    /**
     * @return the L
     */
    public double getL()
    {
        return L;
    }

    /**
     * @param L the L to set
     */
    public void setL(double L)
    {
        this.L = L;
    }

    /**
     * @return the M
     */
    public double getM()
    {
        return M;
    }

    /**
     * @param M the M to set
     */
    public void setM(double M)
    {
        this.M = M;
    }

    /**
     * @return the N
     */
    public double getN()
    {
        return N;
    }

    /**
     * @param N the N to set
     */
    public void setN(double N)
    {
        this.N = N;
    }

    /**
     * @return the LS
     */
    public double getLS()
    {
        return LS;
    }

    /**
     * @param LS the LS to set
     */
    public void setLS(double LS)
    {
        this.LS = LS;
    }

    /**
     * @return the MS
     */
    public double getMS()
    {
        return MS;
    }

    /**
     * @param MS the MS to set
     */
    public void setMS(double MS)
    {
        this.MS = MS;
    }

    /**
     * @return the NS
     */
    public double getNS()
    {
        return NS;
    }

    /**
     * @param NS the NS to set
     */
    public void setNS(double NS)
    {
        this.NS = NS;
    }

    /**
     * @return the QSS
     */
    public double getQSS()
    {
        return QSS;
    }

    /**
     * @param QSS the QSS to set
     */
    public void setQSS(double QSS)
    {
        this.QSS = QSS;
    }

    /**
     * @return the FI
     */
    public double getFI()
    {
        return FI;
    }

    /**
     * @param FI the FI to set
     */
    public void setFI(double FI)
    {
        this.FI = FI;
    }

    /**
     * @return the RS
     */
    public double getRS()
    {
        return RS;
    }

    /**
     * @param RS the RS to set
     */
    public void setRS(double RS)
    {
        this.RS = RS;
    }

    /**
     * @return the R
     */
    public double getR()
    {
        return R;
    }

    /**
     * @param R the R to set
     */
    public void setR(double R)
    {
        this.R = R;
    }

    /**
     * @return the G
     */
    public double getG()
    {
        return G;
    }

    /**
     * @param G the G to set
     */
    public void setG(double G)
    {
        this.G = G;
    }

    /**
     * @return the KFLAG
     */
    public double getKFLAG()
    {
        return KFLAG;
    }

    /**
     * @param KFLAG the KFLAG to set
     */
    public void setKFLAG(double KFLAG)
    {
        this.KFLAG = KFLAG;
    }

    /**
     * @return the KCOLOR
     */
    public double getKCOLOR()
    {
        return KCOLOR;
    }

    /**
     * @param KCOLOR the KCOLOR to set
     */
    public void setKCOLOR(double KCOLOR)
    {
        this.KCOLOR = KCOLOR;
    }

    /**
     * @return the BETA
     */
    public double getBETA()
    {
        return BETA;
    }

    /**
     * @param BETA the BETA to set
     */
    public void setBETA(double BETA)
    {
        this.BETA = BETA;
    }

    /**
     * @return the BASE
     */
    public double getBASE()
    {
        return BASE;
    }

    /**
     * @param BASE the BASE to set
     */
    public void setBASE(double BASE)
    {
        this.BASE = BASE;
    }

    /**
     * @return the QCONT
     */
    public double getQCONT()
    {
        return QCONT;
    }

    /**
     * @param QCONT the QCONT to set
     */
    public void setQCONT(double QCONT)
    {
        this.QCONT = QCONT;
    }

}
