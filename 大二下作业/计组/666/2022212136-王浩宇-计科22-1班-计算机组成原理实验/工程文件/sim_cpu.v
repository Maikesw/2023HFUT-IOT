`timescale 1ns / 1ps
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