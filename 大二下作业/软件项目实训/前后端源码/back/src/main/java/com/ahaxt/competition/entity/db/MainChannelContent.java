package com.ahaxt.competition.entity.db;

import com.ahaxt.competition.entity.base.BaseInfo;
import lombok.Getter;
import lombok.Setter;
import org.springframework.data.annotation.CreatedDate;

import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.Transient;
import java.util.Date;
import java.util.List;


/**
 * 频道内容信息表
 * @author liufeng
 */
@Getter
@Setter
@Entity
public class MainChannelContent extends BaseInfo {
    @Column(columnDefinition = "smallint unsigned not null comment '频道类别id'")
    private Integer channelTypeId;
    @Column(columnDefinition = "varchar(255) comment '标题'")
    private String channelCName;
    @Column(columnDefinition = "varchar(255) comment '内容'")
    private String channelCText;
    @Column(columnDefinition = "integer unsigned comment '点击次数'")
    private Integer hitTimes;
    @Column(columnDefinition = " bit(1) default b'0' comment '是否发布'")
    private Boolean publishStatus=false;
    @Column(columnDefinition = " datetime  comment '发布时间'")
    private Date channelCTime;
    @Column(columnDefinition = "integer unsigned comment '历史id'")
    private Integer oldId;
    @Column(columnDefinition = " bit(1) default b'0' comment '是否轮播图'")
    private Boolean channelCSlider=false;
    @Transient
    private String firstImg;
    @Transient
    private List<SysOssFile> files;


}
