<template>
  <div>
    <div v-if="curd" id="crud-opts" class="crud-opts">
      <span>
        <!--左侧插槽-->
        <slot name="left" />
        <el-button v-if="curd.create" size="mini" type="primary" icon="el-icon-plus" :disabled="addDisabled" @click="$emit('create')">新增</el-button>
        <el-button v-if="curd.update" size="mini" type="info" icon="el-icon-edit" :disabled="checkedNodes.length !== 1" @click="edit(checkedNodes[0])">修改</el-button>
        <el-button v-if="curd.delete" slot="reference" type="danger" icon="el-icon-delete" size="mini" :disabled="checkedNodes.length < 1" @click="remove()">删除</el-button>
        <el-button v-if="curd.export" size="mini" type="warning" :disabled="checkedNodes.length < 1" icon="el-icon-upload" @click="exportData">导出</el-button>
        <el-button v-if="curd.batchExport" size="mini" type="warning" icon="el-icon-upload" @click="exportAllData">全部导出</el-button>
        <!--右侧-->
        <slot name="right" />
      </span>
      <span v-if="curd.search">
        <el-button-group>
          <el-button size="mini" icon="el-icon-refresh" title="刷新" @click="initDataTree()" />
          <el-popover placement="bottom-end" width="100" trigger="click">
            <el-button slot="reference" size="mini" title="选择性展示" icon="el-icon-s-grid" />
            <el-checkbox v-model="checkAll" :indeterminate="indeterminate" @change="handleCheckAllChange">全选</el-checkbox>
            <el-checkbox v-for="item in tableColumns" :key="item.id" v-model="item.visible" @change="handleCheckSingleChange(item)">{{ item.showName }}</el-checkbox>
          </el-popover>
        </el-button-group>
      </span>
    </div>

    <el-card shadow="never">
      <slot slot="header" name="cardTitle" />
      <el-tree ref="tree" v-adaptive-card :data="treeTableData" :props="defaultProps" :show-checkbox="checkFlag" draggable node-key="id" highlight-current check-strictly default-expand-all @check="checkSelected">
        <span slot-scope="{ node, data }" class="custom-tree-node">
          <span>{{ node.label }}</span>
          <span>
            <el-button type="primary" icon="el-icon-plus" size="mini" title="新增下级" @click="() => append(data)" />
            <el-button type="danger" icon="el-icon-delete" size="mini" title="删除" @click="() => remove([data.id])" />
          </span>
        </span>
      </el-tree>
    </el-card>
  </div>
</template>
<script>
import treeAPI from '@/api/tree'
import listAPI from '@/api/list'
import IEAPI from '@/api/importAndExport'
import variables from '@/assets/css/variables.scss'
import { getTemplateFile } from '@/utils/common'
import adaptiveCard from '@/directive/el-card'

