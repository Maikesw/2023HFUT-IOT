package com.ahaxt.competition.service.impl;

import com.ahaxt.competition.base.Base;
import com.ahaxt.competition.base.BaseResponse;
import com.ahaxt.competition.service.IDataListService;
import com.ahaxt.competition.utils.FastJsonUtil;
import com.ahaxt.competition.utils.LogUtil;
import com.alibaba.fastjson.JSONObject;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Sort;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;
import org.springframework.util.ObjectUtils;
import org.springframework.util.StringUtils;

import java.util.ArrayList;
import java.util.Date;
import java.util.List;
import java.util.Map;

@Service
@Transactional(rollbackFor = Exception.class)
public class DataListServiceImpl extends Base implements IDataListService {

    /**
     * 条件查询/模糊查询
     * searchKey中的字段和reg中的字段对应
     * @param tblName
     * @param searchKeys
     * @param regMap
     * @return
     */
    @Override
    public Object getSomeRecords(String tblName, JSONObject searchKeys, Map<String, String> regMap, Sort sort, Integer page, Integer size) {
        Object rawRet;
        rawRet = iCommonService.getSomeRecords(tblName, searchKeys, regMap, sort, page, size);
        return rawRet;
    }

    @Override
    public Object changeTwoNodes(String tblName, int nodeId, int nodeChangeId) {
        JSONObject obj1 = FastJsonUtil.toJson(iCommonService.getOneRecordById(tblName, nodeId));
        JSONObject obj2 = FastJsonUtil.toJson(iCommonService.getOneRecordById(tblName, nodeChangeId));
        //step1判断
        //todo:未判空
        int order1 = obj1.getInteger("theOrder");
        int order2 = obj2.getInteger("theOrder");
        obj1.put("theOrder",order2);
        obj2.put("theOrder",order1);
        List<Object> list = new ArrayList<>();
        list.add(obj1);
        list.add(obj2);
        return iCommonService.saveSomeRecords(tblName, list);
    }

    @Override
    public Object changeNodeOrder(String tblName, int nodeId, boolean up, JSONObject searchKeys, Map<String,String> regMap) {
        JSONObject obj1 = FastJsonUtil.toJson(iCommonService.getOneRecordById(tblName, nodeId));
        Sort sort = Sort.by(Sort.Direction.DESC, "theOrder");
        searchKeys.put("theOrder", obj1.getInteger("theOrder"));
        if (up) {
            regMap.put("theOrder", LT);
        } else {
            regMap.put("theOrder", GT);
            sort = Sort.by(Sort.Direction.ASC, "theOrder");
        }
        if (searchKeys != null) {
            for (Map.Entry entry : searchKeys.entrySet()) {
                if (StringUtils.isEmpty(entry.getValue())) {
                    regMap.remove(entry.getKey());
                    searchKeys.remove(entry.getKey());
                }
            }
        }
        Object objArr = iCommonService.getSomeRecords(tblName,searchKeys, regMap, sort);
        if (FastJsonUtil.toJson(objArr).getJSONArray("content").size() != 0) {
            JSONObject obj2 = FastJsonUtil.toJson(objArr).getJSONArray("content").getJSONObject(0);
            int tempOrder = obj2.getIntValue("theOrder");
            obj2.put("theOrder", obj1.getIntValue("theOrder"));
            obj1.put("theOrder", tempOrder);
            List<Object> list = new ArrayList<>();
            list.add(obj1);
            list.add(obj2);
            Object obtemp = iCommonService.saveSomeRecords(tblName, list);
            return obtemp;
        } else {
            return null;
        }
    }
    @Override
    public Object editOneNode(String tblName, JSONObject node) {
        //新增
        if (node.getInteger("id") == null || node.getInteger("id") == 0) {
            try {
                Class<?> clazzInfo = Class.forName(Base.entityPackage + tblName);
                if (clazzInfo.getSuperclass().getName().contains("OrderInfo") && (node.getString("theOrder")==null) ) {
                    Sort sort = Sort.by(Sort.Direction.DESC, "theOrder");
                    //获取最大值theOrder,+1后加入
                    List<Object> rawRet = ((Page<Object>)iCommonService.getSomeRecords(tblName, null, null, sort)).getContent();
                    if (rawRet.size() == 0) {
                        node.put("theOrder", 1);
                    } else {
                        JSONObject maxOrder = FastJsonUtil.toJson(rawRet.get(0));
                        node.put("theOrder", maxOrder.getIntValue("theOrder") + 1);
                    }
                }
            }  catch (ClassNotFoundException e) {
                LogUtil.error(logger, e);
                throw BaseResponse.moreInfoError.error("tblName 异常");
            }
        } else { //修改

        }
        if(tblName.equals("MainChannelContent")){
            if(ObjectUtils.isEmpty(node.getDate("channelCTime"))
                    &&!ObjectUtils.isEmpty(node.getBoolean("publishStatus"))
                    &&node.getBoolean("publishStatus")){
                node.put("channelCTime",new Date());
            }else if(!ObjectUtils.isEmpty(node.getBoolean("publishStatus"))&&!node.getBoolean("publishStatus")){
                node.put("channelCTime",null);
            }
        }
        return iCommonService.saveOneRecord(tblName, node);
    }

    /**
     * 删除某些节点
     * @param tblName
     * @param ids
     * @return
     */
    @Override
    public Object deleteSomeNodes(String tblName, List<Integer> ids) {
        iCommonService.deleteSomeRecords(tblName, ids);
        return null;
    }
}
