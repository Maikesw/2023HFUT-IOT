package com.ahaxt.competition.aop;

import com.ahaxt.competition.utils.FastJsonUtil;
import com.ahaxt.competition.utils.LogUtil;
import com.alibaba.fastjson.JSONObject;
import org.aspectj.lang.JoinPoint;
import org.aspectj.lang.annotation.Aspect;
import org.aspectj.lang.annotation.Pointcut;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.core.NamedThreadLocal;
import org.springframework.stereotype.Component;
import org.springframework.web.context.request.RequestContextHolder;
import org.springframework.web.context.request.ServletRequestAttributes;
import org.springframework.web.multipart.MultipartFile;

import javax.servlet.http.HttpServletRequest;
import java.util.Arrays;
import java.util.Set;
import java.util.stream.Collectors;

/**
 * @author HongZhangming
 */
@Component
@Aspect
public class LogAspect {
    private Logger logger = LoggerFactory.getLogger(this.getClass());
    private Set<String> ignoreUrl = Arrays.stream(new String[]{
            "不打日志的uri"
    }).collect(Collectors.toSet());
    private static final ThreadLocal<Long> startTimeThreadLocal = new NamedThreadLocal<>("ThreadLocal ");

    @Pointcut("execution(* com.ahaxt.competition.controller.*.*(..)) || execution(* com.ahaxt.competition.controller.*.*.*(..)) || execution(* com.ahaxt.competition.controller.*.*.*.*(..)) || execution(* com.ahaxt.competition.controller.*.*.*.*.*(..)) || execution(* com.ahaxt.competition.controller.*.*.*.*.*.*(..)) || execution(* com.ahaxt.competition.controller.*.*.*.*.*.*.*(..))")
    public void pointcutControl() {
    }

    //@Before(value = " pointcutControl()")
    public void before(JoinPoint joinPoint) {
        ServletRequestAttributes attributes = (ServletRequestAttributes) RequestContextHolder.getRequestAttributes();
        HttpServletRequest request = attributes.getRequest();
        if (!ignoreUrl.contains(request.getRequestURI())) {
            try {
                //开始时间 线程绑定变量（该数据只有当前请求的线程可见）
                long beginTime = System.currentTimeMillis();
                startTimeThreadLocal.set(beginTime);
                org.slf4j.LoggerFactory.getLogger(joinPoint.getTarget().getClass()).info(
                        "<REQ - {}> [{} {} {}]\t['{}' - {}()]\tPARAMS:{}"
                        , beginTime
                        , request.getServerName()
                        , request.getMethod()
                        , request.getContentType()
                        , request.getRequestURI()
                        , joinPoint.getSignature().getDeclaringTypeName() + "." + joinPoint.getSignature().getName()
                        , joinPoint.getArgs() == null || joinPoint.getArgs().length == 0
                                ? "null"
                                : (joinPoint.getArgs().length == 1 && joinPoint.getArgs()[0] instanceof MultipartFile
                                ? "{\"MultipartFile\":" + ((MultipartFile) joinPoint.getArgs()[0]).getName() + "}"
                                : FastJsonUtil.toString(joinPoint.getArgs())
                        )
                );
            } catch (Exception e) {
                LogUtil.error(LoggerFactory.getLogger(getClass()), e);
            }
        }
    }

    //@AfterReturning(pointcut = "pointcutControl()", returning = "response")
    public void AfterReturning(Object response) {
        JSONObject res = FastJsonUtil.toJson(response);
        res.put("data", "...");
        ServletRequestAttributes attributes = (ServletRequestAttributes) RequestContextHolder.getRequestAttributes();
        HttpServletRequest request = attributes.getRequest();
        if (!ignoreUrl.contains(request.getRequestURI())) {
            long beginTime = startTimeThreadLocal.get();
            logger.info(
                    "<RES - {}> [耗时：{}ms] [内存({}m)：{}m/{}m]\tRESPONSE：{}",
                    beginTime,
                    System.currentTimeMillis() - beginTime,
                    Runtime.getRuntime().maxMemory() / 1024 / 1024,
                    (Runtime.getRuntime().totalMemory() - Runtime.getRuntime().freeMemory()) / 1024 / 1024,
                    Runtime.getRuntime().totalMemory() / 1024 / 1024,
                    res
            );
        }
        //移除线程变量中的数据，防止内存泄漏
        startTimeThreadLocal.remove();
    }
}
