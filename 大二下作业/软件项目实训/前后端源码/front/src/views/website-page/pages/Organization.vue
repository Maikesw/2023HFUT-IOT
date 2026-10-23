<template>
  <transition name="fade-transform" mode="out-in">
    <div v-if="!$route.path.includes('Detail')" v-loading="loading" class="Organization">
      <TitleAndThumbnail v-for="(item, index) in organizationList" :key="item.id" :detail="item" :row-number="1" :has-bottom-border="!(index+1===organizationList.length)" path="/website/organization/organizationDetail" />
    </div>
    <router-view v-else />
  </transition>
</template>
<script>
import TitleAndThumbnail from '../components/TitleAndThumbnail.vue'
import webAPI from '@/api/website'
export default {
  name: 'Organization',
  components: { TitleAndThumbnail },
  data() {
    return {
      loading: false,
      organizationList: []
    }
  },
  watch: {
    $route(newVal, oldVal) {
      if (newVal.name === 'Organization' && oldVal.name === 'organizationDetail') {
        this.getData(this.$route.meta.id)
      }
    }
  },
  created() {
    if (!this.$route.path.includes('Detail')) {
      this.getData(this.$route.meta.id)
    }
  },
  methods: {
    async getData(type) {
      try {
        this.loading = true
        const res = await webAPI.columnContent({
          channelTypeId: type,
          page: 1,
          size: 999
        })
        this.organizationList = res.content
        this.loading = false
      } catch (error) {
        this.loading = false
      }
    }
  }
}
</script>

<style lang="scss" scoped>
@import "~@/assets/css/variables.scss";
</style>
