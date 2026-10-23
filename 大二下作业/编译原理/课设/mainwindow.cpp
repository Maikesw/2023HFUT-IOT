#include "mainwindow.h"
#include <QSplitter>
#include <QApplication>
#include <sstream>
#include <fstream>

MainWindow::MainWindow(QWidget *parent)
    : QMainWindow(parent), hasValidDAG(false)
{
    setupUI();
    setWindowTitle("算术表达式转DAG可视化工具");
    resize(1000, 700);
}

MainWindow::~MainWindow()
{
}

void MainWindow::setupUI()
{
    // 创建中央部件
    QWidget *centralWidget = new QWidget(this);
    setCentralWidget(centralWidget);
    
    // 主布局
    QVBoxLayout *mainLayout = new QVBoxLayout(centralWidget);
    
    // 输入区域
    QGroupBox *inputGroup = new QGroupBox("表达式输入", this);
    QHBoxLayout *inputLayout = new QHBoxLayout(inputGroup);
    
    expressionInput = new QLineEdit(this);
    expressionInput->setPlaceholderText("请输入算术表达式，例如：a+b*c");
    parseButton = new QPushButton("分析", this);
    clearButton = new QPushButton("清除", this);
    exampleButton = new QPushButton("示例", this);
    
    inputLayout->addWidget(expressionInput);
    inputLayout->addWidget(parseButton);
    inputLayout->addWidget(clearButton);
    inputLayout->addWidget(exampleButton);
    
    mainLayout->addWidget(inputGroup);
    
    // 创建分割器，用于分隔文本输出和可视化区域
    QSplitter *splitter = new QSplitter(Qt::Horizontal, this);
    
    // 文本输出区域
    QGroupBox *textOutputGroup = new QGroupBox("DAG文本表示", this);
    QVBoxLayout *textOutputLayout = new QVBoxLayout(textOutputGroup);
    
    dagTextOutput = new QTextEdit(this);
    dagTextOutput->setReadOnly(true);
    saveButton = new QPushButton("保存到文件", this);
    saveButton->setEnabled(false);
    
    textOutputLayout->addWidget(dagTextOutput);
    textOutputLayout->addWidget(saveButton);
    
    splitter->addWidget(textOutputGroup);
    
    // 可视化区域
    QGroupBox *visualGroup = new QGroupBox("DAG可视化", this);
    QVBoxLayout *visualLayout = new QVBoxLayout(visualGroup);
    
    dagVisualizer = new DAGVisualizer(this);
    visualLayout->addWidget(dagVisualizer);
    
    splitter->addWidget(visualGroup);
    
    // 设置分割器的初始大小
    splitter->setSizes(QList<int>() << 300 << 700);
    
    mainLayout->addWidget(splitter);
    
    // 状态标签
    statusLabel = new QLabel("就绪", this);
    mainLayout->addWidget(statusLabel);
    
    // 连接信号和槽
    connect(parseButton, &QPushButton::clicked, this, &MainWindow::onParseButtonClicked);
    connect(saveButton, &QPushButton::clicked, this, &MainWindow::onSaveButtonClicked);
    connect(clearButton, &QPushButton::clicked, this, &MainWindow::onClearButtonClicked);
    connect(exampleButton, &QPushButton::clicked, this, &MainWindow::onExampleButtonClicked);
}

void MainWindow::onParseButtonClicked()
{
    QString expression = expressionInput->text().trimmed();
    if (expression.isEmpty()) {
        QMessageBox::warning(this, "警告", "请输入算术表达式");
        return;
    }
    
    try {
        currentDAG = parser.parse(expression.toStdString());
        hasValidDAG = true;
        updateDAGView();
        saveButton->setEnabled(true);
        statusLabel->setText("分析成功");
    } catch (const std::exception& e) {
        QMessageBox::critical(this, "错误", QString("分析失败: %1").arg(e.what()));
        statusLabel->setText("分析失败");
    }
}

void MainWindow::onSaveButtonClicked()
{
    if (!hasValidDAG) {
        QMessageBox::warning(this, "警告", "没有有效的DAG可保存");
        return;
    }
    
    QString fileName = QFileDialog::getSaveFileName(this, "保存DAG", "", "文本文件 (*.txt)");
    if (fileName.isEmpty()) {
        return;
    }
    
    std::ofstream outFile(fileName.toStdString());
    if (outFile.is_open()) {
        currentDAG.saveToFile(outFile);
        outFile.close();
        statusLabel->setText("DAG已保存到文件: " + fileName);
    } else {
        QMessageBox::critical(this, "错误", "无法打开文件进行写入");
    }
}

void MainWindow::onClearButtonClicked()
{
    expressionInput->clear();
    dagTextOutput->clear();
    dagVisualizer->clearDAG();
    hasValidDAG = false;
    saveButton->setEnabled(false);
    statusLabel->setText("就绪");
}

void MainWindow::onExampleButtonClicked()
{
    expressionInput->setText("a+b*c+(a+b)*d");
}

void MainWindow::updateDAGView()
{
    if (!hasValidDAG) {
        return;
    }
    
    // 更新文本输出
    std::stringstream ss;
    currentDAG.print(ss);
    dagTextOutput->setText(QString::fromStdString(ss.str()));
    
    // 更新可视化视图
    dagVisualizer->setDAG(currentDAG);
} 