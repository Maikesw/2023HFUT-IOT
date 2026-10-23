<template>
  <div class="Homepage">
    <!-- 顶部banner -->
    <div class="carousel-container">
      <el-carousel 
        ref="carousel"
        class="box" 
        height="660px" 
        arrow="always" 
        indicator-position="outside" 
        :autoplay="true" 
        :interval="5000">
        <el-carousel-item v-for="(item, index) in bannerList" :key="index" :class="`carousel-bg-${index % 5 + 1}`">
          <!-- 添加半透明遮罩层增强文字可见性 -->
          <div class="carousel-overlay"></div>
          <div class="carousel-content">
            <h3 class="banner-title animated-text">{{ item.title || item.channelCName }}</h3>
            <p class="banner-text animated-text">{{ item.description || removeHtmlStyle(item.channelCText) }}</p>
            <div class="banner-button animated-text" @click="handleBannerAction(item)">
              {{ item.actionText || '了解更多' }} <i class="el-icon-arrow-right"></i>
            </div>
          </div>
        </el-carousel-item>
    </el-carousel>
    </div>
    
    <!-- 核心功能区 -->
    <div class="feature-section">
      <div class="feature-container">
        <div class="feature-item" @click="viewMore('/website/newsCenter')" :style="{ animationDelay: '0.1s' }">
          <div class="feature-icon">
            <i class="el-icon-document"></i>
          </div>
          <div class="feature-text">
            <div class="feature-title">新闻资讯</div>
            <div class="feature-desc">了解研究会最新动态</div>
          </div>
        </div>
        <div class="feature-item" @click="viewMore('/website/notice')" :style="{ animationDelay: '0.2s' }">
          <div class="feature-icon">
            <i class="el-icon-bell"></i>
          </div>
          <div class="feature-text">
            <div class="feature-title">通知公告</div>
            <div class="feature-desc">查看最新通知与公告</div>
          </div>
        </div>
        <div class="feature-item" @click="viewMore('/website/organization')" :style="{ animationDelay: '0.3s' }">
          <div class="feature-icon">
            <i class="el-icon-office-building"></i>
          </div>
          <div class="feature-text">
            <div class="feature-title">组织机构</div>
            <div class="feature-desc">探索成员院校与机构</div>
          </div>
        </div>
        <div class="feature-item" @click="viewMore('/website/competition')" :style="{ animationDelay: '0.4s' }">
          <div class="feature-icon">
            <i class="el-icon-trophy"></i>
          </div>
          <div class="feature-text">
            <div class="feature-title">竞赛活动</div>
            <div class="feature-desc">参与各类学术竞赛</div>
          </div>
        </div>
      </div>
    </div>
    
    <div class="content">
      <!-- 滚动新闻 -->
      <TitleAndMore style="margin:50px 0 40px" :title="currentType" @viewMore="viewMore('/website/newsCenter', activityIndex)">
        <el-popover slot="title-right" v-model="popper" popper-class="index-popover" placement="right" trigger="click">
          <ul class="list">
            <li v-for="(item, index) in typeList" :key="index" class="list-item" @click="changeType(index)">{{ item.title }}</li>
          </ul>
          <span slot="reference" class="show-popover" title="点击切换"><i class="el-icon-arrow-down" /></span>
        </el-popover>
      </TitleAndMore>
      <div class="first-list">
        <div v-for="(item, index) in scrollNews.slice(0, 6)" :key="item.id" 
            class="news-item" 
            :class="{'animate-in': true}" 
            :style="{ animationDelay: index * 0.1 + 's' }" 
            @click="toDetail(item, 1)">
          <div class="news-img-container">
            <div class="news-img-placeholder" :style="{ backgroundColor: getColorByIndex(index) }">
              <i :class="getIconByIndex(index)"></i>
            </div>
          </div>
          <div class="news-content">
            <div class="news-title" :title="item.channelCName">{{ item.channelCName }}</div>
            <div class="news-time"><i class="el-icon-time"></i> {{ item.channelCTime | filterDateTime }}</div>
            <div class="news-remarks">{{ removeHtmlStyle(item.channelCText) }}</div>
          </div>
        </div>
      </div>
      <!-- 通知公告 -->
      <TitleAndMore style="margin:50px 0 40px" title="通知公告" @viewMore="viewMore('/website/notice')" />
      <div class="second-list">
        <div v-for="(item, index) in notices.slice(0, 4)" :key="item.id" 
            class="notice-item" 
            :class="{'animate-in': true}" 
            :style="{ animationDelay: index * 0.1 + 's' }" 
            @click="toDetail(item, 2)">
          <div class="notice-top" :class="`notice-bg-${index % 4 + 1}`">
            <span class="notice-title" :title="item.channelCName">{{ item.channelCName }}</span>
            <span class="notice-time"><i class="el-icon-time"></i> {{ item.channelCTime | filterDateTime }}</span>
          </div>
          <div class="notice-content">{{ removeHtmlStyle(item.channelCText) }}</div>
        </div>
      </div>
    </div>
    <!-- 组织机构 -->
    <div class="organization-bg">
      <div class="organization-content">
        <TitleAndMore style="margin:39px 0 39px" title="组织机构" @viewMore="viewMore('/website/organization')" />
        <div class="organization-list">
          <div v-for="(item, index) in organizationList.slice(0, 8)" 
              :key="item.id" 
              class="organization-item" 
              :class="{'animate-in': true}" 
              :style="{ animationDelay: index * 0.05 + 's' }" 
              @click="toDetail(item, 3)">
            <div class="circle">
              <div class="logo-placeholder" :class="`org-bg-${index % 8 + 1}`">
                <i class="el-icon-office-building"></i>
              </div>
            </div>
            <span class="organization-name" :title="item.channelCName">{{ item.channelCName }}</span>
          </div>
        </div>
      </div>
    </div>
    <!-- 竞赛专栏 -->
    <div class="competition">
      <div class="competition-content">
        <TitleAndMore style="margin:50px 0 40px" title="竞赛专栏" @viewMore="viewMore('/website/competition')" />
        <div class="competition-container">
          <div v-for="(item, index) in raceList.slice(0, 4)" 
              :key="item.id" 
              class="competition-item" 
              :class="[`comp-bg-${index % 4 + 1}`, {'animate-in': true}]" 
              :style="{ animationDelay: index * 0.1 + 's' }" 
              @click="toDetail(item, 4)">
            <div class="competition-badge">{{ getBadgeText(index) }}</div>
          <span class="competition-title" :title="item.channelCName">{{ item.channelCName }}</span>
            <span class="competition-time"><i class="el-icon-time"></i> {{ item.channelCTime | filterDateTime }}</span>
          <span class="competition-remarks">{{ removeHtmlStyle(item.channelCText) }}</span>
            <div class="competition-action">查看详情 <i class="el-icon-arrow-right"></i></div>
          </div>
        </div>
      </div>
    </div>    
    <Footer />
  </div>
