<template>
  <el-dialog v-dialogDrag append-to-body :visible.sync="dialogShow" :close-on-click-modal="false" :close-on-press-escape="false" :modal-append-to-body="false" :class="autoMax ? 'view-dialog' : ''" :fullscreen="autoMax" :before-close="beforeCloseDlg" :width="width" :height="height" @opened="openFun" @close="closeDlg">
    <template #title>
      <!-- <span>{{ dialog.title + dlgSuffix }}</span> -->
      <span>{{ dlgTitle + dlgSuffix }}</span>
      <svg-icon v-if="needMaxBtn" class="fullscreen" icon-class="fullscreen" @click="clickFull" />
    </template>
    <slot ref="mainForm" name="mainForm">默认值</slot>
    <div v-if="!noFooter" slot="footer" class="dialog-footer">
      <slot name="otherBtn" />
      <el-button v-if="buttonProps.cancel.show" size="small" :type="buttonProps.cancel.type" @click="beforeCloseDlg">{{ buttonProps.cancel.name }}</el-button>
      <el-button v-if="buttonProps.close.show" size="small" :type="buttonProps.close.type" @click="beforeCloseDlg">{{ buttonProps.close.name }}</el-button>
      <el-button v-if="buttonProps.confirm.show" :disabled="needValidate && validate" :type="buttonProps.confirm.type" size="small" :loading="buttonLoading.confirm" @click.native.prevent="onConfirm">{{ buttonProps.confirm.name }}</el-button>
      <el-button v-if="buttonProps.last.show" :type="buttonProps.last.type" size="small" :loading="buttonLoading.last" @click.native.prevent="onModalLast">{{ buttonProps.last.name }}</el-button>
      <el-button v-if="buttonProps.next.show" :type="buttonProps.next.type" size="small" :loading="buttonLoading.next" @click.native.prevent="onModalNext">{{ buttonProps.next.name }}</el-button>
      <el-button v-if="buttonProps.finish.show" :type="buttonProps.finish.type" size="small" :loading="buttonLoading.finish" @click="beforeCloseDlg">{{ buttonProps.finish.name }}</el-button>
      <el-button v-if="buttonProps.submit.show" :disabled="needValidate && validate" :type="buttonProps.submit.type" size="small" :loading="buttonLoading.submit" @click.native.prevent="onModalSubmit">{{ buttonProps.submit.name }}</el-button>
      <el-button v-if="buttonProps.repeatAdd.show && option === 'append'" :disabled="needValidate && validate" :type="buttonProps.repeatAdd.type" size="small" :loading="buttonLoading.repeatAdd" @click.native.prevent="onModalRepeatAdd">{{ buttonProps.repeatAdd.name }}</el-button>
    </div>
  </el-dialog>
</template>
<script>

