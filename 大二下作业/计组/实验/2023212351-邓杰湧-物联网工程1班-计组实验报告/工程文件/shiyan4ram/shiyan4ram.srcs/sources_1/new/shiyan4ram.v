`timescale 1ns / 1ps
//////////////////////////////////////////////////////////////////////////////////
// Company: 
// Engineer: 
// 
// Create Date: 2025/07/03 22:15:21
// Design Name: 
// Module Name: shiyan4ram
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


module ram(
    input wire clk,
    input wire wr_en,
    input wire [9:0] addr,
    input wire [15:0] data_in,
    output reg [15:0] data_out
);

    reg [15:0] ram[511:0];

    always @(posedge clk) begin
        if (wr_en) begin
            ram[addr] <= data_in;
        end
        else begin
            data_out <= ram[addr];
        end
    end
endmodule
