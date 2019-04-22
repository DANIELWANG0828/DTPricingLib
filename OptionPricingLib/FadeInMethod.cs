using MathNet.Numerics.Distributions;
using MathNet.Numerics.Integration;
using Accord.Statistics.Distributions.Multivariate;
using System;

namespace OptionPricingLib
{
    public class FadeInMethod
    {
        private static double M(double x, double y, double rho)
        { return MultivariateNormalDistribution.Bivariate(0, 0, 1, 1, rho).DistributionFunction(new double[] { x , y}); }

        public static double FadeIn(string cpflg, double S, double X, double L,double U, double n, double T, double r, double b, double vol)
        {
            double price = double.NaN;
            double t1,rho;
            for (int i = 1; i <= n; i++)
            {
                t1 = i * T / n;
                rho = Math.Sqrt(t1 / T);
                double d1 = (Math.Log(S / X) + (b + vol * vol / 2) * T) / (vol * Math.Sqrt(T));
                double d2 = d1 - vol * Math.Sqrt(T);
                double d3 = (Math.Log(S / L) + (b + vol * vol / 2) * t1) / (vol * Math.Sqrt(n));
                double d4 = d3 - vol * Math.Sqrt(t1);
                double d5 = (Math.Log(S / U) + (b + vol * vol / 2) * t1) / (vol * Math.Sqrt(n));
                double d6 = d5 - vol * Math.Sqrt(t1);
                if (cpflg.Equals("c"))
                {
                    price += (S * Math.Exp((b - r) * T) * (M(-d5, d1, -rho) - M(-d3, d1, -rho)) - X * Math.Exp(-r * T) * (M(-d6, d2, -rho) - M(-d4, d2, -rho))) / n;
                }
                if (cpflg.Equals("p"))
                {
                    price += (-S * Math.Exp((b - r) * T) * (M(-d5, -d1, rho) - M(-d3, -d1, rho)) + X * Math.Exp(-r * T) * (M(-d6, -d2, rho) - M(-d4, -d2, rho))) / n;
                }
                
            }
         
            return price;
        }

        public static double FDA_Delta(string cpflg, double S, double X, double L, double U, double n, double T, double r, double b, double vol, double ds)
        {
            double delta = double.NaN;
            double bsr = FadeIn(cpflg, S + ds, X, L, U, n, T, r, b, vol);
            double bs = FadeIn(cpflg, S, X, L, U, n, T, r, b, vol);
            double bsl = FadeIn(cpflg, S - ds, X, L, U, n, T, r, b, vol);
            delta = (bsr - bsl) / (2 * ds);
            return delta;
        }

        public static double FDA_DeltaR(string cpflg, double S, double X, double L,double U, double n, double T, double r, double b, double vol,double ds)
        {
            double delta = double.NaN;
            double bsr = FadeIn(cpflg, S + ds, X, L, U, n, T, r, b, vol);
            double bs = FadeIn(cpflg, S, X, L, U, n, T, r, b, vol);
            double bsl = FadeIn(cpflg, S - ds, X, L, U, n, T, r, b, vol);
            delta = (bsr - bs) / (ds);
            return delta;
        }

        public static double FDA_DeltaL(string cpflg, double S, double X, double L,double U, double n, double T, double r, double b, double vol,double ds)
        {
            double delta = double.NaN;
            double bsr = FadeIn(cpflg, S + ds, X, L, U, n, T, r, b, vol);
            double bs = FadeIn(cpflg, S, X, L, U, n, T, r, b, vol);
            double bsl = FadeIn(cpflg, S - ds, X, L, U, n, T, r, b, vol);
            delta = (bs - bsl) / (ds);
            return delta;
        }

        public static double FDA_GammaP(string cpflg, double S, double X, double L,double U, double n, double T, double r, double b, double vol,double ds)
        {
            double gammap = double.NaN;
            double bsr = FadeIn(cpflg, S + ds, X, L, U, n, T, r, b, vol);
            double bs = FadeIn(cpflg, S, X, L, U, n, T, r, b, vol);
            double bsl = FadeIn(cpflg, S - ds, X, L, U, n, T, r, b, vol);
            gammap = S / 100 * (bsr - 2 * bs + bsl) / (ds * ds);
            return gammap;
        }

        public static double FDA_Vega(string cpflg, double S, double X, double L,double U, double n, double T, double r, double b, double vol,double ds)
        {
            double vega = double.NaN;
            double bsr = FadeIn(cpflg, S, X, L, U, n, T, r, b, vol + 0.01);
            double bsl = FadeIn(cpflg, S, X, L, U, n, T, r, b, vol - 0.01);
            vega = (bsr - bsl) / 2.0;
            return vega;
        }

        public static double FDA_Theta(string cpflg, double S, double X, double L,double U, double n, double T, double r, double b, double vol,double ds)
        {
            double theta = double.NaN;
            double deltaT;
            if (T <= 1 / 252)
            {
                deltaT = 1 - 0.000005;
            }
            else
            {
                deltaT = 1 / 252;
            }
            double bsr = FadeIn(cpflg, S, X, L, U, n, T-deltaT, r, b, vol);
            double bsl = FadeIn(cpflg, S, X, L, U, n, T, r, b, vol);
            theta = bsr - bsl;
            return theta;
        }
    }
}