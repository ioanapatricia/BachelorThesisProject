<template>
  <b-link
    v-if="category.products.length"
    :href="'#' + category.name"
    class="wrapper"
    :style="{ order: category.sortingOrderOnWebpage }"
  >
    <img :src="getImageUrl" :alt="category.name + ' logo'" />
    {{ category.name }}
  </b-link>
</template>

<script>
import { getImageForDisplayUrl } from '../../apiService/apiService'
export default {
  name: 'Category',
  props: ['category'],
  computed: {
    getImageUrl() {
      return getImageForDisplayUrl(this.category.logo.id)
    },
  },
}
</script>

<style scoped>
.wrapper {
  display: flex;
  align-items: center;
  justify-content: center;
  flex-direction: column;
  color: rgba(0, 0, 0, 0.7);
  width: 5rem;
}
.wrapper img {
  height: 2rem;
  margin-bottom: 10px;
}
</style>
