#include "program_builder.hpp"
#include "ir/ir.hpp"

void LoongArch::ProgramBuilder::visit(ir::ir_reg &node) {
    
}

void LoongArch::ProgramBuilder::visit(ir::ir_constant &node) {
  
}

void LoongArch::ProgramBuilder::visit(ir::ir_basicblock &node) 
{
    for(auto & i : node.instructions)
        i->accept(*this);
}

void LoongArch::ProgramBuilder::visit(ir::ir_module &node) { 

    this->prog = std::make_shared<LoongArch::Program>();
    this->cur_mapping = std::make_shared<IrMapping>();

    //可以调用寄存器分配函数进行寄存器分配
     node.reg_allocate();

    for(auto & [name,func] : node.usrfuncs){
        this->func_name = name;
        prog->functions.emplace_back(std::make_shared<LoongArch::Function>(name));
        func->accept(*this);
    }
    //后面可以用来处理全局变量，不过这里没有
}

void LoongArch::ProgramBuilder::visit(ir::ir_userfunc &node) {
    
    this->cur_func = prog->functions.back();
    
    this->cur_func->stack_size = 16; //默认为16字节，分别存储$ra和父函数的fp值
    this->cur_func->cur_pos = 20;
    for(auto [ir_reg,backend_reg] : node.regAllocateOut)
    {
        this->cur_mapping->reg_mapping[ir_reg->id] = Reg{backend_reg};
    }
    
    //处理内存变量

    //build a entry
    cur_func->entry = std::make_shared<LoongArch::Block>(".entry_" + func_name);
    cur_func->blocks.push_back(cur_func->entry);
    prog->block_n++;
    
    //构造函数的进入部分
   // this->cur_func->entry->instructions.push_back(std::make_shared<LoongArch::RegImmInst>(RegImmInst::addi_d,Reg{sp},Reg{sp},-cur_func->stack_size));  //sp:栈顶指针
  //  this->cur_func->entry->instructions.push_back(std::make_shared<LoongArch::st>(Reg{ra},Reg{sp},cur_func->stack_size-8,st::st_d));//保存ra//返回地址
   // this->cur_func->entry->instructions.push_back(std::make_shared<LoongArch::RegImmInst>(RegImmInst::addi_d,Reg{fp},Reg{sp},cur_func->stack_size)); //确定

    
    for(auto & block : node.bbs){
        auto cur_block_name = ".L" + std::to_string(prog->block_n);
        auto backend_block = std::make_shared<LoongArch::Block>(cur_block_name);
        //把entry的终点设定成L0
        if(cur_block_name == ".L1"){
            this->cur_func->entry->instructions.push_back(std::make_shared<LoongArch::Jump>(backend_block.get()));       
        }
        prog->block_n++;
        //确定映射关系
        this->cur_mapping->blockmapping[block] = backend_block;
        this->cur_mapping->rev_blockmapping[backend_block] = block;

        cur_func->blocks.push_back(backend_block);
    }

    //在遍历block的过程中，可能会牵扯到下一个block
    for(int i = 0; i < cur_func->blocks.size(); ++i){
        if(cur_func->blocks[i] != cur_func->entry){
            this->cur_block = cur_func->blocks[i];
            if(i < cur_func->blocks.size() - 1) this->next_block = cur_func->blocks[i+1];
            else this->next_block = nullptr;
            this->cur_mapping->rev_blockmapping[cur_func->blocks[i]]->accept(*this);
        }
    }

    //消解phi指令
    struct PendingMove {
        std::shared_ptr<LoongArch::Block> block;
        LoongArch::Reg to, from;
    };

    std::vector<PendingMove> Pending_moves;
    for(auto &bb : node.bbs){
        for(auto &inst : bb->instructions){
            if(auto *cur = dynamic_cast<ir::phi*>(inst.get())){
                for(auto &prev : cur->uses){
                    auto b = cur_mapping->blockmapping[prev.second];
                    std::shared_ptr<ir::ir_reg> use_reg = std::dynamic_pointer_cast<ir::ir_reg>(prev.first);
                    if(use_reg) {
                        Reg temp = cur_mapping->transfer_reg(*use_reg.get());
                        Reg mid = cur_mapping->new_reg();
                        b->insert_before_jump(
                            std::make_shared<RegImmInst>(RegImmInst::addi_w,mid,temp,0)
                        );
                        Pending_moves.push_back({b,this->cur_mapping->transfer_reg(*cur->dst.get()),mid});
                    }else{
                        std::shared_ptr<ir::ir_constant> use_constant = std::dynamic_pointer_cast<ir::ir_constant>(prev.first);
                        //直接把那个立即数放到相应的phi的目的寄存器里面就行了，在uses block的的jump的前一句
                        //这里直接使用LoongArch的加载立即数指令
                        auto value = use_constant->init_val.value();
                        auto mid = cur_mapping->new_reg();
                        if(std::holds_alternative<int>(value)){
                            int value_num = std::get<int>(value);
                            b->insert_before_jump(
                                std::make_shared<LoongArch::LoadImm>(mid,value_num)
                            );
                        }
                        Pending_moves.push_back({b,this->cur_mapping->transfer_reg(*cur->dst.get()),mid});
                    }

                }
            }
        }
    }

    this->cur_func->entry->instructions.insert(this->cur_func->entry->instructions.begin(),std::make_shared<LoongArch::RegImmInst>(RegImmInst::addi_d,Reg{fp},Reg{sp},cur_func->stack_size)); //确定
    this->cur_func->entry->instructions.insert(this->cur_func->entry->instructions.begin(),std::make_shared<LoongArch::st>(Reg{ra},Reg{sp},cur_func->stack_size-8,st::st_d));//保存ra//返回地址
    this->cur_func->entry->instructions.insert(this->cur_func->entry->instructions.begin(),std::make_shared<LoongArch::RegImmInst>(RegImmInst::addi_d,Reg{sp},Reg{sp},-cur_func->stack_size));  //sp:栈顶指针


    for(auto &i : Pending_moves){
        i.block->insert_before_jump(std::make_shared<RegImmInst>(RegImmInst::addi_w,i.to,i.from,0)); //插入一条move指令
        cur_func->regn = cur_mapping->regn;
    }
}

