package com.ahaxt.competition.repository.db;

import com.ahaxt.competition.entity.db.SysOssFile;
import com.ahaxt.competition.repository.base.BaseDao;
import org.springframework.data.jpa.repository.Modifying;
import org.springframework.data.jpa.repository.Query;
import org.springframework.stereotype.Repository;

import java.util.List;
import java.util.Optional;

/**
 * @author hongzhangming
 */
@Repository
public interface SysOssFileDao extends BaseDao<SysOssFile, Integer> {

    /**
     * 跟据文件id查询
     * @param fileIds
     * @return
     */
    List<SysOssFile> getByIdInAndIsDeletedFalse(Iterable<Integer> fileIds);

    Optional<SysOssFile> findByIdAndIsDeletedFalse(Integer fileId);

    SysOssFile findByRelationIdAndTypeAndTableNameAndIsDeletedFalse(Integer relationId, Integer type, String tabName);

    List<SysOssFile> findByRelationIdAndTypeAndIsDeletedFalse(Integer userId, int i);

    @Modifying
    @Query(nativeQuery = true,value = "update SYS_OSS_FILE SET IS_DELETED = 1 where ID in ?1")
    void updateByIds(List<Integer> ids);


    @Query(nativeQuery = true,value = "select * from SYS_OSS_FILE where IS_DELETED = 0 and RELATION_ID = ?1 and TYPE = ?2")
    List<SysOssFile> findByFlowCaseIdAndType(Integer flowCaseId,Integer type);

    List<SysOssFile> findByFileIdentifierAndIsDeletedFalse(String identifier);

    SysOssFile findByFileIdentifierAndRelationIdAndTypeAndIsDeletedFalse(String identifier,Integer relationId,Integer type);
    List<SysOssFile> findByTypeAndIsDeletedFalse(Integer type);

    List<SysOssFile> findByFileIdentifierAndTypeAndIsDeletedFalse(String identifier, Integer type);

    List<SysOssFile> findByRelationIdInAndTypeAndIsDeletedFalse(List<Integer> relationIds, Integer type);

    @Query(nativeQuery = true,value = "select * from SYS_OSS_FILE where IS_DELETED = 0 and TYPE = 1 and RELATION_ID = ?1")
    List<SysOssFile> getImages(Integer relationId);

}
