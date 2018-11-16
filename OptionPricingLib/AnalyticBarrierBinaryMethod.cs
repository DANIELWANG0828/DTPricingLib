using MathNet.Numerics.Distributions;
using System;


namespace OptionPricingLib
{
    //[1] Down-and-in cash-(at-hit)-or-nothing(S>H)
    //[2] Up-and-in cash-(at-hit)-or-nothing(S<H)
    //[3] Down-and-in asset-(at-hit)-or-nothing(K= H) (S>H)
    //[4] Up-and-in asset-(at-hit)-or-nothing(K= H)(S<H)
    //[5] Down-and-in cash-(at-expiry)-or-nothing(S>H)
    //[6] Up-and-in cash-(at-expiry)-or-nothing(S<H)
    //[7]Down-and-in asset-(at-expiry)-or-nothing(S>H)
    //[8] Up-and-in asset-(at-expiry)-or-nothing(S<H)
    //[9] Down-and-out cash-(at-expiry)-or-nothing(S>H)
    //[10]Up-and-out cash-(at-expiry)-or-nothing(S<H)
    //[11] Down-and-out asset-(at-expiry)-or-nothing(S>H)
    //[12] Up-and-out asset-(at-expiry)-or-nothing(S<H)
    //[13]Down-and-in cash-(at-expiry)-or-nothing call(S>H)
    //[14] Up-and-in cash-(at-expiry)-or-nothing call(S<H)
    //[15] Down-and-in asset-(at-expiry)-or-nothing call(S>H)
    //[16] Up-and-in asset-(at-expiry)-or-nothing call(S<H)
    //[17] Down-and-in cash-(at-expiry)-or-nothing put(S>H)
    //[18]Up-and-in cash-(at-expiry)-or-nothing put(S<H)
    //[19] Down-and-in asset-(at-expiry)-or-nothing put(S>H)
    //[20] Up-and-in asset-(at-expiry)-or-nothing put(S<H)
    //[21] Down-and-out cash-(at-expiry)-or-nothing call(S>H)
    //[22] Up-and-out cash-(at-expiry)-or-nothing call(S<H)
    //[23] Down-and-out asset-(at-expiry)-or-nothing call(S>H)
    //[24] Up-and-out asset-(at-expiry)-or-nothing call(S<H)
    //[25] Down-and-out cash-(at-expiry)-or-nothing put(S>H)
    //[26]Up-and-out cash-(at-expiry)-or-nothing put(S<H)
    //[27]Down-and-out asset-(at-expiry)-or-nothing put(S>H)
    //[28] Up-and-out asset-(at-expiry)-or-nothing put(S<H)
    public class AnalyticBarrierBinaryMethod
    {   /*
        public void main()
        {
            Console.WriteLine("1");
            string a = Console.ReadLine();
        }
        */


        //Util Functions
        private static double Log(double X) {return Math.Log(X);}
        private static double CND(double X) {return Normal.CDF(0, 1, X);}
        private static double Exp(double X) {return Math.Exp(X); }
        private static double Sqr(double x) {return Math.Sqrt(x); }

