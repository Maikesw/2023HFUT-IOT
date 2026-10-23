`timescale 1ns / 1ps

module cpu_tb;
    // 定义测试信号
    reg clk;
    reg rst;
    wire a;
    
    // 实例化CPU
    cpu uut (
        .clk(clk),
        .rst(rst),
        .a(a)
    );
    
    // 时钟生成，周期为10ns
    always #5 clk = ~clk;
    
    // 初始化指令内存
    // 假设指令格式: [15:12]操作码, [11:0]地址/立即数
    // 操作码定义:
    // 0000: CLA (清除累加器)
    // 0001: COM (累加器取反)
    // 0010: SHR (算术右移一位)
    // 0011: CSL (循环左移一位)
    // 0100: STP (停机)
    // 0101: ADD (加法指令)
    // 0110: STA (存数指令)
    // 0111: LDA (取数指令)
    // 1000: JMP (无条件转移)
    // 1001: BAN (负则转)
    initial begin
        // 为指令内存和数据内存设置初值
        // 这里假设有一个全局数组可以访问指令内存
        uut.insMem.memory[0] = 16'h0000;      // CLA: 清除累加器
        uut.insMem.memory[1] = 16'h7064;      // LDA 100: 加载地址100的值到ACC (2351)
        uut.insMem.memory[2] = 16'h5065;      // ADD 101: 将地址101的值(2023)加到ACC
        uut.insMem.memory[3] = 16'h6066;      // STA 102: 存储ACC的值到地址102
        uut.insMem.memory[4] = 16'h0001;      // COM: ACC取反
        uut.insMem.memory[5] = 16'h6067;      // STA 103: 存储取反后的值到地址103
        uut.insMem.memory[6] = 16'h0002;      // SHR: 算术右移一位
        uut.insMem.memory[7] = 16'h6068;      // STA 104: 存储右移后的值到地址104
        uut.insMem.memory[8] = 16'h0003;      // CSL: 循环左移一位
        uut.insMem.memory[9] = 16'h6069;      // STA 105: 存储左移后的值到地址105
        uut.insMem.memory[10] = 16'h800F;     // JMP 15: 无条件跳转到地址15
        uut.insMem.memory[15] = 16'h706A;     // LDA 106: 加载负数到ACC (假设地址106存放负数)
        uut.insMem.memory[16] = 16'h9005;     // BAN 5: 如果ACC为负，跳转到地址PC+5=21
        uut.insMem.memory[17] = 16'h0004;     // STP: 如果ACC不为负，停机
        uut.insMem.memory[21] = 16'h706B;     // LDA 107: 加载地址107的值
        uut.insMem.memory[22] = 16'h0004;     // STP: 停机

        // 初始化数据内存
        uut.dataMem.memory[100] = 16'h2351;   // 地址100: 操作数2351
        uut.dataMem.memory[101] = 16'h2023;   // 地址101: 操作数2023
        uut.dataMem.memory[106] = 16'h8000;   // 地址106: 负数(最高位为1)
        uut.dataMem.memory[107] = 16'hABCD;   // 地址107: 随机值用于验证BAN指令
        
        // 初始化测试信号
        clk = 0;
        rst = 1;
        
        // 复位释放
        #20;
        rst = 0;
        
        // 运行足够长的时间执行所有指令
        #500;
        
        // 检查结果
        $display("测试结果:");
        $display("地址102 (2351+2023): %h", uut.dataMem.memory[102]);
        $display("地址103 (取反结果): %h", uut.dataMem.memory[103]);
        $display("地址104 (右移结果): %h", uut.dataMem.memory[104]);
        $display("地址105 (左移结果): %h", uut.dataMem.memory[105]);
        $display("地址107 (BAN测试): %h", uut.dataMem.memory[107]);
        
        $finish;
    end
    
    // 监控程序计数器和当前指令
    always @(posedge clk) begin
        if (!rst && !uut.stop)
            $display("时间=%0t, PC=%h, 指令=%h", $time, uut.pc_addr, uut.ins);
    end
endmodule

