function fitness = lstm_fitness(params, XTrain, YTrain, XTest, YTest, lags)
    % 提取参数
    numHiddenUnits = round(params(1));
    learnRate = params(2);
    dropoutRate = params(3);
    maxEpochs = round(params(4));
    
    % 构建LSTM网络
    layers = [ ...
        sequenceInputLayer(lags)
        lstmLayer(numHiddenUnits)
        dropoutLayer(dropoutRate)
        fullyConnectedLayer(1)
        regressionLayer];
    
    options = trainingOptions('adam', ...
        'MaxEpochs', maxEpochs, ...
        'InitialLearnRate', learnRate, ...
        'Verbose', 0, ...
        'Plots', 'none');
    
    
    % 训练网络
    net = trainNetwork(XTrain, YTrain, layers, options);
    
    % 预测
    YPred = predict(net, XTest);
    
    % 计算适应度(这里使用RMSE作为适应度)
    fitness = sqrt(mean((YTest - YPred).^2));
end