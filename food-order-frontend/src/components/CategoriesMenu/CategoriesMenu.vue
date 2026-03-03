<template>
  <div class="categories-menu-container">
    <category
      v-for="(category, index) in categories"
      :key="index"
      :category="category"
    />
  </div>
</template>

<script>
import Category from './Category'
export default {
  name: 'CategoriesMenu',
  components: { Category },
  computed: {
    categories() {
      return this.$store.getters.getCategoriesWithProducts
    },
  },
}
</script>

<style scoped>
.categories-menu-container {
  width: 100%;
  display: flex;
  flex-direction: row;
  justify-content: space-evenly;
  padding: 20px 0;
  background-color: #fff;
  box-shadow: 0 1px 6px 0 rgb(0 0 0 / 6%);
}
</style>
