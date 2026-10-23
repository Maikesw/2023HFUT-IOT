package com.ahaxt.competition.service.impl;

import java.util.regex.Matcher;
import java.util.regex.Pattern;
import com.ahaxt.competition.base.Base;
import com.ahaxt.competition.base.BaseResponse;
import com.ahaxt.competition.base.Constant;
import com.ahaxt.competition.entity.db.*;
import com.ahaxt.competition.repository.db.BaseUserDao;
import com.ahaxt.competition.repository.db.RelUserRoleDao;
import com.ahaxt.competition.service.IDataListService;
import com.ahaxt.competition.service.IUserService;
import com.ahaxt.competition.utils.*;
import com.alibaba.fastjson.JSONObject;
import org.apache.shiro.SecurityUtils;
import org.apache.shiro.authc.IncorrectCredentialsException;
import org.apache.shiro.authc.UnknownAccountException;
import org.apache.shiro.authc.UsernamePasswordToken;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Sort;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;
import org.springframework.util.StringUtils;
import org.springframework.web.multipart.MultipartFile;

import javax.annotation.Resource;
import java.util.*;
import java.util.stream.Collectors;

/**
 * @author hongzhangming
 */
@Service
@Transactional(rollbackFor = Exception.class)
public class UserServiceImpl extends Base implements IUserService {
    @Resource
    private BaseUserDao tblUserInfoDao;
    @Resource
    private RelUserRoleDao tblUserRoleRelDao;
    @Resource
    private IDataListService iDataListService;

    private static Pattern pattern = Pattern.compile("-?[0-9]+(\\\\.[0-9]+)?");




    private Integer getDepartmentId(JSONObject json) {
        Integer departmentId = json.getInteger("departmentId");
        if (departmentId == null) {
            Integer departmentId1 =  json.getInteger("departmentId1");
            String departmentId2 =  json.getString("departmentId2");
            String departmentId3 =  json.getString("departmentId3");
            Matcher m3 = pattern.matcher(departmentId3);
            if (m3.matches()) {
                departmentId = Integer.parseInt(departmentId3);
            } else {
                Matcher m2 = pattern.matcher(departmentId2);
                if (m2.matches()) {
                    JSONObject node = new JSONObject();
                    node.put("name", departmentId3);
                    node.put("parentId", departmentId2);
                    JSONObject nodeInfo = FastJsonUtil.toJson(iDataTreeService.editOneNode("BaseDepartment", node)).getJSONObject("nodeInfo");
                    departmentId = nodeInfo.getInteger("id");
                } else {
                    JSONObject node1 = new JSONObject();
                    node1.put("name", departmentId2.trim());
                    node1.put("parentId", departmentId1);
                    JSONObject nodeInfo1 = new JSONObject();
                    List<BaseDepartment> department1 = ((Page<BaseDepartment>)iCommonService.getSomeRecords("BaseDepartment",node1)).getContent();
                    if (department1.size()!=0) {
                        nodeInfo1 = FastJsonUtil.toJson(department1.get(0)).getJSONObject("nodeInfo");
                    } else {
                        nodeInfo1 = FastJsonUtil.toJson(iDataTreeService.editOneNode("BaseDepartment", node1)).getJSONObject("nodeInfo");
                    }
                    JSONObject node2 = new JSONObject();
                    node2.put("name", departmentId3.trim());
                    node2.put("parentId", nodeInfo1.getInteger("id"));
                    JSONObject nodeInfo2 = new JSONObject();
                    List<BaseDepartment> department2 = ((Page<BaseDepartment>)iCommonService.getSomeRecords("BaseDepartment",node2)).getContent();
                    if (department2.size()!=0) {
                        nodeInfo2 = FastJsonUtil.toJson(department2.get(0)).getJSONObject("nodeInfo");
                    } else {
                        nodeInfo2 = FastJsonUtil.toJson(iDataTreeService.editOneNode("BaseDepartment", node2)).getJSONObject("nodeInfo");
                    }
                    departmentId = nodeInfo2.getInteger("id");
                }
            }
        }
        return departmentId;
    }

