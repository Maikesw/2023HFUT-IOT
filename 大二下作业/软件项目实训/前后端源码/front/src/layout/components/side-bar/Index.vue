<template>
  <aside :class="{ 'has-logo': showLogo }">
    <!-- <logo v-if="showLogo" :collapse="isCollapse" /> -->
    <el-scrollbar wrap-class="scrollbar-wrapper">
      <el-menu :default-active="activeMenu" :collapse="isCollapse" :background-color="variables.menuBg" :unique-opened="true" :collapse-transition="false" mode="vertical">
        <SideBarItem v-for="(route, index) in permission_routes" :key="index" :item="route" :base-path="route.path" @tabList="getTabList" />
      </el-menu>
    </el-scrollbar>
  </aside>
</template>

<script>
import { mapGetters } from 'vuex'
import SideBarItem from './SideBarItem'
import variables from '@/assets/css/variables.scss'

export default {
  components: { SideBarItem },
  data() {
    return {
      tabList: []
    }
  },

  computed: {
    ...mapGetters(['permission_routes', 'sideBar', 'roles']),
    routes() {
      return this.$router.options.routes
    },
    activeMenu() {
      const route = this.$route
      const { meta, path } = route
      // if set path, the sideBar will highlight the path you set
      if (meta.activeMenu) {
        return meta.activeMenu
      }
      return path
    },
    showLogo() {
      return this.$store.state.settings.sideBarLogo
    },
    variables() {
      return variables
    },
    isCollapse() {
      return !this.sideBar.opened
    }
  },
  methods: {
    getTabList(val) {
      this.tabList = val
    }
  }
}
</script>
