<template>
  <div class="Breadcrumb">
    <div class="content">
      <svg-icon icon-class="location" class="icon" />
      <span style="margin-left:4px">当前位置：</span>
      <el-breadcrumb class="app-breadcrumb" separator=">">
        <transition-group name="breadcrumb">
          <el-breadcrumb-item v-for="item in levelList" :key="item.path" :to="item.path">
            <span>{{ item.meta.title }}</span>
          </el-breadcrumb-item>
        </transition-group>
      </el-breadcrumb>
    </div>
  </div>
</template>

<script>
export default {
  data() {
    return {
      levelList: null
    }
  },
  watch: {
    $route() {
      this.getBreadcrumb()
    }
  },
  created() {
    this.getBreadcrumb()
  },
  methods: {
    getBreadcrumb() {
      this.levelList = this.$route.matched.filter(
        (item) => item.meta && item.meta.title
      )
      this.levelList[0] = {
        path: '/website/homepage',
        meta: { title: '首页' }
      }
      if (this.levelList[1].name === 'commonDetail') {
        const node = this.getInfo(parseInt(this.$route.query.type))
        this.levelList.splice(1, 0, node)
      }
    },
    getInfo(type) {
      switch (type) {
        case 1:
          return {
            path: '/website/newsCenter',
            meta: { title: '新闻中心' }
          }
        case 2:
          return {
            path: '/website/notice',
            meta: { title: '通知公告' }
          }
        case 3:
          return {
            path: '/website/organization',
            meta: { title: '组织结构' }
          }
        case 4:
          return {
            path: '/website/competition',
            meta: { title: '竞赛专栏' }
          }
        case 5:
          return {
            path: '/website/members',
            meta: { title: '单位会员' }
          }
        default:
          return {}
      }
    }
  }
}
</script>

<style lang="scss" scoped>
@import "~@/assets/css/element-variables.scss";
@import "~@/assets/css/variables.scss";
::v-deep.Breadcrumb {
  font-family: PingFang SC;
  width: 100%;
  color: #999696 !important;
  font-size: 12px;
  overflow: hidden;
  .content {
    margin-top: 15px;
    margin-bottom: 10px;
    display: flex;
    align-items: center;
    width: 1400px;
    margin-left: 50%;
    transform: translateX(-50%);
    .icon{
      font-size: 15px;
      margin-top: -1px;
    }
    .el-breadcrumb__item{
      color: #999696 !important;
      font-size: 12px;
    }
    .el-breadcrumb__inner.is-link, .el-breadcrumb__inner a{
      color: #999696 !important;
      font-weight: 400;
    }
    .el-breadcrumb__separator{
      margin: 0 2px;
    }
  }
}
</style>
