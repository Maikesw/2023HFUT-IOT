`timescale 1ns / 1ps
//////////////////////////////////////////////////////////////////////////////////
// Company: 
// Engineer: 
// 
// Create Date: 2025/07/03 21:10:03
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


module test;
    reg clk;
    reg rst;
    wire [7:0] pc;
    pc uut (
        .clk(clk),
        .rst(rst),
        .pc(pc)
    );
    
    // 生成时钟，周期为10ns
    always #5 clk = ~clk;
    initial begin
        clk = 0;
        rst = 1;
        
        // 复位5个时钟周期
        #50;
        
        // 释放复位，开始计数
        rst = 0;
        
        // 观察PC计数20个周期
        #60;
        
        // 再次复位
        rst = 1;
        #20;
        
        // 再次释放复位，继续计数
        rst = 0;
        #40;
        $finish;
    end
    
    // 监视PC值变化
    initial begin
        $monitor("Time=%t, rst=%b, pc=%d", $time, rst, pc);
    end
    
endmodule
