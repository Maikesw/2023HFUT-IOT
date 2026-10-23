#ifndef DAGVISUALIZER_H
#define DAGVISUALIZER_H

#include <QWidget>
#include <QPainter>
#include <QMouseEvent>
#include <QWheelEvent>
#include <QPoint>
#include <unordered_map>
#include <vector>
#include "dag.h"

class DAGVisualizer : public QWidget
{
    Q_OBJECT

public:
    explicit DAGVisualizer(QWidget *parent = nullptr);
    ~DAGVisualizer();

    void setDAG(const DAG& dag);
    void clearDAG();

protected:
    void paintEvent(QPaintEvent *event) override;
    void mousePressEvent(QMouseEvent *event) override;
    void mouseMoveEvent(QMouseEvent *event) override;
    void wheelEvent(QWheelEvent *event) override;

private:
    struct NodeVisual {
        int id;
        QString label;
        QPoint position;
        std::vector<int> children;
    };

    void calculateNodePositions();
    void drawNode(QPainter& painter, const NodeVisual& node);
    void drawEdge(QPainter& painter, const NodeVisual& from, const NodeVisual& to);

    std::vector<NodeVisual> nodes;
    bool hasDAG;
    
    // 用于平移和缩放
    QPoint lastMousePos;
    QPoint offset;
    double scale;
};

#endif // DAGVISUALIZER_H 