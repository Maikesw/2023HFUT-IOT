package com.ahaxt.competition.entity.db;

import com.ahaxt.competition.entity.base.BaseInfo;
import lombok.Getter;
import lombok.Setter;
import lombok.experimental.Accessors;

import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.Transient;

/**
 * OSS文件信息表
 *
 * @author hongzhangming
 */
@Getter
@Setter
@Entity
@Accessors(chain = true)
public class SysOssFile extends BaseInfo {
    @Column(columnDefinition = "integer unsigned not null")
    private Integer userId;
    @Column(columnDefinition = "varchar(50) ")
    private String bucketName;
    @Column(columnDefinition = "varchar(255) ")
    private String name;
    @Column(columnDefinition = "varchar(255) ")
    private String ossPath;
    @Column(columnDefinition = "varchar(50) ")
    private String suffix;
    @Column(columnDefinition = "varchar(50) ")
    private String fileSize;
    @Column(columnDefinition = "integer unsigned ")
    private Integer relationId;
    @Column(columnDefinition = "varchar(50) comment '关联表名'")
    private String tableName;
    @Column(columnDefinition = "integer unsigned not null ")
    private Integer type;
    @Column(columnDefinition = "varchar(255) comment '文件url' ")
    private String url;

    //分片上传的uploadId
    @Column(columnDefinition = "varchar(255) ")
    private String uploadId;
    //文件唯一标识（md5）
    @Column(columnDefinition = "varchar(255) ")
    private String fileIdentifier;
    //每个分片大小（byte）
    @Column(columnDefinition = "integer unsigned ")
    private Long chunkSize;
    //分片数量
    @Column(columnDefinition = "integer unsigned ")
    private Integer chunkNum;
    @Column(columnDefinition = "integer unsigned")
    private Integer duration;
    @Column(columnDefinition = "smallint  comment '排序号'")
    private Integer theOrder ;
}
