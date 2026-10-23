import adaptiveCard from './adaptive-card'

const install = function(Vue) {
  // 绑定v-adaptive指令
  Vue.directive('adaptive-card', adaptiveCard)
}

if (window.Vue) {
  window['adaptive-card'] = adaptiveCard
  // eslint-disable-next-line no-undef
  Vue.use(install)
}

adaptiveCard.install = install

export default adaptiveCard
