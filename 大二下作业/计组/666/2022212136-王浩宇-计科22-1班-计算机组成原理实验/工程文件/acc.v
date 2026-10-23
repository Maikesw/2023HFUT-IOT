`timescale 1ns / 1ps
//////////////////////////////////////////////////////////////////////////////////
// 累加器模块，存储和输出ALU的计算结果
//////////////////////////////////////////////////////////////////////////////////
module acc(
    input wire clk,                 // 时钟信号
    input wire acc_wr,              // 累加器写使能
    input wire [15:0] data_in,      // 输入数据，来自ALU
    output wire [15:0] data_out     // 输出数据，送往ALU
);
    reg [15:0] acc;                 // 累加器寄存器
    
    // 初始化累加器为0
    initial begin
        acc = 0;
    end
    
    // 输出累加器的值
    assign data_out = acc;
    
    // 在时钟下降沿，如果写使能有效，则更新累加器的值
    always@(negedge clk) begin
        if(acc_wr == 1)
            acc <= data_in;         // 将ALU的结果写入累加器
    end
endmodule 