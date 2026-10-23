<template>
  <header id="title-bar" class="title-bar fixed-title-bar" :style="bgColor">
    <div class="headline">
      <img v-if="logo" :src="logo" />
      <h4>{{ title }}</h4>
    </div>
    <div class="right-menu">
      <div class="dashboard-picker">
        <svg-icon icon-class="axt-s-dashboard" />
        <theme-picker @change="themeChange" />
      </div>
      <!-- <el-avatar v-if="$store.getters.userInfo.headImage" :size="36" :src="$store.getters.userInfo.headImage" />1
      <el-avatar v-else :size="36" icon="el-icon-user-solid" /> -->
      <el-dropdown trigger="click">
        <div class="title-user-name">
          <span>{{ $store.getters.userInfo.name || "先生" }}，你好&nbsp;</span>
          <i class="el-icon-caret-bottom" />
        </div>
        <el-dropdown-menu slot="dropdown" class="user-dropdown">
          <el-dropdown-item @click.native="personalInfo">
            <span>个人信息</span>
          </el-dropdown-item>
          <el-dropdown-item @click.native="updatePassword">
            <span>修改密码</span>
          </el-dropdown-item>
          <el-dropdown-item @click.native="logout">
            <span style="display: block">退出登录</span>
          </el-dropdown-item>
        </el-dropdown-menu>
      </el-dropdown>
    </div>
    <DlgBasic ref="dlgPassword" :default-props="defaultDBProps" :dlgbasic-confirm="submit" @closeDialog="closeDialog">
      <template slot="mainForm">
        <el-form ref="form" :model="form" size="small" label-suffix=":" label-width="100px">
          <el-form-item prop="password" label="新密码">
            <el-input type="password" v-model="form.password"  placeholder="请输入新密码" />
          </el-form-item>
          <!-- <el-form-item prop="checkPass" label="确认新密码">
            <el-input v-model="form.checkPass" type="password" placeholder="请再次输入新密码" />
          </el-form-item> -->
        </el-form>
      </template>
    </DlgBasic>
  </header>
</template>
<script>
import defaultSettings from '@/settings'
import DlgBasic from '@/components/DlgBasic'
import ThemePicker from '@/components/theme-picker/Index'
import userAPI from '@/api/user'
import logo from '@/assets/img/logo.png'

export default {
  components: { DlgBasic, ThemePicker },
  data() {
    return {
      logo,
      title: '',      
      userInfo: { ...this.$store.getters.userInfo },
      form: { password: '' },
      defaultDBProps: {
        dlgTitle: '修改密码',
        footButtons: { repeatAdd: { show: false }},
        someFlags: {
          needValidate: false
        }
      },
      btnDisabled: false, // 获取二维码按钮点击状态
      passwordFocus: false, // 密码输入框焦点
      passwordsFocus: false // 密码输入框焦点
    }
  },
  computed: {
    bgColor() {
      return { backgroundColor: this.$store.getters.userInfo.themeColor }
    },
    headImage() {
      return this.$store.getters.userInfo.headImage
    }
  },
  watch: {
    bgColor(color) {
      this.$emit('bgColor', this.$store.getters.userInfo.themeColor)
    }
  },
  created() {
    this.getTitle()
  },
  methods: {
    async themeChange(color) {
      this.userInfo.themeColor = color
      await userAPI.editUserInfo(this.userInfo.id, this.userInfo)
      this.$store.dispatch('user/updateUserInfo', this.userInfo)
      document.getElementsByTagName('body')[0].style.setProperty('--color-primary', color)
    },
    async getTitle() {
      this.title = defaultSettings.title
    },
    toggleSideBar() {
      this.$store.dispatch('app/toggleSideBar')
    },
    personalInfo() {
      this.$router.push('/personal-information-manage/Personal')
    },
    updatePassword() {
      this.$refs.dlgPassword.showDialog(true, this.form)
    },
    async logout() {
      await this.$store.dispatch('user/logout')
      this.$router.push(`/Login`)
    },
    async submit() {
      await userAPI.editPassword(this.$store.getters.userInfo.id, this.form.password)
      this.$message.success('修改成功！')
      this.$refs.dlgPassword.showDialog(false, this.form)
    },
    closeDialog(data) {
      this.$emit('closeDialog', data)
    }
  }
}
</script>

<style lang="scss" scoped>
@import "~@/assets/css/variables.scss";
.title-bar {
  width: 100%;
  height: $titleBarHeight;
  display: flex;
  justify-content: space-between;
  .headline {
    img {
      width: 35px;
      height: 35px;
      padding-right: 15px;
    }
    span {
      margin-left: 10px;
    }
    display: flex;
    align-items: center;
    letter-spacing: 1px;
    font-size: 16px;
    color: $headlineColor;
    margin-left: 1em;
  }
  .right-menu {
    display: flex;
    justify-content: space-between;
    align-items: center;
    .el-avatar ::v-deep img {
      width: 100%;
    }
    .el-avatar + .el-dropdown {
      margin-left: 15px;
    }
    margin-right: 1em;
    .el-dropdown {
      cursor: pointer;
    }
  }
  .title-user-name {
    color: $headlineColor;
  }
}
// .fixed-title-bar {
//   position: fixed;
//   top: 0;
//   // z-index: 999;
// }
.dashboard-picker {
  position: relative;
  width: 30px;
  margin-right: 20px;
  height: $titleBarHeight;
  .theme-picker,
  .axt-s-dashboard {
    position: absolute;
    top: 50%;
    left: 0;
    transform: translateY(-50%);
  }
  .theme-picker {
    opacity: 0;
    top: 32px;
  }
  .axt-s-dashboard {
    color: #fff;
    font-size: 26px;
  }
}
</style>

