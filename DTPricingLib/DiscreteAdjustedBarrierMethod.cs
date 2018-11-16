using ExcelDna.Integration;
using OPLib = OptionPricingLib;

namespace DTPricingLib
{
    public class DiscreteAdjustedBarrierMethod
    {
        [ExcelFunction(Description = "Returns approximate adjusted barrier of discrete observation")]
        public static object dtu_discreteadjustedbarrier([ExcelArgument(Name = "S", Description = "Spot price")] double S,
                                                    [ExcelArgument(Name = "H", Description = "Barrier")] double H,
                                                    [ExcelArgument(Name = "V", Description = "Volatility")] double v,
                                                    [ExcelArgument(Name = "DT", Description = "Observation Interval")] double dt)
        {
            double result = OPLib.DiscreteAdjustedBarrierMethod.DiscreteAdjustedBarrier(S, H, v, dt);
            return result;
        }
    }
}
