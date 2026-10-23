package com.ahaxt.competition.base;

import com.ahaxt.competition.entity.db.*;
import com.ahaxt.competition.repository.db.BaseDepartmentDao;
import com.ahaxt.competition.repository.db.BaseUserDao;
import com.ahaxt.competition.repository.db.SysOssFileDao;
import com.ahaxt.competition.repository.db.SysRoleDao;
import com.ahaxt.competition.service.ICommonService;
import com.ahaxt.competition.service.IDataListService;
import com.ahaxt.competition.service.IDataTreeService;
import com.ahaxt.competition.service.IUserService;
import com.ahaxt.competition.service.impl.DataTreeServiceImpl;
import com.ahaxt.competition.utils.FastJsonUtil;
import com.ahaxt.competition.utils.RedisUtil;
import com.ahaxt.competition.utils.TreeUtil;
import com.alibaba.fastjson.JSONObject;
import org.apache.shiro.SecurityUtils;
import org.hibernate.query.criteria.internal.CriteriaBuilderImpl;
import org.hibernate.query.criteria.internal.predicate.CompoundPredicate;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.context.annotation.Configuration;
import org.springframework.data.domain.Page;
import org.springframework.data.jpa.domain.Specification;
import org.springframework.util.StringUtils;
import org.springframework.web.bind.annotation.CrossOrigin;

import javax.annotation.Resource;
import javax.persistence.PersistenceContext;
import javax.persistence.criteria.CriteriaBuilder;
import javax.persistence.criteria.Predicate;
import javax.persistence.criteria.Root;
import java.lang.reflect.Field;
import java.util.*;
import java.util.stream.Collectors;

/**
 * @author hongzhangming
 */
@Configuration
@PersistenceContext
@CrossOrigin(allowCredentials = "true", allowedHeaders = "*")
public class Base extends Constant {
    protected Logger logger = LoggerFactory.getLogger(this.getClass());
    @Resource
    protected RedisUtil redis;
    @Resource
    protected ICommonService iCommonService;
    @Resource
    protected IDataTreeService iDataTreeService;
    @Resource
    protected DataTreeServiceImpl dataTreeServiceImpl;
    @Resource
    protected BaseUserDao tblUserInfoDao;
    @Resource
    protected SysRoleDao tblRoleInfoDao;
    @Resource
    protected BaseDepartmentDao tblDepartmentInfoDao;
    @Resource
    protected IDataListService iDataListService;
    @Resource
    protected IUserService iUserService;

    @Resource
    protected SysOssFileDao sysOssFileDao;


    public void loggerRecord(String action, JSONObject obj) {
        JSONObject json = new JSONObject();
        json.put("userId", getLoginUserId());
        json.put("action", action);
        if (obj.toJSONString().length()>1000) {
            json.put("detail", obj.toJSONString().substring(0, 1000));
        } else {
            json.put("detail", obj.toJSONString());
        }
        iCommonService.saveOneRecord("SysLogger", json);
    }

    /**
     * 当前登录用户编号
     *
     * @return
     */
    public Integer getLoginUserId() {
        try {
            return Integer.parseInt(SecurityUtils.getSubject().getPrincipal().toString(), 10);
        } catch (NullPointerException e) {
            throw BaseResponse.moreInfoError.error("用户Id获取失败，请重新登录后尝试！");
        }
    }

