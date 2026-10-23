<template>
  <BaseTreeList ref="baseTreeList" :default-props="defaultProps" @append-click="appendClick" @edit-click="editClick">
    <template slot="dlg">
      <DlgUserInfo ref="dlgUser" @update-record="initDataList" />
    </template>
  </BaseTreeList>
</template>
<script>

import BaseTreeList from '@/views/master-page/BaseTreeList'
import DlgUserInfo from '@/views/dialogs/DlgUserInfo'
import CONSTANT from '@/constant'

export default {
  name: 'User',
  components: { BaseTreeList, DlgUserInfo },
  props: {

  },
  data() {
    return {
      defaultProps: {
        treeRelColName: 'departmentId',
        defaultDTLProps: {
          title: { mainTitle: '人员列表' },
          someFlags: {
            noAdvancedSearch: false
          },
          searchItems: [
            { name: '姓名', field: 'name', type: 'input' },
            { name: '手机号', field: 'phone', type: 'input' }
          ],
          defaultDTHProps: {
            keyWord: { edit: 'BaseUser', view: 'ViewBaseUser' },
            allTableColumns: { },
            buttonProps: { update: { show: true }, create: { show: false }, delete: { show: true }, export: { show: false }, search: { show: true }, batchCreate: { show: false }}
          }
        },
        defaultDTProps: {
          keyWord: 'BaseDepartment',
          title: { mainTitle: '单位部门列表' }
        }
      },
      regKey: { roleIds: CONSTANT.SEARCH_OPERATOR.NOT_IN }
    }
  },
  created() {
    this.allTableColumns = this.defaultProps.defaultDTLProps.defaultDTHProps.allTableColumns
    Object.assign(this.allTableColumns, [
      { id: 1, showName: '单位', theOrder: 1, tableColumnName: 'departmentName' },
      { id: 2, showName: '姓名', theOrder: 2, tableColumnName: 'name', sortable: true },
      { id: 3, showName: '职务', theOrder: 3, tableColumnName: 'jobName' },
      { id: 4, showName: '手机号', theOrder: 4, tableColumnName: 'phone', sortable: true }
    ])
  },
  methods: {
    async initDataList() {
      this.$refs.baseTreeList.initDataList()
    },
    appendClick() {
      this.form = { id: null, workId: null, name: '' }
      this.$refs.dlgUser.showDialog(true, this.form)
    },
    editClick(val) {
      this.form = { ...val }
      this.$refs.dlgUser.showDialog(true, this.form)
    }
  }
}
</script>
