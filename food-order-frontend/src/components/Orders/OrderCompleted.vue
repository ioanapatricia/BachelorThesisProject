<template>
  <div class="order-competed-container">
    <div class="order">
      <cart-header text="Cart" />
      <div class="cart-empty-container">
        Order placed. Will be delivered soon
      </div>
      <div class="order-footer">
        <page-footer />
      </div>
    </div>
  </div>
</template>

<script>
import CartHeader from './CartHeader'
import PageFooter from '../Footer'
export default {
  name: 'OrderCompleted',
  components: { CartHeader, PageFooter },
}
</script>

<style lang="scss" scoped>
.order-competed {
  &-container {
    display: flex;
    flex-direction: row;
    // min-width: 100%;
  }
}
.order {
  background-color: #fff;
  min-width: 100%;
  color: #111;
  position: relative;
  &:before {
    content: ' ';
    display: block;
    position: absolute;
    left: 0;
    top: 0;
    width: 100%;
    height: 100%;
    opacity: 0.3;
    background-image: url('../../assets/img/pizza-eye.png');
    background-repeat: no-repeat;
    background-position: 50% 15%;
    background-size: 30%;
    z-index: 0;
  }
}
.order-footer {
  min-width: 100%;
  background-color: #111;
  color: #fff;
}
.cart-empty-container {
  font-size: 44px;
  text-shadow: 0px 0px 10px #fff;
  display: flex;
  flex-direction: row;
  align-items: center;
  justify-content: center;
  width: 100%;
  height: 60vh;
  text-align: center;
  position: relative;
  z-index: 1;
}
</style>
