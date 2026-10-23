`timescale 1ns / 1ps
//////////////////////////////////////////////////////////////////////////////////
// 程序计数器模块，负责指令地址的产生和更新
//////////////////////////////////////////////////////////////////////////////////
module pc(
   input wire clk,         // 时钟信号
   input wire rst,         // 复位信号
   input wire stop,        // 停机信号，为1时停止PC自增
   input wire ct,          // 条件转移信号，由ALU根据ACC值产生
   input wire uct,         // 无条件转移信号，来自控制单元
   input wire [11:0] offset, // 指令中的偏移量
   output reg [11:0] pc     // 程序计数器值
);
    
    // 正边沿时，处理正常的PC自增或复位
    always@(posedge clk) begin
        if(rst == 1)
            pc <= 0;              // 复位时PC清零
        else if(stop == 0)        // 非停机状态下PC自增
            pc <= pc + 1;
    end
    
    // 负边沿时，处理跳转指令
    always@(negedge clk) begin
        if(!rst) begin            // 非复位状态下处理转移指令
            if(uct == 1)          // 无条件转移(JMP)
                pc <= offset;     // 直接使用指令中的偏移作为新PC
            else if(ct == 1)      // 条件转移(BAN)，累加器为负时
                pc <= pc + offset; // 当前PC加上偏移量
        end
    end    
endmodule 