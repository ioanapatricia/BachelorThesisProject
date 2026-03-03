<template>
  <div class="side-menu">
    <b-link class="item" to="/">
      <img :src="require('../assets/img/buddha.png')" style="height:60%"/>
    </b-link>
    <b-link class="item" to="/cart">
      <!-- <img :src="require('../assets/img/shopping-cart.svg')" /> -->
      <b-icon-cart3 scale="2" class="mb-3" />
      Shopping Cart
      <b-button
        v-if="cartItemsNumber > 0"
        pill
        size="sm"
        variant="warning"
        class="numberOfItems"
        >{{ cartItemsNumber }}</b-button
      >
    </b-link>
    <b-link class="item" to="/">
      <b-icon-chat-left-dots scale="2" class="mb-3" />
      Chat
    </b-link>
    <b-link class="item" v-b-modal.modal-map>
      <b-icon-map scale="2" class="mb-3" />
      Location
    </b-link>
    <b-link class="item" to="/">
      <b-icon-people scale="2" class="mb-3" />
      About Us
    </b-link>
    <div class="item">
      <b-icon-telephone scale="2" class="mb-3" />
      {{ settings ? settings.phoneNumber : '' }}
    </div>
    <div class="item">
      <b-icon-clock scale="2" class="mb-3" />
      {{ settings ? settings.schedule : '' }}
    </div>
  </div>
</template>

<script>
export default {
  name: 'SideMenu',
  computed: {
    cartItemsNumber() {
      return this.$store.getters.getCartItems.length
    },
    settings() {
      return this.$store.getters.getSettings
    },
  },
}
</script>

<style scoped lang="scss">
.side-menu {
  position: fixed;
  top: 0;
  left: 0;
  width: 8rem;
  height: 100vh;
  display: flex;
  flex-direction: column;
  border-right: 1px solid #615f5b;
  background-color: #111;
}
.item {
  position: relative;
  height: 14.1666%;
  display: flex;
  align-items: center;
  justify-content: center;
  flex-direction: column;
}
.item:first-child {
  height: 15%;
}
.item:first-child,
.item:nth-child(5) {
  border-bottom: 1px solid #615f5b;
}
img {
  height: 30px;
  filter: invert(1);
  margin-bottom: 10px;
}
.numberOfItems {
  position: absolute;
  top: 25px;
  right: 30px;
}

.router-link-active {
  &:link {
    color: aliceblue;
  }
  &:visited {
    color: aliceblue;
  }
  text-decoration: none;
}
.router-link-active:hover {
  color: #615f5b;
}
.item {
  &:link {
    color: aliceblue;
  }
  &:visited {
    color: aliceblue;
  }
  text-decoration: none;
}
.item:hover {
  color: #615f5b;
}
</style>
