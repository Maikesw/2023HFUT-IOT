#pragma once

#include <functional>
#include <map>
#include <sstream>
#include <vector>
#include <memory>
#include <set>
#include <iostream>

#include "ir/ir.hpp"
#include "loongarch/arch.hpp"
#include "loongarch/register_allocator.hpp"

namespace LoongArch {


class Program;
class Function;
class Block;
class ProgramBuilder;
class IrMapping;
struct Inst;
struct Jump;
struct RegImmInst;


class Program {
    friend LoongArch::ProgramBuilder;
protected:
    std::stringstream asm_code;
    std::vector<std::shared_ptr<Function>> functions;
    int block_n;
public:
    Program();
    void get_asm(std::ostream &out);
};

class Function : public std::enable_shared_from_this<Function> {
    friend LoongArch::ProgramBuilder;
protected:
    int regn; 
    std::string name;
    std::shared_ptr<LoongArch::Block> entry,ret_block;
    std::vector<std::shared_ptr<LoongArch::Block>> blocks;
    std::stringstream asm_code;
    int stack_size;                                         //用来存储使用栈的字节数
    int cur_pos;                                            //用来存储当前的变量的栈帧
    int bitalign(int value, int align);                     //用来进行字节对齐
public:
    Function(std::string name);
    void get_asm(std::ostream& out);
};

class Block {
    friend LoongArch::ProgramBuilder;
    friend LoongArch::Jump;
protected:
    bool label_used;
    std::string name;
    std::list<std::shared_ptr<LoongArch::Inst>> instructions;
public:
    Block(std::string name);
    void insert_before_jump(std::shared_ptr<Inst> inst);  //用来在跳转指令的前面添加一条别的指令
    void get_asm(std::ostream &out);
};

class IrMapping //关键，用来完成中间语言到具体的架构的映射
{
    friend LoongArch::ProgramBuilder;
    int regn;
protected:
    //完成block之间的映射
    std::unordered_map<std::shared_ptr<ir::ir_basicblock>,std::shared_ptr<LoongArch::Block>> blockmapping;
    //block之间的逆映射
    std::unordered_map<std::shared_ptr<LoongArch::Block>,std::shared_ptr<ir::ir_basicblock>> rev_blockmapping;
    //中端reg到后端reg的映射，寄存器分配之后关系可以放在这里
    std::unordered_map<int,LoongArch::Reg> reg_mapping;
    //内存变量和栈中的偏移之间的映射
    std::unordered_map<std::shared_ptr<ir::ir_memobj>,int> objoffset_mapping;
    //中端地址寄存器和内存变量之间的映射
    std::unordered_map<std::shared_ptr<ir::ir_reg>,std::shared_ptr<ir::ir_memobj>> addrMemObj;
    //上面的逆映射
    std::unordered_map<std::shared_ptr<ir::ir_memobj>,std::shared_ptr<ir::ir_reg>> revAddMemObj;

public:
    //申请一个被预留的寄存器
    Reg new_reg();
    //对寄存器进行转换
    Reg&transfer_reg(ir::ir_reg irReg);
    IrMapping();
};



} // namespace ArchLA
