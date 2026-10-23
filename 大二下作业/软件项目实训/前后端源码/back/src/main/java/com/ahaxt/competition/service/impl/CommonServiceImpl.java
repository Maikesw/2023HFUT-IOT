package com.ahaxt.competition.service.impl;

import com.ahaxt.competition.base.Base;
import com.ahaxt.competition.base.BaseResponse;
import com.ahaxt.competition.repository.db.BaseJobPositionDao;
import com.ahaxt.competition.service.ICommonService;
import com.ahaxt.competition.utils.DateUtil;
import com.ahaxt.competition.utils.FastJsonUtil;
import com.ahaxt.competition.utils.LogUtil;
import com.alibaba.fastjson.JSONObject;
import org.springframework.context.ApplicationContext;
import org.springframework.data.domain.PageRequest;
import org.springframework.data.domain.Pageable;
import org.springframework.data.domain.Sort;
import org.springframework.data.jpa.domain.Specification;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;
import org.springframework.util.StringUtils;

import javax.annotation.Resource;
import java.lang.reflect.InvocationTargetException;
import java.util.*;

/**
 * @author hongzhangming
 */
@Service
@Transactional(rollbackFor = Exception.class)
public class CommonServiceImpl extends Base implements ICommonService {
    @Resource
    private ApplicationContext applicationContext;

    @Resource
    private BaseJobPositionDao baseJobPosition;
    //private Class<?> clazzDao;
    private Class<?> clazzInfo;
    private Object beanDao;

    /**
     * FIXME 建议删除此方法,定义的类变量 在并发情况下，映射的对象被替换了，导致数据查询时DAO映射异常。
     * @param tblName
     */
//    @Deprecated
//    private void getDao(String tblName) {
//        try {
//            clazzDao = Class.forName(Base.repositoryPackage + tblName + "Dao");
//            clazzInfo = Class.forName(Base.entityPackage + tblName);
//            beanDao = applicationContext.getBean(tblName.substring(0, 1).toLowerCase() + tblName.substring(1) + "Dao", clazzDao);
//        } catch (ClassNotFoundException e) {
//            LogUtil.error(logger, e);
//            throw BaseResponse.moreInfoError.error("tblName 异常");
//        }
//    }

    //region getOneRecordById
    /**
     * 根据主键获取一条记录
     * @param tblName 操作表名
     * @param id      主键id，object形式传入，可以是整型或者字符型
     * @return
     */
    @Override
    public Object getOneRecordById(String tblName, Object id) {
        return getOneRecordById(tblName, id, false);
    }

    /**
     * 根据主键获取一条记录
     * @param tblName 操作表名
     * @param id      主键id，object形式传入，可以是整型或者字符型
     * @param delFlag 是否考虑伪删除
     * @return
     */
    @Override
    public Object getOneRecordById(String tblName, Object id, Boolean delFlag) {
        try {
            Class<?> clazzDao = Class.forName(Base.repositoryPackage + tblName + "Dao");
            Class<?> clazzInfo = Class.forName(Base.entityPackage + tblName);
            Object beanDao = applicationContext.getBean(tblName.substring(0, 1).toLowerCase() + tblName.substring(1) + "Dao", clazzDao);
            if (delFlag) {
                return clazzDao.getMethod("getOne", Object.class).invoke(beanDao, (Integer) id);
            } else {
                return clazzDao.getMethod("getByIdAndIsDeletedFalse", Object.class).invoke(beanDao, (Integer) id);
            }
        } catch (ClassNotFoundException | NoSuchMethodException | IllegalAccessException | InvocationTargetException ex) {
            LogUtil.error(logger, ex);
            throw BaseResponse.moreInfoError.error("reflection processing failed");
        }
    }
    //endregion

    //region getRecordsByIds
    /**
     * 根据一组主键Id获得对应的多条记录
     * @param tblName 表名
     * @param ids     一组id
     * @return
     */
    @Override
    public Object getRecordsByIds(String tblName, Set<Integer> ids) {
        return getRecordsByIds(tblName, ids, false);
    }

    /**
     * 根据一组主键Id获得对应的多条记录
     * @param tblName 表名
     * @param ids     一组id
     * @return
     * @parem delFlag 是否考虑伪删除，默认考虑
     */
    @Override
    public Object getRecordsByIds(String tblName, Set<Integer> ids, Boolean delFlag) {
        try {
            Class<?> clazzDao = Class.forName(Base.repositoryPackage + tblName + "Dao");
            Class<?> clazzInfo = Class.forName(Base.entityPackage + tblName);
            Object beanDao = applicationContext.getBean(tblName.substring(0, 1).toLowerCase() + tblName.substring(1) + "Dao", clazzDao);

            if (delFlag) {
                return clazzDao.getMethod("findByIdIn", Iterable.class).invoke(beanDao, ids);
            } else {
                return clazzDao.getMethod("findByIdInAndIsDeletedFalse", Iterable.class).invoke(beanDao, ids);
            }
        } catch (ClassNotFoundException | NoSuchMethodException | IllegalAccessException | InvocationTargetException ex) {
            LogUtil.error(logger, ex);
            throw BaseResponse.moreInfoError.error("reflection processing failed");
        }
    }
    //endregion

