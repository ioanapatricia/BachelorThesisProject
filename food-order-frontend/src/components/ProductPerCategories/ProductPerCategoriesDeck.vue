<template>
  <div class="product-per-categories-container">
    <category-card
      v-for="(category, index) in categoriesWithProducts"
      :key="index"
      :category="category"
    />
  </div>
</template>

<script>
import CategoryCard from './CategoryCard'
export default {
  name: 'ProductPerCategories',
  components: { CategoryCard },
  computed: {
    categoriesWithProducts() {
      return this.$store.getters.getCategoriesWithProducts
    },
  },
}
</script>

<style scoped>
.product-per-categories-container {
  padding-top: 60px;
  background-color: #fff;
  color: #000;
  display: flex;
  flex-direction: column;
}
</style>
