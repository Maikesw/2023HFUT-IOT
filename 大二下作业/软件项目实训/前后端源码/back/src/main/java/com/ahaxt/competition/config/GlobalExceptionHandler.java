package com.ahaxt.competition.config;

import com.ahaxt.competition.aop.LogAspect;
import com.ahaxt.competition.base.BaseException;
import com.ahaxt.competition.base.BaseResponse;
import com.ahaxt.competition.utils.LogUtil;
import org.apache.shiro.SecurityUtils;
import org.apache.shiro.authz.UnauthenticatedException;
import org.apache.shiro.authz.UnauthorizedException;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.web.bind.annotation.ExceptionHandler;
import org.springframework.web.bind.annotation.RestControllerAdvice;

import javax.annotation.Resource;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;

/**
 * 全局异常处理
 * @author hongzhangming
 */
@RestControllerAdvice
public class GlobalExceptionHandler {
    private Logger logger = LoggerFactory.getLogger(getClass());
    @Resource
    private LogAspect logAspect;

    @ExceptionHandler(Exception.class)
    public BaseResponse errorMsg(Exception e, HttpServletResponse response, HttpServletRequest request) {
        BaseResponse br;
        if (e instanceof BaseException) {
            br = ((BaseException) e).getBaseResponse();
        } else if (e instanceof UnauthenticatedException) {
            br = BaseResponse.unAuthorization;
            br.setData(e.getMessage());
            SecurityUtils.getSubject().logout();
        } else if (e instanceof UnauthorizedException) {
            br = BaseResponse.lackPermissions;
            br.setData(e.getMessage());
        } else {
            br = BaseResponse.notCaptured;
            br.setData(e.getMessage());
        }
        response.setStatus(br.getHttpStatus());
        try {
            // remove ThreadLocal ，prevent memory overflow
            logAspect.AfterReturning(br);
        } catch (Exception ee) {
            long flag = System.currentTimeMillis();
            logger.error(
                    "<REQ - {}> [{} {} {}]\t['{}']\tPARAMS:{}"
                    , flag
                    , request.getServerName()
                    , request.getMethod()
                    , request.getContentType()
                    , request.getRequestURI()
                    , request.getQueryString()
            );
            logger.error(
                    "<RES - {}> [内存({}m)：{}m/{}m]\tRESPONSE：{}",
                    flag,
                    Runtime.getRuntime().maxMemory() / 1024 / 1024,
                    (Runtime.getRuntime().totalMemory() - Runtime.getRuntime().freeMemory()) / 1024 / 1024,
                    Runtime.getRuntime().totalMemory() / 1024 / 1024,
                    br.toString()
            );
        }
        LogUtil.error(logger, e);
        return br;
    }
}
