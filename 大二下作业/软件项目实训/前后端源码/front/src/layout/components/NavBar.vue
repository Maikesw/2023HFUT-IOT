<template>
  <div id="NavBar" class="nav-bar">
    <svg-icon icon-class="axt-s-location" />
    <breadcrumb id="breadcrumb-container" class="breadcrumb-container" />
  </div>
</template>

<script>
import Breadcrumb from '../Breadcrumb'

export default {
  name: 'Navbar',
  components: {
    Breadcrumb
  }
}
</script>

<style lang="scss" scoped>
@import "~@/assets/css/variables.scss";
.nav-bar {
  height: $navBarHeight;
  overflow: hidden;
  position: relative;
  background: #fff;
  padding: 0 $paddingTop;
  display: flex;
  .svg-icon {
    margin-top: $navBarHeight/2;
    transform: translateY(-50%);
  }
}
</style>
