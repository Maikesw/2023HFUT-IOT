package com.ahaxt.competition.config;

import com.ahaxt.competition.utils.ReflectUtils;
import io.minio.MinioAsyncClient;
import io.minio.MinioClient;
import lombok.Data;
import org.springframework.boot.autoconfigure.condition.ConditionalOnBean;
import org.springframework.boot.autoconfigure.condition.ConditionalOnMissingBean;
import org.springframework.boot.context.properties.EnableConfigurationProperties;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;

import javax.annotation.Resource;

/**
 * 传统连接MinIO方式
 * @Author Qi
 * @Date 2023-03-14 09:31
 **/
@Data
@Configuration
@EnableConfigurationProperties(MinioProperties.class)
public class MinIOConfig {

    @Resource
    private MinioProperties minioProperties;

    /**
     * 注入minio 客户端
     */
    @Bean
    public MinioClient minioClient() {

        return MinioClient.builder()
                .endpoint(minioProperties.getEndpoint())
                .credentials(minioProperties.getUserName(), minioProperties.getPassword())
                .build();
    }

    @Bean
    @ConditionalOnBean({MinioClient.class})
    @ConditionalOnMissingBean(ParallelMinioClient.class)
    public ParallelMinioClient parallelMinioClient(MinioClient minioClient) {
        MinioAsyncClient asyncClient = ReflectUtils.getFieldValue(minioClient, "asyncClient");
        return new ParallelMinioClient(asyncClient);
    }
}
