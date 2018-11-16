using MathNet.Numerics.Distributions;
using System;

namespace OptionPricingLib
{
    public class StandardBarrierMethod
    {


        //Util Functions
        private static double Log(double X) { return Math.Log(X); }
        private static double CND(double X) { return Normal.CDF(0, 1, X); }
        private static double Exp(double X) { return Math.Exp(X); }
        private static double Sqr(double x) { return Math.Sqrt(x); }



        public static double StandardBarrierOption(string tpflag, double S,
                                                     double X, double H, double k, double T,
                                                     double r, double b, double v)
        {
            double price = double.NaN;
            double mu;
            double lambda;
            double X1, X2;
            double y1, y2;
            double z;

            int eta;
            int phi;

            double f1, f2, f3, f4, f5, f6;

            mu = (b - v * v / 2) / (v * v);
            lambda = Sqr(mu * mu + 2 * r / (v * v));

            X1 = Log(S / X) / (v * Sqr(T)) + (1 + mu) * v * Sqr(T);
            X2 = Log(S / H) / (v * Sqr(T)) + (1 + mu) * v * Sqr(T);
            y1 = Log(H * H / (S * X)) / (v * Sqr(T)) + (1 + mu) * v * Sqr(T);
            y2 = Log(H / S) / (v * Sqr(T)) + (1 + mu) * v * Sqr(T);
            z = Log(H / S) / (v * Sqr(T)) + lambda * v * Sqr(T);

            if (tpflag == "cdi" || tpflag == "cdo")
            {
                eta = 1;
                phi = 1;
            }
            else if (tpflag == "cui" || tpflag == "cuo")
            {
                eta = -1;
                phi = 1;
            }
            else if (tpflag == "pdi" || tpflag == "pdo")
            {
                eta = 1;
                phi = -1;
            }
            else if (tpflag == "pui" || tpflag == "puo")
            {
                eta = -1;
                phi = -1;
            }
            else
            {
                eta = 0;
                phi = 0;
            }

            f1 = phi * S * Exp((b - r) * T) * CND(phi * X1) - phi * X * Exp(-r * T) * CND(phi * X1 - phi * v * Sqr(T));
            f2 = phi * S * Exp((b - r) * T) * CND(phi * X2) - phi * X * Exp(-r * T) * CND(phi * X2 - phi * v * Sqr(T));
            f3 = phi * S * Exp((b - r) * T) * Math.Pow(H / S, 2 * (mu + 1)) * CND(eta * y1) - phi * X * Exp(-r * T) * Math.Pow(H / S, 2 * mu) * CND(eta * y1 - eta * v * Sqr(T));
            f4 = phi * S * Exp((b - r) * T) * Math.Pow(H / S, 2 * (mu + 1)) * CND(eta * y2) - phi * X * Exp(-r * T) * Math.Pow(H / S, 2 * mu) * CND(eta * y2 - eta * v * Sqr(T));
            f5 = k * Exp(-r * T) * (CND(eta * X2 - eta * v * Sqr(T)) - Math.Pow(H / S, 2 * mu) * CND(eta * y2 - eta * v * Sqr(T)));
            f6 = k * (Math.Pow(H / S, mu + lambda) * CND(eta * z) + Math.Pow(H / S, mu - lambda) * CND(eta * z - 2 * eta * lambda * v * Sqr(T)));

            if (X > H)
            {
                switch (tpflag)
                {
                    case "cdi":
                        price = f3 + f5;
                        break;
                    case "cui":
                        price = f1 + f5;
                        break;
                    case "pdi":
                        price = f2 - f3 + f4 + f5;
                        break;
                    case "pui":
                        price = f1 - f2 + f4 + f5;
                        break;
                    case "cdo":
                        price = f1 - f3 + f6;
                        break;
                    case "cuo":
                        price = f6;
                        break;
                    case "pdo":
                        price = f1 - f2 + f3 - f4 + f6;
                        break;
                    case "puo":
                        price = f2 - f4 + f6;
                        break;
                }
            }
            else if (X < H)
            {
                switch (tpflag)
                {
                    case "cdi":
                        price = f1 - f2 + f4 + f5;
                        break;
                    case "cui":
                        price = f2 - f3 + f4 + f5;
                        break;
                    case "pdi":
                        price = f1 + f5;
                        break;
                    case "pui":
                        price = f3 + f5;
                        break;
                    case "cdo":
                        price = f2 + f6 - f4;
                        break;
                    case "cuo":
                        price = f1 - f2 + f3 - f4 + f6;
                        break;
                    case "pdo":
                        price = f6;
                        break;
                    case "puo":
                        price = f1 - f3 + f6;
                        break;
                }
            }
            else
            {
                price = double.NaN;
            }


            return price;
        }

