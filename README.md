# DTPricingLib
目录
1.	Black-Scholes Method\\
2.	Asian Method
3.	AutoCall Method
4.	Binary Method
5.	BS American Approx Method
6.	BSImpVol
7.	Discrete Adjusted Barrier Method
8.	Double Barrier Method
9.	Analytic Barrier Binary Method
10.	Forward Start Method
11.	Interpolation Method
12.	Standard Barrier Method
13.	Three Assets Spread Approx Method
14.	Two Assets Spread Approx Method
15.	Vanilla Spread Method
16.	Version Util




















命名规则：1.开头字母dt。
2.第三个字母 u代表utility functions。
3.第四个字母（若有）c 代表 closed-form solutions，m 代表Monte Carlo solutions，i代表integration solutions。
4.下划线”_”。
5.相应函数名称。



1. Black-Scholes Method
dtec_blackscholes(OutPutFlag,CallPutFlag, S, X, T, r, b, v,dS)
OutPutFlag代表输出类型，可以是’p’--价格，’d’--delta，’gp’—gamma percentage，’v’--vega，’t’--theta。
CallPutFlag代表期权类型，’c’—看涨期权，’p’—看跌期权。
S代表资产现价。
X代表行权价格。
T代表期权到期日。
r代表无风险利率。
b代表carry。
v代表年化波动率。
dS代表差分步长。
	此函数返回一个单独的输出类型。


dtgc_blackscholes(CallPutFlag, S, X, T, r, b, v)
CallPutFlag代表期权类型，’c’—看涨期权，’p’—看跌期权。
S代表资产现价。
X代表行权价格。
T代表期权到期日。
r代表无风险利率。
b代表carry。
v代表年化波动率。
dS代表差分步长。
	此函数返回期权的价格以及Greeks。


2. Asian Method
dtgc_discreteAsianhhm(CallPutFlag, S, SA, X, t1, T, n, m, r, b, v,dS)
CallPutFlag代表期权类型，’c’—看涨期权，’p’—看跌期权。
S代表资产现价。
SA 代表资产已实现的平均价格。
X代表行权价格。
t1代表到观察期还剩余的时间。
T代表期权到期日。
n代表总共的观察点个数。
m代表已经实现的观察点个数。
r代表无风险利率。
b代表carry。
v代表年化波动率。
dS代表差分步长。
此函数返回期权的价格以及Greeks。


dtec_discreteAsianhhm(OutPutFlag ,CallPutFlag, S, SA, X, t1, T, n, m, r, b, v,dS)
OutPutFlag代表输出类型，可以是’p’--价格，’d’--delta，’gp’—gamma percentage，’v’--vega，’t’--theta。
CallPutFlag代表期权类型，’c’—看涨期权，’p’—看跌期权。
S代表资产现价。
SA 代表资产已实现的平均价格。
X代表行权价格。
t1代表到观察期还剩余的时间。
T代表期权到期日。
n代表总共的观察点个数。
m代表已经实现的观察点个数。
r代表无风险利率。
b代表carry。
v代表年化波动率。
dS代表差分步长。
	此函数返回一个单独的输出类型。
3. Autocall Method
dtem_autocall(S0, r, b, vol, fixings, remained_T, total_T, ko_price, ki_price, K, coupon, rebate, nominal, funding, annpay, nsims)
OutPutFlag代表输出类型，可以是’p’--价格，’d’--delta，’gp’—gamma percentage，’v’--vega，’t’--theta。
S0代表资产现价。
r代表无风险利率。
b代表carry。
v代表年化波动率。
remained_T代表剩余时间。
total_T代表存续期总共时间。
ko_price代表敲出价格。
ki_price代表期权到期日。
K代表敲入看跌期权行权价格。
coupon代表票息。
rebate代表未敲入未敲出的收益。
nominal代表名义本金。
funding代表融资成本。
Annpay代表是否年化收益，0代表绝对收益，1代表年化收益。
nsims 代表模拟次数。
	此函数返回一个单独的输出类型。

dtgm_autocall(OutPutFlag,S0, r, b, vol, fixings, remained_T, total_T, ko_price, _ki_price, K, coupon, rebate, nominal, funding, annpay, nsims)
S0代表资产现价。
r代表无风险利率。
b代表carry。
v代表年化波动率。
remained_T代表剩余时间。
total_T代表存续期总共时间。
ko_price代表敲出价格。
ki_price代表期权到期日。
K代表敲入看跌期权行权价格。
coupon代表票息。
rebate代表未敲入未敲出的收益。
nominal代表名义本金。
funding代表融资成本。
Annpay代表是否年化收益，0代表绝对收益，1代表年化收益。
nsims 代表模拟次数。
	此函数返回期权的价格以及Greeks。

