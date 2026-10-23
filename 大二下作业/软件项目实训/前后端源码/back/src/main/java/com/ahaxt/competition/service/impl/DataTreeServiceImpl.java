package com.ahaxt.competition.service.impl;

import com.ahaxt.competition.base.Base;
import com.ahaxt.competition.base.BaseResponse;
import com.ahaxt.competition.entity.base.BaseTreeInfo;
import com.ahaxt.competition.service.IDataTreeService;
import com.ahaxt.competition.utils.DateUtil;
import com.ahaxt.competition.utils.FastJsonUtil;
import com.alibaba.fastjson.JSONArray;
import com.alibaba.fastjson.JSONObject;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Sort;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;
import org.springframework.util.StringUtils;

import java.util.ArrayList;
import java.util.Date;
import java.util.List;
import java.util.Map;
import java.util.stream.Collectors;

/**
 * @author hongzhangming
 */
@Service
@Transactional(rollbackFor = Exception.class)
public class DataTreeServiceImpl extends Base implements IDataTreeService {

    //树接口多结果查询排序默认按theOrder
    private final Sort orderSort = Sort.by(Sort.Direction.ASC, "theOrder");

    //region 一些private
    public List<Object> getSubNodes(Object wholeObj, Integer parentId, Boolean lazy, String preName) {
        return privGetSubNodes(wholeObj, parentId, lazy, preName);
    }
    private List<Object> privGetSubNodes(Object wholeObj, Integer parentId, Boolean lazy, String preStr) {
        List<Object> tree = new ArrayList<>();
        List<BaseTreeInfo<Object>> res = (List<BaseTreeInfo<Object>>) wholeObj;
        for (BaseTreeInfo<Object> nowObj : res) {
            if (nowObj.getParentId().equals(parentId)) {
                String nowStr = "";
                if (!preStr.equals("")) {
                    nowStr = preStr + "-" + nowObj.getName();
                } else {
                    nowStr = nowObj.getName();
                }
                nowObj.setAllNodeNames(nowStr);
                if (lazy) {
                    nowObj.setHasChildren(!nowObj.getIsLeaf());
                }
                else {
                    if (nowObj.getIsLeaf().equals(false)) {
                        nowObj.setChildren(privGetSubNodes(wholeObj, nowObj.getId(), lazy, nowStr));
                    }
                }
                //nowObj转换json过程中自动去除值为null的字段，这样最后一级就没有children字段了
                tree.add(FastJsonUtil.toJson(nowObj));
            }
        }
        return tree;
    }
    private void getArraySubNodes(ArrayList<Object> tree, List<BaseTreeInfo<Object>> wholeObj, Integer parentId) {
        privGetArraySubNodes(tree, wholeObj, parentId, "");
    }
    private void privGetArraySubNodes(ArrayList<Object> tree, List<BaseTreeInfo<Object>> wholeObj, Integer parentId, String preStr) {
        ArrayList<BaseTreeInfo<Object>> res = (ArrayList<BaseTreeInfo<Object>>) wholeObj;
        for (BaseTreeInfo<Object> nowObj : res) {
            if (nowObj.getParentId().equals(parentId)) {
                String nowStr = "";
                if (preStr != "") {
                    nowStr = preStr + "/" + nowObj.getName();
                } else {
                    nowStr = nowObj.getName();
                }
                nowObj.setAllNodeNames(nowStr);
                // nowObj.setHasChildren(nowObj.getIsLeaf());
                tree.add(nowObj);
                if (nowObj.getIsLeaf().equals(false)) {
                    privGetArraySubNodes(tree, wholeObj, nowObj.getId(), nowStr);
                }
            }
        }
    }
    //endregion

    /**
     * 查询树结构
     * @return
     */
    @Override
    public Object getTreeList(String tblName, Boolean virtualRootFlag, JSONObject searchKey, Boolean lazy, String preName, Integer parentId,Sort sortJson) {
        if (lazy) {
            if ( searchKey == null) {
                searchKey = new JSONObject();
            }
            if (!searchKey.containsKey("parentId")) {
                searchKey.put("parentId", parentId);
            }
        }
        List<BaseTreeInfo<Object>> array = new ArrayList<>();
        //array = ((Page<BaseTreeInfo<Object>>) iCommonService.getSomeRecords(tblName, searchKey,null, orderSort)).getContent();
        array = ((Page<BaseTreeInfo<Object>>) iCommonService.getSomeRecords(tblName, searchKey,null, sortJson)).getContent();
        List<Object> tree = new ArrayList<>();
        BaseTreeInfo<Object> root = new BaseTreeInfo<>();
        if (array.size() != 0) {
            if(!virtualRootFlag) {
                tree = getSubNodes(array, parentId, lazy, preName);
            } else {
                root.setName("全部");
                root.setId(-1);
                root.setChildren(getSubNodes(array, parentId, lazy, preName));
                tree.add(root);
            }
        }
        return tree;
    }
    @Override
    public Object getTreeArrayList(String tblName, JSONObject searchKey) {
        if (searchKey == null) {
            searchKey = new JSONObject();
        }
        List<BaseTreeInfo<Object>> array = ((Page<BaseTreeInfo<Object>>) iCommonService.getSomeRecords(tblName, searchKey,null, orderSort)).getContent();
        ArrayList<Object> tree = new ArrayList<>();
        if (array.size() != 0) {
            getArraySubNodes(tree, array, -1);
        }
        return tree;
    }

