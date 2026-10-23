#include "ir/irbuilder.hpp"

void ir::IrBuilder::visit(ast::compunit_syntax &node)
{ 
    //编译入口
    /*
        init global variable
    */
    this->cur_block = nullptr;
    this->cur_func = nullptr;
    this->cur_func_name = "";
    this->found_main = false;
    this->return_bb = nullptr;
    this->compunit = std::make_shared<ir::ir_module>();
    /*
        register lib functions
    */

    /*
        visit children
    */
    for(auto & i : node.global_defs)
        i->accept(*this);
}

void ir::IrBuilder::visit(ast::func_def_syntax &node){
    
    auto rettype = node.rettype;
    string name = node.name;
    /*
        first we should register this function in symbol table
    */
    cur_func = compunit->new_func(name);   //one module
    this->functions.push_func(name,cur_func);
    /*
        next we should deal with entry module
        in this module, we should insert instructions which is responsible for args.  
    */
    cur_func->entry = cur_func->new_block();
    cur_block = cur_func->entry;
    if(name == "main"){
        this->found_main = true;
    }
    return_bb = cur_func->new_block();          //到目前为止，一个函数里面至少2个BB
    /*
        deal with block
    */
   cur_func->rettype = rettype;
    node.body->accept(*this);
    /*
        deal with ret
    */
   
    switch (rettype)
    {
    case vartype::VOID:
        {
            cur_block->push_back(std::make_shared<ir::jump>(return_bb));//放置跳转到return_bb的ir语句
            auto ret_value = std::make_shared<ir::ir_constant>(0);//给默认值0
            return_bb->push_back(std::make_shared<ir::ret>(ret_value,false));//放置返回语句
            //false express non return
        }
        break;
    
    default:
        {
            auto ret_val = std::make_shared<ir::ir_constant>(0);//给默认值
            return_value.emplace_back(ret_val,cur_block);//构造返回单元
            cur_block->push_back(std::make_shared<ir::jump>(return_bb));//放置跳转到return_bb的ir语句
            /*
                set a default value to last block
            */
            /*
                add a phi inst
            */
            auto ret_dst = cur_func->new_reg(vartype::INT);
            auto phi_inst = std::make_shared<ir::phi>(ret_dst);
            phi_inst->uses = return_value;
            return_bb->push_back(phi_inst);    //选择“空块”的0还是真实的返回值，给ret_dst
            return_bb->push_back(std::make_shared<ir::ret>(ret_dst,true));
        }
        break;
    }
}

void ir::IrBuilder::visit(ast::rel_cond_syntax &node)
{
    auto dst = cur_func->new_reg(vartype::INT);
    node.lhs->accept(*this);
    auto src1 = pass_value;
    node.rhs->accept(*this);
    auto src2 = pass_value;

    cur_block->push_back(std::make_shared<ir::cmp_ins>(dst, src1, src2, node.op));
    pass_value = dst;
}

void ir::IrBuilder::visit(ast::logic_cond_syntax &node)
{
    auto dst = cur_func->new_reg(vartype::INT);
    node.lhs->accept(*this);
    auto src1 = pass_value;
    node.rhs->accept(*this);
    auto src2 = pass_value;

    cur_block->push_back(std::make_shared<ir::logic_ins>(dst,src1,src2,node.op));
    pass_value = dst;
    
}

void ir::IrBuilder::visit(ast::binop_expr_syntax &node)
{
    auto dst = cur_func->new_reg(vartype::INT);
    node.lhs->accept(*this);
    auto src1 = pass_value;
    node.rhs->accept(*this);
    auto src2 = pass_value;
    auto binary_op_ins = std::make_shared<ir::binary_op_ins>(dst,src1,src2,node.op);
    cur_block->push_back(binary_op_ins);
    pass_value = dst;
}

void ir::IrBuilder::visit(ast::unaryop_expr_syntax &node)
{
    auto dst = cur_func->new_reg(vartype::INT);
    node.rhs->accept(*this);
    auto src = pass_value;
    cur_block->push_back(std::make_shared<ir::unary_op_ins>(dst,src,node.op));
    pass_value = dst;

}

