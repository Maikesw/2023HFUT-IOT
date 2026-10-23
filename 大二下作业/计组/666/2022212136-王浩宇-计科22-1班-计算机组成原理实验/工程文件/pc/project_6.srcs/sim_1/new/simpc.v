module simpc;
    reg clk;
    reg rst;
    wire [7:0] pc;
    
    initial begin
        clk = 1;
        
            forever begin
                if($time == 0 || $time ==80)
                    rst = 1;
                else
                    rst = 0;
                if($time >= 160) $stop;
                #5 clk = ~clk;
            end
        end
        
        pc sim_pc(
            .clk(clk),
            .rst(rst),
            .pc(pc)
    );
endmodule
