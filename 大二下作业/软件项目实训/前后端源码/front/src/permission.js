import router, { constantRoutes } from './router/index'
import getWebMenu from './api/website'
import store from './store'
import { Message } from 'element-ui'
import NProgress from 'nprogress' // progress bar
import 'nprogress/nprogress.css' // progress bar style
import { getToken } from '@/utils/auth' // get token from cookie
import { filterAsyncRoutes } from '@/store/modules/permission'
import { generateRoutes, loadView } from '@/store/modules/webSite'
import _ from 'lodash'

NProgress.configure({ showSpinner: false }) // NProgress Configuration
const whiteList = ['/Login', '/Forget', '/Register'] // no redirect whitelist
router.beforeEach(async(to, from, next) => {
  NProgress.start()
  if (!store.state.webSite.isSave) {
    const list = await getcolumn()    
    const websiteRoutes = await generateRoutes(list)    
    store.dispatch('updateStoreChange', true)
    store.dispatch('Nav', websiteRoutes)
    await addWebsiteRoutes(websiteRoutes, next, to)    
  }
  // 确定用户是否已经登录
  const hasToken = getToken()
  if (hasToken) {
    if (to.path === '/Login') {
      const val = '/Homepage'
      const nextUrl = constantRoutes.filter((i) => {
        val === i.redirect
      })
      if (nextUrl.length) {
        next('/Homepage')
      } else {
        const { menuList } = await store.dispatch('user/getUserInfo')
        const asyncRoutes = await filterAsyncRoutes(menuList)
        router.addRoute({ path: '*', redirect: '/404', hidden: true })
        await store.dispatch('generateRoutes', asyncRoutes)
        next(`${store.state.permission.addRoutes[0].children[0].path}`)
      }
      NProgress.done()
    } else {
      // 确定用户是否通过 getUserInfo 获取了权限角色
      const hasRoles = store.getters.roles && store.getters.roles.length > 0
      if (hasRoles) {
        next()
      } else {
        try {
          // 获取菜单信息及用户信息
          const { menuList } = await store.dispatch('user/getUserInfo')
          await loadMenus(menuList, next, to)
          if (to.path === '/Homepage') {
            const val = '/Homepage'
            const nextUrl = constantRoutes.filter((i) => {
              val === i.redirect
            })
            // 如果没有Homepage
            if (nextUrl.length === 0) {
              next(menuList[0].children[0].component)
            }
          }
        } catch (error) {
          // remove token and go to login page to re-login
          await store.dispatch('user/resetToken')
          Message.error(error.message || '功能获取失败，请联系管理员审核权限！')
          next(`/Login?redirect=${to.path}`)
          NProgress.done()
          // await store.dispatch('user/logout')
          // location.reload() // 为了重新实例化vue-router对象 避免bug
        }
      }
    }
  } else {
    // has no token
    if ((whiteList.indexOf(to.path) !== -1 || to.path.includes('website')) && !to.path.includes('website-')) {
      // 如果需要跳转的页面在 whiteList 中，就直接跳转
      next()
    } else {
      // 否则重定向到登录页
      // next(`/Login?redirect=${to.path}`)
      next('/website/homepage')
      NProgress.done()
    }
  }
})

const loadMenus = async(roles, next, to) => {
  const asyncRoutes = await filterAsyncRoutes(roles)
  router.addRoute({ path: '*', redirect: '/404', hidden: true })
  await store.dispatch('generateRoutes', asyncRoutes)
  if (to.path === '/') {
    next('/website/homepage')
    // next({
    //   path: asyncRoutes[0].children[0].path
    //   // replace: true
    // })
  } else {
    next({
      ...to
      // replace: true
    })
  }
}

// 获取所有栏目
const getcolumn = async() => {
  const list = await getWebMenu.column()
  return list
}
// 动态添加路由
const addWebsiteRoutes = async(list, next, to) => {
  list.forEach(i => {
    const temp = _.cloneDeep(i)
    temp.children = []
    router.addRoute('Website', temp)    
    if (i.children && i.children.length) {
      i.children.forEach(item => {
        item.component = loadView('website-page/pages/Detail')
        const parentName = item.name.replace('Detail', '')
        router.addRoute(parentName[0].toUpperCase() + parentName.slice(1), item)
      })
    }
  })
  // 用于首页跳转详情
  const commomDetail = {
    path: '/website/commonDetail',
    name: 'commonDetail',
    meta: { title: '详情', noCache: true, index: '1' },
    component: loadView('website-page/pages/Detail'),
    hidden: true
  }
  router.addRoute('Website', commomDetail)
  next({ path: to.path, query: to.query })
}

router.afterEach(() => {
  NProgress.done()
})
