#include "dagvisualizer.h"
#include <queue>
#include <map>
#include <set>
#include <algorithm>

DAGVisualizer::DAGVisualizer(QWidget *parent)
    : QWidget(parent), hasDAG(false), offset(0, 0), scale(1.0)
{
    setMinimumSize(400, 300);
    setMouseTracking(true);
}

DAGVisualizer::~DAGVisualizer()
{
}

void DAGVisualizer::setDAG(const DAG& dag)
{
    nodes.clear();
    
    // 获取DAG的节点信息
    const std::vector<DAGNode>& dagNodes = dag.getNodes();
    
    // 创建可视化节点
    for (size_t i = 0; i < dagNodes.size(); ++i) {
        const DAGNode& dagNode = dagNodes[i];
        NodeVisual node;
        node.id = i;
        
        // 设置节点标签
        if (dagNode.isLeaf()) {
            node.label = QString::fromStdString(dagNode.getName());
        } else {
            node.label = QString::fromStdString(std::string(1, dagNode.getOp()));
        }
        
        // 添加子节点引用
        if (dagNode.getLeft() != -1) {
            node.children.push_back(dagNode.getLeft());
        }
        if (dagNode.getRight() != -1) {
            node.children.push_back(dagNode.getRight());
        }
        
        nodes.push_back(node);
    }
    
    hasDAG = true;
    calculateNodePositions();
    update();
}

void DAGVisualizer::clearDAG()
{
    nodes.clear();
    hasDAG = false;
    update();
}

void DAGVisualizer::paintEvent(QPaintEvent* event)
{
    Q_UNUSED(event);
    
    QPainter painter(this);
    painter.setRenderHint(QPainter::Antialiasing);
    
    // 填充背景
    painter.fillRect(rect(), Qt::white);
    
    if (!hasDAG) {
        painter.drawText(rect(), Qt::AlignCenter, "无DAG数据");
        return;
    }
    
    // 应用缩放和平移
    painter.translate(offset);
    painter.scale(scale, scale);
    
    // 先画边，再画节点，避免边覆盖节点
    for (const NodeVisual& node : nodes) {
        for (int childId : node.children) {
            auto it = std::find_if(nodes.begin(), nodes.end(), 
                [childId](const NodeVisual& n) { return n.id == childId; });
            if (it != nodes.end()) {
                drawEdge(painter, node, *it);
            }
        }
    }
    
    for (const NodeVisual& node : nodes) {
        drawNode(painter, node);
    }
}

void DAGVisualizer::mousePressEvent(QMouseEvent* event)
{
    if (event->button() == Qt::LeftButton) {
        lastMousePos = event->pos();
    }
}

void DAGVisualizer::mouseMoveEvent(QMouseEvent* event)
{
    if (event->buttons() & Qt::LeftButton) {
        QPoint delta = event->pos() - lastMousePos;
        offset += delta;
        lastMousePos = event->pos();
        update();
    }
}

void DAGVisualizer::wheelEvent(QWheelEvent* event)
{
    // 缩放因子
    const double scaleFactor = 1.1;
    
    // 获取鼠标位置
    QPoint pos = event->position().toPoint();
    
    // 计算鼠标相对于当前视图的位置
    QPointF mousePos = (pos - offset) / scale;
    
    // 根据滚轮方向调整缩放
    if (event->angleDelta().y() > 0) {
        scale *= scaleFactor;
    } else {
        scale /= scaleFactor;
    }
    
    // 限制缩放范围
    scale = qBound(0.1, scale, 5.0);
    
    // 调整偏移，使鼠标指向的点保持在同一位置
    offset = pos - mousePos * scale;
    
    update();
}

