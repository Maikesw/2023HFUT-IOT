module alu(
    input wire [15:0] in1,//输入的数据1
    input wire [15:0] in2,//输入的数据2
    input wire [2:0] alu_op,//操作码
    output reg [15:0] result//输出
    );
    
    parameter
    ADD = 3'b000,//加法
    SUB = 3'b001,//减法
    AND = 3'b010,//逻辑与
    OR =  3'b011,//逻辑或
    SLL = 3'b100,//逻辑左移
    SRL = 3'b101,//逻辑右移
    SLA = 3'b110,//算术左移
    SRA = 3'b111;//算术右移
    
    always @(*)
    begin
        case(alu_op)
            ADD: result = in1 + in2;
            SUB: result = in1 - in2;
            AND: result = in1 && in2;
            OR:  result = in1 || in2;
            SLL: result = in1 << in2;
            SRL: result = in1 >> in2;
            SLA: result = in1 <<< in2;
            SRA: result = in1 >>> in2;
            default: result = 5;
        endcase
    end
    
endmodule


