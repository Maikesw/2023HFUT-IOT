#include"Management.h"
#include"Window.h"
#include"Table.h"
#include<iostream>

int main()
{
	Window w(960,640,EX_SHOWCONSOLE);
	w.setWindowTilte("AVLÊ÷ÑÝÊ¾ÏµÍ³");

	Management m;
	m.run();

	return w.exec();
}