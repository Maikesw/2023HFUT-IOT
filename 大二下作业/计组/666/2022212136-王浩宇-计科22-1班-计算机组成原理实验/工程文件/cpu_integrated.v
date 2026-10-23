`timescale 1ns / 1ps

module cpu(
    input wire clk,            
    input wire rst,            
    output wire[15:0] acc_out  
);
     
    wire stop;                 
    wire ct;                   
    wire uct;                  
    wire acc_wr;               
    wire dataMem_wr;           
    wire [11:0] pc_addr;       
    wire [3:0] alu_op;         
    wire [15:0] ins;           
    wire [15:0] in1;         
    wire [15:0] in2;          
    wire [15:0] Z;           
    
     
    assign acc_out = in1;
    
     
    pc pc_inst(
        .clk(clk),
        .rst(rst),
        .stop(stop),
        .ct(ct),
        .uct(uct),
        .offset(ins[11:0]),     
        .pc(pc_addr)
    );
    
     
    insMem insMem(
        .addr(pc_addr),         
        .Ins(ins)               
    );
    
     
    acc acc(
        .clk(clk),
        .acc_wr(acc_wr),        
        .data_in(Z),            
        .data_out(in1)          
    );
    
     
    dataMem dataMem(
        .clk(clk),
        .dataMem_wr(dataMem_wr),  
        .addr(ins[11:0]),         
        .data_in(in1),            
        .data_out(in2)            
    );
    
     
    cu cu(
        .operate(ins[15:12]),     
        .stop(stop),              
        .uct(uct),                
        .ct(ct),                  
        .acc_wr(acc_wr),          
        .dataMem_wr(dataMem_wr),  
        .alu_op(alu_op)           
    );
    
     
    alu alu(
        .in1(in1),                
        .in2(in2),                
        .alu_op(alu_op),          
        .ct(ct),                  
        .Z(Z)                     
    );
     
endmodule

 
 
 
module pc(
   input wire clk,         // 时钟信号
   input wire rst,         // 复位信号
   input wire stop,        // 停机信号，为1时停止PC自增
   input wire ct,          // 条件转移信号，由ALU根据ACC值产生
   input wire uct,         // 无条件转移信号，来自控制单元
   input wire [11:0] offset, // 指令中的偏移量
   output reg [11:0] pc     // 程序计数器值
);
    
     
    always@(posedge clk) begin
        if(rst == 1)
            pc <= 0;               
        else if(stop == 0)         
            pc <= pc + 1;
    end
    
     
    always@(negedge clk) begin
        if(!rst) begin             
            if(uct == 1)           
                pc <= offset;      
            else if(ct == 1)       
                pc <= pc + offset;  
        end
    end    
endmodule

 
 
 
module alu(
    input wire [15:0] in1,     
    input wire [15:0] in2,     
    input wire [3:0] alu_op,   
    output reg ct,             
    output reg [15:0] Z        
);
     
    initial begin
        ct = 0;
        Z = 0;
    end
    
     
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
    
     
    always@(operate) begin
         
        stop = 0;
        uct = 0;
        ct = 0;
        acc_wr = 0;
        dataMem_wr = 0;
        
         
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

module acc(
    input wire clk,                 // 时钟信号
    input wire acc_wr,              // 累加器写使能
    input wire [15:0] data_in,      // 输入数据，来自ALU
    output wire [15:0] data_out     // 输出数据，送往ALU
);
    reg [15:0] acc;                 // 累加器寄存器
    
     
    initial begin
        acc = 0;
    end
    
     
    assign data_out = acc;
    
     
    always@(negedge clk) begin
        if(acc_wr == 1)
            acc <= data_in;          
    end
endmodule


module dataMem(
    input wire clk,                 // 时钟信号
    input wire dataMem_wr,          // 数据存储器写使能
    input wire[11:0] addr,          // 地址，来自指令的低12位
    input wire[15:0] data_in,       // 输入数据，来自累加器
    output wire [15:0] data_out     // 输出数据，送往ALU
);
     
    reg[15:0] dataMem[4095:0];
    
     
    assign data_out = dataMem[addr];
    
     
    initial begin
         
        dataMem[100] = 16'h2351;     
        dataMem[101] = 16'h2023;     
        dataMem[102] = 16'h0000;     
        dataMem[103] = 16'h0000;     
        dataMem[104] = 16'h0000;     
        dataMem[105] = 16'h0000;     
        dataMem[106] = 16'h8765;     
        dataMem[107] = 16'h2022;     
        dataMem[108] = 16'h0000;     
    end
    
     
    always@(negedge clk) begin
        if(dataMem_wr == 1)
            dataMem[addr] <= data_in;   
    end
endmodule

 
 
 
module insMem(
    input wire [11:0] addr,          
    output wire [15:0] Ins           
);
     
    reg[15:0] insMem[4095:0];
    
     
    initial begin
         
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
        
         
        insMem[0] = 16'b0000000000000000;   
        
         
        insMem[1] = 16'b0110000001100100;   
        insMem[2] = 16'b0100000001100101;   
        insMem[3] = 16'b0101000001100110;   
        
         
        insMem[4] = 16'b0001000000000000;   
        insMem[5] = 16'b0101000001100111;   
        insMem[6] = 16'b0110000001100110;   
        insMem[7] = 16'b0010000000000000;   
        insMem[8] = 16'b0101000001101000;   
        insMem[9] = 16'b0110000001100110;   
        insMem[10] = 16'b0011000000000000;  
        insMem[11] = 16'b0101000001101001;  
        
         
        insMem[12] = 16'b0111000000010000;  
        insMem[13] = 16'b1001000000000000;  
        insMem[14] = 16'b1001000000000000;  
        insMem[15] = 16'b1001000000000000;  
        
         
        insMem[16] = 16'b0110000001101010;  
        insMem[17] = 16'b1000000000000011;  
        insMem[18] = 16'b1001000000000000;  
        
         
        insMem[20] = 16'b0110000001101011;  
        insMem[21] = 16'b0100000001100100;  
        insMem[22] = 16'b0101000001101100;  
        
         
        insMem[23] = 16'b1001000000000000;  
    end
    
     
    assign Ins = insMem[addr];
endmodule

 
// 仿真测试模块，用于测试CPU的功能
 
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