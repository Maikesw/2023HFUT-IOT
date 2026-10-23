package com.ahaxt.competition.controller.common.ctrl;


import com.ahaxt.competition.annotation.PathRestController;
import com.ahaxt.competition.base.Base;
import com.ahaxt.competition.base.BaseResponse;
import com.ahaxt.competition.base.Constant;
import com.ahaxt.competition.entity.db.SysOssFile;
import com.ahaxt.competition.service.ICommonService;
import com.ahaxt.competition.utils.EncryptUtil;
import com.ahaxt.competition.utils.FileUtil;
import com.ahaxt.competition.utils.MinIOUtils;
import com.alibaba.fastjson.JSONArray;
import com.alibaba.fastjson.JSONObject;
import io.swagger.annotations.Api;
import io.swagger.annotations.ApiOperation;
import org.springframework.data.domain.Sort;
import org.springframework.http.MediaType;
import org.springframework.util.ObjectUtils;
import org.springframework.util.StringUtils;
import org.springframework.web.bind.annotation.*;
import org.springframework.web.multipart.MultipartFile;

import javax.annotation.Resource;
import javax.servlet.http.HttpServletResponse;
import java.util.*;
import java.util.stream.Collectors;

/**
 * @author hongzhangming
 * 查询：@getOneRecordById，@getRecordsByID，@getSomeRecords
 * 保存：@saveOneRecord，@saveSomeRecords
 * 新增：addOneRecord，addRecords
 * 删除：@deleteRecordByDelflag，@deleteRecordsByDelflag，@deleteRecords(这个是真的删除)
 */
@Api(tags = "common")
@PathRestController(path = "common")
public class CommonController extends Base {
    @Resource
    protected EncryptUtil encryptUtil;
    @Resource
    protected ICommonService iCommonService;
    @Resource
    private MinIOUtils minIOUtils;


    public String getRandomString(int length){
        String str="abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        Random random=new Random();
        StringBuffer sb=new StringBuffer();
        for(int i=0;i<length;i++){
            int number = random.nextInt(62);
            sb.append(str.charAt(number));
        }
        return sb.toString();
    }


    @PostMapping(path = "getKey" )
    public Object getKey() {
        String key = getRandomString(7);
        JSONObject val = encryptUtil.setAllKeys(key);
        return BaseResponse.ok(val);
    }



     /**
     * 编号查询
     * @param tblName
     * @param id
     * @param delFlag   null：全部    0: 未逻辑删除的   else:报参数异常
     * @return
     */
    @GetMapping(path = "getRecordsById/{tblName}" )
    public Object getRecordsById(@PathVariable String tblName, @RequestParam Set<Integer> id, Boolean delFlag) {
        return BaseResponse.ok(iCommonService.getRecordsByIds(tblName, id, delFlag));
    }
    /**
     * @param tblName
     * @param json
     * @return
     */
    @PostMapping(path = "getSomeRecords/{tblName}", consumes = MediaType.APPLICATION_JSON_VALUE)
    public Object getSomeRecords(@PathVariable String tblName, @RequestBody JSONObject json) {
        String sort = json.getString("sort");
        String order = json.getString("order");
        Sort sorts;
        if (StringUtils.isEmpty(sort) || StringUtils.isEmpty(order)) {
            sorts = Sort.unsorted();
        } else {
            List<Sort.Order> orders = new ArrayList<>();
            String[] sortArr = sort.split(",", -1);
            String[] orderArr = order.split(",", -1);
            if (sortArr.length != orderArr.length) {
                throw BaseResponse.parameterInvalid.error();
            }
            for (int i = 0; i < sortArr.length; i++) {
                if (Sort.Direction.ASC.name().equals(orderArr[i])) {
                    orders.add(Sort.Order.asc(sortArr[i]));
                } else if (Sort.Direction.DESC.name().equals(orderArr[i])) {
                    orders.add(Sort.Order.desc(sortArr[i]));
                } else {
                    throw BaseResponse.parameterInvalid.error();
                }
            }
            sorts = Sort.by(orders);
        }
        return BaseResponse.ok(iCommonService.getSomeRecords(tblName, json, null, sorts, json.getInteger("page"), json.getInteger("size")));
    }

