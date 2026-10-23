package com.ahaxt.competition.config;

import lombok.Data;
import org.springframework.boot.context.properties.ConfigurationProperties;
import org.springframework.stereotype.Component;

/**
 * MinIO配置类
 * @Author Qi
 * @Date 2023-03-14 08:26
 **/
@Data
@Component
@ConfigurationProperties("minio")
public class MinioProperties {

    private String endpoint;

    private String userName;

    private String password;

    private String bucketName;

}
