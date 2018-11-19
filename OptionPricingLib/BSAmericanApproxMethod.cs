
using MathNet.Numerics.Distributions;
using Accord.Statistics.Distributions.Multivariate;
using System;

namespace OptionPricingLib
{
    public class BSAmericanApproxMethod
    {
        public static double CBND(double X, double Y, double rho)
        {
            double[] _mean = new double[] { 0.0, 0.0 };
            double[,] _convariance = new double[,] { { 1.0, rho }, { rho, 1.0 } };
            var dist = new MultivariateNormalDistribution(_mean, _convariance);
            double[] para = { X, Y };
            double cdf = dist.DistributionFunction(para);
            return cdf;
        }

        public static double CND(double X)
        {
            return Normal.CDF(0, 1, X);
        }

        public static double ksi(double S, double T2, double gamma, double h, double I2, double I1, double t1, double r, double b, double v)
        {
            double ksi_value;
            double e1, e2, e3, e4;
            double f1, f2, f3, f4;
            double rho, kappa, lambda;
            e1 = (Math.Log(S / I1) + (b + (gamma - 0.5) * v * v) * t1) / (v * Math.Sqrt(t1));
            e2 = (Math.Log(I2 * I2 / (S * I1)) + (b + (gamma - 0.5) * v * v) * t1) / (v * Math.Sqrt(t1));
            e3 = (Math.Log(S / I1) - (b + (gamma - 0.5) * v * v) * t1) / (v * Math.Sqrt(t1));
            e4 = (Math.Log(I2 * I2 / (S * I1)) - (b + (gamma - 0.5) * v * v) * t1) / (v * Math.Sqrt(t1));
            f1 = (Math.Log(S / h) + (b + (gamma - 0.5) * v * v) * T2) / (v * Math.Sqrt(T2));
            f2 = (Math.Log(I2 * I2 / (S * h)) + (b + (gamma - 0.5) * v * v) * T2) / (v * Math.Sqrt(T2));
            f3 = (Math.Log(I1 * I1 / (S * h)) + (b + (gamma - 0.5) * v * v) * T2) / (v * Math.Sqrt(T2));
            f4 = (Math.Log(S * I1 * I1 / (h * I2 * I2)) + (b + (gamma - 0.5) * v * v) * T2) / (v * Math.Sqrt(T2));
            rho = Math.Sqrt(t1 / T2);
            lambda = -r + gamma * b + 0.5 * gamma * (gamma - 1) * v * v;
            kappa = 2 * b / (v * v) + (2 * gamma - 1);

            ksi_value = Math.Exp(lambda * T2) * Math.Pow(S, gamma) * (CBND(-e1, -f1, rho)
                 - Math.Pow(I2 / S, kappa) * CBND(-e2, -f2, rho)
                 - Math.Pow(I1 / S, kappa) * CBND(-e3, -f3, -rho)
                 + Math.Pow(I1 / I2, kappa) * CBND(-e4, -f4, -rho));
            return ksi_value;
        }

        public static double phi(double S, double T, double gamma, double h, double i, double r, double b, double v)
        {
            double phi_value;
            double lamda, kappa;
            double d;

            lamda = (-r + gamma * b + 0.5 * gamma * (gamma - 1) * v * v) * T;
            d = -(Math.Log(S / h) + (b + (gamma - 0.5) * v * v) * T) / (v * Math.Sqrt(T));
            kappa = 2 * b / (v * v) + (2 * gamma - 1);

            phi_value = Math.Exp(lamda) * Math.Pow(S, gamma) * (CND(d) - Math.Pow(i / S, kappa) * CND(d - 2 * Math.Log(i / S) / (v * Math.Sqrt(T))));
            return phi_value;
        }

        public static double BSAmericanCallApprox2002(double S, double X, double T, double r, double b, double v)
        {
            double option_price = double.NaN;
            double t1 = 0.5 * (Math.Sqrt(5) - 1) * T;
            if (b >= r)//Never optimal to exercise before maturity
            {
                option_price = BlackScholesMethod.BlackScholes("c", S, X, T, r, b, v);
            }
            else
            {

                double beta = (0.5 - b / (v * v)) + Math.Sqrt(Math.Pow((b / (v * v) - 0.5), 2) + 2 * r / (v * v));
                double binfinity = beta / (beta - 1) * X;
                double b0 = Math.Max(X, r / (r - b) * X);

                double ht1 = -(b * t1 + 2 * v * Math.Sqrt(t1)) * X * X / ((binfinity - b0) * b0);
                double ht2 = -(b * T + 2 * v * Math.Sqrt(T)) * X * X / ((binfinity - b0) * b0);
                double I1 = b0 + (binfinity - b0) * (1 - Math.Exp(ht1));
                double I2 = b0 + (binfinity - b0) * (1 - Math.Exp(ht2));
                double alfa1 = (I1 - X) * Math.Pow(I1, -beta);
                double alfa2 = (I2 - X) * Math.Pow(I2, -beta);
                if (S >= I2)
                {
                    option_price = S - X;
                }
                else
                {
                    option_price = alfa2 * Math.Pow(S, beta) - alfa2 * phi(S, t1, beta, I2, I2, r, b, v)
                                    + phi(S, t1, 1, I2, I2, r, b, v) - phi(S, t1, 1, I1, I2, r, b, v)
                                    - X * phi(S, t1, 0, I2, I2, r, b, v) + X * phi(S, t1, 0, I1, I2, r, b, v)
                                    + alfa1 * phi(S, t1, beta, I1, I2, r, b, v) - alfa1 * ksi(S, T, beta, I1, I2, I1, t1, r, b, v)
                                    + ksi(S, T, 1, I1, I2, I1, t1, r, b, v) - ksi(S, T, 1, X, I2, I1, t1, r, b, v)
                                    - X * ksi(S, T, 0, I1, I2, I1, t1, r, b, v) + X * ksi(S, T, 0, X, I2, I1, t1, r, b, v);
                }
            }
            return option_price;
        }

