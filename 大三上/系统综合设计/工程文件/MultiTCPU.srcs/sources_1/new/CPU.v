`timescale 1ns / 1ps
//////////////////////////////////////////////////////////////////////////////////
// Company: 
// Engineer: 
// 
// Create Date: 2025/12/11 18:57:59
// Design Name: 
// Module Name: CPU
// Project Name: 
// Target Devices: 
// Tool Versions: 
// Description: 五级流水线CPU - 多条指令同时在不同阶段执行
// 
// Dependencies: 
// 
// Revision:
// Revision 0.01 - File Created
// Additional Comments:
// 
//////////////////////////////////////////////////////////////////////////////////

module CPU(
    input clk0,                     
    input clk,                      
    
    output reg[6:0] a_to_g,       // 数码管段选信号输出，显示ID阶段的指令
    output wire [31:0] pcAddr,     // 当前PC地址输出（IF阶段）
    output reg[7:0] an,           // 数码管位选信号输出
    output wire [31:0] instruction_ID_out);  // ID阶段指令输出（用于仿真观察）
    
    // res信号固定为0（不复位），因为使用RST按钮作为单步时钟
    wire res = 1'b0;
    
    // ==================== IF阶段信号 ====================
    wire [31:0] PC_IF;              // IF阶段的PC
    wire [31:0] instruction_IF;     // IF阶段读取的指令
    wire [31:0] PCPlus4_IF;         // IF阶段的PC+4
    
    // ==================== IF/ID流水线寄存器 ====================
    reg [31:0] instruction_ID;      // ID阶段的指令
    reg [31:0] PCPlus4_ID;          // ID阶段的PC+4
    
    // 输出ID阶段指令（用于仿真观察）
    assign instruction_ID_out = instruction_ID;
    
    // ==================== ID阶段信号 ====================
    wire [31:0] Read1_ID, Read2_ID; // ID阶段读取的寄存器数据
    wire [31:0] ExtOut_ID;          // ID阶段的立即数扩展结果
    wire [31:0] JumpAddr_ID;        // ID阶段的跳转地址
    
    // ID阶段控制信号（从CU模块生成）
    wire [1:0] PCSrc_ID;
    wire RegRead_ID;
    wire RegWr_ID;
    wire RegDes_ID;
    wire ALUSrcA_ID;
    wire ALUSrcB_ID;
    wire [3:0] ALUOp_ID;
    wire MemRead_ID;
    wire MemWrite_ID;
    wire ExtSel_ID;
    wire LuiSel_ID;
    wire [1:0] RegDataSel_ID;
    wire JalSel_ID;
    
    // ==================== ID/EXE流水线寄存器 ====================
    // 控制信号
    reg RegWr_EXE;
    reg RegDes_EXE;
    reg ALUSrcA_EXE;
    reg ALUSrcB_EXE;
    reg [3:0] ALUOp_EXE;
    reg MemRead_EXE;
    reg MemWrite_EXE;
    reg [1:0] RegDataSel_EXE;
    reg JalSel_EXE;
    // 数据
    reg [31:0] Read1_EXE, Read2_EXE;
    reg [31:0] ExtOut_EXE;
    reg [31:0] PCPlus4_EXE;
    reg [4:0] rs_EXE, rt_EXE, rd_EXE;  // 寄存器号
    reg [4:0] sa_EXE;                   // 移位量
    
    reg [31:0] instruction_EXE;         
    reg isBranch_EXE_reg, isJump_EXE_reg, isJumpReg_EXE_reg;  
    reg [31:0] BranchTarget_EXE;        
    reg predict_taken_EXE_reg;          
    wire [31:0] result_EXE;             
    wire [31:0] HI_out, LO_out;         
    wire bcon_EXE;                      
    wire [1:0] ForwardA, ForwardB;
    wire isBranch_ID;              
    wire isJump_ID;                
    wire isJumpReg_ID;             
    wire isBranch_EXE;             
    wire isJump_EXE;               
    wire isJumpReg_EXE;            
    wire predict_taken;            
    wire branch_mispredict;        
    wire flush_ID;                 
    wire [31:0] BranchTarget_ID;   
    
    wire [31:0] actual_next_PC;    
    reg RegWr_MEM;
    reg MemRead_MEM;
    reg MemWrite_MEM;
    reg [1:0] RegDataSel_MEM;
    reg JalSel_MEM;
    
    reg [31:0] ALUResult_MEM;           
    reg [31:0] WriteData_MEM;           
    reg [4:0] WriteReg_MEM;             
    reg [31:0] PCPlus4_MEM;
    wire [31:0] DataOut_MEM;            
    reg RegWr_WB;
    reg [1:0] RegDataSel_WB;
    reg JalSel_WB;
    
    reg [31:0] ALUResult_WB;            
    reg [31:0] DataMem_WB;              
    reg [4:0] WriteReg_WB;              
    reg [31:0] PCPlus4_WB;
    reg [31:0] HI_WB, LO_WB;            
    wire [31:0] WriteData_WB;           
    wire [31:0] HI_out_delayed, LO_out_delayed;
    Delay HIDR (.clk(clk), .in(HI_out), .out(HI_out_delayed));
    Delay LODR (.clk(clk), .in(LO_out), .out(LO_out_delayed));
    reg [31:0] PC_reg;
    initial begin
        PC_reg = 32'b0;
        instruction_ID = 32'b0;
        PCPlus4_ID = 32'b0;
        RegWr_EXE = 1'b0;
        RegDes_EXE = 1'b0;
        ALUSrcA_EXE = 1'b0;
        ALUSrcB_EXE = 1'b0;
        ALUOp_EXE = 4'b0;
        MemRead_EXE = 1'b0;
        MemWrite_EXE = 1'b0;
        RegDataSel_EXE = 2'b0;
        JalSel_EXE = 1'b0;
        Read1_EXE = 32'b0;
        Read2_EXE = 32'b0;
        ExtOut_EXE = 32'b0;
        PCPlus4_EXE = 32'b0;
        rs_EXE = 5'b0;
        rt_EXE = 5'b0;
        rd_EXE = 5'b0;
        sa_EXE = 5'b0;
        RegWr_MEM = 1'b0;
        MemRead_MEM = 1'b0;
        MemWrite_MEM = 1'b0;
        RegDataSel_MEM = 2'b0;
        JalSel_MEM = 1'b0;
        ALUResult_MEM = 32'b0;
        WriteData_MEM = 32'b0;
        WriteReg_MEM = 5'b0;
        PCPlus4_MEM = 32'b0;
        RegWr_WB = 1'b0;
        RegDataSel_WB = 2'b0;
        JalSel_WB = 1'b0;
        ALUResult_WB = 32'b0;
        DataMem_WB = 32'b0;
        WriteReg_WB = 5'b0;
        PCPlus4_WB = 32'b0;
        HI_WB = 32'b0;
        LO_WB = 32'b0;
    end
    wire [31:0] next_PC;  
    assign next_PC = (instruction_ID != 32'b0 && (isJump_ID || isJumpReg_ID)) ? 
                     (isJumpReg_ID ? Read1_ID : JumpAddr_ID) :  
                     (instruction_ID != 32'b0 && isBranch_ID && predict_taken) ? BranchTarget_ID :  
                     PCPlus4_IF;  
    
    always @(negedge clk) begin
        if (res)
            PC_reg <= 32'b0;
        else if (branch_mispredict)
            PC_reg <= actual_next_PC;  
        else
            PC_reg <= next_PC;  
    end
    assign PC_IF = PC_reg;
    assign pcAddr = PC_IF;
    assign PCPlus4_IF = PC_IF + 4;
    InsMem insmem (.clk(clk), .InsMemRead(1'b1), .addr(PC_IF), .ins(instruction_IF));
    always @(negedge clk) begin
        if (res) begin
            instruction_ID <= 32'b0;  
            PCPlus4_ID <= 32'b0;
        end         else if (flush_ID || branch_mispredict) begin  
            instruction_ID <= 32'b0;  
            PCPlus4_ID <= PC_IF + 4;  
        end else begin
            instruction_ID <= instruction_IF;
            PCPlus4_ID <= PCPlus4_IF;
        end
    end
    wire bcon_for_PCSrc;  
    assign bcon_for_PCSrc = 1'b0;  
    CU cu (.clk(clk), .rst(res), .Opcode(instruction_ID[31:26]), .func(instruction_ID[5:0]), 
           .PCWre(), .PCSrc(PCSrc_ID), .InsMemRead(), .RegWr(RegWr_ID), 
           .RegRead(RegRead_ID), .RegDes(RegDes_ID), .bcon(bcon_for_PCSrc), .ALUSrcA(ALUSrcA_ID), 
           .ALUSrcB(ALUSrcB_ID), .ALUOp(ALUOp_ID), 
           .MemRead(MemRead_ID), .MemWrite(MemWrite_ID), .ExtSel(ExtSel_ID), .LuiSel(LuiSel_ID),
           .RegDataSel(RegDataSel_ID), .JalSel(JalSel_ID));
    Reg reg0 (.RegRead(RegRead_ID), .RegWr(RegWr_WB), 
              .rs(instruction_ID[25:21]), .rt(instruction_ID[20:16]), .rd(instruction_ID[15:11]), 
              .RegDes(RegDes_ID), .DataALU(ALUResult_WB), .DataMem(DataMem_WB), 
              .DataHI(HI_WB), .DataLO(LO_WB), .JalPC(PCPlus4_WB), .RegDataSel(RegDataSel_WB),
              .JalSel(JalSel_WB), .Read1(Read1_ID), .Read2(Read2_ID));
    Extend extend (.in(instruction_ID[15:0]), .ExtSel(ExtSel_ID), .LuiSel(LuiSel_ID), .out(ExtOut_ID));
    wire [31:0] PC_ID_for_jump;
    assign PC_ID_for_jump = (PCPlus4_ID >= 4) ? (PCPlus4_ID - 4) : 32'b0;  
    
    wire [31:0] jump_addr_temp;
    PCAddress pcaddress (.in_addr(instruction_ID[25:0]), .PC0(PC_ID_for_jump), .addr(jump_addr_temp));
    assign JumpAddr_ID = (instruction_ID != 32'b0 && (isJump_ID || isJumpReg_ID)) ? jump_addr_temp : 32'b0;
    assign isBranch_ID = (instruction_ID[31:26] == 6'b010000) ||  
                         (instruction_ID[31:26] == 6'b010001) ||  
                         (instruction_ID[31:26] == 6'b010010) ||  
                         (instruction_ID[31:26] == 6'b010011);    
    assign isJump_ID = (instruction_ID[31:26] == 6'b011000) ||    
                       (instruction_ID[31:26] == 6'b011001);      
    assign isJumpReg_ID = (instruction_ID[31:26] == 6'b000000) && 
                          (instruction_ID[5:0] == 6'b010000);     
    wire [31:0] ExtImm_ID;  
    assign ExtImm_ID = {{16{instruction_ID[15]}}, instruction_ID[15:0]};  
    
    assign BranchTarget_ID = (instruction_ID != 32'b0) ? (PCPlus4_ID + (ExtImm_ID << 2)) : PCPlus4_ID;
    wire [31:0] PC_ID;  
    assign PC_ID = (PCPlus4_ID >= 4) ? (PCPlus4_ID - 4) : 32'b0;  
    wire actual_branch_taken;
    assign actual_branch_taken = (instruction_EXE[31:26] == 6'b010000 && bcon_EXE) ||  
                                  (instruction_EXE[31:26] == 6'b010001 && !bcon_EXE) || 
                                  (instruction_EXE[31:26] == 6'b010010 && bcon_EXE) ||  
                                  (instruction_EXE[31:26] == 6'b010011 && !bcon_EXE);   
    
    BranchPredictor branch_predictor(
        .clk(clk),
        .rst(res),
        .PC_ID(PC_ID),                          
        .isBranch_ID(isBranch_ID),              
        .predict_taken(predict_taken),          
        .PC_EXE(PCPlus4_EXE - 4),              
        .isBranch_EXE(isBranch_EXE),            
        .actual_taken(actual_branch_taken),     
        .update_enable(1'b1)                    
    );
    wire predict_taken_EXE;  
    assign actual_next_PC = (isBranch_EXE && actual_branch_taken) ? BranchTarget_EXE : PCPlus4_EXE;  
    assign branch_mispredict = isBranch_EXE && (predict_taken_EXE_reg != actual_branch_taken);
    assign flush_ID = branch_mispredict || (instruction_ID != 32'b0 && (isJump_ID || isJumpReg_ID));
    always @(negedge clk) begin
        if (res) begin
            RegWr_EXE <= 1'b0;
            RegDes_EXE <= 1'b0;
            ALUSrcA_EXE <= 1'b0;
            ALUSrcB_EXE <= 1'b0;
            ALUOp_EXE <= 4'b0;
            MemRead_EXE <= 1'b0;
            MemWrite_EXE <= 1'b0;
            RegDataSel_EXE <= 2'b0;
            JalSel_EXE <= 1'b0;
            Read1_EXE <= 32'b0;
            Read2_EXE <= 32'b0;
            ExtOut_EXE <= 32'b0;
            PCPlus4_EXE <= 32'b0;
            rs_EXE <= 5'b0;
            rt_EXE <= 5'b0;
            rd_EXE <= 5'b0;
            sa_EXE <= 5'b0;
            instruction_EXE <= 32'b0;
            isBranch_EXE_reg <= 1'b0;
            isJump_EXE_reg <= 1'b0;
            isJumpReg_EXE_reg <= 1'b0;
            BranchTarget_EXE <= 32'b0;
            predict_taken_EXE_reg <= 1'b0;
        end else begin
            RegWr_EXE <= RegWr_ID;
            RegDes_EXE <= RegDes_ID;
            ALUSrcA_EXE <= ALUSrcA_ID;
            ALUSrcB_EXE <= ALUSrcB_ID;
            ALUOp_EXE <= ALUOp_ID;
            MemRead_EXE <= MemRead_ID;
            MemWrite_EXE <= MemWrite_ID;
            RegDataSel_EXE <= RegDataSel_ID;
            JalSel_EXE <= JalSel_ID;
            Read1_EXE <= Read1_ID;
            Read2_EXE <= Read2_ID;
            ExtOut_EXE <= ExtOut_ID;
            PCPlus4_EXE <= PCPlus4_ID;
            rs_EXE <= instruction_ID[25:21];
            rt_EXE <= instruction_ID[20:16];
            rd_EXE <= instruction_ID[15:11];
            sa_EXE <= instruction_ID[10:6];
            instruction_EXE <= flush_ID ? 32'b0 : instruction_ID;  
            isBranch_EXE_reg <= flush_ID ? 1'b0 : isBranch_ID;
            isJump_EXE_reg <= flush_ID ? 1'b0 : isJump_ID;
            isJumpReg_EXE_reg <= flush_ID ? 1'b0 : isJumpReg_ID;
            BranchTarget_EXE <= BranchTarget_ID;
            predict_taken_EXE_reg <= flush_ID ? 1'b0 : predict_taken;  
        end
    end
    
    assign isBranch_EXE = isBranch_EXE_reg;
    assign isJump_EXE = isJump_EXE_reg;
    assign isJumpReg_EXE = isJumpReg_EXE_reg;
    assign predict_taken_EXE = predict_taken_EXE_reg;
    ForwardingUnit forwarding_unit(
        .EXE_rs(rs_EXE),
        .EXE_rt(rt_EXE),
        .RegWrite_MEM(RegWr_MEM),
        .WriteReg_MEM(WriteReg_MEM),
        .MemRead_MEM(MemRead_MEM),
        .RegWrite_WB(RegWr_WB),
        .WriteReg_WB(WriteReg_WB),
        .ForwardA(ForwardA),
        .ForwardB(ForwardB)
    );
    reg [31:0] ALU_A_in;
    always @(*) begin
        case(ForwardA)
            2'b00: ALU_A_in = Read1_EXE;                    
            2'b01: ALU_A_in = ALUResult_MEM;                
            2'b10: ALU_A_in = WriteData_WB;                 
            default: ALU_A_in = Read1_EXE;
        endcase
    end
    reg [31:0] Read2_forwarded;
    always @(*) begin
        case(ForwardB)
            2'b00: Read2_forwarded = Read2_EXE;             
            2'b01: Read2_forwarded = ALUResult_MEM;         
            2'b10: Read2_forwarded = WriteData_WB;          
            default: Read2_forwarded = Read2_EXE;
        endcase
    end
    wire [31:0] ALU_A, ALU_B;
    assign ALU_A = ALUSrcA_EXE ? {27'b0, sa_EXE} : ALU_A_in;
    assign ALU_B = ALUSrcB_EXE ? ExtOut_EXE : Read2_forwarded;
    ALU alu(.Read1(ALU_A), .Read2(ALU_B), .imm(32'b0), .ALUSrcA(1'b0), 
            .ALUSrcB(1'b0), .sa(5'b0), .ALUOp(ALUOp_EXE), 
            .clk(clk), .rst(res), .bcon(bcon_EXE), .result(result_EXE), 
            .HI(HI_out), .LO(LO_out));
    wire [4:0] WriteReg_EXE;
    assign WriteReg_EXE = RegDes_EXE ? rt_EXE : rd_EXE;
    always @(negedge clk) begin
        if (res) begin
            RegWr_MEM <= 1'b0;
            MemRead_MEM <= 1'b0;
            MemWrite_MEM <= 1'b0;
            RegDataSel_MEM <= 2'b0;
            JalSel_MEM <= 1'b0;
            ALUResult_MEM <= 32'b0;
            WriteData_MEM <= 32'b0;
            WriteReg_MEM <= 5'b0;
            PCPlus4_MEM <= 32'b0;
        end else begin
            RegWr_MEM <= RegWr_EXE;
            MemRead_MEM <= MemRead_EXE;
            MemWrite_MEM <= MemWrite_EXE;
            RegDataSel_MEM <= RegDataSel_EXE;
            JalSel_MEM <= JalSel_EXE;
            ALUResult_MEM <= result_EXE;
            WriteData_MEM <= Read2_forwarded;  
            WriteReg_MEM <= WriteReg_EXE;
            PCPlus4_MEM <= PCPlus4_EXE;
        end
    end
    DataMEM datamem (.MemRead(MemRead_MEM), .MemWrite(MemWrite_MEM), 
                     .Addr(ALUResult_MEM), .DataIn(WriteData_MEM), .DataOut(DataOut_MEM));
    always @(negedge clk) begin
        if (res) begin
            RegWr_WB <= 1'b0;
            RegDataSel_WB <= 2'b0;
            JalSel_WB <= 1'b0;
            ALUResult_WB <= 32'b0;
            DataMem_WB <= 32'b0;
            WriteReg_WB <= 5'b0;
            PCPlus4_WB <= 32'b0;
            HI_WB <= 32'b0;
            LO_WB <= 32'b0;
        end else begin
            RegWr_WB <= RegWr_MEM;
            RegDataSel_WB <= RegDataSel_MEM;
            JalSel_WB <= JalSel_MEM;
            ALUResult_WB <= ALUResult_MEM;
            DataMem_WB <= DataOut_MEM;
            WriteReg_WB <= WriteReg_MEM;
            PCPlus4_WB <= PCPlus4_MEM;
            HI_WB <= HI_out_delayed;
            LO_WB <= LO_out_delayed;
        end
    end
    assign WriteData_WB = (RegDataSel_WB == 2'b01) ? DataMem_WB :     
                          (RegDataSel_WB == 2'b10) ? HI_WB :          
                          (RegDataSel_WB == 2'b11) ? LO_WB :          
                          (JalSel_WB) ? PCPlus4_WB :                  
                          ALUResult_WB;                                
    reg [19:0] clkdiv_display;  
    reg [3:0] digit_display;
    initial begin
        clkdiv_display = 20'b0;
    end
    always @(posedge clk0) begin
        clkdiv_display <= clkdiv_display + 1;
    end
    always @(*) begin
        case(clkdiv_display[19:17])
            3'd0: digit_display = instruction_ID[3:0];    
            3'd1: digit_display = instruction_ID[7:4];    
            3'd2: digit_display = instruction_ID[11:8];   
            3'd3: digit_display = instruction_ID[15:12];  
            3'd4: digit_display = instruction_ID[19:16];  
            3'd5: digit_display = instruction_ID[23:20];  
            3'd6: digit_display = instruction_ID[27:24];  
            3'd7: digit_display = instruction_ID[31:28];  
            default: digit_display = instruction_ID[3:0];
        endcase
    end
    always @(*) begin
        case(digit_display)
            4'h0: a_to_g = 7'b1111110;
            4'h1: a_to_g = 7'b0110000;
            4'h2: a_to_g = 7'b1101101;
            4'h3: a_to_g = 7'b1111001;
            4'h4: a_to_g = 7'b0110011;
            4'h5: a_to_g = 7'b1011011;
            4'h6: a_to_g = 7'b1011111;
            4'h7: a_to_g = 7'b1110000;
            4'h8: a_to_g = 7'b1111111;
            4'h9: a_to_g = 7'b1111011;
            4'hA: a_to_g = 7'b1110111;
            4'hB: a_to_g = 7'b0011111;
            4'hC: a_to_g = 7'b1001110;
            4'hD: a_to_g = 7'b0111101;
            4'hE: a_to_g = 7'b1001111;
            4'hF: a_to_g = 7'b1000111;
            default: a_to_g = 7'b1111110;
        endcase
    end
    always @(*) begin
        case(clkdiv_display[19:17])
            3'd0: an = 8'b00000001;  
            3'd1: an = 8'b00000010;  
            3'd2: an = 8'b00000100;  
            3'd3: an = 8'b00001000;  
            3'd4: an = 8'b00010000;  
            3'd5: an = 8'b00100000;  
            3'd6: an = 8'b01000000;  
            3'd7: an = 8'b10000000;  
            default: an = 8'b00000001;
        endcase
    end

endmodule
