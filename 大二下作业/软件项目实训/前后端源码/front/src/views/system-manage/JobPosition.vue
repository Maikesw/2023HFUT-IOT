<template>
  <BaseList :default-props="defaultProps" />
</template>

<script>
import BaseList from '@/views/master-page/BaseList'

export default {
  name: 'JobPosition',
  components: { BaseList },
  data() {
    return {
      defaultProps: {
        defaultDTLProps: {
          defaultDTHProps: {
            keyWord: { edit: 'BaseJobPosition' }
          },
          sortStr: { properties: 'theOrder', direction: 'ASC' }
        },
        defaultSDProps: {
          keyWord: 'BaseJobPosition'
        }
      }
    }
  }
}
</script>
