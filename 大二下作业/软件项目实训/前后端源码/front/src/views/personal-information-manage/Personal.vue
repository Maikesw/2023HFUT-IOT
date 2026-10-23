<template>
  <div>
    <el-form ref="mainForm" :model="form" :rules="formRule" label-position="right" label-width="120px" class="personal-form" label-suffix=":">
      <el-form-item label="账号" prop="account">
        <el-input v-model="form.account" />
      </el-form-item>
      <el-form-item label="姓名" prop="name">
        <el-input v-model="form.name" />
      </el-form-item>
      <el-form-item label="性别">
        <el-radio-group v-model="form.sex">
          <el-radio label="男">男</el-radio>
          <el-radio label="女">女</el-radio>
        </el-radio-group>
      </el-form-item>
      <el-form-item label="所在学校/单位">
        <div>{{ form.schoolName || '--' }}</div>
      </el-form-item>
      <el-form-item label="所在院系/部门" prop="departmentId">
        <!-- @update-value="onSimpleSelectChange" @init-finish="simpleSelectInitFinish" -->
        <SimpleSelect v-model="form.departmentId" key-words="BaseDepartment" :search-key="searchKey" @update-value="onSimpleSelectChange"  />
      </el-form-item>
      <el-form-item label="身份类型">
        <div>{{ form.jobName || '--' }}</div>
      </el-form-item>
      <el-form-item label="身份证号">
        <el-input v-model="form.idCard" />
      </el-form-item>
      <el-form-item label="电子邮箱">
        <el-input v-model="form.email" />
      </el-form-item>
      <el-form-item label="联系地址">
        <el-input v-model="form.address" />
      </el-form-item>
      <el-form-item label="邮政编码">
        <el-input v-model="form.postalCode" />
      </el-form-item>
    </el-form>
    <div class="button-group">
      <!-- <el-button size="mini" plain round @click="$router.go(-1)">
        返 回
      </el-button> -->
      <el-button size="mini" type="primary" round :loading="loading" @click="handleSave()">
        保 存
      </el-button>
    </div>    
  </div>
</template>

<script>

import UserAPI from '@/api/user'
import SimpleSelect from '@/components/SimpleSelect'

export default {
  name: 'Personal',
  components: { SimpleSelect },
  data() {
    return {
      editable: false,
      userId: this.$store.getters.userInfo.id,
      form: JSON.parse(JSON.stringify(this.$store.getters.userInfo)),
      formRule: {
        account: [{ required: true, trigger: 'blur', message: '请输入登录账号' }],
        name: [{ required: true, trigger: 'blur', message: '请输入用户姓名' }],
        departmentId: [{ required: true, trigger: 'blur', message: '部门不能为空' }]
      },
      loading: false

    }
  },
  computed: {
    searchKey() {
      return { parentId: this.form.schoolId, theLevel: 3 }
    }
  },
  methods: {
    onSimpleSelectChange(val, field, options) {
      this.$set(this.form, 'departmentId', val)
    },
    // 保存个人信息
    async handleSave() {
      this.$refs.mainForm.validate(async valid => {
        if (valid) {
          this.loading = true
          // 信息保存
          try {
            const obj = JSON.parse(JSON.stringify(this.form))
            await UserAPI.editUserInfo(this.userId, obj)
            this.$store.dispatch('user/updateUserInfo', obj)
            this.$message.success('保存成功！')
            this.loading = false
          } catch (error) {
            this.loading = false
          }
        }
      })
    }
  }
}
</script>

<style lang="scss" scoped>
@import "~@/assets/css/element-variables.scss";
.el-form {
  width: fit-content;
  margin-left: 12%;
}
.personal-form {
  &::v-deep .el-form-item__label {
    color: #afb1b5;
  }
  &::v-deep .el-form-item__content {
    margin-left: 150px !important;
  }
}
input {
  outline: none;
}
.svg-icon {
  cursor: pointer;
  margin-left: 50px;
  color: #1890ff;
  &:hover {
    color: $--color-primary;
  }
}
.button-group {
  margin-left: 36%;
  margin-bottom: 40px;
}
</style>
