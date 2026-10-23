<template>
  <!-- 这个组件是首页的标题+右侧查看更多 -->
  <div class="body">
    <div class="title">{{ title }}</div>
    <slot name="title-right" />
    <div class="more" @click="viewMore">查看更多<svg-icon icon-class="right" /></div>
  </div>
</template>
<script>
export default {
  name: 'TitleAndMore',
  props: {
    title: { type: String, default: '' }
  },
  data() {
    return {
    }
  },
  created() {
  },
  methods: {
    viewMore() {
      this.$emit('viewMore')
    }
  }
}
</script>

<style lang="scss" scoped>
::v-deep.body {
  position: relative;
  font-family: PingFang SC;
  .title {
    position: relative;
    font-size: 32px;
    color: #333;
    font-weight: 700;
    margin-left: 22px;
    letter-spacing: 3px;
    &::before {
      content: "";
      position: absolute;
      top: 5px;
      left: -22px;
      background-color: #3776ff;
      width: 9px;
      height: 29px;
    }
  }
  .more {
    position: absolute;
    right: 0;
    top: 50%;
    transform: translateY(-50%);
    font-size: 18px;
    color: #666666;
    cursor: pointer;
  }
}
</style>
