clc;
clear;
close all;

% 加载两个实验结果
try
    load('results_abilene_cpu.mat');
    load('results_price_cpu.mat');
    
    % 创建对比表格
    metrics = {'RMSE', 'MAE', 'MAPE', '训练时间(秒)', '预测时间(秒)'};
    abilene_values = [results_abilene.RMSE, results_abilene.MAE, results_abilene.MAPE*100, results_abilene.trainTime, results_abilene.predictTime];
    price_values = [results_price.RMSE, results_price.MAE, results_price.MAPE*100, results_price.trainTime, results_price.predictTime];
    
    comparison_table = table(metrics', abilene_values', price_values', ...
        'VariableNames', {'评价指标', 'Abilene网络流量', '商品价格序列'});
    
    % 显示对比表格
    disp('=================== 两个数据集预测效果对比 ===================');
    disp(comparison_table);
    
    % 绘制对比图
    figure('Position', [100, 100, 1000, 800]);
    
    % Abilene预测曲线
    subplot(2,1,1);
    plot(results_abilene.YTest, 'b', 'LineWidth', 1);
    hold on;
    plot(results_abilene.YPred, 'r--', 'LineWidth', 1);
    title('Abilene网络流量预测结果对比');
    legend('实际值', '预测值');
    xlabel('时间');
    ylabel('流量');
    grid on;
    
    % 商品价格预测曲线
    subplot(2,1,2);
    plot(results_price.YTest, 'b', 'LineWidth', 1);
    hold on;
    plot(results_price.YPred, 'r--', 'LineWidth', 1);
    title('商品价格序列预测结果对比');
    legend('实际值', '预测值');
    xlabel('时间');
    ylabel('价格');
    grid on;
    
    % 保存图像
    saveas(gcf, '两数据集预测结果对比.png');
    disp('对比图已保存为: 两数据集预测结果对比.png');
    
    % 分析二者差异
    disp('=== 两个数据集的LSTM预测差异分析 ===');
    if results_abilene.RMSE < results_price.RMSE
        disp('- Abilene网络流量预测的RMSE更小，整体预测效果更好');
    else
        disp('- 商品价格序列预测的RMSE更小，整体预测效果更好');
    end
    
    if results_abilene.MAPE < results_price.MAPE
        disp('- Abilene网络流量预测的MAPE更小，相对误差更低');
    else
        disp('- 商品价格序列预测的MAPE更小，相对误差更低');
    end
    
    % 计算两个序列的自相关性
    max_lag = 50;
    [acf_abilene, lags] = autocorr(results_abilene.YTest, max_lag);
    [acf_price, ~] = autocorr(results_price.YTest, max_lag);
    
    figure;
    plot(lags, acf_abilene, 'b-o', 'LineWidth', 1);
    hold on;
    plot(lags, acf_price, 'r-x', 'LineWidth', 1);
    title('两个数据集的自相关性对比');
    legend('Abilene网络流量', '商品价格序列');
    xlabel('时间滞后');
    ylabel('自相关系数');
    grid on;
    saveas(gcf, '两数据集自相关性对比.png');
    
    % 数据特征对比
    disp('=== 数据特征对比 ===');
    abilene_std = std(results_abilene.YTest);
    price_std = std(results_price.YTest);
    fprintf('- Abilene数据标准差: %.4f, 商品价格数据标准差: %.4f\n', abilene_std, price_std);
    
    figure;
    subplot(1,2,1);
    histogram(results_abilene.YTest, 20, 'Normalization', 'probability');
    title('Abilene网络流量分布');
    xlabel('流量值');
    ylabel('概率');
    
    subplot(1,2,2);
    histogram(results_price.YTest, 20, 'Normalization', 'probability');
    title('商品价格序列分布');
    xlabel('价格值');
    ylabel('概率');
    
    sgtitle('两个数据集分布对比');
    saveas(gcf, '两数据集分布对比.png');
    
catch ME
    disp(['错误: ' ME.message]);
    disp('请先运行两个实验并保存结果');
end