package com.ahaxt.competition.utils;

import com.ahaxt.competition.base.Constant;
import com.alibaba.fastjson.JSONObject;
import org.springframework.data.domain.Sort;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

/**
 * @author cherish
 */
public class GeneralUtil {

    /**
     * 处理分页信息
     * 规则：
     * 1、pageInfo为空是传默认值
     * 2、pageInfo不为空且page = -1时，传null,查询所有数据
     * 3、其他时候传正常值
     * @param pageInfo
     * @return
     */
    public static Map<String,Integer> getPageInfo(JSONObject pageInfo){
        Integer page = Constant.DEFAULT_PAGE;
        Integer size = Constant.DEFAULT_SIZE;
        if (pageInfo != null && pageInfo.size() > 0) {
            if(pageInfo.getInteger("page") != null && pageInfo.getInteger("page") == -1){
                page = null;
                size = null;
            } else {
                page = pageInfo.getInteger("page") != null ? pageInfo.getInteger("page") : page;
                size = pageInfo.getInteger("size") != null ? pageInfo.getInteger("size") : size;
            }
        }
        else {
            page = null;
            size = null;
        }
        Map<String,Integer> res = new HashMap<>();
        res.put("page", page);
        res.put("size", size);
        return res;
    }

    /**
     *处理排序信息
     * 默认按照 createTime 降序
     * 目前只支持单个字段排序
     */
    public static Sort getSortInfo(JSONObject sortJson){
        Sort sort = Sort.by(Sort.Direction.DESC, "createTime");
        if(sortJson != null && sortJson.getString("direction") != null && sortJson.getString("properties") != null){
            String direction = sortJson.getString("direction");
            sort = Sort.by(direction.equals("ASC") ? Sort.Direction.ASC : Sort.Direction.DESC, sortJson.getString("properties"));
        }
        return sort;
    }

    /**
     * 处理表名转为为外键
     * 规则：1、除去Tbl
     * 2、有Info结尾的去除
     * 3、首字母转化为小写
     * 4、加上ID
     */
    public static String dealTblName(String tblName){

        tblName = tblName.substring(3);
        if(tblName.contains("Info")){
            tblName = tblName.replace("Info","");
        }
        String resTblName = tblName.substring(0, 1).toLowerCase() + tblName.substring(1) + "Id";
        return resTblName;
    }

    /**
     * Integer类型的数组转化为String，用“，”分隔
     * @param iterator
     * @return
     */
    public static String iteratorToString(Iterable<Integer> iterator) {
        if (!iterator.iterator().hasNext()) {
            return "";
        }
        StringBuilder res = new StringBuilder();
        for(Integer num : iterator){
            res.append(num).append(Constant.SPLIT_OPERATOR.COMMA);
        }
        return res.substring(0, res.length() - 1);
    }

    /**
     * 用“,”分隔的String转化为Integer类型的数组
     * @param string
     * @return
     */
    public static List<Integer> stringToList(String string) {
        List<Integer> ints = new ArrayList<>();
        String[] strs = string.split(Constant.SPLIT_OPERATOR.COMMA);
        for (String str : strs) {
            ints.add(Integer.valueOf(str));
        }
        return ints;
    }
}