        public static double BSAmericanPutApprox2002(double S, double X, double T, double r, double b, double v)
        {
            double option_price = double.NaN;
            option_price = BSAmericanCallApprox2002(X, S, T, r - b, -b, v);
            return option_price;
        }

        public static double BSAmericanApprox2002(string cpflg, double S, double X, double T, double r, double b, double v)
        {
            double option_price = double.NaN;
            if (T == 0)
            {
                if (cpflg.Equals("c"))
                {
                    option_price = S - X;
                }
                else
                {
                    option_price = X - S;
                }
                return option_price;
            }
            if (cpflg.Equals("c"))
            {
                option_price = BSAmericanCallApprox2002(S, X, T, r, b, v);
            }
            else if (cpflg.Equals("p"))
            {
                option_price = BSAmericanPutApprox2002(S, X, T, r, b, v);
            }
            else
            {
                option_price = double.NaN;
            }
            return option_price;
        }


        public static double FDA_Delta(string cpflg, double S, double X, double T, double r, double b, double v, double ds)
        {
            double delta = double.NaN;
            double bsr = BSAmericanApprox2002(cpflg, S + ds, X, T, r, b, v);
            double bs = BSAmericanApprox2002(cpflg, S, X, T, r, b, v);
            double bsl = BSAmericanApprox2002(cpflg, S - ds, X, T, r, b, v);
            delta = (bsr - bsl) / (2 * ds);
            return delta;
        }

        public static double FDA_DeltaR(string cpflg, double S, double X, double T, double r, double b, double v, double ds)
        {
            double delta = double.NaN;
            double bsr = BSAmericanApprox2002(cpflg, S + ds, X, T, r, b, v);
            double bs = BSAmericanApprox2002(cpflg, S, X, T, r, b, v);
            double bsl = BSAmericanApprox2002(cpflg, S - ds, X, T, r, b, v);
            delta = (bsr - bs) / (ds);
            return delta;
        }

        public static double FDA_DeltaL(string cpflg, double S, double X, double T, double r, double b, double v, double ds)
        {
            double delta = double.NaN;
            double bsr = BSAmericanApprox2002(cpflg, S + ds, X, T, r, b, v);
            double bs = BSAmericanApprox2002(cpflg, S, X, T, r, b, v);
            double bsl = BSAmericanApprox2002(cpflg, S - ds, X, T, r, b, v);
            delta = (bs - bsl) / (ds);
            return delta;
        }

        public static double FDA_GammaP(string cpflg, double S, double X, double T, double r, double b, double v, double ds)
        {
            double gammap = double.NaN;
            double bsr = BSAmericanApprox2002(cpflg, S + ds, X, T, r, b, v);
            double bs = BSAmericanApprox2002(cpflg, S, X, T, r, b, v);
            double bsl = BSAmericanApprox2002(cpflg, S - ds, X, T, r, b, v);
            gammap = S / 100 * (bsr - 2 * bs + bsl) / (ds * ds);
            return gammap;
        }

        public static double FDA_Vega(string cpflg, double S, double X, double T, double r, double b, double v, double ds)
        {
            double vega = double.NaN;
            double bsr = BSAmericanApprox2002(cpflg, S, X, T, r, b, v + 0.01);
            double bsl = BSAmericanApprox2002(cpflg, S, X, T, r, b, v - 0.01);
            vega = (bsr - bsl) / 2.0;
            return vega;
        }

        public static double FDA_Theta(string cpflg, double S, double X, double T, double r, double b, double v, double ds)
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
            double bsr = BSAmericanApprox2002(cpflg, S, X, T - deltaT, r, b, v);
            double bsl = BSAmericanApprox2002(cpflg, S, X, T, r, b, v);
            theta = bsr - bsl;
            return theta;
        }
    }

}
