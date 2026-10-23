`timescale 1ns / 1ps
module insMem(
    input wire [11:0] addr,  
    output wire [15:0] Ins
);
    reg[15:0] insMem[4096:0];  

    initial 
    begin
        insMem[0] = 16'b0100000000000001 ;   
        insMem[1] = 16'b0101000000000000;    
        insMem[2] = 16'b0110000000000000;   
        insMem[3] = 16'b0000000000000000;   
        insMem[4] = 16'b0001000000000000;    
        insMem[5] = 16'b0010000000000000;   
        insMem[6] = 16'b0011000000000000;    
        insMem[7] = 16'b0111000000001001;   
        insMem[9] = 16'b1000000000001001;   
        insMem[10] = 16'b100100000000000;    
    end

    assign Ins = insMem[addr];

endmodule