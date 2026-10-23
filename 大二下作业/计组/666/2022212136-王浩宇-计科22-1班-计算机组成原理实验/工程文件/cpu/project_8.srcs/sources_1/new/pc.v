
module pc(
   input wire clk,rst,stop,ct,uct,
   input wire [11:0] offset,
   output reg [11:0] pc
);
    
    assign stop = 0;
    
    always@(posedge clk) begin
        if(rst == 1)
            pc = 0;
        else
            pc = pc + 1;
    end
    
    always@(negedge clk) begin
        if(uct == 1)
            pc = offset - 1;
        if(ct == 1)
            pc = pc + offset - 1;
    end    
endmodule
