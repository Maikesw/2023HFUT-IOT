package com.ahaxt.competition.service.impl;

import com.ahaxt.competition.base.Base;
import com.ahaxt.competition.base.Constant;
import com.ahaxt.competition.entity.db.RelRoleMenu;
import com.ahaxt.competition.entity.db.SysMenu;
import com.ahaxt.competition.service.IRoleService;
import com.ahaxt.competition.utils.FastJsonUtil;
import com.ahaxt.competition.utils.TreeUtil;
import com.alibaba.fastjson.JSONArray;
import com.alibaba.fastjson.JSONObject;
import org.springframework.data.domain.Page;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.util.*;
import java.util.stream.Collectors;

/**
 * @author hongzhangming
 */
@Service
@Transactional(rollbackFor = Exception.class)
public class RoleServiceImpl extends Base implements IRoleService {

    @Override
    public Object editPermission(String roleId, JSONObject permissions) {
        //保存权限，一次只可能是一个role
        //先删除所有权限
        JSONObject jsSearch = new JSONObject();
        jsSearch.put("roleId", roleId);
        List<RelRoleMenu> listRelRoleMenus = ((Page<RelRoleMenu>)iCommonService.getSomeRecords("RelRoleMenu", jsSearch)).getContent();
        List<Integer> roleMenuIds = listRelRoleMenus.stream().map(RelRoleMenu::getId).collect(Collectors.toList());
        iCommonService.deleteSomeRecords("RelRoleMenu", roleMenuIds);
        //由树结构转化为列表，并处理了一些数据
        ArrayList<RelRoleMenu> authorizationList = new ArrayList();
        authorizationList = tranTreeToList(permissions.getJSONArray("children"), authorizationList);
        authorizationList.forEach(sysRole -> sysRole.setRoleId(Integer.parseInt(roleId)));
        //再全部重新保存
        iCommonService.saveSomeRecords("RelRoleMenu", authorizationList);
        return true;
    }

    /**
     * 获取用户或角色权限
     * @param roleIds
     * @return
     */

