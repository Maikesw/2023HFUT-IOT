`timescale 1ns / 1ps
module dataMem(
    input wire dataMem_wr, clk, 
    input wire [11:0] addr,
    input wire [15:0] data_in,  
    output wire [15:0] data_out
);
    reg [15:0] dataMem[4096:0]; 

    assign data_out = dataMem[addr];

    initial 
    begin
      dataMem[0] = 16'b0010000000100001;   
      dataMem[1] = 16'b0100011100010000;    
    end

    always @(negedge clk) 
    begin
      if(dataMem_wr == 1)
        dataMem[addr] = data_in;
    end

endmodule