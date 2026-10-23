#include "AVLTree.h"
#include <graphics.h>
#include <conio.h>
#include <iostream>
#include <fstream>
#include <sstream>

using namespace std;

template <typename T>
std::string to_string(T value) {
    std::ostringstream os;
    os << value;
    return os.str();
}

AVLTree::AVLTree() : root(nullptr) {}

int AVLTree::height(AVLNode* node) {
    return node ? node->height : 0;
}

int AVLTree::getHeight(AVLNode* node) {
    return max(height(node->left), height(node->right)) + 1;
}

int AVLTree::getBalance(AVLNode* node) {
    return node ? height(node->left) - height(node->right) : 0;
}

AVLNode* AVLTree::rightRotate(AVLNode* y) {
    AVLNode* x = y->left;
    AVLNode* T2 = x->right;
    x->right = y;
    y->left = T2;
    y->height = getHeight(y);
    x->height = getHeight(x);
    return x;
}

AVLNode* AVLTree::leftRotate(AVLNode* x) {
    AVLNode* y = x->right;
    AVLNode* T2 = y->left;
    y->left = x;
    x->right = T2;
    x->height = getHeight(x);
    y->height = getHeight(y);
    return y;
}

AVLNode* AVLTree::insert(AVLNode* node, int data) {
    if (!node) return new AVLNode(data);
    if (data < node->data)
        node->left = insert(node->left, data);
    else if (data > node->data)
        node->right = insert(node->right, data);
    else
        return node;

    node->height = getHeight(node);
    int balance = getBalance(node);

    if (balance > 1 && data < node->left->data)
        return rightRotate(node);
    if (balance < -1 && data > node->right->data)
        return leftRotate(node);
    if (balance > 1 && data > node->left->data) {
        node->left = leftRotate(node->left);
        return rightRotate(node);
    }
    if (balance < -1 && data < node->right->data) {
        node->right = rightRotate(node->right);
        return leftRotate(node);
    }

    return node;
}

AVLNode* AVLTree::search(AVLNode* node, int data) {
    if (!node || node->data == data) return node;
    if (data < node->data)
        return search(node->left, data);
    return search(node->right, data);
}

void AVLTree::inorder(AVLNode* root) {
    if (root) {
        inorder(root->left);
        cout << root->data << " ";
        inorder(root->right);
    }
}

void AVLTree::drawTree2(AVLNode* root, int x, int y, int dx, int dy,int data) {
    if (root) {
        circle(x, y, 20);
        outtextxy(x - 5, y - 5, to_string(root->data).c_str());
        if (data < root->data) {
            line(x, y, x - dx, y + dy);
            drawTree2(root->left, x - dx, y + dy, dx / 2, dy, data);
        }
        if (data > root->data) {
            line(x, y, x + dx, y + dy);
            drawTree2(root->right, x + dx, y + dy, dx / 2, dy, data);
        }
    }
}

void AVLTree::drawTree(AVLNode* root, int x, int y, int dx, int dy) {
    if (root) {
        circle(x, y, 20);
        outtextxy(x - 5, y - 5, to_string(root->data).c_str());
        if (root->left) {
            line(x, y, x - dx, y + dy);
            drawTree(root->left, x - dx, y + dy, dx / 2, dy);
        }
        if (root->right) {
            line(x, y, x + dx, y + dy);
            drawTree(root->right, x + dx, y + dy, dx / 2, dy);
        }
    }
}

void AVLTree::visualize() {
    initgraph(800, 600);
    setbkcolor(WHITE);
    cleardevice();
    setcolor(BLACK);
    drawTree(root, 400, 50, 200, 100);
    /*   _getch();*/
    closegraph();
}

void AVLTree::insert(int data) {
    root = insert(root, data);
}

AVLNode* AVLTree::search(int data) {
    return search(root, data);
}

void AVLTree::inorder() {
    inorder(root);
    cout << endl;
}

void AVLTree::fileInput(const std::string& filename) {
    ifstream file(filename);
    if (file.is_open()) {
        int value;
        while (file >> value) {
            root = insert(root, value);
        }
        file.close();
    }
    else {
        cout << "无法打开文件" << endl;
    }
}