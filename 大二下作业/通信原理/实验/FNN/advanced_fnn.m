clc;
clear; 
close all;
tic; % 开始计时

%% 固定随机种子
rng(42); % 设置随机种子
if canUseGPU
gpurng(42); % 固定GPU随机种子
end
%% 数据处理与可视化
load Abilene
data = Abilene(1:4500);
figure 
plot(data);
title('Abilene网络流量数据');
xlabel('时间');
ylabel('流量');

%% 高级数据预处理与特征工程
% 1. 多尺度时间窗口
main_lags = 30;  % 主要时间窗口

% 2. 简化特征工程 - 减少特征数量，避免维度过大
% 2.1 时域特征
data_diff1 = diff(data);  % 一阶差分
data_diff1 = [0; data_diff1];  

% 2.2 简化的移动平均 - 只用两个窗口大小
window_sizes = [5, 15];
ma_features = zeros(length(data), length(window_sizes));
for i = 1:length(window_sizes)
    win_size = window_sizes(i);
    ma_features(:, i) = movmean(data, win_size);
end

% 2.3 简化异常值检测
z_scores = abs((data - mean(data)) / std(data));
outlier_indices = find(z_scores > 3);
data_clean = data;
for i = 1:length(outlier_indices)
    idx = outlier_indices(i);
    if idx > 1 && idx < length(data)
        data_clean(idx) = (data(idx-1) + data(idx+1)) / 2;
    end
end

% 调用特征工程函数，但使用更简单的特征集
[inputs, outputs] = simplified_feature_engineering(data, data_clean, data_diff1, ma_features, main_lags);

% 检查特征维度
disp(['特征维度: ', num2str(size(inputs, 1))]);

