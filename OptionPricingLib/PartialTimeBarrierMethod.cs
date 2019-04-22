using MathNet.Numerics.Distributions;
using Accord.Statistics.Distributions.Multivariate;
using System;


namespace OptionPricingLib
{
    public class PartialTimeBarrierMethod
    {
        //Util Functions
        private static double Log(double X) { return Math.Log(X); }
        private static double CND(double X) { return Normal.CDF(0, 1, X); }
        private static double Exp(double X) { return Math.Exp(X); }
        private static double Sqr(double x) { return Math.Sqrt(x); }
        private static double M(double x, double y, double rho)
        { return MultivariateNormalDistribution.Bivariate(0, 0, 1, 1, rho).DistributionFunction(new double[] { x, y }); }


        public static double PartialTimeBarrierOption(string TypeFlag, double S, double x, double h, double t1, double T2, double r, double b, double v)
        {
            double price = double.NaN;
            double d1, d2, f1, f2, e1, e2, e3, e4, g1, g2, g3, g4, mu, rho, eta, z1, z2, z3, z4, z5, z6, z7, z8;

            eta = 1;
            if (TypeFlag == "cdoA") { eta = 1; }
            else if (TypeFlag == "cuoA") { eta = -1; }


            d1 = (Log(S / x) + (b + v * v / 2) * T2) / (v * Sqr(T2));
            d2 = d1 - v * Sqr(T2);
            f1 = (Log(S / x) + 2 * Log(h / S) + (b + v * v / 2) * T2) / (v * Sqr(T2));
            f2 = f1 - v * Sqr(T2);
            e1 = (Log(S / h) + (b + v * v / 2) * t1) / (v * Sqr(t1));
            e2 = e1 - v * Sqr(t1);
            e3 = e1 + 2 * Log(h / S) / (v * Sqr(t1));
            e4 = e3 - v * Sqr(t1);
            mu = (b - v * v / 2) / v * v;
            rho = Sqr(t1 / T2);
            g1 = (Log(S / h) + (b + v * v / 2) * T2) / (v * Sqr(T2));
            g2 = g1 - v * Sqr(T2);
            g3 = g1 + 2 * Log(h / S) / (v * Sqr(T2));
            g4 = g3 - v * Sqr(T2);


            z1 = CND(e2) - Math.Pow(h / S, 2 * mu) * CND(e4);
            z2 = CND(-e2) - Math.Pow(h / S, 2 * mu) * CND(-e4);
            z3 = M(g2, e2, rho) - Math.Pow(h / S, 2 * mu) * M(g4, -e4, -rho);
            z4 = M(-g2, -e2, rho) - Math.Pow(h / S, 2 * mu) * M(-g4, e4, -rho);
            z5 = CND(e1) - Math.Pow(h / S, 2 * mu + 2) * CND(e3);
            z6 = CND(-e1) - Math.Pow(h / S, 2 * mu + 2) * CND(-e3);
            z7 = M(g1, e1, rho) - Math.Pow(h / S, 2 * mu + 2) * M(g3, -e3, -rho);
            z8 = M(-g1, -e1, rho) - Math.Pow(h / S, 2 * mu + 2) * M(-g3, e3, -rho);


            if (TypeFlag == "cdoA" | TypeFlag == "cuoA") // call down-and out and up-and-out type A
            {
                price = S * Exp((b - r) * T2) * (M(d1, eta * e1, eta * rho) - Math.Pow(h / S, 2 * mu + 2) * M(f1, eta * e3, eta * rho))
                - x * Exp(-r * T2) * (M(d2, eta * e2, eta * rho) - Math.Pow(h / S, 2 * mu) * M(f2, eta * e4, eta * rho));
            }
            else if (TypeFlag == "cdoB2" && x < h)// call down-and-out type B2
                price = S * Exp((b - r) * T2) * (M(g1, e1, rho) - Math.Pow(h / S, 2 * mu + 2) * M(g3, -e3, -rho))
                - x * Exp(-r * T2) * (M(g2, e2, rho) - Math.Pow(h / S, 2 * mu) * M(g4, -e4, -rho));
            else if (TypeFlag == "cdoB2" && x > h)
                price = PartialTimeBarrierOption("coB1", S, x, h, t1, T2, r, b, v);
            else if (TypeFlag == "cuoB2" && x < h)  // call up-and-out type B2
                price = S * Exp((b - r) * T2) * (M(-g1, -e1, rho) - Math.Pow(h / S, 2 * mu + 2) * M(-g3, e3, -rho))
                - x * Exp(-r * T2) * (M(-g2, -e2, rho) - Math.Pow(h / S, 2 * mu) * M(-g4, e4, -rho))
                - S * Exp((b - r) * T2) * (M(-d1, -e1, rho) - Math.Pow(h / S, 2 * mu + 2) * M(e3, -f1, -rho))
                + x * Exp(-r * T2) * (M(-d2, -e2, rho) - Math.Pow(h / S, 2 * mu) * M(e4, -f2, -rho));
            else if (TypeFlag == "coB1" && x > h)// call out type B1
                price = S * Exp((b - r) * T2) * (M(d1, e1, rho) - Math.Pow(h / S, 2 * mu + 2) * M(f1, -e3, -rho))
                - x * Exp(-r * T2) * (M(d2, e2, rho) - Math.Pow(h / S, 2 * mu) * M(f2, -e4, -rho));
            else if (TypeFlag == "coB1" && x < h)
                price = S * Exp((b - r) * T2) * (M(-g1, -e1, rho) - Math.Pow(h / S, 2 * mu + 2) * M(-g3, e3, -rho))
                - x * Exp(-r * T2) * (M(-g2, -e2, rho) - Math.Pow(h / S, 2 * mu) * M(-g4, e4, -rho))
               - S * Exp((b - r) * T2) * (M(-d1, -e1, rho) - Math.Pow(h / S, 2 * mu + 2) * M(-f1, e3, -rho))
               + x * Exp(-r * T2) * (M(-d2, -e2, rho) - Math.Pow(h / S, 2 * mu) * M(-f2, e4, -rho))
               + S * Exp((b - r) * T2) * (M(g1, e1, rho) - Math.Pow(h / S, 2 * mu + 2) * M(g3, -e3, -rho))
               - x * Exp(-r * T2) * (M(g2, e2, rho) - Math.Pow(h / S, 2 * mu) * M(g4, -e4, -rho));
            else if (TypeFlag == "pdoA")// put down-and out and up-and-out type A
            { price = PartialTimeBarrierOption("cdoA", S, x, h, t1, T2, r, b, v) - S * Exp((b - r) * T2) * z5 + x * Exp(-r * T2) * z1; }
            else if (TypeFlag == "puoA")
            { price = PartialTimeBarrierOption("cuoA", S, x, h, t1, T2, r, b, v) - S * Exp((b - r) * T2) * z6 + x * Exp(-r * T2) * z2; }
            else if (TypeFlag == "poB1")  // put out type B1
            { price = PartialTimeBarrierOption("coB1", S, x, h, t1, T2, r, b, v) - S * Exp((b - r) * T2) * z8 + x * Exp(-r * T2) * z4 - S * Exp((b - r) * T2) * z7 + x * Exp(-r * T2) * z3; }
            else if (TypeFlag == "pdoB2")// put down-and-out type B2
            { price = PartialTimeBarrierOption("cdoB2", S, x, h, t1, T2, r, b, v) - S * Exp((b - r) * T2) * z7 + x * Exp(-r * T2) * z3; }
            else if (TypeFlag == "puoB2")// put up-and-out type B2
            { price = PartialTimeBarrierOption("cuoB2", S, x, h, t1, T2, r, b, v) - S * Exp((b - r) * T2) * z8 + x * Exp(-r * T2) * z4; }

            return price;
        }

