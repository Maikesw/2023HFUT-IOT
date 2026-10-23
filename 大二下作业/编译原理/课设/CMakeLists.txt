cmake_minimum_required(VERSION 3.10)
project(ArithmeticDAG)

# 设置C++标准
set(CMAKE_CXX_STANDARD 11)
set(CMAKE_CXX_STANDARD_REQUIRED ON)

# 查找Qt包
find_package(Qt5 COMPONENTS Widgets REQUIRED)

# 源文件列表
set(SOURCES
    lexer.cpp
    dag.cpp
    parser.cpp
)

# 头文件列表
set(HEADERS
    lexer.h
    dag.h
    parser.h
)

# Qt界面源文件
set(UI_SOURCES
    main_qt.cpp
    mainwindow.cpp
    dagvisualizer.cpp
)

# Qt界面头文件
set(UI_HEADERS
    mainwindow.h
    dagvisualizer.h
)

# 添加自动生成的Qt文件
set(CMAKE_AUTOMOC ON)
set(CMAKE_AUTORCC ON)
set(CMAKE_AUTOUIC ON)

# 创建命令行可执行文件
add_executable(arithmetic_dag_cli main.cpp ${SOURCES} ${HEADERS})

# 创建GUI可执行文件
add_executable(arithmetic_dag_gui ${UI_SOURCES} ${SOURCES} ${HEADERS} ${UI_HEADERS})

# 链接Qt库
target_link_libraries(arithmetic_dag_gui Qt5::Widgets)

# 如果需要安装
install(TARGETS arithmetic_dag_cli arithmetic_dag_gui DESTINATION bin) 