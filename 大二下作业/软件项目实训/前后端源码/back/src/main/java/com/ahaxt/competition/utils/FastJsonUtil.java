package com.ahaxt.competition.utils;

import com.alibaba.fastjson.JSONObject;
import com.alibaba.fastjson.serializer.ContextValueFilter;
import com.alibaba.fastjson.serializer.SerializerFeature;
import com.alibaba.fastjson.support.config.FastJsonConfig;
import com.alibaba.fastjson.support.spring.FastJsonHttpMessageConverter;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.context.annotation.Bean;
import org.springframework.stereotype.Component;

import java.nio.charset.StandardCharsets;
import java.text.SimpleDateFormat;
import java.util.Date;
import java.util.List;

/**
 * Json Util {@link com.alibaba.fastjson}
 * @author hongzhangming
 */
@Component
public class FastJsonUtil extends JSONObject {
    private static String pattern;

    public String getPattern() {
        return pattern;
    }

    @Value("${spring.jackson.date-format}")
    public void setPattern(String pattern) {
        FastJsonUtil.pattern = pattern;
    }

    public static String toString(Object params) {
        ContextValueFilter valueFilter = (beanContext, object, key, value) -> {
            if(value instanceof Date){
                return new SimpleDateFormat(pattern).format(value);
            }
            return value;
        };
        return toJSONString(params,valueFilter);
//        return toJSONStringWithDateFormat(params);
    }
    public static String toJSONStringWithDateFormat(Object params) {
        return toJSONStringWithDateFormat(params, pattern);
    }

    public static String toJSONStringWithDateFormat(Object params,String pattern) {
        return toJSONStringWithDateFormat(params, pattern);
    }

    public static JSONObject toJson(String params) {
        return parseObject(params);
    }

    public static JSONObject toJson(Object params) {
        return parseObject(toString(params));
    }

    public static <T extends Object> T toObject(String jsonString, Class<T> clazz) {
        return parseObject(jsonString, clazz);
    }

    public static <T extends Object> T toObject(Object params, Class<T> clazz) {
        return parseObject(toString(params), clazz);
    }

    public static <T extends Object> List<T> toArray(Object params, Class<T> clazz) {
        return parseArray(toString(params), clazz);
    }

    @Bean
    public FastJsonHttpMessageConverter converters() {
        FastJsonConfig fastJsonConfig = new FastJsonConfig();
        fastJsonConfig.setDateFormat(pattern);
        fastJsonConfig.setCharset(StandardCharsets.UTF_8);
        fastJsonConfig.setSerializerFeatures(
                SerializerFeature.DisableCircularReferenceDetect,
                SerializerFeature.WriteNullListAsEmpty,
                SerializerFeature.WriteNullStringAsEmpty,
                SerializerFeature.WriteMapNullValue,
//                SerializerFeature.WriteNullNumberAsZero,
                SerializerFeature.WriteNullBooleanAsFalse,
                SerializerFeature.WriteEnumUsingToString,
                SerializerFeature.PrettyFormat
        );
        FastJsonHttpMessageConverter fastConverter = new FastJsonHttpMessageConverter();
        fastConverter.setFastJsonConfig(fastJsonConfig);
        return fastConverter;
    }

}
