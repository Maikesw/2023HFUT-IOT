% 创建两种算法数据处理流程对比图的脚本（修正版）
clear;
close all;
clc;

% 设置中文字体
set(0,'DefaultAxesFontName','Microsoft YaHei');
set(0,'DefaultTextFontName','Microsoft YaHei');

% 创建一个大尺寸的图形窗口
figure('Position', [100, 100, 1200, 700]);

% 基础算法流程图 - 左侧
subplot(1, 2, 1);

% 创建基础算法流程框
x_left = 0.2;
y_positions = [0.85, 0.65, 0.45, 0.25];
box_width = 0.6;
box_height = 0.15;

% 画框和添加文字
for i = 1:length(y_positions)
    rectangle('Position', [x_left-0.15, y_positions(i)-0.07, box_width, box_height], ...
              'FaceColor', [0.9, 0.9, 1], 'EdgeColor', 'k');
    
    switch i
        case 1
            text_str = '原始数据加载';
        case 2
            text_str = '时间窗口构建 (lags=20)';
        case 3
            text_str = '数据归一化 (mapminmax)';
        case 4
            text_str = '数据集划分 (训练:测试=9:1)';
    end
    
    text(x_left+0.15, y_positions(i), text_str, 'HorizontalAlignment', 'center', 'FontSize', 12);
    
    % 添加箭头（除了最后一个框）
    if i < length(y_positions)
        arrow_y_start = y_positions(i) - 0.07;
        arrow_y_end = y_positions(i+1) + 0.08;
        annotation('arrow', [0.25, 0.25], ...
                  [arrow_y_start/1.2, arrow_y_end/1.2], 'Color', 'k');
    end
end

% 基础算法标题
title('基础LSTM算法数据处理流程', 'FontSize', 14, 'FontWeight', 'bold');
axis([0 1 0 1]);
axis off;

% 改进算法流程图 - 右侧
subplot(1, 2, 2);

% 创建改进算法主要流程框
y_positions_right = [0.85, 0.65, 0.45, 0.25, 0.05];
for i = 1:length(y_positions_right)
    rectangle('Position', [x_left-0.15, y_positions_right(i)-0.07, box_width, box_height], ...
              'FaceColor', [1, 0.9, 0.9], 'EdgeColor', 'k');
    
    switch i
        case 1
            text_str = '原始数据加载与初步分析';
        case 2
            text_str = '多尺度时间窗口 (lags=30)';
        case 3
            % 特征工程框，不在这里添加文字
            continue;
        case 4
            text_str = '特征组合与归一化';
        case 5
            text_str = '三部分数据集划分 (7:2:1)';
    end
    
    text(x_left+0.15, y_positions_right(i), text_str, 'HorizontalAlignment', 'center', 'FontSize', 12);
    
    % 添加箭头（除了最后一个框）
    if i < length(y_positions_right)
        arrow_y_start = y_positions_right(i) - 0.07;
        arrow_y_end = y_positions_right(i+1) + 0.08;
        annotation('arrow', [0.75, 0.75], ...
                  [arrow_y_start/1.2, arrow_y_end/1.2], 'Color', 'k');
    end
end

% 特征工程小模块
x_feat = [0.1, 0.35, 0.6];
y_feat = y_positions_right(3);
for i = 1:3
    rectangle('Position', [x_left-0.15+0.2*(i-1), y_feat-0.05, 0.18, 0.1], ...
              'FaceColor', [1, 0.95, 0.95], 'EdgeColor', 'k');
    
    switch i
        case 1
            feat_text = '一阶差分';
        case 2
            feat_text = '移动平均';
        case 3
            feat_text = '异常值检测';
    end
    
    text(x_left-0.06+0.2*(i-1), y_feat, feat_text, 'HorizontalAlignment', 'center', 'FontSize', 10);
    
    % 从特征工程小模块到下一个大框的箭头
    arrow_y_start = y_feat - 0.05;
    arrow_y_end = y_positions_right(4) + 0.08;
    annotation('arrow', [0.55+0.1*(i-1), 0.55+0.1*(i-1)], ...
              [arrow_y_start/1.2, arrow_y_end/1.2], 'Color', 'k');
end

% 改进算法标题
title('改进算法数据处理流程', 'FontSize', 14, 'FontWeight', 'bold');
axis([0 1 0 1]);
axis off;

% 添加总标题
sgtitle('两种算法数据处理流程对比', 'FontSize', 16, 'FontWeight', 'bold');

% 保存图像
print('两种算法数据处理流程对比图','-dpng','-r300');
disp('图像已保存为：两种算法数据处理流程对比图.png');