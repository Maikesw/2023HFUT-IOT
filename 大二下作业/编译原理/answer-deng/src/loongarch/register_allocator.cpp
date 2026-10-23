#include "register_allocator.hpp"
#include <limits>
#include <algorithm>
#include <stack>
#include <queue>

namespace LoongArch {

ColoringAllocator::ColoringAllocator(std::shared_ptr<ir::ir_userfunc> _func) 
    : func(_func) {
    BuildConflictGraph();
}

void LoongArch::ColoringAllocator::BuildConflictGraph() {
    // 第一步：为每个虚拟寄存器计算活跃区间
    for (auto &bb : func->bbs) {
        int pos = 0; // 指令位置计数器
        
        for (auto &inst : bb->instructions) {
            // 处理def
            for (auto &def : inst->def_reg()) {
                auto reg = std::dynamic_pointer_cast<ir::ir_reg>(def);
                if (reg) {
                    if (mappingToInterval.find(reg) == mappingToInterval.end()) {
                        mappingToInterval[reg].l = pos;
                    }
                    mappingToInterval[reg].r = pos;
                }
            }
            
            // 处理use
            for (auto &use : inst->use_reg()) {
                auto reg = std::dynamic_pointer_cast<ir::ir_reg>(use);
                if (reg) {
                    if (mappingToInterval.find(reg) == mappingToInterval.end()) {
                        mappingToInterval[reg].l = pos;
                    }
                    mappingToInterval[reg].r = pos;
                }
            }
            pos++;
        }
    }
    
    // 第二步：构建冲突图
    for (auto it1 = mappingToInterval.begin(); it1 != mappingToInterval.end(); ++it1) {
        for (auto it2 = std::next(it1); it2 != mappingToInterval.end(); ++it2) {
            if (it1->second.r >= it2->second.l && it2->second.r >= it1->second.l) {
                conflictGraph[it1->first].push_back(it2->first);
                conflictGraph[it2->first].push_back(it1->first);
            }
        }
    }
    
    // 去重冲突边
    for (auto &entry : conflictGraph) {
        auto &neighbors = entry.second;
        std::sort(neighbors.begin(), neighbors.end());
        neighbors.erase(std::unique(neighbors.begin(), neighbors.end()), neighbors.end());
    }
}

void LoongArch::ColoringAllocator::SimplifyGraph() {
    std::queue<std::shared_ptr<ir::ir_reg>> lowDegreeNodes;
    const int kAvailableRegs = ALLOCABLE_REGISTER_COUNT;
    
    // 初始化低度数节点队列
    for (auto &entry : conflictGraph) {
        if (entry.second.size() < kAvailableRegs) {
            lowDegreeNodes.push(entry.first);
        }
    }
    
    // 简化过程
    while (!lowDegreeNodes.empty()) {
        auto reg = lowDegreeNodes.front();
        lowDegreeNodes.pop();
        
        if (conflictGraph.find(reg) == conflictGraph.end()) continue;
        
        simplifyStack.push_back(reg);
        auto neighbors = conflictGraph[reg];
        conflictGraph.erase(reg);
        
        // 更新邻居度数
        for (auto &neighbor : neighbors) {
            auto &edges = conflictGraph[neighbor];
            edges.erase(std::remove(edges.begin(), edges.end(), reg), edges.end());
            
            if (edges.size() == kAvailableRegs - 1) {
                lowDegreeNodes.push(neighbor);
            }
        }
    }
}

void LoongArch::ColoringAllocator::Spill() {
    if (conflictGraph.empty()) return;
    
    // 启发式：选择溢出代价最小的节点
    std::shared_ptr<ir::ir_reg> spillCandidate = nullptr;
    double minCost = std::numeric_limits<double>::max();
    
    for (auto &entry : conflictGraph) {
        auto reg = entry.first;
        int degree = entry.second.size();
        int liveLength = mappingToInterval[reg].r - mappingToInterval[reg].l + 1;
        
        // 简单代价模型：度数/活跃长度
        double cost = (liveLength == 0) ? degree : static_cast<double>(degree) / liveLength;
        
        if (cost < minCost) {
            minCost = cost;
            spillCandidate = reg;
        }
    }
    
    if (spillCandidate) {
        mappingToReg[spillCandidate] = -1; // -1表示需要溢出
        conflictGraph.erase(spillCandidate);
    }
}

std::unordered_map<std::shared_ptr<ir::ir_reg>, int> 
LoongArch::ColoringAllocator::getAllocate() {
    // 简化阶段
    SimplifyGraph();
    
    // 如果还有冲突，进行溢出
    while (!conflictGraph.empty()) {
        Spill();
        SimplifyGraph();
    }
    
    // 着色阶段
    std::unordered_map<int, bool> usedColors; // 记录已用颜色
    
    while (!simplifyStack.empty()) {
        auto reg = simplifyStack.back();
        simplifyStack.pop_back();
        
        // 重置颜色使用标记
        usedColors.clear();
        
        // 标记冲突寄存器使用的颜色
        if (conflictGraph.find(reg) != conflictGraph.end()) {
            for (auto &neighbor : conflictGraph[reg]) {
                if (mappingToReg.find(neighbor) != mappingToReg.end() && 
                    mappingToReg[neighbor] != -1) {
                    usedColors[mappingToReg[neighbor]] = true;
                }
            }
        }
        
        // 分配第一个可用颜色
        for (int i = 0; i < ALLOCABLE_REGISTER_COUNT; ++i) {
            if (!usedColors[ALLOCABLE_REGISTERS[i]]) {
                mappingToReg[reg] = ALLOCABLE_REGISTERS[i];
                break;
            }
        }
        
        // 如果没有可用颜色，标记为溢出
        if (mappingToReg.find(reg) == mappingToReg.end()) {
            mappingToReg[reg] = -1;
        }
    }
    
    return mappingToReg;
}

std::unordered_map<std::shared_ptr<ir::ir_reg>, int> 
LoongArch::ColoringAllocator::run() {
    return this->getAllocate();
}

} // namespace LoongArch