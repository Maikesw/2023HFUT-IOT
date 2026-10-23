% 随机游走小代码
close all;

Value = 3000;
for i = 1:1000
    if rand>0.5 % 是否存在简易写法？
        Value(i+1) = Value(i) + abs(randn);
    else
        Value(i+1) = Value(i) - abs(randn);
    end
end

plot(Value)
grid on;