`timescale 1ns / 1ps
//////////////////////////////////////////////////////////////////////////////////
// Company: 
// Engineer: 
// 
// Create Date: 2025/12/11 09:49:51
// Design Name: 
// Module Name: extend
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
module Extend(
        input [15:0] in,
        input ExtSel,
        input LuiSel,  // lui指令选择信号：1-lui，0-其他
		output reg [31:0] out);
  
    always @(in or ExtSel or LuiSel) begin
        if(LuiSel) begin
            // lui指令：将立即数加载到高16位，低16位为0
            out <= {in[15:0], 16'b0};
        end
        else if(ExtSel)begin
            out <= {{16{in[15]}}, in[15:0]}; // sign-extend，addi、slti、lw、sw、beq、bne(beq、bne的左移在pc中完成)
         end
         else begin
            out <= {{16{1'b0}}, in[15:0]};                  // zero-extend，ori、andi、xori
         end
    end
 
endmodule

