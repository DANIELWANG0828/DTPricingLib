using ExcelDna.Integration;
using System;
using OPLib = OptionPricingLib;

namespace DTPricingLib
{
    public class AnalyticBarrierBinaryMethod
    {
        [ExcelFunction(Description = "Returns standard barrier option price and greeks through analytic method")]
        public static object dtec_barrierbinary([ExcelArgument(Name = "OutPutFlag", Description = Flag.OutputFlag)] string OutPutFlag,
                                                      [ExcelArgument(Name = "TypeFlag", Description = Flag.BarrierBinaryStyle)] string TypeFlag,
                                                      [ExcelArgument(Name = "S", Description = "Spot price")] double S,
                                                      [ExcelArgument(Name = "x", Description = "Strike price")] double x,
                                                      [ExcelArgument(Name = "h", Description = "Barrier")] double h,
                                                      [ExcelArgument(Name = "k", Description = "Rebate cash")] double k,
                                                      [ExcelArgument(Name = "T", Description = "Days to expiration")] double T,
                                                      [ExcelArgument(Name = "r", Description = "Interest rate")] double r,
                                                      [ExcelArgument(Name = "b", Description = "Cost of carry")] double b,
                                                      [ExcelArgument(Name = "v", Description = "Volatility")] double v,
                                                      [ExcelArgument(Name = "dS", Description = "delta S")] double ds)
        {
            double result = double.NaN;

            int _TypeFlag = 0;
            switch (TypeFlag)
            {
                case "hit_cash_di":
                    _TypeFlag = 1;
                    break;
                case "hit_cash_ui":
                    _TypeFlag = 2;
                    break;
                case "hit_asset_di":
                    _TypeFlag = 3;
                    break;
                case "hit_asset_ui":
                    _TypeFlag = 4;
                    break;
                case "exp_cash_di":
                    _TypeFlag = 5;
                    break;
                case "exp_cash_ui":
                    _TypeFlag = 6;
                    break;
                case "exp_asset_di":
                    _TypeFlag = 7;
                    break;
                case "exp_asset_ui":
                    _TypeFlag = 8;
                    break;
                case "exp_cash_do":
                    _TypeFlag = 9;
                    break;
                case "exp_cash_uo":
                    _TypeFlag = 10;
                    break;
                case "exp_asset_do":
                    _TypeFlag = 11;
                    break;
                case "exp_asset_uo":
                    _TypeFlag = 12;
                    break;
                case "exp_cash_call_di":
                    _TypeFlag = 13;
                    break;
                case "exp_cash_call_ui":
                    _TypeFlag = 14;
                    break;
                case "exp_asset_call_di":
                    _TypeFlag = 15;
                    break;
                case "exp_asset_call_ui":
                    _TypeFlag = 16;
                    break;
                case "exp_cash_put_di":
                    _TypeFlag = 17;
                    break;
                case "exp_cash_put_ui":
                    _TypeFlag = 18;
                    break;
                case "exp_asset_put_di":
                    _TypeFlag = 19;
                    break;
                case "exp_asset_put_ui":
                    _TypeFlag = 20;
                    break;
                case "exp_cash_call_do":
                    _TypeFlag = 21;
                    break;
                case "exp_cash_call_uo":
                    _TypeFlag = 22;
                    break;
                case "exp_asset_call_do":
                    _TypeFlag = 23;
                    break;
                case "exp_asset_call_uo":
                    _TypeFlag = 24;
                    break;
                case "exp_cash_put_do":
                    _TypeFlag = 25;
                    break;
                case "exp_cash_put_uo":
                    _TypeFlag = 26;
                    break;
                case "exp_asset_put_do":
                    _TypeFlag = 27;
                    break;
                case "exp_asset_put_uo":
                    _TypeFlag = 28;
                    break;
            }

            if (S >= h && (_TypeFlag / 2.0 - (int)(_TypeFlag / 2.0)) == 0)
            {
                if (OutPutFlag.Equals("price"))
                {
                    if (_TypeFlag == 2)
                        result = k;
                    else if (_TypeFlag == 4)
                        result = S;
                    else if (_TypeFlag == 6)
                        result = Math.Exp(-r * T) * k;
                    else if (_TypeFlag == 8)
                        result = Math.Exp(-r * T) * S;
                    else if (_TypeFlag == 10)
                        result = 0;
                    else if (_TypeFlag == 12)
                        result = 0;
                    else if (_TypeFlag == 14)
                    {
                        if (S >= x)
                            result = Math.Exp(-r * T) * k;
                        else
                            result = 0;
                    }
                    else if (_TypeFlag == 16)
                    {
                        if (S >= x)
                            result = Math.Exp(-r * T) * S;
                        else
                            result = 0;
                    }
                    else if (_TypeFlag == 18)
                    {
                        if (S <= x)
                            result = Math.Exp(-r * T) * k;
                        else
                            result = 0;
                    }
                    else if (_TypeFlag == 20)
                    {
                        if (S <= x)
                            result = Math.Exp(-r * T) * S;
                        else
                            result = 0;
                    }
                    else if (_TypeFlag == 22)
                        result = 0;
                    else if (_TypeFlag == 24)
                        result = 0;
                    else if (_TypeFlag == 26)
                        result = 0;
                    else if (_TypeFlag == 28)
                        result = 0;
                }
                else if (OutPutFlag.Equals("delta") || OutPutFlag.Equals("delta+") || OutPutFlag.Equals("delta-"))
                {
                    if (_TypeFlag == 4)
                        result = 1;
                    else if (_TypeFlag == 8 || _TypeFlag == 16 || _TypeFlag == 20)
                        result = Math.Exp((b - r) * T) * 1;
                }
                else
                {
                    result = 0;
                }
                return result;
            }

            if (S <= h && (_TypeFlag / 2.0 - (int)(_TypeFlag / 2.0) != 0))
            {
                if (OutPutFlag.Equals("price"))
                {
                    if (_TypeFlag == 1)
                        result = k;
                    else if (_TypeFlag == 3)
                        result = S;
                    else if (_TypeFlag == 5)
                        result = Math.Exp(-r * T) * k;
                    else if (_TypeFlag == 7)
                        result = Math.Exp(-r * T) * S;
                    else if (_TypeFlag == 9)
                        result = 0;
                    else if (_TypeFlag == 11)
                        result = 0;
                    else if (_TypeFlag == 13)
                    {
                        if (S >= x)
                            result = Math.Exp(-r * T) * k;
                        else
                            result = 0;
                    }
                    else if (_TypeFlag == 15)
                    {
                        if (S >= x)
                            result = Math.Exp(-r * T) * S;
                        else
                            result = 0;
                    }
                    else if (_TypeFlag == 17)
                    {
                        if (S <= x)
                            result = Math.Exp(-r * T) * k;
                        else
                            result = 0;
                    }
                    else if (_TypeFlag == 19)
                    {
                        if (S <= x)
                            result = Math.Exp(-r * T) * S;
                        else
                            result = 0;
                    }
                    else if (_TypeFlag == 21)
                        result = 0;
                    else if (_TypeFlag == 23)
                        result = 0;
                    else if (_TypeFlag == 25)
                        result = 0;
                    else if (_TypeFlag == 27)
                        result = 0;
                }
                else if (OutPutFlag.Equals("delta") || OutPutFlag.Equals("delta+") || OutPutFlag.Equals("delta-"))
                {
                    if (_TypeFlag == 3)
                        result = 1;
                    else if (_TypeFlag == 7 || _TypeFlag == 15 || _TypeFlag == 19)
                        result = Math.Exp((b - r) * T) * 1;
                }
                else
                {
                    result = 0;
                }
                return result;
            }

            if (OutPutFlag.Equals("price"))
            {
                result = OPLib.AnalyticBarrierBinaryMethod.BarrierBinary(TypeFlag, S, x, h, k, T, r, b, v);
            }

            else if (OutPutFlag.Equals("delta"))
            {
                result = OPLib.AnalyticBarrierBinaryMethod.FDA_Delta(TypeFlag, S, x, h, k, T, r, b, v, ds);
            }

            else if (OutPutFlag.Equals("delta+"))
            {
                result = OPLib.AnalyticBarrierBinaryMethod.FDA_DeltaR(TypeFlag, S, x, h, k, T, r, b, v, ds);
            }

            else if (OutPutFlag.Equals("delta-"))
            {
                result = OPLib.AnalyticBarrierBinaryMethod.FDA_DeltaL(TypeFlag, S, x, h, k, T, r, b, v, ds);
            }


            else if (OutPutFlag.Equals("gammap"))
            {
                result = OPLib.AnalyticBarrierBinaryMethod.FDA_GammaP(TypeFlag, S, x, h, k, T, r, b, v, ds);
            }

            else if (OutPutFlag.Equals("vega"))
            {
                result = OPLib.AnalyticBarrierBinaryMethod.FDA_Vega(TypeFlag, S, x, h, k, T, r, b, v, ds);
            }

            else if (OutPutFlag.Equals("theta"))
            {
                result = OPLib.AnalyticBarrierBinaryMethod.FDA_Theta(TypeFlag, S, x, h, k, T, r, b, v, ds);
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
