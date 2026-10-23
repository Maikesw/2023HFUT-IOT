<template>
  <div v-loading="loading" class="Workshop">
    <Article :article="article" />
  </div>
</template>
<script>
import webAPI from '@/api/website'
import Article from '../components/Article.vue'
export default {
  name: 'Workshop',
  components: { Article },
  data() {
    return {
      loading: false,
      article: {}
    }
  },

  created() {
    this.getData(this.$route.meta.id)
  },
  methods: {
    async addReadNum() {
      await webAPI.readNum(this.article.id)
    },
    async getData(type) {
      try {
        this.loading = true
        const res = await webAPI.columnContent({
          channelTypeId: type,
          page: 1,
          size: 1
        })
        this.article = res.content[0]
        this.loading = false
        this.addReadNum()
      } catch (error) {
        this.loading = false
      }
    }
  }
}
</script>

<style lang="scss" scoped>
::v-deep.Workshop {
  width: 100%;
  margin-top: 15px;
  .el-scrollbar__wrap {
    overflow-x: hidden;
  }
}
</style>
