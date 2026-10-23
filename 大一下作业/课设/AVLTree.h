#ifndef AVLTREE_H
#define AVLTREE_H

#include "AVLNode.h"
#include <algorithm>
#include <string>

class AVLTree {
public:
    AVLNode* root;

    int height(AVLNode* node);
    int getHeight(AVLNode* node);
    int getBalance(AVLNode* node);
    AVLNode* rightRotate(AVLNode* y);
    AVLNode* leftRotate(AVLNode* x);
    AVLNode* insert(AVLNode* node, int data);
    AVLNode* search(AVLNode* node, int data);
    void inorder(AVLNode* root);


    void drawTree(AVLNode* root, int x, int y, int dx, int dy);
    void drawTree2(AVLNode* root, int x, int y, int dx, int dy, int data);
    AVLTree();
    void insert(int data);
    AVLNode* search(int data);
    void inorder();
    void visualize(); // 将 visualize 函数声明移到 public 区域
    void fileInput(const std::string& filename);
};

#endif
