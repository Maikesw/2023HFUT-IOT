`timescale 1ns / 1ps
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