</template>
<script>
import moment from 'moment'
import webAPI from '@/api/website'
import { removeHtmlStyle } from '@/utils/common'
import TitleAndMore from '../components/TitleAndMore.vue'
import Footer from '../website-layout/Footer.vue'
export default {
  components: { TitleAndMore, Footer },
  filters: {
    filterDateTime(val) {
      return moment(val).format('YYYY-MM-DD HH:mm')
    }
  },
  data() {
    return {
      bannerList: [
        { id: 1, firstImg: '', channelCName: '安徽省教育厅关于公布2023年安徽省高校教师教学创新大赛获奖名单的通知', channelCText: '<p>近日，安徽省教育厅公布了2023年安徽省高校教师教学创新大赛获奖名单，我会多位会员单位教师获奖。</p>' },
        { id: 2, firstImg: '', channelCName: '关于举办2024年安徽省高校计算机基础课程青年教师教学竞赛的通知', channelCText: '<p>为提高我省高校计算机基础课程教学水平，发现和培养优秀青年教师，促进教师更新教学理念、掌握现代信息技术。</p>' },
        { id: 3, firstImg: '', channelCName: '2024年教育部产学合作协同育人项目申报通知', channelCText: '<p>教育部高等教育司委托我会组织开展2024年教育部产学合作协同育人项目申报工作，现将有关事项通知如下。</p>' },
        { id: 4, firstImg: '', channelCName: '关于举办第十五届蓝桥杯全国软件和信息技术专业人才大赛安徽赛区选拔赛的通知', channelCText: '<p>由工业和信息化部人才交流中心举办的第十五届蓝桥杯全国软件和信息技术专业人才大赛正在报名中。</p>' },
        { id: 5, firstImg: '', channelCName: '第八届安徽省高校微课教学比赛成绩公示', channelCText: '<p>根据《关于举办第八届安徽省高等学校微课教学比赛》的通知要求，经各高校推荐、专家评审，现将比赛结果予以公示。</p>' }
      ],
      resp: {},
      scrollNews: [
        { id: 1, firstImg: '', channelCName: '安徽省高校大学计算机课程建设交流会在安徽大学召开', channelCTime: '2024-03-05 10:30:00', channelCText: '<p>3月2日，安徽省高校大学计算机课程建设交流会在安徽大学召开。会议围绕一流课程建设标准、教学内容改革、教学方法创新等主题展开深入研讨。</p>' },
        { id: 2, firstImg: '', channelCName: '关于组织申报2024年安徽省高等学校教学研究项目的通知', channelCTime: '2024-02-28 14:20:00', channelCText: '<p>为深入贯彻全国教育大会精神，落实立德树人根本任务，提高高等教育质量，推进我省普通本科高等学校教育教学改革与创新，促进教育公平。</p>' },
        { id: 3, firstImg: '', channelCName: '研究会召开2024年度工作会议', channelCTime: '2024-02-20 09:00:00', channelCText: '<p>2月18日，安徽省高等学校计算机教育研究会2024年度工作会议在合肥召开。会议总结了2023年工作，部署了2024年重点工作任务。</p>' },
        { id: 4, firstImg: '', channelCName: '关于举办2024年安徽省高校计算机基础课程青年教师教学竞赛的通知', channelCTime: '2024-02-15 16:00:00', channelCText: '<p>为提高我省高校计算机基础课程教学水平，发现和培养优秀青年教师，促进教师更新教学理念、掌握现代信息技术。</p>' },
        { id: 5, firstImg: '', channelCName: '教育部产学合作协同育人项目对接会在合肥举行', channelCTime: '2024-01-25 11:30:00', channelCText: '<p>1月20日，教育部产学合作协同育人项目对接会在合肥举行。来自全省高校的代表与企业代表进行了深入交流，探讨产教融合新模式。</p>' },
        { id: 6, firstImg: '', channelCName: '安徽省高校计算机课程思政研讨会成功举办', channelCTime: '2024-01-15 15:40:00', channelCText: '<p>安徽省高校计算机课程思政研讨会于1月10日在合肥工业大学成功举办。会议围绕"课程思政建设与实践"主题，分享了典型案例和成功经验。</p>' }
      ],
      notices: [
        { id: 1, channelCName: '关于举办2024年安徽省高校计算机基础课程青年教师教学竞赛的通知', channelCTime: '2024-02-15 10:00:00', channelCText: '<p>为提高我省高校计算机基础课程教学水平，发现和培养优秀青年教师，促进教师更新教学理念、掌握现代信息技术，提升教学能力，根据《安徽省教育厅关于举办2024年安徽省高校教师教学竞赛的通知》要求，决定举办2024年安徽省高校计算机基础课程青年教师教学竞赛。</p>' },
        { id: 2, channelCName: '关于举办第十五届蓝桥杯全国软件和信息技术专业人才大赛安徽赛区选拔赛的通知', channelCTime: '2024-01-20 14:30:00', channelCText: '<p>由工业和信息化部人才交流中心举办的第十五届蓝桥杯全国软件和信息技术专业人才大赛正在报名中。根据组委会的安排，安徽赛区的比赛定于2024年4月13日举行。请各参赛院校高度重视，认真组织，做好参赛选手的选拔和培训工作。</p>' },
        { id: 3, channelCName: '关于举办2024年安徽省高校微课教学比赛的通知', channelCTime: '2024-01-10 09:20:00', channelCText: '<p>为深入贯彻习近平新时代中国特色社会主义思想和党的二十大精神，落实立德树人根本任务，推进高校教育教学改革，提升教师现代信息技术应用能力和教学能力，促进信息技术与教育教学的深度融合，现将举办2024年安徽省高校微课教学比赛有关事项通知如下。</p>' },
        { id: 4, channelCName: '2024年教育部产学合作协同育人项目申报通知', channelCTime: '2023-12-25 15:45:00', channelCText: '<p>教育部高等教育司委托安徽省高等学校计算机教育研究会组织开展2024年教育部产学合作协同育人项目申报工作，现将有关事项通知如下。各高校计算机及相关专业教师可根据自身教学、人才培养实际需求，结合企业提供的支持内容，自主选择项目类型、支持企业和项目内容进行申报。</p>' }
      ],
      organizationList: [
        { id: 1, channelCName: '安徽大学计算机科学与技术学院', channelCTime: '2023-01-01 00:00:00', channelCText: '<p>安徽大学计算机科学与技术学院始建于1982年，是安徽省最早设立的计算机专业之一，是研究会的理事长单位。</p>' },
        { id: 2, channelCName: '合肥工业大学计算机与信息学院', channelCTime: '2023-01-01 00:00:00', channelCText: '<p>合肥工业大学计算机与信息学院始建于1976年，是我会的副理事长单位。</p>' },
        { id: 3, channelCName: '安徽工业大学计算机科学与技术学院', channelCTime: '2023-01-01 00:00:00', channelCText: '<p>安徽工业大学计算机科学与技术学院成立于1999年，是我会的理事单位。</p>' },
        { id: 4, channelCName: '安徽师范大学计算机与信息学院', channelCTime: '2023-01-01 00:00:00', channelCText: '<p>安徽师范大学计算机与信息学院成立于1992年，是我会的理事单位。</p>' },
        { id: 5, channelCName: '安徽财经大学计算机科学与应用系', channelCTime: '2023-01-01 00:00:00', channelCText: '<p>安徽财经大学计算机科学与应用系是我会的理事单位。</p>' },
        { id: 6, channelCName: '安徽建筑大学计算机与信息学院', channelCTime: '2023-01-01 00:00:00', channelCText: '<p>安徽建筑大学计算机与信息学院是我会的理事单位。</p>' },
        { id: 7, channelCName: '合肥学院计算机与信息工程学院', channelCTime: '2023-01-01 00:00:00', channelCText: '<p>合肥学院计算机与信息工程学院是我会的理事单位。</p>' },
        { id: 8, channelCName: '安庆师范大学计算机与信息学院', channelCTime: '2023-01-01 00:00:00', channelCText: '<p>安庆师范大学计算机与信息学院是我会的理事单位。</p>' }
      ],
      raceList: [
        { id: 1, channelCName: '第十五届蓝桥杯全国软件和信息技术专业人才大赛安徽赛区选拔赛', channelCTime: '2024-01-20 14:30:00', channelCText: '<p>由工业和信息化部人才交流中心举办的第十五届蓝桥杯全国软件和信息技术专业人才大赛正在报名中。比赛设置C/C++程序设计、Java程序设计、Python程序设计等多个赛道。</p>' },
        { id: 2, channelCName: '2024年安徽省高校计算机基础课程青年教师教学竞赛', channelCTime: '2024-02-15 09:15:00', channelCText: '<p>为提高我省高校计算机基础课程教学水平，发现和培养优秀青年教师，促进教师更新教学理念、掌握现代信息技术，提升教学能力，决定举办2024年安徽省高校计算机基础课程青年教师教学竞赛。</p>' },
        { id: 3, channelCName: '2024年安徽省高校微课教学比赛', channelCTime: '2024-01-10 11:00:00', channelCText: '<p>为深入贯彻习近平新时代中国特色社会主义思想和党的二十大精神，落实立德树人根本任务，推进高校教育教学改革，提升教师现代信息技术应用能力和教学能力，促进信息技术与教育教学的深度融合。</p>' },
        { id: 4, channelCName: '第八届安徽省高校微课教学比赛成绩公示', channelCTime: '2023-12-10 16:30:00', channelCText: '<p>根据《关于举办第八届安徽省高等学校微课教学比赛》的通知要求，经各高校推荐、专家评审，现将比赛结果予以公示。</p>' }
      ],
      activityIndex: 0,
      typeList: [],
      currentType: '',
      popper: false,
      members: [],
      introduce: '',
      defaultImg: require('@/assets/img/default.png'),
      bannerImg: require('@/assets/img/banner.png'),
      observer: null,
      carouselCheckInterval: null
    }
  },
  watch: {
    currentType(newVal, oldVal) {
      if (this.resp[newVal]) {
        this.scrollNews = this.resp[newVal]
      }
    }
  },
  created() {
    this.typeList = this.$store.state.webSite.routeList.filter(e => e.name === 'NewsCenter')[0].meta.children
    this.currentType = this.typeList[0].title
    // 初始化响应数据结构
    if (!this.resp) this.resp = {}
    this.resp[this.currentType] = this.scrollNews
    
    // 确保轮播图数据有效
    this.validateBannerList()
    
    // 添加多次强制刷新逻辑
    setTimeout(() => {
      this.$forceUpdate()
      console.log('Homepage forced update after 300ms')
    }, 300)
    
    setTimeout(() => {
      this.$forceUpdate()
      console.log('Homepage forced update after 1000ms')
    }, 1000)
  },
  mounted() {
    console.log('Homepage mounted, data loaded:', {
      bannerList: this.bannerList,
      scrollNews: this.scrollNews,
      notices: this.notices,
      organizationList: this.organizationList,
      raceList: this.raceList
    })
    
    this.$nextTick(() => {
      this.setupIntersectionObserver()
      this.initCarousel()
      this.$forceUpdate()
      console.log('Homepage forced update in mounted')
    })
    
    // 多次强制刷新确保内容始终显示
    for (let i = 1; i <= 5; i++) {
      setTimeout(() => {
        this.$forceUpdate()
        if (i === 2 || i === 4) {
          this.initCarousel() // 再次尝试初始化轮播
        }
        console.log(`Homepage forced update #${i} after ${i * 500}ms`)
      }, i * 500)
    }
    
    // 添加窗口失焦/获取焦点事件处理
    window.addEventListener('blur', this.handleWindowBlur)
    window.addEventListener('focus', this.handleWindowFocus)
    
    // 添加定时器确保轮播正常工作
    this.carouselCheckInterval = setInterval(this.checkCarouselStatus, 3000)
  },
  beforeDestroy() {
    // 清理 Intersection Observer
    if (this.observer) {
      this.observer.disconnect()
    }
    
    // 清理事件监听和定时器
    window.removeEventListener('blur', this.handleWindowBlur)
    window.removeEventListener('focus', this.handleWindowFocus)
    
    if (this.carouselCheckInterval) {
      clearInterval(this.carouselCheckInterval)
    }
  },
  methods: {
    removeHtmlStyle(html) {
      return removeHtmlStyle(html)
    },
    setupIntersectionObserver() {
      // 检查浏览器是否支持 Intersection Observer API
      if ('IntersectionObserver' in window) {
        const options = {
          root: null, // 使用视口作为根元素
          rootMargin: '0px',
          threshold: 0.1 // 当元素10%进入视口时触发
        }
        
        this.observer = new IntersectionObserver((entries, observer) => {
          entries.forEach(entry => {
            if (entry.isIntersecting) {
              // 添加一个特殊的类来触发动画
              entry.target.classList.add('is-visible')
              // 一旦触发，就不再观察该元素
              observer.unobserve(entry.target)
            }
          })
        }, options)
        
        // 观察所有需要动画的元素
        document.querySelectorAll('.animate-in').forEach(el => {
          this.observer.observe(el)
        })
        
        document.querySelectorAll('.feature-item').forEach(el => {
          this.observer.observe(el)
        })
      }
    },
    async getData() {
      // 不执行任何获取逻辑，防止覆盖
      console.log('getData方法已禁用，使用静态内容')
    },
    addDemoData() {
      console.log('Adding demo data to Homepage')
      
      // 添加演示banner数据
      if (!this.bannerList || this.bannerList.length === 0) {
        this.bannerList = [
          {
            id: 1,
            title: '安徽省高等学校计算机教育研究会',
            description: '致力于促进安徽省高等学校计算机教育研究与发展',
            actionText: '了解更多',
            link: '/website/about'
          },
          {
            id: 2,
            title: '2024年度学术交流大会',
            description: '汇聚教育精英，共商计算机教育发展大计',
            actionText: '查看详情',
            link: '/website/news'
          },
          {
            id: 3,
            title: '第十五届蓝桥杯大赛',
            description: '提升编程实践能力，培养创新型IT人才',
            actionText: '立即报名',
            link: '/website/competition'
          },
          {
            id: 4,
            title: '师资培训计划启动',
            description: '提升教师教学水平，促进教学质量全面提升',
            actionText: '参与培训',
            link: '/website/notice'
          },
          {
            id: 5,
            title: '产学研合作新模式',
            description: '深化校企合作，促进计算机学科发展与产业需求对接',
            actionText: '合作详情',
            link: '/website/organization'
          }
        ]
      }
      
      // 添加滚动新闻示例数据
      const newsItems = [
        { id: 1, firstImg: '', channelCName: '安徽省高校大学计算机课程建设交流会在安徽大学召开', channelCTime: '2024-03-05 10:30:00', channelCText: '<p>3月2日，安徽省高校大学计算机课程建设交流会在安徽大学召开。会议围绕一流课程建设标准、教学内容改革、教学方法创新等主题展开深入研讨。</p>' },
        { id: 2, firstImg: '', channelCName: '关于组织申报2024年安徽省高等学校教学研究项目的通知', channelCTime: '2024-02-28 14:20:00', channelCText: '<p>为深入贯彻全国教育大会精神，落实立德树人根本任务，提高高等教育质量，推进我省普通本科高等学校教育教学改革与创新，促进教育公平。</p>' },
        { id: 3, firstImg: '', channelCName: '研究会召开2024年度工作会议', channelCTime: '2024-02-20 09:00:00', channelCText: '<p>2月18日，安徽省高等学校计算机教育研究会2024年度工作会议在合肥召开。会议总结了2023年工作，部署了2024年重点工作任务。</p>' },
        { id: 4, firstImg: '', channelCName: '关于举办2024年安徽省高校计算机基础课程青年教师教学竞赛的通知', channelCTime: '2024-02-15 16:00:00', channelCText: '<p>为提高我省高校计算机基础课程教学水平，发现和培养优秀青年教师，促进教师更新教学理念、掌握现代信息技术。</p>' },
        { id: 5, firstImg: '', channelCName: '教育部产学合作协同育人项目对接会在合肥举行', channelCTime: '2024-01-25 11:30:00', channelCText: '<p>1月20日，教育部产学合作协同育人项目对接会在合肥举行。来自全省高校的代表与企业代表进行了深入交流，探讨产教融合新模式。</p>' },
        { id: 6, firstImg: '', channelCName: '安徽省高校计算机课程思政研讨会成功举办', channelCTime: '2024-01-15 15:40:00', channelCText: '<p>安徽省高校计算机课程思政研讨会于1月10日在合肥工业大学成功举办。会议围绕"课程思政建设与实践"主题，分享了典型案例和成功经验。</p>' }
      ]
      
      this.scrollNews = newsItems
      if (!this.resp) this.resp = {}
      this.resp[this.currentType] = newsItems
      
      // 添加通知公告示例数据
      this.notices = [
        { id: 1, channelCName: '关于举办2024年安徽省高校计算机基础课程青年教师教学竞赛的通知', channelCTime: '2024-02-15 10:00:00', channelCText: '<p>为提高我省高校计算机基础课程教学水平，发现和培养优秀青年教师，促进教师更新教学理念、掌握现代信息技术，提升教学能力，根据《安徽省教育厅关于举办2024年安徽省高校教师教学竞赛的通知》要求，决定举办2024年安徽省高校计算机基础课程青年教师教学竞赛。</p>' },
        { id: 2, channelCName: '关于举办第十五届蓝桥杯全国软件和信息技术专业人才大赛安徽赛区选拔赛的通知', channelCTime: '2024-01-20 14:30:00', channelCText: '<p>由工业和信息化部人才交流中心举办的第十五届蓝桥杯全国软件和信息技术专业人才大赛正在报名中。根据组委会的安排，安徽赛区的比赛定于2024年4月13日举行。请各参赛院校高度重视，认真组织，做好参赛选手的选拔和培训工作。</p>' },
        { id: 3, channelCName: '关于举办2024年安徽省高校微课教学比赛的通知', channelCTime: '2024-01-10 09:20:00', channelCText: '<p>为深入贯彻习近平新时代中国特色社会主义思想和党的二十大精神，落实立德树人根本任务，推进高校教育教学改革，提升教师现代信息技术应用能力和教学能力，促进信息技术与教育教学的深度融合，现将举办2024年安徽省高校微课教学比赛有关事项通知如下。</p>' },
        { id: 4, channelCName: '2024年教育部产学合作协同育人项目申报通知', channelCTime: '2023-12-25 15:45:00', channelCText: '<p>教育部高等教育司委托安徽省高等学校计算机教育研究会组织开展2024年教育部产学合作协同育人项目申报工作，现将有关事项通知如下。各高校计算机及相关专业教师可根据自身教学、人才培养实际需求，结合企业提供的支持内容，自主选择项目类型、支持企业和项目内容进行申报。</p>' }
      ]
      
      // 添加组织机构示例数据
      this.organizationList = [
        { id: 1, channelCName: '安徽大学计算机科学与技术学院', channelCTime: '2023-01-01 00:00:00', channelCText: '<p>安徽大学计算机科学与技术学院始建于1982年，是安徽省最早设立的计算机专业之一，是研究会的理事长单位。</p>' },
        { id: 2, channelCName: '合肥工业大学计算机与信息学院', channelCTime: '2023-01-01 00:00:00', channelCText: '<p>合肥工业大学计算机与信息学院始建于1976年，是我会的副理事长单位。</p>' },
        { id: 3, channelCName: '安徽工业大学计算机科学与技术学院', channelCTime: '2023-01-01 00:00:00', channelCText: '<p>安徽工业大学计算机科学与技术学院成立于1999年，是我会的理事单位。</p>' },
        { id: 4, channelCName: '安徽师范大学计算机与信息学院', channelCTime: '2023-01-01 00:00:00', channelCText: '<p>安徽师范大学计算机与信息学院成立于1992年，是我会的理事单位。</p>' },
        { id: 5, channelCName: '安徽财经大学计算机科学与应用系', channelCTime: '2023-01-01 00:00:00', channelCText: '<p>安徽财经大学计算机科学与应用系是我会的理事单位。</p>' },
        { id: 6, channelCName: '安徽建筑大学计算机与信息学院', channelCTime: '2023-01-01 00:00:00', channelCText: '<p>安徽建筑大学计算机与信息学院是我会的理事单位。</p>' },
        { id: 7, channelCName: '合肥学院计算机与信息工程学院', channelCTime: '2023-01-01 00:00:00', channelCText: '<p>合肥学院计算机与信息工程学院是我会的理事单位。</p>' },
        { id: 8, channelCName: '安庆师范大学计算机与信息学院', channelCTime: '2023-01-01 00:00:00', channelCText: '<p>安庆师范大学计算机与信息学院是我会的理事单位。</p>' }
      ]
      
      // 添加竞赛专栏示例数据
      this.raceList = [
        { id: 1, channelCName: '第十五届蓝桥杯全国软件和信息技术专业人才大赛安徽赛区选拔赛', channelCTime: '2024-01-20 14:30:00', channelCText: '<p>由工业和信息化部人才交流中心举办的第十五届蓝桥杯全国软件和信息技术专业人才大赛正在报名中。比赛设置C/C++程序设计、Java程序设计、Python程序设计等多个赛道。</p>' },
        { id: 2, channelCName: '2024年安徽省高校计算机基础课程青年教师教学竞赛', channelCTime: '2024-02-15 09:15:00', channelCText: '<p>为提高我省高校计算机基础课程教学水平，发现和培养优秀青年教师，促进教师更新教学理念、掌握现代信息技术，提升教学能力，决定举办2024年安徽省高校计算机基础课程青年教师教学竞赛。</p>' },
        { id: 3, channelCName: '2024年安徽省高校微课教学比赛', channelCTime: '2024-01-10 11:00:00', channelCText: '<p>为深入贯彻习近平新时代中国特色社会主义思想和党的二十大精神，落实立德树人根本任务，推进高校教育教学改革，提升教师现代信息技术应用能力和教学能力，促进信息技术与教育教学的深度融合。</p>' },
        { id: 4, channelCName: '第八届安徽省高校微课教学比赛成绩公示', channelCTime: '2023-12-10 16:30:00', channelCText: '<p>根据《关于举办第八届安徽省高等学校微课教学比赛》的通知要求，经各高校推荐、专家评审，现将比赛结果予以公示。</p>' }
      ]
      
      // 强制更新视图
      this.$nextTick(() => {
        this.$forceUpdate()
        console.log('Homepage content updated with actual data')
      })
    },
    toDetail(item, type) {
      // 为了防止跳转到空白页面，我们需要使用正确的路由路径并传递参数
      // type: 1-新闻中心, 2-通知公告, 3-组织机构, 4-竞赛专栏
      
      // 首先确保有效的ID
      if (!item || !item.id) {
        console.error('无效的项目或ID', item);
        return;
      }
      
      // 将完整对象序列化传递
      const query = { row: JSON.stringify(item), type: type };
      
      // 使用Router定义的通用详情页路径
      this.$router.push({
        path: '/website/commonDetail',
        query: query
      });
    },
    viewMore(path, index) {
      if (index !== undefined) {
        this.$store.commit('websiteStore/SET_ACTIVITY_INDEX', index)
      }
      this.$router.push(path)
    },
    changeType(index) {
      this.activityIndex = index
      if (this.resp && this.resp[this.typeList[index].title]) {
        this.scrollNews = this.resp[this.typeList[index].title]
      } else {
        this.scrollNews = this.scrollNews
      }
      this.currentType = this.typeList[index].title
      this.popper = false
    },
    getColorByIndex(index) {
      const colors = ['#3776FF', '#4285F4', '#228B22', '#FF6B6B', '#6B66FF', '#FF8C42']
      return colors[index % colors.length]
    },
    getIconByIndex(index) {
      const icons = ['el-icon-document', 'el-icon-news', 'el-icon-bell', 'el-icon-message', 'el-icon-reading', 'el-icon-collection'];
      return icons[index % icons.length];
    },
    getBadgeText(index) {
      const statusMap = ['进行中', '已结束', '即将开始', '报名中']
      return statusMap[index % statusMap.length]
    },
    getBannerImage(index) {
      // 使用纯CSS渐变代替外部图片
      return '';
    },
    getNewsImage(index) {
      // 使用本地图片资源
      return '';
    },
    getOrgLogo(index) {
      // 使用本地图片资源
      return '';
    },
    validateBannerList() {
      // 确保轮播图数据有效
      if (!this.bannerList || this.bannerList.length === 0) {
        this.bannerList = [
          { id: 1, firstImg: '', channelCName: '安徽省教育厅关于公布2023年安徽省高校教师教学创新大赛获奖名单的通知', channelCText: '<p>近日，安徽省教育厅公布了2023年安徽省高校教师教学创新大赛获奖名单，我会多位会员单位教师获奖。</p>' },
          { id: 2, firstImg: '', channelCName: '关于举办2024年安徽省高校计算机基础课程青年教师教学竞赛的通知', channelCText: '<p>为提高我省高校计算机基础课程教学水平，发现和培养优秀青年教师，促进教师更新教学理念、掌握现代信息技术。</p>' },
          { id: 3, firstImg: '', channelCName: '2024年教育部产学合作协同育人项目申报通知', channelCText: '<p>教育部高等教育司委托我会组织开展2024年教育部产学合作协同育人项目申报工作，现将有关事项通知如下。</p>' },
          { id: 4, firstImg: '', channelCName: '关于举办第十五届蓝桥杯全国软件和信息技术专业人才大赛安徽赛区选拔赛的通知', channelCText: '<p>由工业和信息化部人才交流中心举办的第十五届蓝桥杯全国软件和信息技术专业人才大赛正在报名中。</p>' },
          { id: 5, firstImg: '', channelCName: '第八届安徽省高校微课教学比赛成绩公示', channelCText: '<p>根据《关于举办第八届安徽省高等学校微课教学比赛》的通知要求，经各高校推荐、专家评审，现将比赛结果予以公示。</p>' }
        ]
      }
    },
    handleWindowBlur() {
      // 窗口失去焦点时暂停轮播
      if (this.$refs.carousel) {
        this.$refs.carousel.pause()
      }
    },
    handleWindowFocus() {
      // 窗口获得焦点时恢复轮播并刷新页面
      if (this.$refs.carousel) {
        this.$refs.carousel.setActiveItem(0)
        this.$refs.carousel.play()
      }
      this.$forceUpdate()
    },
    checkCarouselStatus() {
      // 检查轮播状态是否正常，如果不正常则重置
      const carousel = this.$refs.carousel
      if (carousel) {
        const activeItem = this.$el.querySelector('.el-carousel__item.is-active')
        if (!activeItem) {
          console.log('轮播异常，尝试修复...')
          carousel.setActiveItem(0)
          this.$forceUpdate()
        }
      }
    },
    initCarousel() {
      // 手动初始化轮播组件
      const carousel = this.$el.querySelector('.el-carousel')
      if (carousel) {
        console.log('Initializing carousel')
        // 添加初始化类
        carousel.classList.add('carousel-initialized')
        
        // 找到轮播项，确保它们有正确的类名
        const items = carousel.querySelectorAll('.el-carousel__item')
        items.forEach((item, index) => {
          // 确保轮播项有正确的类名
          item.classList.add(`carousel-bg-${index % 5 + 1}`)
        })
        
        // 强制激活第一个轮播项
        if (this.$refs.carousel) {
          setTimeout(() => {
            this.$refs.carousel.setActiveItem(0)
          }, 100)
        }
      }
    },
    handleBannerAction(item) {
      // 处理轮播图点击事件
      if (item.link) {
        if (item.link.startsWith('http')) {
          window.open(item.link, '_blank');
        } else {
          this.$router.push(item.link);
        }
      } else {
        // 使用与toDetail相同的逻辑，确保能够正确跳转
        const query = { row: JSON.stringify(item), type: 1 }; // 轮播图默认作为新闻处理
        this.$router.push({
          path: '/website/commonDetail',
          query: query
        });
      }
    }
  }
}
</script>

