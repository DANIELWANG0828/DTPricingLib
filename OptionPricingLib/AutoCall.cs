using MathNet.Numerics.LinearAlgebra;
using System;
using System.Linq;

namespace OptionPricingLib
{
    public class AutoCall
    {
        private static Matrix<double> CumSum(Matrix<double> W)
        {

            Vector<double> tmp = Vector<double>.Build.Dense(W.ColumnCount, 0.0);
            for (int i = 0; i < W.RowCount; i++)
            {
                W.SetRow(i, W.Row(i) + tmp);
                tmp = W.Row(i);

            }
            return W;
        }
        private static Matrix<double> linspace(int nsteps)
        {

            var linspace = Matrix<double>.Build.Dense(nsteps, 1, 0);
            for (int i = 0; i < nsteps; i++)
            {
                linspace[i, 0] = (i + 1);

            }
            return linspace;
        }
        private static int[] getFixingDays(double [] fixings)
        {
            int[] fixing_ko_days = new int[fixings.Length];
            for (int j = 0; j < fixings.Length; j++)
            {
                fixing_ko_days[j] = (int)Math.Round(fixings[j] * 252);
            }
            return fixing_ko_days;
        }



        public static double[] AutoCallable(double S0, double r, double b,
           double vol, double[] fixings, double ko_price, double ki_price, double K,
           double coupon, double rebate, double nominal, double funding, double annpay, int nsims)
        {
            //annpay 0 stands for absolute,1 stands for annualized
            int nsteps = (int) Math.Round(fixings[fixings.Length-1] * 252);
            Vector<double> payoff_vec1 = Vector<double>.Build.Dense(nsims,0.0);
            Vector<double> payoff_vec2 = Vector<double>.Build.Dense(nsims,0.0);
            Vector<double> jdt = Vector<double>.Build.Dense(nsims, fixings.Last());//specifies ko time at each simulation path

            double dt = 1 / 252.0;
            int[] fixing_ko_days = getFixingDays(fixings);
            bool with_ki_feature = true;
            if (ki_price == -1)
            {
                with_ki_feature = false;
            }
           



            
            Matrix<double> W = Matrix<double>.Build.Random(nsteps, nsims);
            Matrix<double> Ts = linspace(nsteps).Multiply(Matrix<double>.Build.Dense(1, nsims, 1 / 252.0));



            Matrix<double> path1 = S0 * ((b - 0.5 * vol * vol) * Ts + vol * CumSum(W * Math.Sqrt(dt))).PointwiseExp();
            for (int j = 0; j < path1.ColumnCount; j++)
            {
                bool not_out_flag = true;
                for (int k = 0; k < fixing_ko_days.Length; k++)
                {

                    if (path1.Column(j)[fixing_ko_days[k] - 1] > ko_price)
                    {
                        jdt[j] = fixings[k];
                        not_out_flag = false;
                        payoff_vec1[j] = (coupon-funding) * (Math.Pow(fixings[k], annpay));
                        break;
                    }
                }

                if (not_out_flag && with_ki_feature && path1.Column(j).Min() < ki_price)
                {
                    payoff_vec1[j] = -Math.Max(K - path1.Column(j).Last(), 0) * Math.Exp(-r * fixings.Last())-
                        funding * (Math.Pow(fixings.Last(), annpay));


                }
                else if (not_out_flag)
                {
                    payoff_vec1[j] = (rebate-funding) * (Math.Pow(fixings.Last(), annpay));

                }

            }
            double price1 = payoff_vec1.Average();
            Vector<double> w_s0 = 1 / (vol * S0 * Math.Sqrt(dt)) * W.Row(0);
            double delta1 = payoff_vec1.PointwiseMultiply(w_s0).Average();
            double gamma1 = (payoff_vec1.PointwiseMultiply((w_s0.PointwisePower(2) - Math.Pow((S0 * vol), -2) / dt - w_s0 / S0))).Average();
            Vector<double> w_sigma = ((W.PointwisePower(2) - 1) / vol - W * Math.Sqrt(dt)).ColumnSums();
            double vega1 = payoff_vec1.PointwiseMultiply(w_sigma).Average();
            double theta1 = payoff_vec1.PointwiseMultiply(-(r * jdt / dt) / 252 - 1 / (2 * dt) + 1 / 252 * (W.PointwisePower(2) / (2 * dt) + (W * 
                (r - 0.5 * vol * vol) / (vol * Math.Sqrt(dt)))).ColumnSums()).Average();
            path1 = null;
            w_s0 = null;
            w_sigma = null;
            payoff_vec1 = null;
            GC.Collect();
            GC.WaitForPendingFinalizers();








            Matrix <double> path2 = S0 * ((b - 0.5 * vol * vol) * Ts + vol * CumSum(-W * Math.Sqrt(dt))).PointwiseExp();
            jdt = Vector<double>.Build.Dense(nsims, fixings.Last());
            for (int j = 0; j < path2.ColumnCount; j++)
            {
                bool not_out_flag = true;
                for (int k = 0; k < fixing_ko_days.Length; k++)
                {

                    if (path2.Column(j)[fixing_ko_days[k] - 1] > ko_price)
                    {
                        jdt[j] = fixings[k];
                        not_out_flag = false;
                        payoff_vec2[j] = (coupon-funding)  * (Math.Pow(fixings[k], annpay));
                        break;
                    }
                }

                if (not_out_flag && with_ki_feature && path2.Column(j).Min() < ki_price)
                {
                    payoff_vec2[j] = -Math.Max(K - path2.Column(j).Last(), 0) * Math.Exp(-r * fixings.Last())-
                        funding * (Math.Pow(fixings.Last(), annpay)); ;
             
                }
                else if (not_out_flag)
                {
                    payoff_vec2[j] = (rebate-funding) * (Math.Pow(fixings.Last(), annpay));

                }

            }
            W = -W;
            w_s0 = 1 / (vol * S0 * Math.Sqrt(dt)) * W.Row(0);
            double price2 = payoff_vec2.Average();
            double delta2 = payoff_vec2.PointwiseMultiply(w_s0).Average();
            double gamma2 = (payoff_vec2.PointwiseMultiply((w_s0.PointwisePower(2) - Math.Pow((S0 * vol), -2) / dt - w_s0 / S0))).Average();
            w_sigma = ((W.PointwisePower(2) - 1) / vol - W * Math.Sqrt(dt)).ColumnSums();
            double vega2 = payoff_vec2.PointwiseMultiply(w_sigma).Average();
            double theta2 = payoff_vec2.PointwiseMultiply(-(r * jdt / dt) / 252 - 1 / (2 * dt) + 1 / 252 * (W.PointwisePower(2) / (2 * dt) + (W * (r - 0.5 * vol * vol) / (vol * Math.Sqrt(dt)))).ColumnSums()).Average();
            path2 = null;
            w_s0 = null;
            w_sigma = null;
            payoff_vec2 = null;
            GC.Collect();
            GC.WaitForPendingFinalizers();
















            double price = 0.5 * price1 + 0.5 * price2;
            double delta = 0.5 * delta1 + 0.5 * delta2;
            double gammap = (0.5 * gamma1 + 0.5 * gamma2) * S0 / 100;
            double vega = 0.5 * vega1 + 0.5 * vega2;
            double theta = 0.5 * theta1 + 0.5 * theta2;




            double[] result = { price, delta, gammap, vega, theta };

            return result;
            

        }

       
    }
}
