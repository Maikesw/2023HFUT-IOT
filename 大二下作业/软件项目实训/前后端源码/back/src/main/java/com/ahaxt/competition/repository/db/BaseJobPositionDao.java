package com.ahaxt.competition.repository.db;

import com.ahaxt.competition.entity.db.BaseJobPosition;
import com.ahaxt.competition.repository.base.BaseDao;
import org.springframework.stereotype.Repository;

@Repository
public interface BaseJobPositionDao extends BaseDao<BaseJobPosition, Integer> {
}