    /**
     * 当前登录用户信息(包含 roles  permissions)
     * 写入redis中三个键，1 userInfo，包含用户的基本信息；2 roles包含用户的角色名称信息；
     * 3 permissions包含用户的角色Id信息；4 menuList 可打开的菜单信息；
     *
     * @return
     */
    public Object getLoginUser() {
        return getLoginUser(null, null);
    }
    public Object getLoginUser(Date date, String userAgent) {
        Integer userId = getLoginUserId();
        String key = APPLICATION_NAME + ":userInfo:" + userId;
        //防止缓存导致无法立即更新相关信息
        if (redis.hasKey(key)) {
            redis.delete(key);
        }
        if (!redis.hasKey(key)) {
//            if (date!=null) {
//                Calendar cal = Calendar.getInstance();
//                long timeSpan = (cal.getTimeInMillis() - date.getTime());
//                redis.set("timeSpan", timeSpan);
//                redis.set("userAgent", userAgent);
//            }
            JSONObject jsReturnKey = new JSONObject();
            //下面首先创建基本信息键值
            BaseUser userInfo = (BaseUser) iCommonService.getOneRecordById(USER_INFO, userId);
            if (userInfo == null) {
                SecurityUtils.getSubject().logout();
                throw BaseResponse.unAuthorization.error();
            }
            ViewBaseUser viewBaseUser = ((ViewBaseUser) iCommonService.getOneRecordById("ViewBaseUser", userInfo.getId()));
            JSONObject userInfoJSON = FastJsonUtil.toJson(userInfo);
            userInfoJSON.put("departmentName", viewBaseUser.getDepartmentName());
            Object school = iDataTreeService.getFirstParent("BaseDepartment",userInfo.getDepartmentId(),2);
            try {
                userInfoJSON.put("schoolId", (int) FastJsonUtil.toJson(school).get("id"));
            } catch(Exception ex) {

            }
            userInfoJSON.put("schoolName", FastJsonUtil.toJson(school).get("name"));
            userInfoJSON.put("jobName", viewBaseUser.getJobName());
            jsReturnKey.put("userInfo", userInfoJSON);
            //下面创建role、contestType和menuList的键值信息
            JSONObject jsRoleSearch = new JSONObject();
            jsRoleSearch.put("userId", userId);
            jsRoleSearch.put("isAudit", 1);
            List<RelUserRole> roleInfoList = ((Page<RelUserRole>)iCommonService.getSomeRecords("RelUserRole", jsRoleSearch)).getContent();
            jsReturnKey.put("roles", roleInfoList.stream().map(RelUserRole::getRoleId).collect(Collectors.toSet()));

            //下面创建permissions键值信息
            Set<Integer> roleIdSet = roleInfoList.stream().map(RelUserRole::getRoleId).collect(Collectors.toSet());
            Set<String> permissions = new HashSet<>();
            //找到roles能够操作的所有菜单
            Object menuList = new ArrayList<>();
            if (!roleIdSet.isEmpty()) {
                Set<Integer> menuIdSet = new HashSet<>();
                JSONObject jsRoleMenuSearch = new JSONObject();
                jsRoleMenuSearch.put("roleId", StringUtils.collectionToDelimitedString(roleIdSet, ","));
                Map roleMenuMap = new HashMap(1);
                roleMenuMap.put("roleId", Constant.IN);
                List<RelRoleMenu> roleMenuRelList = ((Page<RelRoleMenu>)iCommonService.getSomeRecords("RelRoleMenu", jsRoleMenuSearch, roleMenuMap)).getContent();
//                tblAuthorizationDao.findByRoleIdInAndIsDeletedFalse(roleIdSet).forEach(authorizationInfo -> {
                roleMenuRelList.forEach(rolemenurel -> {
                    menuIdSet.add(rolemenurel.getMenuId());
                    if (rolemenurel.getAddFlag()) {
                        permissions.add(rolemenurel.getMenuId() + ":" + Constant.CREATE);
                    }
                    if (rolemenurel.getVisibleFlag()) {
                        permissions.add(rolemenurel.getMenuId() + ":" + Constant.RETRIEVE);
                    }
                    if (rolemenurel.getModifyFlag()) {
                        permissions.add(rolemenurel.getMenuId() + ":" + Constant.UPDATE);
                    }
                    if (rolemenurel.getDeleteFlag()) {
                        permissions.add(rolemenurel.getMenuId() + ":" + Constant.DELETE);
                    }
                });
                // 然后返回能操作的节点
                List<SysMenu> treeOriList = (List<SysMenu>)iCommonService.getRecordsByIds("SysMenu", menuIdSet);
                if (!treeOriList.isEmpty()) {
                    List<SysMenu> treeList = TreeUtil.sortTree(treeOriList);
                    List<Object> subNodes = dataTreeServiceImpl.getSubNodes(treeList, -1, false, "");
                    menuList = subNodes;
                } else {
                    menuList = treeOriList;
                }
            }
            jsReturnKey.put("permissions", permissions);
            jsReturnKey.put("menuList", menuList);
            redis.set(key, jsReturnKey, 0x12c);
            if (permissions.size() == 0) {
                return null;
            }
        }
        Object result = redis.get(key);
        return result;
    }

    /**
     * checkRole(当前登录用户)
     * 与 @RequiresRoles 功能一致
     *
     * @param roleName
     * @return
     */
    public static boolean checkRole(String roleName) {
        try {
            SecurityUtils.getSubject().checkRole(roleName);
            return true;
        } catch (Exception e) {
            return false;
        }
    }

