module simmux8to1;

    reg [7:0] data_in0, data_in1, data_in2, data_in3, data_in4, data_in5, data_in6, data_in7;
    reg [2:0] sel;
    wire [7:0] data_out;
    
    mux8to1 mux_inst (
        .data_in0(data_in0),
        .data_in1(data_in1),
        .data_in2(data_in2),
        .data_in3(data_in3),
        .data_in4(data_in4),
        .data_in5(data_in5),
        .data_in6(data_in6),
        .data_in7(data_in7),
        .sel(sel),
        .data_out(data_out)
    );
    
    initial begin
        sel <= 3'b000;
        data_in0 <= 8'b00000000;
        data_in1 <= 8'b00000000;
        data_in2 <= 8'b00000000;
        data_in3 <= 8'b00000000;
        data_in4 <= 8'b00000000;
        data_in5 <= 8'b00000000;
        data_in6 <= 8'b00000000;
        data_in7 <= 8'b00000000;
        
        #200
        sel <= 3'b001;
        data_in0 <= 8'b00000001;
        data_in1 <= 8'b00000010;
        data_in2 <= 8'b00000100;
        data_in3 <= 8'b00001000;
        data_in4 <= 8'b00010000;
        data_in5 <= 8'b00100000;
        data_in6 <= 8'b01000000;
        data_in7 <= 8'b10000000;
        
        #200
        sel <= 3'b010;
        data_in0 <= 8'b00000011;
        data_in1 <= 8'b00000110;
        data_in2 <= 8'b00001100;
        data_in3 <= 8'b00011000;
        data_in4 <= 8'b00110000;
        data_in5 <= 8'b01100000;
        data_in6 <= 8'b11000000;
        data_in7 <= 8'b10000001;
        
        #200
        sel <= 3'b011;
        data_in0 <= 8'b00000111;
        data_in1 <= 8'b00001110;
        data_in2 <= 8'b00011100;
        data_in3 <= 8'b00111000;
        data_in4 <= 8'b01110000;
        data_in5 <= 8'b11100000;
        data_in6 <= 8'b11000001;
        data_in7 <= 8'b10000011;
        
        #200
        sel <= 3'b100;
        data_in0 <= 8'b00000110;
        data_in1 <= 8'b00001100;
        data_in2 <= 8'b00011000;
        data_in3 <= 8'b00110000;
        data_in4 <= 8'b01100000;
        data_in5 <= 8'b11000000;
        data_in6 <= 8'b10000001;
        data_in7 <= 8'b00000011;
        
        #200
        sel <= 3'b101;
        data_in0 <= 8'b00000101;
        data_in1 <= 8'b00001010;
        data_in2 <= 8'b00010100;
        data_in3 <= 8'b00101000;
        data_in4 <= 8'b01010000;
        data_in5 <= 8'b10100000;
        data_in6 <= 8'b01000001;
        data_in7 <= 8'b10000010;
        
        #200
        sel <= 3'b110;
        data_in0 <= 8'b00001111;
        data_in1 <= 8'b00011110;
        data_in2 <= 8'b00111100;
        data_in3 <= 8'b01111000;
        data_in4 <= 8'b11110000;
        data_in5 <= 8'b11100001;
        data_in6 <= 8'b11000011;
        data_in7 <= 8'b10000111;
        
        #200
        sel <= 3'b111;
        data_in0 <= 8'b00011111;
        data_in1 <= 8'b00111110;
        data_in2 <= 8'b01111100;
        data_in3 <= 8'b11111000;
        data_in4 <= 8'b11110001;
        data_in5 <= 8'b11100011;
        data_in6 <= 8'b11000111;
        data_in7 <= 8'b10001111;
    end

endmodule