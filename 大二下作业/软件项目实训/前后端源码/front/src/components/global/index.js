import Vue from 'vue'
import TreeList from '@/components/tree-list/Index.vue' // TreeList component
import Empty from '@/components/Empty.vue'

const components = {
  'tree-list': TreeList,
  'empty': Empty
}

// register globally
Object.keys(components).forEach((key) => {
  Vue.component(key, components[key])
})

