module simmux4to1;
    reg [3:0] a,b,c,d;
    reg [1:0] sel;
    wire [3:0] y;
    
    mux4to1 unt(
        .a(a),
        .b(b),
        .c(c),
        .d(d),
        .sel(sel),
        .y(y)
    );
    
    initial begin
        sel <= 2'b00;
        a <= 4'b0010;
        b <= 4'b0000;
        c <= 4'b0000;
        d <= 4'b0000;
        
        #200
        sel <= 2'b01;
        a <= 4'b0001;
        b <= 4'b0001;
        c <= 4'b0100;
        d <= 4'b1100;
        
        #200
        sel <= 2'b10;
        a <= 4'b0010;
        b <= 4'b0100;
        c <= 4'b0011;
        d <= 4'b1100;
        
        #200
        sel <= 2'b11;
        a <= 4'b0011;
        b <= 4'b0100;
        c <= 4'b1000;
        d <= 4'b0110;
    end
endmodule