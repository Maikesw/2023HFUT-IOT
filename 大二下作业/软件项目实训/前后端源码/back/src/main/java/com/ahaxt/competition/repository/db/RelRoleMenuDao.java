package com.ahaxt.competition.repository.db;

import com.ahaxt.competition.entity.db.RelRoleMenu;
import com.ahaxt.competition.repository.base.BaseDao;
import org.springframework.stereotype.Repository;

import java.util.List;

/**
 * @author hongzhangming
 */
@Repository
public interface RelRoleMenuDao extends BaseDao<RelRoleMenu, Integer> {

    /**
     * 查询角色权限
     *
     * @param roleIdSet
     * @return
     */
    List<RelRoleMenu> findByRoleIdInAndIsDeletedFalse(Iterable<Integer> roleIdSet);

    /**
     * 查询菜单关联角色
     * @param collect
     * @return
     */
    List<RelRoleMenu> findByMenuIdInAndIsDeletedFalse(Iterable<Integer> collect);
}
