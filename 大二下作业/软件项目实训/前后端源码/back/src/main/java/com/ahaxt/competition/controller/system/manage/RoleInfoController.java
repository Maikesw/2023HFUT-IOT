package com.ahaxt.competition.controller.system.manage;

import com.ahaxt.competition.annotation.PathRestController;
import com.ahaxt.competition.base.BaseResponse;
import com.ahaxt.competition.controller.common.ctrl.CommonController;
import com.ahaxt.competition.service.IRoleService;
import com.alibaba.fastjson.JSONObject;
import io.swagger.annotations.Api;
import io.swagger.annotations.ApiOperation;
import org.springframework.http.MediaType;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;

import javax.annotation.Resource;

/**
 * @author hongzhangming
 */
@Api(tags = "角色管理")
@PathRestController("role")
public class RoleInfoController extends CommonController {
    @Resource
    private IRoleService iRoleService;

    @ApiOperation(value = "3 编辑角色权限")
    @PostMapping(value = "/editRolePermissions", consumes = MediaType.APPLICATION_JSON_VALUE)
    public Object editRolePermission(@RequestBody JSONObject requestJson) {
        String roleId = requestJson.getString("roleId");
        JSONObject permissions = requestJson.getJSONObject("permissions");
        return BaseResponse.ok(iRoleService.editPermission(roleId, permissions));
    }

    @ApiOperation(value = "3 查询角色权限")
    @PostMapping(value = "/getRolePermissions", consumes = MediaType.APPLICATION_JSON_VALUE)
    public Object getRolePermissions(@RequestBody JSONObject requestJson) {
        String roleIds = requestJson.getString("roleId");
        return BaseResponse.ok(iRoleService.getRolePermissions(roleIds));
    }

}