void LoongArch::ProgramBuilder::visit(ir::store &node) {

     auto store_value = std::dynamic_pointer_cast<ir::ir_value>(node.value);
    auto store_addr = std::dynamic_pointer_cast<ir::ir_reg>(node.addr);
  
    auto addr = cur_mapping->transfer_reg(*store_addr);
    if(store_value->type_id() == 1){
        auto value = std::dynamic_pointer_cast<ir::ir_constant>(store_value);
    cur_block->instructions.push_back(
       std::make_shared<LoongArch::RegImmInst>(RegImmInst::addi_w,Reg{12},Reg{0},std::get<int>(value->init_val.value()))
    );


    cur_block->instructions.push_back(
        std::make_shared<LoongArch::st>(Reg{12},Reg{22},addr.offset,st::st_w)
    );}
    else{

        auto reg = std::dynamic_pointer_cast<ir::ir_reg>(store_value);
        auto backend_reg = cur_mapping->transfer_reg(*reg.get());
    cur_block->instructions.push_back(
       std::make_shared<LoongArch::RegRegInst>(RegRegInst::add_w,Reg{12},Reg{0},backend_reg)
    );


    cur_block->instructions.push_back(
        std::make_shared<LoongArch::st>(Reg{12},Reg{22},addr.offset,st::st_w)
    );
    }

}

void LoongArch::ProgramBuilder::visit(ir::jump &node) {
        ptr< Block> jump_target = cur_mapping->blockmapping[node.target];
    if (jump_target != next_block)
        cur_block->instructions.push_back(std::make_unique<Jump>(jump_target.get()));
}

void LoongArch::ProgramBuilder::visit(ir::br &node) {
        // cout<<"br"<<std::endl;
    auto branch = &node;
    //CmpInfo cond;
        // cout<<__PRETTY_FUNCTION__<<std::endl;
    std::shared_ptr<Block> true_target = cur_mapping->blockmapping[branch->get_target_true()],
            false_target = cur_mapping->blockmapping[branch->get_target_false()];
            //  cout<<"b2"<<std::endl;
    auto condVec=branch->use_reg();
    auto cond=std::dynamic_pointer_cast<ir::ir_reg>(condVec[0]);
    // for(auto i:cur_mapping->revReg_mapping){
    //     cout<<i.first<<"   "<<i.second<<std::endl;
    // }
    auto reg=cur_mapping->reg_mapping.at(cond->id);
    // cout<<(int)cond->type;
    int op=Branch::beq;
    cur_block->instructions.push_back(std::make_shared<Branch>( true_target->name,reg,Reg{0},Branch::beq));
    true_target->label_used=true;
    cur_block->instructions.push_back(std::make_shared<LoongArch::Jump>((false_target.get())));
}

