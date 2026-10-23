<template>
  <DataTableHeader ref="dataTBLMother" :selected-columns.sync="selectedColumns" :default-props="defaultProps.defaultDTHProps" @init-click="refreshInit" @show-search="showSearchPanel" @append-click="appendClick" @edit-click="editClick" @delete-click="deleteClick" @audit-click="auditClick" @export-click="exportClick" @audit-on-change="auditOnChange" @more1-click="more1Click" @more2-click="more2Click" @upload-finish="uploadFinish">
    <template slot="searchPanel">
      <!-- v-model="searchName" -->
      <slot name="searchPanel"><BtnSearch :search-name="searchName" :placeholder="searchPlaceholder" :no-advanced-search="noAdvancedSearch" :search-items="searchItems" @search-click="searchClick" @advanced-search-click="advancedSearchClick" /></slot>
    </template>
    <template slot="body">
      <!--列表展示-->
      <el-card shadow="never">
        <slot slot="header" name="cardTitle" />
        <template v-if="title" slot="header">
          <span>{{ title.mainTitle }}</span>
          <span v-if="title.subTitle">&nbsp;|&nbsp;<strong>{{ title.subTitle || "全部" }}</strong></span>
        </template>
        <el-table ref="table" v-adaptive="{bottomOffset}" v-loading="loading" border height="100%" :data="dataList" row-key="id" highlight-current-rows @current-change="handleColumnChange" @selection-change="handleSelectionChange" @sort-change="handleSortChange">
          <el-table-column v-if="checkFlag" fixed :reserve-selection="true" type="selection" width="55" />
          <el-table-column v-else fixed width="55">
            <template slot-scope="scope">
              <el-radio v-model="tableRadio" :label="scope.row"><i /></el-radio>
            </template>
          </el-table-column>
          <el-table-column v-for="(item, index) in tableColumnItem" :key="index" :show-overflow-tooltip="true" :prop="item.tableColumnName" :label="item.showName" :width="item.width" :sortable="item.sortable?'custom':false">
            <template #default="scope">
              <!-- 特殊列格式 -->
              <div v-if="item.tableColumnName.endsWith('Time')">{{ scope.row[item.tableColumnName] | filterDateTime }}</div>
              <div v-else-if="item.tableColumnName === 'isAudit'">{{ scope.row[item.tableColumnName] | filterIsAudit }}</div>
              <div v-else-if="item.tableColumnName === 'status'">{{ scope.row[item.tableColumnName] | filterTeamStatus }}</div>
              <div v-else-if="item.tableColumnName.endsWith('Status')">{{ scope.row[item.tableColumnName] | filterPublishStatus }}</div>
              <!-- 列表自定义显示的内容 tableColumnName 必须以 customize- 开头 -->
              <slot v-else-if="item.tableColumnName.startsWith('customize-')" :name="item.tableColumnName.replace('customize-','')" :row="scope.row" />
              <div v-else>{{ scope.row[item.tableColumnName] || "--" }}</div>
            </template>
          </el-table-column>
          <el-table-column v-if="operateShow" fixed="right" label="操作" :width="operateWidth">
            <template #default="scope">
              <!--查看and编辑and删除-->
              <el-button v-if="button.visible.show" :type="button.visible.type" size="mini" :title="button.visible.name" @click="view([scope.row])">
                <svg-icon icon-class="axt-view" />
              </el-button>
              <el-button v-if="button.update.show" :type="button.update.type" icon="el-icon-edit" size="mini" :title="button.update.name" @click="editClick(scope.row)" />
              <!--列表数据右方的操作按钮-->
              <slot name="rightOperate" :row="scope.row" />
              <el-button v-if="button.delete.show" :type="button.delete.type" icon="el-icon-delete" size="mini" :title="button.delete.name" @click="remove([scope.row])" />
              <el-button v-if="button.up.show" :type="button.up.type" icon="el-icon-top" size="mini" :loading="buttonLoading.up" :title="button.up.name" @click="move(scope.row, true)" />
              <el-button v-if="button.down.show" :type="button.down.type" icon="el-icon-bottom" size="mini" :loading="buttonLoading.down" :title="button.down.name" @click="move(scope.row, false)" />
              <el-button v-if="button.audit.show" :type="button.audit.type" icon="el-icon-s-check" size="mini" :title="button.audit.name" @click="auditClick([scope.row], null)" />
            </template>
          </el-table-column>
        </el-table>
        <v-page v-if="showPage" align="center" :total="totalSize" :current-page="pageInfo.page" :page-size="pageInfo.size" :page-sizes="[25, 50, 75, 100]" :selected-columns="selectedColumns" @size-change="handleSizeChange" @current-change="handleCurrentChange" />
      </el-card>
    </template>
  </DataTableHeader>
