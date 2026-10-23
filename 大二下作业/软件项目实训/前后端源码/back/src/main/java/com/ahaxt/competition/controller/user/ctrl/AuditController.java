package com.ahaxt.competition.controller.user.ctrl;

import com.ahaxt.competition.annotation.PathRestController;
import com.ahaxt.competition.controller.common.ctrl.CommonController;
import io.swagger.annotations.Api;

@Api(tags = "审核相关")
@PathRestController("audit")
public class AuditController extends CommonController {

//    @ApiOperation(value = "批量审核")
//    @PostMapping(value = "/all")
//    public Object auditAll() {
//        List<RelUserRole> relations = ((Page<RelUserRole>) iCommonService.getSomeRecords("RelUserRole")).getContent();
//        for (int i = 0; i < relations.size(); i++){
//            relations.get(i).setIsAudit(1);
//        }
//        return iCommonService.saveSomeRecords("RelUserRole", relations);
//    }
//
//    @ApiOperation(value = "审核/反审核")
//    @PostMapping(value = "/reverse", consumes = MediaType.APPLICATION_JSON_VALUE)
//    public Object auditReverse(@RequestBody JSONObject requestJson) {
//        String key = requestJson.getString("keyWords");
//        int nodeId = requestJson.getInteger("nodeId");
//        JSONObject obj = FastJsonUtil.toJson(iCommonService.getOneRecordById(key, nodeId));
//        boolean flag = obj.getBoolean("isAudit");
//        obj.put("isAudit", !flag);
//        return  saveOneRecord(key, obj);
//    }
//
//    @ApiOperation(value = "新的审核")
//    @PostMapping(value = "/new", consumes = MediaType.APPLICATION_JSON_VALUE)
//    public Object auditNew(@RequestBody JSONObject requestJson) {
//        String key = requestJson.getString("keyWords");
//        int nodeId = requestJson.getInteger("nodeId");
//        int value = requestJson.getInteger("value");
//        JSONObject obj = FastJsonUtil.toJson(iCommonService.getOneRecordById(key, nodeId));
//        obj.put("isAudit", value);
//        return  saveOneRecord(key, obj);
//    }
}
