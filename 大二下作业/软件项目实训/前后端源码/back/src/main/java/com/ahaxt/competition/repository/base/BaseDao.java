package com.ahaxt.competition.repository.base;

import org.hibernate.annotations.DynamicInsert;
import org.hibernate.annotations.DynamicUpdate;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.JpaSpecificationExecutor;
import org.springframework.data.jpa.repository.Modifying;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.NoRepositoryBean;
import org.springframework.transaction.annotation.Transactional;

import java.sql.SQLException;
import java.util.List;

/**
 * @author hongzhangming
 */
@NoRepositoryBean
@DynamicUpdate
@DynamicInsert
@Transactional(rollbackFor = Exception.class)
public interface BaseDao<T,ID> extends JpaRepository<T,ID>, JpaSpecificationExecutor<T> {
//public interface BaseDao<T,ID> extends JpaRepository<T,ID> {
    /**
     * 查询未逻辑删除的数据
     * @param id
     * @return
     */
    @Query("select t from #{#entityName} t where t.id= ?1 and isDeleted=0")
    T getByIdAndIsDeletedFalse(ID id);
    /**
     * 查询未逻辑删除的数据
     * @param id
     * @return
     */
    @Query("select t from #{#entityName} t where t.id in ?1")
    List<T> findByIdIn(Iterable<ID> id);
    /**
     * 查询未逻辑删除的数据
     * @param id
     * @return
     */
    @Query("select t from #{#entityName} t where t.id in ?1 and isDeleted=0")
    List<T> findByIdInAndIsDeletedFalse(Iterable<ID> id);

    /**
     * 逻辑删除
     * @param id
     * @return
     */
    @Transactional(rollbackFor = SQLException.class)
    @Modifying
    @Query(value = "update #{#entityName} set isDeleted = true, updateTime = current_timestamp where id = ?1")
    void deleteByIsDeleted(ID id);

    /**
     * 逻辑删除
     * @param idSet
     */
    @Transactional(rollbackFor = SQLException.class)
    @Modifying
    @Query(value = "update #{#entityName} set isDeleted = true, updateTime = current_timestamp where id in ?1")
    void deleteByIdIn(Iterable<ID> idSet);

    /**
     * 清空 已删除的
     */
    @Transactional(rollbackFor = SQLException.class)
    @Modifying
    @Query(value = "delete from #{#entityName} where isDeleted = true")
    void deleteByIsDeleted();
    /**
     * 查询未逻辑删除的所有数据
     * @return
     */
    List<T> findByIsDeletedFalse();

}
