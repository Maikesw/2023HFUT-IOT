<template>
  <div class="homepage">
    欢迎使用 <h1>{{ settings.title }}</h1>
  </div>
</template>

<script>
import homepage from '@/assets/img/homepage.png'
import defaultSettings from '@/settings'

export default {
  name: 'Homepage',
  data() {
    return {
      homepage
    }
  },
  computed: {
    settings() {
      return defaultSettings
    }
  }
}
</script>

<style lang="scss" scoped>
.homepage {
  img {
    width: 100%;
    height: auto;
  }
}
</style>
