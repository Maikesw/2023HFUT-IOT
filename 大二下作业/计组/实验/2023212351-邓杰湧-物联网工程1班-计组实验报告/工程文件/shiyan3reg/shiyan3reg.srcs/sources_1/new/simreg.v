`timescale 1ns / 1ps
//////////////////////////////////////////////////////////////////////////////////
// Company: 
// Engineer: 
// 
// Create Date: 2025/07/03 15:21:18
// Design Name: 
// Module Name: simreg
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


module register_file (
    input wire clk,                  // 时钟信号
    input wire rst,                  // 复位信号
    input wire [2:0] read_addr1,     // 读端口1地址
    input wire [2:0] read_addr2,     // 读端口2地址
    input wire [2:0] write_addr,     // 写端口地址
    input wire [15:0] write_data,    // 写入数据
    input wire write_enable,         // 写使能信号
    output wire [15:0] read_data1,   // 读端口1数据
    output wire [15:0] read_data2    // 读端口2数据
);

    // 定义8个16位寄存器
    reg [15:0] registers [0:7];
    
    // 初始化寄存器，避免仿真中的X值
    integer i;
    initial begin
        for (i = 0; i < 8; i = i + 1) begin
            registers[i] = 16'h0000;
        end
    end
    
    // 读端口1
    assign read_data1 = registers[read_addr1];
    
    // 读端口2
    assign read_data2 = registers[read_addr2];
    
    // 写端口 - 时钟上升沿触发
    always @(posedge clk) begin
        if (rst) begin
            // 复位时清零所有寄存器
            for (i = 0; i < 8; i = i + 1) begin
                registers[i] <= 16'h0000;
            end
        end
        else if (write_enable) begin
            // 写使能有效时写入数据
            registers[write_addr] <= write_data;
        end
    end

endmodule
