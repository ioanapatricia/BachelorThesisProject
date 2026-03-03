<template>
  <div class="error-competed-container">
    <div class="error">
      <cart-header text="Error" />
      <div class="error-empty-container">
        Something went wrong pal. Let's have a call.
      </div>
      <div class="error-footer">
        <page-footer />
      </div>
    </div>
  </div>
</template>

<script>
import CartHeader from '../components/Orders/CartHeader'
import PageFooter from '../components/Footer'
export default {
  name: 'Error',
  components: { CartHeader, PageFooter },
}
</script>

<style lang="scss" scoped>
.error-competed {
  &-container {
    display: flex;
    flex-direction: row;
    // min-width: 100%;
  }
}
.error {
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
    background-image: url('../assets/img/pizza-crying.jpg');
    background-repeat: no-repeat;
    background-position: 50% 15%;
    background-size: 30%;
    z-index: 0;
  }
}
.error-footer {
  min-width: 100%;
  background-color: #111;
  color: #fff;
}
.error-empty-container {
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