export default {
  name: 'DataTree',
  directives: { adaptiveCard },
  props: {
    // 控制树结构上方的按钮的显隐
    curd: {
      type: Object,
      default: () => {
        return { create: true, update: true, delete: true, export: true, search: true, batchExport: false }
      }
    },
    selectedColumns: { type: Array, default: () => [] },
    expandFlag: { type: Boolean, default: false },
    checkFlag: { type: Boolean, default: false },
    keyWords: { type: String, default: '' },
    operateFlag: { type: Boolean, default: true }
  },
  data() {
    return {
      treeTableData: [],
      checkedNodes: [], // 所选节点数据
      defaultProps: { label: 'name' },
      // 列表显示的属性项
      tableColumns: [],
      tableColumnItem: [],
      checkAll: true,
      indeterminate: false,
      addDisabled: false,
      loading: false,
      isAllSelect: false
    }
  },
  computed: {
    operateWidth() {
      return 5 * 46
    },
    variables() {
      return variables
    },
    delDisabled() {
      // console.log(this.$refs.tree.getCheckedKeys())
      return false
    }
  },
  async created() {
    // 列表展示项的查询
    const resp = await listAPI.getSomeRecords({
      keyWords: 'SysViewTableConfig',
      searchKey: { tableName: this.keyWords },
      sort: { properties: 'id', direction: 'ASC' }
    })
    this.tableColumns = resp.data.content
    this.tableColumns.forEach(e => {
      this.$set(e, 'visible', true)
    })
    this.tableColumnItem = this.tableColumns.filter(e => e.visible)
    this.initDataTree()
  },
  mounted() {
    console.log(this.selectedKeys)
  },
  methods: {
    async initDataTree() {
      try {
        const res = await treeAPI.getAllNodes({
          keyWords: this.keyWords,
          virtualRootFlag: false
        })
        this.treeTableData = res.data
      } catch (error) { error }
    },
    checkSelected(node, data) {
      this.checkedNodes = data.checkedNodes
    },
    // 增加树节点
    append(data) {
      this.$emit('create', data)
    },
    // 删除树节点
    remove(id) {
      const ids = id || this.$refs.tree.getCheckedKeys()
      this.$confirm('此操作将删除该数据, 是否继续?', '提示', {
        confirmButtonText: '确定',
        cancelButtonText: '取消'
      })
        .then(async _ => {
          await listAPI.delOneOrManyNodes(this.keyWords, ids)
          this.$message.success('删除成功！')
          this.initDataTree()
        })
        .catch((error) => error)
    },
    // 部分导出
    async exportData() {
      if (this.selectedColumns.length === 0) {
        return this.$message.warning('请先选择数据')
      }
      const { name } = getTemplateFile(this.keyWords, '.xls')
      this.fullScreenLoading()
      try {
        const params = { tblName: this.keyWords, nodes: this.selectedColumns }
        const content = await IEAPI.exportInfo('treeExport', params)
        this.downloadFile(content, name)
        this.fullScreenLoading().close()
        this.$refs.dataTree.clearSelection()
      } catch (error) {
        this.fullScreenLoading().close()
      }
    },
    // 全部导出
    async exportAllData() {
      const { name } = getTemplateFile(this.keyWords, '.xls')
      this.fullScreenLoading()
      try {
        const content = await IEAPI.exportTreeInfo(this.keyWords)
        this.downloadFile(content, name)
        this.fullScreenLoading().close()
        this.$refs.dataTree.clearSelection()
      } catch (error) {
        this.fullScreenLoading().close()
      }
    },
    // 监听选中的事件
    handleSelection(selection, row) {
      if (selection.indexOf(row) !== -1) {
        this.selectChange(selection, row, true, true)
      } else {
        this.selectChange(selection, row, false, true)
      }
      this.$emit('update:selectedColumns', selection)
    },
    // 监听选中的事件
    handleSelectionChange(selection) {
      // 将不同页面的selectedColumns整合到一起返回给父组件
      this.$emit('update:selectedColumns', selection)
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
    // 新增
    // async append(data) {
    //   this.$emit('create', data.row)
    // },
    // 编辑
    async edit(data) {
      this.$emit('update', data)
    },
    async up(node) {
      const resp = await treeAPI.getAllBrotherIndex(this.keyWords, node.row.id)
      const children = resp.data
      const index = children.map(e => e.id).indexOf(node.row.id)
      if (index === 0) {
        this.$message.warning('当前节点是第一个节点，无法上移！')
      } else {
        await treeAPI.changeTwoNodes(
          this.keyWords,
          children[index].id,
          children[index - 1].id
        )
        this.initDataTree()
        this.$message.success('移动成功！')
      }
    },
    async down(node) {
      const resp = await treeAPI.getAllBrotherIndex(this.keyWords, node.row.id)
      const children = resp.data
      const index = children.map(e => e.id).indexOf(node.row.id) // 当前节点的序号
      if (index === children.length - 1) {
        this.$message.warning('当前节点是最后一个节点，无法下移！')
      } else {
        await treeAPI.changeTwoNodes(
          this.keyWords,
          children[index].id,
          children[index + 1].id
        )
        // 移动成功，需要重新渲染
        this.initDataTree()
        this.$message.success('移动成功！')
      }
    },
    // 监听点击某一行事件
    handleNodeClick(val) {
      this.$emit('node', val)
    },

    // 全选
    handleCheckAllChange(val) {
      this.checkAll = true
      this.tableColumns.forEach(e => {
        this.$set(e, 'visible', true)
      })
      this.tableColumnItem = this.tableColumns.filter(e => e.visible)
      this.indeterminate = false
      this.checkAll = true
    },
    // 单个框的选择
    handleCheckSingleChange(item) {
      this.tableColumnItem = this.tableColumns.filter(e => e.visible)
      if (
        this.tableColumnItem.length &&
        this.tableColumnItem.length === this.tableColumns.length
      ) {
        this.checkAll = true
        this.indeterminate = false
      } else if (
        this.tableColumnItem.length &&
        this.tableColumnItem.length < this.tableColumns.length
      ) {
        this.indeterminate = true
      } else {
        item.visible = true
        this.tableColumnItem = this.tableColumns.filter(e => e.visible)
        this.$message.warning('至少选择一项展示')
      }
    }
  }
}
</script>
<style scoped lang="scss">
::v-deep .el-card__body {
  padding: 0;
}

.crud-opts {
  display: -webkit-flex;
  display: flex;
  align-items: center;
  justify-content: space-between;
  margin-bottom: 10px;
}

.el-tree {
  overflow-y: auto;
}

::v-deep .el-tree-node__content,
.custom-tree-node {
  height: 46px;
  line-height: 46px;
  align-items: center;
  border-bottom: 1px solid #ebeef5;
}
.custom-tree-node {
  border: unset;
}
</style>
