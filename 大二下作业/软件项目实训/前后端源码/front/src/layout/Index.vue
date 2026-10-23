<template>
  <section :class="classObj" class="app-wrapper">
    <!-- 头部标题 -->
    <title-bar @bgColor="getColor" />
    <!-- 侧边栏 -->
    <section class="container">
      <!-- 适配移动端页面 -->
      <div v-if="device === 'mobile' && sideBar.opened" class="drawer-bg" @click="handleClickOutside" />
      <side-bar class="side-bar-container" />
      <!-- 右侧内容 -->
      <section class="main-container-view main-width">
        <!-- 导航栏 -->
        <tags-view v-if="needTagsView" :style="bgColorAlpha" class="tag-bar" />
        <main class="scroll-content">
          <!-- 面包屑导航 -->
          <!-- <nav-bar class="position-sticky" /> -->
          <!-- 主体内容 -->
          <app-main />
        </main>
      </section>
    </section>
  </section>
</template>

<script>
import { AppMain, SideBar, TagsView } from './components'
import TitleBar from './components/TitleBar'
import ResizeMixin from './mixin/resizeHandler'
import { mapState } from 'vuex'
export default {
  name: 'Layout',
  components: { AppMain, SideBar, TagsView, TitleBar },
  mixins: [ResizeMixin],
  data() {
    return {
      themeColor: this.$store.getters.userInfo.themeColor
    }
  },
  computed: {
    bgColorAlpha() {
      return { backgroundColor: this.themeColor + '33' }
    },
    ...mapState({
      sideBar: state => state.app.sideBar,
      device: state => state.app.device,
      needTagsView: state => state.settings.tagsView,
      fixedHeader: state => state.settings.fixedHeader
    }),
    classObj() {
      return {
        'hide-side-bar': !this.sideBar.opened,
        'open-side-bar': this.sideBar.opened,
        withoutAnimation: this.sideBar.withoutAnimation,
        mobile: this.device === 'mobile'
      }
    }
  },
  methods: {
    handleClickOutside() {
      this.$store.dispatch('app/closeSideBar', { withoutAnimation: false })
    },
    getColor(color) {
      this.themeColor = color
    }
  }
}
</script>

<style lang="scss" scoped>
@import "@/assets/css/mixin.scss";
@import "@/assets/css/variables.scss";

.app-wrapper {
  height: 100%;
  width: 100%;
  &.mobile.open-side-bar {
    position: fixed;
    top: 0;
    z-index: 10;
  }
}

.drawer-bg {
  background: #000;
  opacity: 0.5;
  width: 100%;
  top: 0;
  height: 100%;
  position: absolute;
  z-index: 9;
}

.container {
  // margin-top: $titleBarHeight;
  height: calc(100vh - #{$titleBarHeight});
  display: flex;
  flex-direction: row;
  flex: 1;
  flex-basis: auto;
  box-sizing: border-box;
  min-width: 0;
}

.tag-bar {
  width: 100%;
  background: #fff;
}
.main-width {
  transition: width 0.28s;
  width: calc(100% - #{$sideBarWidth});
}
.hide-side-bar .main-width {
  width: calc(100% - 54px);
}
.mobile .main-width {
  width: 100%;
}

.scroll-content {
  height: calc(100vh - #{$titleBarHeight} - 34px);
  box-sizing: border-box;
  overflow: auto;
  position: relative;
}
.app-main {
  padding: $paddingTop $paddingRight $paddingBottom $paddingLeft;
  box-sizing: border-box;
}
.position-sticky {
  position: sticky;
  top: 0;
  left: 0;
  z-index: 998;
}
</style>
