% 悬挂法与支撑法测量数据的多项式拟合演示
% 使用模拟数据进行分析

clear all;
close all;
clc;

% 设置中文显示
set(0,'DefaultAxesFontName','宋体');
set(0,'DefaultTextFontName','宋体');

% 悬挂点位置数据
left_suspend_pos = [5.84, 15.84, 25.84, 35.84, 45.84, 55.84];

% 用户提供的真实频率数据
% 注意：第四个位置是'/'，表示没有数据，需要排除
brass_freq_suspend_full = [707.8, 706.3, 705.6, NaN, 705.3, 706.2]; % 黄铜棒频率
steel_freq_suspend_full = [1044, 1041, 1039, NaN, 1038, 1040]; % 不锈钢棒频率

% 排除NaN值
valid_indices = ~isnan(brass_freq_suspend_full);
left_suspend_pos_valid = left_suspend_pos(valid_indices);
brass_freq_suspend = brass_freq_suspend_full(valid_indices);
steel_freq_suspend = steel_freq_suspend_full(valid_indices);

% 支撑法的数据 - 因为没有提供，继续使用模拟数据
left_support_pos = [5.84, 15.84, 25.84, 35.84, 45.84, 55.84];
brass_freq_support = [160, 175, 190, 195, 185, 165]; % 模拟黄铜棒支撑频率

% 样品参数
brass_length = 0.16; % m
brass_diameter = 0.006; % m
brass_mass = 0.0366; % kg
steel_length = 0.16; % m
steel_diameter = 0.006; % m
steel_mass = 0.035; % kg

% 计算悬挂点相对位置 (x/L)
brass_suspend_rel_pos = left_suspend_pos_valid / brass_length;

% 计算支撑点相对位置 (x/L)
brass_support_rel_pos = left_support_pos / brass_length;

% 计算频率平方 (理论上与悬挂点位置存在二次关系)
brass_freq_suspend_square = brass_freq_suspend.^2;
steel_freq_suspend_square = steel_freq_suspend.^2;
brass_freq_support_square = brass_freq_support.^2;

% 多项式拟合 - 悬挂法黄铜棒
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

% 简单计算杨氏模量
% 对于悬挂法，我们可以使用公式 E = (48*pi^2*rho*L^4*f^2)/(beta^4*d^2)
% 其中beta是与悬挂位置相关的系数
brass_I = (pi * brass_diameter^4) / 64; % 惯性矩
brass_A = pi * brass_radius^2; % 横截面积

% 假设在中点处的悬挂频率
mid_index = 3; % 假设第3个数据点接近中点
brass_mid_freq = brass_freq_suspend(mid_index);
beta_value = 4.73; % 对于悬挂中点的一阶模态，这是一个近似值

brass_E = (4 * pi^2 * brass_length^4 * brass_density * brass_mid_freq^2) / (beta_value^4 * brass_I / brass_A);
fprintf('根据悬挂法计算的黄铜棒杨氏模量估计值：%.2f GPa\n', brass_E / 1e9);

% 创建图表4：三种拟合结果对比
figure(4);
plot(x_fit, y_fit / 1e4, 'b-', 'LineWidth', 2);
hold on;
plot(x_fit, y_fit_steel / 1e4, 'r-', 'LineWidth', 2);
plot(x_fit, y_fit_support / 1e4, 'g-', 'LineWidth', 2);
grid on;
xlabel('相对位置 (x/L)');
ylabel('频率平方 (×10^4 Hz^2)');
title('三种测量方法的多项式拟合结果对比');
legend('黄铜棒悬挂法', '不锈钢棒悬挂法', '黄铜棒支撑法');

% 保存图形结果
saveas(figure(1), '黄铜棒悬挂法拟合.png');
saveas(figure(2), '不锈钢棒悬挂法拟合.png');
saveas(figure(3), '黄铜棒支撑法拟合.png');
saveas(figure(4), '三种测量方法对比.png');

% 新增：使用左悬挂点位置为横坐标，频率为纵坐标的图表
% 使用二次多项式拟合频率数据
figure(5);
p_brass_freq = polyfit(left_suspend_pos_valid, brass_freq_suspend, 2);
x_pos_fit = linspace(min(left_suspend_pos_valid), max(left_suspend_pos_valid), 100);
y_freq_fit = polyval(p_brass_freq, x_pos_fit);

plot(left_suspend_pos_valid, brass_freq_suspend, 'ro', 'MarkerSize', 8, 'LineWidth', 1.5);
hold on;
plot(x_pos_fit, y_freq_fit, 'b-', 'LineWidth', 1.5);
grid on;
xlabel('左悬挂点位置 (cm)');
ylabel('频率 (Hz)');
title('黄铜棒悬挂法共振频率与悬挂点位置关系');
legend('实验数据', '多项式拟合');

% 计算极小值
% 对于二次多项式 ax^2 + bx + c，极值点在 x = -b/(2a)
a = p_brass_freq(1);
b = p_brass_freq(2);
c = p_brass_freq(3);
x_min_pos = -b/(2*a);
y_min_freq = polyval(p_brass_freq, x_min_pos);

% 在图上标记极小值点
plot(x_min_pos, y_min_freq, 'g*', 'MarkerSize', 12, 'LineWidth', 2);
text(x_min_pos+1, y_min_freq+2, ['最小值: (' num2str(x_min_pos, '%.2f') ', ' num2str(y_min_freq, '%.2f') ')']);

% 输出极小值
fprintf('\n黄铜棒共振频率极小值:\n');
fprintf('位置: %.2f cm\n', x_min_pos);
fprintf('频率: %.2f Hz\n', y_min_freq);

% 对不锈钢棒也进行同样的分析
figure(6);
p_steel_freq = polyfit(left_suspend_pos_valid, steel_freq_suspend, 2);
y_steel_freq_fit = polyval(p_steel_freq, x_pos_fit);

plot(left_suspend_pos_valid, steel_freq_suspend, 'ro', 'MarkerSize', 8, 'LineWidth', 1.5);
hold on;
plot(x_pos_fit, y_steel_freq_fit, 'b-', 'LineWidth', 1.5);
grid on;
xlabel('左悬挂点位置 (cm)');
ylabel('频率 (Hz)');
title('不锈钢棒悬挂法共振频率与悬挂点位置关系');
legend('实验数据', '多项式拟合');

% 计算极小值
a_steel = p_steel_freq(1);
b_steel = p_steel_freq(2);
c_steel = p_steel_freq(3);
x_min_pos_steel = -b_steel/(2*a_steel);
y_min_freq_steel = polyval(p_steel_freq, x_min_pos_steel);

% 在图上标记极小值点
plot(x_min_pos_steel, y_min_freq_steel, 'g*', 'MarkerSize', 12, 'LineWidth', 2);
text(x_min_pos_steel+1, y_min_freq_steel+2, ['最小值: (' num2str(x_min_pos_steel, '%.2f') ', ' num2str(y_min_freq_steel, '%.2f') ')']);

% 输出极小值
fprintf('\n不锈钢棒共振频率极小值:\n');
fprintf('位置: %.2f cm\n', x_min_pos_steel);
fprintf('频率: %.2f Hz\n', y_min_freq_steel);

% 保存新增图形
saveas(figure(5), '黄铜棒频率与位置关系.png');
saveas(figure(6), '不锈钢棒频率与位置关系.png'); 