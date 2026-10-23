<template>
  <transition name="fade-transform" mode="out-in">
    <div v-if="!$route.path.includes('Detail')" v-loading="loading" class="Notice">
      <el-scrollbar :style="`height:calc(100vh - 305px)`">
        <el-empty v-if="list.length===0" description="暂无数据" />
        <TitleAndThumbnail v-for="(item, i) in list" :key="item.id" :detail="item" :has-bottom-border="!(i+1===list.length)" path="/website/competition/competitionDetail" />
      </el-scrollbar>
      <div style="width:100%;display:flex;justify-content: center;">
        <el-pagination :current-page.sync="currentPage" :page-sizes="[5,25,50,75,100]" :page-size="pageSize" layout="sizes, prev, pager, next" :total="total" @size-change="handleSizeChange" @current-change="handleCurrentChange" />
      </div>
    </div>
    <router-view v-else />
  </transition>
</template>
<script>
import TitleAndThumbnail from '../../../components/TitleAndThumbnail.vue'
import webAPI from '@/api/website'
export default {
  name: 'CompetitionNotice',
  components: { TitleAndThumbnail },
  data() {
    return {
      currentPage: 1,
      pageSize: 5,
      total: 0,
      index: 1,
      loading: false,
      list: []
    }
  },
  watch: {
    $route(newVal, oldVal) {
      if (newVal.name === 'Competition' && oldVal.name === 'competitionDetail') {
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
</style>
