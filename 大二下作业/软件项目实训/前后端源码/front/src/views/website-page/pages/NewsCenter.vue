<template>
  <transition name="fade-transform" mode="out-in">
    <div v-if="!$route.path.includes('Detail')" v-loading="loading" class="NewsCenter">
      <el-scrollbar :style="`height:calc(100vh - 305px)`">
        <el-empty v-if="list.length===0" description="暂无数据" />
        <TitleThumbnailAndTime v-for="item in list" :key="item.id" top2="8" top="15" img-height="130" img-width="210" style="margin-top: 16px" :detail="item" path="/website/newsCenter/newsCenterDetail" />
      </el-scrollbar>
      <div style="width:100%;display:flex;justify-content: center;">
        <el-pagination :current-page.sync="currentPage" :page-sizes="[5,25,50,75,100]" :page-size="pageSize" layout="sizes, prev, pager, next" :total="total" @size-change="handleSizeChange" @current-change="handleCurrentChange" />
      </div>
    </div>
    <router-view v-else />
  </transition>
</template>
<script>
import TitleThumbnailAndTime from '../components/TitleThumbnailAndTime.vue'
import webAPI from '@/api/website'
export default {
  components: { TitleThumbnailAndTime },
  data() {
    return {
      list: [],
      currentPage: 1,
      pageSize: 5,
      index: 1,
      total: 0,
      loading: false
    }
  },
  watch: {
    $route(newVal, oldVal) {
      if (newVal.name === 'NewsCenter' && oldVal.name === 'newsCenterDetail') {
        this.getData(this.$route.meta.children[0].id)
      }
    }
  },
  created() {
    if (!this.$route.path.includes('Detail')) {
      this.getData(this.$route.meta.children[this.$route.query.index ? parseInt(this.$route.query.index) : 0].id)
    }
  },
  methods: {
    async getData(type) {
      try {
        this.loading = true
        const res = await webAPI.columnContent({
          channelTypeId: type,
          page: this.currentPage,
          size: this.pageSize
        })
        this.total = res.totalElements
        this.list = res.content
        this.loading = false
      } catch (error) {
        this.loading = false
      }
    },
    tabClick(index) {
      this.index = index
      this.currentPage = 1
      this.getData(this.$route.meta.children[parseInt(index) - 1].id)
    },
    handleSizeChange(val) {
      this.pageSize = val
      this.getData(this.$route.meta.children[parseInt(this.index) - 1].id)
    },
    handleCurrentChange(val) {
      this.currentPage = val
      this.getData(this.$route.meta.children[parseInt(this.index) - 1].id)
    }
  }
}
</script>

<style lang="scss" scoped>
@import "~@/assets/css/variables.scss";
::v-deep.NewsCenter {
  font-family: PingFang SC;
}
</style>