    /**
     * 交换两个节点顺序。上移/下移按钮用到，修改TheOrder
     * @param tblName
     * @param nodeId
     * @param nodeChangeId
     * @return
     */
    @Override
    public Object changeTwoNodes(String tblName, int nodeId, int nodeChangeId) {
        JSONObject obj1 = FastJsonUtil.toJson(iCommonService.getOneRecordById(tblName, nodeId));
        JSONObject obj2 = FastJsonUtil.toJson(iCommonService.getOneRecordById(tblName, nodeChangeId));
        //step1判断
        //todo:未判空
        if(!obj1.getInteger("parentId").equals(obj2.getInteger("parentId"))){
            //处理
            throw BaseResponse.moreInfoError.error("节点交换失败，parentId不一致");
        } else {
            int order1 = obj1.getInteger("theOrder");
            int order2 = obj2.getInteger("theOrder");
            obj1.put("theOrder", order2);
            obj2.put("theOrder", order1);
            iCommonService.saveOneRecord(tblName, obj1);
            iCommonService.saveOneRecord(tblName, obj2);
        }
        return null;
    }

    /**
     * 编辑/新增节点。如果node中的ID=0就是新增；否则就是修改。
     * 新增操作需要修改父节点的childNum和isLeaf
     * @param tblName
     * @param node
     * @return
     */
    @Override
    public Object editOneNode(String tblName, JSONObject node) {
        JSONObject retuInfo = new JSONObject();
        Integer nodeId = node.getInteger("id");
        int parentNodeId = node.getInteger("parentId");
        if (StringUtils.isEmpty(nodeId) || nodeId == 0) {
            if(parentNodeId == -1) {
                node.put("theLevel", 1);
            } else {
                node.put("theLevel", FastJsonUtil.toJson(iCommonService.getOneRecordById(tblName, parentNodeId)).getInteger("theLevel") + 1);
            }
            //获取最大值theOrder, +1后加入
            JSONObject searchKey = new JSONObject();
            searchKey.put("parentId", parentNodeId);
            Object rawRet = iCommonService.getSomeRecords(tblName, searchKey,null, Sort.by(Sort.Direction.DESC, "theOrder"), 1, 1,true);
            if(FastJsonUtil.toJson(rawRet).getJSONArray("content").size() == 0) {
                node.put("theOrder",1);
            } else {
                JSONObject maxOrderObj = FastJsonUtil.toJson(rawRet).getJSONArray("content").getJSONObject(0);
                node.put("theOrder",maxOrderObj.getIntValue("theOrder") + 1);
            }
            node.put("updateTime",DateUtil.format(new Date(),"yyyy-MM-dd HH:mm:ss"));
            node.put("createTime",DateUtil.format(new Date(),"yyyy-MM-dd HH:mm:ss"));
            node.put("childNum", 0);
            node.put("isLeaf", 1);
            Object objAdd = iCommonService.saveOneRecord(tblName, node);
            /**新增后更新父节点参数,childNum和isLeaf
             * 如果是根节点，则无父节点，无需更新
             * */
            if(parentNodeId != -1) {
                Object objParent = iCommonService.getOneRecordById(tblName, parentNodeId);
                JSONObject json = FastJsonUtil.toJson(objParent);
                Integer childNum = (Integer) json.get("childNum");
                json.put("childNum", ++childNum);
                //新增后，children节点不为0，则标记为非叶子节点
                json.put("isLeaf", 0);
                iCommonService.saveOneRecord(tblName, json);
            }
            retuInfo.put("operating", "addOneNode");
            retuInfo.put("nodeInfo", FastJsonUtil.toJson(objAdd));
        } else {
            iCommonService.saveOneRecord(tblName, node);
            retuInfo.put("operating", "updateOneNode");
            retuInfo.put("nodeInfo", node);
        }
        return retuInfo;
    }

