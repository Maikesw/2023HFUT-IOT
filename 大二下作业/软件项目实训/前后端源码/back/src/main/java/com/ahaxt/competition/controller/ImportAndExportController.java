package com.ahaxt.competition.controller;

import com.ahaxt.competition.annotation.PathRestController;
import com.ahaxt.competition.base.BaseResponse;
import com.ahaxt.competition.service.IImportAndExportService;
import com.alibaba.fastjson.JSONArray;
import com.alibaba.fastjson.JSONObject;
import io.swagger.annotations.Api;
import io.swagger.annotations.ApiOperation;
import org.springframework.http.MediaType;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.multipart.MultipartFile;

import javax.annotation.Resource;

@Api(tags = "导入导出")
@PathRestController("importAndExport")
public class ImportAndExportController {

    @Resource
    private IImportAndExportService iImportAndExportService;

    @ApiOperation(value = "导入信息（测试）", notes = "导入测试")
    @PostMapping(value = "importProduct/{type}", consumes = MediaType.MULTIPART_FORM_DATA_VALUE)
    public Object importProduct(MultipartFile file, @PathVariable Integer type) {
        //用户
//        if(type == 1){
//            return BaseResponse.ok(iImportAndExportService.importUsers(FileUtil.toFile(file)));
//        }
        return BaseResponse.ok;
    }

    @ApiOperation(value = "导入", notes = "各页面信息导入")
    @PostMapping(value = "importInfo")
    public Object importInfo(MultipartFile file, @RequestParam String keyWord, @RequestParam Boolean type) {
        switch (keyWord) {
            default:
                throw BaseResponse.moreInfoError.error("keyWord异常");
        }
    }

    @ApiOperation(value = "导出", notes = "各页面信息导出")
    @PostMapping(value = "exportInfo")
    public Object exportInfo(@RequestBody JSONObject requestJson) {
        String keyWords = requestJson.getString("keyWords");
        JSONArray nodes = requestJson.getJSONArray("nodes");
        JSONArray allTableColumns = requestJson.getJSONArray("allTableColumns");
        JSONObject searchWords = requestJson.getJSONObject("searchWords");
        return BaseResponse.ok(iImportAndExportService.exportInfo(keyWords, nodes, allTableColumns, searchWords));
    }
}
