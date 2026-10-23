<template>
  <!-- 这个组件是除了首页的其他页面右侧固定显示的东西 -->
  <div class="RightSection">
    <!-- 新闻中心 -->
    <div v-loading="loading1" class="title">
      <el-tabs v-model="active1">
        <el-tab-pane label="新闻中心" name="1" />
      </el-tabs>
      <span @click="checkMoreNews">查看更多</span>
      <div class="list">
        <div v-for="item in newsList.slice(0, size)" :key="item.id" class="news-item" @click="toDetail(1, item)">
          <img v-if="item.firstImg" :src="item.firstImg" alt="加载失败" />
          <div v-else class="img"><i class="el-icon-picture-outline" />
            <div>暂无图片</div>
          </div>
          <div class="right">
            <div class="right-title" :title="item.channelCName">{{ item.channelCName }}</div>
            <div class="right-time">{{ item.channelCTime | filterDateTime }}</div>
          </div>
        </div>
      </div>
    </div>
    <!-- 通知公告 -->
    <div v-loading="loading2" class="title">
      <el-tabs v-model="active1">
        <el-tab-pane label="通知公告" name="1" />
      </el-tabs>
      <span @click="checkMoreNotice">查看更多</span>
      <div class="list">
        <div v-for="item in noticeList.slice(0, size)" :key="item.id" class="notice-item" @click="toDetail(2, item)">
          <div class="right-time">{{ item.channelCTime | filterDateTime }}</div>
          <div class="right-title" :title="item.channelCName">{{ item.channelCName }}</div>
        </div>
      </div>
    </div>
  </div>
</template>
<script>
import moment from 'moment'
import webAPI from '@/api/website'
export default {
  name: 'RightSection',
  filters: {
    filterDateTime(val) {
      return moment(val).format('YYYY年MM月DD日')
    }
  },
  components: {},
  data() {
    return {
      loading1: false,
      active1: '1',
      newsList: [],
      noticeList: [],
      size: 3
    }
  },
  created() {
    this.resize(document.documentElement.clientHeight, true)
  },
  mounted() {
    const that = this
    window.onresize = () => {
      return (() => {
        that.resize(document.documentElement.clientHeight, false)
      })()
    }
  },
  methods: {
    // 一个简单的自适应
    resize(height, bool) {
      if (height < 890) {
        this.size = 2
      } else if (height > 890 && height < 1000) {
        this.size = 3
      } else {
        this.size = 4
      }
      if (bool) {
        this.getData(1)
        this.getData(2)
      }
    },
    async getData(type) {
      type === 1 ? this.loading1 = true : this.loading2 = true
      try {
        const res = await webAPI.columnContent({
          channelTypeId: type === 1 ? this.$store.state.webSite.routeList.filter(e => e.name === 'NewsCenter')[0].meta.children[0].id : this.$store.state.webSite.routeList.filter(e => e.name === 'Notice')[0].meta.id,
          page: 1,
          size: 4
        })
        type === 1 ? this.newsList = res.content : this.noticeList = res.content
        type === 1 ? this.loading1 = false : this.loading2 = false
      } catch (error) {
        type === 1 ? this.loading1 = false : this.loading2 = false
      }
    },
    toDetail(index, item) {
      this.$router.push({
        path: index === 1 ? '/website/newsCenter/newsCenterDetail' : '/website/notice/noticeDetail',
        query: { row: JSON.stringify(item) }
      })
    },
    checkMoreNews() {
      if (this.$route.path !== '/website/newsCenter') {
        this.$router.push('/website/newsCenter')
      }
    },
    checkMoreNotice() {
      if (this.$route.path !== '/website/notice') {
        this.$router.push('/website/notice')
      }
    }
  }
}
</script>

<style lang="scss" scoped>
::v-deep.RightSection {
  font-family: PingFang SC;
  width: 340px;
  margin: 10px 0 20px;
  .title + .title {
    margin-top: 44px;
  }
  .title {
    width: 100%;
    position: relative;
    span {
      position: absolute;
      font-size: 14px;
      color: #999999;
      right: 0;
      top: 8px;
      cursor: pointer;
    }
    .el-tabs__item.is-active {
      font-weight: 700;
      color: #000000;
      font-size: 18px;
      line-height: 30px;
    }
    .el-tabs__active-bar {
      background-color: #3776ff;
      width: 72px !important;
    }
    .list {
      .news-item + .news-item {
        margin-top: 32px;
        position: relative;
        &::after {
          content: "";
          position: absolute;
          height: 1px;
          width: 340px;
          background-color: #f2f2f2;
          top: -16px;
        }
      }
      .news-item {
        display: flex;
        cursor: pointer;
        &:hover {
          .right {
            .right-title {
              color: #3776ff;
            }
          }
        }
        img {
          width: 100px;
          height: 64px;
          border-radius: 4px;
          object-fit: cover;
        }
        .img {
          background-color: #f5f7fa;
          width: 100px;
          height: 64px;
          border-radius: 4px;
          display: flex;
          flex-direction: column;
          align-items: center;
          justify-content: center;
          color: #ccc;
          i {
            font-size: 20px;
            margin-bottom: 10px;
          }
          div {
            font-size: 12px;
          }
        }
        .right {
          position: relative;
          margin-left: 15px;
          .right-title {
            width: 230px;
            word-break: break-all;
            text-overflow: ellipsis;
            overflow: hidden;
            display: -webkit-box;
            -webkit-line-clamp: 2;
            -webkit-box-orient: vertical;
            font-size: 14px;
            color: #333333;
            transition: 0.3s;
          }
          .right-time {
            position: absolute;
            color: #666666;
            font-size: 12px;
            bottom: 4px;
          }
        }
      }
      .notice-item {
        cursor: pointer;
        &:hover {
          .right-title {
            color: #3776ff;
          }
        }
        .right-time {
          color: #666666;
          font-size: 12px;
        }
        .right-title {
          width: 340px;
          word-break: break-all;
          text-overflow: ellipsis;
          overflow: hidden;
          display: -webkit-box;
          -webkit-line-clamp: 2;
          -webkit-box-orient: vertical;
          color: #333333;
          font-size: 14px;
          margin-top: 14px;
          transition: 0.3s;
        }
      }
      .notice-item + .notice-item {
        margin-top: 20px;
        position: relative;
        &::before {
          content: "";
          position: absolute;
          top: -10px;
          width: 340px;
          height: 1px;
          background-color: #f2f2f2;
        }
      }
    }
  }
}
</style>
