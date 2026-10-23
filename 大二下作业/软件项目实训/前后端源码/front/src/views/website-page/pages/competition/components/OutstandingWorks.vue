<template>
  <transition name="fade-transform" mode="out-in">
    <div v-if="!$route.path.includes('Detail')" class="NewsCenter">
      <TitleThumbnailAndTime v-for="item in list" :key="item.id" top="9" img-height="111.4" img-width="180" style="margin-top: 18px" :detail="item" path="/website/competition/competitionDetail" />
    </div>
    <router-view v-else />
  </transition>
</template>
<script>
import TitleThumbnailAndTime from '../../../components/TitleThumbnailAndTime.vue'
export default {
  components: { TitleThumbnailAndTime },
  data() {
    return {
      list: [
        {
          id: 1,
          url: 'https://cube.elemecdn.com/6/94/4d3ea53c084bad6931a56d5158a48jpeg.jpeg',
          title: '关于举办2022年全国大学生信息安全竞赛安徽省赛的通知',
          createTime: new Date(),
          remarks: '据报道，Epic Games据报道，Epic Games和美国互联网公司Match集团正试图扩大他们对谷歌的诉讼，包括对这家搜索巨头的额外指控。在周五向加州北区联邦法院提交的一份动议中，这两家公司指控谷歌向有手段和能力创建与安卓应用商店竞争的开发商行贿。和美国互联网公司Match集团正试图扩大他们对谷歌的诉讼，包括对这家搜索巨头的额外指控。在周五向加州北区联邦法院提交的一份动议中，这两家公司指控谷歌向有手段和能力创建与安卓应用商店竞争的开发商行贿。'
        },
        {
          id: 2,
          url: 'https://cube.elemecdn.com/6/94/4d3ea53c084bad6931a56d5158a48jpeg.jpeg',
          title: '关于举办2022年全国大学生信息安全竞赛安徽省赛的通知',
          createTime: new Date(),
          remarks: '据报道，Epic '
        },
        {
          id: 3,
          url: 'https://cube.elemecdn.com/6/94/4d3ea53c084bad6931a56d5158a48jpeg.jpeg',
          title: '关于举办2022年全国大学生信息安全竞赛安徽省赛的通知',
          createTime: new Date(),
          remarks: '据报道，Epic据报道，Epic Games和美国互联网公司Match集团正试图扩大他们对谷歌的诉讼，包括对这家搜索巨头的额外指控。在周五向加州北区联邦法院提 '
        },
        {
          id: 7,
          url: 'https://cube.elemecdn.com/6/94/4d3ea53c084bad6931a56d5158a48jpeg.jpeg',
          title: '关于举办2022年全国大学生信息安全竞赛安徽省赛的通知',
          createTime: new Date(),
          remarks: '据报道，Epic据报道，Epic Games和美国互联网公司Match集团正试图扩大他们对谷歌的诉讼，包括对这家搜索巨头的额外指控。在周五向加州北区联邦法院提 '
        },
        {
          id: 6,
          url: 'https://cube.elemecdn.com/6/94/4d3ea53c084bad6931a56d5158a48jpeg.jpeg',
          title: '关于举办2022年全国大学生信息安全竞赛安徽省赛的通知',
          createTime: new Date(),
          remarks: '据报道，Epic据报道，Epic Games和美国互联网公司Match集团正试图扩大他们对谷歌的诉讼，包括对这家搜索巨头的额外指控。在周五向加州北区联邦法院提 '
        },
        {
          id: 5,
          url: 'https://cube.elemecdn.com/6/94/4d3ea53c084bad6931a56d5158a48jpeg.jpeg',
          title: '关于举办2022年全国大学生信息安全竞赛安徽省赛的通知',
          createTime: new Date(),
          remarks: '据报道，Epic据报道，Epic Games和美国互联网公司Match集团正试图扩大他们对谷歌的诉讼，包括对这家搜索巨头的额外指控。在周五向加州北区联邦法院提 '
        },
        {
          id: 4,
          url: 'https://cube.elemecdn.com/6/94/4d3ea53c084bad6931a56d5158a48jpeg.jpeg',
          title: '关于举办2022年全国大学生信息安全竞赛安徽省赛的通知',
          createTime: new Date(),
          remarks: '据报道，Epic据报道，Epic Games和美国互联网公司Match集团正试图扩大他们对谷歌的诉讼，包括对这家搜索巨头的额外指控。在周五向加州北区联邦法院提 '
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
