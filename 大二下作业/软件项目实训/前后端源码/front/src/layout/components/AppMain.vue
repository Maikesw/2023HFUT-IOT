<template>
  <section id="app-main" class="app-main">
    <!-- <reset-login :dialog-visible.sync="dialogVisible" /> -->
    <transition name="fade-transform" mode="out-in">
      <keep-alive>
        <router-view v-if="$route.meta.isCache" class="router-view" />
      </keep-alive>
    </transition>
    <transition name="fade-transform" mode="out-in">
      <router-view v-if="!$route.meta.isCache" class="router-view" />
    </transition>
  </section>
</template>
<script>
export default {
  name: 'AppMain',
  data() {
    return {
      dialogVisible: false
    }
  },
  computed: {
    key() {
      return this.$route.path
    }
  }
}
</script>

<style lang="scss" scoped>
@import "~@/assets/css/variables.scss";
.app-main {
  background-color: #fff;
  position: relative;
  padding: $paddingTop $paddingRight $paddingBottom $paddingLeft;
  box-sizing: border-box;
}
</style>

<style lang="scss">
// fix css style bug in open el-dialog
.el-popup-parent--hidden {
  .fixed-header {
    padding-right: 15px;
  }
}

.router-view {
  background-color: #fff;
  border-radius: 5px;
}
</style>
