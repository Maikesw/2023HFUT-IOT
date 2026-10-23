<template>
  <div class="body">
    <transition name="fade-transform" mode="out-in">
      <!-- 竞赛通知 -->
      <CompetitionNotice ref="CompetitionNotice" />
      <!-- 优秀作品 -->
      <!-- <OutstandingWorks v-else-if="index===3" /> -->
      <!-- 优秀教练 -->
      <!-- <OutstandingCoach v-else-if="index===4" /> -->
    </transition>
  </div>
</template>
<script>
import CompetitionNotice from './competition/components/CompetitionNotice.vue'
// import OutstandingWorks from './competition/components/OutstandingWorks.vue'
// import OutstandingCoach from './competition/components/OutstandingCoach.vue'
export default {
  // components: { CompetitionNotice, OutstandingWorks, OutstandingCoach },
  components: { CompetitionNotice },
  data() {
    return {
      index: 1
    }
  },
  created() {
  },
  methods: {
    tabClick(index) {
      this.index = parseInt(index)
      this.$refs.CompetitionNotice.tabClick(this.index)
    }
  }
}
</script>
<style lang="scss" scoped>
@import "~@/assets/css/variables.scss";
</style>
