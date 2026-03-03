<template>
  <div class="main-screen-container">
    <landing-component />
    <categories-menu />
    <product-per-categories-deck />
    <comming-soon />
    <footer-component />
  </div>
</template>

<script>
import LandingComponent from './LandingComponent'
import CategoriesMenu from './CategoriesMenu/CategoriesMenu.vue'
import ProductPerCategoriesDeck from './ProductPerCategories/ProductPerCategoriesDeck'
import CommingSoon from './CommingSoon'
import FooterComponent from './Footer'
export default {
  name: 'MainScreen',
  components: {
    LandingComponent,
    CategoriesMenu,
    ProductPerCategoriesDeck,
    CommingSoon,
    FooterComponent,
  },
}
</script>

<style scoped>
.main-screen-container {
  width: 100%;
  display: flex;
  flex-direction: column;
}
</style>
