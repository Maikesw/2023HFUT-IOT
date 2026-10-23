<template>
  <div>
    <!-- :simpledialog-confirm-more="confirmMore" -->
    <SimpleDialog ref="simpleDlg" :default-props="defaultMainSDProps" :simpledialog-confirm="confirm" @update-record="updateRecord">
      <template slot="otherItems">
        <el-form-item label="角色"><SimpleSelect ref="spsRole" v-model="form.roleIds" multiple key-words="SysRole" @update-value="onSelRoles" /></el-form-item>
        <el-form-item v-if="isContestAdmin" label="竞赛管理员"><SimpleSelect ref="spsContestType1" v-model="form.contestType1" multiple key-words="BaseContestType" :search-key="contestSearchKeys" :auto-select="false" @update-value="onSelContestType1" /></el-form-item>
        <el-form-item v-if="isUniversityAdmin" label="校级管理员"><SimpleSelect ref="spsContestType2" v-model="form.contestType2" multiple key-words="BaseContestType" :search-key="universitySearchKeys" :auto-select="false" @update-value="onSelContestType2" /></el-form-item>
        <el-form-item v-if="isExpert" label="专家"><SimpleSelect ref="spsContestType3" v-model="form.contestType3" multiple key-words="BaseContestType" :search-key="expertSearchKeys" :auto-select="false" @update-value="onSelContestType3" /></el-form-item>
      </template>
    </SimpleDialog>
  </div>
</template>

<script>

import SimpleDialog from '@/components/SimpleDialog'
import SimpleSelect from '@/components/SimpleSelect'
import userAPI from '@/api/user.js'
import listAPI from '@/api/list'
import CONSTANT from '@/constant'
import _ from 'lodash'
export default {
  components: { SimpleDialog, SimpleSelect },
  props: {

  },
  data() {
    return {
      defaultMainSDProps: {
        form: {},
        keyWord: 'BaseUser',
        formItems: [
          { name: '账号', field: 'account', placeholder: '账号不能为空', type: 'input' },
          { name: '密码', field: 'password', placeholder: '密码不能为空', type: 'password' },
          { name: '身份证号', field: 'idCard', placeholder: '身份证号', type: 'input' },
          { name: '姓名', field: 'name', placeholder: '姓名不能为空', type: 'input' },
          { name: '性别', field: 'sex', type: 'select_noremote', options: [{ id: '男', name: '男' }, { id: '女', name: '女' }] },
          { name: '单位部门', field: 'departmentId', keyWords: 'BaseDepartment', type: 'cascader' },
          { name: '身份类型', field: 'jobId', keyWords: 'BaseJobPosition', type: 'select' },
          { name: '手机号', field: 'phone', placeholder: '手机号不能为空', type: 'input' },
          { name: '备注', field: 'remarks', type: 'textarea' }
        ],
        formRules: {
          account: [{ required: true, message: '账号不能为空', trigger: 'blur' }],
          // password: [{ required: true, message: '密码不能为空', trigger: 'blur' }],
          phone: [{ required: true, message: '手机号不能为空', trigger: 'blur' }],
          name: [{ required: true, message: '姓名不能为空', trigger: 'blur' }]
        },
        defaultDBProps: {

        }
      },
      form: {},
      contestSearchKeys: { theLevel: 2 },
      universitySearchKeys: { theLevel: 2 },
      expertSearchKeys: { theLevel: 2 },
      isContestAdmin: false,
      isUniversityAdmin: false,
      isExpert: false
    }
  },
  methods: {
    async showDialog(val, form = {}) {
      this.form = _.cloneDeep(form) // 打开的时候用cloneDeep，这样外层传的时候就直接传row就可以了
      await this.initSomeValues(this.form.id)
      this.$refs.simpleDlg.showDialog(val, this.form, true)
    },
    async getContestRelation(userId) {
      // 根据角色情况初始化另外三个竞赛关联
      if (this.form.roleIds.indexOf(CONSTANT.ROLE_TABLE.CONTEST_ADMIN) !== -1) {
        this.isContestAdmin = true
        const res2 = await listAPI.getSomeRecords({
          keyWords: 'RelUserContestType',
          searchKey: { userId: userId, roleId: CONSTANT.ROLE_TABLE.CONTEST_ADMIN }
        })
        if (res2.data.content.length > 0) {
          this.$set(this.form, 'contestType1', res2.data.content.map(item => item.typeId))
        } else {
          this.$set(this.form, 'contestType1', null)
        }
      } else {
        this.isContestAdmin = false
      }
      if (this.form.roleIds.indexOf(CONSTANT.ROLE_TABLE.UNIVERSITY_ADMIN) !== -1) {
        this.isUniversityAdmin = true
        const res2 = await listAPI.getSomeRecords({
          keyWords: 'RelUserContestType',
          searchKey: { userId: userId, roleId: CONSTANT.ROLE_TABLE.UNIVERSITY_ADMIN }
        })
        if (res2.data.content.length > 0) {
          this.$set(this.form, 'contestType2', res2.data.content.map(item => item.typeId))
        } else {
          this.$set(this.form, 'contestType2', null)
        }
      } else {
        this.isUniversityAdmin = false
      }
      if (this.form.roleIds.indexOf(CONSTANT.ROLE_TABLE.EXPERT) !== -1) {
        this.isExpert = true
        const res2 = await listAPI.getSomeRecords({
          keyWords: 'RelUserContestType',
          searchKey: { userId: userId, roleId: CONSTANT.ROLE_TABLE.EXPERT }
        })
        if (res2.data.content.length > 0) {
          this.$set(this.form, 'contestType3', res2.data.content.map(item => item.typeId))
        } else {
          this.$set(this.form, 'contestType3', null)
        }
      } else {
        this.isExpert = false
      }
    },
    async initSomeValues(userId) {
      if (userId) {
        const res1 = await userAPI.getUserRoles(userId)
        this.$set(this.form, 'roleIds', res1.data.map(item => item.id))
        await this.getContestRelation(userId)
      }
      // 重新克隆oldData
      // this.$refs.simpleDlg.reCloneOldData()
    },
    async updateRecord() {
      this.$emit('update-record')
    },
    async confirm(option, type, form) {
      await userAPI.editUserInfo(form.id, form)
      await this.confirmMore(form)
    },
    async confirmMore(data) {
      await userAPI.saveUserRoles(data.id, this.form.roleIds)
      if (Object.prototype.hasOwnProperty.call(this.form, 'contestType1') && this.form.contestType1 !== undefined) {
        await userAPI.saveUserContest(data.id, CONSTANT.ROLE_TABLE.CONTEST_ADMIN, this.form.contestType1)
      }
      if (Object.prototype.hasOwnProperty.call(this.form, 'contestType2') && this.form.contestType2 !== undefined) {
        await userAPI.saveUserContest(data.id, CONSTANT.ROLE_TABLE.UNIVERSITY_ADMIN, this.form.contestType2)
      }
      if (Object.prototype.hasOwnProperty.call(this.form, 'contestType3') && this.form.contestType3 !== undefined) {
        await userAPI.saveUserContest(data.id, CONSTANT.ROLE_TABLE.EXPERT, this.form.contestType3)
      }
    },

    onSelRoles(val) {
      this.$set(this.form, 'roleIds', val)
      this.getContestRelation(this.form.id)
    },
    async onSelContestType1(val) {
      this.form.contestType1 = val
    },
    async onSelContestType2(val) {
      this.form.contestType2 = val
    },
    async onSelContestType3(val) {
      this.form.contestType3 = val
    }
  }
}
</script>
