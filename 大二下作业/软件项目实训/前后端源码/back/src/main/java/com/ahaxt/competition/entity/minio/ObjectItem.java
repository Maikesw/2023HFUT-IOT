package com.ahaxt.competition.entity.minio;

import lombok.Data;

/**
 * 接收Minio返回内容
 * @Author Qi
 * @Date 2023-03-14 09:36
 **/
@Data
public class ObjectItem {

    private String objectName;
    private Long size;
}
