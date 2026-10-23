package com.ahaxt.competition.config;

import org.springframework.beans.factory.annotation.Value;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import springfox.documentation.builders.ApiInfoBuilder;
import springfox.documentation.builders.PathSelectors;
import springfox.documentation.builders.RequestHandlerSelectors;
import springfox.documentation.spi.DocumentationType;
import springfox.documentation.spring.web.plugins.Docket;
import springfox.documentation.swagger2.annotations.EnableSwagger2;

/**
 * @author hongzhangming
 */
@Configuration
@EnableSwagger2
public class SwaggerConfig {
    private static Boolean enable;
    private static String basePackage;
    private static String title;
    private static String version;
    private static String description;

    @Value(value = "${swagger.enable}")
    public void setUrl(Boolean swagger_enable) {
        enable = swagger_enable;
    }

    @Value(value = "${swagger.basePackage}")
    public void setBasePackage(String swagger_basePackage) {
        basePackage = swagger_basePackage;
    }

    @Value(value = "${swagger.title}")
    public void setTitle(String swagger_title) {
        SwaggerConfig.title = swagger_title;
    }

    @Value(value = "${swagger.version}")
    public void setVersion(String swagger_version) {
        SwaggerConfig.version = swagger_version;
    }

    @Value(value = "${swagger.description}")
    public void setDescription(String swagger_description) {
        SwaggerConfig.description = swagger_description;
    }

    @Bean
    public Docket docKet() {
        return new Docket(DocumentationType.SWAGGER_2)
                .enable(enable)
                .apiInfo(
                        new ApiInfoBuilder()
                                .title(title)
                                .version(version)
                                .description(description)
                                .build()
                ).select()
                .apis(RequestHandlerSelectors.basePackage(basePackage)).paths(PathSelectors.any())
                .build()
                ;
    }
}