    /**
     * Demo
     *
     * @param searchKeys (fieldName,fieldValue)
     * @param regMap     (fieldName,{@link Constant#LT}{@link Constant#GT}{@link Constant#EQ}{@link Constant#LE}{@link Constant#GE}{@link Constant#NE} {@link Constant#LIKE})
     * @return
     */
    public <T> Specification<T> getSpecification(JSONObject searchKeys, Map<String, String> regMap, Map<String, Boolean> andor, Class<T> clazzInfo) {
        return (root, criteriaQuery, criteriaBuilder) -> {
            List<Predicate> listAnd = Base.this.getPredicate(searchKeys, regMap, andor, clazzInfo, root, criteriaBuilder, true);
            Predicate preAnd = criteriaBuilder.and(listAnd.toArray( new Predicate[listAnd.size()]));
            List<Predicate> listOr = Base.this.getPredicate(searchKeys, regMap, andor, clazzInfo, root, criteriaBuilder, false);
            Predicate preOr = criteriaBuilder.or(listOr.toArray( new Predicate[listOr.size()]));
            if (preAnd.getExpressions().size()==0) {
                return criteriaQuery.where(preOr).getRestriction();
            }
            if (preOr.getExpressions().size()==0) {
                return criteriaQuery.where(preAnd).getRestriction();
            }
            return criteriaQuery.where(preAnd, preOr).getRestriction();
        };
    }

    public <T> List<Predicate> getPredicate(JSONObject searchKeys, Map<String, String> regMap, Map<String, Boolean> andor, Class<T> clazzInfo, Root root, CriteriaBuilder criteriaBuilder, Boolean and) {
        return getPredicate(searchKeys, regMap == null ? new HashMap(0) : regMap, andor == null ? new HashMap(0) : andor, clazzInfo, new ArrayList<>(), root, criteriaBuilder, and);
    }

