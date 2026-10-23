package com.ahaxt.competition.base;

/**
 * 自定义运行时异常
 * @author hongzhangming
 */
public class BaseException extends RuntimeException{
    private BaseResponse baseResponse;

    public BaseException(BaseResponse baseResponse) {
        this.baseResponse = baseResponse;
    }

    public BaseResponse getBaseResponse() {
        return baseResponse;
    }
    public void setBaseResponse(BaseResponse baseResponse) {
        this.baseResponse = baseResponse;
    }
}
