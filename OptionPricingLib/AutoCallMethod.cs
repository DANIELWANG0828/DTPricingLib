using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.Distributions;
using System;
using System.Threading;
using System.Linq;
namespace OptionPricingLib
{
    public class AutoCallMethod
    {
        private static bool validate_fixing(double[] fixings)
        {
            for (int i = 0; i < fixings.Length - 1; i++)
            { if (fixings[i] >= fixings[i+1])
                {
                    return false;
                }       
            }
            return true;
        }
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
        private static double kiPayoffType(string ki_type, double K,double S)
        {
            if (ki_type == "put")
            {
                return Math.Max(K -S, 0);
            }
            else if (ki_type == "swap")
            {
                return K - S;
            }
            else
            { return double.NaN; }
        }

       

        public static double PhoenixAutoCallable_smooth_pricer(double S0, double r, double b,
           double vol, double[] fixings, double remained_T, double total_T, double ko_price, double ki_price,string ki_type, double K,
           double coupon, double rebate, double funding, double annpay, int nsims,int seed)
        {
            //annpay 0 stands for absolute,1 stands for annualized
            int nsteps = (int)Math.Round(remained_T * 252);
            Vector<double> payoff_vec1 = Vector<double>.Build.Dense(nsims, 0.0);
            double dt = 1 / 252.0;
            double passed_time = total_T - remained_T;
            //check if the fixing days are valid
            if (!validate_fixing(fixings))
            {
                double error_result = double.NaN;
                return error_result;
            }
            //get fixing days in days number
            int[] fixing_ko_days = getFixingDays(fixings);
            bool with_ki_feature = true;
            if (ki_price == 0)
            {
                with_ki_feature = false;
            }

            System.Random rnd = new System.Random(seed);
            for (int i = 0; i < nsims; i++)
            {
                double s = S0;
                bool ki = false;
                double p_not_out;
                double L = 1;
                double tomorrow_time;
                double dW = Normal.Sample(0, 1);
                for (int j = 1; j <= nsteps; j++)
                {
                    s *= Math.Exp((b - 0.5 * vol * vol) * dt + vol * Math.Sqrt(dt) * dW);
                    dW = Normal.Sample(rnd, 0, 1);
                    if (fixing_ko_days.Contains(j + 1))
                    {
                        tomorrow_time = (j+1) * dt;
                        p_not_out = Normal.CDF(0, 1, (Math.Log(ko_price / s) - (b - Math.Pow(vol, 2) / 2) * dt) / (vol * Math.Sqrt(dt)));
                        payoff_vec1[i] += (1 - p_not_out) * L * (coupon * (Math.Pow(passed_time + tomorrow_time, annpay)) - funding * (passed_time + tomorrow_time))
                            * Math.Exp(-r * tomorrow_time);
                        L *= p_not_out;
                        dW = Normal.InvCDF(0, 1, p_not_out * ContinuousUniform.Sample(rnd, 0, 1));
                    }

                    else if (with_ki_feature && !ki &&  s < ki_price)
                    { ki = true; }

                    else if (j == nsteps)
                    {
                        double last_time = j * dt;
                        if (ki)
                        {
                            payoff_vec1[i] += (-kiPayoffType(ki_type, K, s ) * Math.Exp(-r * nsteps / 252) - funding * (passed_time + last_time) * Math.Exp(-r * nsteps / 252)) * L;
                        }
                        else
                        { payoff_vec1[i] += (rebate * Math.Pow(passed_time + last_time, annpay) * Math.Exp(-r * nsteps / 252) - funding * (passed_time + last_time) * Math.Exp(-r * nsteps / 252)) * L; }
                    }
                }
            }


            double price = payoff_vec1.Average();
            return price;
        }

