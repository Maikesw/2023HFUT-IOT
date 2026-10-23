  <!-- 表单项，通过传入的 formItems 循环生成表单 -->
  <!-- 基础入参包括：
    ① *name:表单项展示名称 String
    ② *field:后端所需入参(绑定值) String
    ③ *type:表单种类 String
    ④ options:各种子类选项 Array
    ⑤ keyWords:获取下拉选项
    ⑥ selectLabel:下拉框的回显字段（针对下拉框编辑时的回显）
    ⑦ multiple:是否开启多选（针对下拉框）
  -->
<template>
  <DlgBasic ref="dlgBasic" :default-props.sync="defaultProps.defaultDBProps" :dlgbasic-confirm="confirm" :dlgbasic-submit="submit" :dlgbasic-spec-confirm="specConfirm" @close-dialog="onCloseDialog" @open-dialog="openDialog">
    <template slot="mainForm">
      <el-form ref="formPanel" :rules="formRules" :model="form" label-suffix=":" :label-width="labelWidth">
        <slot name="upItems" />
        <div v-for="(item, index) in formItems" :key="index">
          <el-form-item v-if="item.type!=='textarea'" :prop="item.field" :label="item.name">
            <!-- 简单显示 -->
            <div v-if="item.type==='label'"><span v-html="form[item.field]" /></div>
            <!-- 双列简单显示 -->
            <el-row v-else-if="item.type==='doubleLabel'">
              <el-col :span="11">
                <el-form-item :prop="item.field1" :label="item.name1"><span v-html="form[item.field1]" /></el-form-item>
              </el-col>
              <el-col :span="2" style="text-align:center">-</el-col>
              <el-col :span="11">
                <el-form-item :prop="item.field2" :label="item.name2"><span v-html="form[item.field2]" /></el-form-item>
              </el-col>
            </el-row>
            <!-- 密码框 -->
            <password v-else-if="item.type==='password'" v-model="form[item.field]" :placeholder="item.placeholder?item.placeholder:'请输入'" @input="resetPass" />
            <!-- <el-input v-else-if="item.type==='password'" type="password" v-model="form[item.field]" :placeholder="item.placeholder?item.placeholder:'请输入'" :disabled="item.disabled" /> -->
            <!-- 开关 -->
            <el-switch v-else-if="item.type==='switch'" v-model="form[item.field]" />
            <!-- 单选 -->
            <el-radio-group v-else-if="item.type==='radio'" v-model="form[item.field]">
              <el-radio v-for="(citem,cindex) in item.options" :key="cindex+'A'" :label="citem.id">{{ citem.value }}</el-radio>
            </el-radio-group>
            <!-- 多选 -->
            <el-checkbox-group v-else-if="item.type==='checkbox'" v-model="form[item.field]">
              <el-checkbox v-for="(citem, cindex) in item.options" :key="cindex+'B'" :label="citem.id">{{ citem.value }}</el-checkbox>
            </el-checkbox-group>
            <!-- 普通输入框 -->
            <el-input v-else-if="item.type==='input'" v-model="form[item.field]" :maxlength="CONSTANT.INFO_MAX_LENGTH" :placeholder="item.placeholder?item.placeholder:'请输入'" :disabled="item.disabled" />
            <!-- 双列普通输入框 -->
            <el-row v-else-if="item.type==='doubleinput'">
              <el-col :span="11">
                <el-form-item :prop="item.field1">
                  <el-input v-model="form[item.field1]" :placeholder="item.placeholder1" />
                </el-form-item>
              </el-col>
              <el-col :span="2" style="text-align:center">-</el-col>
              <el-col :span="11">
                <el-form-item :prop="item.field2">
                  <el-input v-model="form[item.field2]" :placeholder="item.placeholder2" />
                </el-form-item>
              </el-col>
            </el-row>
            <!-- 三列普通输入框 -->
            <el-row v-else-if="item.type==='tripleinput'">
              <el-col :span="7">
                <el-form-item :prop="item.field1">
                  <el-input v-model="form[item.field1]" :placeholder="item.placeholder1" />
                </el-form-item>
              </el-col>
              <el-col :span="2" style="text-align:center">-</el-col>
              <el-col :span="7">
                <el-form-item :prop="item.field2">
                  <el-input v-model="form[item.field2]" :placeholder="item.placeholder2" />
                </el-form-item>
              </el-col>
              <el-col :span="2" style="text-align:center">-</el-col>
              <el-col :span="6">
                <el-form-item :prop="item.field3">
                  <el-input v-model="form[item.field3]" :placeholder="item.placeholder3" />
                </el-form-item>
              </el-col>
            </el-row>
            <!-- 单列时间日期框 -->
            <el-date-picker v-else-if="item.type==='date'" v-model="form[item.field]" type="datetime" placeholder="选择日期时间" />
            <!-- 简单下拉选择框 -->
            <el-select v-else-if="item.type==='select_noremote'" v-model="form[item.field]" placeholder="请选择">
              <el-option v-for="(sitem,sindex) in item.options" :key="sindex" :label="sitem.name" :value="sitem.id" />
            </el-select>
            <!-- 关联数据选择框 -->
            <SimpleSelect v-else-if="item.type==='select'" :ref="'smpSel'+item.field" v-model="form[item.field]" :field="item.field" :select-label="item.selectLabel" :multiple="item.multiple" :form.sync="form" :key-words="item.keyWords" :search-key="form[item.searchKeys]" :reg-key="item.regKey" :disabled="item.disabled" :placeholder="item.placeholder?item.placeholder:'请选择'" :auto-select="item.autoSelect" @update-value="onSimpleSelectChange" @init-finish="simpleSelectInitFinish" />
            <!-- 级联选择框 -->
            <SimpleTreeSelect v-else-if="item.type==='cascader'" v-model="form[item.field]" :field="item.field" :key-words="item.keyWords" :disabled="item.disabled" :check-strictly="item.checkStrictly" :search-keys="item.searchKeys" @update-value="onTreeSelectChange" />
            <!-- 文件上传框 -->
            <SimpleUpload v-else-if="item.type==='uploadFile'" v-model="form[item.field]" :field="item.field" :up-button-info="item.upButtonInfo" :file-max-size="item.fileMaxSize" :file-allowed-types="item.fileAllowedTypes" :multiple="item.allowMultiFiles" :allow-multi-files="item.allowMultiFiles" />
            <!-- 文件下载链接 -->
            <el-button v-else-if="item.type==='downloadFile'" type="text" @click="downloadLinkClick(form[item.ossFileId], form[item.fileName])">{{ form[item.fileName] }}</el-button>
            <!-- 上传--卡片类型 -->
            <el-upload-self v-else-if="item.type==='upload-card'" v-model="fileList" :limit="1" show-limit list-type="picture-card" :file-types="item.fileTypes&&item.fileTypes.length?item.fileTypes:fileTypes" @beforeUpload="beforeUpload" @filesRemove="remove" />
            <!-- 图标选择 -->
            <el-popover v-else-if="item.type==='icon'" placement="bottom-start" width="358" trigger="click">
              <!-- @show="resetSelected" -->
              <IconSelect ref="iconSelect" @selected="selected" />
              <el-input slot="reference" v-model="form.icon" placeholder="点击选择图标" readonly>
                <svg-icon v-if="form.icon" slot="prefix" :icon-class="form.icon" class="el-input__icon" />
                <i v-else slot="prefix" class="el-icon-search el-input__icon" />
              </el-input>
            </el-popover>
          </el-form-item>
        </div>
        <!-- 其他特殊项 -->
        <slot name="otherItems" />
        <div v-for="(item, index) in formItems" :key="'area'+index">
          <el-form-item v-if="item.type==='textarea'" :prop="item.field" :label="item.name">
            <!-- 文本域输入框，始终出现在最下面 -->
            <el-input v-if="item.type==='textarea'" v-model="form[item.field]" type="textarea" :placeholder="item.placeholder||'写点什么...'" :rows="3" />
          </el-form-item>
        </div>
        <slot name="bottomItems" />
      </el-form>
    </template>
  </DlgBasic>
