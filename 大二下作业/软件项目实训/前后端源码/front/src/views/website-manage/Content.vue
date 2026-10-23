<template>
  <BaseTreeList ref="baseTreeList" :default-props="defaultProps" @append-click="appendClick" @edit-click="editClick" @set-type="setTypeId">
    <template slot="dlg">
      <DlgEditContent ref="dlgContent" :form="form" @update-record="initDataList" />
    </template>
  </BaseTreeList>
</template>
<script>
import DlgEditContent from './components/DlgEditContent'
import BaseTreeList from '@/views/master-page/BaseTreeList'
import CONSTANT from '@/constant'
import list from '@/api/list'
export default {
  name: 'User',
  components: { BaseTreeList, DlgEditContent },
  props: {

  },
  data() {
    return {
      defaultProps: {
        treeRelColName: 'channelTypeId',
        defaultDTLProps: {
          title: { mainTitle: '内容列表' },
          someFlags: {
            noAdvancedSearch: false
          },
          searchItems: [
            { name: '标题', field: 'channelCName', type: 'input' }
          ],
          defaultDTHProps: {
            keyWord: { view: 'MainChannelContent', edit: 'MainChannelContent' },
            allTableColumns: { },
            buttonProps: { update: { show: true }, create: { show: true }, delete: { show: true }, export: { show: false }, search: { show: true }, batchCreate: { show: false }}
          }
        },
        defaultDTProps: {
          keyWord: 'BaseChannelType',
          title: { mainTitle: '栏目类别列表' }
        }
      },
      regKey: { roleIds: CONSTANT.SEARCH_OPERATOR.NOT_IN },
      typeId: null,
      form: {}
    }
  },
  created() {
    this.allTableColumns = this.defaultProps.defaultDTLProps.defaultDTHProps.allTableColumns
    Object.assign(this.allTableColumns, [
      { id: 1, showName: '标题', theOrder: 1, tableColumnName: 'channelCName' },
      { id: 2, showName: '发布时间', theOrder: 2, tableColumnName: 'channelCTime', sortable: true },
      { id: 3, showName: '状态', theOrder: 3, tableColumnName: 'publishStatus' },
      { id: 4, showName: '阅读人数', theOrder: 4, tableColumnName: 'hitTimes', sortable: true }
    ])
  },
  methods: {
    async initDataList() {
      this.$refs.baseTreeList.initDataList()
    },
    appendClick() {
      if (!this.typeId) {
        this.$message.error('请先选择具体栏目！')
        return
      }
      this.form = { id: null, workId: null, name: '', channelTypeId: this.typeId, fileList: [] }
      this.$refs.dlgContent.showDialog(true, this.form)
    },
    async editClick(val) {
      this.form = { ...val }
      // 获取图片
      const imgs = await list.getSomeRecords({ keyWords: 'SysOssFile', searchKey: { relationId: val.id, type: 1 }})
      this.$set(this.form, 'images', imgs.data.content)
      // 获取附件
      const files = await list.getSomeRecords({ keyWords: 'SysOssFile', searchKey: { relationId: val.id, type: 2 }})
      this.$set(this.form, 'files', files.data.content)
      this.$refs.dlgContent.showDialog(true, this.form)
    },
    setTypeId(val) {
      this.typeId = val
    }
  }
}
</script>
