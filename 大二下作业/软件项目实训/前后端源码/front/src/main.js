import Vue from 'vue'
import App from './App.vue'
import store from './store'
import router from './router/index'

Vue.config.productionTip = false

import './assets/icons' // icon
import 'normalize.css/normalize.css' // A modern alternative to CSS resets
import './permission' // 权限控制

import ElementUI from 'element-ui'
Vue.use(ElementUI)

import './components/global/index' // 全局公用组件
import './prototypes/index' // 全局挂载方法

new Vue({
  el: '#app',
  router,
  store,
  render: h => h(App)
})