import { arrangeButton } from '@/utils/common'
import _ from 'lodash'
export default {
  name: 'DlgBasic',
  props: {
    defaultProps: {
      type: Object,
      default: () => {
        return {
          form: { }, // 基本form
          width: '40%', // 宽度
          height: '', // 高度
          dlgTitle: '',
          dlgSuffix: '', // 窗口标题后缀
          footButtons: { }, // 底部按钮情况
          someFlags: {
            noFooter: false, // 没有底部按钮
            autoMax: false, // 是否自动最大化
            needMaxBtn: false, // 是否需要最大化按钮
            needValidate: true,
            validate: true,
            needVerifyUpdate: true
          }
        }
      }
    },
    dlgbasicSpecConfirm: { type: Function, default: null },
    dlgbasicConfirm: { type: Function, default: null },
    dlgbasicRepeatAdd: { type: Function, default: null },
    dlgbasicSpecSubmit: { type: Function, default: null },
    dlgbasicSubmit: { type: Function, default: null }

  },
  data() {
    return {
      form: {},
      thisEvents: '',
      continueFlag: false,
      currentSave: false, // 当前是否有数据修改，决定是否保存更新
      dialogShow: false, // 当前窗口的显示隐藏属性
      validate: true,
      buttonLoading: { confirm: false, submit: false, repeatAdd: false, last: false, next: false, finish: false },
      oldData: Object,
      type: ''
    }
  },
  computed: {
    dialog() {
      return this.defaultProps.dialog ? this.defaultProps.dialog : { }
    },
    dlgTitle() {
      if (this.defaultProps.dlgTitle) {
        return this.defaultProps.dlgTitle
      } else {
        if (Object.prototype.hasOwnProperty.call(this.form, 'id') && this.form.id != null && this.form.id !== 0) {
          return '编辑/查看'
        } else {
          return '新增'
        }
      }
    },
    option() { // 目前三种类型，edit append audit
      if (this.defaultProps.option) {
        return this.defaultProps.option
      } else {
        if (Object.prototype.hasOwnProperty.call(this.form, 'id') && this.form.id != null && this.form.id !== 0) {
          return 'edit'
        } else {
          return 'append'
        }
      }
    },
    width() {
      return this.defaultProps.width ? this.defaultProps.width : '40%'
    },
    height() {
      return this.defaultProps.height ? this.defaultProps.height : ''
    },
    dlgSuffix() {
      return this.defaultProps.dlgSuffix ? this.defaultProps.dlgSuffix : ''
    },
    footButtons() {
      return this.defaultProps.footButtons ? this.defaultProps.footButtons : {}
    },
    buttonProps: {
      get() {
        var btn = {
          cancel: { show: true, name: '取 消', type: '' },
          close: { show: false, name: '关 闭', type: 'info' },
          confirm: { show: true, name: '保 存', type: 'primary' },
          submit: { show: false, name: '提 交', type: 'success' },
          last: { show: false, name: '上一步', type: 'primary' },
          next: { show: false, name: '下一步', type: 'success' },
          finish: { show: false, name: '完成', type: 'danger' },
          repeatAdd: { show: true, name: '继续添加', type: 'warning' }
        }
        return arrangeButton(this.footButtons, btn)
      },
      set() {}
    },
    noFooter() {
      if (Object.prototype.hasOwnProperty.call(this.defaultProps, 'someFlags')) {
        if (Object.prototype.hasOwnProperty.call(this.defaultProps.someFlags, 'noFooter')) {
          return this.defaultProps.someFlags.noFooter
        }
      }
      return false
    },
    autoMax: {
      get(val) {
        if (Object.prototype.hasOwnProperty.call(this.defaultProps, 'someFlags')) {
          if (Object.prototype.hasOwnProperty.call(this.defaultProps.someFlags, 'autoMax')) {
            return this.defaultProps.someFlags.autoMax
          }
        }
        return false
      },
      set(val) {
        this.$set(this.defaultProps.someFlags, 'autoMax', val)
      }
    },
    needMaxBtn() {
      if (Object.prototype.hasOwnProperty.call(this.defaultProps, 'someFlags')) {
        if (Object.prototype.hasOwnProperty.call(this.defaultProps.someFlags, 'needMaxBtn')) {
          return this.defaultProps.someFlags.needMaxBtn
        }
      }
      return false
    },
    needValidate() {
      if (Object.prototype.hasOwnProperty.call(this.defaultProps, 'someFlags')) {
        if (Object.prototype.hasOwnProperty.call(this.defaultProps.someFlags, 'needValidate')) {
          return this.defaultProps.someFlags.needValidate
        }
      }
      return true
    },
    // validate: {
    //   get() {
    //     if (this.defaultProps.hasOwnProperty('someFlags')) {
    //       if (this.defaultProps.someFlags.hasOwnProperty('validate')) {
    //         return this.defaultProps.someFlags.validate
    //       }
    //     }
    //     return true
    //   },
    //   set() {

    //   }
    // },
    autoSaveClose() {
      if (Object.prototype.hasOwnProperty.call(this.defaultProps, 'someFlags')) {
        if (Object.prototype.hasOwnProperty.call(this.defaultProps.someFlags, 'autoSaveClose')) {
          return this.defaultProps.someFlags.autoSaveClose
        }
      }
      return true
    },
    needVerifyUpdate() {
      if (Object.prototype.hasOwnProperty.call(this.defaultProps, 'someFlags')) {
        if (Object.prototype.hasOwnProperty.call(this.defaultProps.someFlags, 'needVerifyUpdate')) {
          return this.defaultProps.someFlags.needVerifyUpdate
        }
      }
      return true
    }
  },
  watch: {
    defaultProps: {
      handler(val) {
        // this.dialog = val.dialog
        // this.form = val.form
        // this.someFlags = _.merge(this.someFlags, val.someFlags)
        // this.footButtons = val.footButtons
      },
      deep: true,
      immediate: true
    },
    form: {
      handler(val) {

      }
    }
  },
  mounted() {
    this.thisEvents = JSON.parse(JSON.stringify(this._events))
  },
  methods: {
    // #region  本view调用
    cloneOldData() {
      // this.currentSave = false
      this.oldData = _.cloneDeep(this.form)
    },
    // #endregion
    showDialog(val, form = { }) {
      this.form = form
      if (val) {
        this.currentSave = false
      }
      this.dialogShow = val
    },
    // 窗口打开
    openFun() {
      // 弹框回调
      this.cloneOldData()
      this.$emit('open-dialog', this.form)
    },

    // #region 按钮确认，可以让外层直接执行spec-confirm，也可以执行完后在外层执行confirm,stop
    async _onConfirm() {
      if (JSON.stringify(this.oldData) !== JSON.stringify(this.form)) { // 数据有修改
        this.currentSave = true
        if (this.dlgbasicConfirm && typeof this.dlgbasicConfirm === 'function') {
          this.type = 'stop'
          await this.dlgbasicConfirm(this.option, this.type)
        }
        if (this.autoSaveClose && this.type === 'stop') { // 有修改，判断一下是否关闭窗口
          this.dialogShow = false
        }
      } else { // 没有修改，永远关闭窗口
        this.dialogShow = false
      }
      // if (this.autoSaveClose && this.type=='stop') {
      //   this.dialogShow = false
      // } else {
      //   console.log(this.form)
      // }
    },
    // 确定按钮逻辑：
    // 1, 如果该窗口用于一些特殊处理，而不是简单的保存，直接外层调用spec-confirm，否则在外层直接调用_onConfirm，先判断是否有数据修改，再回到外层调用保存方法
    async onConfirm() {
      this.$set(this.buttonLoading, 'confirm', true)
      if (!(this.dlgbasicSpecConfirm && typeof this.dlgbasicSpecConfirm === 'function')) {
        await this._onConfirm()
      } else {
        this.type = 'stop'
        await this.dlgbasicSpecConfirm(this.option, this.type)
      }
      this.$set(this.buttonLoading, 'confirm', false)
    },
    // #endregion

    // #region 按钮新增，可以让外层直接执行spec-append-confirm，也可以执行完后在外层执行confirm,continue
    async _onModalRepeatAdd() {
      if (JSON.stringify(this.oldData) !== JSON.stringify(this.form)) { // 数据有修改
        this.currentSave = true
        if (this.dlgbasicConfirm && typeof this.dlgbasicConfirm === 'function') {
          this.type = 'continue'
          await this.dlgbasicConfirm(this.option, this.type)
        }
      } else {
        this.$message.warning('尚未填写任何内容，无法新增！')
      }
    },
    async onModalRepeatAdd() {
      this.$set(this.buttonLoading, 'repeatAdd', true)
      if (this.dlgbasicRepeatAdd && typeof this.dlgbasicRepeatAdd === 'function') {
        await this.dlgbasicRepeatAdd()
      } else {
        await this._onModalRepeatAdd()
      }
      this.$set(this.buttonLoading, 'repeatAdd', false)
    },
    // #endregion

    // 按钮提交，可以让外层直接执行spec-submit，也可以执行完后在外层执行submit,stop
    async onModalSubmit() {
      this.$set(this.buttonLoading, 'submit', true)
      if (this.dlgbasicSpecSubmit && typeof this.dlgbasicSpecSubmit === 'function') {
        await this.dlgbasicSpecSubmit()
        this.$set(this.buttonLoading, 'submit', false)
      } else {
        this.$confirm('提交以后不能修改，确定提交吗？', '注意')
          .then(async _ => {
            this.currentSave = true
            await this.dlgbasicSubmit()
            this.$set(this.buttonLoading, 'submit', false)
          }).catch(_ => { this.$set(this.buttonLoading, 'submit', false) })
      }
    },
    // 窗口关闭退出前的判断
    beforeCloseDlg() {
      if (this.needVerifyUpdate) {
        if (JSON.stringify(this.oldData) !== JSON.stringify(this.form)) {
          this.$confirm('数据已经修改，确认不保存退出吗？', '注意')
            .then(_ => {
              this.dialogShow = false
            })
            .catch(_ => {})
        } else {
          this.dialogShow = false
        }
      } else {
        this.dialogShow = false
      }
    },
    onModalLast() {
      this.type = 'continue'
      this.$emit('last-step', 'contine')
    },
    onModalNext() {
      this.type = 'continue'
      this.$emit('next-step', 'contine')
    },
    clickFull() {
      this.autoMax = !this.autoMax
      this.$emit('resize-dialog', this.autoMax)
    },
    closeDlg() {
      this.$emit('close-dialog', this.currentSave)
    }
  }
}
</script>

<style lang="scss" scoped>
.view-dialog {
  ::v-deep .el-dialog__body {
    max-height: unset;
    height: calc(100vh - 108px);
  }
}
</style>
