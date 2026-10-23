<template>
  <div id="search-box"> <!-- class="search-wrapper" -->
    <div v-if="noAdvancedSearch" class="no-advanced-search">
      <el-input ref="inputInfo" v-model.trim="input" size="mini" :placeholder="placeholder" clearable />
      <el-button size="mini" type="primary" icon="el-icon-search" @click="searchClick('common')">搜索</el-button>
    </div>
    <div v-else class="advanced-search-field-box">
      <div v-for="(item, index) in searchItems" :key="index" class="search-item">
        <slot v-if="item.type.startsWith('customize')" :name="item.slot" />
        <el-cascader v-if="item.type==='cascader'" v-model="waitSearch[item.field]" size="mini" :loading="loading" :options="item.options" :props="{
          label: 'name', value: 'id', checkStrictly: item.checkStrictly
        }" :placeholder="item.placeholder" clearable />
        <el-select v-if="item.type==='select'" v-model="waitSearch[item.field]" size="mini" :placeholder="item.placeholder?item.placeholder:item.name" :loading="loading" clearable>
          <el-option v-for="citem in item.options" :key="citem.id" :label="citem.name" :value="citem.id" />
        </el-select>
        <el-input v-if="item.type==='input'" v-model="waitSearch[item.field]" size="mini" :placeholder="item.placeholder?item.placeholder:item.name" clearable />
        <el-date-picker v-if="item.type==='date'" v-model="waitSearch[item.field]" size="mini" type="daterange" :start-placeholder="item.placeholder?item.placeholder:item.name+'：从'" :end-placeholder="item.placeholder?item.placeholder:item.name+'：到'" />
      </div>
      <div class="search-item search-button">
        <el-button size="mini" type="success" icon="el-icon-search" @click="searchClick('notCommon')">搜索</el-button>
        <el-button size="mini" plain icon="el-icon-refresh-left" @click="reset()">重置</el-button>
      </div>
    </div>
  </div>
</template>

<script>
import { resetForm } from '@/utils/common'
import _ from 'lodash'
export default {
  name: 'BtnSearch',
  // 默认情况下，一个组件上的 v-model 会把 value 用作 prop 且把 input 用作 event
  model: {
    prop: 'value', // 注意，是prop，不带s
    event: 'input'
  },
  props: {
    value: { type: [Object, String], default: '' }, // 父组件 v-model 绑定值
    // 不显示高级搜索
    noAdvancedSearch: { type: Boolean, default: false },
    searchItems: { type: Array, default: () => [] },
    placeholder: { type: String, default: '请输入名称' },
    searchName: { type: [String, Array], default: '' }
  },
  data() {
    return {
      waitSearch: [],
      input: '',
      options: [],
      currentField: '',
      loading: false
    }
  },
  watch: {
    // waitSearch: {
    //   handler(newVal) {
    //     // 父组件有 v-model 时的操作
    //     this.$emit('input', newVal)
    //     // 是否是空字符串（no-advanced-search === true）
    //     const blankString = getValueType(newVal) === 'string' && newVal === ''
    //     // 是否是对象并且对象中的值全为空（no-advanced-search === false）
    //     const blankStringObject = getValueType(newVal) === 'object' && isObjEmpty(newVal)
    //     if (blankString || blankStringObject) {
    //       this.$emit('search', newVal)
    //     }
    //   },
    //   deep: true
    // }
  },
  create() {
    this.waitSearch = _.cloneDeep(this.value)
  },
  methods: {
    async handleFocus(val) {
      // if (this.currentField === val.field) return
      // this.currentField = val.field
      // const index = this.searchItems.findIndex(item => item.field === val.field)
      // if (index >= 0) {
      //   try {
      //     if (val.type === 'cascader') {
      //       const res = await treeAPI.getAllNodes({
      //         keyWords: val.keyWords,
      //         virtualRootFlag: false
      //       })
      //       // 除去children
      //       this.searchItems[index].options = JSON.parse(
      //         JSON.stringify(res.data).replace(/\"children"\:\[]/g, '"no":0')
      //       )
      //     } else if (val.type === 'select') {
      //       const res = await listAPI.getSomeRecords({
      //         keyWords: val.keyWords, pageInfo: { page: -1 }
      //       })
      //       this.searchItems[index].options = res.data.content
      //     }
      //     this.loading = false
      //   } catch (error) {
      //     this.loading = false
      //   }
      // }
      // this.$emit('focus', val.field)
    },
    searchClick(type) {
      // this.$emit('search-click', this.waitSearch)
      if (type === 'common') {
        this.$emit('search-click', this.input)
      } else {
        this.$emit('advanced-search-click', this.waitSearch)
      }
    },
    reset() {
      this.waitSearch = resetForm(this.waitSearch)
      // this.$emit('search-click', this.waitSearch)
    }
  }
}
</script>

<style lang="scss" scoped>
@import "~@/assets/css/variables.scss";
#search-box {
  margin-bottom: 20px;
}
.no-advanced-search {
  display: flex;
  .el-button {
    margin-left: 10px;
  }
}
.advanced-search-field-box {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(240px, 1fr));
  gap: 10px;
  .el-button + .el-button {
    margin-left: 10px;
  }
}
// .el-button.is-plain:hover {
  // border-color: $--color-subsidiary;
  // color: $--color-subsidiary;
// }
</style>
