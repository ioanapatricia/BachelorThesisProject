<template>
  <div class="product-card py-5">
    <div class="product-card-left-side">
      <b-img
        :class="{ bambambam: product.category.name == 'Pizza' }"
        :src="getImage(product.image.id)"
        :alt="product.name"
        width="250"
        height="250"
      />
      <div class="product-card-description">
        <p>
          {{ product.name }}
        </p>
        <p>
          {{ 'Variant: ' + product.variants[0].name }}
        </p>
        <p>
          {{ 'Weight: ' + product.variants[0].weight }}
        </p>
      </div>
    </div>

    <div class="product-card-right-side">
      <p>{{ product.variants[0].price + ' RON' }}</p>
      <b-icon
        icon="plus"
        class="product-card-discard"
        rotate="45"
        scale="2"
        @click="onDelete"
      />
    </div>
  </div>
</template>

<script>
import { getImageForDisplayUrl } from '../../apiService/apiService'
export default {
  name: 'ProductCard',
  props: ['product'],
  data: () => ({ isHovering: false }),
  methods: {
    getImage(id) {
      return getImageForDisplayUrl(id)
    },
    onDelete() {
      const productToDelete = {
        productId: this.product.id,
        variantId: this.product.variants[0].id,
      }
      this.$store.dispatch('deleteCartProduct', productToDelete)
    },
  },
}
</script>

<style scoped lang="scss">
.product-card {
  display: flex;
  flex-direction: row;
  justify-content: space-between;
  border-bottom: 1px solid #e7e7e7;

  &-left-side {
    min-height: 100%;
    display: flex;
    flex-direction: row;
    justify-content: space-between;
  }
  &-description {
    position: relative;
    z-index: 1;
    display: flex;
    flex-direction: column;
    justify-content: center;
    margin-left: 30px;
    p {
      font-size: 18px;
      margin-bottom: 30px;
    }
    p:first-child {
      font-size: 24px;
    }
  }
  &-right-side {
    min-height: 100%;
    display: flex;
    flex-direction: row;
    justify-content: center;
    align-items: center;
    font-size: 24px;
  }
  &-discard {
    margin-left: 40px;
    cursor: pointer;
    transition: color 0.2s;
    &:hover {
      color: rgba(0, 0, 0, 0.65);
    }
  }
}
.bambambam {
  position: relative;
  animation: rotation 10s infinite linear;
}
@keyframes rotation {
  from {
    transform: rotate(0deg);
  }
  to {
    transform: rotate(359deg);
  }
}
</style>
