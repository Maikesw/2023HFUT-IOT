package com.ahaxt.competition.controller;

import com.alibaba.fastjson.JSONObject;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.messaging.handler.annotation.MessageMapping;
import org.springframework.messaging.handler.annotation.Payload;
import org.springframework.messaging.handler.annotation.SendTo;
import org.springframework.messaging.simp.SimpMessagingTemplate;
import org.springframework.stereotype.Controller;

import javax.annotation.Resource;

/**
 * @author hongzhangming
 */
@Controller
public class SocketController {
    Logger logger = LoggerFactory.getLogger(this.getClass());
    @Resource
    private SimpMessagingTemplate simpMessagingTemplate;

    /**
     * subscribe ：/socket/one
     */
    @MessageMapping("/one")
    public void test1(@Payload String msg) {
        logger.info("<INFO - socket> {} -- {} --> {}", "/socket/one", msg, "/topic/one/id");

        simpMessagingTemplate.convertAndSend("/topic/one/id", msg);
        logger.info("<INFO - socket> {} -- {} --> {}", "/socket/one", msg, "/user/one/{key}");
        simpMessagingTemplate.convertAndSendToUser("one", "/key", msg);
    }

    /**
     * subscribe ：/socket/two
     */
    @MessageMapping("/two")
    @SendTo("/topic/two")
    public Object test2(@Payload String msg) {
        logger.info("<INFO - socket> {} -- {} --> {}", "/socket/two", msg, "/topic/two");
        JSONObject jsonObject = JSONObject.parseObject(msg);
        if (Double.parseDouble(jsonObject.getString("key")) > 10){
            // iDeviceManage.instructionDeal(3,true);
        }else {
            // iDeviceManage.instructionDeal(3,false);
        }
        return "content:" + msg;
    }


}