        public static double[] PhoenixAutoCallable_smooth(double S0, double r, double b,
          double vol, double[] fixings, double remained_T, double total_T, double ko_price, double ki_price, string ki_type, double K,
          double coupon, double rebate, double funding, double annpay, int nsims)
        {
            int seed = DiscreteUniform.Sample(0,int.MaxValue);
            double[] result = { double.NaN, double.NaN, double.NaN, double.NaN, double.NaN };
            double su= PhoenixAutoCallable_smooth_pricer(S0*1.01, r, b, vol, fixings, remained_T, total_T, ko_price, ki_price, ki_type, K, coupon, rebate, funding, annpay, nsims, seed);
            double s= PhoenixAutoCallable_smooth_pricer(S0, r, b, vol, fixings, remained_T, total_T, ko_price, ki_price, ki_type, K, coupon, rebate, funding, annpay, nsims, seed);
            double sd= PhoenixAutoCallable_smooth_pricer(S0*0.99, r, b, vol, fixings, remained_T, total_T, ko_price, ki_price, ki_type, K, coupon, rebate, funding, annpay, nsims, seed);
            double vu= PhoenixAutoCallable_smooth_pricer(S0, r, b, vol+0.01, fixings, remained_T, total_T, ko_price, ki_price, ki_type, K, coupon, rebate, funding, annpay, nsims, seed);
            double vd= PhoenixAutoCallable_smooth_pricer(S0, r, b, vol-0.01, fixings, remained_T, total_T, ko_price, ki_price, ki_type, K, coupon, rebate, funding, annpay, nsims, seed);
            double ru= PhoenixAutoCallable_smooth_pricer(S0, r+0.0001, b, vol, fixings, remained_T, total_T, ko_price, ki_price, ki_type, K, coupon, rebate, funding, annpay, nsims, seed);
            double rd= PhoenixAutoCallable_smooth_pricer(S0, r-0.0001, b, vol, fixings, remained_T, total_T, ko_price, ki_price, ki_type, K, coupon, rebate, funding, annpay, nsims, seed);
            double[] td_fixings = fixings;
            double delta_t;
            if (fixings.Length != 0 && fixings.Min() <= 1 / 252.0)
            { delta_t = 0.5 / 252; }
            else
            { delta_t = 1 / 252.0; }            
            for (int j = 0; j < td_fixings.Length; j++)
            {
                if (td_fixings[j] > 1 / 252.0)
                {
                    td_fixings[j] -= 1 / 252.0; }
                else
                { td_fixings[j] -= 0.5 / 252.0; }
            }
            double td_remained_T = remained_T;
            double td= PhoenixAutoCallable_smooth_pricer(S0, r, b, vol, td_fixings, td_remained_T, total_T, ko_price, ki_price, ki_type, K, coupon, rebate, funding, annpay, nsims, seed);
            double FD_Delta = (su-sd)/(S0*2*0.01);
            double FD_Gamma = (su - 2 * s + sd) / (S0 * 0.01) / (S0 * 0.01);
            double FD_Vega = (vu - vd) / 2;
            double FD_Rho = (ru - rd) / 2;
            double FD_Theta = (td - s) / delta_t;
            return result;
        }


        public static double[] PhoenixAutoCallable(double S0, double r, double b,
           double vol, double[] fixings, double remained_T, double total_T, double ko_price, double ki_price, string ki_type, double K,
           double coupon, double rebate, double funding, double annpay, int nsims)
        {
            //annpay 0 stands for absolute,1 stands for annualized
            int nsteps = (int)Math.Round(remained_T * 252);
            Vector<double> payoff_vec1 = Vector<double>.Build.Dense(nsims, 0.0);
            Vector<double> payoff_vec2 = Vector<double>.Build.Dense(nsims, 0.0);
            Vector<double> jdt = Vector<double>.Build.Dense(nsims, fixings.Last());//specifies ko time at each simulation path
            double dt = 1 / 252.0;
            double passed_time = total_T - remained_T;
            //check if the fixing days are valid
            if (!validate_fixing(fixings))
            {
                double[] error_result = { double.NaN, double.NaN, double.NaN, double.NaN, double.NaN };
                return error_result;
            }
            //get fixing days in days number
            int[] fixing_ko_days = getFixingDays(fixings);
            bool with_ki_feature = true;
            if (ki_price == 0)
            {
                with_ki_feature = false;
            }





            Matrix<double> W = Matrix<double>.Build.Random(nsteps, nsims);
            Matrix<double> Ts = linspace(nsteps).Multiply(Matrix<double>.Build.Dense(1, nsims, 1 / 252.0));


            //path1  
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
                        payoff_vec1[j] = (coupon * (Math.Pow(passed_time + fixings[k], annpay)) - funding * (passed_time + fixings[k]))
                            * Math.Exp(-r * fixings[k]);
                        break;
                    }
                }