4. Binary Method
dtec_cashornothing (OutPutFlag，CallPutFlag, S, X, k, T, r, b, v,dS)
OutPutFlag代表输出类型，可以是’p’--价格，’d’--delta，’gp’—gamma percentage，’v’--vega，’t’--theta。
CallPutFlag代表期权类型，’c’—看涨期权，’p’—看跌期权。
S代表资产现价。
X代表行权价格。
k代表敲出收益。
T代表期权到期日。
r代表无风险利率。
b代表carry。
v代表年化波动率。
	此函数返回一个单独的输出类型。

5. BS American Approx Method
dtec_american (OutPutFlag ,CallPutFlag, S, X, T, r, b, v,dS)
OutPutFlag代表输出类型，可以是’p’--价格，’d’--delta，’gp’—gamma percentage，’v’--vega，’t’--theta。
CallPutFlag代表期权类型，’c’—看涨期权，’p’—看跌期权。
S代表资产现价。
X代表行权价格。
T代表期权到期日。
r代表无风险利率。
b代表carry。
v代表年化波动率。
dS代表差分步长。
	此函数通过Bjerksund&Stensland方法返回一个单独的输出类型。


6. BSImpVol
dtu_impvol_bisec(CallPutFlag, S, X, T, r, b, cm, epsilon)
CallPutFlag代表期权类型，’c’—看涨期权，’p’—看跌期权。
S代表资产现价。
X代表行权价格。
T代表期权到期日。
r代表无风险利率。
b代表carry。
cm代表期权市场价格。
epsilon代表可容忍误差。
此函数通过二分法返回期权的隐含波动率。


dtu_impvol_nr(CallPutFlag, S, X, T, r, b, cm, epsilon)
CallPutFlag代表期权类型，’c’—看涨期权，’p’—看跌期权。
S代表资产现价。
X代表行权价格。
T代表期权到期日。
r代表无风险利率。
b代表carry。
cm代表期权市场价格。
epsilon代表可容忍误差。
此函数通过牛顿法返回期权的隐含波动率。


7. Discrete Adjusted Barrier Method
dtu_discreteadjustedbarrier(S, H, v, dt)
S代表资产现价。
H代表障碍价格。
v代表年化波动率。
dt代表离散观察的interval。
此函数返回调整后的障碍价格。


8. Double Barrier Method

dtec_doublebarrier(OutPutFlag,TypeFlag, S, X, L, U, T, r, b, v, delta1, delta2,dS)
OutPutFlag 代表输出类型，可以是’p’--价格，’d’--delta，’gp’—gamma percentage，’v’--vega，’t’--theta。
TypeFlag代表期权类型，’c’—看涨期权，’p’—看跌期权。
S代表资产现价。
X代表行权价格。
U代表向上敲出价格。
L代表向下敲出价格。
T代表期权到期日。
r代表无风险利率。
b代表carry。
v代表年化波动率。
delta1代表非线性障碍的curvature。
delta2代表非线性障碍的curvature
dS代表差分步长。。
此函数返回一个单独的输出类型。

9.Analytic Barrier Binary Method
dtec_barrierbinary(OutPutFlag ,TypeFlag, S,X, H, k, T, r, b, v,dS)
OutPutFlag 代表输出类型，可以是’p’--价格，’d’--delta，’gp’—gamma percentage，’v’--vega，’t’--theta。
TypeFlag:
包含”hit_cash_di,hit_cash_ui,hit_asset_di,hit_asset_ui,exp_cash_di,exp_cash_ui, exp_asset_di,exp_asset_ui,exp_cash_do,exp_cash_uo,exp_asset_do,exp_asset_uo,                                                     exp_cash_call_di,exp_cash_call_ui,exp_asset_call_di,exp_asset_call_ui,exp_cash_put_di,                            exp_cash_put_ui,exp_asset_put_di,exp_asset_put_ui,exp_cash_call_do,exp_cash_call_uo,exp_asset_call_do,                   exp_asset_call_uo,exp_cash_put_do,exp_asset_put_do,exp_asset_put_uo”
S代表资产现价。
X代表行权价格。
H代表障碍价格。
k代表敲出收益。
T代表期权到期日。
r代表无风险利率。
b代表carry。
v代表年化波动率。
dS代表差分步长。
此函数返回一个单独的输出类型。


dtgc_barrierbinary(OutPutFlag ,TypeFlag, S,X, H, k, T, r, b, v,dS)
OutPutFlag 代表输出类型，可以是’p’--价格，’d’--delta，’gp’—gamma percentage，’v’--vega，’t’--theta。
TypeFlag:
包含”hit_cash_di,hit_cash_ui,hit_asset_di,hit_asset_ui,exp_cash_di,exp_cash_ui, exp_asset_di,exp_asset_ui,exp_cash_do,exp_cash_uo,exp_asset_do,exp_asset_uo,                                                     exp_cash_call_di,exp_cash_call_ui,exp_asset_call_di,exp_asset_call_ui,exp_cash_put_di,                            exp_cash_put_ui,exp_asset_put_di,exp_asset_put_ui,exp_cash_call_do,exp_cash_call_uo,exp_asset_call_do,                   exp_asset_call_uo,exp_cash_put_do,exp_asset_put_do,exp_asset_put_uo”
S代表资产现价。
X代表行权价格。
H代表障碍价格。
k代表敲出收益。
T代表期权到期日。
r代表无风险利率。
b代表carry。
v代表年化波动率。
dS代表差分步长。
	此函数返回期权的价格以及Greeks。
