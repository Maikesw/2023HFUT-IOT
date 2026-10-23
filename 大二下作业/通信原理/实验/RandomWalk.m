% 随机游走小代码
close all;

Value = 3000;
for i = 1:10000
    if rand>0.5
        Value(i+1) = Value(i) + 1;
    else
        Value(i+1) = Value(i) - 1;
    end
end

plot(Value)
grid on;