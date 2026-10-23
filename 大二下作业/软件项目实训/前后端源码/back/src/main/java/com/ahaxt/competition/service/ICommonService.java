package com.ahaxt.competition.service;

import com.alibaba.fastjson.JSONObject;
import org.springframework.data.domain.Sort;
import org.springframework.stereotype.Service;

import java.util.List;
import java.util.Map;
import java.util.Set;
/**
 * @author hongzhangming
 */
@Service
public interface ICommonService {

    Object getOneRecordById(String tblName, Object id);
    Object getOneRecordById(String tblName, Object id, Boolean delFlag);
    Object getRecordsByIds(String tblName, Set<Integer> ids);
    Object getRecordsByIds(String tblName, Set<Integer> ids, Boolean delFlag);

    Object getSomeRecords(String tblName);
    Object getSomeRecords(String tblName, JSONObject searchKeys);
    Object getSomeRecords(String tblName, JSONObject searchKeys, Map repMap);
    Object getSomeRecords(String tblName, JSONObject searchKeys, Map repMap, Sort sort);
    Object getSomeRecords(String tblName, JSONObject searchKeys, Map repMap, Sort sort, Integer page, Integer size);
    Object getSomeRecords(String tblName, JSONObject searchKeys, Map repMap, Sort sort, Integer page, Integer size, Boolean delFlag);
    Object getSomeRecords(String tblName, JSONObject searchKeys, Map repMap, Sort sort, Integer page, Integer size, Boolean delFlag, Map andor);

    /**
     * 内部通过{@link org.springframework.data.repository.core.support.AbstractEntityInformation#isNew(Object)}判断是新增还是修改
     * @param tblName
     * @param json
     * @return
     */
    Object saveOneRecord(String tblName, JSONObject json);


    /**
     * 内部通过{@link org.springframework.data.repository.core.support.AbstractEntityInformation#isNew(Object)}判断是新增还是修改
     * @param tblName
     * @param iterable
     * @return
     */
    Object saveSomeRecords(String tblName, Iterable iterable);

    /**
     * 真删除
     * @param tblName
     * @param id
     * @return
     */
    Object deleteRecord(String tblName, Integer id);

    /**
     * 逻辑删除
     * @param tblName
     * @param id
     * @return
     */
    Object deleteRecordByDelflag(String tblName, Integer id);

    /**
     * 清空（（已删除的）
     * @param tblName
     * @return
     */
    Object deleteRecordsByDelflag(String tblName);


    /**
     * 逻辑删除多条id
     * @param tblName
     * @return
     */
    Object deleteSomeRecords(String tblName, List<Integer> ids);
}
