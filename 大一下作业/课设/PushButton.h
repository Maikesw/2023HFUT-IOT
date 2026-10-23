#pragma once

#include "BasicWidget.h"
#include "Configure.h"
class PushButton :
    public BasicWidget
{
public:
    PushButton(const std::string& text="Button",int x = 0, int y = 0, int w = 100, int h = 30);
    void show() override;

private:
    std::string m_text; 
private:

};

