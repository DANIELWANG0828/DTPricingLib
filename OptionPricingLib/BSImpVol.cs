using System;

namespace OptionPricingLib
{
    public class BSImpVol
    {
        public static double BSImpVolBisec(string cpflg, double S,
                            double x, double T, double r, double b, double cm, double epsilon)
        {

            double vLow, vHigh, vi;
            double cLow, cHigh;
            double counter;

            vLow = 0.005;
            vHigh = 4;
            cLow = BlackScholesMethod.BlackScholes(cpflg, S, x, T, r, b, vLow);
            cHigh = BlackScholesMethod.BlackScholes(cpflg, S, x, T, r, b, vHigh);
            counter = 0;
            vi = vLow + (cm - cLow) * (vHigh - vLow) / (cHigh - cLow);
            while (Math.Abs(cm - BlackScholesMethod.BlackScholes(cpflg, S, x, T, r, b, vi)) > epsilon)
            {
                counter = counter + 1;
                if (counter == 100)
                {
                    return double.NaN;
                }

                if (BlackScholesMethod.BlackScholes(cpflg, S, x, T, r, b, vi) < cm)
                {
                    vLow = vi;
                }
                else
                {
                    vHigh = vi;
                }
                cLow = BlackScholesMethod.BlackScholes(cpflg, S, x, T, r, b, vLow);
                cHigh = BlackScholesMethod.BlackScholes(cpflg, S, x, T, r, b, vHigh);
                vi = vLow + (cm - cLow) * (vHigh - vLow) / (cHigh - cLow);
            }
            return vi;
        }

        public static double BSImpVolNR(string cpflg, double S, double x,
            double T, double r, double b, double cm, double epsilon)
        {
            double vi, ci;
            double vegai;
            double minDiff;

            //Manaster and Koehler seed value (vi)
            vi = Math.Sqrt(Math.Abs(Math.Log(S / x) + r * T) * 2 / T);
            ci = BlackScholesMethod.BlackScholes(cpflg, S, x, T, r, b, vi);
            vegai = BlackScholesMethod.Vega(S, x, T, r, b, vi);
            minDiff = Math.Abs(cm - ci);


            while (Math.Abs(cm - ci) >= epsilon && Math.Abs(cm - ci) <= minDiff)
            {
                vi = vi - (ci - cm) / vegai;
                ci = BlackScholesMethod.BlackScholes(cpflg, S, x, T, r, b, vi);
                vegai = BlackScholesMethod.Vega(S, x, T, r, b, vi);
                minDiff = Math.Abs(cm - ci);
            }
            if (Math.Abs(cm - ci) < epsilon)
            {
                return vi;
            }
            else
            {
                return double.NaN;
            }
        }
    }
}
