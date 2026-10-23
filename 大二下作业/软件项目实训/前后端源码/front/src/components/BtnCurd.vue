<template>
  <div class="crud-opts">
    <span>
      <!--左侧插槽-->
      <slot name="left" />
      <el-button size="mini" type="primary" icon="el-icon-plus" :disabled="addDisabled" @click="append">
        新增 </el-button>
      <el-button size="mini" icon="el-icon-edit" :disabled="selectedColumns.length !== 1" @click="$emit('update', selectedColumns[0])"> 修改 </el-button>
      <el-button slot="reference" type="danger" icon="el-icon-delete" size="mini" :disabled="selectedColumns.length < 1" @click="$emit('remove')"> 删除 </el-button>
      <el-button size="mini" type="warning" icon="el-icon-download" @click="exportData">导出</el-button>
      <!--右侧-->
      <slot name="right" />
    </span>
    <span>
      <el-button-group>
        <el-button size="mini" plain type="info" icon="el-icon-search" @click="toggleSearch()" />
        <el-button size="mini" icon="el-icon-refresh" @click="refresh()" />
      </el-button-group>
    </span>
  </div>
</template>
<script>
import crud from '@/mixins/curd'
export default {
  mixins: [crud],
  props: {
    selectedColumns: { type: Array, default: () => [] }
  },
  data() {
    return {
      addDisabled: false
    }
  },
  methods: {
    append() {
      this.$emit('create')
    },
    // 刷新数据
    refresh() {
      this.$emit('refresh')
    },
    exportData() { }
  }
}
</script>

<style>
.crud-opts {
  display: -webkit-flex;
  display: flex;
  align-items: center;
  justify-content: space-between;
}
</style>
