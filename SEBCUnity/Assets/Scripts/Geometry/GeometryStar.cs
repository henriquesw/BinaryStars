public class GeometryStar
{

    protected int NTH;//number of grid points along the Theta direction of this star
    protected int NPH;//number of grit points along the Fi direction of this star
    protected double W;//Gravitational potential correspondig to Q;
    protected double R0;//Estimated radius of this star
    protected double Q;//Mass ratio of the two stars
    protected double TP;//temperature on the pole of this star;
    protected double COEF;
    protected double U;
    protected double AB;//Albedo of the surface of this star
    private PointsCollection pontosCartesianos;

    public double[] THE, STH;

    public GeometryStar()
    {
        this.NTH = 31;
        this.NPH = 51;
        this.pontosCartesianos = new PointsCollection();
    }

    protected virtual void initializeVectorsandMatrix()
    {
        THE = new double[this.getNPH()];
        STH = new double[this.getNPH()];
    }

    public void changeCoordenates()
    {
        this.pontosCartesianos.changeCoordenates();
    }

    public void cleanPontos()
    {
        this.pontosCartesianos = new PointsCollection();
    }

    public int getNTH()
    {
        return this.NTH;
    }

    public int getNPH()
    {
        return this.NPH;
    }

    public void setNTH(int NTH)
    {
        this.NTH = NTH;
    }

    public void setNPH(int NPH)
    {
        this.NPH = NPH;
    }

    public double getW()
    {
        return W;
    }

    public void setW(double W)
    {
        this.W = W;
    }

    public double getR0()
    {
        return R0;
    }

    public void setR0(double R0)
    {
        this.R0 = R0;
    }

    public double getQ()
    {
        return Q;
    }

    public void setQ(double Q)
    {
        this.Q = Q;
    }

    public double getTP()
    {
        return TP;
    }

    public void setTP(double TP)
    {
        this.TP = TP;
    }

    public double getCOEF()
    {
        return COEF;
    }

    public void setCOEF(double COEF)
    {
        this.COEF = COEF;
    }

    public double getAB()
    {
        return AB;
    }

    public void setAB(double AB)
    {
        this.AB = AB;
    }

    public PointsCollection getPontos()
    {
        return pontosCartesianos;
    }

    public void setPontos(PointsCollection pontos)
    {
        this.pontosCartesianos = pontos;
    }

    public double getU()
    {
        return U;
    }

    public void setU(double U)
    {
        this.U = U;
    }

}
