`timescale 1ns / 1ps
//////////////////////////////////////////////////////////////////////////////////
// Company: 
// Engineer: 
// 
// Create Date: 2025/12/12 17:39:02
// Design Name: 
// Module Name: multiCPU
// Project Name: 
// Target Devices: 
// Tool Versions: 
// Description: 
// 
// Dependencies: 
// 
// Revision:
// Revision 0.01 - File Created
// Additional Comments:
// 
//////////////////////////////////////////////////////////////////////////////////


module test;
    reg clk;
    reg rst;
    wire[6:0] a_to_g;                     //数码管段选信号输出，显示ID阶段的指令
    wire [31:0] pc;
    wire[7:0] an;
    wire [31:0] instruction_ID;           // ID阶段指令（用于仿真观察）
    initial begin
            clk = 1;
            rst = 1;
            #2 rst = 0;
            #130 $stop;
    end
    
    CPU uut(
            .clk(clk),.clk0(clk),.a_to_g(a_to_g),.pcAddr(pc),.an(an),
            .instruction_ID_out(instruction_ID)
            // res信号已移除，CPU内部固定为0（不复位）
    );
    
    always  #1clk = ~clk;
endmodule
