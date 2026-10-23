<template>
  <div class="body">
    <div class="test">
      <div class="logo" @click="toHomepage" style="cursor: pointer;">
        <img :src="newLogo" alt="加载失败" />
        <div class="title">
          <div class="chinese">安徽省高等学校计算机教育研究会</div>
          <div class="english">Research Association of Computer Education in Colleges and Universlities of Anhui province</div>
        </div>
      </div>
      <el-menu class="box" :default-active="activeIndex" background-color="#3776FF" text-color="#FFFFFF" active-text-color="#FFFFFF" mode="horizontal" @select="handleSelect">
        <el-menu-item v-for="(item,index) in menuList" :key="index" :index="`${index+1}`">{{ item.allNodeNames }}</el-menu-item>
      </el-menu>
      <div class="right-operate">
        <el-select v-model="search" filterable clearable remote reserve-keyword placeholder="搜索" popper-class="option-class" :popper-append-to-body="false" :style="cssVar" :remote-method="remoteMethod" :loading="loading" @focus="focus" @blur="blur" @change="change">
          <i slot="prefix" class="el-icon-search" />
          <el-option v-for="item in options" :key="item.id" :title="item.channelCName" :label="item.channelCName" :value="item.id" />
        </el-select>
        <div class="btn-group">
          <router-link class="btn" target="_blank" :to="{ path: '/Login' }">登录</router-link>
          <span>|</span>
          <router-link class="btn" target="_blank" :to="{ path: '/Register' }">注册</router-link>
        </div>
      </div>
    </div>
  </div>
</template>
<script>
import newLogo from '@/assets/img/newLogo.png'
import webAPI from '@/api/website'
export default {
  name: 'TitleBar',
  components: {},
  data() {
    return {
      loading: false,
      options: [],
      newLogo,
      activeIndex: '1',
      menuList: [],
      search: '',
      inputWidth: '100px'
    }
  },
  computed: {
    cssVar() {
      return {
        '--width': this.inputWidth
      }
    }
  },
  watch: {
    $route() {
      this.activeIndex = this.$route.meta.index
    }
  },
  created() {
    this.menuList = this.$store.state.webSite.routeList
    this.activeIndex = this.$route.meta.index
  },
  methods: {
    focus() {
      this.inputWidth = '150px'
      if (!this.search) {
        this.options = []
      }
    },
    blur() {
      this.search ? this.inputWidth = '150px' : this.inputWidth = '100px'
    },
    change() {
      this.search ? this.inputWidth = '150px' : this.inputWidth = '100px'
      if (this.search) {
        const temp = this.options.filter(e => e.id === this.search)[0]
        this.$router.push({
          path: '/website/commonDetail',
          query: { row: JSON.stringify(temp) }
        })
      }
    },
    async remoteMethod(word) {
      if (word !== '') {
        try {
          this.loading = true
          const res = await webAPI.search(word)
          this.options = res
          this.loading = false
        } catch (error) {
          this.loading = false
        }
      } else {
        this.options = []
      }
    },
    handleSelect(key, keyPath) {
      if (this.$route.path !== this.menuList[key - 1].path) { this.$router.push(this.menuList[key - 1].path) }
    },
    toHomepage() {
      this.$router.push('/website')
    }
  }
}
</script>

<style lang="scss" scoped>

@media screen and (max-width: 765px) {
   .box {
    display: none;
  }
}


::v-deep.body {
  width: 100%;
  background-color: #3776ff;
  min-height: 64px;
  font-family: PingFang SC;
  display: flex;
  justify-content: center;
  .test {
    display: flex;
    align-items: center;
  }
  .logo {
    width: 303px;
    height: 36px;
    margin-right: 26px;
    display: flex;
    img {
      height: 100%;
    }
    .title {
      overflow: hidden;
      color: #fff;
      margin-left: 4px;
      .chinese {
        font-family: Songti TC;
        font-size: 16px;
        width: 100%;
        margin-top: 4px;
        letter-spacing: 1.4px;
      }
      .english {
        width: 550px;
        font-size: 12px;
        transform: scale(0.5) translateX(-50%);
        margin-top: -2px;
      }
    }
  }
  .el-menu-item {
    font-size: 16px;
    height: 64px;
    line-height: 64px;
    padding: 0 12.5px;
  }
  .el-menu--horizontal {
    border-bottom: 0;
    .el-menu-item:not(.is-disabled):hover,
    .el-menu-item:not(.is-disabled):focus {
      background-color: #3776ff !important;
    }
  }
  .el-menu--horizontal {
    .el-menu-item:not(.is-disabled):hover {
      color: #303133 !important;
    }
  }
  .el-menu--horizontal > .el-menu-item.is-active {
    border-bottom-color: #3776ff !important;
    position: relative;
    &::after {
      content: "";
      position: absolute;
      width: 50%;
      height: 6px;
      background-color: #729eff;
      bottom: -2px;
      left: 50%;
      transform: translateX(-50%);
      border-radius: 6px 6px 0 0;
    }
  }
  .right-operate {
    display: flex;
    align-items: center;
    .option-class {
      max-width: 400px;
    }
    .el-select {
      input::-webkit-input-placeholder,
      .el-input__prefix {
        color: #fff;
        top: 33%;
        left: 40px;
      }
      .el-input__suffix {
        padding-right: 10px;
      }
      .el-input__inner {
        width: var(--width);
        transition: all 0.2s linear;
        margin-left: 30px;
        font-size: 16px;
        margin-right: 10px;
        background-color: #3776ff;
        border: 0;
        color: #fff;
      }
    }
    .el-select:hover {
      .el-input__inner {
        width: 150px;
        border: 1px solid #fff;
      }
    }
    .btn-group {
      padding: 0 10px;
      box-sizing: border-box;
      width: 94px;
      min-width: 94px !important;
      height: 32px;
      background-color: #5c8fff;
      border-radius: 4px;
      display: flex;
      justify-content: space-between;
      align-items: center;
      color: #fff;
      font-size: 14px;
      .btn {
        cursor: pointer;
        transition: all 0.2s;
        &:hover {
          color: #303133;
        }
      }
    }
  }
}
</style>
