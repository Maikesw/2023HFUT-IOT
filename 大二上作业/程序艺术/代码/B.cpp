/*
先证环保数的性质有：一个正整数是环保数 当且仅当 它是非 1 奇数或4的倍数
注意到 a^2-b^2=(a+b)(a-b)
故对任意非 1 奇数 N：取 a=(N+1)/2,b=(N-1)/2,有 N=a^2-b^2
对任意非 4 本身的 4 的倍数 4*K:取a=K+1,b=K-1有 4*K=a^2-b^2 
对 1 显然不为环保数
对非4倍数的偶数 2*T：
	若 (a^b)&1=1 则 (a^2-b^2)为奇数
	若 (a^b)&1=0 则 (a+b),(a-b)均为偶数，(a^2-b^2) 为 4 的倍数
故非4倍数的偶数不为环保数 
*/
#include<bits/stdc++.h>
using namespace std;
void work() {
	int x; scanf("%d",&x);
	if(x==1) {puts("3"); return;}
	printf("%d\n",((x+2)%3==2)?(x+2)/3*4+3:(x+2)/3*4+(x+2)%3);
}
int main() {
	int T;
	scanf("%d",&T);
	while(T--) work();
	return 0;
}
