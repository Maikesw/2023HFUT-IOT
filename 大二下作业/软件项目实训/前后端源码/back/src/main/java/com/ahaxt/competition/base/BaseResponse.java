package com.ahaxt.competition.base;

import com.alibaba.fastjson.JSONObject;
import com.alibaba.fastjson.annotation.JSONField;
import com.alibaba.fastjson.annotation.JSONType;
import org.springframework.http.HttpStatus;

/**
 * @author hongzhangming
 */
@JSONType(serializeEnumAsJavaBean = true)
public enum BaseResponse {
    /**
     * 200：HttpStatus.OK
     */
    ok(200, "successful", HttpStatus.OK.value()),
    /**
     * 400：HttpStatus.BAD_REQUEST
     */
    parameterInvalid(1102, "参数无效", HttpStatus.BAD_REQUEST.value()),
    notCaptured(1101, "未捕获的异常", HttpStatus.BAD_REQUEST.value()),
    /**
     * 401：HttpStatus.UNAUTHORIZED
     */
    lackPermissions(1001, "lack of permission", HttpStatus.UNAUTHORIZED.value()),
    unAuthorization(1002, "无登录凭证，请重新登录", HttpStatus.UNAUTHORIZED.value()),
    /**
     * 500：HttpStatus.INTERNAL_SERVER_ERROR
     */
    moreInfoError(1200, "更多错误！", HttpStatus.INTERNAL_SERVER_ERROR.value());
    /**
     * ok返回格式
     */
    public static Object ok(Object data) {
        JSONObject res = new JSONObject();
        res.put("status", ok.status);
        res.put("message", ok.message);
        if(data==null){
            res.put("data","");
        }else {
            res.put("data", data);
        }
        res.put("httpStatus", ok.httpStatus);
        return res;
    }

    /**
     * 抛异常 如 : throw BaseResponse.parameterInvalid.error()
     * @return BaseException
     */
    public BaseException error() {
        return new BaseException(this);
    }
    public BaseException error(String msg) {
        this.message = msg;
        return new BaseException(this);
    }

    /**
     * 异常日志格式
     * @return
     */
    @Override
    public String toString() {
        return "[httpStatus=" + httpStatus + "] " +
                name() + " {" +
                "status=" + status +
                ", message='" + message + '\'' +
                ", data=" + data +
                '}';
    }

    private int status;
    private String message;
    private Object data="";
    @JSONField(serialize = false)
    private int httpStatus;

    BaseResponse(int status, String message, int httpStatus) {
        this.status = status;
        this.message = message;
        this.httpStatus = httpStatus;
    }

    public int getStatus() {
        return status;
    }

    public void setStatus(int status) {
        this.status = status;
    }

    public String getMessage() {
        return message;
    }

    public void setMessage(String message) {
        this.message = message;
    }

    public Object getData() {
        return data;
    }

    public void setData(Object data) {
        this.data = data;
    }

    public int getHttpStatus() {
        return httpStatus;
    }

    public void setHttpStatus(int httpStatus) {
        this.httpStatus = httpStatus;
    }

}
