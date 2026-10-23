clc;
clear; 
close all;

%% 数据处理 - 商品价格序列
disp('正在读取商品价格序列数据...');
try
    price_data = readtable('商品价格序列1.csv');
    % 查看表结构，确定价格列
    disp('数据表结构:');
    disp(price_data(1:5,:));

    % 检查数据类型
    disp('数据类型:');
    disp(varfun(@class, price_data(1,:)));

    % 选择价格列并确保转换为数值类型
    try
        % 尝试直接转换为数值数组
        data = table2array(price_data(:,2));
        
        % 如果数据是cell数组，需要转换
        if iscell(data)
            disp('数据是cell类型，尝试转换为数值...');
            data = cellfun(@str2double, data);
        end
        
        % 检查是否有NaN值(可能是由于转换非数字字符串导致)
        nan_count = sum(isnan(data));
        if nan_count > 0
            disp(['检测到 ' num2str(nan_count) ' 个NaN值，这可能是由于非数字数据转换导致的']);
        end
        
        % 显示数据信息
        disp(['数据大小: ' num2str(size(data, 1)) ' 行']);
        disp(['数据范围: ' num2str(min(data(~isnan(data)))) ' 到 ' num2str(max(data(~isnan(data))))]);
        
        % 使用nanmean来填充NaN值，这不需要fillmissing函数
        data(isnan(data)) = nanmean(data);
        disp('已使用均值填充缺失值');
        
        % 处理异常值 - 使用简单方法代替filloutliers
        mean_val = mean(data);
        std_val = std(data);
        outlier_idx = abs(data - mean_val) > 3 * std_val;
        data(outlier_idx) = mean_val;
        disp(['已处理 ' num2str(sum(outlier_idx)) ' 个异常值']);
        
    catch ME
        % 如果上述方法失败，尝试一种更简单的方法
        disp(['错误: ' ME.message]);
        disp('尝试替代方法...');
        
        % 如果price_data是table，尝试获取变量名
        varnames = price_data.Properties.VariableNames;
        disp(['表格变量名: ' strjoin(varnames, ', ')]);
        
        % 尝试将所有列转换为数值并选择第二列
        numeric_data = zeros(height(price_data), width(price_data));
        for i = 1:width(price_data)
            col_data = price_data{:,i};
            if iscell(col_data)
                numeric_data(:,i) = cellfun(@(x) str2double(regexprep(x, '[^0-9\.]', '')), col_data, 'UniformOutput', true);
            else
                numeric_data(:,i) = col_data;
            end
        end
        data = numeric_data(:,2);
        disp('已强制转换数据为数值类型');
    end

    % 移除最后的NaN值(如果还有)
    if any(isnan(data))
        data(isnan(data)) = mean(data(~isnan(data)));
        disp('已处理剩余的NaN值');
    end
catch
    % 如果CSV读取失败，创建一个随机数据集进行测试
    disp('无法读取CSV文件，创建随机数据进行测试');
    data = cumsum(randn(1000, 1));
end

% 可视化数据
figure 
plot(data);
title('商品价格序列数据');
xlabel('时间');
ylabel('价格');

% 数据量太大时可以考虑减少
if length(data) > 10000
    data = data(1:10000);
    disp('数据量已截取为10000个样本点');
end

%% 准备时间序列数据
disp('准备时间序列数据...');
% 前lags个时刻预测下一个时刻
lags = 20;
try
    [inputs, outputs] = data_process(data, lags);
    disp('数据处理成功');
catch ME
    % 如果data_process函数出错，使用内联实现
    disp(['data_process函数错误: ' ME.message]);
    disp('使用内联实现处理数据...');
    
    % 内联实现data_process函数
    n = length(data) - lags;
    inputs = zeros(lags, n);
    outputs = zeros(1, n);
    
    for i = 1:n
        inputs(:,i) = data(i:i+lags-1);
        outputs(i) = data(i+lags);
    end
    disp('数据处理完成');
end

