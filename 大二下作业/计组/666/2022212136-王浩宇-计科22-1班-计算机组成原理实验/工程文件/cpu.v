`timescale 1ns / 1ps
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
    
    // 程序计数器模块定义（作为子模块）
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