<template>
  <b-col cols="6" class="product-card">
    <div class="image-container">
      <img
        :src="getImageUrl"
        :alt="product.image.name"
        class="product-image"
        @mouseover="isHovering = true"
        @mouseout="isHovering = false"
        :class="{ 'animate__animated animate__pulse': isHovering }"
      />
    </div>
    <div class="product-body">
      <div class="product-data">
        <div class="product-name">
          {{ product.name }}
        </div>
        <div class="product-description">
          {{ product.description }}
        </div>
      </div>
      <div class="variants-single" v-if="product.variants.length == 1">
        <div class="variants-single-data">
          <span>{{
            product.variants[0].weight + ' ' + product.weightType.name
          }}</span>
          <span>{{ product.variants[0].price + ' RON' }}</span>
        </div>
        <b-button
          @click="addToCart(product.id, product.variants[0].id)"
          class="button-single"
          squared
          variant="dark"
        >
          Add to cart <b-icon-cart-plus />
        </b-button>
      </div>
      <div class="variants-multiple" v-else>
        <div class="variants-multiple-data">
          <span>{{
            product.variants[0].weight +
              ' - ' +
              product.variants[product.variants.length - 1].weight +
              ' ' +
              product.weightType.name
          }}</span>
          <span>{{
            product.variants[0].price +
              ' - ' +
              product.variants[product.variants.length - 1].price +
              ' RON'
          }}</span>
        </div>
        <b-button
          :id="'popover-' + product.id"
          class="button-single"
          squared
          variant="dark"
        >
          Choose your fighter
        </b-button>
        <b-popover
          :target="'popover-' + product.id"
          triggers="hover"
          placement="top"
        >
          <b-button
            v-for="(variant, index) in product.variants"
            :key="index"
            class="button-single"
            squared
            variant="dark"
            @click="addToCart(product.id, variant.id)"
          >
            {{ variant.name }}
            {{ variant.weight + product.weightType.name }}
            {{ variant.price + ' RON' }}
          </b-button>
        </b-popover>
      </div>
    </div>
  </b-col>
</template>

<script>
import { getImageForDisplayUrl } from '../../apiService/apiService'

export default {
  name: 'ProductCard',
  props: ['product'],
  data: () => ({ isHovering: false }),

  computed: {
    getImageUrl() {
      return getImageForDisplayUrl(this.product.image.id)
    },
  },
  created() {
    this.sortVariants()
  },
  methods: {
    sortVariants() {
      return this.product.variants.sort((a, b) => a.price - b.price)
    },
    addToCart(productId, variantId) {
      const orderItem = {
        productId: productId,
        variantId: variantId,
      }
      this.$store.dispatch('addItemToCart', orderItem)
    },
  },
}
</script>
<style lang="scss" scoped>
.product {
  &-card {
    display: flex;
    flex-direction: column;
    justify-content: center;
    text-align: center;
  }
  &-body {
    display: flex;
    flex-direction: column;
    justify-content: center;
    text-align: center;
    width: 100%;
    margin-top: 15px;
  }
  &-data {
    align-self: center;
    max-width: 350px;
  }
  &-image {
    height: 290px;
  }
  &-card {
    margin-bottom: 20px;
  }
  &-name {
    font-size: 30px;
    justify-content: space-between;
  }
  &-description {
    font-size: 20px;
    color: #5f5f5f;
    padding-top: 15px;
    padding-bottom: 15px;
    min-height: 110px;
  }
}
.card {
  flex-direction: row;
}
.variants {
  &-single {
    align-self: center;
    width: 350px;
    max-width: 350px;
    &-data {
      margin-bottom: 5px;
      display: flex;
      flex-direction: row;
      justify-content: space-between;
    }
  }
  &-multiple {
    align-self: center;
    width: 350px;
    max-width: 350px;
    &-data {
      margin-bottom: 5px;
      display: flex;
      flex-direction: row;
      justify-content: space-between;
    }
  }
}
.button-single {
  width: 100%;
}
</style>
