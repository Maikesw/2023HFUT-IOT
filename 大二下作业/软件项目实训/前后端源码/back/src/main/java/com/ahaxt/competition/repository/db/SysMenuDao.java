package com.ahaxt.competition.repository.db;

import com.ahaxt.competition.entity.db.SysMenu;
import com.ahaxt.competition.repository.base.BaseTreeDao;
import org.springframework.stereotype.Repository;

/**
 * @author hongzhangming
 */
@Repository
public interface SysMenuDao extends BaseTreeDao<SysMenu, Integer> {
}