void DAGVisualizer::calculateNodePositions()
{
    if (nodes.empty()) return;
    
    // 找出根节点（没有父节点的节点）
    std::set<int> childIds;
    for (const NodeVisual& node : nodes) {
        for (int childId : node.children) {
            childIds.insert(childId);
        }
    }
    
    std::vector<int> rootIds;
    for (const NodeVisual& node : nodes) {
        if (childIds.find(node.id) == childIds.end()) {
            rootIds.push_back(node.id);
        }
    }
    
    // 如果没有根节点，使用最后一个节点作为根节点
    if (rootIds.empty() && !nodes.empty()) {
        rootIds.push_back(nodes.back().id);
    }
    
    // 计算每个节点的层级
    std::map<int, int> nodeLevels;
    std::queue<std::pair<int, int>> queue; // <nodeId, level>
    
    for (int rootId : rootIds) {
        queue.push({rootId, 0});
    }
    
    while (!queue.empty()) {
        auto [nodeId, level] = queue.front();
        queue.pop();
        
        // 如果节点已经有层级，并且新层级更高，则更新
        if (nodeLevels.find(nodeId) != nodeLevels.end()) {
            nodeLevels[nodeId] = std::min(nodeLevels[nodeId], level);
        } else {
            nodeLevels[nodeId] = level;
        }
        
        // 将子节点加入队列
        auto it = std::find_if(nodes.begin(), nodes.end(), 
            [nodeId](const NodeVisual& n) { return n.id == nodeId; });
        if (it != nodes.end()) {
            for (int childId : it->children) {
                queue.push({childId, level + 1});
            }
        }
    }
    
    // 计算每一层的节点数量
    std::map<int, int> levelCounts;
    for (const auto& [nodeId, level] : nodeLevels) {
        levelCounts[level]++;
    }
    
    // 计算每一层的节点位置
    std::map<int, int> levelCurrentPos;
    const int nodeWidth = 60;
    const int nodeHeight = 40;
    const int levelHeight = 100;
    
    for (auto& node : nodes) {
        int level = nodeLevels[node.id];
        int levelCount = levelCounts[level];
        int levelWidth = levelCount * nodeWidth;
        
        // 计算该层的起始x位置，使节点居中
        int levelStartX = (width() - levelWidth) / 2;
        
        // 如果是该层的第一个节点，初始化当前位置
        if (levelCurrentPos.find(level) == levelCurrentPos.end()) {
            levelCurrentPos[level] = levelStartX;
        }
        
        // 设置节点位置
        node.position = QPoint(levelCurrentPos[level] + nodeWidth / 2, level * levelHeight + nodeHeight);
        
        // 更新该层的当前位置
        levelCurrentPos[level] += nodeWidth;
    }
}

void DAGVisualizer::drawNode(QPainter& painter, const NodeVisual& node)
{
    const int nodeRadius = 20;
    
    // 绘制节点圆形
    painter.setPen(QPen(Qt::black, 2));
    painter.setBrush(Qt::white);
    painter.drawEllipse(node.position, nodeRadius, nodeRadius);
    
    // 绘制节点标签
    painter.setPen(Qt::black);
    QFont font = painter.font();
    font.setBold(true);
    painter.setFont(font);
    painter.drawText(QRect(node.position.x() - nodeRadius, node.position.y() - nodeRadius,
                          nodeRadius * 2, nodeRadius * 2),
                    Qt::AlignCenter, node.label);
}

void DAGVisualizer::drawEdge(QPainter& painter, const NodeVisual& from, const NodeVisual& to)
{
    const int nodeRadius = 20;
    const double arrowSize = 10.0;
    
    // 计算边的起点和终点
    QLineF line(from.position, to.position);
    
    // 调整起点和终点，使其位于节点边缘
    double angle = std::atan2(-line.dy(), line.dx());
    QPointF fromPoint = from.position - QPointF(std::cos(angle) * nodeRadius, -std::sin(angle) * nodeRadius);
    QPointF toPoint = to.position + QPointF(std::cos(angle) * nodeRadius, -std::sin(angle) * nodeRadius);
    
    // 绘制边
    painter.setPen(QPen(Qt::black, 1.5));
    painter.drawLine(fromPoint, toPoint);
    
    // 绘制箭头
    double arrowAngle = std::atan2(line.dy(), line.dx()) + M_PI;
    QPointF arrowP1 = toPoint + QPointF(std::cos(arrowAngle + M_PI / 6) * arrowSize,
                                       std::sin(arrowAngle + M_PI / 6) * arrowSize);
    QPointF arrowP2 = toPoint + QPointF(std::cos(arrowAngle - M_PI / 6) * arrowSize,
                                       std::sin(arrowAngle - M_PI / 6) * arrowSize);
    
    QPolygonF arrow;
    arrow << toPoint << arrowP1 << arrowP2;
    painter.setBrush(Qt::black);
    painter.drawPolygon(arrow);
} 
 