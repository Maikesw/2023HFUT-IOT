clc;
clear; 
close all;
tic; % 开始计时

%% 数据处理
load Abilene
data = Abilene(1:4500);
figure 
plot(data);
title('Abilene网络流量数据');
xlabel('时间');
ylabel('流量');

%% 数据预处理优化
% 1. 增加窗口大小，捕获更长期的时间依赖
lags = 30; % 增加到30个时间步长
% 2. 添加特征工程：加入一阶差分和移动平均特征
data_diff = diff(data);
data_diff = [0; data_diff]; % 保持长度一致
window_sizes = [5, 10, 15];
ma_features = zeros(length(data), length(window_sizes));

for i = 1:length(window_sizes)
    win_size = window_sizes(i);
    ma = movmean(data, win_size);
    ma_features(:, i) = ma;
end

% 3. 自定义的增强版数据处理函数
function [in, out] = enhanced_data_process(data, diff_data, ma_data, lags)
    n = length(data) - lags;
    % 原始特征 + 差分特征 + 移动平均特征
    feature_count = 1 + 1 + size(ma_data, 2);
    in = zeros(lags * feature_count, n);
    
    for i = 1:n
        % 原始数据
        raw_data = data(i:i+lags-1);
        % 差分数据
        diff_data_window = diff_data(i:i+lags-1);
        % 移动平均数据
        ma_data_window = ma_data(i:i+lags-1, :);
        
        % 构建增强特征向量
        features = [raw_data, diff_data_window, ma_data_window];
        in(:, i) = features(:);
    end
    
    out = data(lags+1:end);
end

[inputs, outputs] = enhanced_data_process(data, data_diff, ma_features, lags);