    /**
     * 用户注册
     */
    @Override
    public Object register(JSONObject json) {
        //首先校验密码
//        if (!EncodeUtil.isStrongPwd(password)) {
//            throw BaseResponse.moreInfoError.error("弱密码(应包含大小写字母、特殊符号及数字且长度大于8位)");
//        }
        //查重
        String account = json.getString("account");
        String phone = json.getString("phone");
        JSONObject searchKeys = new JSONObject();
        searchKeys.put("account",account);
        List<BaseUser> userList = ((Page<BaseUser>)iCommonService.getSomeRecords("BaseUser",searchKeys)).getContent();
        if (userList.size() != 0) {
            throw BaseResponse.moreInfoError.error("用户已存在");
        }
        searchKeys.clear();
        searchKeys.put("phone", phone);
        userList = ((Page<BaseUser>)iCommonService.getSomeRecords("BaseUser",searchKeys)).getContent();
        if (userList.size()!=0) {
            throw BaseResponse.moreInfoError.error("用户已存在");
        }
        //注册
        String password = json.getString("password");
        String name = json.getString("name");
        Integer departmentId = getDepartmentId(json);
        Integer jobId = json.getInteger("jobId");

        JSONObject userInfo=new JSONObject();
        userInfo.put("phone", phone.trim());
        userInfo.put("account",account.trim());
//        userInfo.put("registerTime",new Date());
        if (name!=null) {
            userInfo.put("name", name.trim());
        }
        if (departmentId != null) {
            userInfo.put("departmentId", departmentId);
        }
        if (jobId != null) {
            userInfo.put("jobId", jobId);
        }
        // 先新增
        BaseUser userEntity = (BaseUser)iCommonService.saveOneRecord("BaseUser",userInfo);
        //再加密
        userInfo.put("id", userEntity.getId());
        userInfo.put("password",EncodeUtil.pwdShiro(password.trim(), userEntity.getId()));
        //再保存
        logger.info("<INFO> 用户注册 phone:{} account:{} password:{}", phone, userInfo.getString("account"), userInfo.getString("password"));
        return iCommonService.saveOneRecord("BaseUser",userInfo);
    }


    /**
     * 登录
     * @param account
     * @param password
     * @param rememberMe
     */
    @Override
    public void login(String account, String password, Boolean rememberMe) {
        try {
            // 查用户 isDeleted=false  && (username == account || phone == account)
            JSONObject searchKeys = new JSONObject();
            searchKeys.put("account",account);
            List<BaseUser> userInfo = ((Page<BaseUser>)iCommonService.getSomeRecords("BaseUser",searchKeys)).getContent();
            if (userInfo.size()==0) {
                searchKeys.clear();
                searchKeys.put("phone",account);
                userInfo = ((Page<BaseUser>)iCommonService.getSomeRecords("BaseUser",searchKeys)).getContent();
                if (userInfo.size()==0) {
                    throw new UnknownAccountException();
                }
            }
            SecurityUtils.getSubject().login(new UsernamePasswordToken(userInfo.get(0).getId().toString(), password, rememberMe));
        } catch (UnknownAccountException e) {
            throw BaseResponse.moreInfoError.error("用户不存在");
        } catch (IncorrectCredentialsException e) {
            throw BaseResponse.moreInfoError.error("密码错误！");
        } catch (Exception e) {
            LogUtil.error(logger, e);
            throw BaseResponse.notCaptured.error();
        }
    }
    /**
     * 修gai密码
     * @param password
     */
    @Override
    public void editPassword(String userId, String password) {

        JSONObject obj = FastJsonUtil.toJson(iCommonService.getOneRecordById("BaseUser",Integer.parseInt(userId)));
        obj.put("Password",EncodeUtil.pwdShiro(password.trim(), userId));
        iCommonService.saveOneRecord("BaseUser",obj);
    }


    @Override
    public Object userList() {
        return tblUserInfoDao.findByIsDeletedFalse();
    }

    @Override
    public void deleteUser(String phone) {
        List<BaseUser> userInfo = tblUserInfoDao.findByPhoneAndIsDeletedFalse(phone);
        userInfo.forEach(u -> iCommonService.deleteRecordByDelflag(USER_INFO, u.getId()));
    }

