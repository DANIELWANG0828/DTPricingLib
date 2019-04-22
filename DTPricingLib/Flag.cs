namespace DTPricingLib
{
    public class Flag
    {
        public const string OutputFlag = "price,delta,delta+,delta-,gammap,vega,theta";

        public const string VanillaStyle = "c,p";

        public const string StandardBarrierStyle = "cdi,cdo,cui,cuo,pdi,pdo,pui,puo";
        public const string SoftBarrierStyle = "cdi,cdo,pui,puo";
        public const string BestOfOrWorstOfTwoStyle = "bc,bp,wc,wp";
        public const string OneTouchDoubleBarrierBinaryStyle = "i,o";
        public const string DoubleBarrier = "c,p";

        public const string BarrierBinaryStyle = @"hit_cash_di,hit_cash_ui,hit_asset_di,hit_asset_ui,exp_cash_di,exp_cash_ui,
                                                     exp_asset_di,exp_asset_ui,exp_cash_do,exp_cash_uo,exp_asset_do,exp_asset_uo,
                                                     exp_cash_call_di,exp_cash_call_ui,exp_asset_call_di,exp_asset_call_ui,exp_cash_put_di,
                                                     exp_cash_put_ui,exp_asset_put_di,exp_asset_put_ui,exp_cash_call_do,exp_cash_call_uo,exp_asset_call_do,
                                                     exp_asset_call_uo,exp_cash_put_do,exp_cash_put_uo，exp_asset_put_do,exp_asset_put_uo";
        public const string BestOrWorst = "b, w";
    }
}
