package com.ahaxt.competition.entity.db;

import com.ahaxt.competition.entity.base.BaseInfo;
import lombok.Getter;
import lombok.Setter;

import javax.persistence.Column;
import javax.persistence.Entity;

/**
 * 权限信息表
 * @author hongzhangming
 */
@Getter
@Setter
@Entity
public class RelRoleMenu extends BaseInfo {
    @Column(columnDefinition = "integer unsigned comment '角色id'")
    private Integer roleId;
    @Column(columnDefinition = "integer unsigned not null comment '菜单id'")
    private Integer menuId;
    @Column(columnDefinition = "bit(1) DEFAULT NULL")
    private Boolean visibleFlag;
    @Column(columnDefinition = "bit(1) DEFAULT NULL")
    private Boolean addFlag;
    @Column(columnDefinition = "bit(1) DEFAULT NULL")
    private Boolean deleteFlag;
    @Column(columnDefinition = "bit(1) DEFAULT NULL")
    private Boolean modifyFlag;
}