<style lang="scss" scoped>
@media screen and (max-width: 765px) {
   .box {
    display: none;
  }
}

::v-deep.Homepage {
  font-family: PingFang SC;
  
  /* 添加滚动条隐藏样式 */
  .el-carousel {
    overflow: hidden;
    
    .el-carousel__container {
      overflow: hidden;
    }
    
    .el-carousel__indicators {
      overflow: hidden;
    }
    
    &::-webkit-scrollbar {
      display: none;
    }
  }
  
  /* 动画效果 */
  .animate-in {
    opacity: 0;
    transform: translateY(30px);
    transition: opacity 0.8s ease-out, transform 0.8s ease-out;
    
    &.is-visible {
      opacity: 1;
      transform: translateY(0);
    }
  }
  
  .feature-item {
    opacity: 0;
    transform: translateY(30px);
    transition: opacity 0.8s ease-out, transform 0.8s ease-out;
    
    &.is-visible {
      opacity: 1;
      transform: translateY(0);
    }
  }
  
  .carousel-bg-1 {
    background: linear-gradient(135deg, #3a8ee6, #13c2c2);
  }
  .carousel-bg-2 {
    background: linear-gradient(135deg, #6554c0, #8a54ff);
  }
  .carousel-bg-3 {
    background: linear-gradient(135deg, #f5222d, #fa8c16);
  }
  .carousel-bg-4 {
    background: linear-gradient(135deg, #1890ff, #52c41a);
  }
  .carousel-bg-5 {
    background: linear-gradient(135deg, #722ed1, #eb2f96);
  }
  
  .news-bg-1 {
    background: linear-gradient(135deg, #3776FF, #1E40AF);
  }
  .news-bg-2 {
    background: linear-gradient(135deg, #4285F4, #2563EB);
  }
  .news-bg-3 {
    background: linear-gradient(135deg, #228B22, #16A34A);
  }
  .news-bg-4 {
    background: linear-gradient(135deg, #F59E0B, #D97706);
  }
  .news-bg-5 {
    background: linear-gradient(135deg, #8B5CF6, #6D28D9);
  }
  .news-bg-6 {
    background: linear-gradient(135deg, #EC4899, #BE185D);
  }
  
  .notice-bg-1 {
    border-top: 4px solid #3776FF;
  }
  .notice-bg-2 {
    border-top: 4px solid #16A34A;
  }
  .notice-bg-3 {
    border-top: 4px solid #F59E0B;
  }
  .notice-bg-4 {
    border-top: 4px solid #8B5CF6;
  }
  
  .org-bg-1 {
    background: #dce6fe;
    .logo-placeholder {
      color: #3776FF;
    }
  }
  .org-bg-2 {
    background: #d1e0fe;
    .logo-placeholder {
      color: #4285F4;
    }
  }
  .org-bg-3 {
    background: #d1f5e0;
    .logo-placeholder {
      color: #228B22;
    }
  }
  .org-bg-4 {
    background: #fde68a;
    .logo-placeholder {
      color: #D97706;
    }
  }
  .org-bg-5 {
    background: #e2d9f3;
    .logo-placeholder {
      color: #6D28D9;
    }
  }
  .org-bg-6 {
    background: #fad2ea;
    .logo-placeholder {
      color: #BE185D;
    }
  }
  .org-bg-7 {
    background: #d1f5f5;
    .logo-placeholder {
      color: #0E7490;
    }
  }
  .org-bg-8 {
    background: #fecaca;
    .logo-placeholder {
      color: #DC2626;
    }
  }
  
  .comp-bg-1 {
    border-left: 4px solid #3776FF;
  }
  .comp-bg-2 {
    border-left: 4px solid #16A34A;
  }
  .comp-bg-3 {
    border-left: 4px solid #F59E0B;
  }
  .comp-bg-4 {
    border-left: 4px solid #8B5CF6;
  }
  
  .el-carousel__item{
    background-size: cover !important;
    position: relative;
    overflow: hidden;
    
    .banner-decoration {
      position: absolute;
      width: 600px;
      height: 600px;
      background: rgba(255,255,255,0.1);
      border-radius: 50%;
      right: -200px;
      top: -100px;
      display: flex;
      align-items: center;
      justify-content: center;
      
      i {
        font-size: 120px;
        color: rgba(255,255,255,0.3);
        margin-left: -150px;
        margin-top: -50px;
      }
      
      &:before {
        content: '';
        position: absolute;
        width: 400px;
        height: 400px;
        background: rgba(255,255,255,0.1);
        border-radius: 50%;
        right: -100px;
        bottom: -100px;
      }
    }
    
    .banner-content {
      position: absolute;
      top: 50%;
      left: 10%;
      transform: translateY(-50%);
      color: white;
      text-shadow: 0 2px 4px rgba(0,0,0,0.3);
      max-width: 600px;
      z-index: 2;
      
      .banner-title {
        font-size: 48px;
        font-weight: bold;
        margin-bottom: 20px;
        animation: fadeInUp 0.8s ease-out;
      }
      
      .banner-text {
        font-size: 18px;
        line-height: 1.6;
        margin-bottom: 40px;
        animation: fadeInUp 0.8s ease-out 0.2s both;
      }
      
      .banner-button {
        display: inline-block;
        padding: 12px 30px;
        background-color: rgba(255,255,255,0.2);
        border: 1px solid rgba(255,255,255,0.4);
        border-radius: 5px;
        font-size: 16px;
        transition: all 0.3s;
    cursor: pointer;
        animation: fadeInUp 0.8s ease-out 0.4s both;
        
        i {
          margin-left: 5px;
          transition: transform 0.3s;
        }
        
        &:hover {
          background-color: rgba(255,255,255,0.3);
          transform: translateY(-2px);
          
          i {
            transform: translateX(3px);
          }
        }
      }
    }
  }
  
  .feature-section {
    background: #fff;
    padding: 30px 0;
    margin-bottom: 20px;
    box-shadow: 0 4px 12px rgba(0,0,0,0.05);
      position: relative;
    
    &:before {
      content: '';
        position: absolute;
      top: 0;
      left: 0;
      right: 0;
      height: 3px;
      background: linear-gradient(to right, #3776FF, #1D4ED8, #16A34A, #F59E0B, #8B5CF6);
    }
    
    .feature-container {
      max-width: 1200px;
      margin: 0 auto;
      display: flex;
      justify-content: space-between;
      
      .feature-item {
        flex: 1;
        display: flex;
        align-items: center;
        padding: 20px;
        border-radius: 8px;
        transition: all 0.3s;
        cursor: pointer;
        position: relative;
        z-index: 1;
        
        &:before {
          content: '';
          position: absolute;
          top: 0;
          left: 0;
          right: 0;
          bottom: 0;
          background: #f8faff;
          border-radius: 8px;
          z-index: -1;
          opacity: 0;
          transition: all 0.3s;
        }
        
        &:hover {
          transform: translateY(-5px); // 向上浮动
          
          &:before {
            opacity: 1;
          }
          
          .feature-icon {
            transform: rotateY(180deg);
            // y轴翻转180
          }
        }
        
        .feature-icon {
          width: 60px;
          height: 60px;
          background: #3776FF;
          border-radius: 12px;
          display: flex;
          align-items: center;
          justify-content: center;
          margin-right: 15px;
          transition: all 0.5s;// 0.5秒
          
          i {
      font-size: 28px;
            color: white;
          }
        }
        
        &:nth-child(2) .feature-icon {
          background: #16A34A;
        }
        
        &:nth-child(3) .feature-icon {
          background: #F59E0B;
        }
        
        &:nth-child(4) .feature-icon {
          background: #8B5CF6;
        }
        
        .feature-text {
          .feature-title {
            font-size: 18px;
            font-weight: bold;
            color: #333;
            margin-bottom: 6px;
          }
          
          .feature-desc {
            font-size: 14px;
            color: #666;
          }
        }
      }
    }
  }
  
  .content {
    width: 1400px;
    margin-left: 50%;
    transform: translateX(-50%);
    .show-popover {
      background-color: #3776ff;
      color: #fff;
      width: 20px;
      height: 20px;
      border-radius: 50%;
      line-height: 20px;
      text-align: center;
      position: absolute;
      top: 10px;
      left: 170px;
      cursor: pointer;
    }
    .first-list {
      display: flex;
      flex-wrap: wrap;
      justify-content: space-between;
      margin: 0 -15px;
      .news-item:nth-child(n + 3) {
        margin-top: 30px;
      }
      .news-item {
        width: calc(33.33% - 30px);
        margin: 0 15px 40px;
        background: #FFFFFF;
        border-radius: 8px;
        overflow: hidden;
        box-shadow: 0 5px 15px rgba(0,0,0,0.05);
        transition: all .3s;
        cursor: pointer;
        
        &:hover {
          transform: translateY(-10px);
          box-shadow: 0 15px 30px rgba(0,0,0,0.1);
        }
        
        .news-img-container {
          height: 180px;
          overflow: hidden;
          
          .news-img-placeholder {
            width: 100%;
          height: 100%;
            display: flex;
            align-items: center;
            justify-content: center;
            transition: all 0.5s;
            
            i {
              font-size: 40px;
              color: white;
            }
          }
          
          &:hover .news-img-placeholder {
            transform: scale(1.1);
          }
        }
        .news-content {
          flex: 1;
          padding: 20px;
          .news-title {
            font-size: 20px;
            font-weight: 600;
            color: #333;
            width: 100%;
            overflow: hidden;
            text-overflow: ellipsis;
            white-space: nowrap;
            margin-bottom: 10px;
          }
          .news-time {
            font-size: 14px;
            font-weight: 400;
            color: #999;
            margin-bottom: 10px;
            
            i {
              margin-right: 4px;
            }
          }
          .news-remarks {
            font-size: 16px;
            font-weight: 400;
            color: #666;
            width: 100%;
            height: 80px;
            line-height: 26px;
            display: -webkit-box;
            overflow: hidden;
            white-space: normal;
            text-overflow: ellipsis;
            -webkit-line-clamp: 3;
            -webkit-box-orient: vertical;
          }
        }
      }
    }
    .second-list {
      display: flex;
      .notice-item {
        width: 320px;
        height: 220px;
        background: #FFFFFF;
        box-shadow: 0 1px 4px rgba(0,0,0,.1);
        box-sizing: border-box;
        border-radius: 8px;
        cursor: pointer;
        transition: 0.4s cubic-bezier(0.175, 0.885, 0.32, 1.275);
        position: relative;
        overflow: hidden;
        
        &:hover {
          box-shadow: 0 15px 30px rgba(0,0,0,.1);
          transform: translateY(-5px) scale(1.02);
        }
        .notice-top {
          width: 100%;
          height: 80px;
          box-sizing: border-box;
          border-radius: 8px 8px 0 0;
          background-color: #fafdff;
          padding: 16px 20px;
          overflow: hidden;
          
          .notice-title {
            display: block;
            font-size: 16px;
            font-weight: 700;
            color: #333333;
            word-break: break-all;
            text-overflow: ellipsis;
            overflow: hidden;
            display: -webkit-box;
            -webkit-line-clamp: 1;
            -webkit-box-orient: vertical;
            margin-bottom: 10px;
          }
          .notice-time {
            display: block;
            color: #666666;
            font-size: 14px;
            
            i {
              margin-right: 4px;
            }
          }
        }
        .notice-content {
          padding: 16px 20px;
          font-size: 14px;
          color: #666666;
          line-height: 22px;
          word-break: break-all;
          text-overflow: ellipsis;
          overflow: hidden;
          display: -webkit-box;
          -webkit-line-clamp: 5;
          -webkit-box-orient: vertical;
        }
      }
      .notice-item + .notice-item {
        margin-left: 40px;
      }
    }
  }
  .organization-bg {
    background: linear-gradient(135deg, #f1f5ff, #e4eaff);
    background-repeat: no-repeat;
    background-size: cover;
    background-position: center;
    padding: 30px 0 70px;
    margin-top: 50px;
    .organization-content {
      width: 1200px;
      margin: 0 auto;
      .organization-list {
        display: flex;
        flex-wrap: wrap;
        margin: 0 -15px;
        .organization-item {
          margin: 0 15px 30px;
          width: 270px;
          height: 100px;
          background: #FFFFFF;
          border-radius: 8px;
          border: 1px solid #E2E6EF;
          display: flex;
          padding: 0 25px;
          align-items: center;
          cursor: pointer;
          transition: all .4s cubic-bezier(0.175, 0.885, 0.32, 1.275);
          position: relative;
          overflow: hidden;
          
          &:after {
            content: '';
            position: absolute;
            left: 0;
            top: 0;
            bottom: 0;
            width: 3px;
            background: linear-gradient(to bottom, #3776FF, #1D4ED8);
            transform: scaleY(0);
            transition: transform .3s ease;
            transform-origin: 0 0;
          }
          
          &:hover {
            box-shadow: 0 15px 30px rgba(0,0,0,.1);
            transform: translateY(-5px);
            
            &:after {
              transform: scaleY(1);
            }
            
          .circle {
              transform: scale(1.1);
            }
          }
          .circle {
            width: 70px;
            height: 70px;
            border-radius: 50%;
            display: flex;
            align-items: center;
            justify-content: center;
            overflow: hidden;
            
            .logo-placeholder {
              width: 100%;
              height: 100%;
              border-radius: 50%;
              display: flex;
              align-items: center;
              justify-content: center;
              
              i {
                font-size: 30px;
                color: white;
              }
            }
          }
          .organization-name {
            margin-left: 20px;
            font-size: 16px;
            font-weight: 500;
            color: #333;
            flex: 1;
            overflow: hidden;
            text-overflow: ellipsis;
            white-space: nowrap;
          }
        }
      }
    }
  }
  .competition {
    margin: 50px 0;
    .competition-content {
      width: 1400px;
      margin-left: 50%;
      transform: translateX(-50%);
      
      .competition-container {
        display: flex;
        flex-wrap: wrap;
        justify-content: space-between;
        
      .competition-item {
          width: 49%;
          background-color: #FFFFFF;
        border: 1px solid #ededed;
          border-radius: 8px;
          padding: 24px 30px;
          margin-bottom: 20px;
        box-sizing: border-box;
          position: relative;
        overflow: hidden;
        cursor: pointer;
        transition: 0.4s cubic-bezier(0.175, 0.885, 0.32, 1.275);
          
          .competition-badge {
            position: absolute;
            top: 12px;
            right: -30px;
            background: #3776FF;
            color: white;
            padding: 4px 30px;
            font-size: 12px;
            transform: rotate(45deg);
          }
          
          &.comp-bg-2 .competition-badge {
            background: #16A34A;
          }
          
          &.comp-bg-3 .competition-badge {
            background: #F59E0B;
          }
          
          &.comp-bg-4 .competition-badge {
            background: #8B5CF6;
          }
          
        &:hover {
            box-shadow: 0 15px 30px rgba(0,0,0,.1);
            transform: translateY(-5px) scale(1.02);
            
            .competition-action i {
              transform: translateX(8px);
            }
          }
          
        .competition-title {
            display: block;
            font-size: 18px;
          font-weight: 700;
          color: #333333;
            margin-bottom: 10px;
          word-break: break-all;
          text-overflow: ellipsis;
          overflow: hidden;
          display: -webkit-box;
          -webkit-line-clamp: 1;
          -webkit-box-orient: vertical;
        }
          
        .competition-time {
            display: block;
            font-size: 14px;
          color: #808080;
            margin-bottom: 15px;
            
            i {
              margin-right: 4px;
        }
        }
          
        .competition-remarks {
            display: block;
          font-size: 14px;
          color: #666666;
            line-height: 22px;
          word-break: break-all;
          text-overflow: ellipsis;
          overflow: hidden;
          display: -webkit-box;
          -webkit-line-clamp: 2;
          -webkit-box-orient: vertical;
            margin-bottom: 15px;
          }
          
          .competition-action {
            display: inline-block;
            font-size: 14px;
            color: #3776FF;
            font-weight: 500;
            cursor: pointer;
            
            i {
              margin-left: 4px;
              transition: transform 0.3s;
            }
          }
        }
      }
    }
  }
}

@keyframes fadeInUp {
  from {
    opacity: 0;
    transform: translateY(30px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}

@keyframes fadeIn {
  from {
    opacity: 0;
  }
  to {
    opacity: 1;
  }
}

/* 新增渐进式滚动动画 */
.first-list .news-item:nth-child(even) {
  transform: translateX(30px);
  
  &.is-visible {
    transform: translateX(0);
  }
}

.first-list .news-item:nth-child(odd) {
  transform: translateX(-30px);
  
  &.is-visible {
    transform: translateX(0);
  }
}

.second-list .notice-item:nth-child(even) {
  transform: translateY(30px);
  
  &.is-visible {
    transform: translateY(0);
  }
}

.second-list .notice-item:nth-child(odd) {
  transform: translateY(-30px);
  
  &.is-visible {
    transform: translateY(0);
  }
}

.organization-list .organization-item:nth-child(even) {
  transform: scale(0.9) translateY(20px);
  
  &.is-visible {
    transform: scale(1) translateY(0);
  }
}

.organization-list .organization-item:nth-child(odd) {
  transform: scale(0.9) translateY(-20px);
  
  &.is-visible {
    transform: scale(1) translateY(0);
  }
}

.competition-container .competition-item:nth-child(even) {
  transform: translateX(30px);
  
  &.is-visible {
    transform: translateX(0);
  }
}

.competition-container .competition-item:nth-child(odd) {
  transform: translateX(-30px);
  
  &.is-visible {
    transform: translateX(0);
  }
}

/* 添加轮播图样式优化 */
.el-carousel {
  .el-carousel__arrow {
    background-color: rgba(31, 45, 61, 0.3);
    &:hover {
      background-color: rgba(31, 45, 61, 0.5);
    }
  }
  
  .el-carousel__indicators {
    z-index: 10;
    
    .el-carousel__indicator {
      .el-carousel__button {
        background-color: rgba(255, 255, 255, 0.6);
        width: 10px;
        height: 10px;
        border-radius: 50%;
      }
      
      &.is-active .el-carousel__button {
        background-color: #fff;
      }
    }
  }
}

/* 强制激活轮播效果 */
.el-carousel__item {
  position: absolute;
  top: 0;
  left: 0;
    width: 100%;
  height: 100%;
  display: inline-block;
  transition: transform 0.4s ease;
    overflow: hidden;
}

.carousel-container {
  position: relative;
  width: 100%;
      overflow: hidden;
  
  .carousel-content {
    position: absolute;
    top: 50%;
    left: 10%;
    transform: translateY(-50%);
    color: #fff;
    text-align: left;
    z-index: 2;
    width: 50%;
    
    h3 {
      font-size: 48px;
      margin-bottom: 20px;
      font-weight: bold;
      text-shadow: 0 2px 4px rgba(0, 0, 0, 0.3);
    }
    
    .description {
      font-size: 22px;
      margin-bottom: 30px;
      line-height: 1.5;
      text-shadow: 0 1px 3px rgba(0, 0, 0, 0.3);
    }
    
    .banner-actions {
      .el-button {
        padding: 12px 24px;
        font-size: 16px;
        font-weight: 500;
        transition: all 0.3s;
        
          &:hover {
          transform: translateY(-2px);
          box-shadow: 0 5px 15px rgba(0, 0, 0, 0.2);
        }
      }
    }
  }
  
  .animated-text {
    animation: fadeInUp 0.8s both;
    
    &:nth-child(2) {
      animation-delay: 0.2s;
    }
    
    &:nth-child(3) {
      animation-delay: 0.4s;
    }
  }
}

@keyframes fadeInUp {
  from {
    opacity: 0;
    transform: translateY(30px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}

.carousel-bg-1 {
  background: linear-gradient(135deg, #3a8ee6, #13c2c2);
}
.carousel-bg-2 {
  background: linear-gradient(135deg, #6554c0, #8a54ff);
}
.carousel-bg-3 {
  background: linear-gradient(135deg, #f5222d, #fa8c16);
}
.carousel-bg-4 {
  background: linear-gradient(135deg, #1890ff, #52c41a);
}
.carousel-bg-5 {
  background: linear-gradient(135deg, #722ed1, #eb2f96);
}

// 确保轮播图始终显示，防止空白
.carousel-initialized {
  .el-carousel__item {
    transition: transform 0.5s ease-in-out;
    opacity: 1 !important;
  }
}

.el-carousel__item {
  background-size: cover !important;
  position: relative;
  overflow: hidden;

  .banner-content, .carousel-content {
    position: absolute;
    top: 50%;
    left: 10%;
    transform: translateY(-50%);
    color: white;
    text-shadow: 0 2px 4px rgba(0,0,0,0.3);
    max-width: 600px;
    z-index: 10 !important;
    
    .banner-title {
      font-size: 48px;
      font-weight: bold;
      margin-bottom: 20px;
      color: white !important;
      animation: fadeInUp 0.8s ease-out;
    }
    
    .banner-text {
        font-size: 18px;
      line-height: 1.6;
      margin-bottom: 40px;
      color: white !important;
      animation: fadeInUp 0.8s ease-out 0.2s both;
    }
    
    .banner-button {
      display: inline-block;
      padding: 12px 30px;
      background-color: rgba(255,255,255,0.2);
      border: 1px solid rgba(255,255,255,0.4);
      border-radius: 5px;
      font-size: 16px;
      transition: all 0.3s;
      cursor: pointer;
      color: white !important;
      animation: fadeInUp 0.8s ease-out 0.4s both;
      
      i {
        margin-left: 5px;
        transition: transform 0.3s;
      }
      
      &:hover {
        background-color: rgba(255,255,255,0.3);
        transform: translateY(-2px);
        
        i {
          transform: translateX(3px);
        }
      }
    }
  }
}

.carousel-overlay {
  position: absolute;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background: linear-gradient(90deg, rgba(0,0,0,0.5) 0%, rgba(0,0,0,0.3) 50%, rgba(0,0,0,0.1) 100%);
  z-index: 5;
}

.el-carousel__item {
  .carousel-content {
    z-index: 20 !important;
    
    .banner-title {
      font-size: 52px !important;
      text-shadow: 0 2px 8px rgba(0,0,0,0.7) !important;
      margin-bottom: 30px !important;
    }
    
    .banner-text {
      font-size: 22px !important;
      line-height: 1.5 !important;
      text-shadow: 0 1px 4px rgba(0,0,0,0.7) !important;
      margin-bottom: 40px !important;
    }
    
    .banner-button {
      font-size: 18px !important;
      font-weight: bold !important;
      padding: 15px 30px !important;
      background-color: rgba(255,255,255,0.25) !important;
      box-shadow: 0 4px 12px rgba(0,0,0,0.15) !important;
      
      &:hover {
        background-color: rgba(255,255,255,0.35) !important;
      }
    }
  }
}

// 适应不同屏幕尺寸
@media screen and (max-width: 991px) {
  .el-carousel__item .carousel-content {
    .banner-title {
      font-size: 36px !important;
    }
    
    .banner-text {
      font-size: 18px !important;
    }
  }
}
</style>
<style lang="scss">
.index-popover.el-popover {
  min-width: unset !important;
  margin: 0 !important;
  padding: 0 !important;
  .list {
    padding: 0;
    margin: 0;
    .list-item {
      padding: 8px 10px !important;
      list-style: none;
      margin: 0;
      cursor: pointer;
      &:hover {
        color: #4575ff;
        background-color: mix(#4575ff, #fff, 3%);
      }
    }
  }
}

/* 全局滚动条控制 */
.el-carousel, 
.el-carousel__container, 
.el-carousel__indicators, 
.el-carousel__item {
  overflow: hidden !important;
}

.el-carousel::-webkit-scrollbar,
.el-carousel__container::-webkit-scrollbar,
.el-carousel__indicators::-webkit-scrollbar {
  width: 0 !important;
  height: 0 !important;
  display: none !important;
}

.box {
  overflow: hidden !important;
}

/* 修复轮播空白问题 - 全局样式重置 */
.el-carousel__container {
  position: relative;
  height: 660px !important;
}

.el-carousel__item {
  height: 100% !important;
  position: absolute !important;
  top: 0 !important;
  left: 0 !important;
  width: 100% !important;
  z-index: 0;
}

.el-carousel__item.is-active {
  z-index: 2 !important;
  opacity: 1 !important;
  visibility: visible !important;
}

/* 确保页面不滚动 */
.box, .el-carousel, .el-carousel__container {
  overflow: hidden !important;
  position: relative !important;
}

/* 防止空白闪烁 */
.el-carousel__item:not(.is-active) {
  opacity: 0 !important;
}

// 全局样式修复
.home .carousel-container {
  .el-carousel {
    .el-carousel__container {
      height: 660px !important;
    }
    
    .el-carousel__indicators {
      z-index: 10;
      
      .el-carousel__indicator {
        .el-carousel__button {
          width: 12px;
          height: 12px;
          border-radius: 50%;
          opacity: 0.7;
          background-color: white;
          
          &:hover {
            opacity: 1;
          }
        }
        
        &.is-active {
          .el-carousel__button {
            opacity: 1;
            background-color: #fff;
          }
        }
      }
    }
    
    .el-carousel__arrow {
      background-color: rgba(31, 45, 61, 0.3);
      font-size: 20px;
      width: 44px;
      height: 44px;
      
      &:hover {
        background-color: rgba(31, 45, 61, 0.5);
      }
    }
    
    .el-carousel__item {
      position: absolute;
      left: 0;
      top: 0;
      width: 100%;
      height: 100%;
      display: inline-block;
      overflow: hidden;
      z-index: 0;
      
      &.is-active {
        z-index: 2;
      }
      
      &.is-animating {
        transition-timing-function: ease-in-out;
      }
    }
    
    // 修复特定环境的空白问题
    .el-carousel__item:first-child {
      z-index: 1; // 确保第一项总是可见
    }
  }
}

// 确保每次进入页面时轮播图自动重置
.Homepage {
  .el-carousel__item {
    transform: translateX(0%) !important;
    
    &.is-active {
      transform: translateX(0%) !important;
      z-index: 2;
    }
  }
}

/* 特别修复轮播图内容可见性的问题 */
.Homepage .carousel-container {
  .el-carousel__item {
    overflow: visible !important;
    
    .carousel-content, .banner-content {
      color: #fff !important;
      position: absolute !important;
      z-index: 999 !important;
      visibility: visible !important;
      opacity: 1 !important;
      display: block !important;
      pointer-events: auto !important;
      
      h3, p, .banner-button, .banner-actions {
        color: #fff !important;
        visibility: visible !important;
        opacity: 1 !important;
        text-shadow: 0 2px 4px rgba(0,0,0,0.5) !important;
        font-weight: bold !important;
      }
      
      .banner-button {
        background-color: rgba(255,255,255,0.2) !important;
        border: 1px solid rgba(255,255,255,0.4) !important;
        display: inline-block !important;
      }
    }
  }
  
  /* 强制第一个轮播项显示，防止空白 */
  .el-carousel__item.is-active {
    z-index: 10 !important;
    transform: translateX(0) !important;
    
    .carousel-content, .banner-content {
      display: block !important;
    }
  }
  
  /* 禁止轮播项的transition，防止影响内容 */
  .el-carousel__item {
    transition: none !important;
  }
}

/* 强制显示箭头和指示器 */
.el-carousel__arrow {
  visibility: visible !important;
  opacity: 1 !important;
  z-index: 999 !important;
}

.el-carousel__indicators {
  visibility: visible !important;
  opacity: 1 !important;
  z-index: 999 !important;
  
  .el-carousel__indicator {
    visibility: visible !important;
    opacity: 1 !important;
  }
}
</style>