        public static double FDA_Delta(string tpflag, double S, double X, double H, double k, double T,
                                       double r, double b, double v, double ds)
        {
            double delta = double.NaN;
            double bsr = StandardBarrierOption(tpflag, S + ds, X, H, k, T, r, b, v);
            double bs = StandardBarrierOption(tpflag, S, X, H, k, T, r, b, v);
            double bsl = StandardBarrierOption(tpflag, S - ds, X, H, k, T, r, b, v);
            delta = (bsr - bsl) / (2 * ds);
            return delta;
        }

        public static double FDA_DeltaR(string tpflag, double S, double X, double H, double k, double T,
                                       double r, double b, double v, double ds)
        {
            double delta = double.NaN;
            double bsr = StandardBarrierOption(tpflag, S + ds, X, H, k, T, r, b, v);
            double bs = StandardBarrierOption(tpflag, S, X, H, k, T, r, b, v);
            double bsl = StandardBarrierOption(tpflag, S - ds, X, H, k, T, r, b, v);
            delta = (bsr - bs) / (ds);
            return delta;
        }

        public static double FDA_DeltaL(string tpflag, double S, double X, double H, double k, double T,
                                       double r, double b, double v, double ds)
        {
            double delta = double.NaN;
            double bsr = StandardBarrierOption(tpflag, S + ds, X, H, k, T, r, b, v);
            double bs = StandardBarrierOption(tpflag, S, X, H, k, T, r, b, v);
            double bsl = StandardBarrierOption(tpflag, S - ds, X, H, k, T, r, b, v);
            delta = (bs - bsl) / (ds);
            return delta;
        }

        public static double FDA_GammaP(string tpflag, double S, double X, double H, double k, double T,
                                       double r, double b, double v, double ds)
        {
            double gammap = double.NaN;
            double bsr = StandardBarrierOption(tpflag, S + ds, X, H, k, T, r, b, v);
            double bs = StandardBarrierOption(tpflag, S, X, H, k, T, r, b, v);
            double bsl = StandardBarrierOption(tpflag, S - ds, X, H, k, T, r, b, v);
            gammap = S / 100 * (bsr - 2 * bs + bsl) / (ds * ds);
            return gammap;
        }

        public static double FDA_Vega(string tpflag, double S, double X, double H, double k, double T,
                                       double r, double b, double v, double ds)
        {
            double vega = double.NaN;
            double bsr = StandardBarrierOption(tpflag, S, X, H, k, T, r, b, v + 0.01);
            double bsl = StandardBarrierOption(tpflag, S, X, H, k, T, r, b, v - 0.01);
            vega = (bsr - bsl) / 2.0;
            return vega;
        }

        public static double FDA_Theta(string tpflag, double S, double X, double H, double k, double T,
                                       double r, double b, double v, double ds)
        {
            double theta = double.NaN;
            double deltaT;
            if (T <= 1 / 365.0)
            {
                deltaT = 1 - 0.000005;
            }
            else
            {
                deltaT = 1 / 365.0;
            }
            double bsr = StandardBarrierOption(tpflag, S, X, H, k, T - deltaT, r, b, v);
            double bsl = StandardBarrierOption(tpflag, S, X, H, k, T, r, b, v);
            theta = bsr - bsl;
            return theta;
        }



    }
}
