  <!-- 选择窗口，里面只有一个datatable -->
<template>
  <DlgBasic ref="dlgBasic" :default-props="defaultDBProps" :dlgbasic-spec-confirm="onConfirm">
    <template slot="mainForm">
      <DataTableList ref="dataTableList" :default-props="defaultDTLProps" />
    </template>
  </DlgBasic>
</template>

<script>

import DlgBasic from '@/components/DlgBasic'
import DataTableList from '@/components/DataTableList'
import _ from 'lodash'
import { customize } from '@/utils/common'

export default {
  name: 'SelectDialog',
  components: { DlgBasic, DataTableList },
  props: {
    defaultProps: {
      type: Object,
      default: () => {
        return {
          defaultDBProps: {
            dlgSuffix: {},
            someFlags: {
              autoMax: true,
              needMaxBtn: true
            },
            footButtons: { repeatAdd: { show: false }, confirm: { name: '确定' }}
          },
          defaultDTLProps: {
            bottomOffset: 90,
            title: { },
            nowSearchWords: {
              searchKey: { },
              regKey: { },
              andor: { }
            },
            defaultDTHProps: {
              showTopButtons: false,
              keyWord: { }
            }
          }
        }
      }

    },
    selectdialogConfirm: { type: Function, default: null }
  },
  data() {
    return {
      defaultInitDBProps: {
        someFlags: {
          autoMax: true,
          needMaxBtn: true,
          needValidate: false
        },
        footButtons: { repeatAdd: { show: false }, confirm: { name: '确定' }}
      },
      defaultDBProps: {},
      defaultInitDTLProps: {
        bottomOffset: 90,
        defaultDTHProps: {
          showTopButtons: false
        }
      },
      defaultDTLProps: {},
      selectedColumns: []
      // treeInfo: {}
    }
  },
  methods: {
    showDialog(val) {
      if (val) {
        this.defaultDBProps = _.cloneDeep(this.defaultInitDBProps)
        this.defaultDTLProps = _.cloneDeep(this.defaultInitDTLProps)
        _.mergeWith(this.defaultDTLProps, this.defaultProps.defaultDTLProps, customize)
        _.mergeWith(this.defaultDBProps, this.defaultProps.defaultDBProps, customize)
      }
      this.$refs.dlgBasic.showDialog(val)
    },
    async onConfirm() {
      this.selectedColumns = this.$refs.dataTableList.selectedColumns
      if (this.selectdialogConfirm && typeof this.selectdialogConfirm === 'function') {
        await this.selectdialogConfirm(this.selectedColumns)
      }
      this.$refs.dlgBasic.showDialog(false)
    }

  }
}
</script>