%% 先进的数据归一化与标准化
% 对输入特征应用标准归一化 (而非鲁棒归一化)
method = @mapminmax;
[inputs_norm, input_params] = method(inputs, 0, 1);
[outputs_norm, output_params] = method(outputs', 0, 1);
inputs = inputs_norm;
outputs = outputs_norm';

%% 数据集划分
n = length(inputs);
% 训练集:验证集:测试集 = 7:2:1
numTimeStepsTrain = floor(0.7 * n);
numTimeStepsVal = floor(0.2 * n);

% 划分数据集
X_train = inputs(:, 1:numTimeStepsTrain);
Y_train = outputs(1:numTimeStepsTrain);

X_val = inputs(:, numTimeStepsTrain+1:numTimeStepsTrain+numTimeStepsVal);
Y_val = outputs(numTimeStepsTrain+1:numTimeStepsTrain+numTimeStepsVal);

X_test = inputs(:, numTimeStepsTrain+numTimeStepsVal+1:end);
Y_test = outputs(numTimeStepsTrain+numTimeStepsVal+1:end);

%% 先进的神经网络架构 - 最大兼容性方案
numFeatures = size(X_train, 1);
disp(['实际特征维度: ', num2str(numFeatures)]);
numResponses = 1;

% 检查MATLAB版本以便诊断
disp(['MATLAB版本: ', version]);

% 尝试创建最简单的神经网络结构 - 使用fitnet函数
% 这是MATLAB中最基本的神经网络创建方式之一
disp('使用fitnet创建神经网络...');
net = fitnet([128, 64, 32]);  % 三层隐藏层

% 设置网络参数
net.trainFcn = 'trainscg';  % 使用梯度下降训练
net.performFcn = 'mse';     % 使用均方误差作为损失函数
net.divideFcn = 'dividerand'; % 随机划分数据
net.divideMode = 'sample';    % 按样本划分
net.divideParam.trainRatio = 0.7;  % 70%用于训练
net.divideParam.valRatio = 0.15;   % 15%用于验证
net.divideParam.testRatio = 0.15;  % 15%用于测试
net.trainParam.epochs = 500;       % 最大训练轮数
net.trainParam.goal = 1e-5;        % 目标误差
net.trainParam.max_fail = 15;      % 验证检查失败最大次数

% 显示网络结构
disp('网络结构创建完成');
view(net);

%% 训练网络
disp('开始训练');

% 准备用于训练的数据
% 检查数据维度，确保格式正确
disp(['X_train维度: ', num2str(size(X_train,1)), ' x ', num2str(size(X_train,2))]);
disp(['Y_train维度: ', num2str(size(Y_train,1)), ' x ', num2str(size(Y_train,2))]);

% 在神经网络工具箱中，输入应该是[特征数 x 样本数]格式，目标应该是[输出数 x 样本数]格式
% 确保Y_train是行向量
if size(Y_train, 1) > 1
    Y_train_matrix = Y_train';  % 转置为行向量
else
    Y_train_matrix = Y_train;  % 已经是行向量
end

% 确保X_train是[特征数 x 样本数]格式
X_train_matrix = X_train;  % 已经是正确格式

% 打印调整后的维度
disp(['调整后X_train_matrix维度: ', num2str(size(X_train_matrix,1)), ' x ', num2str(size(X_train_matrix,2))]);
disp(['调整后Y_train_matrix维度: ', num2str(size(Y_train_matrix,1)), ' x ', num2str(size(Y_train_matrix,2))]);

% 确保样本数匹配
if size(X_train_matrix, 2) ~= size(Y_train_matrix, 2)
    disp('错误：调整后输入和目标样本数仍不匹配!');
    disp('尝试强制匹配样本数...');
    
    % 确定最小样本数
    min_samples = min(size(X_train_matrix, 2), size(Y_train_matrix, 2));
    X_train_matrix = X_train_matrix(:, 1:min_samples);
    Y_train_matrix = Y_train_matrix(:, 1:min_samples);
    
    disp(['最终X_train_matrix维度: ', num2str(size(X_train_matrix,1)), ' x ', num2str(size(X_train_matrix,2))]);
    disp(['最终Y_train_matrix维度: ', num2str(size(Y_train_matrix,1)), ' x ', num2str(size(Y_train_matrix,2))]);
end

% 使用旧版train函数训练网络
[trainedNet, tr] = train(net, X_train_matrix, Y_train_matrix);

% 保存模型
save('simple_nn_model.mat', 'trainedNet', 'tr', 'input_params', 'output_params');
disp('已保存为 simple_nn_model.mat');

%% 预测
% 准备测试数据
disp(['X_test维度: ', num2str(size(X_test,1)), ' x ', num2str(size(X_test,2))]);

% 确保X_test格式与X_train_matrix一致
X_test_matrix = X_test;  % 维度已匹配

disp(['X_test_matrix维度: ', num2str(size(X_test_matrix,1)), ' x ', num2str(size(X_test_matrix,2))]);

% 使用旧版sim函数进行预测
Y_pred_matrix = sim(trainedNet, X_test_matrix);
disp(['Y_pred_matrix维度: ', num2str(size(Y_pred_matrix,1)), ' x ', num2str(size(Y_pred_matrix,2))]);

% 反归一化
Y_pred_original = method('reverse', Y_pred_matrix, output_params);
Y_test_original = method('reverse', Y_test', output_params)';

% 检查维度并确保匹配
disp(['Y_pred_original维度: ', num2str(size(Y_pred_original,1)), ' x ', num2str(size(Y_pred_original,2))]);
disp(['Y_test_original维度: ', num2str(size(Y_test_original,1)), ' x ', num2str(size(Y_test_original,2))]);

% 确保两者都是列向量或行向量，以便正确计算误差
if ~isequal(size(Y_pred_original), size(Y_test_original))
    disp('警告: Y_pred_original和Y_test_original维度不匹配，尝试调整...');
    
    % 确保两者都是列向量
    if size(Y_pred_original, 1) == 1
        Y_pred_original = Y_pred_original';  % 转置为列向量
    end
    
    if size(Y_test_original, 1) == 1
        Y_test_original = Y_test_original';  % 转置为列向量
    end
    
    % 如果样本数不同，截取较小的部分
    min_samples = min(length(Y_pred_original), length(Y_test_original));
    Y_pred_original = Y_pred_original(1:min_samples);
    Y_test_original = Y_test_original(1:min_samples);
    
    disp(['调整后Y_pred_original维度: ', num2str(size(Y_pred_original,1)), ' x ', num2str(size(Y_pred_original,2))]);
    disp(['调整后Y_test_original维度: ', num2str(size(Y_test_original,1)), ' x ', num2str(size(Y_test_original,2))]);
end

%% 高级可视化与评估
figure('Position', [100, 100, 1200, 600])

% 1. 预测结果可视化
subplot(2, 2, [1, 2])
plot(Y_test_original, 'LineWidth', 1.5);
hold on;
plot(Y_pred_original, 'LineWidth', 1.5);
legend('实际值', '预测值', 'FontSize', 12);
title('神经网络模型预测结果', 'FontSize', 14);
xlabel('时间', 'FontSize', 12);
ylabel('流量', 'FontSize', 12);
grid on;

% 添加置信区间
% 确保error计算不会出错
error = abs(Y_test_original - Y_pred_original);
error_std = std(error);

% 如果是列向量，需要转置为行向量用于填充
if size(Y_pred_original, 2) == 1
    Y_pred_row = Y_pred_original';
    error_bounds_upper = Y_pred_row + 2*error_std;
    error_bounds_lower = Y_pred_row - 2*error_std;
else
    error_bounds_upper = Y_pred_original + 2*error_std;
    error_bounds_lower = Y_pred_original - 2*error_std;
end

x_values = 1:length(Y_pred_original);
fill([x_values, fliplr(x_values)], ...
     [error_bounds_upper, fliplr(error_bounds_lower)], ...
     'b', 'FaceAlpha', 0.1, 'EdgeColor', 'none');

% 2. 误差分析
subplot(2, 2, 3)
histogram(Y_test_original - Y_pred_original, 20, 'Normalization', 'probability');
title('预测误差分布', 'FontSize', 12);
xlabel('误差', 'FontSize', 10);
ylabel('频率', 'FontSize', 10);
grid on;

% 3. 实际值与预测值散点图
subplot(2, 2, 4)
scatter(Y_test_original, Y_pred_original, 25, 'filled');
hold on;
min_val = min(min(Y_test_original), min(Y_pred_original));
max_val = max(max(Y_test_original), max(Y_pred_original));
plot([min_val, max_val], [min_val, max_val], 'r--', 'LineWidth', 1.5);
title('实际值 vs 预测值', 'FontSize', 12);
xlabel('实际值', 'FontSize', 10);
ylabel('预测值', 'FontSize', 10);
grid on;

%% 全面的评估指标
disp('全面结果分析:')

% 1. 基础指标
rmse = sqrt(mean((Y_test_original - Y_pred_original).^2));
mae = mean(abs(Y_test_original - Y_pred_original));
mape = mean(abs((Y_test_original - Y_pred_original)./Y_test_original)) * 100;

% 2. 高级指标
% R?评估
SS_tot = sum((Y_test_original - mean(Y_test_original)).^2);
SS_res = sum((Y_test_original - Y_pred_original).^2);
r_squared = 1 - SS_res/SS_tot;

% 调整后的R?
n_samples = length(Y_test_original);
p_features = numFeatures;  % 特征数
adj_r_squared = 1 - (1-r_squared)*(n_samples-1)/(n_samples-p_features-1);

% 赤池信息准则 (AIC)
aic = n_samples * log(SS_res/n_samples) + 2 * p_features;

% 时序特定指标 - 方向准确度
direction_accuracy = mean(sign(diff(Y_test_original)) == sign(diff(Y_pred_original))) * 100;

% 3. 打印所有指标
disp(['均方根误差 (RMSE): ', num2str(rmse)])
disp(['平均绝对误差 (MAE): ', num2str(mae)])
disp(['平均绝对百分比误差 (MAPE): ', num2str(mape), '%'])
disp(['决定系数 (R?): ', num2str(r_squared)])
disp(['调整后的决定系数 (Adj-R?): ', num2str(adj_r_squared)])
disp(['赤池信息准则 (AIC): ', num2str(aic)])
disp(['方向准确度: ', num2str(direction_accuracy), '%'])

trainTime = toc;
disp(['总执行时间: ', num2str(trainTime), '秒'])

% 4. 与原始模型对比
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

% 5. 保存详细结果
results_advanced = struct();
results_advanced.YTest = Y_test_original;
results_advanced.YPred = Y_pred_original;
results_advanced.RMSE = rmse;
results_advanced.MAE = mae;
results_advanced.MAPE = mape;
results_advanced.R2 = r_squared;
results_advanced.AdjR2 = adj_r_squared;
results_advanced.AIC = aic;
results_advanced.DirectionAccuracy = direction_accuracy;
results_advanced.trainTime = trainTime;

save('results_advanced_lstm.mat', 'results_advanced');
disp('先进模型结果已保存为 results_advanced_lstm.mat');

%% 以下是所有函数定义 - MATLAB要求函数定义必须放在脚本末尾

% 特征工程函数
function [in, out] = simplified_feature_engineering(data, data_clean, diff1, ma_data, lags)
    n = length(data) - lags;
    
    % 计算特征总数 - 大幅减少特征数量
    feature_count = 1 + 1 + 1 + size(ma_data, 2);  % 原始 + 清洗 + 差分 + 移动平均
    total_features = lags * feature_count;
    
    in = zeros(total_features, n);
    
    for i = 1:n
        % 原始和清洗后的数据
        raw_window = data(i:i+lags-1);
        clean_window = data_clean(i:i+lags-1);
        
        % 差分特征
        diff1_window = diff1(i:i+lags-1);
        
        % 移动平均特征
        ma_window = ma_data(i:i+lags-1, :);
        
        % 组合特征 - 大幅简化
        features = [raw_window, clean_window, diff1_window, ma_window];
        in(:, i) = features(:);
    end
    
    out = data(lags+1:end);
end 