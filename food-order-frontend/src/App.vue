<template>
  <div id="app">
    <side-menu />
    <map-modal />
    <transition name="fade" mode="out-in">
      <router-view class="margin-left" />
    </transition>
  </div>
</template>

<script>
import SideMenu from './components/SideMenu'
import MapModal from './components/Map/Map'
export default {
  components: { SideMenu, MapModal },
  created() {
    this.$store.dispatch('fetchCategoriesWithProducts')
    this.$store.dispatch('fetchProductsFromApi')
    this.$store.dispatch('fetchSettingsFromApi')
  },
}
</script>

<style lang="scss">
@import './assets/style/reset.css';
@import url('https://fonts.googleapis.com/css2?family=Roboto:ital,wght@0,100;0,300;0,400;0,500;0,700;0,900;1,100;1,300;1,400;1,500;1,700;1,900&display=swap');

html {
  font-size: 17px;
  line-height: 1.15;
  background-color: #111;
}

html,
body,
#app {
  font-family: 'Roboto', sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
}

#app {
  color: #fff;
}

#nav {
  padding: 30px;

  a {
    font-weight: bold;
    // color: #2c3e50;

    &.router-link-exact-active {
      color: #42b983;
    }
  }
}

.fade-enter-from,
.fade-leave-to {
  opacity: 0;
}

.fade-enter-active {
  transition: opacity 0.3s ease-out;
}

.fade-leave-active {
  transition: opacity 0.3s ease-in;
}

.fade-enter-to,
.fade-leave-from {
  opacity: 1;
}

.margin-left {
  margin-left: 8rem;
}
</style>