    @Override
    public Object userRoleSet(Integer[] roleSet, Integer userId) {
        //通过userId查找出对应的 UserRoleRel Ids,然后删除
        JSONObject searchKeys = new JSONObject();
        searchKeys.put("userId",userId);
        Object oldUserRoleSet = iCommonService.getSomeRecords("RelUserRole",searchKeys);
        ArrayList<Integer> oldUserRoleIds = new ArrayList<>();
        for(Object obj:((Page<Object>)oldUserRoleSet).getContent()){
            oldUserRoleIds.add(FastJsonUtil.toJson(obj).getIntValue("id"));
        }
        tblUserRoleRelDao.deleteAll(tblUserRoleRelDao.findByIdInAndIsDeletedFalse(oldUserRoleIds));
        //保存新的记录
        List<RelUserRole> userRoleRels = new ArrayList<>();
        Arrays.stream(roleSet).forEach(integer -> {
            RelUserRole userRoleRel = new RelUserRole();
            userRoleRel.setRoleId(integer);
            userRoleRel.setUserId(userId);
            userRoleRels.add(userRoleRel);
        });
        return tblUserRoleRelDao.saveAll(userRoleRels);
    }

    /**
     * 获取所有未删除的用户信息，并添加所用所拥有的roleIds
     * @param pageInfo
     * @param sortJson
     * @return
     */
    @Override
    public Object getUserListIsNotDelete(JSONObject pageInfo, JSONObject sortJson) {
        Sort sort = GeneralUtil.getSortInfo(sortJson);
        Object res = iDataListService.getSomeRecords(USER_INFO, null, null, sort, pageInfo.getInteger("page"), pageInfo.getInteger("size"));
        JSONObject resJson = FastJsonUtil.toJson(res);
        for(Object obj : resJson.getJSONArray("content")){
            int userId = FastJsonUtil.toJson(obj).getInteger("id");
            //按userId查找TblUserRoleRel
            JSONObject searchKeys = new JSONObject();
            searchKeys.put("userId", userId);
            JSONObject pagInfo2 = new JSONObject();
            pagInfo2.put("page", -1);
            pagInfo2.put("size", 10);
            Object obj1 = iDataListService.getSomeRecords("RelUserRole", searchKeys, null, null, GeneralUtil.getPageInfo(pagInfo2).get("page"), GeneralUtil.getPageInfo(pagInfo2).get("size"));
            Set<Integer> roleIds = new HashSet<>();
            for(Object obj2: (ArrayList)obj1){
                roleIds.add(FastJsonUtil.toJson(obj2).getInteger("roleId"));
            }
            ((JSONObject)obj).put("roleIds",roleIds);
        }
        return resJson;
    }

    /**
     * 编辑用户信息的内容
     * @param json
     * @param userId
     * @return
     */
    @Override
    public Object editUserInfo( JSONObject json, Integer userId){
        BaseUser userInfo = json.toJavaObject(BaseUser.class);
        // 更新redis缓存的用户信息
        String key = APPLICATION_NAME + ":userInfo:" + userInfo.getId();
        redis.delete(key);
        //返回给前端头像的headImage 和fileId
        JSONObject searchKey = new JSONObject();
        searchKey.put("relationId",getLoginUserId());
        searchKey.put("type",1);
        List<SysOssFile> tblOssFileInfos = ((Page<SysOssFile>)iCommonService.getSomeRecords(OSS_FILE_INFO, searchKey)).getContent();
        SysOssFile tblOssFileInfo;
        if(tblOssFileInfos.size()!=0){
            tblOssFileInfo=tblOssFileInfos.get(0);
            // userInfo.setHeadImage(ossUtil.getUrl(tblOssFileInfo.getId()));
            userInfo.setOssFileId(tblOssFileInfo.getId());
        }
        tblUserInfoDao.save(userInfo);
        return userInfo;
    }

