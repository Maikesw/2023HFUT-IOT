module mux8to1 (
    input [7:0] data_in0,
    input [7:0] data_in1,
    input [7:0] data_in2,
    input [7:0] data_in3,
    input [7:0] data_in4,
    input [7:0] data_in5,
    input [7:0] data_in6,
    input [7:0] data_in7,
    input [2:0] sel,
    output reg [7:0] data_out
);

always @ (sel or data_in0 or data_in1 or data_in2 or data_in3 or data_in4 or data_in5 or data_in6 or data_in7)
begin
    case(sel)
        3'b000: data_out = data_in0;
        3'b001: data_out = data_in1;
        3'b010: data_out = data_in2;
        3'b011: data_out = data_in3;
        3'b100: data_out = data_in4;
        3'b101: data_out = data_in5;
        3'b110: data_out = data_in6;
        3'b111: data_out = data_in7;
    endcase
end

endmodule