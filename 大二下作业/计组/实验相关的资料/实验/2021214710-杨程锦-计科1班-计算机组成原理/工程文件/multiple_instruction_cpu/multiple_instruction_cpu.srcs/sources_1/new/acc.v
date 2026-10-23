`timescale 1ns / 1ps
module acc(
    input wire clk, acc_wr, 
    input wire [15:0] data_in, 
    output wire [15:0] data_out 
);
    reg [15:0] acc; 

    initial 
    begin
        acc = 1; 
    end

    assign data_out = acc; 

    always@(negedge clk) 
    begin
      if(acc_wr == 1)
        acc = data_in;
    end

endmodule