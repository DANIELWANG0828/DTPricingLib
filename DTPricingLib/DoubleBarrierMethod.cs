using ExcelDna.Integration;
using OPLib = OptionPricingLib;

namespace DTPricingLib
{
    public class DoubleBarrierMethod
    {   public static object dtec_doublebarrier([ExcelArgument(Name = "OutPutFlag", Description = Flag.OutputFlag)] string OutPutFlag,
                                               [ExcelArgument(Name = "TypeFlag", Description = Flag.DoubleBarrier)] string TypeFlag,
                                               [ExcelArgument(Name = "S", Description = "Spot price")] double S,
                                               [ExcelArgument(Name = "X", Description = "Strike price")] double X,
                                               [ExcelArgument(Name = "L", Description = "Lower Barrier price")] double l,
                                               [ExcelArgument(Name = "U", Description = "Higher Barrier price")] double U,
                                               [ExcelArgument(Name = "T", Description = "Days to expiration")] double T,
                                               [ExcelArgument(Name = "r", Description = "Interest rate")] double r,
                                               [ExcelArgument(Name = "b", Description = "Cost of carry")] double b,
                                               [ExcelArgument(Name = "v", Description = "Volatility")] double v,
                                               [ExcelArgument(Name = "Delta1", Description = "Barrier curvature1")] double delta1,
                                               [ExcelArgument(Name = "Delta2", Description = "Barrier curvature2")] double delta2,
                                               [ExcelArgument(Name = "dS", Description = "Delta S")] double ds)
        {

            double result = double.NaN;
            if (OutPutFlag.Equals("p"))
            {
                result = OPLib.DoubleBarrierMethod.DoubleBarrier(TypeFlag, S, X, l, U, T, r, b, v, delta1, delta2);
            }

            else if (OutPutFlag.Equals("p"))
            {
                result = OPLib.DoubleBarrierMethod.FDA_Delta(TypeFlag, S, X, l, U, T, r, b, v, delta1, delta2);
            }

            else if (OutPutFlag.Equals("d+"))
            {
                result = OPLib.DoubleBarrierMethod.FDA_DeltaR(TypeFlag, S, X, l, U, T, r, b, v, delta1, delta2);
            }

            else if (OutPutFlag.Equals("d-"))
            {
                result = OPLib.DoubleBarrierMethod.FDA_DeltaL(TypeFlag, S, X, l, U, T, r, b, v, delta1, delta2);
            }


            else if (OutPutFlag.Equals("gp"))
            {
                result = OPLib.DoubleBarrierMethod.FDA_GammaP(TypeFlag, S, X, l, U, T, r, b, v, delta1, delta2);
            }

            else if (OutPutFlag.Equals("v"))
            {
                result = OPLib.DoubleBarrierMethod.FDA_Vega(TypeFlag, S, X, l, U, T, r, b, v, delta1, delta2);
            }

            else if (OutPutFlag.Equals("t"))
            {
                result = OPLib.DoubleBarrierMethod.FDA_Theta(TypeFlag, S, X, l, U, T, r, b, v, delta1, delta2);
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


            return result;
        }

        

    }
}
