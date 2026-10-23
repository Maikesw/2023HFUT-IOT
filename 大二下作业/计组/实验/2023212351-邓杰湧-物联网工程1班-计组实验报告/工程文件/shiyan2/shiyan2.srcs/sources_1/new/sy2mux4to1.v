`timescale 1ns / 1ps
//////////////////////////////////////////////////////////////////////////////////
// Company: 
// Engineer: 
// 
// Create Date: 2025/07/02 21:22:29
// Design Name: 
// Module Name: sy2mux4to1
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


module mux4to1 (
    input wire [3:0] a,       
    input wire [3:0] b,       
    input wire [3:0] c,       
    input wire [3:0] d,       
    input wire [1:0] sel,     
    output reg [3:0] y        
);

    
    always @(a or b or c or d or sel)
    begin
        case (sel)
            2'b00: y = a;     
            2'b01: y = b;     
            2'b10: y = c;     
            2'b11: y = d;     
            default: y = 4'b0000; 
        endcase
    end

endmodule