</template>

<script>
import _ from 'lodash'
import DlgBasic from '@/components/DlgBasic'
import SimpleSelect from '@/components/SimpleSelect'
import ElUploadSelf from '@/components/ElUploadSelf'
import dlgAPI from '@/utils/forDialog'
import SimpleTreeSelect from '@/components/SimpleTreeSelect'
import Password from '@/components/Password'
import IconSelect from '@/components/icon-select/Index'
import SimpleUpload from '@/components/SimpleUpload'
import { resetForm } from '@/utils/common'
import fileAPI from '@/api/file.js'
import userAPI from '@/api/user'
import CONSTANT from '@/constant'
export default {
  name: 'SimpleDialog',
  components: { DlgBasic, SimpleSelect, ElUploadSelf, SimpleTreeSelect, Password, IconSelect, SimpleUpload },
  props: {
    defaultProps: {
      type: Object,
      default: () => {
        return {
          labelWidth: '',
          keyWord: ' ',
          isAudit: false,
          autoSaveClose: true, // 是否根据当前form变化状态自动关闭窗口，如果存储涉及多表需要多次保存，则将其设为false
          formItems: [],
          formRules: {},
          defaultDBProps: {
            footButtons: { }
          }
        }
      }

    },
    simpledialogSpecConfirm: { type: Function, default: null },
    simpledialogConfirm: { type: Function, default: null },
    simpledialogConfirmMore: { type: Function, default: null },
    simpledialogSubmit: { type: Function, default: null }
  },
  data() {
    return {
      // options: [],
      // fileTypes: ['jpg', 'jpeg', 'png', 'JPG', 'JPEG', 'PNG'],
      // fileList: []
      form: {},
      keyWord: this.defaultProps.keyWord, // keyWord必传并且不会改变
      thisEvents: '',
      footButtons: {}
    }
  },
  computed: {
    labelWidth() {
      return this.defaultProps.labelWidth ? this.defaultProps.labelWidth : '100px'
    },
    isAudit() {
      if (Object.prototype.hasOwnProperty.call(this.defaultProps, 'isAudit')) {
        return this.defaultProps.isAudit
      }
      return false
    },
    autoSaveClose() {
      if (Object.prototype.hasOwnProperty.call(this.defaultProps, 'autoSaveClose')) {
        return this.defaultProps.autoSaveClose
      }
      return true
    },
    formItems() {
      return this.defaultProps.formItems ? this.defaultProps.formItems : [
        { name: '编码', field: 'code', type: 'input' },
        { name: '名称', field: 'name', type: 'input' },
        { name: '备注', field: 'remarks', type: 'textarea' }
      ]
    },
    formRules() {
      return this.defaultProps.formRules ? this.defaultProps.formRules : { name: [{ required: true, message: '名称不能为空', trigger: 'blur' }] }
    }
  },
  watch: {
    form: {
      handler(val, oldVal) {
        this.verifyValid()
      },
      deep: true
    }
  },
  created() {
    // this.footButtons = this.defaultProps.defaultDBProps.footButtons
  },
  mounted() {
    this.thisEvents = JSON.parse(JSON.stringify(this._events))
  },
  methods: {
    showDialog(val, form = { }, bind = false) {      
      if (form !== null) {
        if (bind) {
          this.form = form
        } else {
          this.form = _.cloneDeep(form)
        }
      }
      this.$refs.dlgBasic.showDialog(val, this.form)
      setTimeout(() => { this.$refs.formPanel.clearValidate() }, 100)
    },
    // 窗口打开
    async openDialog(row) {
      this.$emit('open-dialog', row)
    },
    async resetPass(password) {
      const phone = this.form.phone
      try {
        await userAPI.editPassword({ phone: phone, verCode: '1980', password: password })
        this.$message.success('重置完毕')
      } catch (error) {
        error()
      }
    },
    verifyValid() {
      this.$nextTick(() => {
        this.$refs.formPanel.validate((valid) => {
          this.$refs.dlgBasic.validate = !valid
        })
      })
    },
    // #region 点击确认按钮，
    // 1, 如果不想执行数据是否修改判断，直接外层spec-confirm; 2, 如果执行数据修改判断后再特殊操作，外层on-confirm；3，如果执行通用保存后外层再有操作，外层执行confirm-more
    async _confirm(option, type, form = null) {
      if (form != null) {
        this.form = { ...form }
      }
      // isTree 是否为树结构
      var parentId = null
      var allNodeNames = null
      const isTree = this.form.parentId >= -1
      if (isTree) {
        parentId = this.form.parentId
        allNodeNames = this.form ? this.form.allNodeNames : '全部'
      }
      const userId = this.$store.getters.userInfo.id
      var res = await dlgAPI.commonSubmitDlg(this.$refs.formPanel, this.form, this.keyWord, option, isTree, this.isAudit, userId)
      if (res.message === 'successful') {
        if (this.simpledialogConfirmMore && typeof this.simpledialogConfirmMore === 'function') {
          await this.simpledialogConfirmMore(res.data)
        }
        if (type === 'continue') {
          this.$refs.formPanel.resetFields()
          this.form = resetForm(this.form)
          if (parentId) {
            this.form.allNodeNames = allNodeNames
            this.form.parentId = parentId
            this.form.isLeaf = true
            this.form.childNum = 0
          }
        // } else {
        //   if (this.autoSaveClose) {
        //     this.$refs.dlgBasic.showDialog(false, this.form)
        //   }
        }
      }
    },
    async confirm(option, type) {
      if (!(this.simpledialogConfirm && typeof this.simpledialogConfirm === 'function')) {
        await this._confirm(option, type)
      } else {
        await this.simpledialogConfirm(option, type, this.form)
      }
    },
    async specConfirm(option, type) {
      if (!(this.simpledialogSpecConfirm && typeof this.simpledialogSpecConfirm === 'function')) {
        await this.$refs.dlgBasic._onConfirm()
      } else {
        await this.simpledialogSpecConfirm(option, type, this.form)
      }
    },
    // #endregion

    async onCloseDialog(saveType) {
      if (saveType === true) {
        this.$emit('update-record', this.form)
      }
      this.$emit('close-dialog')
    },
    // 获取子组件 SimpleSelect 传递的值
    recieve(val, field) {
      // this.form[field] = val
    },
    remove() {
      if (this.fileList[0].id) {
        this.delId = [this.fileList[0].id]
      }
      this.fileList = []
    },

    async submit() {
      if (!(this.simpledialogSubmit && typeof this.simpledialogSubmit === 'function')) {
        this.$set(this.form, 'isAudit', CONSTANT.AUDIT_STATUS.SUBMIT)
        await this._onConfirm('submit', 'stop')
      } else {
        await this.simpledialogSubmit(this.form)
      }
    },
    beforeUpload(file) {
      const isLt5M = file.size / 1024 / 1024 < 5
      if (!isLt5M) {
        this.$message.error('上传文件大小不能超过 5MB!')
      }
      return isLt5M
    },
    // 选中图标
    selected(name) {
      this.form.icon = name
    },
    // resetSelected() {
    //   this.$refs.iconSelect.reset()
    // },
    onSimpleSelectChange(val, field, options) {
      this.$set(this.form, field, val)
      this.$emit('simple-select-change', val, field, this.form, options)
    },
    simpleSelectInitFinish(field, options) {
      this.$emit('simple-select-init-finish', field, options)
    },
    onTreeSelectChange(val, field) {
      this.$set(this.form, field, val)
      this.$emit('tree-select-change', val, field, this.form)
    },

    // reCloneOldData() {
    //   this.$refs.dlgBasic.cloneOldData()
    // },
    async downloadLinkClick(id, fileName) {
      var content = await fileAPI.downloadFile(id)
      this.downloadFile(content, fileName)
    }
  }
}
</script>

<style lang="scss" scoped>
.coordinate {
  display: flex;
}
</style>
