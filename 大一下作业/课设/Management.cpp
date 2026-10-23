#include "Management.h"
#include<conio.h>
#include<iostream>
#include<fstream>
#include<sstream>
#include<algorithm>
using namespace std;
Management::Management()
{
	::loadimage(&m_bk, "./images/bk.jpg", Window::width(), Window::height());
	::settextstyle(20, 0, "宋体");
	//主界面按钮初始化
	menu_btns.emplace_back(new PushButton("绘画AVL树"));
	menu_btns.emplace_back(new PushButton("添加节点 "));
	menu_btns.emplace_back(new PushButton("查找节点"));
	menu_btns.emplace_back(new PushButton("退出系统"));
	for (int i = 0; i < menu_btns.size(); i++)
	{
		menu_btns[i]->setFixedSize(250, 50);
		
		int bx = (Window::width() - menu_btns[i]->width())/2;

		int vspace = (Window::height() - menu_btns.size()* menu_btns[i]->height()) / 2;
		int by = vspace + i * menu_btns[i]->height();

		menu_btns[i]->move(bx,by);
	}

	m_addBtn.reset(new PushButton("添加",670,260,80,40));
	m_addEdit.reset(new LineEdit(150,260,500,40));
	m_addEdit->setTitle("请输入数据");


}
void Management::run()
{
	//获取菜单的返回值
	AVLTree tree;
	m_op = Menu;
	Window::beginDraw();
	while (true)
	{
		Window::clear();
		drawBackground();
		
		if (Window::hasMsg())
		{
			switch (Window::getMsg().message)
			{
			case WM_KEYDOWN:	//按键按下
				//按ESC退出操作，返回主界面
				if (Window::getMsg().vkcode == VK_ESCAPE)
				{
					m_op = Menu;
				}
				break;
			default:		//鼠标操作
				enevtLoop();
				break;
			}
		}

		
		switch (m_op)
		{
		case Menu:
			m_op = menu();
			break;
		case 0:
			tree.drawTree(tree.root, 400, 50, 200, 100);
			break;
		case 1:
			add(tree);
			break;
		case 2:
			search(tree);
			break;
		default:
			exit(666);
			break;
		}

		Window::flushDraw();
		Window::getMsg().message = 0;
	}
	Window::endDraw();
}

int Management::menu()
{
	for (int i = 0; i < menu_btns.size(); i++)
	{
		menu_btns[i]->show();
		menu_btns[i]->event();
		if (menu_btns[i]->isClicked())
		{
			return i;
		}
	}
	return Menu;
}


void Management::add(AVLTree &tree)
{
	const char* title = "请输入数据";
	settextstyle(36, 0, "楷体");
	outtextxy((Window::width() - textwidth(title))/2, 150, title);

	m_addBtn->show();
	m_addEdit->show();
	auto str = m_addEdit->text();
	if (m_addBtn->isClicked() && !str.empty())
	{
		
		tree.insert(stoi(str));
		std::replace(str.begin(), str.end(), ' ', '\t');
		m_addEdit->clear();
	}
}


void Management::search(AVLTree& tree)
{
	const char* title = "请输入数据";
	settextstyle(36, 0, "楷体");
	outtextxy((Window::width() - textwidth(title)) / 2, 150, title);

	m_addBtn->show();
	m_addEdit->show();
	string str = m_addEdit->text();
	if (m_addBtn->isClicked() && !str.empty())
	{
		Window::beginDraw();
		while (1) 
		{
			Window::clear();
			drawBackground();

			if (Window::hasMsg())
			{
				switch (Window::getMsg().message)
				{
				case WM_KEYDOWN:	//按键按下
					//按ESC退出操作，返回主界面
					if (Window::getMsg().vkcode == VK_ESCAPE)
					{
						std::replace(str.begin(), str.end(), ' ', '\t');
						m_addEdit->clear();
						return;
					}
					break;
				default:		//鼠标操作
					enevtLoop();
					break;
				}
			}
			tree.drawTree2(tree.root, 400, 50, 200, 100, stoi(str));
			Window::flushDraw();
		}
		Window::endDraw();
	}
}

void Management::drawBackground()
{
	::putimage(0, 0, &m_bk);
}

void Management::enevtLoop()
{


	m_addBtn->event();
	m_addEdit->event();


	for (auto& e : m_stuEdits)
	{
		e->event();
	}

}
