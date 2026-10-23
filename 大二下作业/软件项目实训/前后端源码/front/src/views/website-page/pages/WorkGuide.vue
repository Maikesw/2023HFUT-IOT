<template>
  <div v-loading="loading" class="WorkGuide">
    <div class="header">
      <span class="title">{{ detail.channelCName }}</span>
      <div class="btn-group">
        <el-button type="primary" :title="currentPage===1?'当前已是第一篇':''" :disabled="currentPage===1" icon="el-icon-arrow-left" size="mini" @click="changePage(-1)">上一篇</el-button>
        <el-button type="primary" :title="totalElements===currentPage?'当前已是最后一篇':''" :disabled="totalElements===currentPage" size="mini" @click="changePage(1)">下一篇<i class="el-icon-arrow-right el-icon--right" /></el-button>
      </div>
    </div>
    <div class="time">
      <svg-icon icon-class="axt-time" style="margin-right:7px" />
      <span>{{ detail.channelCTime | filterDateTime }}</span>
      <span style="margin: 0 13px">|</span>
      <svg-icon icon-class="eye" style="margin-right:6px" />
      <span>{{ detail.hitTimes }}</span>
    </div>
    <div class="text v-html" v-html="detail.channelCText" />
  </div>
</template>
<script>
import moment from 'moment'
import webAPI from '@/api/website'
import adaptive from '@/directive/el-table'
export default {
  name: 'WorkGuide',
  directives: { adaptive },
  filters: {
    filterDateTime(val) {
      return moment(val).format('YYYY/MM/DD')
    }
  },
  components: {},
  data() {
    return {
      loading: false,
      detail: {},
      currentPage: 1,
      totalElements: 0
    }
  },
  created() {
    this.getData(this.$route.meta.id)
  },
  methods: {
    async addReadNum() {
      await webAPI.readNum(this.detail.id)
    },
    changePage(num) {
      this.currentPage += num
      this.getData(this.$route.meta.id)
    },
    async getData(type) {
      try {
        this.loading = true
        const res = await webAPI.columnContent({
          channelTypeId: type,
          page: this.currentPage,
          size: 1
        })
        this.detail = res.content[0]
        console.log(this.detail)
        this.totalElements = res.totalElements
        this.loading = false
        this.addReadNum()
      } catch (error) {
        this.loading = false
      }
    },
    indexMethod(index) {
      if (index + 1 < 10) {
        return `0${index + 1}`
      } else {
        return `${index + 1}`
      }
    }
  }
}
</script>

<style lang="scss" scoped>
::v-deep .text  table {
  border-collapse: collapse;
  border-spacing: 0;
  width: calc(100% - 20px) !important;
  p{
    margin: 0;
  }
    tbody{
      tr:nth-child(1){
        background-color:#F8FAFF;
        td{
          font-weight: bold;
          span{
            font-family: PingFang SC !important;
          }
        }
      }
    }
}
::v-deep .text td {
  border:1px solid #F2F2F2 !important;
  height: 44px;
  line-height: 44px;
  border-collapse: collapse;
    border-spacing: 0;
}
::v-deep .text td {
  border:1px solid #000;
}
::v-deep.WorkGuide {
  font-family: PingFang SC;
  margin-bottom: 10px;
  .header {
    margin-top: 16px;
    display: flex;
    justify-content: space-between;
    padding-right: 20px;
    .title {
      width: 700px;
      font-size: 24px;
      font-weight: bold;
      color: #000000;
      letter-spacing: 2px;
    }
    .btn-group {
      .el-button {
        color: #3776ff;
        background-color: #fff;
        border: 1px solid #3776ff;
        font-size: 14px;
        padding: 7px 10px;
        transition: all 0.3s;
        &:hover {
          background-color: #ecf5ff;
        }
      }
    }
  }
  .time {
    margin-top: 12px;
    color: #999999;
    font-size: 12px;
    margin-bottom: 20px;
  }
}
</style>
