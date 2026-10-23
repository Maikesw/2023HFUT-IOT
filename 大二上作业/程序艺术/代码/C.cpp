/*
不难发现对于一式为关于 x=0 对称的凹函数
二式为关于 x=b 对称的凹函数
易证最小值点在[0,b]上，且两个公式的和在[0,b]上为凹函数 
故使用三分法求解 
*/
#include<bits/stdc++.h>
using namespace std;
double a,b;
double calc(double x) {
	return sqrt(x*x+a)+sqrt((b-x)*(b-x)+1);
}
int main() {
	scanf("%lf%lf",&a,&b);
	double l=0.0,r=b;
	while(r-l>1e-8) {
		double m1=l+(r-l)/3;
		double m2=r-(r-l)/3;
		if(calc(m1)<calc(m2)) r=m2-1e-9;
		else l=m1+1e-9;
	}
	printf("%.6lf\n",calc(l));
	return 0;
}
