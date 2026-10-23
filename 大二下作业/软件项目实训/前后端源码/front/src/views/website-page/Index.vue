<template>
  <section>
    <!-- 头部菜单 -->
    <title-bar />
    
    <!-- 检查当前路径，如果是根路径则直接显示首页内容 -->
    <div v-if="$route.path === '/website' || $route.path === '/website/'" class="direct-homepage">
      <div class="banner">
        <div class="banner-title">安徽省高等学校计算机教育研究会</div>
        <div class="banner-subtitle">致力于提升安徽省高校计算机教育质量和水平</div>
      </div>
      
      <div class="main-content">
        <!-- 新闻列表 -->
        <div class="section">
          <div class="section-title">最新动态</div>
          <div class="news-list">
            <div class="news-item" v-for="(item, index) in newsList" :key="index">
              <div class="news-image" :style="`background-color: ${item.color}`">
                <span class="image-text">{{item.image}}</span>
              </div>
              <div class="news-content">
                <div class="news-title">{{item.title}}</div>
                <div class="news-date">{{item.date}}</div>
                <div class="news-desc">{{item.desc}}</div>
              </div>
            </div>
          </div>
        </div>
        
        <!-- 通知公告 -->
        <div class="section">
          <div class="section-title">通知公告</div>
          <div class="notice-list">
            <div class="notice-item" v-for="(item, index) in noticeList" :key="index">
              <div class="notice-header">
                <div class="notice-title">{{item.title}}</div>
                <div class="notice-date">{{item.date}}</div>
              </div>
              <div class="notice-content">{{item.content}}</div>
            </div>
          </div>
        </div>
        
        <!-- 组织机构 -->
        <div class="organization-section">
          <div class="section-title">组织机构</div>
          <div class="org-list">
            <div class="org-item" v-for="(item, index) in orgList" :key="index">
              <div class="org-icon">
                <span class="icon-circle">组</span>
              </div>
              <div class="org-name">{{item.name}}</div>
            </div>
          </div>
        </div>
        
        <!-- 竞赛专栏 -->
        <div class="section">
          <div class="section-title">竞赛专栏</div>
          <div class="competition-list">
            <div class="competition-item" v-for="(item, index) in competitionList" :key="index">
              <div class="competition-title">{{item.title}}</div>
              <div class="competition-date">{{item.date}}</div>
              <div class="competition-desc">{{item.desc}}</div>
            </div>
          </div>
        </div>
      </div>
      
      <div class="footer">
        <p>© 2023 安徽省高等学校计算机教育研究会 版权所有</p>
        <p>地址：安徽省合肥市</p>
      </div>
    </div>
    
    <page-main v-else />
    <Footer-code v-if="!$route.path.includes('homepage')" />
  </section>
