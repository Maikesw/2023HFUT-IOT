package com.ahaxt.competition.config;

import org.springframework.context.annotation.Configuration;
import org.springframework.messaging.simp.config.MessageBrokerRegistry;
import org.springframework.scheduling.concurrent.ThreadPoolTaskScheduler;
import org.springframework.web.socket.config.annotation.EnableWebSocketMessageBroker;
import org.springframework.web.socket.config.annotation.StompEndpointRegistry;
import org.springframework.web.socket.config.annotation.WebSocketMessageBrokerConfigurer;

/**
 * @author hongzhangming
 */
@Configuration
@EnableWebSocketMessageBroker
public class SocketConfig implements WebSocketMessageBrokerConfigurer {
    @Override
    public void configureMessageBroker(MessageBrokerRegistry registry){
        registry.setApplicationDestinationPrefixes("/socket");
        ThreadPoolTaskScheduler threadPool = new ThreadPoolTaskScheduler();
        threadPool.setPoolSize(100);
        threadPool.setThreadNamePrefix("wss-heartbeat-thread-");
        threadPool.initialize();
        registry.enableSimpleBroker("/topic","/user")
                .setHeartbeatValue(new long[]{10000,10000})
                .setTaskScheduler(threadPool);
        registry.setUserDestinationPrefix("/user");
    }

    @Override
    public void registerStompEndpoints(StompEndpointRegistry registry){
        //前端连接时后缀
        registry.addEndpoint("/sockJs").setAllowedOrigins("*").withSockJS();
    }
}