<template>
  <div class="side-bar-logo-container" :class="{'collapse':collapse}">
    <transition name="sideBarLogoFade">
      <router-link v-if="collapse" key="collapse" class="side-bar-logo-link" to="/">
        <img v-if="logo" :src="logo" class="side-bar-logo" />
        <h1 v-else class="side-bar-title">{{ title }} </h1>
      </router-link>
      <router-link v-else key="expand" class="side-bar-logo-link" to="/">
        <img v-if="logo" :src="logo" class="side-bar-logo" />
        <h1 class="side-bar-title">{{ title }} </h1>
      </router-link>
    </transition>
  </div>
</template>

<script>
export default {
  name: 'SideBarLogo',
  props: {
    collapse: { type: Boolean, required: true }
  },
  data() {
    return {
      title: 'Vue Admin Template',
      logo: 'https://wpimg.wallstcn.com/69a1c46c-eb1c-4b46-8bd4-e9e686ef5251.png'
    }
  }
}
</script>

<style lang="scss" scoped>
.sideBarLogoFade-enter-active {
  transition: opacity 1.5s;
}

.sideBarLogoFade-enter,
.sideBarLogoFade-leave-to {
  opacity: 0;
}

.side-bar-logo-container {
  position: relative;
  width: 100%;
  height: 50px;
  line-height: 50px;
  background: #2b2f3a;
  text-align: center;
  overflow: hidden;

  & .side-bar-logo-link {
    height: 100%;
    width: 100%;

    & .side-bar-logo {
      width: 32px;
      height: 32px;
      vertical-align: middle;
      margin-right: 12px;
    }

    & .side-bar-title {
      display: inline-block;
      margin: 0;
      color: #fff;
      font-weight: 600;
      line-height: 50px;
      font-size: 14px;
      font-family: Avenir, Helvetica Neue, Arial, Helvetica, sans-serif;
      vertical-align: middle;
    }
  }

  &.collapse {
    .side-bar-logo {
      margin-right: 0px;
    }
  }
}
</style>
