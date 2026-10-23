<template>
  <div class="my-main">
    <!-- 面包屑 -->
    <breadcrumb v-if="!$route.path.includes('/website/homepage')" id="breadcrumb-container" class="breadcrumb-container" />
    <el-scrollbar ref="scrollbar1" :style="`height:${$route.path.includes('/website/homepage')?'calc( 100vh - 64px )':'calc( 100vh - 64px - 140px )'}`">
      <section class="main-content">
        <!-- 缓存 keep-alive -->
        <transition name="fade-transform" mode="out-in">
          <keep-alive>
            <!-- 首页 -->
            <router-view v-if="!$route.meta.noCache&&$route.path.includes('/website/homepage')" class="router-view" />
            <!-- 其他页(有右侧通知新闻) -->
            <div v-else-if="!$route.meta.noCache&&!$route.path.includes('/website/homepage')" class="page-content">
              <div class="content-body">
                <div class="left">
                  <!-- 工作指南页面、详情页不需要tabs -->
                  <div v-if="!(['/website/workGuide','/website/newsCenter', '/website/competition'].includes($route.path)||$route.path.includes('Detail'))" class="tab">
                    <el-tabs v-model="active1">
                      <el-tab-pane :label="$route.meta.title" name="1" />
                    </el-tabs>
                  </div>
                  <div v-else-if="(['/website/newsCenter', '/website/competition'].includes($route.path))" :style="cssVars" style="margin-top: 6px" class="tab">
                    <el-tabs v-model="active2" @tab-click="tabClick">
                      <el-tab-pane v-for="(item, index) in $route.meta.children" :key="index" :label="item.title" :name="`${index+1}`" />
                    </el-tabs>
                  </div>
                  <el-scrollbar ref="scrollbar2" :style="`height:${$route.path.includes('Detail')||$route.path.includes('workGuide')?'calc(100vh - 215px)':'calc((100vh - 265px)'}`">
                    <transition name="fade-transform" mode="out-in">
                      <router-view ref="component" />
                    </transition>
                  </el-scrollbar>
                </div>
                <RightSection />
              </div>
            </div>
          </keep-alive>
        </transition>
        <!-- 不缓存 -->
        <transition name="fade-transform" mode="out-in">
          <!-- 首页 -->
          <router-view v-if="$route.meta.noCache&&$route.path.includes('/website/homepage')" class="router-view" />
          <!-- 其他页(有右侧通知新闻) -->
          <div v-else-if="$route.meta.noCache&&!$route.path.includes('/website/homepage')" class="page-content">
            <div class="content-body">
              <div class="left">
                <!-- 工作指南页面、详情页不需要tabs -->
                <div v-if="!(['/website/workGuide','/website/newsCenter', '/website/competition'].includes($route.path)||$route.path.includes('Detail'))" :style="cssVars" class="tab">
                  <el-tabs v-model="active1">
                    <el-tab-pane :label="$route.meta.title" name="1" />
                  </el-tabs>
                </div>
                <div v-else-if="(['/website/newsCenter', '/website/competition'].includes($route.path))" :style="cssVars" style="margin-top: 6px" class="tab">
                  <el-tabs v-model="active2" @tab-click="tabClick">
                    <el-tab-pane v-for="(item, index) in $route.meta.children" :key="index" :label="item.title" :name="`${index+1}`" />
                  </el-tabs>
                </div>
                <el-scrollbar ref="scrollbar2" :style="`height:${$route.path.includes('Detail')||$route.path.includes('workGuide')?'calc(100vh - 215px)':'calc((100vh - 265px)'}`">
                  <transition name="fade-transform" mode="out-in">
                    <router-view ref="component" />
                  </transition>
                </el-scrollbar>
              </div>
              <RightSection />
            </div>
          </div>
        </transition>
      </section>
    </el-scrollbar>
  </div>
</template>
<script>
import Breadcrumb from '../components/Breadcrumb.vue'
import RightSection from '../components/RightSection.vue'
export default {
  name: 'PageMain',
  components: { Breadcrumb, RightSection },
  data() {
    return {
      active1: '1',
      active2: '1',
      currentRoute: ''
    }
  },
  computed: {
    cssVars() {
      return {
        '--width': `${this.$route.meta.title.length * 18}px`
      }
    }
  },
  watch: {
    // 切换路由时将active2置为1，在首页切换滚动新闻时路由会携带参数，将active2置为当前显示的新闻类型下标
    $route() {
      if (this.$route.path.includes('newsCenter')) {
        this.active2 = this.$route.query.index ? `${parseInt(this.$route.query.index) + 1}` : '1'
      } else {
        this.active2 = '1'
      }
      // 切换菜单后页面滚动到顶部 这里判断Homepage是因为产品不希望从首页跳到某详情页时页面出现先滚动到顶部再跳转的情况
      if (this.$refs.scrollbar1 && this.currentRoute !== 'Homepage') {
        this.$refs.scrollbar1.wrap.scrollTop = 0
      }
      if (this.$refs.scrollbar2 && this.currentRoute !== 'Homepage') {
        this.$refs.scrollbar2.wrap.scrollTop = 0
      }
      this.currentRoute = this.$route.name
    }
  },
  created() {
    this.currentRoute = this.$route.name
  },
  methods: {
    tabClick() {
      this.$refs.component.tabClick(this.active2)
    }
  }
}
</script>

<style lang="scss" scoped>
@import "~@/assets/css/variables.scss";
::v-deep.my-main {
  height: calc(100vh - 175px);
  .el-scrollbar__wrap {
    overflow-x: hidden !important;
  }
  .default-img {
    background-color: #f5f7fa;
    width: 260px;
    height: 100%;
    border-radius: 4px;
    display: flex;
    flex-direction: column;
    align-items: center;
    justify-content: center;
    color: #ccc;
    i {
      font-size: 26px;
      margin-bottom: 20px;
    }
  }
  .el-pagination {
    margin-top: 5px;
    font-size: 14px;
    .btn-prev,
    .btn-next {
      border: 1px solid #f2f2f2;
      border-radius: 4px;
      padding: 0 7px;
      min-width: unset;
    }
    .btn-next {
      margin-left: 10px;
    }
    .el-pager {
      .active {
        background-color: #4575ff !important;
        border-color: #4575ff !important;
        color: #fff !important;
      }
      li {
        font-size: 14px;
        color: #333333;
        border-radius: 4px;
        border: 1px solid #f2f2f2;
        font-weight: 400;
        margin-left: 10px;
        width: 28px;
        height: 28px;
        box-sizing: border-box;
        min-width: 28px;
        transition: all 0.3s;
      }
    }
  }
  .main-content {
    // overflow-y: auto;
    .page-content {
      width: 100%;
      display: flex;
      .content-body {
        width: 1400px;
        min-width: 1400px;
        margin-left: 50%;
        transform: translateX(-690px);
        display: flex;
        .left {
          width: 1010px;
          min-width: 1010px;
          margin-right: 30px;
          .tab {
            width: 100%;
            margin-top: 10px;
            .el-tabs__item {
              font-size: 16px;
              color: #666666;
            }
            .el-tabs__item.is-active {
              font-weight: 700;
              color: #000000;
              font-size: 18px;
              line-height: 30px;
            }
            .el-tabs__active-bar {
              background-color: #3776ff;
              width: var(--width) !important;
            }
            .el-tabs__header {
              margin: 0;
            }
          }
        }
      }
    }
  }
}
</style>
