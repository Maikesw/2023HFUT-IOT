`timescale 1ns / 1ps
//////////////////////////////////////////////////////////////////////////////////
// 集成文件 - 包含所有CPU相关模块
//////////////////////////////////////////////////////////////////////////////////

//////////////////////////////////////////////////////////////////////////////////
// 顶层CPU模块，集成各功能部件
//////////////////////////////////////////////////////////////////////////////////
module cpu(
    input wire clk,           // 时钟信号
    input wire rst,           // 复位信号
    output wire[15:0] acc_out // 累加器输出（用于观察结果）
);
    // 内部连线
    wire stop;                // 停机信号
    wire ct;                  // 条件转移信号（BAN指令使用）
    wire uct;                 // 无条件转移信号（JMP指令使用）
    wire acc_wr;              // 累加器写使能
    wire dataMem_wr;          // 数据存储器写使能
    wire [11:0] pc_addr;      // 程序计数器值
    wire [3:0] alu_op;        // ALU操作码
    wire [15:0] ins;          // 当前指令
    wire [15:0] in1;          // ALU输入1（来自累加器）
    wire [15:0] in2;          // ALU输入2（来自数据存储器）
    wire [15:0] Z;            // ALU输出
    
    // 将累加器值输出，用于观察
    assign acc_out = in1;
    
    // 程序计数器模块实例
    pc pc_inst(
        .clk(clk),
        .rst(rst),
        .stop(stop),
        .ct(ct),
        .uct(uct),
        .offset(ins[11:0]),    // 指令的低12位作为偏移量
        .pc(pc_addr)
    );
    
    // 指令存储器模块实例
    insMem insMem(
        .addr(pc_addr),        // 使用PC的值作为地址
        .Ins(ins)              // 输出的指令
    );
    
    // 累加器模块实例
    acc acc(
        .clk(clk),
        .acc_wr(acc_wr),       // 控制单元提供的写使能
        .data_in(Z),           // ALU的结果写入累加器
        .data_out(in1)         // 累加器的值作为ALU的输入1
    );
    
    // 数据存储器模块实例
    dataMem dataMem(
        .clk(clk),
        .dataMem_wr(dataMem_wr), // 控制单元提供的写使能
        .addr(ins[11:0]),        // 指令的低12位作为地址
        .data_in(in1),           // 累加器的值写入内存
        .data_out(in2)           // 内存读出的值作为ALU的输入2
    );
    
    // 控制单元模块实例
    cu cu(
        .operate(ins[15:12]),    // 指令的高4位作为操作码
        .stop(stop),             // 停机信号
        .uct(uct),               // 无条件转移信号
        .ct(ct),                 // 条件转移信号
        .acc_wr(acc_wr),         // 累加器写使能
        .dataMem_wr(dataMem_wr), // 数据存储器写使能
        .alu_op(alu_op)          // ALU操作码
    );
    
    // ALU模块实例
    alu alu(
        .in1(in1),               // 来自累加器的输入
        .in2(in2),               // 来自数据存储器的输入
        .alu_op(alu_op),         // 来自控制单元的操作码
        .ct(ct),                 // 条件转移信号
        .Z(Z)                    // ALU计算结果
    );
     
endmodule

//////////////////////////////////////////////////////////////////////////////////
// 程序计数器模块，负责指令地址的产生和更新
//////////////////////////////////////////////////////////////////////////////////
module pc(
   input wire clk,         // 时钟信号
   input wire rst,         // 复位信号
   input wire stop,        // 停机信号，为1时停止PC自增
   input wire ct,          // 条件转移信号，由ALU根据ACC值产生
   input wire uct,         // 无条件转移信号，来自控制单元
   input wire [11:0] offset, // 指令中的偏移量
   output reg [11:0] pc     // 程序计数器值
);
    
    // 正边沿时，处理正常的PC自增或复位
    always@(posedge clk) begin
        if(rst == 1)
            pc <= 0;              // 复位时PC清零
        else if(stop == 0)        // 非停机状态下PC自增
            pc <= pc + 1;
    end
    
    // 负边沿时，处理跳转指令
    always@(negedge clk) begin
        if(!rst) begin            // 非复位状态下处理转移指令
            if(uct == 1)          // 无条件转移(JMP)
                pc <= offset;     // 直接使用指令中的偏移作为新PC
            else if(ct == 1)      // 条件转移(BAN)，累加器为负时
                pc <= pc + offset; // 当前PC加上偏移量
        end
    end    
endmodule

//////////////////////////////////////////////////////////////////////////////////
// ALU模块，实现各种算术逻辑运算
//////////////////////////////////////////////////////////////////////////////////
module alu(
    input wire [15:0] in1,    // 输入1，来自累加器
    input wire [15:0] in2,    // 输入2，来自数据存储器
    input wire [3:0] alu_op,  // 操作码，决定ALU的功能
    output reg ct,            // 条件转移信号，用于BAN指令
    output reg [15:0] Z       // 运算结果
);
    // 初始化输出信号
    initial begin
        ct = 0;
        Z = 0;
    end
    
    // 根据操作码进行不同的运算
    always@(*) begin
        case(alu_op)
            4'b0000: Z = 16'h0000;                      // CLA: 清除累加器
            4'b0001: Z = ~in1;                          // COM: 累加器取反
            4'b0010: Z = {in1[15], in1[15:1]};          // SHR: 算术右移一位
            4'b0011: Z = {in1[14:0], in1[15]};          // CSL: 循环左移一位
            4'b0100: Z = in1 + in2;                     // ADD: 加法
            4'b0101: Z = in1;                           // STA: 存数（保持输入不变）
            4'b0110: Z = in2;                           // LDA: 取数（将输入2传递到输出）
            4'b0111: Z = in1;                           // JMP: 无需ALU操作
            4'b1000: begin                              // BAN: 检查累加器最高位
                     Z = in1;                           // 保持累加器值不变
                     ct = (in1[15] == 1) ? 1 : 0;       // 如果ACC为负数，设置条件转移信号
                     end
            4'b1001: Z = in1;                           // STP: 无需ALU操作
            default: Z = 16'h0000;                      // 默认情况，清零
        endcase
    end
endmodule

//////////////////////////////////////////////////////////////////////////////////
// 控制单元模块，根据指令操作码生成各种控制信号
//////////////////////////////////////////////////////////////////////////////////
module cu(
    input wire[3:0] operate,         // 操作码，指令的高4位
    output reg stop,                  // 停机信号
    output reg uct,                   // 无条件转移信号
    output reg ct,                    // 条件转移信号
    output reg acc_wr,                // 累加器写使能
    output reg dataMem_wr,            // 数据存储器写使能
    output reg [3:0] alu_op          // ALU操作码
);
    // 初始化所有控制信号
    initial begin
        stop = 0;
        uct = 0;
        ct = 0;
        acc_wr = 0;
        dataMem_wr = 0;
        alu_op = 4'b0000;
    end
    
    // 根据操作码生成控制信号
    always@(operate) begin
        // 默认所有控制信号为0
        stop = 0;
        uct = 0;
        ct = 0;
        acc_wr = 0;
        dataMem_wr = 0;
        
        // 设置ALU操作码为当前指令的操作码
        alu_op = operate;
        
        case(operate)
            4'b0000: acc_wr = 1;                         // CLA: 写入累加器
            4'b0001: acc_wr = 1;                         // COM: 写入累加器
            4'b0010: acc_wr = 1;                         // SHR: 写入累加器
            4'b0011: acc_wr = 1;                         // CSL: 写入累加器
            4'b0100: acc_wr = 1;                         // ADD: 写入累加器
            4'b0101: dataMem_wr = 1;                     // STA: 写入数据存储器
            4'b0110: acc_wr = 1;                         // LDA: 写入累加器
            4'b0111: uct = 1;                            // JMP: 无条件转移
            4'b1000: ct = 0;                             // BAN: 条件转移（由ALU设置ct信号）
            4'b1001: stop = 1;                           // STP: 停机
            default: ;                                   // 默认无操作
        endcase
    end
endmodule

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
        // 存放测试数据 - 使用2051和其他数据
        dataMem[100] = 16'h2351;    // 测试数据1 - 2051 (0x0803)
        dataMem[101] = 16'h2023;    // 测试数据2 - 1998 (0x07CE)
        dataMem[102] = 16'h0000;    // 空位置，用于存储结果 - ADD结果
        dataMem[103] = 16'h0000;    // 空位置，用于存储取反结果
        dataMem[104] = 16'h0000;    // 空位置，用于存储右移结果
        dataMem[105] = 16'h0000;    // 空位置，用于存储左移结果
        dataMem[106] = 16'h8765;    // 负数，用于测试BAN指令
        dataMem[107] = 16'h2022;    // 另一个测试数据 - 2022 (0x07E6)
        dataMem[108] = 16'h0000;    // 空位置，用于存储更多结果
    end
    
    // 在时钟下降沿，如果写使能有效，则写入数据
    always@(negedge clk) begin
        if(dataMem_wr == 1)
            dataMem[addr] <= data_in;  // 将累加器的值写入指定地址
    end
endmodule

//////////////////////////////////////////////////////////////////////////////////
// 指令存储器模块，用于存储和输出指令
//////////////////////////////////////////////////////////////////////////////////
module insMem(
    input wire [11:0] addr,         // 地址，来自程序计数器
    output wire [15:0] Ins          // 输出的指令
);
    // 4K×16位的指令存储器
    reg[15:0] insMem[4095:0];
    
    // 初始化指令内存，存放测试程序
    initial begin
        // 操作码定义:
        // 0000: CLA (清除累加器)
        // 0001: COM (累加器取反)
        // 0010: SHR (算术右移一位)
        // 0011: CSL (循环左移一位)
        // 0100: ADD (加法指令)
        // 0101: STA (存数指令)
        // 0110: LDA (取数指令)
        // 0111: JMP (无条件转移)
        // 1000: BAN (负则转)
        // 1001: STP (停机)
        
        // 测试程序：基于2051和其他数据测试所有指令
        insMem[0] = 16'b0000000000000000;  // CLA: 清除累加器
        
        // 测试LDA和ADD指令
        insMem[1] = 16'b0110000001100100;  // LDA 100: 从地址100加载数据(0x2051)
        insMem[2] = 16'b0100000001100101;  // ADD 101: 加上地址101的值(0x1998)
        insMem[3] = 16'b0101000001100110;  // STA 102: 存储结果到地址102 (0x2051+0x1998=0x39E9)
        
        // 测试COM, SHR, CSL指令
        insMem[4] = 16'b0001000000000000;  // COM: 累加器取反
        insMem[5] = 16'b0101000001100111;  // STA 103: 存储取反结果到地址103
        insMem[6] = 16'b0110000001100110;  // LDA 102: 重新加载加法结果
        insMem[7] = 16'b0010000000000000;  // SHR: 算术右移一位
        insMem[8] = 16'b0101000001101000;  // STA 104: 存储右移结果到地址104
        insMem[9] = 16'b0110000001100110;  // LDA 102: 重新加载加法结果
        insMem[10] = 16'b0011000000000000; // CSL: 循环左移一位
        insMem[11] = 16'b0101000001101001; // STA 105: 存储左移结果到地址105
        
        // 测试JMP指令
        insMem[12] = 16'b0111000000010000; // JMP 16: 跳转到地址16
        insMem[13] = 16'b1001000000000000; // STP: 停机（不会执行到）
        insMem[14] = 16'b1001000000000000; // STP: 停机（不会执行到）
        insMem[15] = 16'b1001000000000000; // STP: 停机（不会执行到）
        
        // 测试BAN指令
        insMem[16] = 16'b0110000001101010; // LDA 106: 加载负数(0x8765)
        insMem[17] = 16'b1000000000000011; // BAN 3: 如果ACC为负，PC+3
        insMem[18] = 16'b1001000000000000; // STP: 停机（如果ACC不为负，会执行到这里）
        
        // 附加测试 - 执行额外的加法运算
        insMem[20] = 16'b0110000001101011; // LDA 107: 加载地址107的值(0x2022)
        insMem[21] = 16'b0100000001100100; // ADD 100: 加上地址100的值(0x2051)
        insMem[22] = 16'b0101000001101100; // STA 108: 存储结果到地址108 (0x2022+0x2051=0x4073)
        
        // 最终停机
        insMem[23] = 16'b1001000000000000; // STP: 停机
    end
    
    // 输出当前地址的指令
    assign Ins = insMem[addr];
endmodule

//////////////////////////////////////////////////////////////////////////////////
// 仿真测试模块，用于测试CPU的功能
//////////////////////////////////////////////////////////////////////////////////
module sim_cpu;
    // 定义测试信号
    reg clk;                        // 时钟信号
    reg rst;                        // 复位信号
    wire [15:0] acc_out;            // 累加器输出
    
    // 实例化CPU模块
    cpu uut (
        .clk(clk),
        .rst(rst),
        .acc_out(acc_out)
    );
    
    // 时钟生成，周期为10ns
    initial begin
        clk = 0;
        forever #5 clk = ~clk;      // 10ns周期，5ns半周期
    end
    
    // 测试过程
    initial begin
        // 初始化测试信号
        rst = 1;                    // 复位信号有效
        
        // 延时20ns后释放复位信号
        #20;
        rst = 0;
        
        // 运行足够长的时间执行所有指令
        // 每条指令需要一个时钟周期执行，总共约15条指令，加上一些额外的时间
        #300;
        
        // 结束仿真
        $finish;
    end
    
    // 监视关键信号变化
    always @(posedge clk) begin
        if (!rst) begin
            // 监视PC、指令和累加器的值
            $write("Time=%0t, PC=%h, Ins=%h, ACC=%h", 
                  $time, uut.pc_addr, uut.ins, uut.in1);
                  
            // 根据当前指令的操作码判断执行的指令
            case(uut.ins[15:12])
                4'b0000: $write(" [CLA]");
                4'b0001: $write(" [COM]");
                4'b0010: $write(" [SHR]");
                4'b0011: $write(" [CSL]");
                4'b0100: $write(" [ADD %h]", uut.ins[11:0]);
                4'b0101: $write(" [STA %h]", uut.ins[11:0]);
                4'b0110: $write(" [LDA %h]", uut.ins[11:0]);
                4'b0111: $write(" [JMP %h]", uut.ins[11:0]);
                4'b1000: $write(" [BAN %h]", uut.ins[11:0]);
                4'b1001: $write(" [STP]");
            endcase
            
            // 换行
            $write("\n");
            
            // 如果是停机指令，显示停机信息
            if (uut.stop)
                $write("CPU已停机\n");
        end
    end
endmodule 