package com.ahaxt.competition.repository.db;

import com.ahaxt.competition.entity.db.BaseGuestBook;
import com.ahaxt.competition.repository.base.BaseDao;
import org.springframework.stereotype.Repository;

@Repository
public interface BaseGuestBookDao extends BaseDao<BaseGuestBook, Integer> {
}
