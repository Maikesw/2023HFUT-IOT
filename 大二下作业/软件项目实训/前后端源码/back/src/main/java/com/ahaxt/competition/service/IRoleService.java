package com.ahaxt.competition.service;

import com.alibaba.fastjson.JSONObject;
import org.springframework.stereotype.Service;

/**
 * @author hongzhangming
 */
@Service
public interface IRoleService {


    /**
     * 修改角色或用户权限
     * @param roleId
     * @param permissions
     * @return
     */
    Object editPermission(String roleId, JSONObject permissions);

    /**
     * 获取用户或角色权限
     * @param roleIds
     * @return
     */
    Object getRolePermissions(String roleIds);


}
