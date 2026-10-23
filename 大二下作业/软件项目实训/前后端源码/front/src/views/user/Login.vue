<template>
  <el-container>
    <el-header height="68px">
      <img src="@/assets/img/logo.png" style="height:60px;width:60px" />
      <span class="title-span">{{ title }}</span>
    </el-header>
    <el-main>
      <el-form ref="loginForm" :model="loginForm" :rules="loginRules" class="login-form" auto-complete="off" label-position="right">
        <h1 style="margin-top: 0">{{ title2 }}</h1>
        <el-form-item prop="account">
          <el-input v-model="loginForm.account" type="text" auto-complete="on" placeholder="手机号/用户名" @focus="accountFocus = true" @blur="accountFocus = false">
            <svg-icon slot="prefix" icon-class="axt-s-user" :class="accountFocus ? 'active': ''" />
          </el-input>
        </el-form-item>
        <el-form-item prop="password">
          <el-input v-model="loginForm.password" type="password" auto-complete="on" placeholder="密码" show-password @keyup.enter.native="handleLogin" @focus="passwordFocus = true" @blur="passwordFocus = false">
            <svg-icon slot="prefix" icon-class="axt-s-password" :class="passwordFocus ? 'active': ''" />
          </el-input>
        </el-form-item>
        <!-- <div class="display-flex">
          <el-checkbox v-model="loginForm.rememberMe" label="记住密码" />
          <el-link @click="forgetPassword">
            <span>忘记密码？</span>
            <span class="text-color">点我找回</span>
          </el-link>
        </div> -->
        <div>
          <el-button :loading="loading" type="primary" class="button-login" @click.native.prevent="handleLogin">
            {{ loading?'登 录 中...':'登 录' }}
          </el-button>
        </div>
        <!-- <div align="center">
          <el-link @click="Register">
            <span>没有账号？</span>
            <span class="text-color">点我注册</span>
          </el-link>
        </div> -->
      </el-form>
    </el-main>
  </el-container>
</template>

<script>
import authAPI from '@/utils/auth'
import defaultSettings from '@/settings'
export default {
  name: 'Login',
  data() {
    return {
      loginForm: { account: '', password: '', rememberMe: false },
      // loginForm: { account: '15505096851', password: 'Axt-1234', rememberMe: false },
      // loginForm: { account: 'admin', password: 'Axt-1234', rememberMe: false },
      userInfo: { ...this.$store.getters.userInfo },
      loginRules: {
        // 登录规则
        account: [{ required: true, message: '请输入手机号/用户名', trigger: ['blur', 'change'] }],
        password: [{ required: true, trigger: ['blur', 'change'], message: '密码不能为空' }]
      },
      loading: false, // 加载中
      passwordType: 'password',
      redirect: undefined,
      accountFocus: false, // 账号输入框焦点
      passwordFocus: false // 密码输入框焦点
    }
  },
  computed: {
    title() {
      return defaultSettings.title
    },
    title2() {
      return defaultSettings.title2
    }
  },
  watch: {
    $route: {
      handler(route) {
        this.redirect = route.query && route.query.redirect
      },
      immediate: true
    }
  },
  created() {
    if (typeof authAPI.getAccount() === 'object') {
      this.loginForm = { ...authAPI.getAccount() }
    }
  },
  methods: {
    async handleLogin() {
      this.$refs.loginForm.validate(async valid => {
        if (valid) {
          this.loading = true
          try {
            await this.$store.dispatch('user/login', this.loginForm)
            if (this.loginForm.rememberMe) {
              authAPI.setAccount(this.loginForm)
            } else {
              authAPI.removeAccount()
            }
            this.$message.success('登录成功')
            // 添加箭头函数是由于vue-router的升级，需要避免 navigation guard 的错误
            setTimeout(() => { this.$router.push(this.redirect || '/Homepage', () => { }) }, 500)
            this.loading = false
          } catch (error) {
            this.loading = false
            return
          }
        } else {
          return false
        }
      })
    },
    // 忘记密码
    forgetPassword() {
      this.$router.replace('/Forget')
    },
    Register() {
      this.$router.replace('/Register')
    }
  }
}
</script>
<style lang="scss" scoped>
@import "~@/assets/css/variables.scss";

.el-header {
  display: flex;
  align-items: center;
  background-color: #e9eef3;
  img {
    width: 55px;
    height: 70px;
  }
  .title-span {
    line-height: 60px;
    font-size: 24px;
    color: $--color-primary;
    font-weight: bold;
    margin-left: 10px;
  }
}
.el-main {
  background: url("~@/assets/img/login_background.jpg") no-repeat;
  background-size: 100% auto;
  color: #333;
  .login-form {
    background: #f5f7fa;
    position: absolute;
    right: 60px;
    z-index: 999;
    width: 320px;
    top: 50%;
    transform: translateY(-50%);
    padding: 35px;
    border-radius: 6px;
    overflow: hidden;
    .input-icon {
      height: 39px;
      width: 14px;
      margin-left: 2px;
    }
  }
}
.el-container {
  width: 100%;
  height: 100%;
}
.text-color {
  color: $--color-primary;
}
.svg-icon {
  color: #8a8a8a;
}
.display-flex {
  display: flex;
  justify-content: space-between;
  align-items: center;
}
.svg-icon.active {
  color: $--color-primary;
}
.button-login {
  width: 100%;
  margin-top: 20px;
  margin-bottom: 20px;
}
</style>

