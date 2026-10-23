`timescale 1ns / 1ps
//////////////////////////////////////////////////////////////////////////////////
// Company: 
// Engineer: 
// 
// Create Date: 2025/07/03 22:15:40
// Design Name: 
// Module Name: test
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


module simram();
    reg clk;
    reg wr_en;
    reg [9:0] addr;
    reg [15:0] data_in;
    wire [15:0] data_out;
    
    ram dut(
        .clk(clk),
    .wr_en(wr_en),
    .addr(addr),
    .data_in(data_in),
    .data_out(data_out)
    );
    
    initial begin
        clk = 1;
        wr_en = 0;
        addr = 0;
        data_in = 0;
        #10;
        wr_en = 1;
        addr = 0;
        data_in = 16'h2351;
        #10;
        wr_en = 0;
        addr = 0;
        #10;
        #10;
        $finish;
    end
    
  always #5 clk = ~clk;
endmodule

