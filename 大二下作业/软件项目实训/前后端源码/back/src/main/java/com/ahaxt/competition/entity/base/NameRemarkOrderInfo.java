package com.ahaxt.competition.entity.base;

import lombok.Getter;
import lombok.Setter;

import javax.persistence.Column;
import javax.persistence.MappedSuperclass;

/**
 * @author hongzhangming
 */
@MappedSuperclass
@Getter
@Setter
public class NameRemarkOrderInfo extends NameRemarkInfo {
    @Column(nullable = false,columnDefinition = "smallint default '1' comment '排序号'")
    private Integer theOrder = 1;
}
