module simalu(
    );
    reg [15:0] in1, in2;
    reg [2:0] alu_op;
    wire [15:0] result;
    
    alu dut(
        .in1(in1),
        .in2(in2),
        .alu_op(alu_op),
        .result(result)
    );
    
    parameter
    ADD = 3'b000,
    SUB = 3'b001,
    AND = 3'b010,
    OR =  3'b011,
    SLL = 3'b100,
    SRL = 3'b101,
    SLA = 3'b110,
    SRA = 3'b111;
    
    initial begin
        in1 = 16'h2136;
        in2 = 16'h0001;
        
        //≤‚ ‘º”∑®
        alu_op = ADD;
        #10$display("in1 + in2 = %b",result);
        
        //≤‚ ‘ºı∑®
        alu_op = SUB;
        #10$display("in1 - in2 = %b",result);
        
        //≤‚ ‘¬ﬂº≠”Î
        alu_op = AND;
        #10$display("in1 && in2 = %b",result);
        
        //≤‚ ‘¬ﬂº≠ªÚ
        alu_op = OR;
        #10$display("in1 || in2 = %b",result);
        
        //≤‚ ‘¬ﬂº≠◊Û“∆
        alu_op = SLL;
        #10$display("in1 << in2 = %b",result);
        
        //≤‚ ‘¬ﬂº≠”““∆
        alu_op = SRL;
        #10$display("in1 >> in2 = %b",result);
        
        //≤‚ ‘À„ ı◊Û“∆
        alu_op = SLA;
        #10$display("in1 <<< in2 = %b",result);
        
        //≤‚ ‘À„ ı”““∆
        alu_op = SRA;
        #10$display("in1 >>> in2 = %b",result);
        
        $finish;
    end
endmodule
