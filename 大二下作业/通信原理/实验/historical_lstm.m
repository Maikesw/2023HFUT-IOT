clc;
clear; 
close all;
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

%数据归一化
method=@mapminmax;
% method=@mapstd;%标准化
[xs,mappingx]=method(inputs',0,1);inputs=xs;
[ys,mappingy]=method(outputs',0,1);outputs=ys;

%% 设置训练集和测试集比例
n=length(inputs);
%训练集和测试集比例为9：1
numTimeStepsTrain =floor(0.9*n); 
%input=zeros(lags,numTimeStepsTrain);
input=inputs(:,1:numTimeStepsTrain);
output=outputs(1:numTimeStepsTrain);
XTest=inputs(:,numTimeStepsTrain+1:end);
YTest=outputs(:,numTimeStepsTrain+1:end);

%% lstm网络设置
numFeatures = lags;
numResponses = 1;
numHiddenUnits = 100;

layers = [ ...
    % 输入层  输入特征维度
    sequenceInputLayer(numFeatures)
    % LSTM 层是一个 RNN 层，该层学习时间序列和序列数据中时间步之间的长期相关性。
    lstmLayer(numHiddenUnits)
    % 丢弃层，该运算实际上更改了迭代之间的底层网络架构，有助于防止网络过拟合。数字越大，训练过程中丢弃的元素越多。在预测时，层的输出等于其输入。
     dropoutLayer(0.2)
    % 全连接层  输出维度
    fullyConnectedLayer(numResponses)
    regressionLayer];

    %指定训练选项，求解器设置为adam， 1500 轮训练。
    %梯度阈值设置为 1。指定初始学习率 0.005，在 250 轮训练后通过乘以因子 0.2 来降低学习率。
options = trainingOptions('adam', ...
    'MaxEpochs',500, ...
    'GradientThreshold',1, ...
    'InitialLearnRate',0.005, ...
    'LearnRateSchedule','piecewise', ...
    'LearnRateDropPeriod',250, ...
    'LearnRateDropFactor',0.2, ...
    'Verbose',1, ...  % 设为1显示训练进度
    'ExecutionEnvironment','cpu', ...  % 明确指定CPU
    'Plots','training-progress');

    %训练网络
  net = trainNetwork(input,output,layers,options);

  save('net_abilene_cpu.mat', 'net');
  disp('模型已保存为 net_abilene_cpu.mat');

    % 利用测试集更新网络
[net,YPred] = predictAndUpdateState(net,XTest,'ExecutionEnvironment','cpu');

    % 去归一化
YPred=method('reverse',YPred,mappingy);
YTest=method('reverse',YTest,mappingy);

%% 画图
figure
plot(YTest);
hold on;
plot(YPred);
legend('实际值','预测值');
title('Abilene网络流量预测结果');
xlabel('时间');
ylabel('流量');
grid on;

disp('结果分析')
rmse=sqrt(mean((YTest-YPred).^2));
disp(['根均方差(RMSE)：',num2str(rmse)])

mae=mean(abs(YTest-YPred));
disp(['平均绝对误差（MAE）：',num2str(mae)])

mape=mean(abs((YTest-YPred)./YTest));
disp(['平均相对百分误差（MAPE）：',num2str(mape*100),'%'])

fprintf('\n')

% 保存结果
results_abilene.YTest = YTest;
results_abilene.YPred = YPred;
results_abilene.RMSE = rmse;
results_abilene.MAE = mae;
results_abilene.MAPE = mape;
results_abilene.trainTime = toc;
results_abilene.predictTime = toc;
save('results_abilene_cpu.mat', 'results_abilene');