    //region getSomeRecords
    @Override
    public Object getSomeRecords(String tblName) {
        JSONObject json = new JSONObject();
        return getSomeRecords(tblName, json);
    }

    @Override
    public Object getSomeRecords(String tblName, JSONObject searchKeys) {
        return getSomeRecords(tblName, searchKeys, null);
    }

    @Override
    public Object getSomeRecords(String tblName, JSONObject searchKeys, Map repMap) {
        Sort sort = Sort.by(Sort.Direction.DESC, "id");
        return getSomeRecords(tblName, searchKeys, repMap, sort);
    }

    @Override
    public Object getSomeRecords(String tblName, JSONObject searchKeys, Map repMap, Sort sort) {
        return getSomeRecords(tblName, searchKeys, repMap, sort, null, null);
    }

    @Override
    public Object getSomeRecords(String tblName, JSONObject searchKeys, Map repMap, Sort sort, Integer page, Integer size) {
        return getSomeRecords(tblName, searchKeys, repMap, sort, page, size, false);
    }

    @Override
    public Object getSomeRecords(String tblName, JSONObject searchKeys, Map repMap, Sort sort, Integer page, Integer size, Boolean delFlag) {
        return getSomeRecords(tblName, searchKeys, repMap, sort, page, size, delFlag, null);
    }
    /**
     * 注意：json中isDeleted字段设置了默认值false，若想查全部需设置 json.put("isDeleted":"");
     * @param tblName
     * @param searchKeys
     * @param page
     * @param size
     * @param sort
     * @return
     */
    @Override
    public Object getSomeRecords(String tblName, JSONObject searchKeys, Map repMap, Sort sort, Integer page, Integer size, Boolean delFlag, Map andor) {
        Object ret;
        if (searchKeys != null) {
            // 过滤掉空内容
            JSONObject searchKey = new JSONObject();
            searchKey.putAll(searchKeys);
            for (Map.Entry entry : searchKey.entrySet()) {
                if (StringUtils.isEmpty(entry.getValue())) {
                    repMap.remove(entry.getKey());
                    searchKeys.remove(entry.getKey());
                }
            }
        }
        else {
            searchKeys = new JSONObject();
        }
        if (!delFlag) {
            searchKeys.put("isDeleted", 0);
        }
        try {
            Class<?> clazzDao = Class.forName(Base.repositoryPackage + tblName + "Dao");
            Class<?> clazzInfo = Class.forName(Base.entityPackage + tblName);
            Object beanDao = applicationContext.getBean(tblName.substring(0, 1).toLowerCase() + tblName.substring(1) + "Dao", clazzDao);
            if (page == null || size == null || size == -1) {
                ret = clazzDao.getMethod("findAll", Specification.class, Pageable.class).invoke(beanDao, super.getSpecification(searchKeys, repMap, andor, clazzInfo), PageRequest.of(0, 10000, sort));
            } else {
                ret = clazzDao.getMethod("findAll", Specification.class, Pageable.class).invoke(beanDao, super.getSpecification(searchKeys, repMap, andor, clazzInfo), PageRequest.of(page - 1, size, sort));
            }
            return ret;
        } catch (ClassNotFoundException | NoSuchMethodException | IllegalAccessException | InvocationTargetException ex) {
            LogUtil.error(logger, ex);
            throw BaseResponse.moreInfoError.error("reflection processing failed");
        }
    }
    //endregion

    @Override
    public Object saveOneRecord(String tblName, JSONObject json) {
        try {
            Class<?> clazzDao = Class.forName(Base.repositoryPackage + tblName + "Dao");
            Class<?> clazzInfo = Class.forName(Base.entityPackage + tblName);
            Object beanDao = applicationContext.getBean(tblName.substring(0, 1).toLowerCase() + tblName.substring(1) + "Dao", clazzDao);
            if ( json.keySet().contains("id") && json.get("id")!=null && !json.get("id").equals(0)) {
                json.put("updateTime", DateUtil.format(new Date(), "yyyy-MM-dd HH:mm:ss"));
            }
            return clazzDao.getMethod("save", Object.class).invoke(beanDao, FastJsonUtil.toJavaObject(json, Class.forName(Base.entityPackage + tblName)));
        } catch (ClassNotFoundException e) {
            LogUtil.error(logger, e);
            throw BaseResponse.moreInfoError.error("tblName 异常");
        } catch (NoSuchMethodException | IllegalAccessException | InvocationTargetException ex) {
            LogUtil.error(logger, ex);
            throw BaseResponse.moreInfoError.error("reflection processing failed");
        }
    }

