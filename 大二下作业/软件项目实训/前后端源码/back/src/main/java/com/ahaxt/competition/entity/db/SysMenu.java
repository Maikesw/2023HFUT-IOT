package com.ahaxt.competition.entity.db;
import com.ahaxt.competition.entity.base.BaseTreeInfo;
import lombok.Getter;
import lombok.Setter;

import javax.persistence.Column;
import javax.persistence.Entity;

/**
 * 菜单信息表
 * @author hongzhangming
 */
@Getter
@Setter
@Entity
public class SysMenu extends BaseTreeInfo {
    @Column(columnDefinition = "varchar(255) comment '图标'")
    private String icon;
    @Column(columnDefinition = "varchar(255) comment '前端路由地址'")
    private String path;
    @Column(columnDefinition = "varchar(255) comment '前端组件路径 指向文件'")
    private String component;
    @Column(columnDefinition = "bit(1) default b'0' comment '是否隐藏'" )
    private Boolean hidden=false ;
}