% 数据归一化
disp('数据归一化...');
method = @mapminmax;
[xs, mappingx] = method(inputs', 0, 1); inputs = xs';
[ys, mappingy] = method(outputs', 0, 1); outputs = ys';

%% 设置训练集和测试集比例
n = size(inputs, 2);
% 训练集和测试集比例为8:2
numTimeStepsTrain = floor(0.8*n); 
input = inputs(:, 1:numTimeStepsTrain);
output = outputs(1:numTimeStepsTrain);
XTest = inputs(:, numTimeStepsTrain+1:end);
YTest = outputs(numTimeStepsTrain+1:end);

disp(['总样本数: ' num2str(n)]);
disp(['训练集样本数: ' num2str(numTimeStepsTrain)]);
disp(['测试集样本数: ' num2str(n-numTimeStepsTrain)]);

%% LSTM网络设置
numFeatures = lags;
numResponses = 1;
numHiddenUnits = 100;

layers = [ ...
    sequenceInputLayer(numFeatures)
    lstmLayer(numHiddenUnits)
    dropoutLayer(0.2)
    fullyConnectedLayer(numResponses)
    regressionLayer];

% CPU训练选项
options = trainingOptions('adam', ...
    'MaxEpochs', 500, ...
    'GradientThreshold', 1, ...
    'InitialLearnRate', 0.005, ...
    'LearnRateSchedule', 'piecewise', ...
    'LearnRateDropPeriod', 250, ...
    'LearnRateDropFactor', 0.2, ...
    'Verbose', 1, ...
    'ExecutionEnvironment', 'cpu', ...
    'MiniBatchSize', 32, ...  % 添加批量大小
    'Plots', 'training-progress');

%% 训练与预测
disp('开始使用CPU训练网络(商品价格数据)...');
tic;
try
    net = trainNetwork(input', output', layers, options);
    trainTime = toc;
    disp(['CPU训练完成，耗时: ' num2str(trainTime) ' 秒']);

    % 保存模型
    save('net_price_cpu.mat', 'net');
    disp('模型已保存为 net_price_cpu.mat');

    % CPU预测
    disp('开始预测...');
    tic;
    [net, YPred] = predictAndUpdateState(net, XTest', 'ExecutionEnvironment', 'cpu');
    predictTime = toc;
    disp(['预测完成，耗时: ' num2str(predictTime) ' 秒']);

    % 去归一化
    YPred = method('reverse', YPred', mappingy)';
    YTest = method('reverse', YTest', mappingy)';

    %% 结果分析
    figure
    plot(YTest);
    hold on;
    plot(YPred);
    legend('实际值','预测值');
    title('商品价格序列预测结果 (CPU)');
    xlabel('时间');
    ylabel('价格');
    grid on;

    % 计算评价指标
    disp('结果分析:')
    rmse = sqrt(mean((YTest-YPred).^2));
    disp(['根均方差(RMSE)：',num2str(rmse)])

    mae = mean(abs(YTest-YPred));
    disp(['平均绝对误差(MAE)：',num2str(mae)])

    mape = mean(abs((YTest-YPred)./YTest));
    disp(['平均相对百分误差(MAPE)：',num2str(mape*100),'%'])

    % 保存结果
    results_price.YTest = YTest;
    results_price.YPred = YPred;
    results_price.RMSE = rmse;
    results_price.MAE = mae;
    results_price.MAPE = mape;
    results_price.trainTime = trainTime;
    results_price.predictTime = predictTime;
    save('results_price_cpu.mat', 'results_price');
    
catch ME
    % 处理训练过程中的错误
    disp(['训练或预测过程中出错: ' ME.message]);
    disp(['错误位置: ' ME.stack(1).name ', 行 ' num2str(ME.stack(1).line)]);
    
    % 显示更多调试信息
    disp('输入数据信息:');
    disp(['输入维度: ' num2str(size(input))]);
    disp(['输出维度: ' num2str(size(output))]);
    disp(['是否包含NaN: 输入=' num2str(any(isnan(input(:)))) ', 输出=' num2str(any(isnan(output(:))))]);
    disp(['是否包含Inf: 输入=' num2str(any(isinf(input(:)))) ', 输出=' num2str(any(isinf(output(:))))]);
    
    % 尝试简化网络配置进行训练
    disp('尝试简化网络进行训练...');
    simpleLayers = [ ...
        sequenceInputLayer(numFeatures)
        lstmLayer(50)  % 减少隐藏单元
        fullyConnectedLayer(numResponses)
        regressionLayer];
    
    simpleOptions = trainingOptions('adam', ...
        'MaxEpochs', 200, ...  % 减少训练轮数
        'GradientThreshold', 1, ...
        'InitialLearnRate', 0.01, ...
        'MiniBatchSize', 32, ...
        'Verbose', 1, ...
        'ExecutionEnvironment', 'cpu', ...
        'Plots', 'training-progress');
    
    try
        disp('使用简化网络训练...');
        net = trainNetwork(input', output', simpleLayers, simpleOptions);
        
        % 如果简化网络训练成功，进行预测
        [net, YPred] = predictAndUpdateState(net, XTest', 'ExecutionEnvironment', 'cpu');
        YPred = method('reverse', YPred', mappingy)';
        YTest = method('reverse', YTest', mappingy)';
        
        figure
        plot(YTest);
        hold on;
        plot(YPred);
        legend('实际值','预测值');
        title('商品价格序列预测结果(简化网络)');
        grid on;
        
        rmse = sqrt(mean((YTest-YPred).^2));
        disp(['根均方差(RMSE)：',num2str(rmse)]);
        save('net_price_cpu_simple.mat', 'net');
    catch ME2
        disp(['简化网络训练也失败: ' ME2.message]);
    end
end