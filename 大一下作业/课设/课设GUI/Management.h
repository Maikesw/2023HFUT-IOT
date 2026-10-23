#pragma once

#include"Window.h"
#include<vector>
#include"PushButton.h"
#include"AVLNode.h"
#include"AVLTree.h"
#include"Table.h"
#include"LineEdit.h"
#include<memory>
class Management
{
	enum Operator
	{
		Display,
		Insert,
		Remove,
		Modify,
		Search,
		Menu = 66
	};
public:
	Management();
	//启动管理类
	void run();

	int menu();

	//1,显示所有的学生
	void display();
	//2,添加
	void add(AVLTree& tree);
	//3,删除
	void erase(AVLTree& tree);
	//4,修改
	//void modify();
	//5,查找
	void search(AVLTree& tree);

	void drawBackground();
	void enevtLoop();

private:
	int m_op;
	IMAGE m_bk;

	std::vector< std::unique_ptr<PushButton>> menu_btns;



private:
	std::unique_ptr<PushButton> m_addBtn;  //添加按钮
	std::unique_ptr<LineEdit> m_addEdit;

	std::unique_ptr<PushButton> m_delBtn;  //删除按钮
	std::unique_ptr<LineEdit> m_delEdit;

	std::unique_ptr<LineEdit> m_modifyEdit;	//修改
	std::vector<std::unique_ptr<LineEdit>> m_stuEdits;
	bool isFind = false;	
};

