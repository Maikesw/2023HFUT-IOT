#include "stdio.h"
#include "buzzer.h"
#define GPKCON0     		(*((volatile unsigned long *)0x7F008800))
#define GPNCON     			(*((volatile unsigned long *)0x7F008830))
#define GPNDAT     			(*((volatile unsigned long *)0x7F008834))
#define PRIORITY 	    	(*((volatile unsigned long *)0x7F008280))
#define GPKDATA     		(*((volatile unsigned long *)0x7F008808))
#define EINT0CON0  			(*((volatile unsigned long *)0x7F008900))
#define EINT0MASK  			(*((volatile unsigned long *)0x7F008920))
#define EINT0PEND  			(*((volatile unsigned long *)0x7F008924))
#define SERVICE     		(*((volatile unsigned long *)0x7F008284))
#define SERVICEPEND 		(*((volatile unsigned long *)0x7F008288))
#define VIC0IRQSTATUS  		(*((volatile unsigned long *)0x71200000))
#define VIC0FIQSTATUS  		(*((volatile unsigned long *)0x71200004))
#define VIC0RAWINTR    		(*((volatile unsigned long *)0x71200008))
#define VIC0INTSELECT  		(*((volatile unsigned long *)0x7120000c))
#define VIC0INTENABLE  		(*((volatile unsigned long *)0x71200010))
#define VIC0INTENCLEAR 		(*((volatile unsigned long *)0x71200014))
#define VIC0PROTECTION 		(*((volatile unsigned long *)0x71200020))
#define VIC0SWPRIORITYMASK 	(*((volatile unsigned long *)0x71200024))
#define VIC0PRIORITYDAISY  	(*((volatile unsigned long *)0x71200028))
#define VIC0ADDRESS        	(*((volatile unsigned long *)0x71200f00))

#define		PWMTIMER_BASE			(0x7F006000)
#define		TCFG0    	( *((volatile unsigned long *)(PWMTIMER_BASE+0x00)) )
#define		TCFG1    	( *((volatile unsigned long *)(PWMTIMER_BASE+0x04)) )
#define		TCON      	( *((volatile unsigned long *)(PWMTIMER_BASE+0x08)) )
#define		TCNTB0    	( *((volatile unsigned long *)(PWMTIMER_BASE+0x0C)) )
#define		TCMPB0    	( *((volatile unsigned long *)(PWMTIMER_BASE+0x10)) )
#define		TCNTO0    	( *((volatile unsigned long *)(PWMTIMER_BASE+0x14)) )
#define		TCNTB1    	( *((volatile unsigned long *)(PWMTIMER_BASE+0x18)) )
#define		TCMPB1    	( *((volatile unsigned long *)(PWMTIMER_BASE+0x1C)) )
#define		TCNTO1    	( *((volatile unsigned long *)(PWMTIMER_BASE+0x20)) )
#define		TCNTB2    	( *((volatile unsigned long *)(PWMTIMER_BASE+0x24)) )
#define		TCMPB2    	( *((volatile unsigned long *)(PWMTIMER_BASE+0x28)) )
#define		TCNTO2    	( *((volatile unsigned long *)(PWMTIMER_BASE+0x2C)) )
#define		TCNTB3    	( *((volatile unsigned long *)(PWMTIMER_BASE+0x30)) )
#define		TCMPB3    	( *((volatile unsigned long *)(PWMTIMER_BASE+0x34)) )
#define		TCNTO3    	( *((volatile unsigned long *)(PWMTIMER_BASE+0x38)) )
#define		TCNTB4    	( *((volatile unsigned long *)(PWMTIMER_BASE+0x3C)) )
#define		TCNTO4    	( *((volatile unsigned long *)(PWMTIMER_BASE+0x40)) )
#define		TINT_CSTAT 	( *((volatile unsigned long *)(PWMTIMER_BASE+0x44)) )

