<template>
  <div>
    <tree-list>
      <template #aside>
        <DataTree :default-props="defaultDTProps" @node-click="handleNodeClick" />
      </template>
      <template #main>
        <!-- 列表1 -->
        <DataTableList ref="dataTableList" :default-props="defaultDTLProps" @edit-click="editClick" @append-click="appendClick" />
      </template>
    </tree-list>
    <slot name="dlg">
      <!-- 简单窗口 -->
      <SimpleDialog :default-props="defaultSDProps" />
    </slot>
  </div>
</template>

<script>
import DataTree from '@/components/DataTree'
import DataTableList from '@/components/DataTableList'
import SimpleDialog from '@/components/SimpleDialog'
import { customize } from '@/utils/common'
import _ from 'lodash'
export default {
  name: 'BaseTreeList',
  components: { DataTableList, DataTree, SimpleDialog },
  props: {
    defaultProps: {
      type: Object,
      default: () => {
        return {
          keyWord: { },
          treeRelColName: '',
          defaultDTLProps: {
            title: {},
            defaultDTHProps: {
              tableColumns: { }
            }
          },
          defaultSDProps: {
            formItems: [],
            formRules: {},
            defaultDBProps: {

            }
          }
        }
      }
    }
  },
  data() {
    return {
      defaultDTProps: {},
      defaultDTLProps: {
        treeInfo: { }
        // searchItems: []
      },
      defaultSDProps: {},
      treeInfo: {},
      thisEvents: ''
    }
  },
  computed: {
    // calRegKey: {
    //   get: function() {
    //     return this.regKey
    //   },
    //   set: function() {

    //   }
    // }
  },
  created() {
    _.mergeWith(this.defaultDTProps, this.defaultProps.defaultDTProps, customize)
    _.mergeWith(this.defaultDTLProps, this.defaultProps.defaultDTLProps, customize)
    _.mergeWith(this.defaultSDProps, this.defaultProps.defaultSDProps, customize)
    this.treeInfo = this.defaultDTLProps.treeInfo
  },
  mounted() {
    this.thisEvents = JSON.parse(JSON.stringify(this._events))
  },
  methods: {
    async initDataList() {
      this.$refs.dataTableList.initDataList()
    },
    handleNodeClick(val) {
      Object.assign(this.treeInfo, { treeKeyWords: this.defaultDTProps.keyWord, treeNodeId: val.id, treeRelColName: this.defaultProps.treeRelColName })
      this.$emit('set-type', val.id)
      this.initDataList()
    },
    appendClick() {
      if (Object.prototype.hasOwnProperty.call(this.thisEvents, 'append-click')) {
        this.$emit('append-click')
      } else {
        // this.form = resetForm(this.form)
      }
    },
    editClick(row) {
      if (Object.prototype.hasOwnProperty.call(this.thisEvents, 'edit-click')) {
        this.$emit('edit-click', row)
      } else {
        // this.form = _.cloneDeep(row)
      }
    },
    searchClick(val) {
      this.regKey = { name: '≈', ...this.regKey }
      this.searchKey = { name: val, ...this.searchKey }
      // 父子组件传递数据不及时的问题
      setTimeout(() => {
        this.$refs.dataTableList.initDataList()
      }, 500)
    }
  }
}
</script>
