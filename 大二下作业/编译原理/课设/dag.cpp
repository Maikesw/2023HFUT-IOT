#include "dag.h"
#include <iostream>
#include <iomanip>
#include <algorithm>

DAGNode::DAGNode(int id, NodeType type, const std::string& value)
    : id(id), type(type), value(value) {
}

void DAGNode::addChild(int childId) {
    children.push_back(childId);
}

bool DAGNode::isEquivalentTo(const DAGNode& other) const {
    if (type != other.type || value != other.value || children.size() != other.children.size()) {
        return false;
    }
    
    // Check children
    for (size_t i = 0; i < children.size(); ++i) {
        if (children[i] != other.children[i]) {
            return false;
        }
    }
    
    return true;
}

DAG::DAG() : rootId(-1), nextId(0) {
}

int DAG::addLeaf(const std::string& value) {
    // Check if leaf node already exists
    int existingId = findEquivalentNode(NodeType::OPERAND, value, {});
    if (existingId != -1) {
        return existingId;
    }
    
    // Create new node
    int id = nextId++;
    nodes.emplace_back(id, NodeType::OPERAND, value);
    return id;
}

int DAG::addOperator(const std::string& op, int leftChild, int rightChild) {
    std::vector<int> children;
    children.push_back(leftChild);
    
    // Some operators are commutative, like + and *
    if (op == "+" || op == "*") {
        // Ensure left operand ID is less than or equal to right operand ID
        // to detect common subexpressions
        if (leftChild > rightChild) {
            std::swap(leftChild, rightChild);
        }
        children.clear();
        children.push_back(leftChild);
        children.push_back(rightChild);
    } else {
        // For non-commutative operators, keep original order
        children.push_back(rightChild);
    }
    
    // Check if equivalent node already exists
    int existingId = findEquivalentNode(NodeType::OPERATOR, op, children);
    if (existingId != -1) {
        return existingId;
    }
    
    // Create new node
    int id = nextId++;
    nodes.emplace_back(id, NodeType::OPERATOR, op);
    
    // Add children
    for (int childId : children) {
        nodes.back().addChild(childId);
    }
    
    return id;
}

int DAG::findEquivalentNode(NodeType type, const std::string& value, const std::vector<int>& children) {
    for (const auto& node : nodes) {
        if (node.type == type && node.value == value) {
            // Check children
            if (children.size() == node.children.size()) {
                bool match = true;
                for (size_t i = 0; i < children.size(); ++i) {
                    if (children[i] != node.children[i]) {
                        match = false;
                        break;
                    }
                }
                if (match) {
                    return node.id;
                }
            }
        }
    }
    return -1;
}

int DAG::getRootId() const {
    return rootId;
}

void DAG::setRoot(int rootId) {
    this->rootId = rootId;
}

const DAGNode& DAG::getNode(int id) const {
    for (const auto& node : nodes) {
        if (node.id == id) {
            return node;
        }
    }
    throw std::runtime_error("Node ID does not exist");
}

void DAG::print() const {
    std::cout << "DAG Node List:" << std::endl;
    std::cout << std::setw(5) << "ID" << std::setw(10) << "Type" << std::setw(10) << "Value" << "  Children" << std::endl;
    std::cout << std::string(40, '-') << std::endl;
    
    for (const auto& node : nodes) {
        std::cout << std::setw(5) << node.id;
        std::cout << std::setw(10) << (node.type == NodeType::OPERAND ? "Operand" : "Operator");
        std::cout << std::setw(10) << node.value;
        
        std::cout << "  ";
        for (int childId : node.children) {
            std::cout << childId << " ";
        }
        std::cout << std::endl;
    }
    
    if (rootId != -1) {
        std::cout << "\nRoot node: " << rootId << std::endl;
    }
}

void DAG::saveToFile(std::ofstream& outFile) const {
    outFile << "DAG Node List:" << std::endl;
    outFile << std::setw(5) << "ID" << std::setw(10) << "Type" << std::setw(10) << "Value" << "  Children" << std::endl;
    outFile << std::string(40, '-') << std::endl;
    
    for (const auto& node : nodes) {
        outFile << std::setw(5) << node.id;
        outFile << std::setw(10) << (node.type == NodeType::OPERAND ? "Operand" : "Operator");
        outFile << std::setw(10) << node.value;
        
        outFile << "  ";
        for (int childId : node.children) {
            outFile << childId << " ";
        }
        outFile << std::endl;
    }
    
    if (rootId != -1) {
        outFile << "\nRoot node: " << rootId << std::endl;
    }
} 