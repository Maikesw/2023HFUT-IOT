<template>
  <el-cascader v-if="isShowInfo" ref="cascader" v-model="arrVal" v-loading="loading" class="cascader" :multiple="multiple" :append-to-body="false" :size="size" :options="options" :props="props" :placeholder="placeholder" clearable="" @change="handleChange" />
</template>
<script>
// cascader没有loading属性
import treeAPI from '@/api/tree'
import _ from 'lodash'

export default {
  name: 'SimpleTreeSelect',
  props: {
    value: { type: Number, default: () => null }, // 父级 v-model 绑定值（后端返回的 id 值）
    field: { type: String, default: '' },
    keyWords: { type: String, default: '' }, // 搜索级联选项关键字
    placeholder: { type: String, default: '请选择' },
    checkStrictly: { type: Boolean, default: true }, // 是否严格的遵守父子节点不互相关联
    size: { type: String, default: '' },
    searchKeys: { type: Object, default: () => {} },
    lazy: { type: Boolean, default: true },
    multiple: { type: Boolean, default: false }
  },
  data() {
    return {
      isShowInfo: true,
      arrVal: this.value,
      valTemp: [],
      options: [], // 回显备选项
      loading: true,
      props: {
        label: 'name',
        value: 'id',
        children: 'children',
        checkStrictly: this.checkStrictly,
        lazy: this.lazy,
        lazyLoad: this.lazyLoad
      }
    }
  },
  watch: {
    // 通过value激发initOptions
    value: {
      async handler(val) {
        this.changeSelection()
      },
      deep: true,
      immediate: true
    }
  },
  methods: {
    async changeSelection() {
      this.loading = true
      if (this.value) {
        const res = await treeAPI.getAllParentIndex(this.keyWords, this.value)
        if (res.data.length > 0) {
          this.valTemp = res.data.map(item => item.id).reverse()
        }
      } else {
        this.valTemp = []
      }
      await this.initOptions()

      if (this.lazy) {
        this.isShowInfo = false
        this.arrVal = [...this.valTemp]
        this.$nextTick(() => {
          this.isShowInfo = true
        })
      }
      this.loading = false
    },
    async initOptions(searchKey) {
      if (searchKey != null) {
        this.searchKeys = searchKey
      }
      try {
        if (!this.lazy) { // 不是懒加载
          if (this.options.length === 0) { // 没有加载过,直接加载完
            const res = await treeAPI.getAllNodes({
              keyWords: this.keyWords,
              virtualRootFlag: false,
              searchKey: this.searchKeys,
              lazy: false
            })
            this.options = res.data
          }
        } else { // 懒加载
          var arrNow = _.cloneDeep(this.valTemp)
          arrNow.unshift(-1)
          arrNow.pop()
          arrNow.reverse()
          var res = []
          var lastRes = []
          var lastParent = 0
          this.options = []
          arrNow.forEach(async element => {
            res = await treeAPI.getAllNodes({
              keyWords: this.keyWords,
              virtualRootFlag: false,
              searchKey: searchKey,
              lazy: true,
              parentId: element
            })
            res = res.data
            if (lastParent !== 0) {
              res.forEach(el => {
                if (el.id === element) {
                  el.children.push(lastRes)
                }
              })
            }
            lastRes = _.cloneDeep(res)
            lastParent = element
          })
          this.options = res
        }
      } catch (error) { console.log(error) }
    },
    // listToTree(list) {
    //   const cloneData = JSON.parse(JSON.stringify(list)) // 对源数据深度克隆
    //   return cloneData.filter(parent => { // 循环所有项，并添加children属性
    //     const branchArr = cloneData.filter(child => parent['id'] === child['parentId']) // 返回每一项的子级数组
    //     branchArr.length > 0 ? parent['children'] = branchArr : '' // 给父级添加一个children属性，并赋值
    //     return parent['parentId'] === -1 // 返回第一层
    //   })
    // },
    async getCurrentLevels(parentId) {
      try {
        const res = await treeAPI.getAllNodes({
          keyWords: this.keyWords,
          virtualRootFlag: false,
          searchKey: this.searchKeys,
          lazy: this.lazy,
          parentId: parentId
        })
        const nodes = res.data.map(item => ({
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
        // this.changeSelection()
      } catch (error) {
        resolve([])
      }
    },
    // 接口入参需求为最后一级 id
    handleChange(val) {
      const lastId = val[val.length - 1]
      this.$emit('update-value', lastId, this.field)
    },
    handleFocus(val) {
      console.log(val)
    }
  }
}
</script>
