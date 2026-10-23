<template>
  <el-dialog :close-on-modal="false" :visible="true" title="重置登录密码" append-to-body width="400px" @close="closeDialog">
    <el-form ref="pwdForm" :model="pwdForm" :rules="pwdRule">
      <el-form-item prop="password">
        <el-input v-model.trim="pwdForm.password" type="password" placeholder="请输入重置后的密码" :maxlength="CONSTANT.INFO_MAX_LENGTH" />
      </el-form-item>
      <el-form-item prop="password2">
        <el-input v-model.trim="pwdForm.password2" type="password" placeholder="请再次输入密码" :maxlength="CONSTANT.INFO_MAX_LENGTH" />
      </el-form-item>
      <el-form-item>
        <el-button style="width:100%;" type="primary" @click="handleConfirm()">确认重置</el-button>
      </el-form-item>
    </el-form>
  </el-dialog>
</template>
<script>
import userAPI from '@/api/user'
import { checkPassword, checkIsEmpty } from '@/utils/formRules'
export default {
  name: 'DlgForgetPwd',  
  props: {
    dialogVisible: { type: Boolean, default: false }
  },
  data() {
    const checkPassword2 = (rule, value, callback) => {
      if (!checkIsEmpty(value)) {
        callback(new Error('密码不能为空'))
      }
      if (value !== this.pwdForm.password) {
        callback(new Error('两次密码输入不一致，请检查后重新输入'))
      }
      callback()
    }
    return {
      pwdForm: { phone: '', verCode: '', password: '', password2: '' },
      pwdRule: {
        password: [{ required: true, validator: checkPassword, trigger: 'blur' }],
        password2: [{ required: true, validator: checkPassword2, trigger: 'blur' }]
      },      
      btnDisabled: false
    }
  },
  mounted() {
    this.$nextTick(() => {
      this.$refs['pwdForm'].resetFields()
    })
  },
  methods: {
    // 确认修改密码
    handleConfirm() {
      this.$refs.pwdForm.validate(async valid => {
        if (valid) {
          try {
            await userAPI.editPassword(this.pwdForm)
            this.$message.success('密码重置成功')
            this.$emit('update:dialogVisible', false)
          } catch (error) {
            console.log(error)
          }
        }
      })
    },
    closeDialog() {
      this.$emit('update:dialogVisible', false)
    }
  }
}
</script>