        public static double FDA_Delta(string TypeFlag, double S, double x, double h, double t1, double T2, double r, double b, double vol, double ds)
        {
            double delta = double.NaN;
            double bsr = PartialTimeBarrierOption(TypeFlag, S + ds, x, h, t1, T2, r, b, vol);
            double bs = PartialTimeBarrierOption(TypeFlag, S, x, h, t1, T2, r, b, vol);
            double bsl = PartialTimeBarrierOption(TypeFlag, S - ds, x, h, t1, T2, r, b, vol);
            delta = (bsr - bsl) / (2 * ds);
            return delta;
        }


        public static double FDA_GammaP(string TypeFlag, double S, double x, double h, double t1, double T2, double r, double b, double vol, double ds)
        {
            double gammap = double.NaN;
            double bsr = PartialTimeBarrierOption(TypeFlag, S + ds, x, h, t1, T2, r, b, vol);
            double bs = PartialTimeBarrierOption(TypeFlag, S, x, h, t1, T2, r, b, vol);
            double bsl = PartialTimeBarrierOption(TypeFlag, S - ds, x, h, t1, T2, r, b, vol);
            gammap = S / 100 * (bsr - 2 * bs + bsl) / (ds * ds);
            return gammap;
        }

        public static double FDA_Vega(string TypeFlag, double S, double x, double h, double t1, double T2, double r, double b, double vol, double ds)
        {
            double vega = double.NaN;
            double bsr = PartialTimeBarrierOption(TypeFlag, S, x, h, t1, T2, r, b, vol +0.01);
            double bsl = PartialTimeBarrierOption(TypeFlag, S, x, h, t1, T2, r, b, vol -0.01);
            vega = (bsr - bsl) / 2.0;
            return vega;
        }

        public static double FDA_Theta(string TypeFlag, double S, double x, double h, double t1, double T2, double r, double b, double vol, double ds)
        {
            double theta = double.NaN;
            double deltaT;
            if (t1 <= 1 / 252.0)
            {
                deltaT = 1 - 0.000005;
            }
            else
            {
                deltaT = 1 / 252.0;
            }
            double bsr = PartialTimeBarrierOption(TypeFlag, S, x, h, t1 - deltaT, T2, r, b, vol);
            double bsl = PartialTimeBarrierOption(TypeFlag, S, x, h, t1, T2, r, b, vol);
            theta = bsr - bsl;
            return theta;
        }


    }
}