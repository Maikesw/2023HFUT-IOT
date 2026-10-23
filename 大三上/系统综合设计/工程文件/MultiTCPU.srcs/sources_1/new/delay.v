`timescale 1ns / 1ps
//////////////////////////////////////////////////////////////////////////////////
// Company: 
// Engineer: 
// 
// Create Date: 2025/12/11 09:50:04
// Design Name: 
// Module Name: delay
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
module Delay(                                       //数据延迟1个CLK
        input [31:0] in,
        input clk,
        output reg [31:0] out);
 
    always @(negedge clk) begin
        out[31:0] = in[31:0];
    end
 
endmodule
