close all;
clear all;

x = vpa(sym(pi),1e6+2);
pi_str = char(x);

for i = 3:1e6+2
    pi_bits(i-2) = str2num(pi_str(i:i));
end

% rcd = zeros(10,10);
% for i = 2:length(pi_bits)
% 	rcd(pi_bits(i-1)+1,pi_bits(i)+1) = rcd(pi_bits(i-1)+1,pi_bits(i)+1) + 1;
% end
% rcd

time_sequence(1) = 10000;
for i = 2:length(pi_bits)
    time_sequence(i) = time_sequence(i-1) + pi_bits(i-1)-4.5;
end

time_sequence_minutes = time_sequence(10:10:length(time_sequence));
plot(time_sequence_minutes); grid on;