package com.ahaxt.competition.service;

import com.alibaba.fastjson.JSONArray;
import com.alibaba.fastjson.JSONObject;
import org.springframework.data.domain.Sort;
import org.springframework.stereotype.Service;

import java.util.ArrayList;
import java.util.Map;

/**
 *
 */
@Service
public interface IDataTreeService {
    /**
     * 查询
     * @return
     */
    Object getTreeList(String tblName, Boolean virtualRootFlag, JSONObject searchKey, Boolean lazy, String preName, Integer parentId, Sort sortJson);
    Object getTreeArrayList(String tblName, JSONObject searchKey);
    Object changeTwoNodes(String tblName, int nodeId, int nodeChangeId);
    Object editOneNode(String tblName, JSONObject node);
    Object delOneNode(String tblName, JSONObject node);
    Object delManyNodes(String tblName, JSONArray nodes);
    Object getAllParentIndex(String tblName, Integer node);
    Object getNearestParent(String tblName, Integer nodeId);
    Object getFirstParent(String tblName, Integer nodeId, Integer theLevel);
    Object getAllBrotherIndex(String tblName, Integer nodeId);
    ArrayList<JSONObject> getAllChildren(JSONObject treeInfo);

    /**
     * 树结构关联查询列表结构
     *
     * @param treeInfo   树结构表名
     * @param listTblName  列表结构表名
     * @param searchKey  列表的搜索条件
     * @return 返回所有属于treeNodeId及其子节点的列表元素
     * @example 查询部门员工，此接口入参(BaseDepartment,BaseUser,部门Id)
     * 返回属于部门Id及其下属部门的所有员工。
     */
    Object getTreeJointList(JSONArray treeInfo, String listTblName, JSONObject searchKey, Map<String, String> regMap, Sort sort, Integer page, Integer size);
}
