package com.ahaxt.competition.entity.db;

import com.ahaxt.competition.entity.base.BaseInfo;
import com.ahaxt.competition.entity.base.NameRemarkOrderInfo;
import lombok.Getter;
import lombok.Setter;

import javax.persistence.Column;
import javax.persistence.Entity;

/**
 * 角色信息表
 * @author hongzhangming
 */
@Getter
@Setter
@Entity
public class SysLogger extends BaseInfo {
    @Column(columnDefinition = "integer  unsigned not null")
    private Integer userId;
    @Column(columnDefinition = "varchar(255) comment ''")
    private String action;
    @Column(columnDefinition = "varchar(1000) comment ''")
    private String detail;


}