void LoongArch::ProgramBuilder::visit(ir::ret &node) {
    auto ret_reg = std::dynamic_pointer_cast<ir::ir_reg>(node.value);
    auto backend_reg = cur_mapping->transfer_reg(*ret_reg.get());
    cur_block->instructions.push_back(
        std::make_shared<LoongArch::RegRegInst>(RegRegInst::orw,Reg{4},backend_reg,Reg{0})
    );
    cur_block->instructions.push_back(
        std::make_shared<LoongArch::ld>(Reg{22},Reg{3},this->cur_func->stack_size - 8,ld::ld_d)
    );
    cur_block->instructions.push_back(
        std::make_shared<LoongArch::RegImmInst>(RegImmInst::addi_d,Reg{sp},Reg{sp},cur_func->stack_size)
    );
    cur_block->instructions.push_back(std::make_shared<LoongArch::jr>(true));
   
}

void LoongArch::ProgramBuilder::visit(ir::load &node) {
    auto dst_reg = std::dynamic_pointer_cast<ir::ir_reg>(node.dst);
    auto load_addr = std::dynamic_pointer_cast<ir::ir_reg>(node.addr);
    auto backend_reg = cur_mapping->transfer_reg(*dst_reg.get());
    auto backend_addr = cur_mapping->transfer_reg(*load_addr);
   
    cur_block->instructions.push_back(
        std::make_shared<LoongArch::ld>(backend_reg,Reg{22},backend_addr.offset,ld::ld_w)
    );
}

void LoongArch::ProgramBuilder::visit(ir::alloc &node) {
    auto var_addr = std::dynamic_pointer_cast<ir::ir_reg>(node.def_reg()[0]);

    cur_func->cur_pos += var_addr->size;
    cur_func->stack_size = cur_func->bitalign(cur_func->cur_pos,16);
    LoongArch::Reg &mapping_reg = cur_mapping->transfer_reg(*var_addr);
    if(true){
        mapping_reg.type = Rtype::INT;
    } 
    mapping_reg.offset = -cur_func->cur_pos;
//auto tmp=cur_mapping->new_reg();
    //auto ptr=std::make_shared<Reg>(&tmp);
    // auto ptra=std::make_shared<ir::alloc>(&node);
//auto var_addr = std::dynamic_pointer_cast<ir::ir_reg>(node.def_reg()[0]);
    
//auto var_reg=std::dynamic_pointer_cast<ir::ir_reg>(node.def_reg()[1]);
//int id=var_reg->id;

//建立映射关系
//cur_func->cur_pos += var_addr->size;
//cur_func->stack_size = cur_func->bitalign(cur_func->cur_pos,16);
//cur_mapping->reg_mapping.insert(std::make_pair(id,tmp));

//auto mem=std::make_shared<ir::ir_memobj>("",var_addr,64);
//cur_mapping->addrMemObj.insert(std::make_pair(var_addr, mem));
//cur_mapping->objoffset_mapping.insert(std::make_pair(mem, -(cur_func->cur_pos)));

    


   }







void LoongArch::ProgramBuilder::visit(ir::phi &node) {


}

void LoongArch::ProgramBuilder::visit(ir::unary_op_ins &node) {
    auto op = node.op;
    auto dst = cur_mapping->transfer_reg(*std::dynamic_pointer_cast<ir::ir_reg>(node.dst));
    auto src = std::dynamic_pointer_cast<ir::ir_constant>(node.src);
    cur_block->instructions.push_back(std::make_shared<LoadImm>(dst, std::get<int>(src->init_val.value())));

}

