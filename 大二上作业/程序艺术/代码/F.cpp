#include<bits/stdc++.h>
#define mn 200005
using namespace std;
int n,head[mn],tot;
long long a[mn],f1[mn],f2[mn];
struct Node {
	int v,ne;
} edge[mn];
void add(int x,int y) {
	edge[++tot].v=y;
	edge[tot].ne=head[x];
	head[x]=tot;	
}
void dfs(int x,int f) {
	f1[x]=f2[x]=0;
	for(int i=head[x]; i; i=edge[i].ne) {
		int v=edge[i].v;
		if(v==f) continue;
		dfs(v,x); 
		f1[x]+=max(f1[v],f2[v]);
	}
	for(int i=head[x]; i; i=edge[i].ne) {
		int v=edge[i].v;
		if(v==f) continue;
		f2[x]=max(f2[x],f1[x]-max(f1[v],f2[v])+f1[v]+a[x]*a[v]);
	}
}
int main() {
	scanf("%d",&n);
	for(int i=1; i<n; ++i) {
		int x,y; scanf("%d%d",&x,&y);
		add(x,y); add(y,x);
	}
	for(int i=1; i<=n; ++i) scanf("%lld",&a[i]);
	dfs(1,0);
	printf("%lld\n",max(f1[1],f2[1]));
	return 0;
}