typedef void (isr) (void);
extern void asm_k1_irq();
extern void asm_timer_irq();

// 十六进制数相关变量
volatile int hex_high = 0;    // 高位(K2)
volatile int hex_low = 0;     // 低位(K3)
volatile int actual_hex_high = 0;  // 实际的高位十六进制值（用于计算）
volatile int actual_hex_low = 0;   // 实际的低位十六进制值（用于计算）
volatile int k1_pressed = 0;  // K1按下状态，用于辅助输入A-F
volatile int state = 0;       // 0:初始状态, 1:输入状态, 2:确认后LED闪烁, 3:K4短按比较, 4:重置
volatile int input_timeout = 0; // 输入超时计数器(3秒)
volatile int confirm_hex = 0;   // 确认的十六进制数

// LED闪烁相关变量
volatile int led_flash_count = 0;  // LED闪烁计数
volatile int led_flash_target = 0; // LED闪烁目标次数
volatile int led_state = 0;        // LED状态标志

// K4比较结果相关变量
volatile int comparison_result = 0; // 比较结果：1表示个位>星期，0表示个位<=星期
volatile int buzzer_pattern = 0;    // 蜂鸣器模式：0长短模式，1短促模式
volatile int buzzer_count = 0;      // 蜂鸣器计数
volatile int buzzer_target = 0;     // 蜂鸣器目标次数
volatile int buzzer_state = 0;      // 蜂鸣器状态
volatile int buzzer_timer = 0;      // 蜂鸣器时长计数器
volatile int led_running_pos = 0;   // 跑马灯位置
volatile int led_running_count = 0; // 跑马灯计数

//volatile int k4_click_count = 0;   
//volatile int k4_click_timer = 0;   
// 定时器分频计数器（0.5秒定时器实现不同时间间隔）
volatile int timer_500ms_count = 0;  // 0.5秒计数器，用于实现1秒间隔

volatile int xinqi = 3; // 当前星期（比较值）

// 记录上次按键状态用于检测输入完成
volatile int last_hex_high = 0;
volatile int last_hex_low = 0;

void delay(volatile unsigned int n)
{
	while(n--){
		volatile unsigned int m=0x7ff;
		while(m--){}
	}
}

void irq_init(void)
{
	/* 配置GPN0~3引脚为中断功能 */
	GPNCON &= ~(0xff);
	GPNCON |= 0xaa;

	/* 设置中断触发方式为: 下降沿触发 */
	EINT0CON0 &= ~(0xff);
	EINT0CON0 |= 0x33;

	/* 禁止屏蔽中断 */
	EINT0MASK &= ~(0x0f);
	/* 同时启用Timer0中断(bit23)和外部中断(eint0~11: bit0和bit1) */
	VIC0INTENABLE |= (1 << 23);
	VIC0INTENABLE |=(0x3);  // 0x3 等价于 (1 << 1) | (1 << 0)
	// Select INT_EINT0 mode as irq  
	VIC0INTSELECT = 0;
	isr** isr_array = (isr**)(0x71200100);
  	isr_array[0] = (isr*)asm_k1_irq;

	// isr_array[23] = (isr*)asm_timer_irq; // 定时器0中断
	isr** isr_timer_array = (isr**)(0x7120015C);
	isr_timer_array[0] = (isr*)asm_timer_irq;
	
	
	/*将GPK4-GPK7配置为输出口*/
	GPKCON0 = 0x11110000;
	
	/*熄灭四个LED灯*/
	GPKDATA = 0xf0;
    //timer_init();
}

