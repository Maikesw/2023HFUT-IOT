package com.ahaxt.competition.controller.user.ctrl;

import com.ahaxt.competition.annotation.PathRestController;
import com.ahaxt.competition.annotation.Permissions;
import com.ahaxt.competition.base.BaseResponse;
import com.ahaxt.competition.controller.common.ctrl.CommonController;
import com.alibaba.fastjson.JSONObject;
import io.swagger.annotations.Api;
import io.swagger.annotations.ApiOperation;
import org.apache.commons.beanutils.ConvertUtils;
import org.apache.shiro.SecurityUtils;
import org.apache.shiro.authz.annotation.RequiresUser;
import org.springframework.http.MediaType;
import org.springframework.web.bind.annotation.*;
import org.springframework.web.multipart.MultipartFile;


/**
 * @author hongzhangming
 */
@Api(tags = "登录模块")
@PathRestController(value = "sign")
public class SignController extends CommonController {

    //默认账号：15505096851， 密码：Axt-1234
    @ApiOperation(value = "3 登录",notes = "{\"account\": \"18855059407\",\n" +
            "\"password\": \"123456\",\n" +
            "\"rememberMe\": true\n" +
            "}")
    @PostMapping(value = "/login", consumes = MediaType.APPLICATION_JSON_VALUE)
    public Object login(@RequestBody JSONObject requestJson) {
        String account = requestJson.getString("account");
        String password = requestJson.getString("password");
        Boolean rememberMe = requestJson.getBoolean("rememberMe");
        iUserService.login(account, password, rememberMe);
        return BaseResponse.ok("login success");
    }

    @ApiOperation(value = "5 登出")
    @PostMapping(value = "/logout")
    public Object logout() {
        if (SecurityUtils.getSubject().getPrincipal() != null) {
            SecurityUtils.getSubject().logout();
        }
        return BaseResponse.ok;
    }

    @ApiOperation(value = "6 查询登录用户信息,角色,菜单 权限", consumes = MediaType.APPLICATION_JSON_VALUE)
    @GetMapping(value = "/info")
    public Object info() {
        return BaseResponse.ok(super.getLoginUser());
    }


    @ApiOperation("8 编辑用户角色")
    @RequiresUser
    @Permissions(value = "user", c = false, r = true, u = true, d = false, orAndNon = OR)
    @PutMapping("/userRoleSet/{userId}/{roleSet}")
    public Object userRoleSet(@PathVariable Integer[] roleSet, @PathVariable Integer userId) {
        return BaseResponse.ok(iUserService.userRoleSet(roleSet, userId));
    }

    @ApiOperation("9 获取所有用户信息及其权限信息")
    @PostMapping(value = "/getUserListIsNotDelete", consumes = MediaType.APPLICATION_JSON_VALUE)
    public Object getUserListIsNotDelete(@RequestBody JSONObject requestJson) {
        JSONObject pageInfo = requestJson.getJSONObject("pageInfo");
        JSONObject sort = requestJson.getJSONObject("sort");
        return BaseResponse.ok(iUserService.getUserListIsNotDelete(pageInfo, sort));
    }

    @ApiOperation("10 编辑用户信息")
    @PostMapping(value = "/editUserInfo/{userId}", consumes = MediaType.APPLICATION_JSON_VALUE)
    public Object editUserInfo(@RequestBody JSONObject requestJson, @PathVariable Integer userId) {
        return BaseResponse.ok(iUserService.editUserInfo(requestJson, userId));
    }

    @ApiOperation(value = "13 忘记密码-修改密码")
    @PostMapping(value = "editPassword", consumes = MediaType.APPLICATION_JSON_VALUE)
    public Object editPassword(@RequestBody JSONObject requestJson) {
        String userId = requestJson.getString("userId");
        String password = requestJson.getString("password");
        iUserService.editPassword(userId, password);
        return BaseResponse.ok;
    }
    @ApiOperation("14 上传新头像并删除上个头像")
    @PostMapping(value = "/oss/uploadAvatar",consumes = MediaType.MULTIPART_FORM_DATA_VALUE)
    public Object uploadAvatar(@RequestParam MultipartFile file ){
        return BaseResponse.ok(iUserService.uploadAvatar(file));
    }

    @ApiOperation(value = "获得用户权限")
    @PostMapping(value = "/getUserRoles", consumes = MediaType.APPLICATION_JSON_VALUE)
    public Object getUserRoles(@RequestBody JSONObject requestJson) {
        String userId = requestJson.getString("userId");
        return BaseResponse.ok(iUserService.getUserRoles(userId));
    }
    @ApiOperation(value = "保存用户权限")
    @PostMapping(value = "/saveUserRoles", consumes = MediaType.APPLICATION_JSON_VALUE)
    public Object saveUserRoles(@RequestBody JSONObject requestJson) {
        String userId = requestJson.getString("userId");
        String strRoleIds = requestJson.getString("roleIds");
        Integer[] roleIds = (Integer[]) ConvertUtils.convert(strRoleIds.substring(1, strRoleIds.length() - 1).split(SPLIT_OPERATOR.COMMA), Integer.class);
        return BaseResponse.ok(iUserService.saveUserRoles(userId, roleIds));
    }

    @ApiOperation(value = "获取单位信息")
    @PostMapping(value = "/getDepartment", consumes = MediaType.APPLICATION_JSON_VALUE)
    public Object getDepartment(@RequestBody JSONObject requestJson) {
        String parentId = requestJson.getString("parentId");
        return BaseResponse.ok(iUserService.getDepartment(parentId));
    }
}
