`timescale 1ns / 1ps
//////////////////////////////////////////////////////////////////////////////////
// Company: 
// Engineer: 
// 
// Create Date: 2025/07/02 23:49:44
// Design Name: 
// Module Name: testmux
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


module simmux4to1;

    reg [3:0] a, b, c, d;    
    reg [1:0] sel;           
    wire [3:0] y;             
    mux4to1 uut(
        .a(a),
        .b(b),
        .c(c),
        .d(d),
        .sel(sel),
        .y(y)
    );
    
    initial begin
        sel <= 2'b00;
        a <= 4'b0010;   
        b <= 4'b0011;  
        c <= 4'b0101; 
        d <= 4'b0001;  
        #200
        sel <= 2'b01; 
        #200
        sel <= 2'b10; 
        #200
        sel <= 2'b11; 
        #200
        $finish;
    end
    
endmodule