    /**
     * 删除节点。注意还需要修改上级节点相关信息
     * 如果这里数据库字段有问题，直接修改数据库
     * @param tblName
     * @param node
     * @return
     */
    @Override
    public Object delOneNode(String tblName, JSONObject node) {
        int nodeId = node.getInteger("id");
        int parentNodeId = node.getInteger("parentId");
        iCommonService.deleteRecordByDelflag(tblName, nodeId);
        //此处有个问题，上面那个语句，删除一个不存在的节点返回时null，正常删除一个节点也返回null
        //删除一个不存在的节点时，下面这个语句时不用执行的
        //即流程可能要优化为先查再删
        if (parentNodeId != -1) {
            Object obj = iCommonService.getOneRecordById(tblName, parentNodeId);
            JSONObject json = FastJsonUtil.toJson(obj);
            Integer childNum = (Integer) json.get("childNum");
            childNum = Math.max(0,childNum - 1);
            json.put("childNum", childNum);
            if (childNum == 0) {
                json.put("isLeaf", 1);
            }
            iCommonService.saveOneRecord(tblName, json);
        }
        return null;
    }

    /**
     * 删除一个节点-包括非叶子节点
     * @param tblName
     * @param node
     * @return
     */
    public Object delOneNodeIncludeNoLeaf(String tblName, JSONObject node) {
        //首先判断是否是非叶子节点
//        if(!node.getBoolean("isLeaf")){
            ArrayList<JSONObject> children = new ArrayList<>();
            getChildren(node,tblName,children);
            iCommonService.deleteSomeRecords(tblName, children.stream().map(p->p.getInteger("id")).collect(Collectors.toList()));
//        }else{
//            int nodeId = node.getInteger("id");
//            iCommonService.deleteRecordByDelflag(tblName, nodeId);
//        }

        int parentNodeId = node.getInteger("parentId");
        //此处有个问题，上面那个语句，删除一个不存在的节点返回时null，正常删除一个节点也返回null
        //删除一个不存在的节点时，下面这个语句时不用执行的
        //即流程可能要优化为先查再删
        if (parentNodeId != -1) {
            Object obj = iCommonService.getOneRecordById(tblName, parentNodeId);
            JSONObject json = FastJsonUtil.toJson(obj);
            Integer childNum = (Integer) json.get("childNum");
            childNum = Math.max(0,childNum - 1);
            json.put("childNum", childNum);
            if (childNum == 0) {
                json.put("isLeaf", 1);
            }
            iCommonService.saveOneRecord(tblName, json);
        }
        return null;
    }

    @Override
    public Object delManyNodes(String tblName, JSONArray nodes) {
        List<BaseTreeInfo> treeNodes = nodes.toJavaList(BaseTreeInfo.class);
        treeNodes.sort(((o1, o2) -> o2.getTheLevel() -o1.getTheLevel()));
        //暂时这么做，后续优化下算法，减少下时间复杂度
        for(BaseTreeInfo nodeInfo : treeNodes){
            delOneNodeIncludeNoLeaf(tblName , FastJsonUtil.toJson(nodeInfo));
        }
        return null;
    }

    /**
     * 返回当前节点和其所有父节点。循环返回直至到根节点，即 parentId = -1;
     * @param tblName
     * @param nodeId
     * @return
     */
    @Override
    public Object getAllParentIndex(String tblName, Integer nodeId) {
        if (nodeId > 0) {
            List<Object> list = new ArrayList<>();
            Object obj = iCommonService.getOneRecordById(tblName, nodeId, true);
            list.add(obj);
            JSONObject json = FastJsonUtil.toJson(obj);
            Integer parentId = (Integer) json.get("parentId");
            while (parentId != -1) {
                obj = iCommonService.getOneRecordById(tblName, parentId, true);
                json = FastJsonUtil.toJson(obj);
                list.add(obj);
                parentId = (Integer) json.get("parentId");
            }
            return list;
        }
        else {
            return null;
        }
    }

    /**
     * 返回当前节点直接父节点信息
     * @param tblName
     * @param nodeId
     * @return
     */
    @Override
    public Object getNearestParent(String tblName, Integer nodeId) {
        Object objTemp = null;
        if(nodeId == 0) {
            return objTemp;
        }
        try{
            objTemp = iCommonService.getOneRecordById(tblName, nodeId);
            JSONObject json = FastJsonUtil.toJson(objTemp);
            Integer parentId =  (Integer)json.get("parentId");
            objTemp = iCommonService.getOneRecordById(tblName, parentId);
            JSONObject searchkey = new JSONObject();
            Object rawRet = ((Page<Object>) iCommonService.getSomeRecords(tblName, searchkey,null,orderSort)).getContent();
            if (rawRet != null) {
                ((BaseTreeInfo) objTemp).setChildren(getSubNodes(rawRet, parentId, false, ""));
            }
        }catch (Exception e){
            logger.error("返回当前节点直接父节点信息失败",e);
        }
        return objTemp;
    }

