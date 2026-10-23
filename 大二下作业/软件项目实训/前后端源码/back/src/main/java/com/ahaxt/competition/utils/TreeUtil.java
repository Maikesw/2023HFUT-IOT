package com.ahaxt.competition.utils;

import com.ahaxt.competition.entity.base.BaseTreeInfo;
import com.ahaxt.competition.repository.base.BaseTreeDao;
import org.springframework.util.LinkedMultiValueMap;
import org.springframework.util.MultiValueMap;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;
import java.util.stream.Collectors;

/**
 * @author hongzhangming
 */
@SuppressWarnings("unchecked")
public class TreeUtil {

    /**
     * 树结构转换
     * @param treeList
     * @param <Tree>
     * @return
     */
    public static <Tree extends BaseTreeInfo> List<Tree> toTree(List<Tree> treeList) {
        if (treeList.isEmpty()) {
            return treeList;
        }
        treeList = sortTree(treeList);
        int topLevel = treeList.get(0).getTheLevel();
        MultiValueMap<Integer, Tree> multiValueMap = new LinkedMultiValueMap<>();
        Map<Integer, Tree> map = new HashMap<>(16);
        treeList.forEach(tree -> {
            multiValueMap.add(tree.getParentId(), tree);
            map.put(tree.getId(), tree);
        });
        treeList.forEach(tree -> tree.setChildren(multiValueMap.get(tree.getId())));
        return treeList.stream().filter(tree -> tree.getTheLevel() == topLevel).collect(Collectors.toList());
    }

    /**
     * 排序
     * @param treeList
     * @return
     */
    public static <Tree extends BaseTreeInfo> List<Tree> sortTree(List<Tree> treeList) {
        boolean flag = true;
        for (int i = 1; i < treeList.size(); i++) {
            if (treeList.get(i).getTheLevel() < treeList.get(i - 1).getTheLevel()
                    || (treeList.get(i).getTheLevel().equals(treeList.get(i - 1).getTheLevel()) &&
                    treeList.get(i).getTheOrder() < treeList.get(i - 1).getTheOrder())) {
                CollectionUtil.conversion(treeList, i, i - 1);
                flag = false;
            }
        }
        return flag ? treeList : sortTree(treeList);
    }

    /**
     * 返回childList
     * @param list
     * @param parentId
     * @param dao
     * @param <ID>
     * @param <Tree>
     * @param <Dao>
     * @return
     */
    public static <ID, Tree extends BaseTreeInfo, Dao extends BaseTreeDao<Tree, ID>> List<Tree> findChildList(List<Tree> list, ID parentId, Dao dao) {
        List<Tree> flag = dao.findByParentIdAndIsDeletedFalseOrderByTheOrder(parentId);
        list = CollectionUtil.sum(list, flag);
        for (Tree t : flag) {
            findChildList(list, (ID) t.getId(), dao);
        }
        return list;
    }

    /**
     * 添加
     * @param treeInfo
     * @param dao
     * @param <Tree>
     * @param <Dao>
     * @return
     */
    public static <Tree extends BaseTreeInfo, Dao extends BaseTreeDao<Tree, Integer>> Tree addEntity(Tree treeInfo, Dao dao) {
        treeInfo.setIsLeaf(true);
        treeInfo.setChildNum(0);
        Tree parentInfo = dao.getByIdAndIsDeletedFalse(treeInfo.getParentId());
        if (parentInfo != null) {
            parentInfo.setIsLeaf(false);
            parentInfo.setChildNum(parentInfo.getChildNum() + 1);
            dao.save(parentInfo);
            treeInfo.setTheOrder(parentInfo.getChildNum());
            treeInfo.setTheLevel(parentInfo.getTheLevel() + 1);
        } else {
            treeInfo.setTheLevel(1);
            List<Tree> brotherList = dao.findByParentIdAndIsDeletedFalseOrderByTheOrder(0);
            treeInfo.setTheOrder(brotherList.size() + 1);
        }
        return dao.save(treeInfo);
    }

