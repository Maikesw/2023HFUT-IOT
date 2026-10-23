<template>
  <div>
    <!--搜索框-->
    <slot v-if="showSearchPanel" name="searchPanel" />
    <!--上方按钮插槽-->
    <div v-if="showTopButtons" class="crud-opts">
      <el-row type="flex" justify="space-between">
        <!--左侧插槽-->
        <slot name="left" />
        <el-button v-if="button.create.show" size="mini" :type="button.create.type" icon="el-icon-plus" @click="$emit('append-click')">{{ button.create.name }}</el-button>
        <el-button v-if="button.update.show" size="mini" :type="button.update.type" icon="el-icon-edit" :disabled="selectedColumns.length != 1" @click="edit(selectedColumns[0])">{{ button.update.name }}</el-button>
        <el-button v-if="button.delete.show" :type="button.delete.type" icon="el-icon-delete" size="mini" :disabled="selectedColumns.length < 1" @click="remove(selectedColumns)">{{ button.delete.name }}</el-button>
        <el-tooltip class="item" effect="dark" content="直接点击会导出当前全部内容，否则请先选择需要导出的项目后再点击。" placement="top">
          <el-button v-if="button.export.show" size="mini" :type="button.export.type" icon="el-icon-upload" @click="exportData">{{ button.export.name }}</el-button>
        </el-tooltip>
        <el-button v-if="button.more1.show" size="mini" :type="button.more1.type" icon="el-icon-circle-plus-outline" @click="more1Click(selectedColumns)">{{ button.more1.name }}</el-button>
        <el-button v-if="button.more2.show" size="mini" :type="button.more2.type" icon="el-icon-s-promotion" @click="more2Click(selectedColumns)">{{ button.more2.name }}</el-button>
        <div v-if="button.batchCreate.show" style="padding-left:10px;padding-right:10px;">
          <el-upload ref="upload" v-model="fileList" action :file-list="fileList" :show-file-list="false" :http-request="handleUpload">
            <el-button size="small" icon="el-icon-upload" :type="button.batchCreate.type">{{ button.batchCreate.name }}</el-button>
          </el-upload>
        </div>
        <div v-if="button.audit.show" style="padding-left:10px;padding-right:10px;">
          <el-dropdown ref="auditButton" split-button size="mini" :type="button.audit.type" :disabled="selectedColumns.length < 1" @command="auditDropdownSelect" @click="auditDropdownClick(selectedColumns)">{{ auditButtonName }}
            <el-dropdown-menu slot="dropdown">
              <el-dropdown-item v-if="button.audit.showPass" :command="CONSTANT.AUDIT_STATUS.PASS">批量审核通过</el-dropdown-item>
              <el-dropdown-item v-if="button.audit.showNotPass" :command="CONSTANT.AUDIT_STATUS.NOTPASS">批量审核不通过</el-dropdown-item>
              <el-dropdown-item v-if="button.audit.showSave" :command="CONSTANT.AUDIT_STATUS.SAVE">批量打回</el-dropdown-item>
              <el-dropdown-item v-if="button.audit.showBack" :command="CONSTANT.AUDIT_STATUS.BACK">批量审核退回</el-dropdown-item>
            </el-dropdown-menu>
          </el-dropdown>
        </div>
      </el-row>
      <!--列表上方的业务模块插槽-->
      <slot name="topOperate" />
      <!--右侧-->
      <slot name="right" />
      <el-button-group v-if="button.buttonGroup.show">
        <el-button v-if="button.search.show" size="mini" icon="el-icon-search" title="搜索" @click="showSearch" />
        <el-button size="mini" icon="el-icon-refresh" title="刷新" @click="initData" />
        <el-popover placement="bottom-end" width="100" trigger="click">
          <el-button slot="reference" size="mini" title="选择性展示" icon="el-icon-s-grid" />
          <el-checkbox v-model="checkAll" :disabled="!indeterminate" :indeterminate="indeterminate" @change="handleCheckAllChange">全选</el-checkbox>
          <el-checkbox v-for="item in allTableColumns" :key="item.id" v-model="item.firstVisible" class="checkbox-item" @change="handleCheckSingleChange(item)">{{ item.showName }}</el-checkbox>
        </el-popover>
      </el-button-group>
    </div>
    <el-card shadow="never">
      <slot slot="header" name="cardTitle" />
      <slot name="body" />
    </el-card>
  </div>
</template>

<script>

import IEAPI from '@/api/importAndExport'
import { getTemplateFile, arrangeButton } from '@/utils/common'
import fileAPI from '@/api/file'
import { getTableColumns } from '@/utils/tableColumns'
import CONSTANT from '@/constant'