</template>

<script>

import DataTableHeader from '@/components/DataTableHeader'
import listAPI from '@/api/list'
import BtnSearch from '@/components/BtnSearch'

import VPage from '@/components/Pagination'
import moment from 'moment'
import adaptive from '@/directive/el-table'
import CONSTANT from '@/constant'
import _ from 'lodash'
export default {
  name: 'DataTableList',
  components: { VPage, DataTableHeader, BtnSearch },
  directives: { adaptive },
  filters: {
    filterDateTime(val) {
      if (!val) {
        return '--'
      } else {
        return moment(val).format('YYYY-MM-DD HH:mm')
      }
    },
    filterIsAudit(val) { // 这个地方出现的constant，需要在本文件头部import constant，不知道原因
      switch (val) {
        case CONSTANT.AUDIT_STATUS.SUBMIT: return CONSTANT.AUDIT_STATUS.SUBMITNAME
        case CONSTANT.AUDIT_STATUS.PASS: return CONSTANT.AUDIT_STATUS.PASSNAME
        case CONSTANT.AUDIT_STATUS.NOTPASS: return CONSTANT.AUDIT_STATUS.NOTPASSNAME
        case CONSTANT.AUDIT_STATUS.BACK: return CONSTANT.AUDIT_STATUS.BACKNAME
        default: return CONSTANT.AUDIT_STATUS.SAVENAME
      }
    },
    filterTeamStatus(val) {
      switch (val) {
        case 1: return '校赛'
        case 2: return '省赛初赛'
        case 3: return '省赛决赛'
      }
    },
    filterPublishStatus(val) {
      switch (val) {
        case false: return '未发布'
        case true: return '已发布'
      }
    }
  },
  props: {
    defaultProps: { // 注意这里的默认值没有用，必须在create中再设置
      type: Object,
      default: () => {
        return {
          keyWord: { },
          title: { mainTile: '', subTitle: '' },
          bottomOffset: 0,
          sortStr: { properties: 'Id', direction: 'DESC' }, // 排序方法
          treeInfo: { },
          someFlags: {
            operateShow: true, // 最右边的按钮操作面板是否出现
            checkFlag: true, // 是否出现最左边的checkBox
            showPage: true, // 是否显示底部翻页
            autoInit: true // 初始时是否显示数据
          },
          initSearchWords: { // 初始时查询的三个关键词
            searchKey: { },
            regKey: { },
            andor: { }
          },
          nowSearchWords: { // 变化查询的三个关键词
            searchKey: { },
            regKey: { },
            andor: { }
          },
          moveSearchWords: { // 用于上移下移时候的查询字
            moveSearchKey: { },
            moveRegKey: { }
          },
          searchPanel: {
            name: '',
            placeholder: ''
          },
          defaultDTHProps: {
            // defaultDTHProps中的所有对象属性
          }
        }
      }
    }
  },
  data() {
    return {
      usedSearchWords: { // 最终使用查询的三个关键词
        searchKey: { },
        regKey: { },
        andor: { }
      },
      selectedColumns: [],
      // loading: true,
      // layout: 'total, sizes, prev, pager, next, jumper',
      dataList: [],
      pageInfo: { page: 1, size: 25 },
      totalSize: 0,
      tableColumnItem: [],
      loading: false,
      buttonLoading: { up: false, down: false },
      tableRadio: null,
      // 列表中的字体颜色
      redArr: [],
      greenArr: [],
      blueArr: [],
      yellowArr: [],

      operateWidth: 60,
      thisEvents: '',
      isPageInit: false
    }
  },
  computed: {
    keyWord() {
      return this.$refs.dataTBLMother.keyWord
    },
    title() {
      return this.defaultProps.title ? this.defaultProps.title : null
    },
    button() {
      return this.$refs.dataTBLMother.button
    },
    operateShow() {
      if (Object.prototype.hasOwnProperty.call(this.defaultProps, 'someFlags')) {
        if (Object.prototype.hasOwnProperty.call(this.defaultProps.someFlags, 'operateShow')) {
          return this.defaultProps.someFlags.operateShow
        }
      }
      return true
    },
    checkFlag() {
      if (Object.prototype.hasOwnProperty.call(this.defaultProps, 'someFlags')) {
        if (Object.prototype.hasOwnProperty.call(this.defaultProps.someFlags, 'checkFlag')) {
          return this.defaultProps.someFlags.checkFlag
        }
      }
      return true
    },
    noAdvancedSearch() {
      if (Object.prototype.hasOwnProperty.call(this.defaultProps, 'someFlags')) {
        if (Object.prototype.hasOwnProperty.call(this.defaultProps.someFlags, 'noAdvancedSearch')) {
          return this.defaultProps.someFlags.noAdvancedSearch
        }
      }
      return true
    },
    showPage() {
      if (Object.prototype.hasOwnProperty.call(this.defaultProps, 'someFlags')) {
        if (Object.prototype.hasOwnProperty.call(this.defaultProps.someFlags, 'showPage')) {
          return this.defaultProps.someFlags.showPage
        }
      }
      return true
    },
    autoInit() {
      if (Object.prototype.hasOwnProperty.call(this.defaultProps, 'someFlags')) {
        if (Object.prototype.hasOwnProperty.call(this.defaultProps.someFlags, 'autoInit')) {
          return this.defaultProps.someFlags.autoInit
        }
      }
      return true
    },
    bottomOffset: {
      get: function() {
        return this.defaultProps.bottomOffset ? this.defaultProps.bottomOffset : 0
      },
      set: function() {
        this.$forceUpdate()
      }
    },
    searchItems() {
      return this.defaultProps.searchItems ? this.defaultProps.searchItems : []
    },
    sortStr() {
      return this.defaultProps.sortStr ? this.defaultProps.sortStr : { properties: 'Id', direction: 'DESC' }
    },
    initSearchWords() {
      return this.defaultProps.initSearchWords ? this.defaultProps.initSearchWords : { }
    },
    nowSearchWords: {
      get() {
        return this.defaultProps.nowSearchWords ? this.defaultProps.nowSearchWords : { }
      },
      set(val) {

      }
    },
    moveSearchWords() {
      return this.defaultProps.moveSearchWords ? this.defaultProps.moveSearchWords : { }
    },
    treeInfo() {
      return this.defaultProps.treeInfo ? this.defaultProps.treeInfo : {}
    },
    searchName() {
      if (Object.prototype.hasOwnProperty.call(this.defaultProps, 'searchPanel')) {
        return this.defaultProps.searchPanel.name ? this.defaultProps.searchPanel.name : ''
      }
      return ''
    },
    searchPlaceholder() {
      if (Object.prototype.hasOwnProperty.call(this.defaultProps, 'searchPanel')) {
        return this.defaultProps.searchPanel.placeholder ? this.defaultProps.searchPanel.placeholder : '请输入名称'
      }
      return '请输入名称'
    }
    // variables() {
    //   return variables
    // },
    // colors() {
    //   return eVariables
    // }
  },
  watch: {
    autoInit: {
      handler(val) {
        if (val) {
          setTimeout(() => { this.initDataList() }, 500)
        }
      }
    },
    nowSearchWords: {
      handler(val) {
        if (this.autoInit) {
          setTimeout(() => { this.initDataList() }, 500)
        }
      },
      deep: true,
      immediate: true
    }
  },
  mounted() {
    this.thisEvents = JSON.parse(JSON.stringify(this._events))
    var btn = this.$refs.dataTBLMother.button
    let num = 0
    const arr = [
      btn.visible.show,
      btn.delete.show,
      btn.update.show,
      btn.up.show,
      btn.down.show,
      btn.audit.show
    ]
    num = arr.reduce((acc, cur) => {
      acc = cur ? acc + 1 : acc
      return acc
    }, 0)

    var width = num * 46
    if (width < 60) {
      width = 60
    }
    this.operateWidth = width

    setTimeout(() => { this.initDataList() }, 500)
  },
  methods: {
    // #region initDataList
    getUsedSearchWords() {
      this.$set(this.usedSearchWords, 'searchKey', {})
      this.$set(this.usedSearchWords, 'regKey', {})
      this.$set(this.usedSearchWords, 'andor', {})

      if (Object.prototype.hasOwnProperty.call(this.initSearchWords, 'searchKey') && (Object.keys(this.initSearchWords.searchKey).length !== 0)) {
        this.usedSearchWords.searchKey = _.cloneDeep(this.initSearchWords.searchKey)
        if (Object.prototype.hasOwnProperty.call(this.initSearchWords, 'regKey') && (Object.keys(this.initSearchWords.regKey).length !== 0)) {
          this.usedSearchWords.regKey = _.cloneDeep(this.initSearchWords.regKey)
        }
        if (Object.prototype.hasOwnProperty.call(this.initSearchWords, 'andor') && (Object.keys(this.initSearchWords.andor).length !== 0)) {
          this.usedSearchWords.andor = _.cloneDeep(this.initSearchWords.andor)
        }
      }
      if (Object.prototype.hasOwnProperty.call(this.nowSearchWords, 'searchKey') && (Object.keys(this.nowSearchWords.searchKey).length !== 0)) {
        this.usedSearchWords.searchKey = _.merge(this.nowSearchWords.searchKey, this.initSearchWords.searchKey)
        if (Object.prototype.hasOwnProperty.call(this.nowSearchWords, 'regKey') && (Object.keys(this.nowSearchWords.regKey).length !== 0)) {
          this.usedSearchWords.regKey = _.merge(this.nowSearchWords.regKey, this.initSearchWords.regKey)
        }
        if (Object.prototype.hasOwnProperty.call(this.nowSearchWords, 'andor') && (Object.keys(this.nowSearchWords.andor).length !== 0)) {
          this.usedSearchWords.andor = _.merge(this.nowSearchWords.andor, this.initSearchWords.andor)
        }
      }
    },
    async _initDataList() {
      this.loading = true
      try {
        if (this.selectedColumns.length > 0 && !this.isPageInit) {
          this.selectedColumns = this.selectedColumns.slice(0, 0)
          this.$refs.table.clearSelection()
        }
        // 每次刷新前，都应该拼一下usedSearchWords
        this.getUsedSearchWords()
        var resp
        try {
          if (JSON.stringify(this.treeInfo) === '{}') {
            resp = await listAPI.getSomeRecords({
              keyWords: this.keyWord.view,
              pageInfo: this.pageInfo,
              treeInfo: null,
              searchKey: this.usedSearchWords.searchKey,
              sort: this.sortStr,
              reg: this.usedSearchWords.regKey,
              andor: this.usedSearchWords.andor
            })
          } else {
            resp = await listAPI.getSomeRecords({
              keyWords: this.keyWord.view,
              pageInfo: this.pageInfo,
              treeInfo: this.treeInfo,
              searchKey: this.usedSearchWords.searchKey,
              sort: this.sortStr,
              reg: this.usedSearchWords.regKey,
              andor: this.usedSearchWords.andor
            })
          }
        } catch (error) {
          console.log(error)
        }
        this.dataList = _.cloneDeep(resp.data.content)
        this.totalSize = resp.data.totalElements
        this.$emit('after-init-data', this.dataList)
        this.$emit('total-size', this.totalSize)
      } catch (error) {
        console.log(error)
        this.loading = false
      }
      this.loading = false
    },
    async initDataList(manual = false) { // manual: 手动刷新按钮
      if (this.autoInit || manual) {
        await this._initDataList()
      }
    },
    // #endregion
    showSearchPanel(flag) {
      this.$forceUpdate()
    },
    auditOnChange(value) {
      this.$emit('audit-on-change', value)
    },
    // #region 按钮相关
    // 按钮新增
    async appendClick() {
      this.$emit('append-click')
    },
    // 按钮修改
    async editClick(row) {
      this.$emit('edit-click', row)
    },
    // #region 按钮删除
    // 删除主函数
    async _deleteClick(row) {
      var ids = row.map(e => e.id)
      await listAPI.delOneOrManyNodes(this.keyWord.edit, ids)
      this._deleteSuccess(ids.size, row)
    },
    // 删除成功后的处理
    async _deleteSuccess(num, row) {
      this.$message.success('删除成功！')
      this.$emit('delete-finish-click', row)
      // 避免删除最后出现分页空白bug
      if ((this.totalSize - num) % this.pageInfo.size === 0 && this.pageInfo.page > 1) {
        this.pageInfo.page--
      }
      this.$refs.table.clearSelection()
      this.initDataList()
    },
    uploadFinish() {
      this.initDataList()
    },
    // 导出按钮
    async _exportClick() {
      await this.$refs['dataTBLMother']._exportData()
    },
    async exportClick() {
      if (Object.prototype.hasOwnProperty.call(this.thisEvents, 'export-click')) {
        this.$emit('export-click')
      } else {
        await this._exportClick()
      }
    },
    // 删除按钮
    async deleteClick(row) {
      if (Object.prototype.hasOwnProperty.call(this.thisEvents, 'delete-click')) {
        this.$emit('delete-click', row)
      } else {
        this._deleteClick(row)
      }
    },
    // 直接点删除时的弹框提示，调用上一层的函数
    remove(row) {
      if (!Object.prototype.hasOwnProperty.call(this.thisEvents, 'spec-remove')) {
        this.$refs['dataTBLMother'].remove(row)
      } else {
        this.$emit('spec-remove', row)
      }
    },
    // #endregion

    // #region 上移下移
    async _move(row, up) {
      try {
        await listAPI.changeNodeOrder(this.keyWord.edit, row.id, up, this.moveSearchWords.searchKey, this.moveSearchWords.regKey)
        await this.initDataList()
        this.$message.success('移动成功！')
        return true
      } catch (error) {
        console.log(error)
        return false
      }
    },
    // 节点上下移
    async move(row, up) {
      if (up) {
        this.$set(this.buttonLoading, 'up', true)
      } else {
        this.$set(this.buttonLoading, 'down', true)
      }
      if (!Object.prototype.hasOwnProperty.call(this.thisEvents, 'spec-move')) {
        await this._move(row, up)
      } else {
        this.$emit('spec-move', row, up)
      }
      if (up) {
        this.$set(this.buttonLoading, 'up', false)
      } else {
        this.$set(this.buttonLoading, 'down', false)
      }
    },
    // #endregion

    // #region 按钮审核
    // 审核节点
    async auditClick(row, operName) {
      if (Object.prototype.hasOwnProperty.call(this.thisEvents, 'audit-click')) {
        this.$emit('audit-click', row, operName)
      } else {
        this._audit(row, operName)
      }
    },
    _audit(row, operName) {      
      if (row.length === 1) {        
        this.$parent.openDlg('audit', row[0])
      } else {
        row.forEach(ro => {
          ro.auditUserId = this.$store.getters.userInfo.id
          ro.auditTime = new Date()
          switch (operName) {
            case '批量审核通过':
              ro.isAudit = CONSTANT.AUDIT_STATUS.PASS
              ro.reason = CONSTANT.AUDIT_STATUS.PASSNAME
              break
            case '批量审核不通过':
              ro.isAudit = CONSTANT.AUDIT_STATUS.NOTPASS
              ro.reason = CONSTANT.AUDIT_STATUS.NOTPASSNAME
              break
            case '批量打回':
              ro.isAudit = CONSTANT.AUDIT_STATUS.SAVE
              ro.reason = CONSTANT.AUDIT_STATUS.SAVENAME
              ro.auditTime = null
              break
            case '批量审核退回':
              ro.isAudit = CONSTANT.AUDIT_STATUS.BACK
              ro.reason = CONSTANT.AUDIT_STATUS.BACKNAME
              break
          }
          listAPI.editOneNode(this.keyWord.edit, ro)
        })
      }
    },
    // #endregion

    // 按钮查询
    searchClick(searchInfo) {
      var searchKey = {}
      var regKey = {}
      if (typeof this.searchName === 'string') {
        this.$set(searchKey, this.searchName, searchInfo)
        this.$set(regKey, this.searchName, CONSTANT.SEARCH_OPERATOR.LIKE)
      // } else {
      //   for (var i=0;i<this.searchName.length;i++) {
      //     this.$set(searchKey, this.searchName[i], searchInfo)
      //     this.$set(regKey, this.searchName[i], CONSTANT.SEARCH_OPERATOR.LIKE)
      //     this.$set(andor, this.searchName[i], false)
      //   }
      //   this.$set(this.nowSearchWords, 'andor', andor)
      }
      this.$set(this.nowSearchWords, 'searchKey', searchKey)
      this.$set(this.nowSearchWords, 'regKey', regKey)
      setTimeout(() => { this.initDataList() }, 500)
    },
    advancedSearchClick(searchInfo) {
      var searchKey = {}
      var regKey = {}
      const keys = Object.keys(searchInfo)
      for (var i = 0; i < keys.length; i++) {
        const item = this.searchItems.filter(item => item.field === keys[i])
        if (item.length > 0) {
          if (item[0].type === 'input') {
            this.$set(searchKey, keys[i], searchInfo[keys[i]])
            this.$set(regKey, keys[i], CONSTANT.SEARCH_OPERATOR.LIKE)
          } else if (item[0].type === 'select') {
            this.$set(searchKey, keys[i], searchInfo[keys[i]])
          } else if (item[0].type === 'date') {
            this.$set(searchKey, keys[i], { beginDate: searchInfo[keys[i]][0], endDate: searchInfo[keys[i]][1] })
            this.$set(regKey, keys[i], CONSTANT.SEARCH_OPERATOR.RANGE)
          }
        }
      }
      this.$set(this.nowSearchWords, 'searchKey', searchKey)
      this.$set(this.nowSearchWords, 'regKey', regKey)
      setTimeout(() => { this.initDataList() }, 500)
    },
    // 更多内容1
    async more1Click(row) {
      this.$emit('more1-click', row)
    },
    // 更多内容2
    async more2Click(row) {
      this.$emit('more2-click', row)
    },

    // #endregion

    // 刷新操作
    refreshInit() {
      this.$set(this.nowSearchWords, 'searchKey', {})
      this.$set(this.nowSearchWords, 'regKey', {})
      this.$set(this.nowSearchWords, 'andor', {})
      // console.log(this.initSearchWords)
      // if (!this.search) {
      //
      //   this.$emit('update:regKey', {})
      //   this.$emit('update:searchKey', {})
      //   this.$emit('update:treeInfo', {})
      //   this.$emit('update:topTreeInfo', [])
      //   this.search = false
      //   setTimeout(() => { this.initDataList() }, 500)
      // } else {
      this.initDataList()
      // }
    },
    // 自定义排序
    handleSortChange(column) {
      this.sortStr.properties = column.prop
      if (column.order === 'ascending') {
        this.sortStr.direction = 'ASC'
      } else {
        this.sortStr.direction = 'DESC'
      }
      setTimeout(() => { this.initDataList() }, 500)
    },
    // 监听点击某一行事件
    handleColumnChange(val) {
      if (!this.checkFlag) {
        this.tableRadio = val
        this.selectedColumns = [val]
      }
      this.$emit('update-column', val)
    },
    // 监听选中的事件
    handleSelectionChange(selection) {
      this.selectedColumns = selection
    },
    // 查看节点
    async view(val) {
      this.$emit('view-click', val)
    },
    // 改变页码
    async handleSizeChange(val) {
      this.pageInfo.size = val
      this.isPageInit = true
      await this.initDataList()
      this.isPageInit = false
    },
    async handleCurrentChange(val) {
      this.pageInfo.page = val
      this.isPageInit = true
      await this.initDataList()
      this.isPageInit = false
    }
  }
}
// </script>
// <style scoped lang="scss">
// @import "~@/assets/css/variables.scss";
// .primary {
//   background-color: $--color-primary;
//   border-color: $--color-primary;
//   &:hover {
//     opacity: 0.8;
//   }
// }
// ::v-deep .el-card__body {
//   padding: 0;
// }
// .curd-opts {
//   display: -webkit-flex;
//   display: flex;
//   align-items: center;
//   justify-content: space-between;
//   margin-bottom: 10px;
// }

// .color-yellow {
//   color: #ffb400;
// }
// .color-red {
//   color: #f5585f;
// }
// .color-green {
//   color: #009140;
// }
// .color-blue {
//   color: #1890ff;
// }

// .button-group {
//   display: flex;
//   justify-content: space-between;
//   width: 100%;
// }

// .checkbox-item:nth-last-of-type(1) {
//   margin-right: 30px;
// }
// </style>

