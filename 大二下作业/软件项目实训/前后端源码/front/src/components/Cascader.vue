<template>
  <el-cascader ref="cascader" v-model="arrVal" class="cascader" :size="size" :options="options" :props="props" :placeholder="placeholder" clearable="" @change="handleChange" />
</template>
<script>
import treeAPI from '@/api/tree'

export default {
  props: {
    value: { type: [Number, Array], default: () => [] }, // 父级 v-model 绑定值（后端返回的 id 值）
    keyWords: { type: String, default: 'TblAreaInfo' }, // 搜索级联选项关键字
    placeholder: { type: String, default: '请选择' },
    checkStrictly: { type: Boolean, default: false }, // 是否严格的遵守父子节点不互相关联
    size: { type: String, default: '' }
  },
  data() {
    return {
      arrVal: [],
      options: [], // 回显备选项
      props: {
        label: 'name',
        value: 'id',
        checkStrictly: this.checkStrictly,
        lazy: true,
        lazyLoad: this.lazyLoad
      }
    }
  },
  async created() {
    // 测试数据
    // const list = [
    //   { name: '河南省', parentId: -1, id: 410000 },
    //   { name: '郑州市', parentId: 410000, id: 410100 },
    //   { name: '开封市', parentId: 410000, id: 410200 }
    // ]
    // console.log(this.listToTree(list), '测试树结构')
    if (this.value && !(this.value instanceof Array)) {
      // 查询所有父节点
      const res = await treeAPI.getAllParentIndex(this.keyWords, this.value)
      this.arrVal = res.data.map(item => item.id).reverse()
      const ids = [-1].concat(this.arrVal)
      const levels = []
      for (let i = 0; i < ids.length - 1; i++) {
        const level = await this.getCurrentLevels(ids[i])
        levels.push(level)
      }
      // 这里的数组打印为空（待处理）
      this.options = this.listToTree(levels)
    }
  },
  methods: {
    listToTree(list) {
      const cloneData = JSON.parse(JSON.stringify(list)) // 对源数据深度克隆
      return cloneData.filter(parent => { // 循环所有项，并添加children属性
        const branchArr = cloneData.filter(child => parent['id'] === child['parentId']) // 返回每一项的子级数组
        branchArr.length > 0 ? parent['children'] = branchArr : '' // 给父级添加一个children属性，并赋值
        return parent['parentId'] === -1 // 返回第一层
      })
    },
    async getCurrentLevels(parentId) {
      try {
        const resp = await treeAPI.commonSearch({
          listKeyWords: this.keyWords,
          pageInfo: { page: -1 },
          searchKey: { parentId: parentId },
          sortJson: { properties: 'id', direction: 'ASC' }
        })
        const nodes = resp.data.map(item => ({
          id: item.id,
          name: item.name,
          parentId: parentId,
          leaf: item.childNum === 0 // 判断是否为末尾节点
        }))
        return nodes
      } catch (error) {
        return []
      }
    },
    // 懒加载
    async lazyLoad(node, resolve) {
      const { level } = node
      const parentId = level ? node.value : -1
      try {
        const nodes = await this.getCurrentLevels(parentId)
        resolve(nodes)
      } catch (error) {
        resolve([])
      }
    },
    handleChange(val) {
      // 接口入参需求为最后一级 id
      const lastId = val[val.length - 1]
      this.$emit('input', lastId)
    }
  }
}
</script>
