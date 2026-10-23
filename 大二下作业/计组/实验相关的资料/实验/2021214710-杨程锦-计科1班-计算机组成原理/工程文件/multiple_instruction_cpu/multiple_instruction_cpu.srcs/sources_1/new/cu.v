`timescale 1ns / 1ps
module cu(
    input wire [3:0] operate,    
    output reg stop, uct, acc_wr, dataMem_wr, 
    output reg [3:0] alu_op
);
    initial 
    begin
      stop = 0;
      uct = 0;
      acc_wr = 0;
      dataMem_wr = 0;
      alu_op = 4'b1111;
    end

    always @(operate) 
    begin
      case(operate)
        4'b0000: {stop,uct,acc_wr,dataMem_wr,alu_op} = 8'b00100000;   
        4'b0001: {stop,uct,acc_wr,dataMem_wr,alu_op} = 8'b00100001;      
        4'b0010: {stop,uct,acc_wr,dataMem_wr,alu_op} = 8'b00100010;   
        4'b0011: {stop,uct,acc_wr,dataMem_wr,alu_op} = 8'b00100011;   
        4'b0100: {stop,uct,acc_wr,dataMem_wr,alu_op} = 8'b00100100;   
        4'b0101: {stop,uct,acc_wr,dataMem_wr,alu_op} = 8'b00010101;   
        4'b0110: {stop,uct,acc_wr,dataMem_wr,alu_op} = 8'b00100110;   
        4'b0111: {stop,uct,acc_wr,dataMem_wr,alu_op} = 8'b01000111;   
        4'b1000: {stop,uct,acc_wr,dataMem_wr,alu_op} = 8'b00001000;  
        4'b1001: {stop,uct,acc_wr,dataMem_wr,alu_op} = 8'b10000000;   
        endcase
    end

endmodule