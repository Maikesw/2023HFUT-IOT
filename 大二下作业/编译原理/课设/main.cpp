#include <iostream>
#include <string>
#include <fstream>
#include "parser.h"

int main() {
    std::cout << "算术表达式转DAG程序" << std::endl;
    std::cout << "请输入算术表达式: ";

    std::string expression;
    std::getline(std::cin, expression);

    Parser parser;
    DAG dag = parser.parse(expression);

    // 输出DAG到屏幕
    std::cout << "\nDAG表示:" << std::endl;
    dag.print();

    // 保存DAG到文件
    std::string filename = "dag_output.txt";
    std::ofstream outFile(filename);
    if (outFile.is_open()) {
        dag.saveToFile(outFile);
        outFile.close();
        std::cout << "\nDAG已保存到文件: " << filename << std::endl;
    }
    else {
        std::cerr << "无法创建输出文件" << std::endl;
    }

    system("pause");
    return 0;
}