                if (with_ki_feature && not_out_flag && path1.Column(j).Min() < ki_price)
                {
                    payoff_vec1[j] = -kiPayoffType(ki_type, K, path1.Column(j).Last()) * Math.Exp(-r * remained_T) -
                        funding * total_T * Math.Exp(-r * remained_T);


                }
                else if (not_out_flag)
                {
                    payoff_vec1[j] = (rebate * (Math.Pow(total_T, annpay)) - funding * total_T) * Math.Exp(-r * remained_T);

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








            //path2 with anti variate
            W = -W;
            Matrix<double> path2 = S0 * ((b - 0.5 * vol * vol) * Ts + vol * CumSum(W * Math.Sqrt(dt))).PointwiseExp();
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
                        payoff_vec2[j] = (coupon * (Math.Pow(passed_time + fixings[k], annpay)) - funding *
                            (passed_time + fixings[k])) * Math.Exp(-r * fixings[k]);
                        break;
                    }
                }

                if (with_ki_feature && not_out_flag && path2.Column(j).Min() < ki_price)
                {
                    payoff_vec2[j] = -kiPayoffType(ki_type, K, path2.Column(j).Last()) * Math.Exp(-r * remained_T) -
                        funding * total_T * Math.Exp(-r * remained_T);

                }
                else if (not_out_flag)
                {
                    payoff_vec2[j] = (rebate * (Math.Pow(total_T, annpay)) - funding * total_T) * Math.Exp(-r * remained_T);

                }

            }
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





            double price = 0.5 * price1 + 0.5 * price2;
            double delta = 0.5 * delta1 + 0.5 * delta2;
            double gammap = (0.5 * gamma1 + 0.5 * gamma2) * S0 / 100;
            double vega = 0.5 * vega1 + 0.5 * vega2;
            double theta = 0.5 * theta1 + 0.5 * theta2;




            double[] result = { price, delta, gammap, vega * 0.01, theta * 1 / 252 };
            return result;
        }

        public static double AutoCallable_smooth_pricer(double S0, double r, double b,
          double vol, double[] fixings, double remained_T, double total_T, double ko_price, double coupon, double rebate, double funding,
          double annpay, int nsims, int seed) 
        {
            //annpay 0 stands for absolute,1 stands for annualized
            int nsteps = (int)Math.Round(remained_T * 252);
            Vector<double> payoff_vec1 = Vector<double>.Build.Dense(nsims, 0.0);
            Vector<double> payoff_vec2 = Vector<double>.Build.Dense(nsims, 0.0);
            double dt = 1 / 252.0;
            double passed_time = total_T - remained_T;
            //check if the fixing days are valid
            if (!validate_fixing(fixings))
            {
                double error_result = double.NaN;
                return error_result;
            }
            //get fixing days in days number
            int[] fixing_ko_days = getFixingDays(fixings);
            System.Random rnd = new System.Random(seed);
            for (int i = 0; i < nsims; i++)
            {
                double dW1,dW2;
                double s1 = S0;
                double s2 = S0;
                double p_not_out1, p_not_out2;
                double L1 = 1;
                double L2 = 1;
                double uniform_rand;
                foreach (int j in fixing_ko_days)
                {
                    //the day before ko observation day
                    dW1 = Normal.Sample(rnd, 0, 1);
                    dW2 = -dW1;
                    s1 *= Math.Exp((b - 0.5 * vol * vol) * dt * (j - 1) + vol * Math.Sqrt(dt * (j - 1)) * dW1);
                    s2 *= Math.Exp((b - 0.5 * vol * vol) * dt * (j - 1) + vol * Math.Sqrt(dt * (j - 1)) * dW2);
                    p_not_out1 = Normal.CDF(0, 1, (Math.Log(ko_price / s1) - (b - Math.Pow(vol, 2) / 2) * dt) / (vol * Math.Sqrt(dt)));
                    p_not_out2 = Normal.CDF(0, 1, (Math.Log(ko_price / s2) - (b - Math.Pow(vol, 2) / 2) * dt) / (vol * Math.Sqrt(dt)));
                    payoff_vec1[i] += (1 - p_not_out1) * L1 * (coupon * (Math.Pow(passed_time + j * dt, annpay)) - funding * (passed_time + j * dt))
                            * Math.Exp(-r * j * dt);
                    payoff_vec2[i] += (1 - p_not_out2) * L2 * (coupon * (Math.Pow(passed_time + j * dt, annpay)) - funding * (passed_time + j * dt))
                            * Math.Exp(-r * j * dt);
                    L1 *= p_not_out1;
                    L2 *= p_not_out2;
                    uniform_rand = ContinuousUniform.Sample(rnd, 0, 1);
                    dW1 = Normal.InvCDF(0, 1, p_not_out1 * uniform_rand);
                    dW2 = Normal.InvCDF(0, 1, p_not_out2 * (1-uniform_rand));
                    s1 *= Math.Exp((b - 0.5 * vol * vol) * dt + vol * Math.Sqrt(dt) * dW1);
                    s2 *= Math.Exp((b - 0.5 * vol * vol) * dt + vol * Math.Sqrt(dt) * dW2);
                }
                payoff_vec1[i] += (rebate * Math.Pow(passed_time + fixings.Last(), annpay) * Math.Exp(-r * nsteps / 252.0) - funding * (passed_time + fixings.Last())
                    * Math.Exp(-r * remained_T)) * L1;
                payoff_vec2[i] += (rebate * Math.Pow(passed_time + fixings.Last(), annpay) * Math.Exp(-r * nsteps / 252.0) - funding * (passed_time + fixings.Last())
                    * Math.Exp(-r * remained_T)) * L2;

            }
            return payoff_vec1.Average()/2+ payoff_vec2.Average()/2;
        }

        public static double[] AutoCallable_smooth(double S0, double r, double b,
          double vol, double[] fixings, double remained_T, double total_T, double ko_price, double coupon, double rebate, double funding,
          double annpay, int nsims)
        {
            int seed = DiscreteUniform.Sample(0, 214748364);
            double su = AutoCallable_smooth_pricer(S0 * 1.01, r, b, vol, fixings, remained_T, total_T, ko_price, coupon, rebate, funding, annpay, nsims, seed);
            double s = AutoCallable_smooth_pricer(S0, r, b, vol, fixings, remained_T, total_T, ko_price, coupon, rebate, funding, annpay, nsims, seed);
            double sd = AutoCallable_smooth_pricer(S0 * 0.99, r, b, vol, fixings, remained_T, total_T, ko_price, coupon, rebate, funding, annpay, nsims, seed);
            double vu = AutoCallable_smooth_pricer(S0, r, b, vol + 0.01, fixings, remained_T, total_T, ko_price, coupon, rebate, funding, annpay, nsims, seed);
            double vd = AutoCallable_smooth_pricer(S0, r, b, vol - 0.01, fixings, remained_T, total_T, ko_price, coupon, rebate, funding, annpay, nsims, seed);
            //double ru = AutoCallable_smooth_pricer(S0, r + 0.0001, b, vol, fixings, remained_T, total_T, ko_price, coupon, rebate, funding, annpay, nsims, seed);
            //double rd = AutoCallable_smooth_pricer(S0, r - 0.0001, b, vol, fixings, remained_T, total_T, ko_price, coupon, rebate, funding, annpay, nsims, seed);
            double[] td_fixings = fixings;
            double delta_t;
            if (fixings.Length != 0 && fixings.Min() <= 1 / 252.0)
            { delta_t = 0.5 / 252; }
            else
            { delta_t = 1 / 252.0; }
            for (int j = 0; j < td_fixings.Length; j++)
            {
                if (td_fixings[j] > 1 / 252.0)
                {
                    td_fixings[j] -= delta_t;
                }

            }
            double td_remained_T = remained_T - delta_t;
            double td = AutoCallable_smooth_pricer(S0, r, b, vol, td_fixings, td_remained_T, total_T, ko_price, coupon, rebate, funding, annpay, nsims, seed);
            double FD_Delta = (su - sd) / (S0 * 2 * 0.01);
            double FD_GammaP = (su - 2 * s + sd) / (S0 * 0.01) / (S0 * 0.01) * S0 / 100;
            double FD_Vega = (vu - vd) / 2;
            //double FD_Rho = (ru - rd) / 2/0.0001;
            double FD_Theta = (td - s) / delta_t;

            double[] result = { s, FD_Delta, FD_GammaP, FD_Vega, FD_Theta };
            return result;
        }


    }
}
