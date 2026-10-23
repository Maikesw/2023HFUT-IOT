package com.ahaxt.competition.repository.db;

import com.ahaxt.competition.entity.db.BaseUser;
import com.ahaxt.competition.repository.base.BaseDao;
import org.springframework.data.jpa.repository.Query;
import org.springframework.stereotype.Repository;

import java.util.List;

/**
 * @author hongzhangming
 */
@Repository
public interface BaseUserDao extends BaseDao<BaseUser, Integer> {

    /**
     * 查询用户
     * @param phone
     * @return
     */
    List<BaseUser> findByPhoneAndIsDeletedFalse(String phone);

    /**
     * 查询account,判重使用
     * */
    @Query(value = "select t.account from BaseUser t where t.isDeleted=false")
    List<String> getAllAccount();
}
