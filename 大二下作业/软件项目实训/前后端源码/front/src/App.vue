<template>
  <div id="app">
    <!-- <transition name="fade-transform" mode="out-in">
    <keep-alive>
      <router-view v-if="$route.meta.isCache" class="router-view" />
    </keep-alive>
    </transition>
    <transition  name="fade-transform" mode="out-in">
      <router-view v-if="!$route.meta.isCache" class="router-view" />
    </transition> -->
    <router-view />
  </div>
</template>

<script>
export default {
  name: 'App',
  data() {
    return {
      network: true // 默认有网
    }
  },
  mounted() {
    // 检测断网
    window.addEventListener('offline', () => {
      this.$message.warning('已断网')
      this.network = false
    })
    window.addEventListener('online', () => {
      this.network = true
    })
  }
}
</script>
<style lang="scss">
@import "@/assets/css/index.scss";
#app {
  overflow: hidden;
}
</style>