    @Override
    public Object getRolePermissions(String roleIds) {
        //逻辑：查出authorizationInfoList中对应的menuList。然后根据allMenuList构建完整树结构，有authorizationInfo的节点填入，没有的构造tblAuthorizationInfoDefalut，即值CRUD全为false。
        //对于没有任何权限的角色，也返回完整树结构，每个节点都使用tblAuthorizationInfoDefalut
        JSONObject jsSearch = new JSONObject();
        jsSearch.put("roleId", roleIds);
        Map regMap = new HashMap(1);
        regMap.put("roleId", Constant.IN);
        List<RelRoleMenu> authorizationList = ((Page<RelRoleMenu>)iCommonService.getSomeRecords("RelRoleMenu", jsSearch, regMap)).getContent();

        JSONArray resList = new JSONArray();
        Set<Integer> roleMenuIds =authorizationList.stream().map(RelRoleMenu::getMenuId).collect(Collectors.toSet());
        List<SysMenu> menuList = TreeUtil.sortTree((List<SysMenu>)iCommonService.getRecordsByIds("SysMenu",roleMenuIds));
        Map<Integer, RelRoleMenu> map = new HashMap<>(16);
        RelRoleMenu flag;
        // 下面思路，把每一条权限都加入到map中。遍历时存在同一权限多次出现情况（来自不同角色），则要把细的增删改查权限或后加入
        for (RelRoleMenu relRoleMenu : authorizationList) {
            flag = map.get(relRoleMenu.getMenuId());
            if (flag == null) {
                map.put(relRoleMenu.getMenuId(), relRoleMenu);
            } else {
                flag.setVisibleFlag(flag.getVisibleFlag() || relRoleMenu.getVisibleFlag());
                flag.setAddFlag(flag.getAddFlag() || relRoleMenu.getAddFlag());
                flag.setModifyFlag(flag.getModifyFlag() || relRoleMenu.getModifyFlag());
                flag.setDeleteFlag(flag.getDeleteFlag() || relRoleMenu.getDeleteFlag());
                map.put(relRoleMenu.getMenuId(), flag);
            }
        }
        //虚节点-所有节点的四个权限，当所有子节点都有授权时，才为true
        Boolean rootAddFlag = true;
        Boolean rootDeleteFlag = true;
        Boolean rootModifyFlag = true;
        Boolean rootVisibleFlag = true;
        List<SysMenu> allMenuList = ((Page<SysMenu>)iCommonService.getSomeRecords("SysMenu")).getContent();
        for(SysMenu tblMenuInfo : allMenuList){
            JSONObject jsonObject;
            jsonObject = FastJsonUtil.toJson(tblMenuInfo);
            boolean flagGetAuthorization = true;
            for (SysMenu authorizationMenu : menuList) {
                if(authorizationMenu.getId().equals(tblMenuInfo.getId()) && flagGetAuthorization) {
                    RelRoleMenu tblObject = map.get(authorizationMenu.getId());
                    jsonObject.put("authorizationAllFlag",(tblObject.getAddFlag() && tblObject.getDeleteFlag() && tblObject.getModifyFlag() && tblObject.getVisibleFlag()));
                    jsonObject.put("authorizationInfo", map.get(authorizationMenu.getId()));
                    flagGetAuthorization = false;
                    //对于虚节点root判断，只要有一个节点不为true,就赋值false
                    rootAddFlag = rootAddFlag && tblObject.getAddFlag();
                    rootDeleteFlag = rootDeleteFlag && tblObject.getDeleteFlag();
                    rootModifyFlag = rootModifyFlag && tblObject.getModifyFlag();
                    rootVisibleFlag = rootVisibleFlag && tblObject.getVisibleFlag();
                }
            }
            if (flagGetAuthorization) {
                RelRoleMenu tblAuthorizationInfoDefalut = new RelRoleMenu();
                tblAuthorizationInfoDefalut.setAddFlag(false);
                tblAuthorizationInfoDefalut.setDeleteFlag(false);
                tblAuthorizationInfoDefalut.setModifyFlag(false);
                tblAuthorizationInfoDefalut.setVisibleFlag(false);
                tblAuthorizationInfoDefalut.setMenuId(tblMenuInfo.getId());
                jsonObject.put("authorizationInfo", tblAuthorizationInfoDefalut);
                jsonObject.put("authorizationAllFlag",false);
                //对于虚节点root,只要进入此分支，全都置false
                rootAddFlag = false;
                rootDeleteFlag = false;
                rootModifyFlag = false;
                rootVisibleFlag = false;
            }
            resList.add(jsonObject);
        }
        //封装一个虚拟根节点
        JSONObject root = new JSONObject();
        root.put("children", getSubNodes(resList,-1));
        root.put("id", -1);
        root.put("name", "全部");

        RelRoleMenu tblAuthorizationInfoRoot = new RelRoleMenu();
        tblAuthorizationInfoRoot.setAddFlag(rootAddFlag);
        tblAuthorizationInfoRoot.setDeleteFlag(rootDeleteFlag);
        tblAuthorizationInfoRoot.setModifyFlag(rootModifyFlag);
        tblAuthorizationInfoRoot.setVisibleFlag(rootVisibleFlag);
        tblAuthorizationInfoRoot.setMenuId(-1);
        root.put("authorizationInfo",tblAuthorizationInfoRoot);
        ArrayList<Object> tree = new ArrayList<Object>();
        tree.add(root);
        return tree;
    }

    /**
    *代码参考DataTreeServiceImpl中的getSubNodes
    */
    private ArrayList<JSONObject> getSubNodes(JSONArray wholeObj, Integer ParentId) {
        ArrayList<JSONObject> tree = new ArrayList<JSONObject>();
        for (int i = 0; i < wholeObj.size(); i++) {
            JSONObject nowObj = (JSONObject)wholeObj.get(i);
            if (ParentId.equals(nowObj.getInteger("parentId"))) {
                if (nowObj.getBoolean("isLeaf").equals(false)) {
                    nowObj.put("children", getSubNodes(wholeObj, nowObj.getInteger("id")));
                }
                tree.add(nowObj);
            }
        }
        return tree;
    }

    private ArrayList<RelRoleMenu> tranTreeToList(JSONArray objectArray, ArrayList<RelRoleMenu> list) {
        for(int i=0;i<objectArray.size();i++ ){
            JSONObject jsonObject = (JSONObject) objectArray.get(i);
            JSONObject  authorizationInfo = jsonObject.getJSONObject("authorizationInfo");
            //增删改查任意一个有权限，就存起来
            if(authorizationInfo.getBoolean("visibleFlag")||authorizationInfo.getBoolean("modifyFlag")||authorizationInfo.getBoolean("deleteFlag")||authorizationInfo.getBoolean("addFlag")){
                authorizationInfo.put("id", null);
                list.add(JSONObject.toJavaObject(authorizationInfo, RelRoleMenu.class));
            }
            if(jsonObject.get("children") != null) {
                tranTreeToList(jsonObject.getJSONArray("children"), list);
            }
        }
        return list;
    }


}
