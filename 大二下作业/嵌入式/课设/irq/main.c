#include "stdio.h"
#include "buzzer.h"


#define GPKCON0     		(*((volatile unsigned long *)0x7F008800))
#define GPNCON     			(*((volatile unsigned long *)0x7F008830))
#define GPNDAT     			(*((volatile unsigned long *)0x7F008834))
#define PRIORITY 	    	(*((volatile unsigned long *)0x7F008280))
#define GPKDATA     			(*((volatile unsigned long *)0x7F008808))
//extern void timer_init(unsigned long utimer,unsigned long uprescaler,unsigned long udivider,unsigned long utcntb,unsigned long utcmpb);
void init_uart(void);
int main()
{
	//timer_init(0,65,4,62500,0);
	
	init_uart();
	// 初始化buzzer
	buzzer_init();
	while (1)
	{
	
	}
	
	return 0;
}
