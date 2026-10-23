`timescale 1ns / 1ps
//////////////////////////////////////////////////////////////////////////////////
// 数据存储器模块，用于存储和读取数据
//////////////////////////////////////////////////////////////////////////////////
module dataMem(
    input wire clk,                 // 时钟信号
    input wire dataMem_wr,          // 数据存储器写使能
    input wire[11:0] addr,          // 地址，来自指令的低12位
    input wire[15:0] data_in,       // 输入数据，来自累加器
    output wire [15:0] data_out     // 输出数据，送往ALU
);
    // 4K×16位的数据存储器
    reg[15:0] dataMem[4095:0];
    
    // 读取数据
    assign data_out = dataMem[addr];
    
    // 初始化部分数据，用于测试
    initial begin
        // 存放测试数据
        dataMem[100] = 16'h1234;    // 测试数据1
        dataMem[101] = 16'h5678;    // 测试数据2
        dataMem[102] = 16'h0000;    // 空位置，用于存储结果
        dataMem[106] = 16'h8000;    // 负数，用于测试BAN指令
    end
    
    // 在时钟下降沿，如果写使能有效，则写入数据
    always@(negedge clk) begin
        if(dataMem_wr == 1)
            dataMem[addr] <= data_in;  // 将累加器的值写入指定地址
    end
endmodule 