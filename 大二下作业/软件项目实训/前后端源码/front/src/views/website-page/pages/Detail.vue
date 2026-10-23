<template>
  <div class="body">
    <Article :article="row" />
  </div>
</template>
<script>
import webAPI from '@/api/website'
import Article from '../components/Article.vue'
export default {
  components: { Article },
  data() {
    return {
      row: {}
    }
  },
  watch: {
    $route: {
      handler() {
        this.row = JSON.parse(this.$route.query.row)
      },
      deep: true
    }
  },
  created() {
    // console.log(this.$route)
    if (!this.$route.query.row) {
      if (this.$route.name === 'commonDetail') {
        this.$router.push('/website/homepage')
      } else {
        this.$router.push(this.$route.path.slice(0, this.$route.path.lastIndexOf('/')))
      }
    } else {
      this.row = JSON.parse(this.$route.query.row)
      this.addReadNum()
    }
  },
  methods: {
    async addReadNum() {
      await webAPI.readNum(this.row.id)
    }

  }
}
</script>

<style lang="scss" scoped>
::v-deep.body {
  margin-top: 15px;
}
</style>
