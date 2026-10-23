package com.ahaxt.competition.repository.db;

import com.ahaxt.competition.entity.db.BaseDepartment;
import com.ahaxt.competition.repository.base.BaseDao;
import org.springframework.stereotype.Repository;

@Repository
public interface BaseDepartmentDao extends BaseDao<BaseDepartment,Integer> {

}
