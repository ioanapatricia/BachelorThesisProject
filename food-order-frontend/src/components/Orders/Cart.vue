<template>
  <div class="cart-container">
    <div class="cart">
      <cart-header text="Cart" />
      <products-deck v-if="cartItems.length > 0" />
      <cart-summary v-if="cartItems.length > 0" />
      <cart-empty v-if="cartItems.length == 0" />
      <div class="cart-footer">
        <page-footer />
      </div>
    </div>
  </div>
</template>

<script>
import CartHeader from './CartHeader'
import ProductsDeck from './ProductsDeck'
import CartSummary from './CartSummary'
import PageFooter from '../Footer'
import CartEmpty from './CartEmpty'

export default {
  name: 'Cart',
  components: {
    CartHeader,
    ProductsDeck,
    CartSummary,
    PageFooter,
    CartEmpty,
  },
  computed: {
    cartItems() {
      return this.$store.getters.getCartItems
    },
  },
}
</script>

<style scoped lang="scss">
.cart-container {
  display: flex;
  flex-direction: row;
  // min-width: 100%;
}
.cart {
  background-color: #fff;
  min-width: 100%;
  color: #111;
}
.cart-footer {
  min-width: 100%;
  background-color: #111;
  color: #fff;
}
</style>
