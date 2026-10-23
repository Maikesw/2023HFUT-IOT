package com.ahaxt.competition.entity.db;

import com.ahaxt.competition.entity.base.AuditInfo;
import lombok.Getter;
import lombok.Setter;

import javax.persistence.Column;
import javax.persistence.Entity;

/**
 * 用户角色表
 * @author hongzhangming
 */
@Getter
@Setter
@Entity
public class RelUserRole extends AuditInfo {
    @Column(columnDefinition = "integer  unsigned not null")
    private Integer userId;
    @Column(columnDefinition = "integer unsigned not null")
    private Integer roleId;
}