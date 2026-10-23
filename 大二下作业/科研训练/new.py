import numpy as np
import matplotlib.pyplot as plt
from mpl_toolkits.mplot3d import Axes3D
from mpl_toolkits.mplot3d import art3d
from matplotlib.tri import Triangulation
import matplotlib.patches as patches
from matplotlib.colors import LinearSegmentedColormap

# 设置中文字体
plt.rcParams['font.sans-serif'] = ['SimHei']  # 用来正常显示中文标签
plt.rcParams['axes.unicode_minus'] = False    # 用来正常显示负号

def create_simple_3d_model():
    """创建一个简单的3D曲面模型（半球形）"""
    u = np.linspace(0, 2 * np.pi, 20)
    v = np.linspace(0, np.pi/2, 10)
    u, v = np.meshgrid(u, v)
    
    # 半球坐标
    x = np.cos(u) * np.sin(v)
    y = np.sin(u) * np.sin(v)
    z = np.cos(v)
    
    return x, y, z

def triangulate_mesh(x, y, z):
    """对3D网格进行三角化处理"""
    points = np.vstack([x.flatten(), y.flatten(), z.flatten()]).T
    
    # 创建三角形面片索引
    rows, cols = x.shape
    triangles = []
    for i in range(rows-1):
        for j in range(cols-1):
            p1 = i * cols + j
            p2 = i * cols + (j + 1)
            p3 = (i + 1) * cols + j
            p4 = (i + 1) * cols + (j + 1)
            
            # 添加两个三角形
            triangles.append([p1, p2, p3])
            triangles.append([p2, p4, p3])
    
    return points, np.array(triangles)

def create_uv_mapping(points, triangles):
    """模拟LSCM (Least Squares Conformal Maps)映射过程"""
    # 在实际应用中，这里会使用LSCM算法求解
    # 这里我们简化为球面投影到平面的映射
    
    # 提取3D点的球坐标
    x, y, z = points[:, 0], points[:, 1], points[:, 2]
    
    # 计算球面坐标对应的UV坐标 (简化的映射)
    u = 0.5 + np.arctan2(y, x) / (2 * np.pi)
    v = 0.5 - np.arcsin(z) / np.pi
    
    uv_coords = np.vstack([u, v]).T
    return uv_coords

def create_texture_pattern():
    """创建一个简单的纹理图案（网格图案）"""
    pattern = np.zeros((100, 100, 3))
    
    # 创建网格线
    for i in range(0, 100, 10):
        pattern[i:i+2, :, :] = 0.8
        pattern[:, i:i+2, :] = 0.8
    
    # 添加一些圆形图案
    for center_x, center_y, radius in [(25, 25, 8), (75, 75, 8), (25, 75, 8), (75, 25, 8)]:
        for i in range(100):
            for j in range(100):
                if (i - center_x)**2 + (j - center_y)**2 < radius**2:
                    pattern[j, i, 0] = 1.0  # 红色
                    pattern[j, i, 1] = 0.5  # 绿色
                    pattern[j, i, 2] = 0.5  # 蓝色
    
    return pattern

def apply_texture_to_3d_model(points, triangles, uv_coords, texture):
    """将纹理应用到3D模型上"""
    # 计算每个三角形的颜色（基于UV坐标和纹理）
    triangle_colors = []
    
    for triangle in triangles:
        # 获取三角形的UV坐标
        uvs = uv_coords[triangle]
        
        # 计算UV坐标在纹理中的位置
        h, w = texture.shape[:2]
        u_pixels = (uvs[:, 0] * (w-1)).astype(int)
        v_pixels = (uvs[:, 1] * (h-1)).astype(int)
        
        # 获取颜色（使用UV坐标的平均值）
        u_avg = int(np.mean(u_pixels))
        v_avg = int(np.mean(v_pixels))
        color = texture[v_avg, u_avg]
        
        triangle_colors.append(color)
    
    return np.array(triangle_colors)

def visualize_mapping_process():
    """可视化整个映射过程"""
    fig = plt.figure(figsize=(15, 12))
    
    # 1. 原始3D模型
    ax1 = fig.add_subplot(221, projection='3d')
    x, y, z = create_simple_3d_model()
    ax1.plot_surface(x, y, z, color='lightgray', alpha=0.7, edgecolor='gray')
    ax1.set_title('1. 网格模型三角化预处理', fontsize=14)
    ax1.set_xlabel('X')
    ax1.set_ylabel('Y')
    ax1.set_zlabel('Z')
    ax1.view_init(elev=30, azim=45)
    
    # 获取三角化的网格
    points, triangles = triangulate_mesh(x, y, z)
    
    # 2. 边界约束计算与LSCM映射
    ax2 = fig.add_subplot(222)
    uv_coords = create_uv_mapping(points, triangles)
    
    # 显示UV平面上的三角形
    for triangle in triangles:
        triangle_uvs = uv_coords[triangle]
        tri = patches.Polygon(triangle_uvs, fill=False, edgecolor='blue', alpha=0.5)
        ax2.add_patch(tri)
    
    ax2.set_xlim(0, 1)
    ax2.set_ylim(0, 1)
    ax2.set_aspect('equal')
    ax2.set_title('2. 边界约束计算与LSCM映射', fontsize=14)
    ax2.set_xlabel('U')
    ax2.set_ylabel('V')
    ax2.grid(True, linestyle='--', alpha=0.7)
    
    # 3. 在UV平面上应用纹理
    ax3 = fig.add_subplot(223)
    texture = create_texture_pattern()
    ax3.imshow(texture)
    
    # 在纹理上叠加UV网格
    for triangle in triangles:
        triangle_uvs = uv_coords[triangle]
        h, w = texture.shape[:2]
        scaled_uvs = triangle_uvs.copy()
        scaled_uvs[:, 0] *= w-1
        scaled_uvs[:, 1] *= h-1
        tri = patches.Polygon(scaled_uvs, fill=False, edgecolor='white', alpha=0.5)
        ax3.add_patch(tri)
    
    ax3.set_title('3. 纹理坐标生成', fontsize=14)
    ax3.set_xlabel('U')
    ax3.set_ylabel('V')
    
    # 4. 将纹理映射回3D模型
    ax4 = fig.add_subplot(224, projection='3d')
    triangle_colors = apply_texture_to_3d_model(points, triangles, uv_coords, texture)
    
    # 绘制带纹理的3D模型
    for i, triangle in enumerate(triangles):
        vertices = points[triangle]
        tri = art3d.Poly3DCollection([vertices], alpha=0.9)
        tri.set_facecolor(triangle_colors[i])
        tri.set_edgecolor('gray')
        ax4.add_collection3d(tri)
    
    ax4.set_title('4. 纹理坐标映射', fontsize=14)
    ax4.set_xlabel('X')
    ax4.set_ylabel('Y')
    ax4.set_zlabel('Z')
    ax4.view_init(elev=30, azim=45)
    ax4.set_xlim(-1, 1)
    ax4.set_ylim(-1, 1)
    ax4.set_zlim(0, 1)
    
    plt.tight_layout()
    plt.suptitle('图4-3：三维映射过程可视化', fontsize=16, y=0.98)
    plt.subplots_adjust(top=0.9)
    plt.show()

if __name__ == "__main__":
    visualize_mapping_process()