void timer_init(void)
{
    unsigned long utimer=0;
    unsigned long uprescaler=65;
    unsigned long udivider=4;
    unsigned long utcntb=31250;
    unsigned long utcmpb=0;
	unsigned long temp0;

	// 定时器的输入时钟 = PCLK / ( {prescaler value + 1} ) / {divider value} = PCLK/(65+1)/16=62500hz

	//设置预分频系数为66
	temp0 = TCFG0;
	temp0 = (temp0 & (~(0xff00ff))) | (uprescaler<<0);
	TCFG0 = temp0;

	// 16分频
	temp0 = TCFG1;
	temp0 = (temp0 & (~(0xf<<4*utimer))& (~(1<<20))) |(udivider<<4*utimer);
	TCFG1 = temp0;

	// 1s = 62500hz
	TCNTB0 = utcntb;
	TCMPB0 = utcmpb;

	// 手动更新
	TCON |= 1<<1;

	// 清手动更新位
	TCON &= ~(1<<1);

	// 自动加载和启动timer0
	TCON |= (1<<0)|(1<<3);

	// 使能timer0中断
	temp0 = TINT_CSTAT;
	temp0 = (temp0 & (~(1<<utimer)))|(1<<(utimer));
	TINT_CSTAT = temp0;
}

// 获取十六进制值（考虑K1辅助输入A-F）
int get_hex_value(int count, int k1_assist)
{
    if (k1_assist && count >= 1 && count <= 6) {
        return count + 9; // 1-6对应A-F (10-15)
    }
    return (count > 9) ? my_mod(count, 10) : count;
}

// 自定义除法函数，避免使用ARM软件除法库
int my_div(int dividend, int divisor)
{
    if (divisor == 0) return 0;
    
    int quotient = 0;
    int temp_dividend = dividend;
    
    while (temp_dividend >= divisor) {
        temp_dividend -= divisor;
        quotient++;
    }
    
    return quotient;
}

// 自定义取模函数，避免使用ARM软件除法库
int my_mod(int dividend, int divisor)
{
    if (divisor == 0) return 0;
    
    int temp_dividend = dividend;
    
    while (temp_dividend >= divisor) {
        temp_dividend -= divisor;
    }
    
    return temp_dividend;
}

void do_irq(void)
{
	// K1按键 - 辅助输入A-F
	if (EINT0PEND & 0x1)
	{
		delay(50); // 消抖
		if (!(GPNDAT & (1<<0))) // 确认按键按下
		{
			k1_pressed = 1;
			state = 1; // 进入输入状态
		}
		else
		{
			k1_pressed = 0;
		}
	}
	
	// K2按键 - 高位输入
	if (EINT0PEND & 0x2)
	{
		delay(50); // 消抖
		if (!(GPNDAT & (1<<1))) // 确认按键按下
		{
			state = 1; // 进入输入状态
			
			if (k1_pressed) // K1辅助输入A-F
			{
				hex_high++;
				if (hex_high > 6) hex_high = 1; // 1-6对应A-F
				actual_hex_high = hex_high + 9; // 直接存储A-F的十进制值(10-15)
				// 显示A-F输入 (LED1+LED2同时亮)
				GPKDATA = 0xc0; 
				delay(300);
				GPKDATA = 0xf0;
			}
			else // 正常0-9输入
			{
				hex_high++;
				if (hex_high > 9) hex_high = 0; // 0-9
				actual_hex_high = hex_high; // 直接存储0-9的值
				// 显示当前高位输入
				GPKDATA = 0xd0; // LED2亮
				delay(300);
				GPKDATA = 0xf0; // 全灭
			}
		}
	}
	
	// K3按键 - 低位输入  
	if (EINT0PEND & 0x4)
	{
		delay(50); // 消抖
		if (!(GPNDAT & (1<<2))) // 确认按键按下
		{
			state = 1; // 进入输入状态
			
			if (k1_pressed) // K1辅助输入A-F
			{
				hex_low++;
				if (hex_low > 6) hex_low = 1; // 1-6对应A-F
				actual_hex_low = hex_low + 9; // 直接存储A-F的十进制值(10-15)
				// 显示A-F输入 (LED1+LED3同时亮)
				GPKDATA = 0xa0;
				delay(300);
				GPKDATA = 0xf0;
			}
			else // 正常0-9输入
			{
				hex_low++;
				if (hex_low > 9) hex_low = 0; // 0-9
				actual_hex_low = hex_low; // 直接存储0-9的值
				// 显示当前低位输入
				GPKDATA = 0xe0; // LED1亮
				delay(300);
				GPKDATA = 0xf0; // 全灭
			}
		}
	}
	
	// K4按键 - 短按比较/长按重置
	if (EINT0PEND & 0x8)
	{
		delay(50); // 消抖
		if (!(GPNDAT & (1<<3))) // 确认按键按下
		{
			delay(1000); // 等待1秒判断长按短按
			if (!(GPNDAT & (1<<3))) // 仍按下 - 长按
			{
				state = 4; // 重置状态
				/*				k4_click_count = 0;
				k4_click_timer = 0;*/
			}
			else // 短按
			{
				if (state == 2) // 只有在确认输入后才能比较
				{
					state = 3; // 进入比较状态
				}
				/*				if (state == 2) // 只有在确认输入后才能进行双击检测
				{
					k4_click_count++;
					k4_click_timer = 0;
					
					if (k4_click_count >= 2) 
					{
						state = 3; 
						k4_click_count = 0;
						k4_click_timer = 0;
					}
					*/
			}
		}
	}

	/* 清中断 */
	EINT0PEND = 0x3f;
	VIC0ADDRESS = 0;
}

