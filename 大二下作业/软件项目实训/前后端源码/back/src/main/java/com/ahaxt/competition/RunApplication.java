package com.ahaxt.competition;


import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import org.springframework.cache.annotation.EnableCaching;
import org.springframework.data.jpa.repository.config.EnableJpaAuditing;
import org.springframework.data.jpa.repository.config.EnableJpaRepositories;
import org.springframework.scheduling.annotation.EnableScheduling;

/**
 * @author hongzhangming
 */
@SpringBootApplication
@EnableJpaAuditing
@EnableScheduling
@EnableCaching
public class RunApplication {

    private static String swaggerUrl;

    @Value(value = "${swagger.url}")
    public void setSwagger_url(String swagger_url) {
        swaggerUrl = swagger_url;
    }

    public static void main(String[] args) {
        SpringApplication.run(RunApplication.class, args);
        LoggerFactory.getLogger(RunApplication.class).info("<INFO> 启动成功 {}",swaggerUrl);
    }
}
