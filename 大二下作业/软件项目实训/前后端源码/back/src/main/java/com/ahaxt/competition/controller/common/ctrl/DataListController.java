package com.ahaxt.competition.controller.common.ctrl;

import com.ahaxt.competition.annotation.PathRestController;
import com.ahaxt.competition.base.BaseResponse;
import com.ahaxt.competition.utils.EncryptUtil;
import com.ahaxt.competition.utils.FastJsonUtil;
import com.ahaxt.competition.utils.GeneralUtil;
import com.ahaxt.competition.utils.RedisUtil;
import com.alibaba.fastjson.JSONArray;
import com.alibaba.fastjson.JSONObject;
import com.fasterxml.jackson.core.JsonParser;
import com.google.gson.JsonObject;
import io.swagger.annotations.Api;
import io.swagger.annotations.ApiOperation;
import org.springframework.data.domain.Sort;
import org.springframework.http.MediaType;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;

import javax.annotation.Resource;
import java.util.*;
import java.util.stream.Collectors;

//import static com.ahaxt.competition.utils.EncryptUtil.aesDecrypt;
//import static com.ahaxt.competition.utils.EncryptUtil.getKeyWord;

/**
 * @author cherish
 */

@Api(tags = "dataList用户控件对应类")
@PathRestController(value = "dataList")
public class DataListController extends CommonController {
    @Resource
    protected EncryptUtil encryptUtil;


    @ApiOperation(value = "1 支持各种条件查询/条件查询",notes = " reg：｛key,  !或<或>或=或<=或>=或！=或≈或()或！()｝")
    @PostMapping(value = "/getSomeRecords", consumes = MediaType.APPLICATION_JSON_VALUE)
    public Object getSomeRecords(@RequestBody JSONObject requestJson) {
        loggerRecord("getSomeRecords", requestJson);
        //条件查询searchKey //模糊查询reg
        String tblName = encryptUtil.getKeyWord(requestJson.getString("keyWords").replace("@","+"));
        JSONObject treeInfo = requestJson.getJSONObject("treeInfo");
        JSONObject searchKeys = new JSONObject();
        JSONObject regJson = new JSONObject();
        try{
            searchKeys = FastJsonUtil.toJson(encryptUtil.getKeyWord(requestJson.getString("searchKey")));
        } catch (Exception e) {

        }
        try {
            regJson = FastJsonUtil.toJson(encryptUtil.getKeyWord(requestJson.getString("reg")));
        } catch (Exception e) {

        }
        JSONObject andorJson = requestJson.getJSONObject("andor");
        Map<String,String> regMap =  new HashMap<>();
        if(regJson != null) { //各种条件查询
            for(String key : regJson.keySet()){
                regMap.put(key, regJson.getString(key));
            }
        }
        Map<String,Boolean> andor =  new HashMap<>();
        if(andorJson != null) { //各种条件查询
            for(String key : andorJson.keySet()){
                andor.put(key, andorJson.getBoolean(key));
            }
        }
        Sort sortJson = GeneralUtil.getSortInfo(requestJson.getJSONObject("sort"));
        Map<String, Integer> pageInfo = GeneralUtil.getPageInfo(requestJson.getJSONObject("pageInfo"));
        if (treeInfo != null && treeInfo.size() > 0) { //有树结构查询搜索要求
            ArrayList<JSONObject> children = iDataTreeService.getAllChildren(treeInfo);
            String treeRelColName = treeInfo.getString("treeRelColName");
            String ids = children.stream().map(p->p.getString("id")).collect(Collectors.joining(SPLIT_OPERATOR.COMMA));
            if (searchKeys == null) {
                searchKeys = new JSONObject();
            }
            searchKeys.put(treeRelColName, ids);
            regMap.put(treeRelColName,IN);
        }
        return BaseResponse.ok(iCommonService.getSomeRecords(tblName, searchKeys, regMap, sortJson, pageInfo.get("page"), pageInfo.get("size"),false, andor));
    }

    @ApiOperation("2 删除一个或多个节点")
    @PostMapping(value = "/delOneOrManyNodes", consumes = MediaType.APPLICATION_JSON_VALUE)
    public Object delOneOrManyNode(@RequestBody JSONObject requestJson) {
        String key = encryptUtil.getKeyWord(requestJson.getString("keyWords"));
        List<String> ids = Arrays.asList(encryptUtil.getKeyWord(requestJson.getString("ids")).split(SPLIT_OPERATOR.COMMA));

        JSONObject logger = new JSONObject();
        logger.put("keyWords", key);
        logger.put("node",ids);
        loggerRecord("delOneOrManyNode", logger);


        return BaseResponse.ok(iDataListService.deleteSomeNodes(key, ids.stream().map(Integer::parseInt).collect(Collectors.toList())));
    }

    @ApiOperation("3 新增/编辑某个节点")
    @PostMapping(value = "/editOneNode", consumes = MediaType.APPLICATION_JSON_VALUE)
    public Object editOneNode(@RequestBody JSONObject requestJson) {
        loggerRecord("editOneNode", requestJson);
      String key = encryptUtil.getKeyWord(requestJson.getString("keyWords"));
      JSONObject node = requestJson.getJSONObject("node");
      return BaseResponse.ok(iDataListService.editOneNode(key, node));
    }

    @ApiOperation("4 交换两个节点顺序")
    @PostMapping(value = "/changeTwoNodes", consumes = MediaType.APPLICATION_JSON_VALUE)
    public Object changeTwoNodes(@RequestBody JSONObject requestJson) {
        loggerRecord("changeTwoNodes", requestJson);
        String key = encryptUtil.getKeyWord(requestJson.getString("keyWords"));
        //String key = requestJson.getString("keyWords");
        int nodeId = requestJson.getInteger("nodeId");
        int nodeChangeId = requestJson.getInteger("nodeChangeId");
        return BaseResponse.ok(iDataListService.changeTwoNodes(key, nodeId, nodeChangeId));
    }
    @ApiOperation("5 单个节点上下移")
    @PostMapping(value = "/changeNodeOrder", consumes = MediaType.APPLICATION_JSON_VALUE)
    public Object changeNodeOrder(@RequestBody JSONObject requestJson) {
        loggerRecord("changeNodeOrder", requestJson);
        String keyWords = encryptUtil.getKeyWord(requestJson.getString("keyWords"));
        //String keyWords = requestJson.getString("keyWords");
        int nodeId = requestJson.getInteger("nodeId");
        boolean up = requestJson.getBoolean("up");
        JSONObject searchKeys = requestJson.getJSONObject("moveSearchKeys");
        JSONObject regKeys = requestJson.getJSONObject("moveRegKeys");
        if (searchKeys == null) {
            searchKeys = new JSONObject();
        }
        Map<String,String> regMap =  new HashMap<>();
        if(regKeys != null) { //各种条件查询
            for(String key : regKeys.keySet()){
                regMap.put(key, regKeys.getString(key));
            }
        }
        Object obj = iDataListService.changeNodeOrder(keyWords, nodeId, up, searchKeys, regMap);
        if (obj != null) {
            return BaseResponse.ok(obj);
        }
        else {
            throw BaseResponse.moreInfoError.error("已经是第一个/最后一个节点，无法移动！");
        }
    }
}