</template>
<script>
import TitleBar from './website-layout/TitleBar.vue'
import PageMain from './website-layout/PageMain.vue'
import FooterCode from './website-layout/Footer.vue'
export default {
  components: { TitleBar, PageMain, FooterCode },
  data() {
    return {
      newsList: [
        { 
          title: '我省举办第七届大学生程序设计竞赛',
          date: '2023-06-15',
          desc: '6月10日-12日，第七届安徽省大学生程序设计竞赛在合肥工业大学举行，来自全省60余所高校的300多支队伍参赛。比赛采用ACM赛制，经过激烈角逐，最终合肥工业大学代表队获得冠军。',
          image: '竞赛',
          color: '#3776FF'
        },
        {
          title: '研究会召开2023年度工作会议',
          date: '2023-03-20',
          desc: '3月18日，安徽省高等学校计算机教育研究会在安徽大学召开2023年度工作会议，会议总结了2022年工作，部署了2023年重点任务，并就高校计算机基础教育改革进行了深入讨论。',
          image: '会议',
          color: '#4285F4'
        },
        {
          title: '高校计算机课程建设培训班成功举办',
          date: '2023-05-10',
          desc: '为提高我省高校计算机基础课程教学质量，研究会于5月8日-9日在安徽师范大学举办了高校计算机课程建设培训班，培训内容包括课程思政建设、教学方法创新、课程评价方式改革等。',
          image: '培训',
          color: '#228B22'
        },
        {
          title: '关于开展2023年优秀教学案例评选活动的通知',
          date: '2023-04-25',
          desc: '为促进计算机教学创新，发掘优秀教学经验，提高教学质量，研究会决定开展2023年优秀教学案例评选活动。各高校教师可围绕课程思政建设、线上线下混合教学、项目式教学等主题提交案例。',
          image: '评选',
          color: '#800080'
        }
      ],
      noticeList: [
        {
          title: '关于举办2023年教师信息化教学能力提升培训的通知',
          date: '2023-10-12',
          content: '为提升我省高校教师信息化教学能力，研究会将于11月10-12日在安徽理工大学举办教师信息化教学能力提升培训班。培训内容包括：信息化教学设计、教学工具使用、混合式教学模式等。'
        },
        {
          title: '2023年学术年会征文通知',
          date: '2023-09-25',
          content: '研究会定于12月中旬召开2023年学术年会，现面向全省高校征集论文。征文主题包括：计算机基础教育改革、人工智能教育融合、编程教学创新等。投稿截止日期：11月30日。'
        },
        {
          title: '关于组织参加全国高校计算机实验教学案例设计竞赛的通知',
          date: '2023-10-05',
          content: '全国高校计算机实验教学案例设计竞赛将于2024年3月举行，请各高校积极组织教师参与。校内初赛截止日期为11月15日，省级选拔赛将于12月举行。'
        },
        {
          title: '关于开展教材建设研讨会的通知',
          date: '2023-10-01',
          content: '为推进我省高校计算机教材建设，研究会将于10月20日在安徽财经大学召开教材建设研讨会。会议将围绕新工科背景下计算机基础课程教材建设、教材数字化转型等议题展开讨论。'
        }
      ],
      orgList: [
        { name: '合肥工业大学计算机学院' },
        { name: '安徽大学计算机科学与技术学院' },
        { name: '中国科学技术大学计算机科学与技术学院' },
        { name: '安徽工业大学计算机科学与技术学院' },
        { name: '安徽理工大学计算机科学与工程学院' },
        { name: '安徽师范大学计算机与信息学院' },
        { name: '安徽财经大学信息管理学院' },
        { name: '安徽建筑大学计算机与信息学院' }
      ],
      competitionList: [
        {
          title: '2023年安徽省大学生计算机设计大赛通知',
          date: '2023-10-15',
          desc: '2023年安徽省大学生计算机设计大赛将于11月举行，大赛分为软件应用与开发、微课与教学辅助、物联网应用、人工智能应用等类别。各高校请于10月30日前完成校内选拔，并推荐优秀作品参加省赛。'
        },
        {
          title: '关于组织参加第15届蓝桥杯大赛安徽赛区比赛的通知',
          date: '2023-10-10',
          desc: '第15届蓝桥杯全国软件和信息技术专业人才大赛安徽赛区比赛将于2024年3月举行，比赛分为软件类和电子类两个方向。请各高校积极组织学生报名参赛，报名截止日期为2024年2月28日。'
        },
        {
          title: '2024年安徽省大学生程序设计竞赛筹备会议通知',
          date: '2023-10-05',
          desc: '2024年安徽省大学生程序设计竞赛筹备会议将于11月3日在合肥工业大学举行，请各参赛高校派代表参加。会议将讨论比赛组织、命题、评审等相关事宜。'
        },
        {
          title: '全国高校人工智能创新大赛安徽赛区选拔赛结果公示',
          date: '2023-09-28',
          desc: '全国高校人工智能创新大赛安徽赛区选拔赛已于9月25日顺利结束，经专家评审，共有20支队伍入围国赛。获奖名单已在研究会官网公示，公示期为7天，如有异议请及时反馈。'
        }
      ]
    }
  },
  created() {
  },
  methods: {
  }
}
</script>

<style lang="scss" scoped>
// @import "@/assets/css/mixin.scss";
// @import "@/assets/css/variables.scss";

