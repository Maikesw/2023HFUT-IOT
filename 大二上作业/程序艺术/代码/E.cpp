#include<bits/stdc++.h>
#define mn 200005
using namespace std;
int n,a,b,vx[mn],vy[mn],col[mn];
void work() {
	scanf("%d%d%d",&n,&a,&b);
	for(int i=0; i<n; ++i)
		scanf("%d%d%d",&vx[i],&vy[i],&col[i]);
	int n1=a,n2=b,n3=a,n4=b,ans=-1;
	for(int i=0; i<=n*4; ++i) {
		if(col[n1]!=col[n2]||col[n3]!=col[n4]) {
			ans=i; break;
		}
		n1=vx[n1]; n2=vx[n2];
		n3=vy[n3]; n4=vy[n4];
	}
	if(~ans) printf("%d\n",ans);
	else puts("GG");
}
int main() {
	int T; scanf("%d",&T);
	while(T--) work();
	return 0;
}
