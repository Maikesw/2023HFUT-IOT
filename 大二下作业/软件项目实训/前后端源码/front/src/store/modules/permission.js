import router, { constantRoutes } from '@/router/index'
import Layout from '@/layout/Index'

/**
 * 递归权限过滤和数据转换
 * @param routes
 * @param parentId
 */

export const filterAsyncRoutes = (routes, parentPath = '') => {
  const result = []
  routes.forEach(route => {
    const temp = generateRoute(route, parentPath)
    // 如果父级路由包含子路由，递归调用 filterAsyncRoutes
    if (route.children && route.children.length) {
      const children = filterAsyncRoutes(route.children, route.path)
      temp.children = children
    }
    result.push(temp)

    // 添加路由规则
    // 如果 parentPath 不为空，则说明是子级路由
    if (parentPath) {
      // console.log(parentName, '添加一条新的路由规则记录作为现有路由的子路由')
      // // 添加一条新的路由规则记录作为现有路由的子路由
      // router.addRoute(parentName, route)
    } else {
      // 否则为父级路由
      // 添加一条新路由规则
      router.addRoute(temp)
    }
  })

  return result
}

// 生成路由格式
export const generateRoute = (route, parentPath) => {
  const path = parentPath ? parentPath + '/' + route.path : route.path
  const tempRoute = {
    path: path,
    name: path.replace(/\//g, ''), // 去除 path 中的 /
    meta: {
      title: route.name,
      icon: route.icon,
      activeMenu: route.activeMenu,
      isBreadcrumb: true,
      affix: false,
      // noCache: true
      isCache: true
    },
    hidden: route.hidden
  }
  tempRoute.component = route.component === 'Layout' ? Layout : loadView(route.component)
  return tempRoute
}

export const loadView = (view) => {
  return (resolve) => require([`@/views/${view}`], resolve)
}

const permission = {
  state: {
    routes: constantRoutes,
    addRoutes: []
  },
  mutations: {
    SET_ROUTES: (state, routes) => {
      const errorPage = [{ path: '*', redirect: '/404', hidden: true }]
      state.addRoutes = routes
      state.routes = constantRoutes.concat(routes).concat(errorPage)
    }
  },
  actions: {
    // 获取路由
    generateRoutes({ commit }, asyncRoutes) {
      commit('SET_ROUTES', asyncRoutes)
    }
  }
}

export default permission
