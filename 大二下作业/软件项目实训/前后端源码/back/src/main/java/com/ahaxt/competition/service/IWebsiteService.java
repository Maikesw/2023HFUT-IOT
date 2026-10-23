package com.ahaxt.competition.service;

import org.springframework.stereotype.Service;

/**
 * @author lhy
 * @date 2022/11/7
 */
@Service
public interface IWebsiteService {
    Object getColumnContent(Integer channelTypeId, Integer page, Integer size);

    Object getHomePage();

    Object readNum(Integer id);

    Object search(String word);

}
