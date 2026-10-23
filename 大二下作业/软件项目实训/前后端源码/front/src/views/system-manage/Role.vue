<template>
  <div>
    <div class="box-flex">
      <div style="width: 45%">
        <BaseList :default-props="defaultBLProps" @update-column="updateColumn" />
      </div>
      <!-- 列表右侧-->
      <div style="width:53%; ">
        <el-card shadow="never">
          <div slot="header">
            <span title="选择指定角色分配菜单">角色授权 | <strong>{{ currentColumn.name || "--" }}</strong></span>
            <el-button :disabled="!currentColumn" :loading="saveLoading" size="mini" style="float: right;padding: 6px 9px;margin-top:-5px" type="primary" :title="currentColumn ? '' : '请在左边列表选择一个角色'" @click="saveRoleMenus">保存</el-button>
          </div>
          <el-table ref="dataTree" v-adaptive="{ noPage: true }" v-loading="loading" :data="treeData" height="100%" row-key="id" :expand-row-keys="['-1']">
            <el-table-column prop="name" label="名称" width="150" :show-overflow-tooltip="true" />
            <el-table-column label="查询">
              <template slot-scope="scope">
                <el-checkbox v-model="scope.row.authorizationInfo.visibleFlag" @change="handleSingleChange( scope.row.authorizationInfo, scope, 'visibleFlag' )" />
              </template>
            </el-table-column>
            <el-table-column label="添加">
              <template slot-scope="scope">
                <el-checkbox v-model="scope.row.authorizationInfo.addFlag" @change="handleSingleChange(scope.row.authorizationInfo,scope,'addFlag' )" />
              </template>
            </el-table-column>
            <el-table-column label="修改">
              <template slot-scope="scope">
                <el-checkbox v-model="scope.row.authorizationInfo.modifyFlag" @change="handleSingleChange( scope.row.authorizationInfo, scope, 'modifyFlag' )" />
              </template>
            </el-table-column>
            <el-table-column label="删除">
              <template slot-scope="scope">
                <el-checkbox v-model="scope.row.authorizationInfo.deleteFlag" @change=" handleSingleChange( scope.row.authorizationInfo, scope, 'deleteFlag' )" />
              </template>
            </el-table-column>
            <el-table-column label="全部">
              <template slot-scope="scope">
                <el-checkbox v-model="scope.row.authorizationAllFlag" :indeterminate=" (
                  scope.row.authorizationInfo.visibleFlag | scope.row.authorizationInfo.addFlag ||scope.row.authorizationInfo.modifyFlag || scope.row.authorizationInfo.deleteFlag)
                  && !scope.row.authorizationAllFlag" @change="handleCheckAllChange(scope.row.authorizationInfo, scope.row)" />
              </template>
            </el-table-column>
          </el-table>
        </el-card>
      </div>
    </div>
  </div>
