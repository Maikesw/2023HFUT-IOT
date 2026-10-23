package com.ahaxt.competition.service;

import com.alibaba.fastjson.JSONArray;
import com.alibaba.fastjson.JSONObject;
import org.springframework.stereotype.Service;


@Service
public interface IImportAndExportService {

    Object exportInfo(String key, JSONArray nodes, JSONArray allTableColumns, JSONObject searchWords);
}
