<template>
  <div class="foot">
    <div class="foot-content">
      <div class="inform">
        <div class="fath">
          <div>版权所有：安徽省高等院校计算机教育研究会</div>
          <el-divider direction="vertical" class="one" />
          <div class="fath"><div>技术支持：</div><div class="logo-placeholder">AXT</div><div>安徽爱学堂教育科技有限公司</div></div>
          <el-divider direction="vertical" />
          <span>专业认证</span>
          <el-divider direction="vertical" />
          <span>智慧学院</span>
        </div>
        <div class="bottom">备案号：皖ICP备14022014号-1 Copyright 2006-2014 All rights reserved</div>
      </div>
      <div class="code">
        <div class="qrcode-placeholder"></div>
        <span>关注公众号</span>
      </div>
    </div>
  </div>
</template>
<script>
export default {
  name: 'Footer',
  components: {},
  data() {
    return {}
  },
  created() {
  },
  methods: {
  }
}
</script>

<style lang="scss" scoped>
.foot {
  font-family: PingFang SC;
  height: 110px;
  width: 100%;
  background-color: #222c3c;
  position: relative;
  overflow: hidden;
  display: flex;
  justify-content: center;
  .foot-content {
    width: 1400px;
    display: flex;
    justify-content: space-between;
    .inform {
      color: #fff;
      margin-top: 32px;
      font-size: 14px;
      .fath{
        display: flex;
      }
      .bottom {
        color: #b8b8b8;
        margin-top: 15px;
      }
      .el-divider--vertical{
        width: 5px;
        transform: scale(0.2, 1);
      }
      .one{
        margin: 0 15px;
      }
      .logo-placeholder {
        background-color: #3776FF;
        color: white;
        font-weight: bold;
        padding: 0 5px;
        margin-right: 5px;
        border-radius: 2px;
      }
    }
    .code {
      margin-top: 15px;
      text-align: center;
      .qrcode-placeholder {
        width: 60px;
        height: 60px;
        background-color: #3776FF;
        position: relative;
        margin: 0 auto;
        border-radius: 5px;
        &:before {
          content: "扫码";
          position: absolute;
          top: 50%;
          left: 50%;
          transform: translate(-50%, -50%);
          color: white;
          font-size: 12px;
        }
      }
      span {
        font-size: 12px;
        color: #fff;
        display: block;
        margin-top: 10px;
      }
    }
  }
}
</style>
