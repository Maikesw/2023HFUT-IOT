<template>
  <!-- 这个组件是标题，时间，缩略内容带图片 -->
  <div class="TitleThumbnailAndTime" @click="toDetail">
    <img v-if="dataDetail.firstImg" :src="dataDetail.firstImg" :style="`min-height:${imgHeight}px;min-width:${imgWidth}px;height:${imgHeight}px;width:${imgWidth}px`" alt="加载失败" />
    <div v-else :style="`min-height:${imgHeight}px;min-width:${imgWidth}px;height:${imgHeight}px;width:${imgWidth}px`" class="default-img"><i class="el-icon-picture-outline" /><span>暂无图片  加载失败</span></div>
    <div class="content">
      <span class="title" :title="dataDetail.channelCName" :style="`margin-top:${top2}px`">{{ dataDetail.channelCName }}</span>
      <div class="time">{{ dataDetail.channelCTime | filterDateTime }}</div>
      <span class="remarks" :style="`margin-top:${top}px`">{{ removeHtmlStyle(dataDetail.channelCText) }}</span>
    </div>
  </div>
</template>
<script>
import moment from 'moment'
import { removeHtmlStyle } from '@/utils/common'
import _ from 'lodash'
export default {
  name: 'TitleThumbnailAndTime',
  filters: {
    filterDateTime(val) {
      return moment(val).format('YYYY-MM-DD HH:mm')
    }
  },
  components: {},
  props: {
    path: { type: String, default: '' },
    detail: { type: Object, default: () => { } },
    imgHeight: { type: String, default: '' },
    imgWidth: { type: String, default: '' },
    top: { type: String, default: '' },
    top2: { type: String, default: '0' }
  },
  data() {
    return {
      dataDetail: {}
    }
  },
  created() {
    this.dataDetail = _.cloneDeep(this.detail)
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
::v-deep.TitleThumbnailAndTime {
  width: 100%;
  display: flex;
  // align-items: center;
  font-family: PingFang SC;
  padding-right: 50px;
  box-sizing: border-box;
  cursor: pointer;
  &:hover {
    .content {
      .title {
        color: #3776ff;
      }
    }
  }
  img {
    border-radius: 4px;
    object-fit: cover;
  }
  .content {
    margin-left: 16px;
    .title {
      width: 500px;
      word-break: break-all;
      text-overflow: ellipsis;
      overflow: hidden;
      display: -webkit-box;
      -webkit-line-clamp: 1;
      -webkit-box-orient: vertical;
      font-size: 18px;
      font-weight: 700;
      color: #000000;
      transition: 0.3s;
      letter-spacing: 1px;
    }
    .time {
      font-size: 12px;
      color: #808080;
      margin-top: 10px;
    }
    .remarks {
      word-break: break-all;
      text-overflow: ellipsis;
      overflow: hidden;
      display: -webkit-box;
      -webkit-line-clamp: 3;
      -webkit-box-orient: vertical;
      font-size: 14px;
      font-weight: 400;
      color: #666666;
      line-height: 20px;
    }
  }
}
</style>
