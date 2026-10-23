<template>
  <div>
    <DataTableList ref="dataTableList" :default-props="defaultProps.defaultDTLProps" @append-click="appendClick" @edit-click="editClick" @update-column="updateColumn" @delete-click="deleteClick" @audit-click="auditClick" @export-click="exportClick" @more1-click="more1Click" @more2-click="more2Click" @after-init-data="afterInitData">
      <!--数据操作按钮类 -->
    <!-- <template slot="searchPanel">
      <slot name="searchPanel" />
    </template> -->
    </DataTableList>
    <slot name="dlg">
      <!-- 简单窗口 -->
      <SimpleDialog ref="simpleDialog" :default-props="defaultProps.defaultSDProps" :simpledialog-confirm="confirm" @update-record="initDataList" @submit-more="submitMore" @simple-select-change="SimpleSelectChange" />
    </slot>
  </div>
</template>

<script>
import DataTableList from '@/components/DataTableList'
import SimpleDialog from '@/components/SimpleDialog'
import { resetForm } from '@/utils/common'

export default {
  name: 'BaseList',
  components: { DataTableList, SimpleDialog },
  props: {
    defaultProps: {
      type: Object,
      default: () => {
        return {
          keyWord: { },
          defaultDTLProps: {
            title: {},
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
            defaultDTHProps: {
              allTableColumns: { },
              buttonProps: { }
            }
          },
          defaultSDProps: {
            isAudit: false
            // defaultDBProps: {
            //   footButtons: { }
            // }
          }
        }
      }
    },
    baselistSpecConfirm: { type: Function, default: null },
    baselistConfirm: { type: Function, default: null },

    searchPlaceholder: { type: String, default: '请输入名称' }
  },
  data() {
    return {
      defaultDTLProps: {},
      defaultSDProps: {
        // form: {},
        // keyWord: '',
        //  isAudit: false,
        defaultDBProps: {
        }
      },

      form: {}, // this.defaultSDProps.form,  //关联form
      keyWord: { edit: '', view: '' }, // this.defaultProps.keyWord,

      treeInfo: {},
      searchName: '',
      thisEvents: ''
    }
  },
  computed: {

  },
  watch: {
    // defaultProps: {
    //   handler(val) {
    //     if (val.hasOwnProperty('defaultDTLProps')) {
    //       this.defaultDTLProps = _.merge(this.defaultDTLProps, this.defaultProps.defaultDTLProps)
    //     }
    //     if  (val.hasOwnProperty('defaultSDProps')) {
    //       this.defaultSDProps = _.merge(this.defaultSDProps, this.defaultProps.defaultSDProps)
    //     }
    //   },
    //   deep: true,
    //   immediate: true
    // }
  },
  created() {
    // if (this.defaultProps.hasOwnProperty('defaultSDProps')) {
    //   this.dialog = this.defaultProps.defaultSDProps.defaultDBProps.dialog //关联dialog
    // }
  },
  mounted() {
    this.thisEvents = JSON.parse(JSON.stringify(this._events))
  },
  methods: {
    async initDataList() {
      this.$refs.dataTableList.initDataList()
    },
    // 当前行的改变
    updateColumn(val) {
      this.$emit('update-column', val)
    },
    afterInitData(dataList) {
      this.$emit('after-init-data', dataList)
    },
    // #region  DataList对应按钮事件
    searchClick() {
      // this.regKey = { name: '≈' }
      // this.searchKey = { name: this.searchName }
      // // 父子组件传递数据不及时的问题
      // setTimeout(() => { initDataList() }, 500)
    },
    async appendClick() {
      if (Object.prototype.hasOwnProperty.call(this.thisEvents, 'append-click')) {
        this.$emit('append-click')
      } else {
        Object.assign(this.form, resetForm(this.form))
        this.$refs.simpleDialog.showDialog(true, this.form)
      }
    },
    async editClick(row) {
      if (!Object.prototype.hasOwnProperty.call(this.thisEvents, 'edit-click')) {
        this.openDlg('edit', row)
      } else {
        this.$emit('edit-click', row)
      }
    },
    async deleteClick(row) {
      if (Object.prototype.hasOwnProperty.call(this.thisEvents, 'delete-click')) {
        this.$emit('delete-click', row)
      } else {
        this.$refs.dataTableList._deleteClick(row)
      }
    },
    async auditClick(row, operName) {
      if (Object.prototype.hasOwnProperty.call(this.thisEvents, 'audit-click')) {
        this.$emit('audit-click', row, operName)
      } else {
        this.$refs.dataTableList._audit(row, operName)
      }
    },
    // 导出按钮
    async _exportClick() {
      await this.$refs.dataTableList._exportClick()
    },
    async exportClick() {
      if (Object.prototype.hasOwnProperty.call(this.thisEvents, 'export-click')) {
        this.$emit('export-click')
      } else {
        await this._exportClick()
      }
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

    // #region simpleDialog对应的按钮事件
    async confirm(option, type) {
      if (!(this.baselistConfirm && typeof this.baselistConfirm === 'function')) {
        await this.$refs.simpleDialog._confirm(option, type)
      } else {
        await this.baselistConfirm(option, type, this.form)
      }
    },
    submitMore(row) {
      this.$emit('submit-more', row)
    },
    openDlg(option, row) {
      if (row != null) {
        this.form = { ...row }
      } else {
        this.form = resetForm(this.form)
      }
      // if (option == null) {
      //   option = this.dialog.option
      // }
      if (option === 'edit') {
        // this.dialog = { title: '编辑', option: 'edit', show: true }
        // Object.assign(this.dialog, { title: '编辑', option: 'edit' })
      } else if (option === 'audit') {
        // Object.assign(this.dialog, { title: '审核', option: 'audit' })
      } else if (option === 'append') {
        // Object.assign(this.dialog, { title: '新增', option: 'append' })
      }
      this.$refs.simpleDialog.showDialog(true, this.form)
    },
    SimpleSelectChange(val, field, form, options) {
      this.$emit('simple-select-change', val, field, form, options)
    }
  }
  // #endregion
}
</script>
