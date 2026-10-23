package com.ahaxt.competition.service;


import com.alibaba.fastjson.JSONObject;
import org.springframework.data.domain.Sort;
import org.springframework.stereotype.Service;

import java.util.List;
import java.util.Map;

@Service
public interface IDataListService {

    /**
    *@Des List结构相应接口
    *@Author cherish
    *@Date 2020/10/27 14:14
    */
    Object getSomeRecords(String tblName, JSONObject searchKeys, Map<String,String> reg, Sort sort, Integer page, Integer size);
    Object changeTwoNodes(String tblName, int nodeId, int nodeChangeId);
    Object editOneNode(String tblName, JSONObject node);
    Object deleteSomeNodes(String tblName, List<Integer> ids);
    Object changeNodeOrder(String tblName, int nodeId, boolean up, JSONObject searchKeys, Map<String,String> regMap);
}
