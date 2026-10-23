import userAPI from '@/api/user'
import { setToken, removeToken } from '@/utils/auth'
import router, { resetRouter } from '@/router'

const state = {
  userInfo: {},
  roles: [],
  contestTypes: []
}

const mutations = {
  SET_TOKEN: (state, token) => {
    state.token = token
  },
  SET_USERINFO: (state, userInfo) => {
    state.userInfo = userInfo
  },
  SET_ROLES: (state, roles) => {
    state.roles = roles
  },
  SET_CONTESTTYPES: (state, types) => {
    state.contestTypes = types
  }
}

const actions = {
  // 用户登录
  login({ commit }, userInfo) {
    userInfo.account = userInfo.account.trim()
    userInfo.password = userInfo.password.trim()
    return new Promise((resolve, reject) => {
      userAPI.login(userInfo).then(resp => {
        const token = resp.data
        setToken(token)
        resolve()
      }).catch(error => {
        reject(error)
      })
    })
  },
  // 获取用户信息
  getUserInfo({ commit, dispatch }) {
    return new Promise((resolve, reject) => {
      userAPI.getUserInfo().then(res => {
        if (!res.data) {
          reject('验证失败，请重新登录')
        }
        // 主题色为空的更改
        res.data.userInfo.themeColor ? '' : res.data.userInfo.themeColor = '#009140'
        commit('SET_USERINFO', res.data.userInfo)
        commit('SET_ROLES', res.data.roles)
        commit('SET_CONTESTTYPES', res.data.contestTypes)
        resolve(res.data)
      }).catch(error => {
        reject(error)
      })
    })
  },

  // 用户登出
  logout({ commit, state, dispatch }) {
    return new Promise((resolve, reject) => {
      userAPI.logout().then(() => {
        commit('SET_USERINFO', {})
        commit('SET_ROLES', [])
        commit('SET_CONTESTTYPES', [])
        removeToken()
        resetRouter()
        // reset visited views and cached views
        dispatch('tagsView/delAllViews', null, { root: true })

        resolve()
      }).catch(error => {
        reject(error)
      })
    })
  },

  // 删除 token roles
  resetToken({ commit }) {
    return new Promise(resolve => {
      commit('SET_ROLES', [])
      removeToken()
      resolve()
    })
  },

  // 更新用户信息
  updateUserInfo({ commit }, userInfo) {
    return commit('SET_USERINFO', userInfo)
  },

  // 动态编辑权限
  async changeRoles({ commit, dispatch }, role) {
    // const token = role + '-token'
    // commit('SET_TOKEN', token)
    // setToken(token)

    const { roles } = await dispatch('getUserInfo')
    resetRouter()
    // generate accessible routes map based on roles
    const accessRoutes = await dispatch('permission/generateRoutes', roles, { root: true })
    // dynamically add accessible routes
    router.addRoutes(accessRoutes)

    // reset visited views and cached views
    dispatch('tagsView/delAllViews', null, { root: true })
  }
}

export default {
  namespaced: true,
  state,
  mutations,
  actions
}

