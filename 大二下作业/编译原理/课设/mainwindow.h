#ifndef MAINWINDOW_H
#define MAINWINDOW_H

#include <QMainWindow>
#include <QLineEdit>
#include <QPushButton>
#include <QTextEdit>
#include <QLabel>
#include <QVBoxLayout>
#include <QHBoxLayout>
#include <QGroupBox>
#include <QMessageBox>
#include <QFileDialog>
#include "parser.h"
#include "dagvisualizer.h"

class MainWindow : public QMainWindow
{
    Q_OBJECT

public:
    MainWindow(QWidget *parent = nullptr);
    ~MainWindow();

private slots:
    void onParseButtonClicked();
    void onSaveButtonClicked();
    void onClearButtonClicked();
    void onExampleButtonClicked();

private:
    void setupUI();
    void updateDAGView();

    QLineEdit *expressionInput;
    QPushButton *parseButton;
    QPushButton *saveButton;
    QPushButton *clearButton;
    QPushButton *exampleButton;
    QTextEdit *dagTextOutput;
    DAGVisualizer *dagVisualizer;
    QLabel *statusLabel;

    Parser parser;
    DAG currentDAG;
    bool hasValidDAG;
};

#endif // MAINWINDOW_H 