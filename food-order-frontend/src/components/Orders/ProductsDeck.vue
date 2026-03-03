<template>
  <b-container>
    <hr class="dotted-line" />
    <product-card
      v-for="(product, index) in cartProducts"
      :key="index"
      :product="product"
    />
  </b-container>
</template>

<script>
import ProductCard from '@/components/Orders/ProductCard.vue'

export default {
  name: 'ProductsDeck',
  components: { ProductCard },

  computed: {
    cartItems() {
      return this.$store.getters.getCartItems
    },
    cartProducts() {
      return this.$store.getters.getCartProducts
    },
  },
}
</script>

<style scoped lang="scss">
.dotted-line {
  border-bottom: 1px dotted #111;
}
</style>
