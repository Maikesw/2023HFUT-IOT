module simmux2to1;
 reg [3:0] a,b;
 reg sel;
 wire [3:0] y;
 mux2to1 unt(
 .a(a),
 .b(b),
 .sel(sel), 
 .y(y)
 );
 
 initial begin
 sel<=0;
 a <= 4'b0000;
 b <= 4'b0000;
 
 #200
 sel<=1;
 a <= 4'b0001;
 b <= 4'b1000;
 
 #200
 sel<=0;
 a <= 4'b0010;
 b <= 4'b0100; 
 
 #200
 sel<=1;
 a <= 4'b0011;
 b <= 4'b1100;

 
 end
endmodule