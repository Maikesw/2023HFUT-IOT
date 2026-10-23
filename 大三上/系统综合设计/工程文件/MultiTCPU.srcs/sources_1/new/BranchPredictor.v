`timescale 1ns / 1ps
//////////////////////////////////////////////////////////////////////////////////
// Company: 
// Engineer: 
// 
// Create Date: 2025/01/XX
// Design Name: 
// Module Name: BranchPredictor
// Project Name: 
// Target Devices: 
// Tool Versions: 
// Description: 分支预测器 - 使用2位饱和计数器（BHT - Branch History Table）
//              预测分支指令是否跳转，减少分支延迟
// 
// Dependencies: 
// 
// Revision:
// Revision 0.01 - File Created
// Additional Comments:
// 
//////////////////////////////////////////////////////////////////////////////////

module BranchPredictor(
    input clk,
    input rst,
    input [31:0] PC_ID,              
    input isBranch_ID,               
    output reg predict_taken,        
    input [31:0] PC_EXE,             
    input isBranch_EXE,              
    input actual_taken,              
    input update_enable              
);
    parameter BHT_SIZE = 256;
    parameter ADDR_WIDTH = 8;
    reg [1:0] bht [0:BHT_SIZE-1];
    wire [ADDR_WIDTH-1:0] index_ID;
    wire [ADDR_WIDTH-1:0] index_EXE;
    
    assign index_ID = PC_ID[ADDR_WIDTH+1:2];   
    assign index_EXE = PC_EXE[ADDR_WIDTH+1:2];
    integer i;
    initial begin
        for (i = 0; i < BHT_SIZE; i = i + 1) begin
            bht[i] = 2'b01;  
        end
        predict_taken = 1'b0;
    end
    always @(posedge clk) begin
        if (rst) begin
            for (i = 0; i < BHT_SIZE; i = i + 1) begin
                bht[i] <= 2'b01;
            end
        end
    end
    always @(*) begin
        if (isBranch_ID) begin
            
            predict_taken = (bht[index_ID][1] == 1'b1);
        end else begin
            predict_taken = 1'b0;  
        end
    end
    always @(posedge clk) begin
        if (update_enable && isBranch_EXE && !rst) begin
            
            case (bht[index_EXE])
                2'b00: begin  
                    if (actual_taken) begin
                        bht[index_EXE] <= 2'b01;  
                    end
                    
                end
                2'b01: begin  
                    if (actual_taken) begin
                        bht[index_EXE] <= 2'b10;  
                    end else begin
                        bht[index_EXE] <= 2'b00;  
                    end
                end
                2'b10: begin  
                    if (actual_taken) begin
                        bht[index_EXE] <= 2'b11;  
                    end else begin
                        bht[index_EXE] <= 2'b01;  
                    end
                end
                2'b11: begin  
                    if (!actual_taken) begin
                        bht[index_EXE] <= 2'b10;  
                    end
                    
                end
            endcase
        end
    end

endmodule