        public static double BarrierBinary(string TypeFlag, double S, double x, double h,
                                                       double k, double T, double r, double b, double v)
        {
            double price = double.NaN;
            double X1, X2;
            double y1, y2;
            double Z, mu, lambda;
            double a1, a2, a3, a4, a5;
            double b1, b2, b3, b4;
            int _TypeFlag = 0;
            int eta, phi;
            eta = 0;
            phi = 0;

            switch (TypeFlag)
            {
                //[1] Down-and-in cash-(at-hit)-or-nothing(S>H)
                case "hit_cash_di":
                    _TypeFlag = 1;
                    eta = 1;
                    break;
                //[2] Up-and-in cash-(at-hit)-or-nothing(S<H)
                case "hit_cash_ui":
                    _TypeFlag = 2;
                    eta = -1;
                    break;
                //[3] Down-and-in asset-(at-hit)-or-nothing(K= H) (S>H)
                case "hit_asset_di":
                    _TypeFlag = 3;
                    eta = 1;
                    break;
                //[4] Up-and-in asset-(at-hit)-or-nothing(K= H)(S<H)
                case "hit_asset_ui":
                    _TypeFlag = 4;
                    eta = -1;
                    break;
                //[5] Down-and-in cash-(at-expiry)-or-nothing(S>H)
                case "exp_cash_di":
                    _TypeFlag = 5;
                    eta = 1;
                    phi = -1;
                    break;
                //[6] Up-and-in cash-(at-expiry)-or-nothing(S<H)
                case "exp_cash_ui":
                    _TypeFlag = 6;
                    eta = -1;
                    phi = 1;
                    break;
                //[7]Down-and-in asset-(at-expiry)-or-nothing(S>H)
                case "exp_asset_di":
                    _TypeFlag = 7;
                    eta = 1;
                    phi = -1;
                    break;
                //[8] Up-and-in asset-(at-expiry)-or-nothing(S<H)
                case "exp_asset_ui":
                    _TypeFlag = 8;
                    eta = -1;
                    phi = 1;
                    break;
                //[9] Down-and-out cash-(at-expiry)-or-nothing(S>H)
                case "exp_cash_do":
                    _TypeFlag = 9;
                    eta = 1;
                    phi = 1;
                    break;
                //[10]Up-and-out cash-(at-expiry)-or-nothing(S<H)
                case "exp_cash_uo":
                    _TypeFlag = 10;
                    eta = -1;
                    phi = -1;
                    break;
                //[11] Down-and-out asset-(at-expiry)-or-nothing(S>H)
                case "exp_asset_do":
                    _TypeFlag = 11;
                    eta = 1;
                    phi = 1;
                    break;
                //[12] Up-and-out asset-(at-expiry)-or-nothing(S<H)
                case "exp_asset_uo":
                    _TypeFlag = 12;
                    eta = -1;
                    phi = -1;
                    break;
                //[13]Down-and-in cash-(at-expiry)-or-nothing call(S>H)
                case "exp_cash_call_di":
                    _TypeFlag = 13;
                    eta = 1;
                    phi = 1;
                    break;
                //[14] Up-and-in cash-(at-expiry)-or-nothing call(S<H)
                case "exp_cash_call_ui":
                    _TypeFlag = 14;
                    eta = -1;
                    phi = 1;
                    break;
                //[15] Down-and-in asset-(at-expiry)-or-nothing call(S>H)
                case "exp_asset_call_di":
                    _TypeFlag = 15;
                    eta = 1;
                    phi = 1;
                    break;
                //[16] Up-and-in asset-(at-expiry)-or-nothing call(S<H)
                case "exp_asset_call_ui":
                    _TypeFlag = 16;
                    eta = -1;
                    phi = 1;
                    break;
                //[17] Down-and-in cash-(at-expiry)-or-nothing put(S>H)
                case "exp_cash_put_di":
                    _TypeFlag = 17;
                    eta = 1;
                    phi = -1;
                    break;
                //[18]Up-and-in cash-(at-expiry)-or-nothing put(S<H)
                case "exp_cash_put_ui":
                    _TypeFlag = 18;
                    eta = -1;
                    phi = -1;
                    break;
                //[19] Down-and-in asset-(at-expiry)-or-nothing put(S>H)
                case "exp_asset_put_di":
                    _TypeFlag = 19;
                    eta = 1;
                    phi = -1;
                    break;
                //[20] Up-and-in asset-(at-expiry)-or-nothing put(S<H)
                case "exp_asset_put_ui":
                    _TypeFlag = 20;
                    eta = -1;
                    phi = -1;
                    break;
                //[21] Down-and-out cash-(at-expiry)-or-nothing call(S>H)
                case "exp_cash_call_do":
                    _TypeFlag = 21;
                    eta = 1;
                    phi = 1;
                    break;
                //[22] Up-and-out cash-(at-expiry)-or-nothing call(S<H)
                case "exp_cash_call_uo":
                    _TypeFlag = 22;
                    eta = -1;
                    phi = 1;
                    break;
                //[23] Down-and-out asset-(at-expiry)-or-nothing call(S>H)
                case "exp_asset_call_do":
                    _TypeFlag = 23;
                    eta = 1;
                    phi = 1;
                    break;
                //[24] Up-and-out asset-(at-expiry)-or-nothing call(S<H)
                case "exp_asset_call_uo":
                    _TypeFlag = 24;
                    eta = -1;
                    phi = 1;
                    break;
                //[25] Down-and-out cash-(at-expiry)-or-nothing put(S>H)
                case "exp_cash_put_do":
                    _TypeFlag = 25;
                    eta = 1;
                    phi = -1;
                    break;
                //[26]Up-and-out cash-(at-expiry)-or-nothing put(S<H)
                case "exp_cash_put_uo":
                    _TypeFlag = 26;
                    eta = -1;
                    phi = -1;
                    break;
                //[27]Down-and-out asset-(at-expiry)-or-nothing put(S>H)
                case "exp_asset_put_do":
                    _TypeFlag = 27;
                    eta = 1;
                    phi = -1;
                    break;
                //[28] Up-and-out asset-(at-expiry)-or-nothing put(S<H)
                case "exp_asset_put_uo":
                    _TypeFlag = 28;
                    eta = -1;
                    phi = -1;
                    break;
            }

            mu = (b - v * v / 2) / (v * v);
            lambda = Sqr(mu * mu + 2 * r / (v * v));
            X1 = Log(S / x) / (v * Sqr(T)) + (mu + 1) * v * Sqr(T);
            X2 = Log(S / h) / (v * Sqr(T)) + (mu + 1) * v * Sqr(T);
            y1 = Log(h * h / (S * x)) / (v * Sqr(T)) + (mu + 1) * v * Sqr(T);
            y2 = Log(h / S) / (v * Sqr(T)) + (mu + 1) * v * Sqr(T);
            Z = Log(h / S) / (v * Sqr(T)) + lambda * v * Sqr(T);


            a1 = S * Exp((b - r) * T) * CND(phi * X1);
            b1 = k * Exp(-r * T) * CND(phi * X1 - phi * v * Sqr(T));
            a2 = S * Exp((b - r) * T) * CND(phi * X2);
            b2 = k * Exp(-r * T) * CND(phi * X2 - phi * v * Sqr(T));
            a3 = S * Exp((b - r) * T) * Math.Pow(h / S, 2 * (mu + 1)) * CND(eta * y1);
            b3 = k * Exp(-r * T) * Math.Pow(h / S, 2 * mu) * CND(eta * y1 - eta * v * Sqr(T));
            a4 = S * Exp((b - r) * T) * Math.Pow(h / S, 2 * (mu + 1)) * CND(eta * y2);
            b4 = k * Exp(-r * T) * Math.Pow(h / S, 2 * mu) * CND(eta * y2 - eta * v * Sqr(T));
            a5 = k * (Math.Pow(h / S, mu + lambda) * CND(eta * Z) + Math.Pow(h / S, mu - lambda) * CND(eta * Z - 2 * eta * lambda * v * Sqr(T)));


            if (x > h)
            {
                if (_TypeFlag < 5)
                    price = a5;
                else if (_TypeFlag < 7)
                    price = b2 + b4;
                else if (_TypeFlag < 9)
                    price = a2 + a4;
                else if (_TypeFlag < 11)
                    price = b2 - b4;
                else if (_TypeFlag < 13)
                    price = a2 - a4;
                else if (_TypeFlag == 13)
                    price = b3;
                else if (_TypeFlag == 14)
                    price = b3;
                else if (_TypeFlag == 15)
                    price = a3;
                else if (_TypeFlag == 16)
                    price = a1;
                else if (_TypeFlag == 17)
                    price = b2 - b3 + b4;
                else if (_TypeFlag == 18)
                    price = b1 - b2 + b4;
                else if (_TypeFlag == 19)
                    price = a2 - a3 + a4;
                else if (_TypeFlag == 20)
                    price = a1 - a2 + a3;
                else if (_TypeFlag == 21)
                    price = b1 - b3;
                else if (_TypeFlag == 22)
                    price = 0;
                else if (_TypeFlag == 23)
                    price = a1 - a3;
                else if (_TypeFlag == 24)
                    price = 0;
                else if (_TypeFlag == 25)
                    price = b1 - b2 + b3 - b4;
                else if (_TypeFlag == 26)
                    price = b2 - b4;
                else if (_TypeFlag == 27)
                    price = a1 - a2 + a3 - a4;
                else if (_TypeFlag == 28)
                    price = a2 - a4;
                else
                    price = double.NaN;
            }
            else if (x < h)
            {
                if (_TypeFlag < 5)
                    price = a5;
                else if (_TypeFlag < 7)
                    price = b2 + b4;
                else if (_TypeFlag < 9)
                    price = a2 + a4;
                else if (_TypeFlag < 11)
                    price = b2 - b4;
                else if (_TypeFlag < 13)
                    price = a2 - a4;
                else if (_TypeFlag == 13)
                    price = b1 - b2 + b4;
                else if (_TypeFlag == 14)
                    price = b2 - b3 + b4;
                else if (_TypeFlag == 15)
                    price = a1 - a2 + a4;
                else if (_TypeFlag == 16)
                    price = a2 - a3 + a4;
                else if (_TypeFlag == 17)
                    price = b1;
                else if (_TypeFlag == 18)
                    price = b3;
                else if (_TypeFlag == 19)
                    price = a1;
                else if (_TypeFlag == 20)
                    price = a3;
                else if (_TypeFlag == 21)
                    price = b2 - b4;
                else if (_TypeFlag == 22)
                    price = b1 - b2 + b3 - b4;
                else if (_TypeFlag == 23)
                    price = a2 - a4;
                else if (_TypeFlag == 24)
                    price = a1 - a2 + a3 - a4;
                else if (_TypeFlag == 25)
                    price = 0;
                else if (_TypeFlag == 26)
                    price = b1 - b3;
                else if (_TypeFlag == 27)
                    price = 0;
                else if (_TypeFlag == 28)
                    price = a1 - a3;
                else
                    price = double.NaN;
            }
            else
            {
                price = double.NaN;
            }
            return price;
        }

