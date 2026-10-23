package com.ahaxt.competition.aop;

import com.ahaxt.competition.annotation.Permissions;
import com.ahaxt.competition.base.Base;
import com.ahaxt.competition.base.BaseResponse;
import com.ahaxt.competition.base.Constant;
import com.ahaxt.competition.utils.FastJsonUtil;
import com.alibaba.fastjson.JSONArray;
import org.aspectj.lang.annotation.Aspect;
import org.aspectj.lang.annotation.Before;
import org.springframework.stereotype.Component;

import javax.annotation.Resource;


/**
 * @author hongzhangming
 */
@Component
@Aspect
public class PermissionAspect {
    @Resource
    private Base base;

    @SuppressWarnings("AlibabaAvoidComplexCondition")
    @Before(value = "@annotation(permissions)", argNames = "permissions")
    public void Before(Permissions permissions) {
        JSONArray permissionSet = FastJsonUtil.toJson(base.getLoginUser()).getJSONArray("permissions");
        if ((Constant.OR.equals(permissions.orAndNon())) || (Constant.AND.equals(permissions.orAndNon()))) {
            boolean permissionSetC = permissionSet.contains(permissions.value() + Constant.CREATE);
            boolean permissionSetR = permissionSet.contains(permissions.value() + Constant.RETRIEVE);
            boolean permissionSetU = permissionSet.contains(permissions.value() + Constant.UPDATE);
            boolean permissionSetD = permissionSet.contains(permissions.value() + Constant.DELETE);
            StringBuffer data = new StringBuffer();
            if (permissions.c() && !permissionSetC) {
                data.append(permissions.value()).append(Constant.CREATE).append(permissions.orAndNon());
            }
            if (permissions.c() && !permissionSetR) {
                data.append(permissions.value()).append(Constant.RETRIEVE).append(permissions.orAndNon());
            }
            if (permissions.c() && !permissionSetU) {
                data.append(permissions.value()).append(Constant.UPDATE).append(permissions.orAndNon());
            }
            if (permissions.c() && !permissionSetD) {
                data.append(permissions.value()).append(Constant.DELETE).append(permissions.orAndNon());
            }
            BaseResponse.lackPermissions.setData(data.substring(0,data.length()-2));
        }
        else {
            BaseResponse.lackPermissions.setData(null);
        }
        throw BaseResponse.lackPermissions.error();
//        if (Constant.OR.equals(permissions.orAndNon())) {
//            if (permissions.c() && permissionSet.contains(permissions.value() + Constant.CREATE)) {
//                return;
//            }
//            if (permissions.r() && permissionSet.contains(permissions.value() + Constant.RETRIEVE)) {
//                return;
//            }
//            if (permissions.u() && permissionSet.contains(permissions.value() + Constant.UPDATE)) {
//                return;
//            }
//            if (permissions.d() && permissionSet.contains(permissions.value() + Constant.DELETE)) {
//                return;
//            }
//            StringBuffer data = new StringBuffer();
//            if (permissions.c() && !permissionSet.contains(permissions.value() + Constant.CREATE)) {
//                data.append(permissions.value()).append(Constant.CREATE).append(Constant.OR);
//            }
//            if (permissions.r() && !permissionSet.contains(permissions.value() + Constant.RETRIEVE)) {
//                data.append(permissions.value()).append(Constant.RETRIEVE).append(Constant.OR);
//            }
//            if (permissions.u() && !permissionSet.contains(permissions.value() + Constant.UPDATE)) {
//                data.append(permissions.value()).append(Constant.UPDATE).append(Constant.OR);
//            }
//            if (permissions.d() && !permissionSet.contains(permissions.value() + Constant.DELETE)) {
//                data.append(permissions.value()).append(Constant.DELETE).append(Constant.OR);
//            }
//            BaseResponse.lackPermissions.setData(data.substring(0,data.length()-2));
//        } else if (Constant.AND.equals(permissions.orAndNon())) {
//            if (permissions.c() && !permissionSet.contains(permissions.value() + Constant.CREATE)) {
//                flag = false;
//            }
//            if (permissions.r() && !permissionSet.contains(permissions.value() + Constant.RETRIEVE)) {
//                flag = false;
//            }
//            if (permissions.u() && !permissionSet.contains(permissions.value() + Constant.UPDATE)) {
//                flag = false;
//            }
//            if (permissions.d() && !permissionSet.contains(permissions.value() + Constant.DELETE)) {
//                flag = false;
//            }
//            if (flag) {
//                return;
//            }
//            StringBuffer data = new StringBuffer();
//            if (permissions.c() && !permissionSet.contains(permissions.value() + Constant.CREATE)) {
//                data.append(permissions.value()).append(Constant.CREATE).append(Constant.AND);
//            }
//            if (permissions.r() && !permissionSet.contains(permissions.value() + Constant.RETRIEVE)) {
//                data.append(permissions.value()).append(Constant.RETRIEVE).append(Constant.AND);
//            }
//            if (permissions.u() && !permissionSet.contains(permissions.value() + Constant.UPDATE)) {
//                data.append(permissions.value()).append(Constant.UPDATE).append(Constant.AND);
//            }
//            if (permissions.d() && !permissionSet.contains(permissions.value() + Constant.DELETE)) {
//                data.append(permissions.value()).append(Constant.DELETE).append(Constant.AND);
//            }
//            BaseResponse.lackPermissions.setData(data.substring(0,data.length()-2));
//        } else {
//            BaseResponse.lackPermissions.setData(null);
//        }
//        throw BaseResponse.lackPermissions.error();
    }
}
