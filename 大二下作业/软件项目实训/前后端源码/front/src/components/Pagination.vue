<template>
  <footer id="vpage-footer">
    <el-pagination
      background :current-page="currentPage" :page-sizes="pageSizes" :page-size="pageSize" :layout="layout" :total="total" @size-change="handleSizeChange" @current-change="handleCurrentChange" />
    <span v-if="selectedColumns.length>0" class="public-footer">当前选中：{{ selectedColumns.length }}条</span>
  </footer>
</template>

<script>
export default {
  // FIXME: Vue属性定义必须用下面这种严格形式，属性名必须小驼峰
  props: {
    pageSize: { type: Number, default: 10 },
    pageSizes: { type: Array, default: () => [10, 20, 30, 40] },
    total: { type: Number, default: 0 },
    currentPage: { type: Number, default: 1 },
    layout: { type: String, default: 'total, sizes, prev, pager, next, jumper' },
    selectedColumns: { type: Array, default: () => [] }
  },
  computed: {
    isCollapsed() {
      return this.$store.getters.collapsed
    }
  },
  methods: {
    // 分页大小改变
    handleSizeChange(val) {
      // 事件传递
      this.$emit('size-change', val)
    },
    // 当前页
    handleCurrentChange(val) {
      // 事件传递
      this.$emit('current-change', val)
    }
  }
}
</script>
<style lang="scss" scoped>
@import "~@/assets/css/variables.scss";
#vpage-footer {
  background-color: #fff;
  height: fit-content;
  display: flex;
  justify-content: center;
  align-content: center;
}
.el-pagination {
  padding: 10px 0;
}
.public-footer {
  // position: fixed;
  z-index: 99;
  right: 24px;
  bottom: 0px;
  padding-left:10px;
  line-height: 48px !important;
  height: 40px !important;
  text-align: center;
}
.private-footer {
  position: static;
}
::v-deep .el-pagination.is-background .el-pager li:not(.disabled).active {
  background-color: $--color-primary !important;
}
</style>