void do_timer_irq()
{
	// 0.5秒定时器计数
	timer_500ms_count++;
	
	//
	// if (k4_click_count > 0) 
	// {
	// 	k4_click_timer++;
	// 	if (k4_click_timer >= 4) 
	// 	{
	// 		k4_click_count = 0;
	// 		k4_click_timer = 0;
	// 	}
	// }
	
	// 状态1: 输入状态 - 检测输入完成(3秒超时) - 每1秒检查一次
	if (state == 1 && (timer_500ms_count % 2 == 0)) // 每1秒执行
	{
		// 检测输入是否有变化
		if (hex_high == last_hex_high && hex_low == last_hex_low)
		{
			input_timeout++;
			if (input_timeout >= 3) // 3秒无输入变化
			{
				// 输入完成，确认十六进制数
				confirm_hex = actual_hex_high * 16 + actual_hex_low;
				led_flash_target = actual_hex_high; 
				led_flash_count = 0;
				state = 2; // 进入LED闪烁状态
				input_timeout = 0;
			}
		}
		else
		{
			// 输入有变化，重置超时计数
			input_timeout = 0;
			last_hex_high = hex_high;
			last_hex_low = hex_low;
		}
	}
	
	// 状态2: LED闪烁高位次数 - 每1秒闪烁一次
	else if (state == 2 && (timer_500ms_count % 2 == 0)) // 每1秒执行
	{
		if (led_flash_count < led_flash_target)
		{
			if (led_state == 0)
			{
				GPKDATA = 0x00; // 四个LED全亮
				led_state = 1;
			}
			else
			{
				GPKDATA = 0xf0; // 四个LED全灭
				led_state = 0;
				led_flash_count++; // 完成一次闪烁（亮-灭）
			}
		}
		else
		{
			GPKDATA = 0xf0; // 闪烁完成，全灭
		}
	}
	
	// 状态3: K4短按比较处理
	else if (state == 3)
	{
		int decimal_value = confirm_hex;
		int ge_wei = my_mod(decimal_value, 10); // 十进制个位
		int shi_wei = my_mod(my_div(decimal_value, 10), 10); // 十进制十位
		
		if (ge_wei > xinqi) // 个位 > 星期
		{
			// LED持续闪烁 - 每1秒闪烁
			if (timer_500ms_count % 2 == 0) // 每1秒执行LED逻辑
			{
				if (led_state == 0)
				{
					GPKDATA = 0x00; // 四个LED全亮
					led_state = 1;
				}
				else
				{
					GPKDATA = 0xf0; // 四个LED全灭
					led_state = 0;
				}
			}
			
			// 蜂鸣器"一长一短"鸣叫十位次数 - 每0.5秒处理状态
			if (buzzer_count < shi_wei)
			{
				if (buzzer_state == 0) // 长音开始
				{
					buzzer_on();
					buzzer_state = 1;
					buzzer_timer = 0;
				}
				else if (buzzer_state == 1) 
				{
					buzzer_timer++;
					if (buzzer_timer >= 2) 
					{
						buzzer_off();
						buzzer_state = 2;
						buzzer_timer = 0;
					}
				}
				else if (buzzer_state == 2)
				{
					buzzer_timer++;
					if (buzzer_timer >= 2) 
					{
						buzzer_on();
						buzzer_state = 3;
						buzzer_timer = 0;
					}
				}
				else if (buzzer_state == 3) 
				{
					buzzer_timer++;
					if (buzzer_timer >= 1)
					{
						buzzer_off();
						buzzer_state = 4;
						buzzer_timer = 0;
					}
				}
				else // 短音结束，准备下一轮(1秒 = 2个0.5秒)
				{
					buzzer_timer++;
					if (buzzer_timer >= 2) // 间隔1秒
					{
						buzzer_state = 0;
						buzzer_timer = 0;
						buzzer_count++; // 完成一次"一长一短"组合
					}
				}
			}
			else
			{
				buzzer_off(); // 鸣叫完成，关闭蜂鸣器
			}
		}
		else // 个位 <= 星期
		{
			// 蜂鸣器0.5秒间隔鸣叫星期次数 - 每0.5秒处理
			if (buzzer_count < xinqi)
			{
				if (buzzer_state == 0)
				{
					buzzer_on();
					buzzer_state = 1;
				}
				else
				{
					buzzer_off();
					buzzer_state = 0;
					buzzer_count++;
				}
			}
			else
			{
				buzzer_off(); // 鸣叫完成，关闭蜂鸣器
			}
			
			// LED跑马灯显示 - 每0.5秒切换
			if (led_running_count < xinqi)
			{
				switch(led_running_pos)
				{
					case 0: GPKDATA = 0xe0; break; // LED1
					case 1: GPKDATA = 0xd0; break; // LED2  
					case 2: GPKDATA = 0xb0; break; // LED3
					case 3: GPKDATA = 0x70; break; // LED4
				}
				led_running_pos++;
				if (led_running_pos >= 4) led_running_pos = 0; // 替换取模运算
				
				if (led_running_pos == 0) // 完成一轮
				{
					led_running_count++;
				}
			}
			else
			{
				GPKDATA = 0xf0; // 跑马灯结束，全灭
			}
		}
	}
	
	// 状态4: 重置所有状态
	else if (state == 4)
	{
		buzzer_off();
		hex_high = 0;
		hex_low = 0;
		actual_hex_high = 0;
		actual_hex_low = 0;
		k1_pressed = 0;
		state = 0;
		input_timeout = 0;
		confirm_hex = 0;
		led_flash_count = 0;
		led_flash_target = 0;
		led_state = 0;
		comparison_result = 0;
		buzzer_count = 0;
		buzzer_target = 0;
		buzzer_state = 0;
		buzzer_timer = 0;
		led_running_pos = 0;
		led_running_count = 0;
		last_hex_high = 0;
		last_hex_low = 0;
		// k4_click_count = 0; 
		// k4_click_timer = 0; 
		timer_500ms_count = 0; // 重置定时器计数
		GPKDATA = 0xf0; // 全灭
	}

	// 清timer0的中断状态寄存器
	unsigned long uTmp = TINT_CSTAT;
	TINT_CSTAT = uTmp;
	VIC0ADDRESS = 0x0;	
}

