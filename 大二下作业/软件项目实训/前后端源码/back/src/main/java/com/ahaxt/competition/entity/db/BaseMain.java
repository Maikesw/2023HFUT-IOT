package com.ahaxt.competition.entity.db;

import com.ahaxt.competition.entity.base.BaseInfo;
import lombok.Getter;
import lombok.Setter;

import javax.persistence.Column;
import javax.persistence.Entity;

/**
 * 网站基本信息表
 * @author liufeng
 */
@Getter
@Setter
@Entity
public class BaseMain extends BaseInfo {
    @Column(columnDefinition = "varchar(255) comment '本站名称'")
    private String mainName;
    @Column(columnDefinition = "varchar(255) comment '本站地址'")
    private String mainUrl;
}