.direct-homepage {
  font-family: PingFang SC, Microsoft YaHei, sans-serif;
  
  .banner {
    height: 400px;
    background-color: #3776FF;
    color: white;
    display: flex;
    flex-direction: column;
    justify-content: center;
    align-items: center;
    
    .banner-title {
      font-size: 36px;
      font-weight: bold;
      margin-bottom: 20px;
    }
    
    .banner-subtitle {
      font-size: 18px;
    }
  }
  
  .main-content {
    max-width: 1200px;
    margin: 0 auto;
    padding: 20px;
  }
  
  .section {
    margin-bottom: 40px;
  }
  
  .section-title {
    font-size: 24px;
    font-weight: bold;
    margin-bottom: 20px;
    position: relative;
    padding-left: 15px;
    
    &:before {
      content: '';
      position: absolute;
      left: 0;
      top: 50%;
      transform: translateY(-50%);
      width: 5px;
      height: 20px;
      background-color: #3776FF;
    }
  }
  
  .news-list {
    display: grid;
    grid-template-columns: repeat(2, 1fr);
    gap: 20px;
  }
  
  .news-item {
    display: flex;
    border: 1px solid #eee;
    border-radius: 4px;
    overflow: hidden;
    height: 150px;
  }
  
  .news-image {
    width: 200px;
    display: flex;
    align-items: center;
    justify-content: center;
    
    .image-text {
      color: white;
      font-size: 24px;
      font-weight: bold;
    }
  }
  
  .news-content {
    flex: 1;
    padding: 15px;
    
    .news-title {
      font-size: 16px;
      font-weight: bold;
      margin-bottom: 10px;
    }
    
    .news-date {
      font-size: 12px;
      color: #666;
      margin-bottom: 10px;
    }
    
    .news-desc {
      font-size: 14px;
      color: #333;
      line-height: 1.6;
      display: -webkit-box;
      -webkit-line-clamp: 3;
      -webkit-box-orient: vertical;
      overflow: hidden;
    }
  }
  
  .notice-list {
    display: flex;
    justify-content: space-between;
  }
  
  .notice-item {
    width: 23%;
    border: 1px solid #eee;
    border-radius: 4px;
    padding: 15px;
  }
  
  .notice-header {
    border-bottom: 1px solid #eee;
    padding-bottom: 10px;
    margin-bottom: 10px;
  }
  
  .notice-title {
    font-size: 16px;
    font-weight: bold;
    margin-bottom: 5px;
    white-space: nowrap;
    overflow: hidden;
    text-overflow: ellipsis;
  }
  
  .notice-date {
    font-size: 12px;
    color: #666;
  }
  
  .notice-content {
    font-size: 14px;
    color: #333;
    line-height: 1.6;
    height: 120px;
    overflow: hidden;
    display: -webkit-box;
    -webkit-line-clamp: 5;
    -webkit-box-orient: vertical;
  }
  
  .organization-section {
    background-color: #f5f5f5;
    padding: 30px 0;
    margin: 40px 0;
  }
  
  .org-list {
    display: grid;
    grid-template-columns: repeat(4, 1fr);
    gap: 20px;
  }
  
  .org-item {
    display: flex;
    align-items: center;
    background-color: #e6eeff;
    border-radius: 60px 0 0 60px;
    padding-right: 20px;
  }
  
  .org-icon {
    width: 80px;
    height: 80px;
    background-color: white;
    border-radius: 50%;
    display: flex;
    align-items: center;
    justify-content: center;
    margin-right: 15px;
    
    .icon-circle {
      color: #3776FF;
      font-size: 24px;
      font-weight: bold;
    }
  }
  
  .org-name {
    font-size: 16px;
    color: #0066be;
    font-weight: bold;
    flex: 1;
    white-space: nowrap;
    overflow: hidden;
    text-overflow: ellipsis;
  }
  
  .competition-list {
    display: flex;
    flex-direction: column;
    gap: 15px;
  }
  
  .competition-item {
    border: 1px solid #eee;
    border-radius: 4px;
    padding: 15px;
    transition: all 0.3s;
    
    &:hover {
      box-shadow: 0 5px 15px rgba(0, 0, 0, 0.1);
    }
  }
  
  .competition-title {
    font-size: 16px;
    font-weight: bold;
    margin-bottom: 10px;
  }
  
  .competition-date {
    font-size: 12px;
    color: #666;
    margin-bottom: 10px;
  }
  
  .competition-desc {
    font-size: 14px;
    color: #333;
    line-height: 1.6;
  }
  
  .footer {
    background-color: #333;
    color: white;
    text-align: center;
    padding: 20px;
    font-size: 14px;
    
    p {
      margin: 5px 0;
    }
  }
}
</style>
