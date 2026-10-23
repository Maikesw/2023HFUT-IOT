package com.ahaxt.competition.config;

import org.springframework.beans.factory.annotation.Value;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.scheduling.annotation.EnableAsync;
import org.springframework.scheduling.concurrent.ThreadPoolTaskExecutor;

import java.util.concurrent.ThreadPoolExecutor;

@Configuration
@EnableAsync
public class ThreadConfig {
    @Value("${thread.config.corePoolSize}")
    private String corePoolSize;
    @Value("${thread.config.maxPoolSize}")
    private String maxPoolSize;

    @Bean
    public ThreadPoolTaskExecutor asyncThread() {
        ThreadPoolTaskExecutor threadPool = new ThreadPoolTaskExecutor();
        //核心线程数
        threadPool.setCorePoolSize(Integer.valueOf(corePoolSize));
        //最大线程数
        threadPool.setMaxPoolSize(Integer.valueOf(maxPoolSize));
        //设置队列大小
        threadPool.setQueueCapacity(99999);
        //线程池中线程名称前缀
        threadPool.setThreadNamePrefix("async-base-");
        //设置等待任务在关机时完成
        threadPool.setWaitForTasksToCompleteOnShutdown(true);
        // rejection-policy：当pool已经达到max size的时候，如何处理新任务
        // CALLER_RUNS：不在新线程中执行任务，而是有调用者所在的线程来执行
        threadPool.setRejectedExecutionHandler(new ThreadPoolExecutor.CallerRunsPolicy());
        //执行初始化
        threadPool.initialize();
        return threadPool;
    }
}