    /**
     * @param tblName
     * @param sort
     * @param page
     * @param size
     * @Des 用户获取List
     * @Author yukai
     * @Date 2020/12/10 16:14
     */
    @Override
    public Object tblUserGetList(String tblName, JSONObject searchKeys, Map repMap, Sort sort, Integer page, Integer size) {
        Object res = iCommonService.getSomeRecords(tblName, searchKeys, repMap, sort, page, size);
        JSONObject resJson = FastJsonUtil.toJson(res);
        JSONObject searchKey2 = new JSONObject();
        searchKey2.put("isDeleted", false);
        List<BaseJobPosition> jobInfoList = ((Page<BaseJobPosition>)iCommonService.getSomeRecords("BaseJobPosition",searchKey2)).getContent();
        List<BaseDepartment> departmentInfoList = ((Page<BaseDepartment>)iCommonService.getSomeRecords("BaseDepartment",searchKey2)).getContent();
        Map<Integer, String> jobMap = jobInfoList.stream().collect(Collectors.toMap(BaseJobPosition::getId, BaseJobPosition::getName));
        Map<Integer, String> departmentMap = departmentInfoList.stream().collect(Collectors.toMap(BaseDepartment::getId, BaseDepartment::getName));
        for(Object obj : resJson.getJSONArray("content")) {
            int userId = FastJsonUtil.toJson(obj).getInteger("id");
            //按userId查找TblUserRoleRel
            JSONObject searchKey1 = new JSONObject();
            searchKey1.put("userId", userId);
            JSONObject pagInfo2 = new JSONObject();
            Object obj1 = ((Page<Object>)iCommonService.getSomeRecords("RelUserRole", searchKey1)).getContent();
            Set<Integer> roleIds = new HashSet<>();
            for(Object obj2: (ArrayList)obj1){
                roleIds.add(FastJsonUtil.toJson(obj2).getInteger("roleId"));
            }
            ((JSONObject)obj).put("roleIds",roleIds);

            List<SysRole> roleInfos = (List<SysRole>) iCommonService.getRecordsByIds("SysRole", roleIds, false);
            StringBuilder role = new StringBuilder();
            for (SysRole roleInfo : roleInfos) {
                role.append(roleInfo.getName()).append("&");
            }
            role = new StringBuilder(role.substring(0, role.length() - 1));
            ((JSONObject)obj).put("role",role);

            Integer jobId =  FastJsonUtil.toJson(obj).getInteger("jobId");
            if (jobId!=null){
                ((JSONObject)obj).put("job",jobMap.get(jobId));
            }
            Integer departmentId =  FastJsonUtil.toJson(obj).getInteger("departmentId");
            if (departmentId!=null){
                ((JSONObject)obj).put("departmentName",jobMap.get(jobId));
            }
        }
        Object rawRet = resJson;
        return rawRet;
    }

    /**
     * @param tblName
     * @param sort
     * @param page
     * @param size
     * @Des 用户获取List
     * @Author yukai
     * @Date 2020/12/10 16:14
     */
    @Override
    public Object ViewUserGetList(String tblName, JSONObject searchKeys, Map repMap, Sort sort, Integer page, Integer size) {
        if(searchKeys.containsKey("roleIds")&&repMap.containsKey("roleIds")){
            JSONObject roleSearchKey = new JSONObject();
            Map roleMap = new HashMap();
            roleSearchKey.put("roleId",searchKeys.getString("roleIds"));
            roleMap.put("roleId",repMap.get("roleIds"));
            List<RelUserRole> userRoleRelList = ((Page<RelUserRole>)iCommonService.getSomeRecords("RelUserRole", roleSearchKey, roleMap, Sort.unsorted())).getContent();
            Set<Integer> userIdSet = userRoleRelList.stream().map(RelUserRole::getUserId).collect(Collectors.toSet());
            StringBuilder stringBuilder = new StringBuilder();
            for (Integer userId : userIdSet) {
                stringBuilder.append(userId).append(",");
            }
            searchKeys.put("id",stringBuilder.substring(0,stringBuilder.length()-1));
            repMap.put("id","()");
            searchKeys.remove("roleIds");
            repMap.remove("roleIds");
        }
        Object res = iCommonService.getSomeRecords(tblName, searchKeys, repMap, sort, page, size);
        JSONObject resJson = FastJsonUtil.toJson(res);
        for(Object obj : resJson.getJSONArray("content")) {
            Set<Integer> roleIds = new HashSet<>();
            String[] str = FastJsonUtil.toJson(obj).getString("roleIds").split(SPLIT_OPERATOR.COMMA);
            for(String c:str){
                roleIds.add(Integer.parseInt(c));
            }
            ((JSONObject)obj).put("roleIds",roleIds);
        }
        return resJson;
    }

