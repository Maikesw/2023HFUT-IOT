#include "parser/SyntaxTree.hpp"
#include "ir/irbuilder.hpp"
#include "ir/ir_printer.hpp"
#include "loongarch/program_builder.hpp"
#include <fstream>
#include <stdlib.h>
ast::SyntaxTree syntax_tree;
int main(){

    ast::parse_file(std::cin);              //对输入进行前端处理
    
    //syntax_tree.print();                    //打印语法树
 
    std::shared_ptr<ir::IrBuilder> irbuilder = std::make_shared<ir::IrBuilder>();
    //std::shared_ptr<ir::IrPrinter> irprinter = std::make_shared<ir::IrPrinter>();
    
    syntax_tree.accept(*irbuilder);         //遍历语法树，生成中间代码IR
    
    //irprinter->visit(*irbuilder->compunit); //打印IR
   
    //下面是后端的部分
    std::shared_ptr<LoongArch::ProgramBuilder> progbuilder= std::make_shared<LoongArch::ProgramBuilder>();
    irbuilder->compunit->accept(*progbuilder);
    auto prog = progbuilder->prog;

    prog->get_asm(std::cout);
}