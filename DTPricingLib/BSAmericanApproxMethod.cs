using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExcelDna.Integration;
using OPLib = OptionPricingLib;

namespace DTPricingLib
{
    public class BSAmericanApproxMethod
    {
        [ExcelFunction(Description = "Returns Ameican option price and greeks through Bjerksund&Stensland approximation method")]
        public static object dtec_american([ExcelArgument(Name = "OutPutFlag", Description = Flag.OutputFlag)] string OutPutFlag,
                                       [ExcelArgument(Name = "CallPutFlag", Description = Flag.VanillaStyle)] string CallPutFlag,
                                       [ExcelArgument(Name = "S", Description = "Spot price")] double S,
                                       [ExcelArgument(Name = "X", Description = "Strike price")] double X,
                                       [ExcelArgument(Name = "T", Description = "Days to expiration")] double T,
                                       [ExcelArgument(Name = "r", Description = "Interest rate")] double r,
                                       [ExcelArgument(Name = "b", Description = "Cost of carry")] double b,
                                       [ExcelArgument(Name = "v", Description = "Volatility")] double v,
                                       [ExcelArgument(Name = "dS", Description = "Delta S")] double ds)
        {
            double result = double.NaN;
            if (OutPutFlag.Equals("p"))
            {
                result = OPLib.BSAmericanApproxMethod.BSAmericanApprox2002(CallPutFlag, S, X, T, r, b, v);
            }
            else if (OutPutFlag.Equals("d"))
            {
                result = OPLib.BSAmericanApproxMethod.FDA_Delta(CallPutFlag, S, X, T, r, b, v, ds);
            }

            else if (OutPutFlag.Equals("d+"))
            {
                result = OPLib.BSAmericanApproxMethod.FDA_DeltaR(CallPutFlag, S, X, T, r, b, v, ds);
            }

            else if (OutPutFlag.Equals("d-"))
            {
                result = OPLib.BSAmericanApproxMethod.FDA_DeltaL(CallPutFlag, S, X, T, r, b, v, ds);
            }


            else if (OutPutFlag.Equals("gp"))
            {
                result = OPLib.BSAmericanApproxMethod.FDA_GammaP(CallPutFlag, S, X, T, r, b, v, ds);
            }

            else if (OutPutFlag.Equals("v"))
            {
                result = OPLib.BSAmericanApproxMethod.FDA_Vega(CallPutFlag, S, X, T, r, b, v, ds);
            }

            else if (OutPutFlag.Equals("t"))
            {
                result = OPLib.BSAmericanApproxMethod.FDA_Theta(CallPutFlag, S, X, T, r, b, v, ds);
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
