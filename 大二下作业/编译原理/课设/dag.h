#ifndef DAG_H
#define DAG_H

#include <string>
#include <vector>
#include <unordered_map>
#include <memory>
#include <fstream>

// DAG node types
enum class NodeType {
    OPERAND,    // Operand (constant or variable)
    OPERATOR    // Operator
};

// DAG Node
class DAGNode {
public:
    int id;                     // Node ID
    NodeType type;              // Node type
    std::string value;          // Value (operand or operator)
    std::vector<int> children;  // Child node ID list

    DAGNode(int id, NodeType type, const std::string& value);
    
    // Add child node
    void addChild(int childId);
    
    // Check if equivalent to another node
    bool isEquivalentTo(const DAGNode& other) const;
};

// DAG Graph
class DAG {
public:
    DAG();
    
    // Add leaf node (operand)
    int addLeaf(const std::string& value);
    
    // Add operator node
    int addOperator(const std::string& op, int leftChild, int rightChild);
    
    // Find equivalent node
    int findEquivalentNode(NodeType type, const std::string& value, const std::vector<int>& children);
    
    // Get root node
    int getRootId() const;
    
    // Set root node
    void setRoot(int rootId);
    
    // Get node by ID
    const DAGNode& getNode(int id) const;
    
    // Print DAG
    void print() const;
    
    // Save DAG to file
    void saveToFile(std::ofstream& outFile) const;
    
private:
    std::vector<DAGNode> nodes;     // All nodes
    int rootId;                     // Root node ID
    int nextId;                     // Next available ID
};

#endif // DAG_H 