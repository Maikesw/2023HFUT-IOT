`timescale 1ns / 1ps
//////////////////////////////////////////////////////////////////////////////////
// Company: 
// Engineer: 
// 
// Create Date: 2025/07/03 14:27:25
// Design Name: 
// Module Name: testalu
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


module alu_tb;

    reg [15:0] a;
    reg [15:0] b;
    reg [2:0] alu_op;
    wire [15:0] result;
    wire zero;
    wire negative;

    alu uut (
        .a(a),
        .b(b),
        .alu_op(alu_op),
        .result(result),
        .zero(zero),
        .negative(negative)
    );

    initial begin
        a = 16'h2023; b = 16'h2351; alu_op = 3'b000;
        #20;
        a = 16'h2351; b = 16'h2023; alu_op = 3'b001;
        #20;
        a = 16'h2023; b = 16'h2351; alu_op = 3'b001;
        #20;     
        a = 16'h2023; b = 16'h2351; alu_op = 3'b010;
        #20;
        a = 16'h2023; b = 16'h2351; alu_op = 3'b011;
        #20;
        a = 16'h00FF; b = 16'h0004; alu_op = 3'b100;
        #20;
        a = 16'hFF00; b = 16'h0004; alu_op = 3'b101;
        #20;
        a = 16'h00FF; b = 16'h0004; alu_op = 3'b110;
        #20;
        a = 16'h0FF0; b = 16'h0004; alu_op = 3'b111;
        #20;
        a = 16'h8FF0; b = 16'h0004; alu_op = 3'b111;
        #20;
        a = 16'h0000; b = 16'h0000; alu_op = 3'b010; 
        #20;
        $finish;
    end
endmodule
