<template>
  <!-- 这个组件是文章详情 -->
  <div class="Article">
    <div class="title">{{ article.channelCName }}</div>
    <div class="time">
      <svg-icon icon-class="axt-time" style="margin-right:7px" />
      <span>{{ article.channelCTime | filterDateTime }}</span>
      <span style="margin: 0 13px">|</span>
      <svg-icon icon-class="eye" style="margin-right:6px" />
      <span>{{ article.hitTimes }}</span>
    </div>
    <div class="remarks" v-html="article.channelCText" />
  </div>
</template>
<script>
import moment from 'moment'
export default {
  name: 'Article',
  filters: {
    filterDateTime(val) {
      return moment(val).format('YYYY/MM/DD')
    }
  },
  components: {},
  props: {
    article: { type: Object, default: () => { } }
  },
  data() {
    return {
    }
  },
  created() {
  },
  methods: {
  }
}
</script>

<style lang="scss" scoped>
::v-deep.Article {
  font-family: PingFang SC;
  padding-right: 30px;
  overflow-x: hidden;
  .title {
    font-size: 24px;
    font-weight: 700;
    color: #000000;
  }
  .time{
      margin-top: 14px;
      color: #999999;
      font-size: 12px;
      margin-bottom: 10px;
  }
  .remarks{
    margin-bottom: 20px;
    font-size: 14px !important;
    line-height: 1.5 !important;
    img{
      max-width: 600px !important;
      margin-left: 50%;
      transform: translateX(-50%);
    }
  }
}
</style>
