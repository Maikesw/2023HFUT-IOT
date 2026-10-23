package com.ahaxt.competition.service;

import com.alibaba.fastjson.JSONObject;
import org.springframework.data.domain.Sort;
import org.springframework.stereotype.Service;
import org.springframework.web.multipart.MultipartFile;

import java.util.Date;
import java.util.Map;

/**
 * @author hongzhangming
 */
@Service
public interface IUserService {
    /**
     * 用户注册
     */
    //Object register(String phone, String account, String password);
    Object register(JSONObject json);


    /**
     * 登录
     * @param account
     * @param password
     * @param rememberMe
     */
    void login(String account, String password, Boolean rememberMe);

    /**
     * 修改密码
     * @param password
     */
    void editPassword(String userId, String password);


    /**
     * 查询所有用户
     * @return
     */
    Object userList();

    /**
     * 删除用户
     * @param phone
     */
    void deleteUser(String phone);


    /**
     * 配置用户角色
     * @param roleSet
     * @param userId
     * @return
     */
    Object userRoleSet(Integer[] roleSet, Integer userId);

    /**
     * 获取所有用户信息
     * @param pageInfo
     * @param sort
     * @return
     */
    Object getUserListIsNotDelete(JSONObject pageInfo, JSONObject sort);

    /**编辑用户信息
     *
     * @param tblUserInfo
     * @return
     */
    Object editUserInfo(JSONObject tblUserInfo, Integer userId);

    /**
     *@Des 用户获取List
     *@Author yukai
     *@Date 2020/12/10 16:14
     */
    Object tblUserGetList(String tblName, JSONObject searchKeys, Map<String, String> repMap, Sort sort, Integer page, Integer size);
    Object ViewUserGetList(String tblName, JSONObject searchKeys, Map<String, String> repMap, Sort sort, Integer page, Integer size);

    /**
     *  用户上传头像
     * @param file
     * @return
     */
    Object uploadAvatar( MultipartFile file);


    Object getUserRoles(String userId);
    Object saveUserRoles(String userId, Integer[] roleIds);
    Object getDepartment(String parentId);
}
