package com.ahaxt.competition.config;

import lombok.Data;
import org.springframework.boot.context.properties.ConfigurationProperties;
import org.springframework.stereotype.Component;

/**
 * @Author Qi @Date 2023-08-17 15:08
 */
@Data
@Component
@ConfigurationProperties("oss")
public class OssConfig {

    private String endpoint;

    private String accessId;

    private String accessKey;

    private String bucketName;
}
