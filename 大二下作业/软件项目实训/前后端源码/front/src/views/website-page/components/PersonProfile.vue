<template>
  <div class="PersonProfile" :style="`border-bottom: ${hasBottomBorder?'1px solid #F2F2F2':'0'};`" @click="toDetail">
    <img :src="detail.url" alt="" />
    <div class="instroduce">
      <span class="name">{{ detail.name }}</span>
      <span class="research">{{ detail.research }}</span>
      <span class="label">主要成就：</span>
      <span class="achievement">{{ detail.achievement }}</span>
    </div>
  </div>
</template>
<script>
export default {
  components: {},
  props: {
    path: { type: String, default: '' },
    detail: { type: Object, default: () => { } },
    hasBottomBorder: { type: Boolean, default: true }
  },
  data() {
    return {
    }
  },
  created() {
  },
  methods: {
    toDetail() {
      this.$router.push(this.path)
    }
  }
}
</script>

<style lang="scss" scoped>
::v-deep.PersonProfile {
  font-family: PingFang SC;
  display: flex;
  height: 196px;
  cursor: pointer;
  &:hover {
    .instroduce {
      .name {
        color: #3776ff;
      }
    }
  }
  img {
    width: 120px;
    min-width: 120px;
    object-fit: contain;
  }
  .instroduce {
    margin: 25px 0 0 29px;
    padding-right: 70px;
    .name {
      font-size: 18px;
      font-weight: 700;
      color: #000000;
      transition: 0.3s;
    }
    .label {
      font-size: 16px;
      color: #000000;
      margin-top: 28px;
      display: inline-block;
    }
    .research {
      word-break: break-all;
      text-overflow: ellipsis;
      overflow: hidden;
      display: -webkit-box;
      -webkit-line-clamp: 2;
      -webkit-box-orient: vertical;
      font-size: 14px;
      color: #808080;
      margin-top: 14px;
    }
    .achievement {
      width: 780px;
      word-break: break-all;
      text-overflow: ellipsis;
      overflow: hidden;
      display: -webkit-box;
      -webkit-line-clamp: 2;
      -webkit-box-orient: vertical;
      font-size: 16px;
      color: #666666;
      line-height: 24px;
    }
  }
}
</style>