    @PutMapping(path = "saveOneRecord", consumes = MediaType.APPLICATION_JSON_VALUE)
    public Object saveOneRecord(@RequestParam String tblName, @RequestBody JSONObject json) {
        return BaseResponse.ok(iCommonService.saveOneRecord(tblName, json));
    }

    @PutMapping(path = "saveSomeRecords", consumes = MediaType.APPLICATION_JSON_VALUE)
    public Object saveSomeRecords(@RequestParam String tblName, @RequestBody JSONArray array) {
        return BaseResponse.ok(iCommonService.saveSomeRecords(tblName, array));
    }

    @ApiOperation("真删除")
    @DeleteMapping(path = "deleteRecords")
    public Object deleteRecords(@RequestParam String tblName, @RequestParam Integer id) {
        return BaseResponse.ok(iCommonService.deleteRecord(tblName, id));
    }

    @ApiOperation("伪删除")
    @DeleteMapping(path = "deleteRecordByDelflag")
    public Object deleteRecordByDelflag(@RequestParam String tblName, @RequestParam Integer id) {
        return BaseResponse.ok(iCommonService.deleteRecordByDelflag(tblName, id));
    }

    @ApiOperation("清空（已删除的）")
    @DeleteMapping(path = "deleteRecordsByDelflag")
    public Object deleteRecordsByDelflag(@RequestParam String tblName) {
        return BaseResponse.ok(iCommonService.deleteRecordsByDelflag(tblName));
    }

    @ApiOperation(value = "minio文件上传", notes = "type: 1 图片；2 文件")
    @PostMapping(value = "/minio/upload", consumes = MediaType.MULTIPART_FORM_DATA_VALUE)
    public Object miniUpload(@RequestParam MultipartFile[] files, @RequestParam Integer userId, @RequestParam Integer relId, @RequestParam Integer type) {
        List<Object> resList = new ArrayList<>();
        try {
            Object fileInfo = minIOUtils.upload(Constant.BUCKET_NAME, files, type, relId);
            resList.add(fileInfo);
//            for (MultipartFile file : files) {
//                Object fileInfo = minIOUtils.upload(Constant.BUCKET_NAME, file, type, relId);
//                resList.add(fileInfo);
//            }
        } catch (Exception ex) {
            logger.error("error", ex);
        }
        return BaseResponse.ok(resList);
    }

    @ApiOperation(value = "minio文件流下载")
    @PostMapping(value = "/minio/downloadFile", consumes = MediaType.APPLICATION_JSON_VALUE)
    public Object downloadFile(@RequestBody JSONObject requestJson, HttpServletResponse response) {
        Integer id = requestJson.getInteger("id");
        SysOssFile ossFile = sysOssFileDao.getByIdAndIsDeletedFalse(id);
        minIOUtils.download(ossFile.getBucketName(), ossFile.getOssPath(), ossFile.getName(), response);
        return BaseResponse.ok;
    }


    @ApiOperation(value = "minio删除文件")
    @DeleteMapping(value = "/minio/deleteFile")
    public Object deleteMinioByIds(@RequestParam Integer[] ossFileIds) {
        List<SysOssFile> ossFiles = sysOssFileDao.findByIdIn(Arrays.asList(ossFileIds));
        List<String> ossPaths = ossFiles.stream().map(x -> x.getOssPath()).collect(Collectors.toList());
        if (!ObjectUtils.isEmpty(ossPaths)) {
            minIOUtils.removeObjectsResult(Constant.BUCKET_NAME, ossPaths);
        }
        sysOssFileDao.updateByIds(Arrays.asList(ossFileIds));
        return BaseResponse.ok;
    }
}
