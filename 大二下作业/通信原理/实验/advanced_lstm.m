clc;
clear; 
close all;
tic; % 开始计时 - 用于记录训练和预测时间
%% 数据处理
load Abilene
data=Abilene(1:4500);
l=15;
figure 
plot(data);
title('Abilene网络流量数据');
xlabel('时间');
ylabel('流量');
%前lags个时刻 预测下一个时刻
lags=20;
[inputs,outputs]=data_process(data,lags);

%% 采用与原始代码完全相同的数据处理方式
% 数据归一化
method=@mapminmax;
% method=@mapstd;%标准化
[xs,mappingx]=method(inputs',0,1); inputs=xs;
[ys,mappingy]=method(outputs',0,1); outputs=ys;

%% 设置训练集和测试集比例
n=length(inputs);
%训练集和测试集比例为9：1
numTimeStepsTrain =floor(0.9*n); 
input=inputs(:,1:numTimeStepsTrain);
output=outputs(1:numTimeStepsTrain);
XTest=inputs(:,numTimeStepsTrain+1:end);
YTest=outputs(:,numTimeStepsTrain+1:end);

%% 改进的lstm网络设置 - 模仿原始结构
numFeatures = lags;
numResponses = 1;
numHiddenUnits = 120; % 增加隐藏单元数

% 与原始模型结构相似，但略有改进
layers = [ ...
    sequenceInputLayer(numFeatures)
    lstmLayer(numHiddenUnits)
    dropoutLayer(0.2)
    fullyConnectedLayer(numResponses)
    regressionLayer];

% 改进的训练选项
options = trainingOptions('adam', ...
    'MaxEpochs',500, ... 
    'GradientThreshold',1, ...
    'InitialLearnRate',0.001, ... % 降低初始学习率
    'LearnRateSchedule','piecewise', ...
    'LearnRateDropPeriod',125, ...
    'LearnRateDropFactor',0.1, ... % 更激进的学习率衰减
    'Verbose',1, ... 
    'Shuffle', 'every-epoch', ... % 添加数据打乱
    'ExecutionEnvironment','cpu', ...
    'Plots','training-progress');

% 训练网络
disp('开始训练改进的LSTM网络...');
[net, info] = trainNetwork(input, output, layers, options);
trainTime = toc;
fprintf('训练完成，用时 %.2f 秒\n', trainTime);

save('improved_net_abilene_cpu.mat', 'net', 'info');
disp('改进模型已保存为 improved_net_abilene_cpu.mat');

tic; % 预测时间计时开始
% 使用与原始代码一样的预测方法
[net, YPred] = predictAndUpdateState(net, XTest, 'ExecutionEnvironment', 'cpu');
predictTime = toc;
fprintf('预测完成，用时 %.2f 秒\n', predictTime);

% 去归一化
YPred = method('reverse', YPred, mappingy);
YTest = method('reverse', YTest, mappingy);

%% 画图
figure
plot(YTest);
hold on;
plot(YPred);
legend('实际值','预测值');
title('改进LSTM的Abilene网络流量预测结果');
xlabel('时间');
ylabel('流量');
grid on;

% 画残差图
figure
residuals = YTest - YPred;
plot(residuals);
title('预测残差');
xlabel('时间');
ylabel('残差值');
grid on;

% 画散点图
figure
scatter(YTest, YPred);
hold on;
plot([min(YTest), max(YTest)], [min(YTest), max(YTest)], 'r--'); % 添加45度线
title('实际值 vs 预测值');
xlabel('实际值');
ylabel('预测值');
grid on;

%% 结果分析
disp('结果分析')
rmse=sqrt(mean((YTest-YPred).^2));
disp(['根均方差(RMSE)：',num2str(rmse)])

mae=mean(abs(YTest-YPred));
disp(['平均绝对误差（MAE）：',num2str(mae)])

mape=mean(abs((YTest-YPred)./YTest));
disp(['平均相对百分误差（MAPE）：',num2str(mape*100),'%'])

% 计算R?决定系数
SST = sum((YTest - mean(YTest)).^2);
SSE = sum((YTest - YPred).^2);
R2 = 1 - SSE/SST;
disp(['R?决定系数：',num2str(R2)])

fprintf('\n')

% 保存结果
results_improved.YTest = YTest;
results_improved.YPred = YPred;
results_improved.RMSE = rmse;
results_improved.MAE = mae;
results_improved.MAPE = mape;
results_improved.R2 = R2;
results_improved.trainTime = trainTime;
results_improved.predictTime = predictTime;
save('results_improved_abilene_cpu.mat', 'results_improved');

% 比较与原始模型性能(如果存在)
try
    load('results_abilene_cpu.mat');
    disp('与原始模型对比:')
    disp(['原始RMSE: ', num2str(results_abilene.RMSE), ' | 改进RMSE: ', num2str(rmse), ' | 改善率: ', num2str((results_abilene.RMSE-rmse)/results_abilene.RMSE*100), '%'])
    disp(['原始MAE: ', num2str(results_abilene.MAE), ' | 改进MAE: ', num2str(mae), ' | 改善率: ', num2str((results_abilene.MAE-mae)/results_abilene.MAE*100), '%'])
    disp(['原始MAPE: ', num2str(results_abilene.MAPE*100), '% | 改进MAPE: ', num2str(mape*100), '% | 改善率: ', num2str((results_abilene.MAPE-mape)/results_abilene.MAPE*100), '%'])
catch
    disp('未找到原始模型结果，无法比较')
end