export default {
  name: 'DataTableHeader',
  props: {
    defaultProps: {
      type: Object,
      default: () => {
        return {
          keyWord: { edit: '', view: '' }, // 关键词，包括view和edit，默认edit
          buttonProps: { update: { show: false }}, // 控制树结构上方的按钮
          allTableColumns: [],
          searchPanel: false, // 初始是否出现搜索按钮
          showTopButtons: true // 是否显示顶部按钮}}
          // defaultProps每一项和默认值无用，写清楚方便后面查询调用
        }
      }
    },
    selectedColumns: { type: Array, default: () => [] }
  },
  data() {
    return {
      fileList: [],
      // treeTableData: [],
      tableColumnItem: [],
      // auditList: [], // 审核checkbox容器
      checkAll: true,
      indeterminate: false,
      // loading: true,
      // isAllSelect: false,
      auditButtonName: '',
      showSearchPanel: false,
      thisEvents: ''
    }
  },
  computed: {
    keyWord() {
      var key = {}
      this.$set(key, 'edit', this.defaultProps.keyWord.edit)
      if (!Object.prototype.hasOwnProperty.call(this.defaultProps.keyWord, 'view')) {
        this.$set(key, 'view', key.edit)
      } else {
        this.$set(key, 'view', this.defaultProps.keyWord.view)
      }
      return key
    },
    specialExportKey() {
      if (Object.prototype.hasOwnProperty.call(this.defaultProps, 'specialExportKey')) {
        return this.defaultProps.specialExportKey ? this.defaultProps.specialExportKey : ''
      }
      return ''
    },
    searchPanel() {
      if (Object.prototype.hasOwnProperty.call(this.defaultProps, 'searchPanel')) {
        return this.defaultProps.searchPanel
      }
      return false
    },
    showTopButtons() {
      if (Object.prototype.hasOwnProperty.call(this.defaultProps, 'showTopButtons')) {
        return this.defaultProps.showTopButtons
      }
      return true
    },
    buttonProps() {
      return this.defaultProps.buttonProps ? this.defaultProps.buttonProps : { update: { show: true }, create: { show: true }, delete: { show: true }, export: { show: true }, up: { show: true }, down: { show: true }}
    },
    allTableColumns() {
      return this.defaultProps.allTableColumns ? Object.values(this.defaultProps.allTableColumns) : getTableColumns(0)
    },
    button() {
      var btn = {
        visible: { show: false, name: '查看详情', type: 'info' },
        create: { show: false, name: '新增', type: 'primary' },
        batchCreate: { show: false, name: '导入', type: 'primary' },
        update: { show: false, name: '修改', type: 'info' },
        delete: { show: false, name: '删除', type: 'danger' },
        search: { show: false, name: '查询' },
        up: { show: false, name: '上移', type: 'warning' },
        down: { show: false, name: '下移', type: 'warning' },
        export: { show: false, name: '导出', type: 'warning' },
        // batchExport: { show: false, name: '全部导出', type:'warning' },
        more1: { show: false, name: '更多操作1', type: 'info' },
        more2: { show: false, name: '更多操作2', type: 'info' },
        audit: { show: false, name: '审核', type: 'primary', showPass: true, showNotPass: true, showSave: true, showBack: true },
        buttonGroup: { show: true }
      }
      return arrangeButton(this.buttonProps, btn)
    }
  },
  mounted() {
    this.thisEvents = JSON.parse(JSON.stringify(this._events))
  },
  async created() {
    await this.createDynamicTableColumns()

    if (this.button.audit.show) {
      if (this.button.audit.showPass) {
        this.auditDropdownSelect(CONSTANT.AUDIT_STATUS.PASS)
      } else if (this.button.audit.showNotPass) {
        this.auditDropdownSelect(CONSTANT.AUDIT_STATUS.NotPASS)
      } else if (this.button.audit.showSave) {
        this.auditDropdownSelect(CONSTANT.AUDIT_STATUS.SAVE)
      } else if (this.button.audit.showBack) {
        this.auditDropdownSelect(CONSTANT.AUDIT_STATUS.BACK)
      }
    }
    // this.$emit('init-click')
  },
  methods: {
    // #region 动态生成头部列
    async createDynamicTableColumns() {
      this.allTableColumns.forEach((e) => {
        if (!Object.prototype.hasOwnProperty.call(e, 'firstVisible')) {
          this.$set(e, 'firstVisible', true)
        }
      })
      await this.initTBLVisibles()
    },
    // 动态生成和配置列
    initTBLVisibles() {
      const obj = this.getIndexInfo()
      const index = obj.index
      if (index >= 0) {
        // 生成配置项显隐性
        this.allTableColumns.forEach((item, i) => {
          item.firstVisible = obj.arr[index].list[i]
        })
      }
      Object.assign(this.tableColumnItem, this.allTableColumns.filter(e => e.firstVisible))
      Object.assign(this.$parent.tableColumnItem, this.tableColumnItem)
    },
    // #endregion
    showSearch() {
      this.showSearchPanel = !this.showSearchPanel
      this.$emit('show-search', this.showSearchPanel)
    },
    // 刷新
    async initData() {
      this.$emit('init-click')
    },
    // 新增
    async append(row) {
      this.$emit('append-click', row)
    },
    // 修改
    async edit(row) {
      this.$emit('edit-click', row)
    },
    // 更多内容1
    async more1Click(row) {
      this.$emit('more1-click', row)
    },
    // 更多内容2
    async more2Click(row) {
      this.$emit('more2-click', row)
    },
    // 导入数据
    async handleUpload(para) {
      this.fullScreenLoading = true
      const params = {
        files: [para.file],
        tabName: this.keyWord.view,
        importFlag: true,
        userId: this.$store.getters.userInfo.id,
        relId: 0,
        initSearchWords: JSON.stringify(this.$parent.initSearchWords)
      }
      try {
        await fileAPI.upload(params)
      } catch (error) {
        this.$message.warning('文件上传失败' + error)
      }
      this.fullScreenLoading = false
      this.$emit('upload-finish')
    },
    // 审核
    audit(row) {
      this.$emit('audit-click', row)
    },
    // 审核状态选择
    auditDropdownSelect(command) {
      switch (command) {
        case CONSTANT.AUDIT_STATUS.PASS:
          this.auditButtonName = '批量审核通过'
          return
        case CONSTANT.AUDIT_STATUS.NOTPASS:
          this.auditButtonName = '批量审核不通过'
          return
        case CONSTANT.AUDIT_STATUS.SAVE:
          this.auditButtonName = '批量打回'
          return
        case CONSTANT.AUDIT_STATUS.BACK:
          this.auditButtonName = '批量审核退回'
          return
      }
    },
    // 审核按钮点击
    auditDropdownClick(row) {
      this.$emit('audit-click', row, this.auditButtonName)
    },
    // 删除
    remove(row) {
      if (row.length > 0) {
        this.$confirm('此操作将删除选中行, 是否继续?', '提示', {
          confirmButtonText: '确定',
          cancelButtonText: '取消'
        }).then(async _ => {
          this.$emit('delete-click', row)
        }).catch(error => error)
      }
    },
    // 导出数据
    async exportData() {
      if (!Object.prototype.hasOwnProperty.call(this.thisEvents, 'export-click')) {
        await this._exportData()
      } else {
        this.$emit('export-click')
      }
    },
    async _exportData() {
      this.fullScreenLoading()
      var key = this.specialExportKey
      if (key.length === 0) {
        key = this.keyWord.view
      }
      const { name } = getTemplateFile(key, '.xls')
      try {
        var content = null
        content = await IEAPI.exportInfo(key, this.selectedColumns, this.allTableColumns, this.$parent.usedSearchWords)
        this.downloadFile(content, name)
        this.fullScreenLoading().close()
      } catch (error) {
        console.log(error)
        this.fullScreenLoading().close()
      }
    },
    // #region 处理显示列的部分显示或隐藏
    // 全选
    handleCheckAllChange() {
      this.checkAll = true
      this.allTableColumns.forEach((e) => { this.$set(e, 'firstVisible', true) })
      this.indeterminate = false
      this.saveTableColumns()
    },
    // 单个框的选择
    handleCheckSingleChange(item) {
      this.tableColumnItem = this.allTableColumns.filter((e) => e.firstVisible)
      const length = this.tableColumnItem.length
      const totalLength = this.allTableColumns.length
      if (length && length === totalLength) {
        this.checkAll = true
        this.indeterminate = false
      } else if (length && length < totalLength) {
        this.indeterminate = true
        this.checkAll = true
      } else {
        item.firstVisible = true
        this.$message.warning('至少选择一项展示')
      }
      this.saveTableColumns()
    },
    saveTableColumns() {
      const obj = this.getIndexInfo()
      const index = obj.index
      // 表头项配置显隐性数组
      const visibles = this.allTableColumns.map(e => e.firstVisible)
      if (index >= 0) {
        obj.arr[index].list = visibles
      } else {
        obj.arr.push({
          keywords: obj.keywords,
          userId: this.$store.getters.userInfo.id,
          list: visibles
        })
      }
      // 将操作设置保存至本地
      localStorage.setItem(`${obj.user}`, JSON.stringify(obj.arr))
      this.tableColumnItem = { ...this.allTableColumns.filter((e) => e.firstVisible) }
      this.$parent.tableColumnItem = { ...this.tableColumnItem }
    },
    getIndexInfo() {
      const obj = { keywords: '', user: '', arr: [], index: -1 }
      // obj.keywords = this.keyWord.view
      obj.keywords = this.$parent._uid
      obj.user = 'user_' + this.$store.getters.userInfo.id
      // 查看本地是否已经存储了该用户该张表的信息
      const savedItem = JSON.parse(localStorage.getItem(`${obj.user}`))
      obj.arr = savedItem || []
      obj.index = obj.arr.findIndex(item => item.keywords === obj.keywords)
      return obj
    }
    // #endregion
  }
}
</script>
<style scoped lang="scss">
::v-deep .el-card__body {
  padding: 0;
}

.crud-opts {
  display: flex;
  align-items: center;
  justify-content: space-between;
  margin-bottom: 10px;
}
.checkbox-item:nth-last-of-type(1) {
  margin-right: 30px;
}
</style>
