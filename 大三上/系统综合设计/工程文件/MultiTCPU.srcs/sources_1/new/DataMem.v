`timescale 1ns / 1ps
//////////////////////////////////////////////////////////////////////////////////
// Company: 
// Engineer: 
// 
// Create Date: 2025/12/10 22:10:26
// Design Name: 
// Module Name: DataMem
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
module DataMEM (
    input MemRead,
    input MemWrite,
    input [31:0] Addr,
    input [31:0] DataIn,
    output reg[31:0] DataOut
    );
    
    reg [7:0] memory [0:63];
    integer i;
    
    initial begin
        for(i = 0; i <64; i=i+1)begin
            memory [i] <= 0;
            if(i == 11)  memory [i] <= 6;                
        end
        memory[0] <= 8'hFF;   
        memory[1] <= 8'hB1;   
        memory[2] <= 8'hA3;   
        memory[3] <= 8'h78;   
        
        memory[4] <= 8'hFF;
        memory[5] <= 8'hB1;
        memory[6] <= 8'hA3;
        memory[7] <= 8'h78;
        DataOut <= 0;
    end
    always @(MemRead or MemWrite) begin
        
        if (MemRead) begin
              DataOut [7:0] = memory [Addr];
              DataOut [15:8] = memory [Addr+1];
              DataOut [23:16] = memory [Addr+2];
              DataOut [31:24] = memory [Addr+3];
        end
        if (MemWrite) begin
                memory [Addr] <= DataIn [7:0];
                memory [Addr+1] <= DataIn [15:8];
                memory [Addr+2] <= DataIn [23:16];
                memory [Addr+3] <= DataIn [31:24];
                DataOut <= 0;
        end
    end
endmodule