    /**
     * 删除
     * @param treeId
     * @param treeDao
     * @return
     */
    public static <Tree extends BaseTreeInfo, Dao extends BaseTreeDao<Tree, Integer>> List<Tree> deleteTree(Integer treeId, Dao treeDao) {
        List<Tree> list = new ArrayList<>();
        Tree treeInfo = treeDao.getByIdAndIsDeletedFalse(treeId);
        Tree parentInfo = treeDao.getByIdAndIsDeletedFalse(treeInfo.getParentId());
        if (parentInfo != null) {
            parentInfo.setChildNum(parentInfo.getChildNum() - 1);
            if (parentInfo.getChildNum() == 0) {
                parentInfo.setIsLeaf(true);
            }
            treeDao.save(parentInfo);
        }
        list.add(treeInfo);
        treeDao.deleteAll(CollectionUtil.sum(list, TreeUtil.findChildList(list, treeId, treeDao)));
        return list;
    }

    /**
     * 修改
     * @param tree
     * @param treeDao
     * @param <Tree>
     * @param <Dao>
     * @return
     */
    public static <Tree extends BaseTreeInfo, Dao extends BaseTreeDao<Tree, Integer>> Tree editTree(Tree tree, Dao treeDao) {
        if (tree.getId() == null) {
            return tree;
        }
        Tree treeOld = treeDao.getByIdAndIsDeletedFalse(tree.getId());
        if (!treeOld.getParentId().equals(tree.getParentId())) {
            //修改 parentId 后放到最后
            Tree oldParentTree = treeDao.getByIdAndIsDeletedFalse(treeOld.getParentId());
            oldParentTree.setChildNum(oldParentTree.getChildNum() - 1);
            oldParentTree.setIsLeaf(oldParentTree.getChildNum() == 0);
            Tree newParentTree = treeDao.getByIdAndIsDeletedFalse(tree.getParentId());
            newParentTree.setChildNum(newParentTree.getChildNum() + 1);
            newParentTree.setIsLeaf(false);
            treeDao.save(newParentTree);
            treeDao.save(oldParentTree);
            tree.setTheLevel(newParentTree.getTheLevel() + 1);
            tree.setTheOrder(newParentTree.getChildNum());
        } else {
            resetOrder(tree, treeOld, treeDao, treeDao.findByParentIdAndIsDeletedFalseOrderByTheOrder(tree.getParentId()));
            tree.setTheLevel(treeOld.getTheLevel());
        }
        return treeDao.save(tree);
    }

    /**
     * 同级修改排序
     * @param tree
     * @param treeOld
     * @param dao
     * @param brotherList
     * @param <Tree>
     * @param <Dao>
     */
    public static <Tree extends BaseTreeInfo, Dao extends BaseTreeDao<Tree, Integer>> void resetOrder(Tree tree, Tree treeOld, Dao dao, List<Tree> brotherList) {
        if (treeOld.getTheOrder() > tree.getTheOrder()) {
            for (Tree tr : brotherList) {
                if (tr.getTheOrder() >= tree.getTheOrder()) {
                    if (tr.getId().equals(tree.getId())) {
                        break;
                    } else {
                        tr.setTheOrder(tr.getTheOrder() + 1);
                        dao.save(tr);
                    }
                }
            }
        } else if (treeOld.getTheOrder() < tree.getTheOrder()) {
            for (Tree tr : brotherList) {
                if (tr.getTheOrder() > treeOld.getTheOrder()) {
                    if (tr.getTheOrder() <= tree.getTheOrder()) {
                        tr.setTheOrder(tr.getTheOrder() - 1);
                        dao.save(tr);
                    } else {
                        break;
                    }
                }
            }
        } else {
            // 未修改 order 排序，不存在修改level
        }
    }
}
