package com.ahaxt.competition.repository.db;

import com.ahaxt.competition.entity.db.MainChannelContent;
import com.ahaxt.competition.repository.base.BaseDao;

import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;

import org.springframework.data.jpa.repository.Modifying;

import org.springframework.data.jpa.repository.Query;
import org.springframework.stereotype.Repository;

import java.util.List;

@Repository
public interface MainChannelContentDao extends BaseDao<MainChannelContent, Integer> {


    @Query(nativeQuery = true,value = "SELECT  * FROM main_channel_content WHERE CHANNEL_TYPE_ID =?1 AND IS_DELETED=0 AND PUBLISH_STATUS =1 order by ID DESC",countQuery = "SELECT  count(ID) FROM main_channel_content WHERE CHANNEL_TYPE_ID =?1 AND IS_DELETED=0 AND PUBLISH_STATUS =1 order by ID DESC")
    Page<MainChannelContent> getContent(Integer channelTypeId, Pageable pageable);

    @Query(nativeQuery = true,value = "SELECT  * FROM main_channel_content WHERE CHANNEL_TYPE_ID =?1 AND IS_DELETED=0 AND PUBLISH_STATUS =1  order by ID DESC LIMIT ?2")
    List<MainChannelContent> findByChannelTypeIdAndIsDeletedFalse(int i,int num);

    @Query(nativeQuery = true,value = "select * from main_channel_content where OLD_ID = ?1 and IS_DELETED=0 ")
    MainChannelContent findByOldId(Integer id);


    @Modifying
    @Query(nativeQuery = true,value = "update main_channel_content set CHANNEL_C_TEXT = ?2 where ID = ?1")
    void appendImg(Integer id,String url);

    @Query(nativeQuery = true,value = "SELECT  * FROM main_channel_content WHERE (CHANNEL_C_TEXT LIKE ?1 OR ?1 IS NULL) AND IS_DELETED=0 AND PUBLISH_STATUS =1 order by ID DESC")
    List<MainChannelContent> search(String word);


    List<MainChannelContent> findByChannelCSliderTrueAndIsDeletedFalse();
}
