<template>
  <!-- 这个组件是标题，时间，缩略内容不带图片 -->
  <div class="thumbnail-body" :style="`border-bottom:${hasBottomBorder?'1px solid #EDEDED':'0'}`" @click="toDetail">
    <span class="title" :title="detail.channelCName">{{ detail.channelCName }}</span>
    <span class="time">{{ detail.channelCTime | filterDateTime }}</span>
    <div v-if="detail.channelCText" class="remarks" :style="`-webkit-line-clamp:${rowNumber}`">{{ removeHtmlStyle(detail.channelCText) }}</div>
  </div>
</template>
<script>
import moment from 'moment'
import { removeHtmlStyle } from '@/utils/common'
export default {
  name: 'TitleAndThumbnail',
  filters: {
    filterDateTime(val) {
      return moment(val).format('YYYY-MM-DD HH:mm')
    }
  },
  components: {},
  props: {
    path: { type: String, default: '' },
    detail: { type: Object, default: () => { } },
    hasBottomBorder: { type: Boolean, default: true }, // 有无底部边框
    rowNumber: { type: Number, default: 2 } // 显示几行文字
  },
  data() {
    return {
    }
  },
  created() {
  },
  methods: {
    removeHtmlStyle(html) {
      return removeHtmlStyle(html)
    },
    toDetail() {
      this.$router.push({
        path: this.path,
        query: { row: JSON.stringify(this.detail) }
      })
    }
  }
}
</script>

<style lang="scss" scoped>
::v-deep.thumbnail-body {
  width: 100%;
  font-family: PingFang SC;
  overflow: hidden;
  padding-bottom: 20px;
  cursor: pointer;
  &:hover {
    .title {
      color: #3776ff;
    }
  }
  .title {
    width: 80%;
    word-break: break-all;
    text-overflow: ellipsis;
    overflow: hidden;
    display: -webkit-box;
    -webkit-line-clamp: 1;
    -webkit-box-orient: vertical;
    margin-top: 15px;
    font-weight: bold;
    color: #000000;
    font-size: 18px;
    transition: 0.3s;
    letter-spacing: 1px;
  }
  .time {
    display: inline-block;
    font-size: 12px;
    color: #808080;
    margin-top: 10px;
  }
  .remarks {
    word-break: break-all;
    text-overflow: ellipsis;
    overflow: hidden;
    display: -webkit-box;
    -webkit-box-orient: vertical;
    font-size: 14px;
    color: #666666;
    margin-top: 15px;
  }
}
</style>
