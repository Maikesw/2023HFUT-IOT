package com.ahaxt.competition.controller.common.ctrl;

import com.ahaxt.competition.annotation.PathRestController;
import com.ahaxt.competition.base.BaseResponse;
import com.ahaxt.competition.utils.EncryptUtil;
import com.ahaxt.competition.utils.GeneralUtil;
import com.alibaba.fastjson.JSONArray;
import com.alibaba.fastjson.JSONObject;
import io.swagger.annotations.Api;
import io.swagger.annotations.ApiOperation;
import org.springframework.data.domain.Sort;
import org.springframework.http.MediaType;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;

import javax.annotation.Resource;
import java.util.*;
import java.util.stream.Collectors;


@Api(tags = "dataTree用户控件对应类")
@PathRestController(value = "dataTree")
public class DataTreeController extends CommonController {
    @Resource
    protected EncryptUtil encryptUtil;


    @ApiOperation("初始化树/读取所有节点")
    @PostMapping(value = "/readAllTreeNodes", consumes = MediaType.APPLICATION_JSON_VALUE)
    public Object readAllTreeNodes(@RequestBody JSONObject requestJson) {
        loggerRecord("readAllTreeNodes", requestJson);
        String key = encryptUtil.getKeyWord(requestJson.getString("keyWords"));
//        String key = requestJson.getString("keyWords");
        String preName = requestJson.getString("preName");
        Boolean virtualRootFlag = true;
        Boolean lazy = true;
        Integer parentId = -1;
        if(requestJson.getBoolean("virtualRootFlag") !=  null) {
            virtualRootFlag = requestJson.getBoolean("virtualRootFlag");
        }
        if(requestJson.getBoolean("lazy") != null) {
            lazy = requestJson.getBoolean("lazy");
            parentId = requestJson.getInteger("parentId");
        }
        JSONObject searchKey = requestJson.getJSONObject("searchKey");
        Sort sortJson = GeneralUtil.getSortInfo(requestJson.getJSONObject("sort"));
        return BaseResponse.ok(iDataTreeService.getTreeList(key, virtualRootFlag, searchKey, lazy, preName, parentId, sortJson));
    }
    @ApiOperation("交换两个节点顺序")
    @PostMapping(value = "/changeTwoNodes", consumes = MediaType.APPLICATION_JSON_VALUE)
    public Object changeTwoNodes(@RequestBody JSONObject requestJson) {
        loggerRecord("changeTwoNodes", requestJson);
        String key = encryptUtil.getKeyWord(requestJson.getString("keyWords"));
//        String key = requestJson.getString("keyWords");
        int nodeId = requestJson.getInteger("nodeId");
        int nodeChangeId = requestJson.getInteger("nodeChangeId");
        return BaseResponse.ok(iDataTreeService.changeTwoNodes(key, nodeId, nodeChangeId));
    }
    @ApiOperation("编辑/新增节点")
    @PostMapping(value = "/editOneNode", consumes = MediaType.APPLICATION_JSON_VALUE)
    public Object editOneNode(@RequestBody JSONObject requestJson) {
        loggerRecord("editOneNode", requestJson);
        String key = encryptUtil.getKeyWord(requestJson.getString("keyWords"));
//        String key = requestJson.getString("keyWords");
        JSONObject node = requestJson.getJSONObject("node");
        System.out.println(key + node.toJSONString());
        return BaseResponse.ok(iDataTreeService.editOneNode(key, node));
    }
    @ApiOperation("删除节点")
    @PostMapping(value = "/delOneNode", consumes = MediaType.APPLICATION_JSON_VALUE)
    public Object delOneNode(@RequestBody JSONObject requestJson) {
        String key = encryptUtil.getKeyWord(requestJson.getString("keyWords"));
//        String key = requestJson.getString("keyWords");
        JSONObject node = requestJson.getJSONObject("node");
        JSONObject logger = new JSONObject();
        logger.put("keyWords", key);
        logger.put("node",node);
        loggerRecord("delOneNode", logger);
        return BaseResponse.ok(iDataTreeService.delOneNode(key, node));
    }
    @ApiOperation("删除多个树节点")
    @PostMapping(value = "/delManyNode", consumes = MediaType.APPLICATION_JSON_VALUE)
    public Object delManyNode(@RequestBody JSONObject requestJson) {
        loggerRecord("delManyNode", requestJson);
        String key = encryptUtil.getKeyWord(requestJson.getString("keyWords"));
        JSONArray treeNodes = requestJson.getJSONArray("nodes");
        return BaseResponse.ok(iDataTreeService.delManyNodes(key,treeNodes));
//        String key = encryptUtil.getKeyWord(requestJson.getString("keyWords"));
//        List<String> treeNodes = Arrays.asList(encryptUtil.getKeyWord(requestJson.getString("nodes")).split(SPLIT_OPERATOR.COMMA));
//        return BaseResponse.ok(iDataListService.deleteSomeNodes(key, treeNodes.stream().map(Integer::parseInt).collect(Collectors.toList())));
    }
    @ApiOperation("获得当前节点的所有父亲节点")
    @PostMapping(value = "/getAllParentIndex", consumes = MediaType.APPLICATION_JSON_VALUE)
    public Object getAllParentIndex(@RequestBody JSONObject requestJson) {
        loggerRecord("getAllParentIndex", requestJson);
        String key = encryptUtil.getKeyWord(requestJson.getString("keyWords"));
        Integer nodeId = requestJson.getInteger("nodeId");
        return BaseResponse.ok(iDataTreeService.getAllParentIndex(key, nodeId));
    }
    @ApiOperation("获得当前节点的直接父亲节点")
    @PostMapping(value = "/getNearestParent", consumes = MediaType.APPLICATION_JSON_VALUE)
    public Object getNearestParent(@RequestBody JSONObject requestJson) {
        loggerRecord("getNearestParent", requestJson);
        String key = encryptUtil.getKeyWord(requestJson.getString("keyWords"));
//        String key = requestJson.getString("keyWords");
        Integer nodeId = requestJson.getInteger("nodeId");
        return BaseResponse.ok(iDataTreeService.getNearestParent(key, nodeId));
    }
    @ApiOperation("获得当前节点的兄弟节点")
    @PostMapping(value = "/getAllBrotherIndex", consumes = MediaType.APPLICATION_JSON_VALUE)
    public Object getAllBrotherIndex(@RequestBody JSONObject requestJson) {
        loggerRecord("getAllBrotherIndex", requestJson);
        String key = encryptUtil.getKeyWord(requestJson.getString("keyWords"));
//        String key = requestJson.getString("keyWords");
        Integer nodeId = requestJson.getInteger("nodeId");
        return BaseResponse.ok(iDataTreeService.getAllBrotherIndex(key, nodeId));
    }
    @ApiOperation("获得当前节点的根节点")
    @PostMapping(value = "/getFirstParent", consumes = MediaType.APPLICATION_JSON_VALUE)
    public Object getFirstParent(@RequestBody JSONObject requestJson) {
        loggerRecord("getFirstParent", requestJson);
        String key = encryptUtil.getKeyWord(requestJson.getString("keyWords"));
//        String key = requestJson.getString("keyWords");
        Integer nodeId = requestJson.getInteger("nodeId");
        return BaseResponse.ok(iDataTreeService.getFirstParent(key, nodeId, null));
    }
    @ApiOperation(value = "树结构联合列表结构通用高级搜索",notes = "reg：｛key,  !或<或>或=或<=或>=或！=或≈或()或！()")
    @PostMapping(value = "/commonSearch", consumes = MediaType.APPLICATION_JSON_VALUE)
    public Object commonSearch(@RequestBody JSONObject requestJson) {
        loggerRecord("commonSearch", requestJson);
        JSONArray treeInfo = requestJson.getJSONArray("treeInfo");
        String listKeyWords = requestJson.getString("listKeyWords");
        JSONObject searchKey = new JSONObject();
        JSONObject regJson = new JSONObject();
        JSONObject pageInfo = new JSONObject();
        JSONObject sortJson = new JSONObject();
        if(requestJson.getJSONObject("searchKey") != null){
            searchKey = requestJson.getJSONObject("searchKey");
        }
        if(requestJson.getJSONObject("regKey") != null){
            regJson = requestJson.getJSONObject("regKey");
        }
        if(requestJson.getJSONObject("pageInfo") != null){
            pageInfo = requestJson.getJSONObject("pageInfo");
        }
        if(requestJson.getJSONObject("sortJson") != null){
            sortJson = requestJson.getJSONObject("sortJson");
        }

        Map<String, String> regMap =  new HashMap<>();
        JSONObject searchKeys = new JSONObject();
        if(regJson != null && regJson.size() > 0){
            //各种类型查询
            for(String key : regJson.keySet()){
                if(key.equals("createTime") || key.equals("updateTime")){
                    if(regJson.getJSONObject(key) != null){
                        searchKeys.put(key, regJson.getJSONObject(key));
                    }
                } else {
                    searchKeys.put(key, searchKey.getString(key));
                    regMap.put(key, regJson.getString(key));
                }
            }
        }
        if(searchKey != null && searchKey.size() > 0) {
            //条件查询
            searchKeys.putAll(searchKey);
        }
        Sort sort = GeneralUtil.getSortInfo(sortJson);
        Map<String,Integer> pageRes = GeneralUtil.getPageInfo(pageInfo);


        Object res = new Object();
        if(listKeyWords != null) {
            if(treeInfo != null && treeInfo.size() > 0){
                //树表联合查询
                res = BaseResponse.ok(iDataTreeService.getTreeJointList(treeInfo, listKeyWords, searchKeys, regMap, sort, pageRes.get("page"), pageRes.get("size")));
            } else {
                //单列表查询
                res = BaseResponse.ok(iDataListService.getSomeRecords(listKeyWords, searchKeys, regMap, sort, pageRes.get("page"),pageRes.get("size")));
            }
        }
        return res;
    }
}
