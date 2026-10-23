package com.ahaxt.competition.entity.db;

import com.ahaxt.competition.entity.base.BaseTreeInfo;
import lombok.Getter;
import lombok.Setter;

import javax.persistence.Column;
import javax.persistence.Entity;


/**
 * 频道类别信息表
 * @author liufeng
 */
@Getter
@Setter
@Entity
public class BaseChannelType extends BaseTreeInfo {

    @Column(columnDefinition = "varchar(255) comment '前端路由地址'")
    private String path;
    @Column(columnDefinition = "varchar(255) comment '前端组件路径 指向文件'")
    private String component;
    @Column(columnDefinition = "bit(1) default b'0' comment '是否隐藏'" )
    private Boolean hidden=false ;
}