</template>
<script>
import roleAPI from '@/api/role'
import BaseList from '@/views/master-page/BaseList'
import adaptive from '@/directive/el-table'
export default {
  name: 'Role',
  components: { BaseList },
  directives: { adaptive },
  data() {
    return {
      // title="角色列表" :curd="curd" :edit-key-words="editKeyWords" :view-key-words="editKeyWords"
      defaultBLProps: {
        defaultDTLProps: {
          sortStr: { properties: 'theOrder', direction: 'ASC' }, // 排序方法
          title: { mainTitle: '角色列表' },
          defaultDTHProps: {
            keyWord: { edit: 'SysRole' }
          }
        },
        defaultSDProps: {
          keyWord: 'SysRole',
          defaultDBProps: {
            dialog: {}
          }
        }
      },

      treeData: [],
      currentColumn: '',
      parentArr: [],
      loading: false,
      saveLoading: false
    }
  },

  watch: {
    'currentColumn.id'(val) {
      this.initDataTree()
    }
  },

  methods: {
    async initDataTree() {
      if (this.currentColumn.id) {
        try {
          this.loading = true
          const resp = await roleAPI.getRolePermissions(this.currentColumn.id)
          this.treeData = resp.data
          this.initAllNodes()
          this.loading = false
        } catch (error) {
          this.loading = false
        }
      }
    },
    // 初始化所有节点信息
    initAllNodes() {
      this.treeData[0].authorizationInfo.visibleFlag = true
      this.treeData[0].authorizationInfo.addFlag = true
      this.treeData[0].authorizationInfo.modifyFlag = true
      this.treeData[0].authorizationInfo.deleteFlag = true
      this.treeData[0].authorizationAllFlag = true
      this.initCallFunc(this.treeData, 'authorizationAllFlag')
      this.initCallFunc(this.treeData, 'visibleFlag')
      this.initCallFunc(this.treeData, 'addFlag')
      this.initCallFunc(this.treeData, 'modifyFlag')
      this.initCallFunc(this.treeData, 'deleteFlag')
    },
    // 增删改查的联动
    initCallFunc(val = this.treeData, prop) {
      if (prop === 'authorizationAllFlag') {
        val.forEach(e => {
          if (e[prop] === false) {
            return (this.treeData[0][prop] = false)
          }
          if (e.children && e.children.length) {
            this.initCallFunc(e.children, prop)
          }
        })
      } else {
        val.forEach(e => {
          if (e.authorizationInfo[prop] === false) {
            return (this.treeData[0].authorizationInfo[prop] = false)
          }
          if (e.children && e.children.length) {
            this.initCallFunc(e.children, prop)
          }
        })
      }
    },
    // 回调循环函数
    allCallFunc(val, status) {
      if (val.children && val.children.length > 0) {
        val.children.forEach(e => {
          e.authorizationAllFlag = status
          e.authorizationInfo.visibleFlag = e.authorizationInfo.addFlag = e.authorizationInfo.modifyFlag = e.authorizationInfo.deleteFlag = status
          this.allCallFunc(e, status)
        })
      }
      const pids = []
      this.getParentNodes(this.treeData, val.id, pids)
      this.parentNodesChange(this.treeData, pids, val)
    },
    // 单个回调函数
    singleCallFunc(val, prop) {
      if (val.children && val.children.length > 0) {
        val.children.forEach(e => {
          prop === 'visibleFlag' ? (e.authorizationInfo.visibleFlag = val.authorizationInfo.visibleFlag) : ''
          prop === 'addFlag' ? (e.authorizationInfo.addFlag = val.authorizationInfo.addFlag) : ''
          prop === 'modifyFlag' ? (e.authorizationInfo.modifyFlag = val.authorizationInfo.modifyFlag) : ''
          prop === 'deleteFlag' ? (e.authorizationInfo.deleteFlag = val.authorizationInfo.deleteFlag) : ''

          // 更新全选-非全选的内容
          e.authorizationAllFlag = false
          if (
            e.authorizationInfo.visibleFlag === true && e.authorizationInfo.addFlag === true &&
            e.authorizationInfo.modifyFlag === true && e.authorizationInfo.deleteFlag === true
          ) {
            e.authorizationAllFlag = true
          } else if (
            val.authorizationInfo.visibleFlag === false && val.authorizationInfo.addFlag === false &&
            val.authorizationInfo.modifyFlag === false && val.authorizationInfo.deleteFlag === false
          ) {
            e.authorizationAllFlag = false
          }
          this.singleCallFunc(e, prop)
        })
      }
      const pids = []
      this.getParentNodes(this.treeData, val.id, pids)
      this.parentNodesChange(this.treeData, pids, val)
    },
    // 当前行的改变
    updateColumn(val) {
      this.currentColumn = val
    },
    // 获取每层父节点的节点数组
    getParentNodes(val, id, arr) {
      val.forEach(e => {
        if (e.id === id) {
          if (e.parentId) {
            // 排除顶级虚节点id=-1
            arr.push(e.parentId)
          }
          this.getParentNodes(this.treeData, e.parentId, arr)
        } else {
          if (e.children && e.children.length) {
            this.getParentNodes(e.children, id, arr)
          }
        }
      })
    },
    parentNodesChange(arr, pids, val) {
      arr.forEach(e => {
        if (pids.indexOf(e.id) > -1) {
          val.authorizationInfo.visibleFlag ? (e.authorizationInfo.visibleFlag = true) : ''
          val.authorizationInfo.addFlag ? (e.authorizationInfo.addFlag = true) : ''
          val.authorizationInfo.modifyFlag ? (e.authorizationInfo.modifyFlag = true) : ''
          val.authorizationInfo.deleteFlag ? (e.authorizationInfo.deleteFlag = true) : ''
          e.authorizationAllFlag = false
          if (
            e.authorizationInfo.visibleFlag === true && e.authorizationInfo.addFlag === true &&
            e.authorizationInfo.modifyFlag === true && e.authorizationInfo.deleteFlag === true
          ) {
            e.authorizationAllFlag = true
          } else if (
            val.authorizationInfo.visibleFlag === false && val.authorizationInfo.addFlag === false &&
            val.authorizationInfo.modifyFlag === false && val.authorizationInfo.deleteFlag === false
          ) {
            e.authorizationAllFlag = false
          }
        }
        if (e.children && e.children.length) {
          this.parentNodesChange(e.children, pids, val)
        }
      })
    },
    // 树节点的联动
    parentNodeChange(val) {
      if (val.children && val.children.length) {
        val.children.forEach(child => {
          if (child.authorizationInfo.visibleFlag === true) {
            return (val.authorizationInfo.visibleFlag = true)
          } else {
            this.parentNodeChange(child)
          }
        })
      }
    },
    // 全选
    handleCheckAllChange(val, val2) {
      if (val2.authorizationAllFlag) {
        val.visibleFlag = val.addFlag = val.modifyFlag = val.deleteFlag = true
        this.allCallFunc(val2, true)
      } else {
        val.visibleFlag = val.addFlag = val.modifyFlag = val.deleteFlag = false
        this.allCallFunc(val2, false)
      }
      this.treeData[0].authorizationAllFlag = true
      this.initAllNodes()
    },
    // 改变单个框的变化
    handleSingleChange(val, val2, prop) {
      val2.row.authorizationAllFlag = false
      if (
        val.visibleFlag === true && val.addFlag === true && val.modifyFlag === true && val.deleteFlag === true
      ) {
        // 使用scope.row直接赋值没有效果
        val2.row.authorizationAllFlag = true
      } else if (
        val.visibleFlag === false && val.addFlag === false && val.modifyFlag === false && val.deleteFlag === false
      ) {
        val2.row.authorizationAllFlag = false
      }

      this.singleCallFunc(val2.row, prop)
      this.treeData[0].authorizationAllFlag = true
      this.initAllNodes()
    },

    // 保存权限
    async saveRoleMenus() {
      try {
        this.saveLoading = true
        await roleAPI.editRolePermission(this.currentColumn.id, this.treeData[0])
        this.saveLoading = false
        this.$message.success('保存成功！')
      } catch (error) {
        this.saveLoading = false
      }
    }
  }
}
</script>
<style lang="scss" scoped>
::v-deep .el-card__body {
  padding: 0;
}
.box-flex {
  display: flex;
  justify-content: space-between;
}
</style>
