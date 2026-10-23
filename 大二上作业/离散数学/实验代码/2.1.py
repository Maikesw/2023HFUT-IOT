import tkinter as tk
from tkinter import messagebox
from threading import Thread
import time

class TrafficLightApp:
    def __init__(self, root):
        # 初始化主窗口
        self.root = root
        self.root.title("交通信号灯模型")

        # 交通信号灯的当前状态
        self.current_state = "Green"
        # 状态是否正在运行
        self.running = False
        # 各个灯的亮灯时间
        self.green_time = 5
        self.yellow_time = 3
        self.red_time = 5
        # 黄灯闪烁的时间间隔
        self.flash_interval = 0.5

        # 创建一个画布，用于显示交通信号灯
        self.canvas = tk.Canvas(self.root, width=200, height=400, bg="white")
        self.canvas.pack()

        # 画布上绘制三个圆形，分别代表红灯、黄灯和绿灯
        self.green_light = self.canvas.create_oval(50, 50, 270, 150, fill="gray")
        self.yellow_light = self.canvas.create_oval(50, 160, 150, 260, fill="gray")
        self.red_light = self.canvas.create_oval(50, 150, 150, 370, fill="gray")

        # 更新信号灯状态
        self.update_light()

        # 创建一个控制框架，用于放置控制按钮
        self.control_frame = tk.Frame(self.root)
        self.control_frame.pack()

        # 创建开始按钮，点击后启动交通信号灯
        self.start_button = tk.Button(self.control_frame, text="开始", command=self.start_traffic_light)
        self.start_button.grid(row=0, column=0, padx=10, pady=10)

        # 创建停止按钮，点击后停止交通信号灯
        self.stop_button = tk.Button(self.control_frame, text="停止", command=self.stop_traffic_light)
        self.stop_button.grid(row=0, column=1, padx=10, pady=10)

        # 创建设置按钮，点击后打开设置窗口
        self.settings_button = tk.Button(self.control_frame, text="设置", command=self.open_settings)
        self.settings_button.grid(row=0, column=2, padx=10, pady=10)

    def update_light(self):
        # 将所有灯的颜色设置为灰色
        self.canvas.itemconfig(self.green_light, fill="gray")
        self.canvas.itemconfig(self.yellow_light, fill="gray")
        self.canvas.itemconfig(self.red_light, fill="gray")

        # 根据当前状态更新相应灯的颜色
        if self.current_state == "Green":
            self.canvas.itemconfig(self.green_light, fill="green")
        elif self.current_state == "Yellow":
            self.canvas.itemconfig(self.yellow_light, fill="yellow")
        elif self.current_state == "Red":
            self.canvas.itemconfig(self.red_light, fill="red")

    def start_traffic_light(self):
        # 如果当前不在运行状态，则启动交通信号灯
        if not self.running:
            self.running = True
            # 创建一个新的线程来运行交通信号灯的定时逻辑
            self.traffic_light_thread = Thread(target=self.run_traffic_light,daemon=True)
            self.traffic_light_thread.start()

    def stop_traffic_light(self):
        # 停止交通信号灯的运行
        self.running = False

    def run_traffic_light(self):
        # 运行交通信号灯的主循环
        while self.running:
            if self.current_state == "Green":
                # 绿灯转黄灯
                self.current_state = "Yellow"
                self.update_light()
                self.flash_yellow_light()
            elif self.current_state == "Yellow":
                # 黄灯转红灯
                self.current_state = "Red"
                self.update_light()
                time.sleep(self.red_time)
            elif self.current_state == "Red":
                # 红灯转绿灯
                self.current_state = "Green"
                self.update_light()
                time.sleep(self.green_time)

    def flash_yellow_light(self):
        # 闪烁黄灯
        for _ in range(int(self.yellow_time / self.flash_interval)):
            if not self.running:
                # 如果交通信号灯停止运行，立即退出闪烁循环
                return
            # 切换黄灯的颜色
            self.canvas.itemconfig(self.yellow_light, fill="gray" if self.canvas.itemcget(self.yellow_light, "fill") == "yellow" else "yellow")
            time.sleep(self.flash_interval)
        # 闪烁结束后，更新信号灯状态
        self.update_light()

    def open_settings(self):
        # 打开设置窗口
        settings_window = tk.Toplevel(self.root)
        settings_window.title("Settings")

        # 创建绿灯时间的标签和输入框
        tk.Label(settings_window, text="Green Light Time:").grid(row=0, column=0, padx=10, pady=10)
        green_time_entry = tk.Entry(settings_window)
        green_time_entry.grid(row=0, column=1, padx=10, pady=10)
        # 设置初始值
        green_time_entry.insert(0, str(self.green_time))

        # 创建黄灯时间的标签和输入框
        tk.Label(settings_window, text="Yellow Light Time:").grid(row=1, column=0, padx=10, pady=10)
        yellow_time_entry = tk.Entry(settings_window)
        yellow_time_entry.grid(row=1, column=1, padx=10, pady=10)
        # 设置初始值
        yellow_time_entry.insert(0, str(self.yellow_time))

        # 创建红灯时间的标签和输入框
        tk.Label(settings_window, text="Red Light Time:").grid(row=2, column=0, padx=10, pady=10)
        red_time_entry = tk.Entry(settings_window)
        red_time_entry.grid(row=2, column=1, padx=10, pady=10)
        # 设置初始值
        red_time_entry.insert(0, str(self.red_time))

        # 定义保存设置的函数
        def save_settings():
            try:
                # 将输入框的值转换为整数并保存
                self.green_time = int(green_time_entry.get())
                self.yellow_time = int(yellow_time_entry.get())
                self.red_time = int(red_time_entry.get())
                # 显示保存成功的消息
                messagebox.showinfo("Settings", "设置成功")
                # 关闭设置窗口
                settings_window.destroy()
            except ValueError:
                # 如果输入的不是有效数字，显示错误消息
                messagebox.showerror("Error", "请输入有效数字")

        # 创建保存按钮，点击后调用 save_settings 函数
        save_button = tk.Button(settings_window, text="保存", command=save_settings)
        save_button.grid(row=3, column=0, columnspan=2, pady=10)

# 创建主窗口
root = tk.Tk()
# 创建应用实例
app = TrafficLightApp(root)
# 进入主事件循环
root.mainloop()