void ir::IrBuilder::visit(ast::lval_syntax &node)
{//这里进来使用标识符就一种情况：给标识符装载数据来使用标识符; 故该函数内放置load[i32*]
//根据逻辑，该函数操作应该是：1）创建新内存reg      2）查找到的变量赋值给新reg
    auto var = this->scope.find_var(node.name);
//不是边执行这条语句边打印，而是先执行完全部语句再全部打印，所以四个不能用一个量去表示，得四个量分别表示alloc,store,ret,load
//所以在这里新建一个new_var来表示指针型的量
    auto nnew_reg = std::make_shared<ir::ir_reg>(var->addr->id,vartype::INTADDR,var->addr->size);
    auto dst = cur_func->new_reg(vartype::INT);
    cur_block->push_back(std::make_shared<ir::load>(dst,nnew_reg));
    pass_value = std::shared_ptr<ir::ir_value>(dst);
}

void ir::IrBuilder::visit(ast::literal_syntax &node)
{
    vartype restype = node.restype;
    pass_value = std::make_shared<ir::ir_constant>(node.intConst);
}

void ir::IrBuilder::visit(ast::var_def_stmt_syntax &node)
{
    vartype restype = node.restype;
    std::string name = node.name;
    auto var = this->cur_func->new_obj(node.name);
   // var->addr->type = vartype::INTADDR;    //这里必须设置类型，否则就会出现i32变成i32*，程序的健壮性
    this->scope.push_var(var->name,var);
    cur_block->push_back(std::make_shared<ir::alloc>(var));    
    if(node.initializer){
        node.initializer->accept(*this);
        auto nnnew_reg = std::make_shared<ir::ir_reg>(var->addr->id,vartype::INTADDR,var->addr->size);
        cur_block->push_back(std::make_shared<ir::store>(nnnew_reg,pass_value));
    }
}

void ir::IrBuilder::visit(ast::assign_stmt_syntax &node)
{//给这个标识符赋值[i32*]store，创建新var来改变类型由i32到i32*
    auto var = this->scope.find_var(node.target->name);
    auto new_reg = std::make_shared<ir::ir_reg>(var->addr->id,vartype::INTADDR,var->addr->size);
    auto cache_target = std::shared_ptr<ir::ir_value>(new_reg);
    node.value->accept(*this);//pass_value = value
  
   cur_block->push_back(std::make_shared<ir::store>(cache_target,pass_value));
   
}

void ir::IrBuilder::visit(ast::block_syntax &node)
{
   this->scope.enter();
    for(auto & i : node.body)
        i->accept(*this);
    this->scope.exit();
}

void ir::IrBuilder::visit(ast::if_stmt_syntax &node)
{
    auto true_block = cur_func->new_block();
    auto false_block = cur_func->new_block();
    auto out_block = cur_func->new_block();

    node.pred->accept(*this);
    auto cond = pass_value;

    cur_block->push_back(std::make_shared<ir::br>(cond, true_block, false_block));

    cur_block = true_block;
    node.then_body->accept(*this);
    cur_block->push_back(std::make_shared<ir::jump>(out_block));

    if(node.else_body){
    cur_block = false_block;
    node.else_body->accept(*this);
    cur_block->push_back(std::make_shared<ir::jump>(out_block));
    }

    


    cur_block = out_block;

}

void ir::IrBuilder::visit(ast::return_stmt_syntax &node)
{
   
    if(cur_func->rettype!=vartype::VOID)//有返回值
    {
        node.exp->accept(*this);
        //放置好返回值和来源BB后跳转到return_bb
        return_value.emplace_back(pass_value,cur_block);
        cur_block->push_back(std::make_shared<ir::jump>(return_bb));
       
    }
    else//没有返回值
    {
        //直接跳转到return的BB
       
        auto jmp_inst=std::make_shared<ir::jump>(return_bb);
        cur_block->push_back(jmp_inst);

    }
    //对BB进行划分，这个BB用于防止return后面的部分（这部分永远不会被执行到）
    this->cur_block=this->cur_func->new_block();    //BB+1

}

void ir::IrBuilder::visit(ast::var_decl_stmt_syntax &node)
{
    
    for(auto & i : node.var_def_list)
        i->accept(*this);
}
