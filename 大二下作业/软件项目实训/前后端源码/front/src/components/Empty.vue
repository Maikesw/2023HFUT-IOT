<template>
  <div class="empty empty-page">
    <div class="empty-box">
      <div class="img-box">
        <img :src="image" />
      </div>
      <p class="title">{{ title }}</p>
      <p class="subtitle">{{ subtitle }}</p>
    </div>
  </div>
</template>

<script>
import defaultImage from '@/assets/img/empty.png'
export default {
  name: 'Empty',
  props: {
    src: { type: String, default: '' }, // 图片路径
    title: { type: String, default: '空空如也' },
    subtitle: { type: String, default: '也是种态度' }
  },
  data() {
    return {
    }
  },
  computed: {
    image() {
      if (this.src) {
        return this.src
      }
      return defaultImage
    }
  }
}
</script>

<style lang="scss" scoped>
.empty-page {
  position: relative;
  // height: 100%;
  // width: 100%;
}
.empty-box {
  // position: absolute;
  // top: 50%;
  // right: 50%;
  // transform: translateX(-50%);
  // transform: translateY(-50%);
  text-align: center;
}
img {
  height: auto;
  width: 12vw;
  min-width: 160px;
}
.title {
  color: #b1c3d6;
}
.subtitle {
  color: #ccdbe9;
}
</style>