    /**
     * 获取当前节点的父亲根节点，即 parentId=-1;
     * @param tblName
     * @param nodeId
     * @param theLevel 如果有值，找到就结束
     * @return
     */
    @Override
    public Object getFirstParent(String tblName, Integer nodeId, Integer theLevel) {
        Object objTemp = null;
        if(nodeId == 0){
            return objTemp;
        }
        //循环获取父节点id，直至获取到根节点，即parentId == -1;
        try{
            while(nodeId != null && nodeId != -1){
                objTemp = iCommonService.getOneRecordById(tblName, nodeId);
                nodeId = (int)FastJsonUtil.toJson(objTemp).get("parentId");
                if ((theLevel != null) && (theLevel == (int)FastJsonUtil.toJson(objTemp).get("theLevel"))) {
                    break;
                }
            }
        }catch (Exception e){
            logger.error(" 获取当前节点的父亲根节点失败",e);
        }
        return objTemp;
    }

    /**
     * 获取所有的兄弟节点
     * @param tblName
     * @param nodeId
     * @return
     */
    @Override
    public Object getAllBrotherIndex(String tblName, Integer nodeId) {
        Object listObject = null;
        try{
            JSONObject json = FastJsonUtil.toJson(iCommonService.getOneRecordById(tblName, nodeId));
            Integer parentId =  (Integer)json.get("parentId");
            JSONObject searchKey = new JSONObject();
            searchKey.put("parentId", parentId);
            listObject = ((Page<Object>)iCommonService.getSomeRecords(tblName, searchKey, null, orderSort)).getContent();
        }catch (Exception e){
            logger.error("返回当前节点直接父节点信息失败", e);
        }
        return listObject;
    }

    @Override
    public ArrayList<JSONObject> getAllChildren(JSONObject treeInfo) {
        ArrayList<JSONObject> treeNodeIds = new ArrayList<>();
        String treeTblName = FastJsonUtil.toJson(treeInfo).getString("treeKeyWords");
        int treeNodeId = FastJsonUtil.toJson(treeInfo).getInteger("treeNodeId");
        Object nodeInfo = iCommonService.getOneRecordById(treeTblName, treeNodeId);
        getChildren(FastJsonUtil.toJson(nodeInfo), treeTblName, treeNodeIds);
        return treeNodeIds;
    }



    /**
     * 树结构关联查询列表结构
     * @param treeInfo
     * @param listTblName
     * @return 返回所有属于treeNodeId及其子节点的列表元素
     */
    @Override
    public Object getTreeJointList(JSONArray treeInfo, String listTblName, JSONObject searchKey, Map<String ,String> regMap, Sort sort, Integer page, Integer size){
//        ArrayList<Integer> treeNodeIds = new ArrayList<>();
//        if (treeInfo == null) {
//            return null;
//        }
//        for(Object tree : treeInfo){
//            String treeTblName = FastJsonUtil.toJson(tree).getString("treeKeyWords");
//            int treeNodeId = FastJsonUtil.toJson(tree).getInteger("treeNodeId");
//
//            //step1 :获取当前树节点及其子节点
//            Object nodeInfo = iCommonService.getOneRecordById(treeTblName, treeNodeId);
//            getChildren(FastJsonUtil.toJson(nodeInfo), treeTblName, treeNodeIds);
//            //step2: 获取treeTblNameId在allTreeNodeIds数组中的列表元素
//            String resTreeTblName = GeneralUtil.dealTblName(treeTblName);
//            searchKey.put(resTreeTblName, GeneralUtil.iteratorToString(treeNodeIds));
//            regMap.put(resTreeTblName, Constant.IN);
//        }
//        Object res = iDataListService.getSomeRecords(listTblName, searchKey, regMap, sort, page, size);
//        return res;
        return null;
    }

    //获取当前节点及其子节点Id,放在treeNodeIds中
    public void getChildren(JSONObject nodeInfo, String treeTblName, ArrayList<JSONObject> treeNodeIds){
        treeNodeIds.add(nodeInfo);
        int curNodeId = nodeInfo.getInteger("id");
        if(!nodeInfo.getBoolean("isLeaf")){
            JSONObject searchKey = new JSONObject();
            searchKey.put("parentId", curNodeId);
            Object raw = ((Page<Object>)iCommonService.getSomeRecords(treeTblName, searchKey)).getContent();
            for(Object obj : (List<Object>)raw){
                getChildren(FastJsonUtil.toJson(obj),treeTblName, treeNodeIds);
            }
        }
    }

    public String idToParentName(String tblName, Integer nodeId){
        Object obj = iCommonService.getOneRecordById(tblName, nodeId);
        JSONObject json = FastJsonUtil.toJson(obj);
        Integer parentId = json.getInteger("parentId");
        String name =json.getString("name");
        while (parentId != -1) {
            obj = iCommonService.getOneRecordById(tblName, parentId);
            json = FastJsonUtil.toJson(obj);
            name=json.getString("name");
        }
        return name;
    }

}
