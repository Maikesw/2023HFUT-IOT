clc;
clear; 
close all;

%% 固定随机种子
rng(42); % 设置随机种子
if canUseGPU
gpurng(42); % 固定GPU随机种子
end

%% 数据处理
load Abilene
data=Abilene(1:4500);
l=15;
figure 
plot(data);


%前lags个时刻 预测下一个时刻
lags=20;
[inputs,outputs]=data_process(data,lags);

%数据归一化
method=@mapminmax;
% method=@mapstd;%标准化
[xs,mappingx]=method(inputs',0,1);inputs=xs;
[ys,mappingy]=method(outputs',0,1);outputs=ys;

%% 设置训练集和测试集比例
n=length(inputs);

%训练集
numTimeStepsTrain =floor(0.9*n); 
%input=zeros(lags,numTimeStepsTrain);
%input=inputs(:,1:numTimeStepsTrain);
%output=outputs(1:numTimeStepsTrain);
XTrain = inputs(:, 1:numTimeStepsTrain);
YTrain = outputs(1:numTimeStepsTrain);
XTest=inputs(:,numTimeStepsTrain+1:end);
YTest=outputs(:,numTimeStepsTrain+1:end);

%% lstm网络设置
numFeatures = lags;
numResponses = 1;


%% 引入麻雀算法实现超参数优化

% 麻雀算法参数设置
pop_size = 15; % 种群大小
max_iter = 10; % 迭代次数
dim = 4; 
lb = [50, 0.001, 0.1, 100]; % 下界调整
ub = [200, 0.01, 0.5, 500]; % 上界调整

%% 麻雀算法主循环
% 初始化麻雀位置
sparrows = zeros(pop_size, dim);
for i = 1:pop_size
    sparrows(i,:) = lb + (ub-lb).*rand(1,dim);
end

% 初始化速度和最佳位置
velocity = zeros(pop_size, dim);
personal_best = sparrows;
personal_best_fitness = inf(1, pop_size);
global_best = zeros(1, dim);
global_best_fitness = inf;

% 主循环
for iter = 1:max_iter
    % 评估适应度
    for i = 1:pop_size
        current_fitness = lstm_fitness(sparrows(i,:), XTrain, YTrain, XTest, YTest, lags);
        % 更新个体最优
        if current_fitness < personal_best_fitness(i)
            personal_best_fitness(i) = current_fitness;
            personal_best(i,:) = sparrows(i,:);
        end
        % 更新全局最优
        if current_fitness < global_best_fitness
            global_best_fitness = current_fitness;
            global_best = sparrows(i,:);
        end
    end
    % 更新麻雀位置(简化版SSA)
    r1 = rand();
    r2 = rand();
    for i = 1:pop_size
        % 发现者位置更新
        if i <= pop_size*0.7 % 70%作为发现者
            sparrows(i,:) = sparrows(i,:) .* exp(-i/(r1*max_iter));
        else
            % 跟随者位置更新
            if i > pop_size/2
                sparrows(i,:) = randn() * exp((global_best - sparrows(i,:)))/i^2;
            else
                % 警戒者位置更新
                A = floor(rand(1,dim)*2)*2-1;
                sparrows(i,:) = global_best + abs(sparrows(i,:) - global_best) ...
                    * A' * (A*A')^(-1);
            end
        end
        % 边界检查
        sparrows(i,:) = max(sparrows(i,:), lb);
        sparrows(i,:) = min(sparrows(i,:), ub);
    end
    fprintf('Iteration %d, Best Fitness: %f\n', iter, global_best_fitness);
end

% 输出最优参数
fprintf('Best Parameters:\n');
fprintf('Hidden Units: %d\n', round(global_best(1)));
fprintf('Learning Rate: %f\n', global_best(2));
fprintf('Dropout Rate: %f\n', global_best(3));
fprintf('Max Epochs: %d\n', round(global_best(4)));

%% 使用优化得到的参数
best_numHiddenUnits = round(global_best(1));
best_learnRate = global_best(2);
best_dropoutRate = global_best(3);
best_maxEpochs = round(global_best(4));

final_layers = [ ...
% 输入层 输入特征维度
sequenceInputLayer(numFeatures)
% LSTM 层是一个 RNN 层，该层学习时间序列和序列数据中时间步之间的长期相关性。
lstmLayer(best_numHiddenUnits)
% 丢弃层，该运算实际上更改了迭代之间的底层网络架构，有助于防止网络过拟合。数字越大，训练过程中丢弃的元素越多。在预测时，层的输出等于其输入。
dropoutLayer(best_dropoutRate)
% 全连接层 输出维度
fullyConnectedLayer(numResponses)
regressionLayer];

%指定训练选项，求解器设置为adam， 1500 轮训练。
%梯度阈值设置为 1。指定初始学习率 0.005，在 250 轮训练后通过乘以因子 0.2 来降低学习率。
final_options = trainingOptions('adam', ...
'MaxEpochs',best_maxEpochs, ...
'Shuffle','every-epoch', ...
'GradientThreshold',1, ...
'InitialLearnRate',best_learnRate, ...
'LearnRateSchedule','piecewise', ...
'LearnRateDropPeriod',250, ...
'LearnRateDropFactor',0.2, ...
'Plots','training-progress');

net = trainNetwork(XTrain,YTrain,final_layers,final_options);

save('net');

% 利用测试集更新网络
[net,YPred] = predictAndUpdateState(net,XTest);

% 去归一化
YPred=method('reverse',YPred,mappingy);
YTest=method('reverse',YTest,mappingy);


%% 画图
figure
plot(YTest);
hold on;
plot(YPred);
disp('结果分析')
rmse=sqrt(mean((YTest-YPred).^2));
disp(['根均方差(RMSE)：',num2str(rmse)])

mae=mean(abs(YTest-YPred));
disp(['平均绝对误差（MAE）：',num2str(mae)])

mape=mean(abs((YTest-YPred)./YTest));
disp(['平均相对百分误差（MAPE）：',num2str(mape*100),'%'])

fprintf('\n')
