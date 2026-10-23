`timescale 1ns / 1ps
//////////////////////////////////////////////////////////////////////////////////
// Company: 
// Engineer: 
// 
// Create Date: 2025/12/11 12:33:43
// Design Name: 
// Module Name: PCAddress
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
//MIPS 架构 CPU 中 J 类（无条件跳转）指令的地址拼接模块，核心作用是将指令中的 26 位偏移地址转换为 32 位完整的目标跳
//转地址，为 PC 模块提供合法的跳转目标地址，是 J 类指令实现大范围直接跳转的关键组件
module PCAddress(
        input [25:0] in_addr,
        input [31:0] PC0,
		output reg [31:0] addr);
		
        wire [27:0] mid;
        
	    assign mid = in_addr << 2;
	    
        always @(in_addr or PC0) begin
            addr <= {PC0[31:28], mid[27:0]};
        end
 
endmodule
