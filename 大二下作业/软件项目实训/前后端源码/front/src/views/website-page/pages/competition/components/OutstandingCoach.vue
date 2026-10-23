<template>
  <transition name="fade-transform" mode="out-in">
    <div v-if="!$route.path.includes('Detail')" class="NewsCenter">
      <PersonProfile v-for="(item, index) in list" :key="item.id" :has-bottom-border="index+1!==list.length" :detail="item" path="/website/competition/competitionDetail" />
    </div>
    <router-view v-else />
  </transition>
</template>
<script>
import PersonProfile from '../../../components/PersonProfile.vue'
export default {
  components: { PersonProfile },
  data() {
    return {
      list: [
        {
          id: 1,
          name: '阿历克斯·弗格森',
          url: 'https://cube.elemecdn.com/6/94/4d3ea53c084bad6931a56d5158a48jpeg.jpeg',
          research: '安徽大学·侧重大学计算机基础教育体系研究',
          achievement: '2座欧冠、13次顶级联赛冠军对于他，我认为，所有的溢美之词都不为过。他在曼联的成功可谓空前绝后。不单单是其取得的荣誉，更因其创下连续执教一家豪门俱乐部时间最长的神话。'
        }, {
          id: 6,
          name: '阿历克斯·弗格森',
          url: 'https://cube.elemecdn.com/6/94/4d3ea53c084bad6931a56d5158a48jpeg.jpeg',
          research: '安徽大学·侧重大学计算机基础教育体系研究',
          achievement: '2座欧冠、13次顶级联赛冠军对于他，我认为，所有的溢美之词都不为过。他在曼联的成功可谓空前绝后。不单单是其取得的荣誉，更因其创下连续执教一家豪门俱乐部时间最长的神话。'
        }, {
          id: 5,
          name: '阿历克斯·弗格森',
          url: 'https://cube.elemecdn.com/6/94/4d3ea53c084bad6931a56d5158a48jpeg.jpeg',
          research: '安徽大学·侧重大学计算机基础教育体系研究',
          achievement: '2座欧冠、13次顶级联赛冠军对于他，我认为，所有的溢美之词都不为过。他在曼联的成功可谓空前绝后。不单单是其取得的荣誉，更因其创下连续执教一家豪门俱乐部时间最长的神话。'
        }, {
          id: 4,
          name: '阿历克斯·弗格森',
          url: 'https://cube.elemecdn.com/6/94/4d3ea53c084bad6931a56d5158a48jpeg.jpeg',
          research: '安徽大学·侧重大学计算机基础教育体系研究',
          achievement: '2座欧冠、13次顶级联赛冠军对于他，我认为，所有的溢美之词都不为过。他在曼联的成功可谓空前绝后。不单单是其取得的荣誉，更因其创下连续执教一家豪门俱乐部时间最长的神话。'
        }, {
          id: 3,
          name: '阿历克斯·弗格森',
          url: 'https://cube.elemecdn.com/6/94/4d3ea53c084bad6931a56d5158a48jpeg.jpeg',
          research: '安徽大学·侧重大学计算机基础教育体系研究',
          achievement: '2座欧冠、13次顶级联赛冠军对于他，我认为，所有的溢美之词都不为过。他在曼联的成功可谓空前绝后。不单单是其取得的荣誉，更因其创下连续执教一家豪门俱乐部时间最长的神话。'
        }, {
          id: 2,
          name: '阿历克斯·弗格森',
          url: 'https://cube.elemecdn.com/6/94/4d3ea53c084bad6931a56d5158a48jpeg.jpeg',
          research: '安徽大学·侧重大学计算机基础教育体系研究',
          achievement: '2座欧冠、13次顶级联赛冠军对于他，我认为，所有的溢美之词都不为过。他在曼联的成功可谓空前绝后。不单单是其取得的荣誉，更因其创下连续执教一家豪门俱乐部时间最长的神话。'
        }
      ]
    }
  },
  created() {
  },
  methods: {
  }
}
</script>
<style lang="scss" scoped>
@import "~@/assets/css/variables.scss";
</style>
