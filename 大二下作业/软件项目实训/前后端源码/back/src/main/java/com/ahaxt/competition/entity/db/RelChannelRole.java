package com.ahaxt.competition.entity.db;

import com.ahaxt.competition.entity.base.BaseInfo;
import lombok.Getter;
import lombok.Setter;

import javax.persistence.Column;
import javax.persistence.Entity;


/**
 * 频道角色关系表
 * @author liufeng
 */
@Getter
@Setter
@Entity
public class RelChannelRole extends BaseInfo {
    @Column(columnDefinition = "smallint unsigned not null comment '角色id'")
    private Integer roleId;

    @Column(columnDefinition = "smallint unsigned not null comment '频道id'")
    private Integer channelId;
}