。

10.Forward Start Method
dtei_forwardstart(OutPutFlag , CallPutFlag, S, t1, t2, r, b, vol, a)
OutPutFlag 代表输出类型，可以是’p’--价格，’d’--delta，’v’--vega。
CallPutFlag代表期权类型，’c’—看涨期权，’p’—看跌期权。
S代表资产现价。
X代表行权价格。
t1代表期权开始日。
t2代表期权到期日。
r代表无风险利率。
b代表carry。
v代表年化波动率。
a代表行权价的偏离值。
此函数返回一个单独的输出类型。


11．Interpolation Method
dtu_linearinterpolation(x1,y1,x2,y2,x3)
(x1,y1)代表第一个点坐标。
(x2,y2)代表第二个点坐标。
x3代表第三个点横坐标。
此函数返回线性插值的第三个点纵坐标。


12. standard Barrier Method
dtec_standardbarrier(OutPutFlag ，TypeFlag, S, X, H, k, T, r, b, v, dS)
OutPutFlag 代表输出类型，可以是’p’--价格，’d’--delta，’gp’—gamma percentage，’v’--vega，’t’--theta。
TypeFlag 代表期权类型。包括"cdi,cdo,cui,cuo,pdi,pdo,pui,puo"。’c’—看涨期权，’p’—看跌期权。’d’—向上，’u’—向下。’i’—敲入，’o’—敲出。
S代表资产现价。
X代表行权价格。
H代表障碍价格。
k代表敲出收益。
T代表期权到期日。
r代表无风险利率。
b代表carry。
v代表年化波动率。
dS代表差分步长。
此函数返回一个单独的输出类型。


13.three Assets Spread Approx Method
dtec_3assetspread(OutPutFlag ,CallPutFlag, S1, S2, S3, Q1, Q2, Q3, X, T, r, b1, b2, b3, v1, v2, v3, rho1, rho2, rho3, dS)
OutPutFlag 代表输出类型，可以是’p’--价格，’d1’—delta1，’gp1’—gamma percentage1，’v1’—vega1， ’d2’—delta2，’gp2’—gamma percentage2，’v2’—vega2，’t’—theta1。
CallPutFlag代表期权类型，’c’—看涨期权，’p’—看跌期权。
S1代表1资产现价。
S2代表2资产现价。
S3代表2资产现价。
Q1代表1资产的数量。
Q2代表2资产的数量。
Q3代表3资产的数量。
X代表行权价。
T代表期权到期日。
r代表无风险利率。
b1代表1资产carry。
v1代表1资产年化波动率。
b2代表2资产carry。
v2代表2资产年化波动率。
b3代表3资产carry。
v3代表3资产年化波动率。
rho1代表12资产相关系数。
rho2代表23资产相关系数。
rho3代表13资产相关系数。
dS代表差分步长。
此函数返回一个单独的输出类型。


14.two Assets Spread Approx Method
dtec_2assetspread(OutPutFlag,CallPutFlag, S1, S2, Q1, Q2, X, T, r, b1, b2, v1, v2, rho,dS)
OutPutFlag 代表输出类型，可以是’p’--价格，’d1’—delta1，’gp1’—gamma percentage1，’v1’—vega1， ’d2’—delta2，’gp2’—gamma percentage2，’v2’—vega2，’t’—theta1。
CallPutFlag代表期权类型，’c’—看涨期权，’p’—看跌期权。
S1代表1资产现价。
S2代表2资产现价。
Q1代表1资产的数量。
Q2代表2资产的数量。
X代表行权价。
T代表期权到期日。
r代表无风险利率。
b1代表1资产carry。
v1代表1资产年化波动率。
b2代表2资产carry。
v2代表2资产年化波动率。
rho代表相关系数。
dS代表差分步长。
此函数返回一个单独的输出类型。


15.vanilla Spread Method
dtec_vanillaspread(OutPutFlag ,CallPutFlag, S, X1,X2, T, r, b, v,dS)
OutPutFlag 代表输出类型，可以是’p’--价格，’d’--delta，’gp’—gamma percentage，’v’--vega，’t’--theta。
CallPutFlag代表期权类型，’c’—看涨期权，’p’—看跌期权。
S代表资产现价。
X1代表低行权价。
X2代表高行权价。
T代表期权到期日。
r代表无风险利率。
b代表carry。
v代表年化波动率。
dS代表差分步长。
此函数返回一个单独的输出类型。


16.version Util
dtpricinglib_version()
返回版本号。














