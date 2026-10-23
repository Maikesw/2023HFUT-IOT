package com.ahaxt.competition.controller.website.ctrl;

import com.ahaxt.competition.annotation.PathRestController;
import com.ahaxt.competition.base.BaseResponse;
import com.ahaxt.competition.service.IDataTreeService;
import com.ahaxt.competition.service.IWebsiteService;
import com.alibaba.fastjson.JSONObject;
import io.swagger.annotations.Api;
import io.swagger.annotations.ApiOperation;
import org.springframework.data.domain.Sort;
import org.springframework.http.MediaType;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestParam;

import javax.annotation.Resource;

/**
 * @author lhy
 * @date 2022/11/7
 */
@Api(tags = "官网")
@PathRestController("website")
public class WebsiteController {


    @Resource
    private IWebsiteService iWebsiteService;
    @Resource
    private IDataTreeService iDataTreeService;

    @ApiOperation(value = "栏目内容")
    @PostMapping(value = "/columnContent", consumes = MediaType.APPLICATION_JSON_VALUE)
    public Object columnContent(@RequestBody JSONObject requestJson){
        Integer channelTypeId = requestJson.getInteger("channelTypeId");
        Integer page = requestJson.getInteger("page");
        Integer size = requestJson.getInteger("size");
        return iWebsiteService.getColumnContent(channelTypeId,page,size);
    }

    @ApiOperation(value = "栏目")
    @GetMapping(value = "/column")
    public Object column(){
        Sort orderSort = Sort.by(Sort.Direction.ASC, "theOrder");
        return iDataTreeService.getTreeList("BaseChannelType",false,new JSONObject(),false,"",-1,orderSort);
    }
    @ApiOperation(value = "首页")
    @GetMapping(value = "/homePage")
    public Object getHomePage(){

        return iWebsiteService.getHomePage();
    }
    @ApiOperation(value = "阅读数增加")
    @GetMapping(value = "/readNum")
    public Object readNum(@RequestParam Integer id){

        return iWebsiteService.readNum(id);
    }
    @ApiOperation(value = "搜索")
    @GetMapping(value = "/search")
    public Object search(@RequestParam String word){

        return iWebsiteService.search(word);
    }
}