    /**
     * 用户上传头像
     * @param file
     * @return
     */
    @Override
    public Object uploadAvatar( MultipartFile file ){
//        SysOssFile fileInfo = ossUtil.upload(HEAD_IMAGE, FileUtil.toFile(file),getLoginUserId());
//        // 删除以前的图片
//         ossUtil.delete(((BaseUser)getLoginUser()).getOssFileId());
//        String url = ossUtil.getUrl(fileInfo.getId());
//        fileInfo.setUrl(url);
//        return  fileInfo;
        return null;
    }
    @Override
    public Object getUserRoles(String userId) {
        JSONObject searchKey = new JSONObject();
        searchKey.put("userId", userId);
        List<RelUserRole> relations = ((Page<RelUserRole>) iCommonService.getSomeRecords("RelUserRole", searchKey)).getContent();
        searchKey.clear();
        Set<Integer> RoleIdSet = relations.stream().map(RelUserRole::getRoleId).collect(Collectors.toSet());
        searchKey.put("id", StringUtils.collectionToDelimitedString(RoleIdSet, ","));
        Map<String, String> repMap=new HashMap();
        repMap.put("id", Constant.IN);
        List<SysRole> roles =((Page<SysRole>)iCommonService.getSomeRecords("SysRole", searchKey, repMap)).getContent();
        return roles;
    }
    @Override
    public Object saveUserRoles(String userId, Integer[] roleIds) {
        // 先查看下有否修改
        JSONObject searchKey = new JSONObject();
        searchKey.put("userId", userId);
        List<RelUserRole> relations = ((Page<RelUserRole>) iCommonService.getSomeRecords("RelUserRole", searchKey)).getContent();
        searchKey.clear();
        List<Integer> oldIds = relations.stream().map(RelUserRole::getId).collect(Collectors.toList());
        Set<Integer> oldRoleIdSet = relations.stream().map(RelUserRole::getRoleId).collect(Collectors.toSet());
        Set<Integer> roleIdSet = Arrays.stream(roleIds).collect(Collectors.toSet());
        // 如果有修改
        if (!oldRoleIdSet.equals(roleIdSet)) {
            //旧的有新的没有，删除
            for (int i=0;i<relations.size();i++) {
                if (!roleIdSet.contains(relations.get(i).getRoleId())) {
                    iCommonService.deleteRecordByDelflag("RelUserRole",relations.get(i).getId());
                }
            }
            //旧的没有新的有，新增
            List<RelUserRole> userRoleRels = new ArrayList<>();
            for (int i=0;i<roleIdSet.size();i++) {
                if (!oldRoleIdSet.contains(roleIdSet.toArray()[i])) {
                    RelUserRole userRoleRel = new RelUserRole();
                    userRoleRel.setRoleId((Integer)roleIdSet.toArray()[i]);
                    userRoleRel.setUserId(Integer.valueOf(userId));
                    if ((Integer)roleIdSet.toArray()[i] == ROLE_TABLE.UNIVERSITY_ADMIN) {
                        userRoleRel.setIsAudit(AUDIT_STATUS.SAVE);
                    }
                    else {
                        userRoleRel.setIsAudit(AUDIT_STATUS.PASS);
                    }
                    userRoleRels.add(userRoleRel);
                }
            }
            Object res = iCommonService.saveSomeRecords("RelUserRole", userRoleRels);
            return res;
        } else {
            return null;
        }
    }


    @Override
    public Object getDepartment(String parentId) {
        JSONObject searchKey = new JSONObject();
        Matcher m = pattern.matcher(parentId);
        if (m.matches()) {
            searchKey.put("parentId", parentId);
        } else {
            searchKey.put("parentId", 0);
        }
        return iCommonService.getSomeRecords("BaseDepartment", searchKey,null, Sort.by(Sort.Direction.ASC, "theOrder"));

    }


}
