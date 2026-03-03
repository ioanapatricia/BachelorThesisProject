<template>
  <b-container class="products-deck">
    <b-row cols="2" align-h="center">
      <product-card
        v-for="(product, index) in products"
        :key="index"
        :product="product"
      />
    </b-row>
  </b-container>
</template>

<script>
import ProductCard from './ProductCard'
export default {
  name: 'ProductsDeck',
  props: ['products'],
  components: { ProductCard },
}
</script>

<style scoped></style>
