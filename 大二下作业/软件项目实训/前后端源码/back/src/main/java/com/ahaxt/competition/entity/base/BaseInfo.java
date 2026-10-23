package com.ahaxt.competition.entity.base;


import lombok.Getter;
import lombok.Setter;
import org.springframework.data.annotation.CreatedBy;
import org.springframework.data.annotation.CreatedDate;
import org.springframework.data.annotation.LastModifiedDate;
import org.springframework.data.jpa.domain.support.AuditingEntityListener;

import javax.persistence.*;
import java.io.Serializable;
import java.util.Date;


/**
 * 公有字段
 *
 * @author hongzhangming
 */
@MappedSuperclass
@Getter
@Setter
@EntityListeners(AuditingEntityListener.class)
public class BaseInfo implements Serializable {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(columnDefinition = "integer unsigned comment '编号'")
    private Integer id;

    @CreatedDate
    @Column(nullable = false, columnDefinition = " datetime default CURRENT_TIMESTAMP comment '创建时间'", updatable = false )
    private Date createTime;

    @LastModifiedDate
    @Column(nullable = false, columnDefinition = "datetime default CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP comment '更新时间'" )
    private Date updateTime;

    @CreatedBy
    @Column(nullable = false, columnDefinition = "bit(1) default b'0' comment '删除标记'" )
    private Boolean isDeleted = false;


}
