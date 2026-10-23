<template>
  <el-card v-adaptive-card :class="title ? '' : 'card-notitle'" shadow="never">
    <template v-if="title" slot="header">
      <span>{{ title.mainTitle }}</span>
      <span v-if="title.subTitle">&nbsp;|&nbsp;<strong>{{ title.subTitle || "全部" }}</strong></span>
    </template>
    <!--  <div v-if="title" id="table-box-title" slot="header">
      <span>{{ title }}</span>
      <span v-show="subtitle">
        &nbsp;|&nbsp;
        <strong>{{ subtitle }}</strong>
      </span>
    </div> -->
    <el-tree ref="dataTree" v-adaptive-card v-loading="loading" :lazy="lazy" :load="lazyLoad" highlight-current node-key="id" :show-checkbox="checkFlag" :expand-on-node="false" :default-expanded-keys="[-1]" :data="treeData" :props="defaultDataProps" @node-click="handleNodeClick" />
  </el-card>
</template>

<script>
import treeAPI from '@/api/tree'
import adaptiveCard from '@/directive/el-card'

export default {
  name: 'DataTree',
  directives: { adaptiveCard },
  props: {
    defaultProps: { // 注意这里的默认值没有用
      type: Object,
      default: () => {
        return {
          title: { mainTitle: '', subTitle: '' },
          checkFlag: { type: Boolean, default: false },
          lazy: { type: Boolean, default: false },
          virtualRootFlag: { type: Boolean, default: true },
          keyWord: { type: String, default: '' },
          initSearchWords: { // 初始时查询的三个关键词
            searchKey: { },
            regKey: { },
            andor: { }
          }

        }
      }
    }
  },
  data() {
    return {
      treeData: [],
      defaultDataProps: {
        label: 'name',
        children: 'children',
        isLeaf: 'isLeaf'
      },
      loading: false,
      maps: new Map()
    }
  },
  computed: {
    // variables() {
    //   return variables
    // },
    keyWord() {
      return this.defaultProps.keyWord ? this.defaultProps.keyWord : null
    },
    title() {
      return this.defaultProps.title ? this.defaultProps.title : null
    },
    lazy() {
      if (Object.prototype.hasOwnProperty.call(this.defaultProps, 'lazy')) {
        return this.defaultProps.lazy
      }
      return true
    },
    checkFlag() {
      if (Object.prototype.hasOwnProperty.call(this.defaultProps, 'checkFlag')) {
        return this.defaultProps.checkFlag
      }
      return false
    },
    sort() {
      return this.defaultProps.sort ? this.defaultProps.sort : { properties: 'theOrder', direction: 'ASC' }
    },
    virtualRootFlag() {
      if (Object.prototype.hasOwnProperty.call(this.defaultProps, 'virtualRootFlag')) {
        return this.defaultProps.virtualRootFlag
      }
      return false
    },
    initSearchWords() {
      return this.defaultProps.initSearchWords ? this.defaultProps.initSearchWords : { }
    }
  },
  created() {
    this.initDataTree()
  },
  methods: {
    // 初始懒加载
    lazyLoad(tree, resolve) {
      if (tree.key) {
        treeAPI.getAllNodes({
          keyWords: this.keyWord,
          virtualRootFlag: this.virtualRootFlag,
          searchKey: this.searchKey,
          lazy: true,
          parentId: tree.key,
          sort: this.sort
        }).then(res => {
          this.maps.set(tree.key, { tree, resolve })
          resolve(res.data)
        })
      }
    },
    // 初始化/刷新树
    async initDataTree(parentId) {
      this.loading = true
      if (!parentId) {
        parentId = -1
      }
      try {
        const res = await treeAPI.getAllNodes({
          keyWords: this.keyWord,
          virtualRootFlag: this.virtualRootFlag,
          searchKey: this.searchKey,
          lazy: this.lazy,
          parentId: parentId,
          sort: this.sort
        })
        this.treeData = res.data
      } catch (error) { this.loading = false }
      this.loading = false
    },
    // 点击节点事件
    handleNodeClick(row) {
      this.$emit('node-click', row)
    }
  }
}
</script>
<style lang="scss" scoped>
::v-deep .el-card__body {
  padding: 5px 12px;
}
.card-notitle {
  border: unset;
  transition: unset;
}
.el-scrollbar .el-scrollbar__wrap {
  overflow-x: hidden;
}
.el-tree {
  overflow: auto;
}
#table-box-title {
  overflow: hidden;
  text-overflow: ellipsis;
  white-space: nowrap;
}
</style>
