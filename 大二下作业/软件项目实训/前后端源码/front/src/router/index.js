import Vue from 'vue'
import Router from 'vue-router'
Vue.use(Router)

// 路由跳转重复异常解决
const originalPush = Router.prototype.push
Router.prototype.push = function push(location) {
  return originalPush.call(this, location).catch(err => err)
}

/**
 * Note: sub-menu only appear when route children.length >= 1
 * Detail see: https://panjiachen.github.io/vue-element-admin-site/guide/essentials/router-and-nav.html
 *
 * hidden: true                   if set true, item will not show in the sideBar(default is false)
 * alwaysShow: true               if set true, will always show the root menu
 *                                if not set alwaysShow, when item has more than one children route,
 *                                it will becomes nested mode, otherwise not show the root menu
 * redirect: noRedirect           if set noRedirect will no redirect in the breadcrumb
 * name:'router-name'             the name is used by <keep-alive> (must set!!!)
 * meta : {
    roles: ['admin','editor']    control the page roles (you can set multiple roles)
    title: 'title'               the name show in sideBar and breadcrumb (recommend set)
    icon: 'svg-name'/'el-icon-x' the icon show in the sideBar
    noCache: true                if set true, the page will no be cached(default is false)
    affix: true                  if set true, the tag will affix in the tags-view
    breadcrumb: false            if set false, the item will hidden in breadcrumb(default is true)
    activeMenu: '/example/list'  if set path, the sideBar will highlight the path you set
  }
 */

/**
 * constantRoutes
 * 没有权限要求的页面
 * 所有权限通用的公共页面
 */
export const constantRoutes = [
  // 如果需要主页，把下面声明打开
  // {
  //   path: '/',
  //   component: Layout,
  //   redirect: '/Homepage',
  //   children: [
  //     {
  //       path: 'Homepage',
  //       component: (resolve) => require(['@/views/Homepage'], resolve),
  //       name: 'Homepage',
  //       meta: { title: '主页', icon: 'axt-homepage', affix: true, noCache: true }
  //     }
  //   ]
  // },
  {
    path: '/Login',
    name: 'Login',
    meta: { title: '登录', noCache: true },
    component: () => import('@/views/user/Login'),
    hidden: true
  },
  {
    path: '/website',
    name: 'Website',
    meta: { title: '官网', noCache: true },
    component: () => import('@/views/website-page/Index'),
    hidden: true
  },
  {
    path: '/404',
    name: 'Page404',
    meta: { title: '404', noCache: true },
    component: () => import('@/views/404'),
    hidden: true
  }
]

/**
 * asyncRoutes
 * 根据用户权限设置的动态路由
 */
export const asyncRoutes = [
  // 404 page must be placed at the end !!!
  { path: '*', redirect: '/404', hidden: true }
]

const createRouter = () => new Router({
  mode: 'history', // require service support
  scrollBehavior: () => ({ y: 0 }),
  routes: constantRoutes
})

const router = createRouter()
// Detail see: https://github.com/vuejs/vue-router/issues/1234#issuecomment-357941465
export function resetRouter() {
  const newRouter = createRouter()
  router.matcher = newRouter.matcher // reset router
}

export default router
