using ExcelDna.Integration;
using System;
using OPLib = OptionPricingLib;

namespace DTPricingLib
{
    public class InterpolationMethod
    {
        public static object dtu_linearinterpolation([ExcelArgument(Name = "x1", Description = "x coordinate of the first point")] double x1,
            [ExcelArgument(Name = "y1", Description = "y coordinate of the first point")] double y1,
            [ExcelArgument(Name = "x2", Description = "x coordinate of the second point")] double x2,
            [ExcelArgument(Name = "y2", Description = "y coordinate of the second point")] double y2,
            [ExcelArgument(Name = "x3", Description = "x coordinate of the third point")] double x3)
        {
            return (x3 - x2) * (y1 - y2) / (x1 - x2) + y2;
        }
    }
}
