package com.ahaxt.competition.config;

import org.springframework.cache.CacheManager;
import org.springframework.cache.annotation.CachingConfigurerSupport;
import org.springframework.cache.annotation.EnableCaching;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.data.redis.cache.RedisCacheConfiguration;
import org.springframework.data.redis.cache.RedisCacheManager;
import org.springframework.data.redis.connection.RedisConnectionFactory;
import org.springframework.data.redis.core.RedisTemplate;
import org.springframework.data.redis.serializer.GenericJackson2JsonRedisSerializer;
import org.springframework.data.redis.serializer.StringRedisSerializer;

import java.time.Duration;
import java.util.HashMap;
import java.util.HashSet;
import java.util.Map;
import java.util.Set;

/**
 * redis cache config
 * @author hongzhangming
 * @date   2019年3月27日
 */
@Configuration
@EnableCaching
public class RedisConfig extends CachingConfigurerSupport {
    /**
     * 修改序列化方式
     * @param factory
     * @return
     */
    @Bean
    public RedisTemplate<String, Object> redisTemplate(RedisConnectionFactory factory) {
        RedisTemplate<String, Object> redisTemplate = new RedisTemplate<>();
        redisTemplate.setConnectionFactory(factory);
        redisTemplate.setValueSerializer(new GenericJackson2JsonRedisSerializer());
        redisTemplate.setHashValueSerializer(new GenericJackson2JsonRedisSerializer());
        redisTemplate.setKeySerializer(new StringRedisSerializer());
        redisTemplate.setHashKeySerializer(new StringRedisSerializer());
        redisTemplate.afterPropertiesSet();
        return redisTemplate;
    }
    /**
     *  缓存管理器
     */
    @Bean
    public CacheManager cacheManager(RedisConnectionFactory redisConnectionFactory) {
        // 初始化的缓存空间set集合
        Set<String> cacheNames =  new HashSet<>();
        // 每个缓存空间不同的配置
        Map<String, RedisCacheConfiguration> configMap = new HashMap<>();

        Map<String, RedisCacheConfiguration> map = getRedisCacheConfigurationMap();
        for (String key:map.keySet()) {
            cacheNames.add(key);
            configMap.put(key,map.get(key));
        }
        // 使用自定义的缓存配置初始化一个cacheManager
        RedisCacheManager cacheManager = RedisCacheManager.builder(redisConnectionFactory)
                // 注意initialCacheNames withInitialCacheConfigurations调用顺序，一定要先调用该方法设置初始化的缓存名，再初始化相关的配置
                .initialCacheNames(cacheNames)
                .withInitialCacheConfigurations(configMap)
                .build();
        return cacheManager;
    }

    /**
     * 自定义配置缓存配置
     */
    private Map<String, RedisCacheConfiguration> getRedisCacheConfigurationMap() {
        // 初始化一个默认模板config
        RedisCacheConfiguration redisCacheConfiguration = RedisCacheConfiguration.defaultCacheConfig();
        // 设置缓存的默认过期时间，也是使用Duration设置
        redisCacheConfiguration = redisCacheConfiguration.entryTtl(Duration.ofMinutes(1))
                .disableCachingNullValues();     // 不缓存空值

        Map<String, RedisCacheConfiguration> map =new HashMap<>();
        /** 自定义配置 **/
        /*
        1min 不缓存空值
         */
        map.put("redisCache",redisCacheConfiguration);
        /*
        120s 不缓存空值
         */
        map.put("redisCache2", redisCacheConfiguration.entryTtl(Duration.ofSeconds(120)));
        return map;
    }

}