package com.ahaxt.competition.entity.base;

import lombok.Getter;
import lombok.Setter;

import javax.persistence.Column;
import javax.persistence.MappedSuperclass;
import java.util.Date;

/**
 * @author hongzhangming
 */
@MappedSuperclass
@Getter
@Setter
public class AuditInfo extends BaseInfo {
    @Column(nullable = false,columnDefinition = "smallint default '1' comment '是否审核'")
    private Integer isAudit;
    @Column(columnDefinition = "varchar(50) comment '审核理由'")
    private String reason;
    @Column(columnDefinition = "integer unsigned comment '最后审核人id'")
    private Integer auditUserId;
    @Column(columnDefinition = " datetime default CURRENT_TIMESTAMP comment '最后审核时间'")
    private Date auditTime;
}
