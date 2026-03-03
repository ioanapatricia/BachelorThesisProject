<template>
  <div class="category-card" :style="{ order: category.sortingOrderOnWebpage }" v-if="category.products.length">
    <a :id="category.name"></a>
    <products-deck :products="category.products" />
    <img :src="getImageUrl" :alt="category.name + ' banner'" />
  </div>
</template>

<script>
import ProductsDeck from './ProductsDeck'
import { getImageForDisplayUrl } from '../../apiService/apiService'
export default {
  name: 'CategoryCard',
  props: ['category'],
  components: { ProductsDeck },

  computed: {
    getImageUrl() {
      return getImageForDisplayUrl(this.category.banner.id)
    },
  },
}
</script>

<style scoped>
.category-card {
}

.category-card img {
  max-width: 100%;
}
</style>
