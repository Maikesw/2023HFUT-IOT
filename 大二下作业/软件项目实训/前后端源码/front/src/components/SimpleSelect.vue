<template>
  <div>
    <el-select v-model="selectValue" v-loading="loading" class="simple-select" :placeholder="placeholder" :size="size" :multiple="multiple" :disabled="disabled" :filterable="filterable" @change="handleChange">
      <el-option v-for="item in options" :key="item.id" :label="item.name" :value="item.id" :value-key="item.id" />
    </el-select>
  </div>
</template>

<script>
import listAPI from '@/api/list'
import _ from 'lodash'
export default {
  props: {
    value: { type: [Number, String, Array], default: () => null },
    field: { type: String, default: '' },
    keyWords: { type: String, default: '' },
    multiple: { type: Boolean, default: () => false },
    placeholder: { type: String, default: '请选择' },
    searchKey: { type: Object, default: () => ({}) },
    regKey: { type: Object, default: () => ({}) },
    sortJson: { type: Object, default: () => ({ properties: 'TheOrder', direction: 'ASC' }) },
    size: { type: String, default: '' },
    filterable: { type: Boolean, default: false },
    autoInit: { type: Boolean, default: true },
    disabled: { type: Boolean, default: false },
    autoSelect: { type: Boolean, default: true },
    changeLabel: { type: String, default: '' }
  },
  data() {
    return {
      options: [],
      loading: false,
      selectValue: this.value
    }
  },
  computed: {
    // selectValue: {
    //   get: function() {
    //     return this.value
    //   },
    //   set: function(val) {
    //     //this.value = val
    //   }
    // }
  },
  watch: {
    value: {
      async handler(val) {
        if (this.autoInit) {
          if (!(this.options) || (this.options.length === 0)) {
            await this.initOptions()
          }
        }
        this.changeSelection()
      },
      immediate: true
    },
    searchKey: {
      async handler(val) {
        await this.initOptions()
        await this.changeSelection()
      },
      deep: true,
      immediate: true
    }
  },
  created() {
    if (this.autoInit) {
      this.initOptions()
    }
  },
  methods: {
    async changeSelection(val) {
      if (this.options.length === 0) {
        await this.initOptions()
      }
      if (val != null) {
        this.selectValue = val
      } else if (this.value != null) {
        if (this.multiple && typeof this.value === 'object') {
          this.selectValue = this.value.filter(item => this.options.map(item2 => item2.id).includes(item))
        } else {
          if (this.options.map(item => item.id).includes(parseInt(this.value))) {
            this.selectValue = parseInt(this.value)
          }
        }
      } else {
        this.selectValue = null
      }
    },
    async initOptions() {
      this.loading = true
      try {
        const resp = await listAPI.getSomeRecords({
          keyWords: this.keyWords,
          searchKey: this.searchKey,
          sort: this.sortJson,
          reg: this.regKey
        })
        this.options = _.cloneDeep(resp.data.content)
        if (this.changeLabel !== '') {
          this.options.forEach(item => {
            this.$set(item, 'name', item[this.changeLabel])
          })
        }
        this.$emit('init-finish', this.field, this.options)
      } catch (error) {
        this.loading = false
      }
      if (this.autoSelect) {
        if (this.selectValue == null && this.options.length > 0) {
          this.selectValue = this.options[0].id
          this.$emit('update-value', this.selectValue, this.field, this.options.filter((value, index) => { return value.id === this.selectValue }))
        }
      }
      this.loading = false
    },
    handleChange(val) {
      this.$emit('update-value', val, this.field, this.options.filter((value, index) => { return value.id === val }))
    }
  }
}
</script>
