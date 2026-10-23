package com.ahaxt.competition.repository.db;

import com.ahaxt.competition.entity.db.SysLogger;
import com.ahaxt.competition.repository.base.BaseDao;
import org.springframework.stereotype.Repository;

@Repository
public interface SysLoggerDao extends BaseDao<SysLogger, Integer> {
}