        public static double FDA_Delta(string _TypeFlag, double S, double x, double h, double k, double T,
                               double r, double b, double v, double ds)
        {
            double delta = double.NaN;
            double bsr = BarrierBinary(_TypeFlag, S + ds, x, h, k, T, r, b, v);
            double bs = BarrierBinary(_TypeFlag, S, x, h, k, T, r, b, v);
            double bsl = BarrierBinary(_TypeFlag, S - ds, x, h, k, T, r, b, v);
            delta = (bsr - bsl) / (2 * ds);
            return delta;
        }

        public static double FDA_DeltaR(string _TypeFlag, double S, double x, double h, double k, double T,
                                       double r, double b, double v, double ds)
        {
            double delta = double.NaN;
            double bsr = BarrierBinary(_TypeFlag, S + ds, x, h, k, T, r, b, v);
            double bs = BarrierBinary(_TypeFlag, S, x, h, k, T, r, b, v);
            double bsl = BarrierBinary(_TypeFlag, S - ds, x, h, k, T, r, b, v);
            delta = (bsr - bs) / (ds);
            return delta;
        }

        public static double FDA_DeltaL(string _TypeFlag, double S, double x, double h, double k, double T,
                                       double r, double b, double v, double ds)
        {
            double delta = double.NaN;
            double bsr = BarrierBinary(_TypeFlag, S + ds, x, h, k, T, r, b, v);
            double bs = BarrierBinary(_TypeFlag, S, x, h, k, T, r, b, v);
            double bsl = BarrierBinary(_TypeFlag, S - ds, x, h, k, T, r, b, v);
            delta = (bs - bsl) / (ds);
            return delta;
        }

