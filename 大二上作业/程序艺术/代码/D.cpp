/*
维护 f[i][j] 从左至右安装第 j 个装置在路口 i 时宣传到的最大市民数 
记 s[x][y] 为前 x 个城市中每日要路过路口 y 的市民数
f[0][0]=0
f[i][j]=Max{k=0~i-1}(f[k][j-1]+s[i][i]-s[k][i]) 
*/
#include<bits/stdc++.h>
#define mn 505
using namespace std;
int n,k;
int a[mn][mn],s[mn][mn]; //a[x][y] 为前 x 个城市中要到达城市 y 的人数 
int f[mn][mn];
int main() {
	scanf("%d%d",&n,&k);
	for(int i=1; i<n; ++i)
		for(int j=i; j<n; ++j)
			scanf("%d",&s[i][j]),s[i][j]+=s[i-1][j];
	for(int i=1; i<n; ++i)
		for(int j=n-1; j>=i; --j)
			s[i][j]+=s[i][j+1];
	memset(f,-0x3f,sizeof f);
	f[0][0]=0;
	for(int i=1; i<n; ++i)
		for(int j=1; j<=i; ++j)
			for(int k=0; k<i; ++k)
				f[i][j]=max(f[i][j],f[k][j-1]+s[i][i]-s[k][i]);
	printf("%d\n",f[n-1][k]);
	return 0;
}
