package com.ahaxt.competition.service.impl;

import com.ahaxt.competition.entity.db.MainChannelContent;
import com.ahaxt.competition.entity.db.SysOssFile;
import com.ahaxt.competition.repository.db.MainChannelContentDao;
import com.ahaxt.competition.repository.db.SysOssFileDao;
import com.ahaxt.competition.service.IDataListService;
import com.ahaxt.competition.service.IWebsiteService;
import com.ahaxt.competition.utils.Base64Util;
import com.alibaba.fastjson.JSONObject;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.PageRequest;
import org.springframework.data.domain.Pageable;
import org.springframework.stereotype.Service;
import org.springframework.util.ObjectUtils;
import org.springframework.util.StringUtils;

import javax.annotation.Resource;
import java.util.List;

/**
 * @author lhy
 * @date 2022/11/7
 */
@Service
public class WebsiteServiceImpl implements IWebsiteService {

    @Resource
    private MainChannelContentDao mainChannelContentDao;
    @Resource
    private IDataListService iDataListService;
    @Resource
    private SysOssFileDao ossFileDao;


    @Override
    public Object  getColumnContent(Integer channelTypeId, Integer page, Integer size) {
        Pageable pageable= PageRequest.of(page-1,size);
        Page<MainChannelContent> channelContents = mainChannelContentDao.getContent(channelTypeId,pageable);
        List<MainChannelContent> content = channelContents.getContent();
        content.stream().forEach(m->{
            if (m.getChannelCText()!=null) {
                List<SysOssFile> images = ossFileDao.getImages(m.getId());
                if (images.size()>0) {
                    String firstImg = images.get(0).getUrl();
                    m.setFirstImg(firstImg);
                }
            }
        });
        return channelContents;
    }

    @Override
    public Object getHomePage() {
        JSONObject json = new JSONObject();

        //List<MainChannelContent> mainChannelContents = mainChannelContentDao.findByIsDeletedFalse();
        List<MainChannelContent> news=mainChannelContentDao.findByChannelTypeIdAndIsDeletedFalse(41,6);
        List<MainChannelContent> newsDynamic=mainChannelContentDao.findByChannelTypeIdAndIsDeletedFalse(42,6);
        List<MainChannelContent> train=mainChannelContentDao.findByChannelTypeIdAndIsDeletedFalse(44,6);
        List<MainChannelContent> competitions=mainChannelContentDao.findByChannelTypeIdAndIsDeletedFalse(43,6);
        List<MainChannelContent> competitionsNotice=mainChannelContentDao.findByChannelTypeIdAndIsDeletedFalse(45,6);
        List<MainChannelContent> notices=mainChannelContentDao.findByChannelTypeIdAndIsDeletedFalse(33,6);
        List<MainChannelContent> members=mainChannelContentDao.findByChannelTypeIdAndIsDeletedFalse(38,12);
        List<MainChannelContent> organizations=mainChannelContentDao.findByChannelTypeIdAndIsDeletedFalse(36,8);
        getFirstImg(news);
        getFirstImg(newsDynamic);
        getFirstImg(train);
        getFirstImg(notices);
        getFirstImg(organizations);
        getFirstImg(competitions);
        getFirstImg(competitionsNotice);
        getFirstImg(members);
        json.put("滚动新闻",news);
        json.put("新闻动态",newsDynamic);
        json.put("师资培训",train);
        json.put("通知公告",notices);
        json.put("组织机构",organizations);
        json.put("竞赛活动",competitions);
        json.put("竞赛专栏",competitionsNotice);
        json.put("单位会员",members);
        // 获取首页轮播图
        List<MainChannelContent> sliders = mainChannelContentDao.findByChannelCSliderTrueAndIsDeletedFalse();
        if(ObjectUtils.isEmpty(sliders)){
            getFirstImg(sliders);
        }
        json.put("sliders",sliders);
        return json;
    }

    private void getFirstImg(List<MainChannelContent> contents) {
        contents.forEach(m->{
            List<SysOssFile> images = ossFileDao.getImages(m.getId());
            if (images.size()>0) {
                String firstImg = images.get(0).getUrl();
                m.setFirstImg(firstImg);
            }
        });
    }

    @Override
    public Object readNum(Integer id) {
        MainChannelContent channelContent = mainChannelContentDao.getByIdAndIsDeletedFalse(id);
        channelContent.setHitTimes((channelContent.getHitTimes()== null ? 0 : channelContent.getHitTimes())+1);
        mainChannelContentDao.save(channelContent);
        //阅读数增加
        return null;
    }

    @Override
    public Object search(String word) {

        if (StringUtils.isEmpty(word)){
            word = null;
        }else {
            word = "%" + word + "%";
        }
        List<MainChannelContent> channelContents = mainChannelContentDao.search(word);

        return channelContents;
    }
}
