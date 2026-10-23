`timescale 1ns / 1ps
//////////////////////////////////////////////////////////////////////////////////
// Company: 
// Engineer: 
// 
// Create Date: 2025/12/11 16:46:33
// Design Name: 
// Module Name: CU
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
module CU(
        input clk,                  
        input rst,                  
        input [5:0] Opcode,         
        input [5:0] func,           
        output reg PCWre,           
        output reg [1:0] PCSrc,     
        output reg InsMemRead,      
        output reg RegRead,         
        output reg RegWr,           
        output reg RegDes,          
        input bcon,                 
        output reg ALUSrcA,        
        output reg ALUSrcB,         
        output reg [3:0] ALUOp,     
        output reg MemRead,         
        output reg MemWrite,        
        output reg ExtSel,          
        output reg LuiSel,          
        output reg [1:0] RegDataSel,
        output reg JalSel);          
    parameter [5:0] add = 6'b000000,  
                 sub = 6'b000001,     
                 move = 6'b000010,    
                 Or = 6'b000011,      
                 And = 6'b000100,     
                 Xor = 6'b000101,     
                 Nor = 6'b000110,     
                 sll = 6'b001000,     
                 srl = 6'b001001,     
                 sra = 6'b001010,     
                 slt = 6'b001100,     
                 mult = 6'b010010,    
                 div = 6'b010011,     
                 mfhi = 6'b010100,    
                 mflo = 6'b010101,    
                 jr = 6'b010000,      
                
                 addi = 6'b000001,    
                 ori = 6'b000100,     
                 andi = 6'b000101,    
                 xori = 6'b000110,    
                 slti = 6'b001000,    
                 lui = 6'b001001,     
                 sw = 6'b110000,      
                 lw = 6'b110001,      
                 beq = 6'b010000,     
                 bne = 6'b010001,     
                 bgtz = 6'b010010,    
                 blez = 6'b010011,    
                
                 j = 6'b011000,       
                 jal = 6'b011001,     
                 halt = 6'b111111;    
    always @(*) begin
       
        PCWre = 1;             
        InsMemRead = 1;        
        RegRead = 1;           
        if (Opcode == halt)
            PCWre = 0;
   
        if (Opcode == 6'b000000 && (func == sll || func == srl || func == sra))
            ALUSrcA = 1;       
        else
            ALUSrcA = 0;       
   
        if (Opcode == addi || Opcode == ori || Opcode == slti || Opcode == lw || Opcode == sw || Opcode == andi || Opcode == xori || Opcode == lui)
            ALUSrcB = 1;       
        else
            ALUSrcB = 0;       
   
        if (Opcode == sw || Opcode == beq || Opcode == bne || Opcode == bgtz || Opcode == blez || Opcode == j || Opcode == halt)
            RegWr = 0;
        else if (Opcode == 6'b000000 && func == jr)
            RegWr = 0;
        else
            RegWr = 1;         
   
        if (Opcode == addi || Opcode == ori || Opcode == slti || Opcode == lw || Opcode == andi || Opcode == xori || Opcode == lui)
            RegDes = 1;
        else if (Opcode == jal)
            RegDes = 1; 
        else 
            RegDes = 0;
   
        if (Opcode == lw)
            MemRead = 1;
        else
            MemRead = 0;
   
        if (Opcode == sw)
            MemWrite = 1;
        else
            MemWrite = 0;
   
        if (Opcode == addi || Opcode == lw || Opcode == sw || Opcode == beq || Opcode == bne || Opcode == slti || Opcode == bgtz || Opcode == blez)
            ExtSel = 1;
        else
            ExtSel = 0;
   
        if (Opcode == lui)
            LuiSel = 1;
        else
            LuiSel = 0;
   
        if (Opcode == j || Opcode == jal)
            PCSrc = 2'b11;
        else if (Opcode == 6'b000000 && func == jr)
            PCSrc = 2'b10;
        else if ((bcon == 1 && (Opcode == beq || Opcode == bgtz)) || (bcon == 0 && (Opcode == bne || Opcode == blez)))
            PCSrc = 2'b01;
        else
            PCSrc = 2'b00;
        if (Opcode == lw)
            RegDataSel = 2'b01; 
        else if (Opcode == 6'b000000 && func == mfhi)
            RegDataSel = 2'b10; 
        else if (Opcode == 6'b000000 && func == mflo)
            RegDataSel = 2'b11; 
        else
            RegDataSel = 2'b00; 
   
        if (Opcode == jal)
            JalSel = 1;
        else
            JalSel = 0;
        case(Opcode)
            addi,lw,sw:begin
                ALUOp = 4'b0000; 
            end
            beq,bne: begin
                ALUOp = 4'b0001; 
            end
            slti: ALUOp = 4'b1000; 
            ori: ALUOp = 4'b0010;  
            andi: ALUOp = 4'b0011; 
            xori: ALUOp = 4'b0100; 
            lui: begin
               
                ALUOp = 4'b0000; 
            end
            bgtz: ALUOp = 4'b1100; 
            blez: ALUOp = 4'b1101; 
            default: ALUOp = 4'b0000;
        endcase
        
        if(Opcode == 6'b000000)begin
            case(func)
                add,move:begin
                    ALUOp = 4'b0000; 
                end
                sub: ALUOp = 4'b0001; 
                Or: ALUOp = 4'b0010;  
                And: ALUOp = 4'b0011; 
                Xor: ALUOp = 4'b0100; 
                Nor: ALUOp = 4'b0101; 
                sll: ALUOp = 4'b0110; 
                srl: ALUOp = 4'b0111; 
                sra: ALUOp = 4'b1001; 
                slt: ALUOp = 4'b1000; 
                mult: ALUOp = 4'b1010;
                div: ALUOp = 4'b1011; 
                mfhi, mflo: ALUOp = 4'b0000; 
                default: ALUOp = 4'b0000;
            endcase
        end
   end
endmodule
