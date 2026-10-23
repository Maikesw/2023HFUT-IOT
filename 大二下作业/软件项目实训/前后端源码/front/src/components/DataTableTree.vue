<template>
  <DataTableHeader ref="dataTBLMother" :default-props="defaultProps.defaultDTHProps" :selected-columns.sync="selectedColumns" @init-click="initDataTree" @append-click="appendClick" @edit-click="editClick" @delete-click="deleteClick" @upload-finish="uploadFinish">
    <template slot="body">
      <el-card shadow="never">
        <slot slot="header" name="cardTitle" />
        <template v-if="title" slot="header">
          <span>{{ title.mainTitle }}</span>
          <span v-if="title.subTitle">&nbsp;|&nbsp;<strong>{{ title.subTitle || "全部" }}</strong></span>
        </template>
        <el-table ref="dataTree" v-adaptive :lazy="lazy" :load="lazyLoad" height="100%" border :data="treeTableData" :tree-props="{children: 'children', hasChildren: 'hasChildren'}" row-key="id" highlight-current-row :default-expand-all="expandFlag" :expand-row-keys="['-1']" @current-change="handleNodeClick" @select="handleSelection" @selection-change="handleSelectionChange" @select-all="handleSelectionAll">
          <el-table-column v-if="checkFlag" type="selection" fixed width="55px" />
          <el-table-column v-for="(item, index) in tableColumnItem" :key="index" :show-overflow-tooltip="true" :label="item.showName" :width="item.width" align="left">
            <template #default="scope">
              <span v-if="item.tableColumnName === 'icon'">
                <svg-icon :icon-class="scope.row[item.tableColumnName]" />
              </span>
              <!-- 判断是否是日期格式 -->
              <span v-else-if="item.tableColumnName.endsWith('Time')">{{ scope.row[item.tableColumnName] | filterDateTime }}</span>
              <!-- 列表自定义显示的内容 >-->
              <slot v-else-if="item.tableColumnName.startsWith('customize-')" :name="item.tableColumnName.replace('customize-','')" :row="scope.row" />
              <span v-else>{{ scope.row[item.tableColumnName] || '--' }}</span>
            </template>
          </el-table-column>
          <el-table-column v-if="operateFlag" fixed="right" label="操作" :width="operateWidth">
            <template #default="scope">
              <el-button type="primary" icon="el-icon-plus" size="mini" title="新增下级" @click="appendClick(scope.row)" />
              <el-button v-if="button.update.show" type="info" icon="el-icon-edit" size="mini" :title="button.update.name" @click="editClick(scope.row)" />
              <el-button v-if="button.delete.show" type="danger" icon="el-icon-delete" size="mini" :title="button.delete.name" @click="remove([scope.row])" />
              <el-button v-if="button.up.show" type="warning" icon=" el-icon-top" size="mini" :title="button.up.name" @click="changeNodes('up',scope.row)" />
              <el-button v-if="button.down.show" type="warning" icon=" el-icon-bottom" size="mini" :title="button.down.name" @click="changeNodes('down',scope.row)" />
            </template>
          </el-table-column>
        </el-table>
      </el-card>
    </template>
  </DataTableHeader>
</template>
<script>

import DataTableHeader from '@/components/DataTableHeader'
import treeAPI from '@/api/tree'
import adaptive from '@/directive/el-table'
import moment from 'moment'