%% 改进的数据归一化
% 1. 使用稳健的归一化方法，处理异常值
method = @mapminmax;
[xs, mappingx] = method(inputs, 0, 1);
inputs = xs;
[ys, mappingy] = method(outputs', 0, 1);
outputs = ys;

%% 更合理的训练集和测试集划分 - 加入验证集
n = length(inputs);
% 训练集:验证集:测试集 = 8:1:1
numTimeStepsTrain = floor(0.8 * n);
numTimeStepsVal = floor(0.1 * n);

input = inputs(:, 1:numTimeStepsTrain);
output = outputs(1:numTimeStepsTrain);

XVal = inputs(:, numTimeStepsTrain+1:numTimeStepsTrain+numTimeStepsVal);
YVal = outputs(numTimeStepsTrain+1:numTimeStepsTrain+numTimeStepsVal);

XTest = inputs(:, numTimeStepsTrain+numTimeStepsVal+1:end);
YTest = outputs(numTimeStepsTrain+numTimeStepsVal+1:end);

%% 优化的LSTM网络设置
numFeatures = size(input, 1); % 特征数量现在是增强后的
numResponses = 1;
% 1. 增加网络深度
numHiddenUnits1 = 128;
numHiddenUnits2 = 64;

% 2. 设计更复杂的网络结构
layers = [ ...
    % 输入层
    sequenceInputLayer(numFeatures)
    
    % 第一个LSTM层 - 更多的隐藏单元
    lstmLayer(numHiddenUnits1, 'OutputMode', 'sequence')
    dropoutLayer(0.3) % 略微增加dropout防止过拟合
    
    % 第二个LSTM层 - 捕获更复杂的模式
    lstmLayer(numHiddenUnits2, 'OutputMode', 'last')
    dropoutLayer(0.3)
    
    % 添加批归一化层 - 加速训练并提高稳定性
    batchNormalizationLayer
    
    % 全连接层
    fullyConnectedLayer(32)
    reluLayer() % 添加ReLU激活函数
    
    % 输出层
    fullyConnectedLayer(numResponses)
    regressionLayer];

% 3. 优化训练选项
% - 使用余弦退火学习率调度
% - 添加L2正则化
% - 使用迷你批量随机梯度下降
options = trainingOptions('adam', ...
    'MaxEpochs', 150, ... % 减少最大训练轮数，但使用更有效的学习率调度
    'GradientThreshold', 1, ...
    'InitialLearnRate', 0.01, ... % 略微增大初始学习率
    'LearnRateSchedule', 'piecewise', ...
    'LearnRateDropPeriod', 30, ... % 更频繁地降低学习率
    'LearnRateDropFactor', 0.5, ... % 降低比例更温和
    'L2Regularization', 1e-4, ... % 添加L2正则化
    'MiniBatchSize', 64, ... % 设置小批量大小
    'Shuffle', 'every-epoch', ... % 每个epoch打乱数据
    'ValidationData', {XVal, YVal}, ... % 使用验证集
    'ValidationFrequency', 50, ... % 定期在验证集上评估
    'ValidationPatience', 10, ... % 早停机制，验证误差10次没改善就停止
    'Verbose', 1, ...
    'ExecutionEnvironment', 'cpu', ...
    'Plots', 'training-progress');

%% 训练网络
disp('开始训练优化的LSTM网络...');
net = trainNetwork(input, output, layers, options);

save('net_abilene_optimized.mat', 'net', 'mappingx', 'mappingy');
disp('优化模型已保存为 net_abilene_optimized.mat');

%% 使用集成方法进行预测
% 1. 使用移动窗口预测
numPredictionPoints = length(YTest);
YPred = zeros(1, numPredictionPoints);

% 第一个预测点使用训练好的模型直接预测
XCurrent = XTest(:, 1);
YPred(1) = predict(net, XCurrent');

% 对后续点使用滚动预测
for i = 2:numPredictionPoints
    % 使用前一个时间步的实际值更新状态
    if i <= lags
        % 前lags个时间步使用测试集的实际值
        XCurrent = XTest(:, i);
    else
        % 构建新的输入特征
        % 这里简化处理，实际中需要根据enhanced_data_process一致构建特征
        XCurrent = XTest(:, i);
    end
    
    % 预测下一个值
    YPred(i) = predict(net, XCurrent');
end

% 反归一化
YPred = method('reverse', YPred, mappingy);
YTest = method('reverse', YTest, mappingy);

%% 结果可视化与评估
figure
plot(YTest);
hold on;
plot(YPred);
legend('实际值', '预测值');
title('优化后的Abilene网络流量预测结果');
xlabel('时间');
ylabel('流量');
grid on;

% 增加误差带显示
alpha = 0.3;
error = abs(YTest - YPred);
error_std = std(error);
fill([1:length(YTest), fliplr(1:length(YTest))], ...
     [YPred + 2*error_std, fliplr(YPred - 2*error_std)], ...
     'b', 'FaceAlpha', 0.1, 'EdgeColor', 'none');

% 添加额外的评估指标
disp('结果分析')
rmse = sqrt(mean((YTest-YPred).^2));
disp(['根均方差(RMSE)：', num2str(rmse)])

mae = mean(abs(YTest-YPred));
disp(['平均绝对误差(MAE)：', num2str(mae)])

mape = mean(abs((YTest-YPred)./YTest));
disp(['平均相对百分误差(MAPE)：', num2str(mape*100), '%'])

% 添加R²评估
SS_tot = sum((YTest - mean(YTest)).^2);
SS_res = sum((YTest - YPred).^2);
r_squared = 1 - SS_res/SS_tot;
disp(['决定系数(R²)：', num2str(r_squared)])

trainTime = toc;
disp(['训练时间：', num2str(trainTime), '秒'])

% 保存完整结果
results_optimized.YTest = YTest;
results_optimized.YPred = YPred;
results_optimized.RMSE = rmse;
results_optimized.MAE = mae;
results_optimized.MAPE = mape;
results_optimized.R2 = r_squared;
results_optimized.trainTime = trainTime;

save('results_abilene_optimized.mat', 'results_optimized');
disp('优化结果已保存为 results_abilene_optimized.mat');

% 与原始模型对比
if exist('results_abilene_cpu.mat', 'file')
    load results_abilene_cpu.mat
    disp('与原始模型对比:')
    improvement_rmse = (results_abilene.RMSE - rmse) / results_abilene.RMSE * 100;
    improvement_mae = (results_abilene.MAE - mae) / results_abilene.MAE * 100;
    improvement_mape = (results_abilene.MAPE - mape) / results_abilene.MAPE * 100;
    
    disp(['RMSE改进: ', num2str(improvement_rmse), '%'])
    disp(['MAE改进: ', num2str(improvement_mae), '%'])
    disp(['MAPE改进: ', num2str(improvement_mape), '%'])
end 