`timescale 1ns / 1ps
module ForwardingUnit(
    
    input [4:0] EXE_rs,          
    input [4:0] EXE_rt,          
    input RegWrite_MEM,          
    input [4:0] WriteReg_MEM,    
    input MemRead_MEM,           
    input RegWrite_WB,           
    input [4:0] WriteReg_WB,     
    output reg [1:0] ForwardA,   
    output reg [1:0] ForwardB    
);
    always @(*) begin
        
        ForwardA = 2'b00;
        ForwardB = 2'b00;
        if (RegWrite_MEM && (WriteReg_MEM != 0) && (WriteReg_MEM == EXE_rs)) begin
            
            if (!MemRead_MEM) begin
                ForwardA = 2'b01;  
            end
        end
        
        else if (RegWrite_WB && (WriteReg_WB != 0) && (WriteReg_WB == EXE_rs)) begin
            ForwardA = 2'b10;  
        end
        if (RegWrite_MEM && (WriteReg_MEM != 0) && (WriteReg_MEM == EXE_rt)) begin
            
            if (!MemRead_MEM) begin
                ForwardB = 2'b01;  
            end
        end
        
        else if (RegWrite_WB && (WriteReg_WB != 0) && (WriteReg_WB == EXE_rt)) begin
            ForwardB = 2'b10;  
        end
    end

endmodule

