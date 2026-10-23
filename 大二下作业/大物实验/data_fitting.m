% 数据处理与拟合分析 - 悬挂法与支撑法测量数据
% 读取Excel文件中的数据并进行多项式拟合分析

clear all;
close all;
clc;

% 设置中文显示
set(0,'DefaultAxesFontName','宋体');
set(0,'DefaultTextFontName','宋体');

% 读取Excel文件数据
filename = '悬挂法测量数据_新版.xlsx';
try
    % 读取表1数据 - 悬挂法
    [~, ~, raw1] = xlsread(filename, '表1');
    % 从第3行开始读取数据
    left_suspend_pos = cell2mat(raw1(3:8, 2)); % 左悬挂点位置
    right_suspend_pos = cell2mat(raw1(3:8, 3)); % 右悬挂点位置
    % 假设频率数据已经填写
    % 注意：实际使用时请确保频率数据已填写，否则以下代码会报错
    brass_freq_suspend = cell2mat(raw1(3:8, 4)); % 黄铜棒悬挂共振频率
    steel_freq_suspend = cell2mat(raw1(3:8, 5)); % 不锈钢棒悬挂共振频率
    
    % 读取表2数据 - 支撑法
    [~, ~, raw2] = xlsread(filename, '表2');
    % 从第3行开始读取数据
    left_support_pos = cell2mat(raw2(3:8, 2)); % 左支撑点位置
    right_support_pos = cell2mat(raw2(3:8, 3)); % 右支撑点位置
    brass_freq_support = cell2mat(raw2(3:8, 4)); % 黄铜棒支撑共振频率
    
    % 读取表3数据 - 样品参数
    [~, ~, raw3] = xlsread(filename, '表3');
    % 提取样品参数
    brass_length = cell2mat(raw3(4, 2)) * 1e-2; % 样品长度 (m)
    brass_diameter = cell2mat(raw3(5, 2)) * 1e-3; % 样品直径 (m)
    brass_mass = cell2mat(raw3(6, 2)) * 1e-3; % 样品质量 (kg)
    steel_length = cell2mat(raw3(4, 3)) * 1e-2; % 样品长度 (m)
    steel_diameter = cell2mat(raw3(5, 3)) * 1e-3; % 样品直径 (m)
    steel_mass = cell2mat(raw3(6, 3)) * 1e-3; % 样品质量 (kg)
    
    % 计算悬挂点相对位置 (x/L)
    brass_suspend_rel_pos = left_suspend_pos / brass_length;
    
    % 计算支撑点相对位置 (x/L)
    brass_support_rel_pos = left_support_pos / brass_length;
    
    % 计算频率平方 (理论上与悬挂点位置存在二次关系)
    brass_freq_suspend_square = brass_freq_suspend.^2;
    steel_freq_suspend_square = steel_freq_suspend.^2;
    brass_freq_support_square = brass_freq_support.^2;
    
    % 多项式拟合 - 悬挂法黄铜棒
    % 使用相对位置与频率平方之间的二次关系进行拟合
    figure(1);
    p_brass_suspend = polyfit(brass_suspend_rel_pos, brass_freq_suspend_square, 2);
    x_fit = linspace(min(brass_suspend_rel_pos), max(brass_suspend_rel_pos), 100);
    y_fit = polyval(p_brass_suspend, x_fit);
    
    plot(brass_suspend_rel_pos, brass_freq_suspend_square, 'ro', 'MarkerSize', 8, 'LineWidth', 1.5);
    hold on;
    plot(x_fit, y_fit, 'b-', 'LineWidth', 1.5);
    grid on;
    xlabel('悬挂点相对位置 (x/L)');
    ylabel('频率平方 (Hz^2)');
    title('黄铜棒悬挂法共振频率平方与悬挂点位置关系');
    legend('实验数据', '多项式拟合');
    
    % 多项式拟合 - 悬挂法不锈钢棒
    figure(2);
    p_steel_suspend = polyfit(brass_suspend_rel_pos, steel_freq_suspend_square, 2);
    y_fit_steel = polyval(p_steel_suspend, x_fit);
    
    plot(brass_suspend_rel_pos, steel_freq_suspend_square, 'ro', 'MarkerSize', 8, 'LineWidth', 1.5);
    hold on;
    plot(x_fit, y_fit_steel, 'b-', 'LineWidth', 1.5);
    grid on;
    xlabel('悬挂点相对位置 (x/L)');
    ylabel('频率平方 (Hz^2)');
    title('不锈钢棒悬挂法共振频率平方与悬挂点位置关系');
    legend('实验数据', '多项式拟合');
    
    % 多项式拟合 - 支撑法黄铜棒
    figure(3);
    p_brass_support = polyfit(brass_support_rel_pos, brass_freq_support_square, 2);
    y_fit_support = polyval(p_brass_support, x_fit);
    
    plot(brass_support_rel_pos, brass_freq_support_square, 'ro', 'MarkerSize', 8, 'LineWidth', 1.5);
    hold on;
    plot(x_fit, y_fit_support, 'b-', 'LineWidth', 1.5);
    grid on;
    xlabel('支撑点相对位置 (x/L)');
    ylabel('频率平方 (Hz^2)');
    title('黄铜棒支撑法共振频率平方与支撑点位置关系');
    legend('实验数据', '多项式拟合');
    
    % 根据拟合结果计算杨氏模量
    % 杨氏模量计算公式：E = (4*pi^2*rho*L^4*f^2)/(β^4)
    % 其中β是与边界条件相关的系数
    
    % 计算黄铜密度
    brass_radius = brass_diameter / 2;
    brass_volume = pi * brass_radius^2 * brass_length;
    brass_density = brass_mass / brass_volume;
    
    % 计算不锈钢密度
    steel_radius = steel_diameter / 2;
    steel_volume = pi * steel_radius^2 * steel_length;
    steel_density = steel_mass / steel_volume;
    
    % 打印拟合结果和密度计算结果
    fprintf('黄铜棒悬挂法多项式拟合系数：a=%f, b=%f, c=%f\n', p_brass_suspend);
    fprintf('不锈钢棒悬挂法多项式拟合系数：a=%f, b=%f, c=%f\n', p_steel_suspend);
    fprintf('黄铜棒支撑法多项式拟合系数：a=%f, b=%f, c=%f\n', p_brass_support);
    
    fprintf('黄铜棒密度：%.2f kg/m^3\n', brass_density);
    fprintf('不锈钢棒密度：%.2f kg/m^3\n', steel_density);
    
    % 计算拟合优度R^2
    % 悬挂法黄铜棒
    y_mean = mean(brass_freq_suspend_square);
    SS_tot = sum((brass_freq_suspend_square - y_mean).^2);
    SS_res = sum((brass_freq_suspend_square - polyval(p_brass_suspend, brass_suspend_rel_pos)).^2);
    R2_brass_suspend = 1 - SS_res/SS_tot;
    fprintf('黄铜棒悬挂法拟合优度R^2：%.4f\n', R2_brass_suspend);
    
    % 悬挂法不锈钢棒
    y_mean = mean(steel_freq_suspend_square);
    SS_tot = sum((steel_freq_suspend_square - y_mean).^2);
    SS_res = sum((steel_freq_suspend_square - polyval(p_steel_suspend, brass_suspend_rel_pos)).^2);
    R2_steel_suspend = 1 - SS_res/SS_tot;
    fprintf('不锈钢棒悬挂法拟合优度R^2：%.4f\n', R2_steel_suspend);
    
    % 支撑法黄铜棒
    y_mean = mean(brass_freq_support_square);
    SS_tot = sum((brass_freq_support_square - y_mean).^2);
    SS_res = sum((brass_freq_support_square - polyval(p_brass_support, brass_support_rel_pos)).^2);
    R2_brass_support = 1 - SS_res/SS_tot;
    fprintf('黄铜棒支撑法拟合优度R^2：%.4f\n', R2_brass_support);
    
catch e
    fprintf('错误：%s\n', e.message);
    fprintf('请确保Excel文件中的数据已填写完整，尤其是频率数据\n');
end 