        public static double FDA_GammaP(string _TypeFlag, double S, double x, double h, double k, double T,
                                       double r, double b, double v, double ds)
        {
            double gammap = double.NaN;
            double bsr = BarrierBinary(_TypeFlag, S + ds, x, h, k, T, r, b, v);
            double bs = BarrierBinary(_TypeFlag, S, x, h, k, T, r, b, v);
            double bsl = BarrierBinary(_TypeFlag, S - ds, x, h, k, T, r, b, v);
            gammap = S / 100 * (bsr - 2 * bs + bsl) / (ds * ds);
            return gammap;
        }

        public static double FDA_Vega(string _TypeFlag, double S, double x, double h, double k, double T,
                                       double r, double b, double v, double ds)
        {
            double vega = double.NaN;
            double bsr = BarrierBinary(_TypeFlag, S, x, h, k, T, r, b, v + 0.01);
            double bsl = BarrierBinary(_TypeFlag, S, x, h, k, T, r, b, v - 0.01);
            vega = (bsr - bsl) / 2.0;
            return vega;
        }

        public static double FDA_Theta(string _TypeFlag, double S, double x, double h, double k, double T,
                                       double r, double b, double v, double ds)
        {
            double theta = double.NaN;
            double deltaT;
            if (T <= 1 / 252.0)
            {
                deltaT = 1 - 0.000005;
            }
            else
            {
                deltaT = 1 / 252.0;
            }
            double bsr = BarrierBinary(_TypeFlag, S, x, h, k, T - deltaT, r, b, v);
            double bsl = BarrierBinary(_TypeFlag, S, x, h, k, T, r, b, v);
            theta = bsr - bsl;
            return theta;
        }

        



    }
}
