package com.ahaxt.competition.repository.db;

import com.ahaxt.competition.entity.db.RelUserRole;
import com.ahaxt.competition.repository.base.BaseDao;
import org.springframework.data.jpa.repository.Modifying;
import org.springframework.data.jpa.repository.Query;
import org.springframework.stereotype.Repository;

import java.util.List;

/**
 * @author hongzhangming
 */
@Repository
public interface RelUserRoleDao extends BaseDao<RelUserRole, Integer> {

    /**
     * 删除某个用户的所有角色
     * */
    @Modifying
    @Query(value = "update RelUserRole t set t.isDeleted=true where t.userId=?1")
    void deleteRoleRel(Integer uid);

    List<RelUserRole> findByRoleIdAndIsDeletedFalse(Integer roleId);
}
