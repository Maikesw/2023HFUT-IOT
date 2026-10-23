module simregister( );
    reg clk;
    reg we;
    reg[2:0] waddr;
    reg[15:0] wdata;
    reg re1;
    reg[2:0] raddr1;
    wire[15:0] rdata1;
    reg re2;
    reg[2:0] raddr2;
    wire[15:0] rdata2; 
    register dut(
        .clk(clk),
        .we(we),
        .waddr(waddr),
        .wdata(wdata),
        .re1(re1),
        .raddr1(raddr1),
        .rdata1(rdata1),
        .re2(re2),
        .raddr2(raddr2),
        .rdata2(rdata2)
    );
    initial begin
        clk = 1;
        we = 0;
        re1 = 0;
        re2 = 1;
        waddr = 3'b000;
        wdata = 16'h2136;
        raddr1 = 3'b000;
        #5; 
        we = 0;
        re1 = 1;
        re2 = 0;
        waddr = 3'b001;
        wdata = 16'h1111;
        raddr2 = 3'b001;
        #5;     
        we = 0;
        re1 = 1;
        re2 = 0;
        waddr = 3'b001;
        wdata = 16'h2136;
        raddr2 = 3'b001;
        #5; 
        we = 1;
        re1 = 0;
        re2 = 0;
        raddr1 = 3'b000;
        raddr2 = 3'b001;
        #5;$finish;
    end
    always #5 clk = ~clk;
endmodule
