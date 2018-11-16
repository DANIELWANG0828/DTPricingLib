using System;
using ExcelDna.Integration;
using OPLib = OptionPricingLib;


namespace DTPricingLib
{
    class VanillaSpreadMethod
    {
        [ExcelFunction(Description = "Returns vanilla option price and greeks through Black-Sholes-Merton method")]
        public static object dtec_vanillaspread([ExcelArgument(Name = "OutPutFlag", Description = Flag.OutputFlag)] string OutPutFlag,
                                              [ExcelArgument(Name = "CallPutFlag", Description = Flag.VanillaStyle)] string CallPutFlag,
                                              [ExcelArgument(Name = "S", Description = "Spot price")] double S,
                                              [ExcelArgument(Name = "X1", Description = "Strike price 1, X2>=X1")] double X1,
                                              [ExcelArgument(Name = "X2", Description = "Strike price 2, X2>=X1")] double X2,
                                              [ExcelArgument(Name = "T", Description = "Days to expiration")] double T,
                                              [ExcelArgument(Name = "r", Description = "Interest rate")] double r,
                                              [ExcelArgument(Name = "b", Description = "Cost of carry")] double b,
                                              [ExcelArgument(Name = "v", Description = "Volatility")] double v,
                                              [ExcelArgument(Name = "dS", Description = "Delta S")] double ds)
        {
            double result = double.NaN;
            double result1 = double.NaN;
            double result2 = double.NaN;
            if (OutPutFlag.Equals("price"))
            {
                result1 = OPLib.BlackScholesMethod.BlackScholes(CallPutFlag, S, X1, T, r, b, v);
                result2 = OPLib.BlackScholesMethod.BlackScholes(CallPutFlag, S, X2, T, r, b, v);
                if (CallPutFlag.Equals("c"))
                {
                    result = result1 - result2;
                }
                else if (CallPutFlag.Equals("p"))
                {
                    result = -result1 + result2;
                }
                else
                {
                    result = double.NaN;
                }
            }

            else if (OutPutFlag.Equals("delta"))
            {
                result1 = OPLib.BlackScholesMethod.FDA_Delta(CallPutFlag, S, X1, T, r, b, v, ds);
                result2 = OPLib.BlackScholesMethod.FDA_Delta(CallPutFlag, S, X2, T, r, b, v, ds);
                if (CallPutFlag.Equals("c"))
                {
                    result = result1 - result2;
                }
                else if (CallPutFlag.Equals("p"))
                {
                    result = -result1 + result2;
                }
                else
                {
                    result = double.NaN; ;
                }
            }

            else if (OutPutFlag.Equals("delta+"))
            {
                result1 = OPLib.BlackScholesMethod.FDA_DeltaR(CallPutFlag, S, X1, T, r, b, v, ds);
                result2 = OPLib.BlackScholesMethod.FDA_Delta(CallPutFlag, S, X2, T, r, b, v, ds);
                if (CallPutFlag.Equals("c"))
                {
                    result = result1 - result2;
                }
                else if (CallPutFlag.Equals("p"))
                {
                    result = -result1 + result2;
                }
                else
                {
                    result = double.NaN; ;
                }
            }

            else if (OutPutFlag.Equals("delta-"))
            {
                result1 = OPLib.BlackScholesMethod.FDA_DeltaL(CallPutFlag, S, X1, T, r, b, v, ds);
                result2 = OPLib.BlackScholesMethod.FDA_Delta(CallPutFlag, S, X2, T, r, b, v, ds);
                if (CallPutFlag.Equals("c"))
                {
                    result = result1 - result2;
                }
                else if (CallPutFlag.Equals("p"))
                {
                    result = -result1 + result2;
                }
                else
                {
                    result = double.NaN; ;
                }
            }


            else if (OutPutFlag.Equals("gammap"))
            {
                result1 = OPLib.BlackScholesMethod.FDA_GammaP(CallPutFlag, S, X1, T, r, b, v, ds);
                result2 = OPLib.BlackScholesMethod.FDA_Delta(CallPutFlag, S, X2, T, r, b, v, ds);
                if (CallPutFlag.Equals("c"))
                {
                    result = result1 - result2;
                }
                else if (CallPutFlag.Equals("p"))
                {
                    result = -result1 + result2;
                }
                else
                {
                    result = double.NaN; ;
                }
            }

            else if (OutPutFlag.Equals("vega"))
            {
                result1 = OPLib.BlackScholesMethod.FDA_Vega(CallPutFlag, S, X1, T, r, b, v, ds);
                result2 = OPLib.BlackScholesMethod.FDA_Delta(CallPutFlag, S, X2, T, r, b, v, ds);
                if (CallPutFlag.Equals("c"))
                {
                    result = result1 - result2;
                }
                else if (CallPutFlag.Equals("p"))
                {
                    result = -result1 + result2;
                }
                else
                {
                    result = double.NaN; ;
                }
            }

            else if (OutPutFlag.Equals("theta"))
            {
                result1 = OPLib.BlackScholesMethod.FDA_Theta(CallPutFlag, S, X1, T, r, b, v, ds);
                result2 = OPLib.BlackScholesMethod.FDA_Delta(CallPutFlag, S, X2, T, r, b, v, ds);
                if (CallPutFlag.Equals("c"))
                {
                    result = result1 - result2;
                }
                else if (CallPutFlag.Equals("p"))
                {
                    result = -result1 + result2;
                }
                else
                {
                    result = double.NaN; ;
                }
            }

            else
            {
                result = double.NaN;
            }

            if (double.IsNaN(result))
            {
                return ExcelError.ExcelErrorValue;
            }
            else
            {
                return result;
            }
        }
    }
}