// 懒加载，新增第一个节点不展开，不出现小箭头，暂时未解决
// 遗留思路，新增第一个节点，让上级节点出现小箭头
export default {
  name: 'DataTableTree',
  components: { DataTableHeader },
  directives: { adaptive },
  filters: {
    filterDateTime(val) {
      return moment(val).format('YYYY-MM-DD HH:mm')
    }
  },
  props: {
    defaultProps: { // 注意这里的默认值没有用
      type: Object,
      default: () => {
        return {
          keyWord: { },
          title: { },
          allTableColumns: { },
          bottomOffset: 0,
          someFlags: {
            expandFlag: false,
            checkFlag: false,
            lazy: true,
            operateFlag: true,
            virtualRootFlag: false
          },
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
      operateWidth: 230,
      treeTableData: [],
      selectedColumns: [],
      // 列表显示的属性项
      tableColumnItem: [],

      checkAll: true,
      indeterminate: false,
      addDisabled: false,
      loading: true,
      isAllSelect: false,

      maps: new Map(),
      thisEvents: ''
    }
  },
  computed: {
    // variables() {
    //   return variables
    // },
    button() {
      return this.$refs.dataTBLMother.button
    },
    keyWord() {
      return this.$refs.dataTBLMother.keyWord
    },
    title() {
      return this.defaultProps.title ? this.defaultProps.title : null
    },
    expandFlag() {
      if (Object.prototype.hasOwnProperty.call(this.defaultProps, 'someFlags')) {
        if (Object.prototype.hasOwnProperty.call(this.defaultProps.someFlags, 'expandFlag')) {
          return this.defaultProps.someFlags.expandFlag
        }
      }
      return false
    },
    checkFlag() {
      if (Object.prototype.hasOwnProperty.call(this.defaultProps, 'someFlags')) {
        if (Object.prototype.hasOwnProperty.call(this.defaultProps.someFlags, 'checkFlag')) {
          return this.defaultProps.someFlags.checkFlag
        }
      }
      return true
    },
    lazy() {
      if (Object.prototype.hasOwnProperty.call(this.defaultProps, 'someFlags')) {
        if (Object.prototype.hasOwnProperty.call(this.defaultProps.someFlags, 'lazy')) {
          return this.defaultProps.someFlags.lazy
        }
      }
      return true
    },
    operateFlag() {
      if (Object.prototype.hasOwnProperty.call(this.defaultProps, 'someFlags')) {
        if (Object.prototype.hasOwnProperty.call(this.defaultProps.someFlags, 'operateFlag')) {
          return this.defaultProps.someFlags.operateFlag
        }
      }
      return true
    },
    virtualRootFlag() {
      if (Object.prototype.hasOwnProperty.call(this.defaultProps, 'someFlags')) {
        if (Object.prototype.hasOwnProperty.call(this.defaultProps.someFlags, 'virtualRootFlag')) {
          return this.defaultProps.someFlags.virtualRootFlag
        }
      }
      return false
    },
    initSearchWords() {
      return this.defaultProps.initSearchWords ? this.defaultProps.initSearchWords : { }
    },
    sort() {
      return this.defaultProps.sort ? this.defaultProps.sort : { properties: 'theOrder', direction: 'ASC' }
    },
    bottomOffset() {
      return this.defaultProps.bottomOffset ? this.defaultProps.bottomOffset : 0
    }
  },
  mounted() {
    this.thisEvents = JSON.parse(JSON.stringify(this._events))
    let num = 1
    const arr = [
      this.button.delete.show,
      this.button.update.show,
      this.button.up.show,
      this.button.down.show
    ]
    num = arr.reduce((acc, cur) => {
      acc = cur ? acc + 1 : acc
      return acc
    }, 1)
    var width = num * 46
    if (width < 60) {
      width = 60
    }
    this.operateWidth = width

    setTimeout(() => { this.initDataTree() }, 500)
  },
  methods: {
    // #region 初始化以及加载
    // 懒加载
    async lazyLoad(tree, treeNode, resolve) {
      if (tree.id === -1) {
        this.initDataTree()
      } else {
        var view = this.keyWord.view
        const res = await treeAPI.getAllNodes({
          keyWords: view,
          parentId: tree.id,
          virtualRootFlag: this.virtualRootFlag,
          searchKey: this.searchKey,
          lazy: true,
          preName: tree.allNodeNames,
          sort: this.sort
        })
        this.maps.set(tree.id, { tree, treeNode, resolve })
        resolve(res.data)
      }
    },
    // 加载部分树
    updatePartTree(row) {
      if (this.lazy) {
        var nowMaps
        if (this.maps.size > 0) {
          nowMaps = this.maps.get(row.parentId)
        }
        if (nowMaps === undefined) {
          this.lazyLoad({ id: -1, allNodeNames: '' }, null, null)
        } else {
          this.lazyLoad(nowMaps.tree, nowMaps.treeNode, nowMaps.resolve)
        }
      } else {
        this.initDataTree()
      }
    },
    // 初始化/刷新树（无论懒加载与否，初始时都会调用）
    async initDataTree(parentId = -1) {
      this.loading = true
      try {
        var view = this.keyWord.view
        var parentNode = { id: parentId, allNodeNames: '' }
        const res = await treeAPI.getAllNodes({
          keyWords: view,
          parentId: parentNode.id,
          virtualRootFlag: this.virtualRootFlag,
          searchKey: this.searchKey,
          lazy: this.lazy,
          preName: parentNode.allNodeNames,
          sort: this.sort
        })
        this.treeTableData = res.data
      } catch (error) {
        this.loading = false
        console.log(error)
      }
      this.loading = false
    },
    // #endregion

    // #region 选择相关
    // 监听选中的事件
    handleSelection(selection, row) {
      // this.$refs['dataTBLMother'].handleSelection(selection, row)
      if (selection.indexOf(row) !== -1) {
        this.selectChange(selection, row, true, true)
      } else {
        this.selectChange(selection, row, false, true)
      }
      this.$emit('update:selectedColumns', selection)
    },
    // 监听选中的事件
    handleSelectionChange(selection) {
      this.selectedColumns = selection
    },
    // 全选
    handleSelectionAll(selection) {
      this.isAllSelect = !this.isAllSelect
      if (this.isAllSelect) {
        selection.forEach(val => {
          this.selectChange(selection, val, true, false)
        })
      } else {
        this.$refs['dataTree'].clearSelection()
        selection = []
      }
      this.$emit('update:selectedColumns', selection)
    },
    selectChange(selection, row, isChecked, notAll) {
      if (row.children && row.children.length) {
        row.children.forEach(val => {
          this.$refs['dataTree'].toggleRowSelection(val, isChecked)
          if (notAll) {
            isChecked
              ? selection.push(val)
              : selection.splice(selection.indexOf(val), 1)
          }
          if (row.children && row.children.length) {
            this.selectChange(selection, val, isChecked, notAll)
          }
        })
      }
    },
    // #endregion
    // 按钮新增
    async appendClick(row) {
      this.$emit('append-click', row)
    },
    // 按钮修改
    async editClick(row) {
      this.$emit('edit-click', row)
    },
    // 按钮删除
    async deleteClick(row) {
      if (Object.prototype.hasOwnProperty.call(this.thisEvents, 'delete-click')) {
        this.$emit('delete-click', row)
      } else {
        this.loading = true
        await treeAPI.delManyNode(this.keyWord.edit, row)
        this.$refs['dataTree'].clearSelection()
        this.updatePartTree(row[0])
        this.$message.success('删除成功！')
        this.loading = false
      }
    },
    // 删除
    remove(row) {
      this.$refs['dataTBLMother'].remove(row)
    },
    // 移动节点
    async changeNodes(direction, node) {
      this.loading = true
      const resp = await treeAPI.getAllBrotherIndex(this.keyWord.edit, node.id)
      const nodes = resp.data
      const index = nodes.findIndex(item => item.id === node.id)
      if (direction === 'up' && index === 0) {
        this.$message.warning('当前节点是第一个节点，无法上移！')
      } else if (direction === 'down' && index === nodes.length - 1) {
        this.$message.warning('当前节点是最后一个节点，无法下移！')
      } else {
        const changeId = direction === 'up' ? nodes[index - 1].id : nodes[index + 1].id
        await treeAPI.changeTwoNodes(this.keyWord.edit, node.id, changeId)
        await this.updatePartTree(node)
        this.$message.success('移动成功！')
      }
      this.loading = false
    },
    // 监听点击某一行事件
    handleNodeClick(val) {
      this.$emit('node-click', val)
    },
    uploadFinish() {
      this.initDataTree()
    }
  }
}
</script>
