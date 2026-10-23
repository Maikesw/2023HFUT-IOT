import matplotlib.pyplot as plt
import pandas as pd

awareness_data = {
    "Awareness Level": ["非常不了解", "比较不了解", "一般", "比较了解", "非常了解"],
    "Percentage": [7.95, 19.89, 36.93, 27.84, 7.39]
}

df_awareness = pd.DataFrame(awareness_data)

plt.rcParams['font.sans-serif'] = ['SimHei']  # 使用黑体
plt.rcParams['axes.unicode_minus'] = False  # 正常显示负号

plt.figure(figsize=(10, 6))
plt.bar(df_awareness["Awareness Level"], df_awareness["Percentage"], color='lightblue')
plt.xlabel("了解程度")
plt.ylabel("比例 (%)")
plt.title("大别山红色民歌革命历史故事了解程度")
plt.show()