module pc(clk, rst, pc);
    input wire clk;
    input wire rst;
    output reg[7:0] pc;
    
    always @(posedge clk) begin
        if(rst)
            pc <= 0;
        else
            pc <= pc + 1;
    end
endmodule
