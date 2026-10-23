`timescale 1ns / 1ps
//////////////////////////////////////////////////////////////////////////////////
// Company: 
// Engineer: 
// 
// Create Date: 2025/07/03 14:26:29
// Design Name: 
// Module Name: alu
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


module alu (
    input wire [15:0] a,          
    input wire [15:0] b,          
    input wire [2:0] alu_op,     
    output reg [15:0] result,     
    output wire zero,           
    output wire negative          
);


    assign zero = (result == 16'b0);
    assign negative = result[15];  
    
  
    always @(*) begin
        case (alu_op)
            3'b000: result = a + b;              // 加法
            3'b001: result = a - b;              // 减法
            3'b010: result = a & b;              // 与运算
            3'b011: result = a | b;              // 或运算
            3'b100: result = a << b[3:0];        // 逻辑左移
            3'b101: result = a >> b[3:0];        // 逻辑右移
            3'b110: result = a <<< b[3:0];       // 算术左移示
            3'b111: result = $signed(a) >>> b[3:0]; // 算术右移
            default: result = 16'b0;             // 默认情况
        endcase
    end

endmodule
