`timescale 1ns / 1ps
//////////////////////////////////////////////////////////////////////////////////
// Company: 
// Engineer: 
// 
// Create Date: 2025/07/03 17:03:29
// Design Name: 
// Module Name: testreg
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


module register_file_tb;
    // 定义测试信号
    reg clk;
    reg rst;
    reg [2:0] read_addr1;
    reg [2:0] read_addr2;
    reg [2:0] write_addr;
    reg [15:0] write_data;
    reg write_enable;
    wire [15:0] read_data1;
    wire [15:0] read_data2;
    reg [2:0] op_code;

    register_file uut (
        .clk(clk),
        .rst(rst),
        .read_addr1(read_addr1),
        .read_addr2(read_addr2),
        .write_addr(write_addr),
        .write_data(write_data),
        .write_enable(write_enable),
        .read_data1(read_data1),
        .read_data2(read_data2)
    );

    always #5 clk = ~clk;

    initial begin

        clk = 0;
        rst = 1;
        read_addr1 = 0;
        read_addr2 = 0;
        write_addr = 0;
        write_data = 0;
        write_enable = 0;
        op_code = 0;
        
        // 复位释放
        #10 rst = 0;
        
        // 初始化寄存器1和寄存器2
        write_enable = 1;
        write_addr = 3'b000;   // 寄存器0
        write_data = 16'h2351; // 操作数in0=0x2351
        #10;
        
        write_addr = 3'b001;   // 寄存器1
        write_data = 16'h0001; // 操作数in1=0x0001
        #10;

        write_enable = 0;
        read_addr1 = 3'b000;   // 读取寄存器0(in0)
        read_addr2 = 3'b001;   // 读取寄存器1(in1)

        repeat (6) begin
 
            case (op_code)
                3'b000: write_data = read_data1 + read_data2;             
                3'b001: write_data = read_data1 - read_data2;             
                3'b010: write_data = read_data1 & read_data2;            
                3'b011: write_data = read_data1 | read_data2;           
                3'b100: write_data = read_data1 << read_data2[3:0];     
                3'b101: write_data = read_data1 >> read_data2[3:0];      
            endcase
            
            // 将结果写入寄存器
            write_addr = 3'b010 + op_code;  // 2, 3, 4, 5, 6, 7
            write_enable = 1;
            #10;
            op_code = op_code + 1;
        end
        
        // 测试读取所有计算结果
        write_enable = 0;
        
        for (op_code = 0; op_code < 6; op_code = op_code + 1) begin
            read_addr1 = 3'b010 + op_code;
            #10;
        end

        $finish;
    end

endmodule
