<template>
  <div class="home-container">
    <main-screen />
  </div>
</template>

<script>
// @ is an alias to /src
import MainScreen from '@/components/MainScreen.vue'

export default {
  name: 'Home',
  components: { MainScreen },
}
</script>

<style scoped>
.home-container {
  display: flex;
  flex-direction: row;
  background-color: #111;
}
</style>
