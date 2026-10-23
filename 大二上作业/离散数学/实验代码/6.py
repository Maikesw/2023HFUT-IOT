import tkinter as tk
from tkinter import simpledialog, messagebox
import math

# 定义图的可视化类
class GraphVisualizer:
    def __init__(self, root):
        self.root = root
        self.root.title("Graph Visualizer")
        # 创建一个 canvas 用于绘制图
        self.canvas = tk.Canvas(root, width=800, height=600, bg="white")
        self.canvas.pack()

        # 定义变量
        self.vertices = {}  # 存储顶点的字典，键为顶点名称，值为 (x, y) 坐标
        self.edges = {}  # 存储边的字典，键为 (start, end) 顶点对，值为权重
        self.selected_vertex = None  # 当前选中的顶点
        self.source = None  # 源点
        self.destination = None  # 终点
        self.mode = None  # 操作模式: "add" 添加顶点, "edge" 添加边, "move" 移动顶点, "delete" 删除顶点, "source" 选择源点, "destination" 选择终点

        # 创建按钮
        self.add_vertex_btn = tk.Button(root, text="添加顶点", command=self.set_add_mode)
        self.add_vertex_btn.pack(side=tk.LEFT)
        self.add_edge_btn = tk.Button(root, text="添加边", command=self.set_edge_mode)
        self.add_edge_btn.pack(side=tk.LEFT)
        self.move_vertex_btn = tk.Button(root, text="移动顶点", command=self.set_move_mode)
        self.move_vertex_btn.pack(side=tk.LEFT)
        self.delete_vertex_btn = tk.Button(root, text="删除顶点", command=self.set_delete_mode)
        self.delete_vertex_btn.pack(side=tk.LEFT)
        self.source_btn = tk.Button(root, text="选择源点", command=self.set_source_mode)
        self.source_btn.pack(side=tk.LEFT)
        self.destination_btn = tk.Button(root, text="选择终点", command=self.set_destination_mode)
        self.destination_btn.pack(side=tk.LEFT)
        self.dijkstra_btn = tk.Button(root, text="Dijkstra 算法", command=self.run_dijkstra)
        self.dijkstra_btn.pack(side=tk.LEFT)
        self.floyd_btn = tk.Button(root, text="Floyd 算法", command=self.run_floyd)
        self.floyd_btn.pack(side=tk.LEFT)

        # 绑定 canvas 事件
        self.canvas.bind("<Button-1>", self.on_canvas_click)
        self.canvas.bind("<B1-Motion>", self.on_canvas_drag)

    # 设置添加顶点模式
    def set_add_mode(self):
        self.mode = "add"

    # 设置添加边模式
    def set_edge_mode(self):
        self.mode = "edge"

    # 设置移动顶点模式
    def set_move_mode(self):
        self.mode = "move"

    # 设置删除顶点模式
    def set_delete_mode(self):
        self.mode = "delete"

    # 设置选择源点模式
    def set_source_mode(self):
        self.mode = "source"

    # 设置选择终点模式
    def set_destination_mode(self):
        self.mode = "destination"

    # 处理 canvas 点击事件
    def on_canvas_click(self, event):
        if self.mode == "add":
            self.add_vertex(event.x, event.y)
        elif self.mode == "edge":
            self.add_edge(event.x, event.y)
        elif self.mode == "move":
            self.select_vertex(event.x, event.y)
        elif self.mode == "delete":
            self.delete_vertex(event.x, event.y)
        elif self.mode == "source":
            self.select_source(event.x, event.y)
        elif self.mode == "destination":
            self.select_destination(event.x, event.y)

    # 处理 canvas 拖动事件
    def on_canvas_drag(self, event):
        if self.mode == "move" and self.selected_vertex:
            self.move_vertex(event.x, event.y)

    # 添加顶点
    def add_vertex(self, x, y):
        name = f"V{len(self.vertices) + 1}"  # 生成新的顶点名称
        self.vertices[name] = (x, y)  # 存储顶点坐标
        self.draw_vertex(name, x, y)  # 绘制顶点

    # 绘制顶点
    def draw_vertex(self, name, x, y):
        self.canvas.create_oval(x - 15, y - 15, x + 15, y + 15, fill="lightgray", tags=name)  # 绘制顶点圆
        self.canvas.create_text(x, y, text=name, tags=name)  # 绘制顶点名称

    # 添加边
    def add_edge(self, x, y):
        vertex = self.get_vertex_at_position(x, y)  # 获取点击位置的顶点
        if vertex:
            if self.selected_vertex:
                weight = simpledialog.askinteger("权重", "输入边的权重：", minvalue=1)  # 弹出对话框输入边的权重
                if weight:                                                                                                                        
                    self.edges[(self.selected_vertex, vertex)] = weight  # 存储边的权重
                    self.draw_edge(self.selected_vertex, vertex, weight)  # 绘制边
                self.selected_vertex = None  # 重置选中的顶点
            else:
                self.selected_vertex = vertex  # 选中起点顶点

    # 绘制边
    def draw_edge(self, start, end, weight):
        x1, y1 = self.vertices[start]  # 获取起点坐标
        x2, y2 = self.vertices[end]  # 获取终点坐标
        self.canvas.create_line(x1, y1, x2, y2, arrow=tk.LAST)  # 绘制带箭头的边
        self.canvas.create_text((x1 + x2) // 2, (y1 + y2) // 2, text=str(weight))  # 绘制边的权重

    # 选中顶点
    def select_vertex(self, x, y):
        self.selected_vertex = self.get_vertex_at_position(x, y)  # 获取点击位置的顶点

    # 移动顶点
    def move_vertex(self, x, y):
        if self.selected_vertex:
            self.vertices[self.selected_vertex] = (x, y)  # 更新顶点坐标
            self.redraw()  # 重新绘制整个图

    # 删除顶点
    def delete_vertex(self, x, y):
        vertex = self.get_vertex_at_position(x, y)  # 获取点击位置的顶点
        if vertex:
            del self.vertices[vertex]  # 删除顶点
            self.edges = {(u, v): w for (u, v), w in self.edges.items() if u != vertex and v != vertex}  # 删除与该顶点相关的所有边
            self.redraw()  # 重新绘制整个图

    # 选择源点
    def select_source(self, x, y):
        vertex = self.get_vertex_at_position(x, y)  # 获取点击位置的顶点
        if vertex:
            self.source = vertex  # 设置源点
            self.redraw()  # 重新绘制整个图

    # 选择终点
    def select_destination(self, x, y):
        vertex = self.get_vertex_at_position(x, y)  # 获取点击位置的顶点
        if vertex:
            self.destination = vertex  # 设置终点
            self.redraw()  # 重新绘制整个图

    # 获取点击位置的顶点
    def get_vertex_at_position(self, x, y):
        for name, (vx, vy) in self.vertices.items():
            if (x - vx) ** 2 + (y - vy) ** 2 <= 15 ** 2:  # 检查点击位置是否在顶点范围内
                return name
        return None

    # 重新绘制整个图
    def redraw(self):
        self.canvas.delete("all")  # 清除 canvas 上的所有内容
        for (start, end), weight in self.edges.items():
            self.draw_edge(start, end, weight)  # 重新绘制所有边
        for name, (x, y) in self.vertices.items():
            self.draw_vertex(name, x, y)  # 重新绘制所有顶点
        if self.source:
            x, y = self.vertices[self.source]
            self.canvas.create_oval(x - 15, y - 15, x + 15, y + 15, fill="red", tags=self.source)  # 高亮显示源点
            self.canvas.create_text(x, y, text=self.source, tags=self.source)
        if self.destination:
            x, y = self.vertices[self.destination]
            self.canvas.create_oval(x - 15, y - 15, x + 15, y + 15, fill="blue", tags=self.destination)  # 高亮显示终点
            self.canvas.create_text(x, y, text=self.destination, tags=self.destination)

    # 运行 Dijkstra 算法
    def run_dijkstra(self):
        if not self.source or not self.destination:
            messagebox.showerror("错误", "请先选择源点和终点")  # 提示用户选择源点和终点
            return
        distances, previous = self.dijkstra()  # 计算最短路径
        path = self.get_path(previous, self.destination)  # 获取路径
        if distances[self.destination] == math.inf:
            messagebox.showinfo("结果", "无法到达终点")  # 提示无法到达终点
        else:
            messagebox.showinfo("结果", f"最短路径长度: {distances[self.destination]}\n路径: {' -> '.join(path)}")  # 显示最短路径长度和路径

    # Dijkstra 算法实现
    def dijkstra(self):
        distances = {vertex: math.inf for vertex in self.vertices}  # 初始化所有顶点的距离为无穷大
        previous = {vertex: None for vertex in self.vertices}  # 初始化所有顶点的前驱为 None
        distances[self.source] = 0  # 源点的距离为 0
        unvisited = set(self.vertices)  # 未访问的顶点集合

        while unvisited:
            current = min(unvisited, key=lambda vertex: distances[vertex])  # 选择距离最小的顶点
            unvisited.remove(current)  # 从未访问集合中移除该顶点

            if distances[current] == math.inf:
                break  # 如果距离为无穷大，说明无法到达

            for neighbor in self.get_neighbors(current):
                alt = distances[current] + self.edges[(current, neighbor)]  # 计算新路径的长度
                if alt < distances[neighbor]:
                    distances[neighbor] = alt  # 更新距离
                    previous[neighbor] = current  # 更新前驱

        return distances, previous

    # 获取顶点的邻居
    def get_neighbors(self, vertex):
        return [end for (start, end) in self.edges if start == vertex]  # 返回所有与该顶点相连的顶点

    # 获取路径
    def get_path(self, previous, target):
        path = []
        while target:
            path.insert(0, target)  # 将顶点插入路径的开头
            target = previous[target]  # 更新目标顶点为前驱
        return path

    # 运行 Floyd 算法
    def run_floyd(self):
        if not self.source or not self.destination:
            messagebox.showerror("错误", "请先选择源点和终点")  # 提示用户选择源点和终点
            return
        dist, next_vertex = self.floyd()  # 计算最短路径
        if dist[self.source][self.destination] == math.inf:
            messagebox.showinfo("结果", "无法到达终点")  # 提示无法到达终点
        else:
            path = self.get_floyd_path(next_vertex, self.source, self.destination)  # 获取路径
            messagebox.showinfo("结果", f"最短路径长度: {dist[self.source][self.destination]}\n路径: {' -> '.join(path)}")  # 显示最短路径长度和路径

    # Floyd 算法实现
    def floyd(self):
        n = len(self.vertices)
        names = list(self.vertices)
        dist = {name: {name2: math.inf for name2 in names} for name in names}  # 初始化距离矩阵
        next_vertex = {name: {name2: None for name2 in names} for name in names}  # 初始化前驱矩阵

        for name in names:
            dist[name][name] = 0  # 顶点到自身的距离为 0
        for (start, end), weight in self.edges.items():
            dist[start][end] = weight  # 初始化边的权重
            next_vertex[start][end] = end  # 初始化前驱

        for k in names:
            for i in names:
                for j in names:
                    if dist[i][j] > dist[i][k] + dist[k][j]:
                        dist[i][j] = dist[i][k] + dist[k][j]  # 更新距离
                        next_vertex[i][j] = next_vertex[i][k]  # 更新前驱

        return dist, next_vertex

    # 获取 Floyd 算法的路径
    def get_floyd_path(self, next_vertex, start, end):
        if not next_vertex[start][end]:
            return []  # 如果没有路径，返回空列表
        path = [start]
        while start != end:
            start = next_vertex[start][end]
            path.append(start)
        return path

# 主程序入口
if __name__ == "__main__":
    root = tk.Tk()  # 创建主窗口
    app = GraphVisualizer(root)  # 创建图可视化器实例
    root.mainloop()  # 运行窗口的主事件循环