    public <T> List<Predicate> getPredicate(JSONObject searchKeys, Map<String, String> regMap, Map<String, Boolean> andor, Class<T> clazzInfo, List<Predicate> list, Root root, CriteriaBuilder criteriaBuilder, Boolean and) {
        Field[] fields = clazzInfo.getDeclaredFields();
        for (Field field : fields) {
            if (searchKeys.containsKey(field.getName())) {
                if ((!andor.containsKey(field.getName()) && and) || (andor.containsKey(field.getName()) && andor.get(field.getName()).equals(and))) {
                    //首先判断in
                    if (regMap.containsKey(field.getName()) && regMap.get(field.getName()).equals(Constant.IN)) {
                        list.add(new CompoundPredicate((CriteriaBuilderImpl) criteriaBuilder,
                                Predicate.BooleanOperator.OR,
                                Arrays.stream(searchKeys.getString(field.getName()).split(",")).map(value -> criteriaBuilder.equal(root.get(field.getName()), value)).collect(Collectors.toList())));
                    }
                    //NOT_IN
                    else if (regMap.containsKey(field.getName()) && regMap.get(field.getName()).equals(Constant.NOT_IN)) {
                        list.add(criteriaBuilder.not(root.get(field.getName()).in(Arrays.stream(searchKeys.getString(field.getName()).split(",")).toArray())));
                    } else {
                        switch (field.getType().getName()) {
                            case "java.lang.String":
                                if (!StringUtils.isEmpty(searchKeys.getString(field.getName()))) {
                                    if (regMap.containsKey(field.getName()) && regMap.get(field.getName()).equals(Constant.LIKE)) {
                                        list.add(criteriaBuilder.like(root.get(field.getName()), "%" + searchKeys.getString(field.getName()) + "%"));
                                    } else if (regMap.containsKey(field.getName()) && regMap.get(field.getName()).equals(Constant.NE)) {
                                        list.add(criteriaBuilder.notEqual(root.get(field.getName()), searchKeys.getString(field.getName())));
                                    } else {
                                        list.add(criteriaBuilder.equal(root.get(field.getName()), searchKeys.getString(field.getName())));
                                    }
                                }
                                break;
                            case "java.util.Date":
//                            if (!StringUtils.isEmpty(searchKeys.getJSONObject(field.getName()))) {
//                                JSONObject betweenDate = searchKeys.getJSONObject(field.getName());
//                                list.add(criteriaBuilder.between(root.get(field.getName()).as(field.getType()), betweenDate.getDate("beginTime"), betweenDate.getDate("endTime")));
//                            }
//                                if (!StringUtils.isEmpty(searchKeys.getDate(field.getName()))) {
                                    if (regMap.containsKey(field.getName())) {
                                        switch (regMap.get(field.getName())) {
                                            case Constant.LT:
                                                list.add(criteriaBuilder.lessThan(root.get(field.getName()).as(field.getType()), searchKeys.getDate(field.getName())));
                                                break;
                                            case Constant.GT:
                                                list.add(criteriaBuilder.greaterThan(root.get(field.getName()).as(field.getType()), searchKeys.getDate(field.getName())));
                                                break;
                                            case Constant.LE:
                                                list.add(criteriaBuilder.lessThanOrEqualTo(root.get(field.getName()).as(field.getType()), searchKeys.getDate(field.getName())));
                                                break;
                                            case Constant.EQ:
                                                list.add(criteriaBuilder.equal(root.get(field.getName()), searchKeys.getDate(field.getName())));
                                                break;
                                            case Constant.GE:
                                                list.add(criteriaBuilder.greaterThanOrEqualTo(root.get(field.getName()).as(field.getType()), searchKeys.getDate(field.getName())));
                                                break;
                                            case Constant.NE:
                                                list.add(criteriaBuilder.notEqual(root.get(field.getName()), searchKeys.getDate(field.getName())));
                                                break;
                                            case Constant.RANGE:
                                                JSONObject obj = searchKeys.getJSONObject(field.getName());
                                                list.add(criteriaBuilder.between(root.get(field.getName()).as(field.getType()), obj.getDate("beginDate"), obj.getDate("endDate")));
                                                break;
                                            default:
                                                break;
                                        }
                                    } else {
                                        list.add(criteriaBuilder.equal(root.get(field.getName()), searchKeys.getDate(field.getName())));
                                    }
//                                }
                                break;
                            case "java.lang.Byte":
                            case "java.lang.Short":
                            case "java.lang.Integer":
                            case "java.lang.Long":
                            case "java.lang.Double":
                            case "java.lang.Float":
                                if (!StringUtils.isEmpty(searchKeys.getInteger(field.getName()))) {
                                    if (regMap.containsKey(field.getName())) {
                                        switch (regMap.get(field.getName())) {
                                            case Constant.LT:
                                                list.add(criteriaBuilder.lessThan(root.get(field.getName()).as(field.getType()), searchKeys.getInteger(field.getName())));
                                                break;
                                            case Constant.GT:
                                                list.add(criteriaBuilder.greaterThan(root.get(field.getName()).as(field.getType()), searchKeys.getInteger(field.getName())));
                                                break;
                                            case Constant.LE:
                                                list.add(criteriaBuilder.lessThanOrEqualTo(root.get(field.getName()).as(field.getType()), searchKeys.getInteger(field.getName())));
                                                break;
                                            case Constant.EQ:
                                                list.add(criteriaBuilder.equal(root.get(field.getName()), searchKeys.getString(field.getName())));
                                                break;
                                            case Constant.GE:
                                                list.add(criteriaBuilder.greaterThanOrEqualTo(root.get(field.getName()).as(field.getType()), searchKeys.getInteger(field.getName())));
                                                break;
                                            case Constant.NE:
                                                list.add(criteriaBuilder.notEqual(root.get(field.getName()), searchKeys.getString(field.getName())));
                                                break;
                                            default:
                                                break;
                                        }
                                    } else {
                                        list.add(criteriaBuilder.equal(root.get(field.getName()), searchKeys.getString(field.getName())));
                                    }
                                }
                                break;
                            case "java.lang.Boolean":
                                list.add(criteriaBuilder.equal(root.get(field.getName()).as(Boolean.class), searchKeys.getBoolean(field.getName())));
                                break;
                            default:
                                logger.warn("<WARN>  [{}] 类型未补全 fieldTypeName [{}],fieldName [{}],fieldValue [{}]", clazzInfo.getName(), field.getType().getName(), field.getName(), searchKeys.get(field.getName()));
                                break;
                        }
                    }
                }
            }
        }
        return clazzInfo.getSuperclass().getDeclaredFields().length != 0 ? getPredicate(searchKeys, regMap, andor, clazzInfo.getSuperclass(), list, root, criteriaBuilder, and) : list;
    }




}



