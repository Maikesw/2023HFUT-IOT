<template>
  <div>
    <!-- :search-key="searchKey"  -->
    <DataTableTree ref="DataTableTree" :default-props="defaultProps.defaultDTTProps" @edit-click="editClick" @append-click="appendClick" />
    <slot name="dlg">
      <!-- 简单窗口 -->
      <SimpleDialog ref="simpleDialog" :default-props="defaultProps.defaultSDProps" @update-record="updateDataTree" />
    </slot>
  </div>
</template>
<script>
import DataTableTree from '@/components/DataTableTree'
import SimpleDialog from '@/components/SimpleDialog'
import { resetForm } from '@/utils/common'
import _ from 'lodash'

export default {
  components: { DataTableTree, SimpleDialog },
  props: {
    defaultProps: {
      type: Object,
      default: () => {
        return {
          keyWord: { },
          defaultDTTProps: {
            title: {},
            defaultDTHProps: {
              tableColumns: { }
            }
          },
          defaultSDProps: {
            formItems: [],
            formRules: {},
            defaultDBProps: {
              dialog: { }
            }
          }
        }
      }
    }
  },
  data() {
    return {
      defaultDTTProps: {
      },
      defaultSDProps: {
        formItems: [
          { name: '完整名称', field: 'allNodeNames', type: 'input', disabled: true },
          { name: '编码', field: 'code', type: 'input' },
          { name: '名称', field: 'name', type: 'input' },
          { name: '备注', field: 'remarks', type: 'textarea' }
        ],
        formRules: { name: [{ required: true, message: '名称不能为空', trigger: 'blur' }] },
        defaultDBProps: {
          dialog: { }
        }
      },
      form: {},
      keyWord: {},
      thisEvents: ''
    }
  },
  created() {
    if (Object.prototype.hasOwnProperty.call(this.defaultProps, 'defaultSDProps')) {
      this.dialog = this.defaultProps.defaultSDProps.defaultDBProps.dialog // 关联dialog
    }
  },
  mounted() {
    this.thisEvents = JSON.parse(JSON.stringify(this._events))
  },
  methods: {
    async updateDataTree(row) {
      this.$refs.DataTableTree.updatePartTree(row)
    },
    _appendClick(data) {
      this.form.allNodeNames = data ? data.allNodeNames : '全部'
      this.form.parentId = data ? data.id : -1
      this.form.isLeaf = true
      this.form.childNum = 0
      this.$refs.simpleDialog.showDialog(true, this.form)
    },
    appendClick(data) {
      if (Object.prototype.hasOwnProperty.call(this.thisEvents, 'append-click')) {
        this.$emit('append-click', data)
      } else {
        this.form = resetForm(this.form)
        this._appendClick(data)
      }
    },
    editClick(data) {
      if (Object.prototype.hasOwnProperty.call(this.thisEvents, 'edit-click')) {
        this.$emit('edit-click', data)
      } else {
        this.form = _.cloneDeep(data)
        this.$refs.simpleDialog.showDialog(true, this.form)
      }
    }
  }
}
</script>

