package com.ahaxt.competition.entity.db;

import com.ahaxt.competition.entity.base.BaseInfo;
import lombok.Getter;
import lombok.Setter;

import javax.persistence.Column;
import javax.persistence.Entity;


/**
 * 网站留言本表
 * @author liufeng
 */
@Getter
@Setter
@Entity
public class BaseGuestBook extends BaseInfo {
    @Column(columnDefinition = "varchar(255) comment '留言本名称'")
    private String guestBName;

    @Column(columnDefinition = "varchar(255) comment '留言本头部'")
    private String guestBHead;
}
