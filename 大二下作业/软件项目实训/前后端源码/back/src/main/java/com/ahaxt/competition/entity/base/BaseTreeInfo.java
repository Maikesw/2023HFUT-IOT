package com.ahaxt.competition.entity.base;

import lombok.Getter;
import lombok.Setter;

import javax.persistence.Column;
import javax.persistence.MappedSuperclass;
import javax.persistence.Transient;
import java.util.List;

/**
 * @author hongzhangming
 */
@MappedSuperclass
@Getter
@Setter
public class BaseTreeInfo<Tree> extends NameRemarkOrderInfo {
    @Column(nullable = false,columnDefinition = "integer default '-1' comment '父节点编号 默认 是 -1'")
    private Integer parentId;
    @Column(nullable = false,columnDefinition = " smallint default '1' comment '等级号'")
    private Integer theLevel;
    @Column(nullable = false,columnDefinition = "bit(1) default b'1' comment '是否是叶子节点'")
    private Boolean isLeaf;
    @Column(nullable = false,columnDefinition = " smallint default '0' comment '孩子节点数量'")
    private Integer childNum;
    @Transient
    private String allNodeNames;
    @Transient
    private List<Tree> children;
    @Transient
    private Boolean hasChildren;
    @Transient
    private String parentName;
}
