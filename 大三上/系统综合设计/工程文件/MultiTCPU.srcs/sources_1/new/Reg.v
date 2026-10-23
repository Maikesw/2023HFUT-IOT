`timescale 1ns / 1ps
//////////////////////////////////////////////////////////////////////////////////
// Company: 
// Engineer: 
// 
// Create Date: 2025/12/08 21:36:49
// Design Name: 
// Module Name: Reg
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
module Reg(
        input RegRead, RegWr,
        input [4:0] rs, rt, rd,
        input RegDes,
        input [31:0] DataALU, DataMem, DataHI, DataLO, JalPC,  // JalPC: jal指令的返回地址
        input [1:0] RegDataSel,  // 写回数据选择：00-ALU，01-Mem，10-HI，11-LO，但需要增加一个选择JalPC
        input JalSel,  // jal指令选择：1-使用JalPC，0-其他
        output reg [31:0] Read1, Read2);
 
        reg [31:0] i_data;	 
        reg [4:0] temp;
        reg [31:0] register [0:31];                                               //32个寄存器，每个32位，其中$0恒为0
        integer i;
        
    initial begin
        temp = 0;
        for (i = 0 ; i < 32; i = i+1) 
		      register[i] = 0;
        // 使用学号2023212351 (0x78A3B1FF) 作为测试数据
        register[1] = 32'h78A3B1FF;      // $1 = 学号（用于算术运算测试）
        register[2] = 32'h78A3B1FF;      // $2 = 学号（用于逻辑运算测试）
        register[3] = 32'h78A3B1FF;      // $3 = 学号（用于内存访问测试）
        register[5] = 32'h00000001;      // $5 = 1（用于加法和数据前推测试）
        register[10] = 32'h78A3B1FF;     // $10 = 学号（用于存储指令测试）
        register[20] = 52;                // $20 = 52（保留原有测试值）
    end
    always @(RegRead or RegWr) begin
           if (RegRead) begin                                                                                                        
                    if(RegDes)begin                                                                 //i类指令，rt作为写寄存器的目标
                        temp = rt;
                    end
                    else begin
                        temp = rd;
                    end
            Read1 = register[rs];                                               
            Read2 = register[rt]; 
            end    
                      
            if (RegWr) begin
                    // 根据JalSel和RegDataSel选择写回数据源
                    if (JalSel) begin
                        i_data = JalPC;  // jal指令：保存返回地址
                    end
                    else begin
                        case(RegDataSel)
                            2'b00: i_data = DataALU;    // ALU结果（add, sub等）
                            2'b01: i_data = DataMem;    // 数据存储器（lw）
                            2'b10: i_data = DataHI;     // HI寄存器（mfhi）
                            2'b11: i_data = DataLO;     // LO寄存器（mflo）
                            default: i_data = DataALU;
                        endcase
                    end
                    // jal指令写入$31（5'b11111），其他指令使用temp
                    if (JalSel) begin
                        if (5'b11111 != 0) begin
                            register[5'b11111] <= i_data;
                        end
                    end
                    else if (temp != 0) begin                             //$0寄存器永远保持0
                            register[temp] <= i_data;
                    end
            end
    end
 
endmodule
