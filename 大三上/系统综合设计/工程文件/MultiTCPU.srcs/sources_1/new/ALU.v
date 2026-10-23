`timescale 1ns / 1ps
//////////////////////////////////////////////////////////////////////////////////
// Company: 
// Engineer: 
// 
// Create Date: 2025/12/10 16:55:13
// Design Name: 
// Module Name: ALU
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
module ALU(
            input [31:0] Read1,      
            input [31:0] Read2,      
            input [31:0] imm,        
            input ALUSrcA,           
            input ALUSrcB,           
            input [4:0] sa,          
            input [3:0] ALUOp,       
            input clk,               
            input rst,               
            output wire bcon,        
            output reg [31:0] result, 
            output reg [31:0] HI,    
            output reg [31:0] LO     
);
    initial begin
        result = 0;
        HI = 0;
        LO = 0;
    end
    always @(posedge clk) begin
        if (rst) begin
            HI <= 0;
            LO <= 0;
        end
    end
    wire [31:0] A;  
    wire [31:0] B;  
    assign A[31:0] = ALUSrcA? {27'b0, sa[4:0]} : Read1[31:0];
    
    assign B[31:0] = ALUSrcB? imm[31:0] : Read2[31:0];
    assign bcon = (result? 0 : 1);
    always @(A or B or ALUOp) begin                                                                                                                                                                            
        case(ALUOp)  
            4'b0000: begin
                
                result = A + B;              
            end
            4'b0001: result = A - B;             
            4'b0010: result = A | B;             
            4'b0011: result = A & B;             
            4'b0100: result = A^B;               
            4'b0101: result = ~(A | B);          
            4'b0110: result = A << B;            
            4'b0111: result = A >> B;            
            4'b1000: result = (A < B ? 1 : 0);  
            4'b1001: begin                       
                result = $signed(A) >>> B[4:0];
            end
            4'b1010: begin                       
                result = 0;  
            end
            4'b1011: begin                       
                result = 0;  
            end
            4'b1100: result = (A > 0 ? 1 : 0);   
            4'b1101: result = (A <= 0 ? 1 : 0);  
        default: result = 0;                     
    endcase
    end
    always @(posedge clk) begin
        if (rst) begin
            HI <= 0;
            LO <= 0;
        end else begin
            case(ALUOp)
                4'b1010: begin  
                    {HI, LO} <= $signed(A) * $signed(B);
                end
                4'b1011: begin  
                    if (B != 0) begin
                        LO <= $signed(A) / $signed(B);   
                        HI <= $signed(A) % $signed(B);   
                    end
                end
            endcase
        end
    end
	 
endmodule
