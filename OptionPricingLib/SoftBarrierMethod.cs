using MathNet.Numerics.Distributions;
using System;


namespace OptionPricingLib
{
    public class SoftBarrierMethod
    {
        //Util Functions
        private static double Log(double X) { return Math.Log(X); }
        private static double CND(double X) { return Normal.CDF(0, 1, X); }
        private static double Exp(double X) { return Math.Exp(X); }
        private static double Sqr(double x) { return Math.Sqrt(x); }

        public static double SoftBarrierOption(string tpflag, double S,
                                                     double X, double L, double U, double T,
                                                     double r, double b, double vol)
        {
            int eta;
            double price = double.NaN;
            string OutInFlag;
            string CallPutFlag;
            OutInFlag = tpflag.Substring(1, 2);
            CallPutFlag = tpflag.Substring(0, 1);
            if (OutInFlag == "di" | OutInFlag == "ui")
            {
                if (CallPutFlag == "c") { eta = 1; }
                else { eta = -1; }
                double mu = (b + vol * vol / 2) / vol / vol;
                double lambda1 = Exp(-0.5 * vol * vol * T * (mu + 0.5) * (mu - 0.5));
                double lambda2 = Exp(-0.5 * vol * vol * T * (mu - 0.5) * (mu - 1.5));

                double d1 = Log(U * U / S / X) / vol / Sqr(T) + mu * vol * Sqr(T);
                double d2 = d1 - (mu + 0.5) * vol * Sqr(T);
                double d3 = Log(U * U / S / X) / vol / Sqr(T) + (mu - 1) * vol * Sqr(T);
                double d4 = d1 - (mu - 0.5) * vol * Sqr(T);
                double e1 = Log(L * L / S / X) / vol / Sqr(T) + mu * vol * Sqr(T);
                double e2 = e1 - (mu + 0.5) * vol * Sqr(T);
                double e3 = Log(L * L / S / X) / vol / Sqr(T) + (mu - 1) * vol * Sqr(T);
                double e4 = e3 - (mu - 0.5) * vol * Sqr(T);
                price = (eta * S * Exp((b - r) * T) * Math.Pow(S, -2 * mu) * Math.Pow(S * X, 0.5 + mu) / (2 * mu + 1) * (Math.Pow(U * U / S / X, mu + 0.5
                    ) * CND(eta * d1) - lambda1 * CND(eta * d2) - Math.Pow(L * L / S / X, mu + 0.5) * CND(eta * e1) + lambda1 * CND(eta * e2))
                    - eta * X * Exp(-r * T) * Math.Pow(S, -2 * mu + 2) * Math.Pow(S * X, -0.5 + mu) / (2 * mu - 1) * (Math.Pow(U * U / S / X, mu - 0.5
                    ) * CND(eta * d3) - lambda2 * CND(eta * d4) - Math.Pow(L * L / S / X, mu - 0.5) * CND(eta * e3) + lambda2 * CND(eta * e4))) / (U - L);
                return price;
            }
            else if (OutInFlag == "do" | OutInFlag == "uo")
            {
                if (CallPutFlag == "c") { eta = 1; }
                else { eta = -1; }
                double mu = (b + vol * vol / 2) / vol / vol;
                double lambda1 = Exp(-0.5 * vol * vol * T * (mu + 0.5) * (mu - 0.5));
                double lambda2 = Exp(-0.5 * vol * vol * T * (mu - 0.5) * (mu - 1.5));

                double d1 = Log(U * U / S / X) / vol / Sqr(T) + mu * vol * Sqr(T);
                double d2 = d1 - (mu + 0.5) * vol * Sqr(T);
                double d3 = Log(U * U / S / X) / vol / Sqr(T) + (mu - 1) * vol * Sqr(T);
                double d4 = d1 - (mu - 0.5) * vol * Sqr(T);
                double e1 = Log(L * L / S / X) / vol / Sqr(T) + mu * vol * Sqr(T);
                double e2 = e1 - (mu + 0.5) * vol * Sqr(T);
                double e3 = Log(L * L / S / X) / vol / Sqr(T) + (mu - 1) * vol * Sqr(T);
                double e4 = e3 - (mu - 0.5) * vol * Sqr(T);
                price = BlackScholesMethod.BlackScholes(CallPutFlag,S,X,T,r,b,vol) -(eta * S * Exp((b - r) * T) * Math.Pow(S, -2 * mu) * Math.Pow(S * X, 0.5 + mu) / (2 * mu + 1) * (Math.Pow(U * U / S / X, mu + 0.5
                    ) * CND(eta * d1) - lambda1 * CND(eta * d2) - Math.Pow(L * L / S / X, mu + 0.5) * CND(eta * e1) + lambda1 * CND(eta * e2))
                    - eta * X * Exp(-r * T) * Math.Pow(S, -2 * mu + 2) * Math.Pow(S * X, -0.5 + mu) / (2 * mu - 1) * (Math.Pow(U * U / S / X, mu - 0.5
                    ) * CND(eta * d3) - lambda2 * CND(eta * d4) - Math.Pow(L * L / S / X, mu - 0.5) * CND(eta * e3) + lambda2 * CND(eta * e4))) / (U - L);

                return price;
            }
            return price;


        }

        public static double FDA_Delta(string tpflag, double S,
                                                     double X, double L, double U, double T,
                                                     double r, double b, double vol, double ds)
        {
            double delta = double.NaN;
            double bsr = SoftBarrierOption(tpflag, S + ds, X, L, U, T, r, b, vol);
            double bs = SoftBarrierOption(tpflag, S, X, L, U, T, r, b, vol);
            double bsl = SoftBarrierOption(tpflag, S - ds, X, L, U, T, r, b, vol);
            delta = (bsr - bsl) / (2 * ds);
            return delta;
        }


        public static double FDA_GammaP(string tpflag, double S,
                                                     double X, double L, double U, double T,
                                                     double r, double b, double vol, double ds)
        {
            double gammap = double.NaN;
            double bsr = SoftBarrierOption(tpflag, S + ds, X, L, U, T, r, b, vol);
            double bs = SoftBarrierOption(tpflag, S, X, L, U, T, r, b, vol);
            double bsl = SoftBarrierOption(tpflag, S - ds, X, L, U, T, r, b, vol);
            gammap = S / 100 * (bsr - 2 * bs + bsl) / (ds * ds);
            return gammap;
        }

        public static double FDA_Vega(string tpflag, double S,
                                                     double X, double L, double U, double T,
                                                     double r, double b, double vol, double ds)
        {
            double vega = double.NaN;
            double bsr = SoftBarrierOption(tpflag, S, X, L, U, T, r, b, vol+0.01);
            double bsl = SoftBarrierOption(tpflag, S, X, L, U, T, r, b, vol-0.01);
            vega = (bsr - bsl) / 2.0;
            return vega;
        }

        public static double FDA_Theta(string tpflag, double S,
                                                     double X, double L, double U, double T,
                                                     double r, double b, double vol, double ds)
        {   double theta = double.NaN;
            double deltaT;
            if (T <= 1 / 252.0)
            {
                deltaT = 1 - 0.000005;
            }
            else
            {
                deltaT = 1 / 252.0;
            }
            double bsr = SoftBarrierOption(tpflag, S, X, L, U, T - deltaT, r, b, vol);
            double bsl = SoftBarrierOption(tpflag, S, X, L, U, T, r, b, vol);
            theta = bsr - bsl;
            return theta;
        }
    }
}