void LoongArch::ProgramBuilder::visit(ir::binary_op_ins &node) {
    //   auto info = inner_info;
  auto binary = &node;
  auto func = cur_func;
    // auto ir_sr1=std::dynamic_pointer_cast<ir::ir_reg>(binary->src1);
    // auto reg_sr1=cur_mapping->reg_mapping.find(ir_sr1->id);
    // if(reg_sr1 !=cur_mapping->reg_mapping.end()){
    //     cur_block->instructions.push_back(std::make_shared<LoongArch::RegImmInst>(RegImmInst::addi_d,dst,Reg{0},0));
      //   return;
  //   }
 
    if (binary->op == binop::plus ||
        binary->op == binop::minus ||
        binary->op == binop::multiply ||
        binary->op == binop::divide||
        binary->op == binop::modulo        
        ) {
        auto dst = cur_mapping->transfer_reg(*std::dynamic_pointer_cast<ir::ir_reg>(binary->dst)),
            s1 = cur_mapping->transfer_reg(*std::dynamic_pointer_cast<ir::ir_reg>(binary->src1));
        //s2 = cur_mapping->transfer_reg(*std::dynamic_pointer_cast<ir::ir_reg>(binary->src2));
        auto s2_value = std::dynamic_pointer_cast<ir::ir_value>(binary->src2);
        if(s2_value->type_id() == 0){
        auto s2_reg = cur_mapping->transfer_reg(*std::dynamic_pointer_cast<ir::ir_reg>(s2_value));
        auto _op=binary->op;
        switch (_op) {
            case binop::plus:
                cur_block->instructions.push_back(std::make_shared<RegRegInst>(RegRegInst::add_w, dst, s1, s2_reg));
                break;
            case binop::minus:
                cur_block->instructions.push_back(std::make_shared<RegRegInst>(RegRegInst::sub_w, dst, s1, s2_reg));
                break;
            case binop::multiply:
                cur_block->instructions.push_back(std::make_shared<RegRegInst>(RegRegInst::mul_w, dst, s1, s2_reg));
                break;
            case binop::divide:
                cur_block->instructions.push_back(std::make_shared<RegRegInst>(RegRegInst::div_w, dst, s1, s2_reg));
                break;
            case binop::modulo:
                //有点复杂  
                //cur_block->instructions.push_back(std::make_shared<RegRegInst>(RegRegInst::m, dst, s1, s2));
                break;
            default:
                assert(0);
            }}
        else{

             auto s2_constant = std::dynamic_pointer_cast<ir::ir_constant>(s2_value);

            auto _op=binary->op;
        switch (_op) {
            case binop::plus:
                cur_block->instructions.push_back(std::make_shared<RegImmInst>(RegImmInst::addi_w, dst, s1, std::get<int>(s2_constant->init_val.value())));
                break;
            case binop::minus:
                cur_block->instructions.push_back(std::make_shared<RegImmInst>(RegImmInst::addi_w, dst, s1, -(std::get<int>(s2_constant->init_val.value()) ) ) );
                break;
            case binop::multiply:
                cur_block->instructions.push_back(std::make_shared<LoadImm>(Reg{12}, std::get<int>(s2_constant->init_val.value())));
                cur_block->instructions.push_back(std::make_shared<RegRegInst>(RegRegInst::mul_w, dst, s1, Reg{12}));
                
                break;
          //  case binop::divide:
          //      cur_block->instructions.push_back(std::make_shared<RegImmInst>(RegImmInst::divi_w, dst, s1, std::get<int>(s2_constant->init_val.value())));
          //      break;
           // case binop::modulo:
                //有点复杂  
                //cur_block->instructions.push_back(std::make_shared<RegRegInst>(RegRegInst::m, dst, s1, s2));
           //     break;
            default:
                assert(0);
            }
        }





    }
}

void LoongArch::ProgramBuilder::visit(ir::cmp_ins &node) {
     auto cmp=&node;
    
    Compare c;
    auto dst = cur_mapping->transfer_reg(*std::dynamic_pointer_cast<ir::ir_reg>(cmp->dst)),
         s1 = cur_mapping->transfer_reg(*std::dynamic_pointer_cast<ir::ir_reg>(cmp->src1));
    auto s2 = std::dynamic_pointer_cast<ir::ir_constant>(cmp->src2);
    auto _op = cmp->op;
    switch (_op) {
    case relop::less:
        c = Lt;
      //  cur_block->instructions.push_back(std::make_shared<RegImmInst>(RegImmInst::addi_w,Reg{12},s1,-(std::get<int>(s2->init_val.value()))));
     //   cur_block->instructions.push_back(std::make_shared<RegImmInst>(RegImmInst::slti,dst,Reg{12},0));
        break;
    case relop::less_equal:
        c = Le;
        break;
    case relop::equal:
        c = Eq;
        cur_block->instructions.push_back(std::make_shared<RegImmInst>(RegImmInst::addi_w,dst,s1,-(std::get<int>(s2->init_val.value()))));      //$r12 = src1 - src2
       // cur_block->instructions.push_back(std::make_shared<RegImmInst>(RegImmInst::slti,dst,Reg{12},1));                                            //dst = $r12 < 1
        break;
    case relop::non_equal:
        c = Ne;
        break;
    case relop::greater:
        c=Gt;
        cur_block->instructions.push_back(std::make_shared<LoadImm>(Reg{12},std::get<int>(s2->init_val.value())));  
        
        cur_block->instructions.push_back(std::make_shared<RegRegInst>(RegRegInst::slt,dst,Reg{12},s1));                        //反着来，a>100 <=> 100<a，因为龙芯汇编没有>
        cur_block->instructions.push_back(std::make_shared<RegImmInst>(RegImmInst::slti, dst, dst, 0));
        break;
    case relop::greater_equal:
        c=Ge;
        break;
    default:
        assert(0);
    }
}

void LoongArch::ProgramBuilder::visit(ir::logic_ins &node) {}
