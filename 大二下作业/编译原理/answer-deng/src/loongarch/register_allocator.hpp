#pragma once
#include<unordered_map>
#include <queue>
#include <set>
#include <utility>
#include <vector>
#include "ir/ir.hpp"
#include "loongarch/arch.hpp"

namespace LoongArch {

struct Reg;
struct Function;

// no coalescing
class ColoringAllocator {
  std::shared_ptr<ir::ir_userfunc> func;
  std::unordered_map<std::shared_ptr<ir::ir_reg> ,std::vector<std::shared_ptr<ir::ir_reg>> > conflictGraph;
  std::unordered_map<std::shared_ptr<ir::ir_reg>,Pass::LiveInterval> mappingToInterval;
  std::unordered_map<std::shared_ptr<ir::ir_reg>,int> mappingToReg;
  std::vector<std::shared_ptr<ir::ir_reg>> simplifyStack;
  void SimplifyGraph();
  void Spill();
  void BuildConflictGraph();
  std::unordered_map<std::shared_ptr<ir::ir_reg>,int> getAllocate();
 public:
  ColoringAllocator(std::shared_ptr<ir::ir_userfunc> _func);
  std::unordered_map<std::shared_ptr<ir::ir_reg>,int> run();
};

}  // namespace Archriscv