<template>
  <v-app>
    <side-menu v-if="!isAuth" />
    <v-main>
      <v-container
        fluid
        :fill-height="isAuth || isHome"
        :class="{ 'grey darken-4': isAuth }"
      >
        <v-fade-transition mode="out-in">
          <router-view />
        </v-fade-transition>
      </v-container>
    </v-main>
  </v-app>
</template>

<script>
import SideMenu from '@/components/SideMenu'

import { getLoggedUser } from './helpers/authExtensions'

export default {
  name: 'App',
  components: { SideMenu },

  data: () => ({}),

  provide: {
    user: getLoggedUser(),
  },

  computed: {
    isAuth() {
      const page = this.$route.name
      return page == 'Auth' ? true : false
    },
    isHome() {
      const page = this.$route.name
      return page == 'Home' ? true : false
    },
  },
}
</script>
