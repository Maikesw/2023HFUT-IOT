// eslint-disable-next-line space-before-function-paren
export const generateRoutes = async (list) => {
  list.forEach((item, index) => {
    item.meta = {
      id: item.id,
      title: item.name,
      noCache: true,
      index: `${index + 1}`,
      children: item.children && item.children.length ? item.children.map(e => {
        return { title: e.name, id: e.id }
      }) : []
    }
    item.name = item.component
    item.component = loadView(`website-page/pages/${item.component}`)
    if (item.path) {
      item.children = [
        {
          path: `${item.path}/${item.path.slice(item.path.lastIndexOf('/') + 1, item.path.length)}Detail`,
          name: `${item.path.slice(item.path.lastIndexOf('/') + 1, item.path.length)}Detail`,
          meta: { title: '详情', noCache: true, index: `${index + 1}` },
          component: () => loadView('website-page/pages/Detail'),
          hidden: false
        }
      ]
    }
  })
  return list
}

export const loadView = (view) => {
  return (resolve) => require([`@/views/${view}`], resolve)
}

const state = {
  isSave: false,
  routeList: []
}

const mutations = {
  CHANGESTORE(state, bool) {
    state.isSave = bool
  },
  HEARDER(state, arr) {
    state.routeList = arr
  }
}

const actions = {
  updateStoreChange(context, state) {
    context.commit('CHANGESTORE', state)
  },
  Nav(context, state) {
    context.commit('HEARDER', state)
  }
}

export default {
  state,
  mutations,
  actions
}
