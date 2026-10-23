/*
注意到最劣情况下：N=2^31-1,,M=1,K=1 此时有ans=ceil(ln(N/M)/ln(100/99)=2138
复杂度 O(ans),可以在规定有限时间内完成 
*/
#include<bits/stdc++.h>
using namespace std;
int main() {
	int n,m,k; scanf("%d%d%d",&n,&m,&k);
	double now=n,fin=m,del=1.0-k/100.0;
	int ans=0;
	while(now-fin>1e-11) now*=del,++ans;
	printf("%d\n",ans);
	return 0;
}