    @Override
    public Object saveSomeRecords(String tblName, Iterable iterable) {
        try {
            Class<?> clazzDao = Class.forName(Base.repositoryPackage + tblName + "Dao");
            Class<?> clazzInfo = Class.forName(Base.entityPackage + tblName);
            Object beanDao = applicationContext.getBean(tblName.substring(0, 1).toLowerCase() + tblName.substring(1) + "Dao", clazzDao);

            List list = new ArrayList();
            Iterator iterator = iterable.iterator();
            while (iterator.hasNext()) {
                list.add(FastJsonUtil.parseObject(FastJsonUtil.toString(iterator.next()), clazzInfo));
            }
            return clazzDao.getMethod("saveAll", Iterable.class).invoke(beanDao, list);
        } catch (ClassNotFoundException | NoSuchMethodException | IllegalAccessException | InvocationTargetException ex) {
            LogUtil.error(logger, ex);
            throw BaseResponse.moreInfoError.error("reflection processing failed");
        }
    }

    /**
     * 真删除
     * @param tblName
     * @param id
     * @return
     */
    @Override
    public Object deleteRecord(String tblName, Integer id) {
        try {
            Class<?> clazzDao = Class.forName(Base.repositoryPackage + tblName + "Dao");
            Class<?> clazzInfo = Class.forName(Base.entityPackage + tblName);
            Object beanDao = applicationContext.getBean(tblName.substring(0, 1).toLowerCase() + tblName.substring(1) + "Dao", clazzDao);

            return clazzDao.getMethod("deleteById", Object.class).invoke(beanDao, id);
        } catch (ClassNotFoundException | NoSuchMethodException | IllegalAccessException | InvocationTargetException ex) {
            LogUtil.error(logger, ex);
            throw BaseResponse.moreInfoError.error("reflection processing failed");
        }
    }

    @Override
    public Object deleteRecordByDelflag(String tblName, Integer id) {
        try {
            Class<?> clazzDao = Class.forName(Base.repositoryPackage + tblName + "Dao");
            Class<?> clazzInfo = Class.forName(Base.entityPackage + tblName);
            Object beanDao = applicationContext.getBean(tblName.substring(0, 1).toLowerCase() + tblName.substring(1) + "Dao", clazzDao);

            return clazzDao.getMethod("deleteByIsDeleted", Object.class).invoke(beanDao, id);
        } catch (ClassNotFoundException | NoSuchMethodException | IllegalAccessException | InvocationTargetException ex) {
            LogUtil.error(logger, ex);
            throw BaseResponse.moreInfoError.error("reflection processing failed");
        }
    }

    /**
     * 清空（（已删除的）
     * @param tblName
     * @return
     */
    @Override
    public Object deleteRecordsByDelflag(String tblName) {
        try {
            Class<?> clazzDao = Class.forName(Base.repositoryPackage + tblName + "Dao");
            Class<?> clazzInfo = Class.forName(Base.entityPackage + tblName);
            Object beanDao = applicationContext.getBean(tblName.substring(0, 1).toLowerCase() + tblName.substring(1) + "Dao", clazzDao);
            return clazzDao.getMethod("deleteByIsDeleted").invoke(beanDao);
        } catch (ClassNotFoundException | NoSuchMethodException | IllegalAccessException | InvocationTargetException ex) {
            LogUtil.error(logger, ex);
            throw BaseResponse.moreInfoError.error("reflection processing failed");
        }
    }

    /**
     * 通过id删除多个
     * @param tblName
     * @param ids
     * @return
     */

    @Override
    public Object deleteSomeRecords(String tblName, List<Integer> ids) {
        try {
            Class<?> clazzDao = Class.forName(Base.repositoryPackage + tblName + "Dao");
            Class<?> clazzInfo = Class.forName(Base.entityPackage + tblName);
            Object beanDao = applicationContext.getBean(tblName.substring(0, 1).toLowerCase() + tblName.substring(1) + "Dao", clazzDao);

            return clazzDao.getMethod("deleteByIdIn", Iterable.class).invoke(beanDao, ids);
        } catch (ClassNotFoundException | NoSuchMethodException | IllegalAccessException | InvocationTargetException ex) {
            LogUtil.error(logger, ex);
            throw BaseResponse.moreInfoError.error("reflection processing failed");
        }
    }
}
