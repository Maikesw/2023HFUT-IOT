<template>
  <transition name="fade-transform" mode="out-in">
    <div v-if="!$route.path.includes('Detail')" v-loading="loading" class="Members">
      <div v-for="item in members" :key="item.id" :title="item.channelCName" class="member-item" @click="toDetail(item)">
        <div class="member-logo">
          <img :src="item.firstImg" alt="加载失败" />
        </div>
        <div class="member-name">{{ item.channelCName }}</div>
      </div>
    </div>
    <router-view v-else />
  </transition>
</template>
<script>
import webAPI from '@/api/website'
// import AXT from '@/assets/img/AXT.jpg'
export default {
  components: {},
  data() {
    return {
      members: [],
      loading: false
    }
  },
  watch: {
    $route(newVal, oldVal) {
      if (newVal.name === 'Members' && oldVal.name === 'membersDetail') {
        this.getData(this.$route.meta.id)
      }
    }
  },
  created() {
    if (!this.$route.path.includes('Detail')) {
      this.getData(this.$route.meta.id)
    }
  },
  methods: {
    toDetail(item) {
      this.$router.push({
        path: '/website/members/membersDetail',
        query: {
          row: JSON.stringify(item)
        }
      })
    },
    async getData(type) {
      try {
        this.loading = true
        const res = await webAPI.columnContent({
          channelTypeId: type,
          page: 1,
          size: 999
        })
        this.members = res
        this.loading = false
      } catch (error) {
        this.loading = false
      }
    }
  }
}
</script>

<style lang="scss" scoped>
@import "~@/assets/css/variables.scss";
::v-deep.Members {
  margin-top: 20px;
  font-family: PingFang SC;
  display: grid;
  grid-template-columns: repeat(5, 181px);
  grid-template-rows: repeat(1, 120px);
  gap: 20px 24px;
  .member-item {
    width: 180px;
    height: 120px;
    box-sizing: border-box;
    border: 1px solid #ededed;
    cursor: pointer;
    &:hover {
      .member-name {
        color: #3776ff;
      }
    }
    .member-logo {
      width: 100%;
      height: 60px;
      background-color: #f7faff;
      display: flex;
      justify-content: center;
      align-items: center;
      img {
        height: 60%;
      }
    }
    .member-name {
      transition: 0.3s;
      text-align: center;
      width: 133px;
      margin-top: 10px;
      line-height: 20px;
      margin-left: 50%;
      transform: translateX(-50%);
      word-break: break-all;
      text-overflow: ellipsis;
      overflow: hidden;
      display: -webkit-box;
      -webkit-line-clamp: 2;
      -webkit-box-orient: vertical;
      color: #333333;
      font-size: 14px;
    }
  }
}
</style>
