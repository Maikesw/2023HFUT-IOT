<template>
  <div>
    <el-row :gutter="15">
      <el-col :span="open ? size : 1" class="col-tree" :class="open?'':'is-shrink'">
        <div class="tree-aside">
          <slot name="aside" />
          <div class="caret-board">
            <i class="el-icon-caret-left" @click="open = !open" />
          </div>
        </div>
        <div class="col-tree-shrink" @click="open = !open">
          <el-card v-adaptive-card shadow="never" />
          <i class="el-icon-caret-right" />
        </div>
      </el-col>
      <el-col :span="open ? 24 - size : 23">
        <slot name="main" />
      </el-col>
    </el-row>
  </div>
</template>

<script>
import adaptiveCard from '@/directive/el-card'
export default {
  name: 'TreeList',
  directives: { adaptiveCard },
  data() {
    return {
      size: 5,
      open: true
    }
  }
}
</script>

<style lang="scss" scoped>
@import "~@/assets/css/element-variables.scss";
.el-col {
  transition: all 300ms linear;
}
.col-tree {
  position: relative;
  overflow: hidden;
  .el-icon-caret-left,
  .el-icon-caret-right {
    cursor: pointer;
    font-size: 20px;
    position: absolute;
    top: 50%;
    right: 10px;
  }
  .el-icon-caret-left:hover,
  .el-icon-caret-right:hover {
    color: $--color-primary;
  }
}

.is-shrink .tree-aside {
  visibility: hidden;
  display: none;
}

.col-tree-shrink {
  display: none;
}

.is-shrink .col-tree-shrink {
  display: block;
  cursor: pointer;
  .el-icon-caret-right {
    right: unset;
    left: 50%;
    transform: translateX(-50%);
  }
  &:hover {
    .el-icon-caret-right {
      color: $--color-primary;
    }
  }
}
</style>
