<template>
  <b-container class="my-5 cart-summary-container">
    <div class="total mb-5">Total: {{ totalPrice }} RON</div>
    <b-dropdown
      class="dropdown-actions btn-lg rounded-0 mr-4"
      :text="
        paymentType ? `Payment: ${paymentType.displayName}` : 'Payment method'
      "
      variant="warning"
      squared
    >
      <b-dropdown-item
        variant="dark"
        style="width:300px;"
        v-for="(type, index) in paymentTypes"
        :key="index"
        @click="paymentType = type"
      >
        {{ type.displayName }}
      </b-dropdown-item>
    </b-dropdown>

    <b-button
      v-b-modal.modal-center
      :disabled="!paymentType"
      flat
      variant="dark"
      squared
      size="lg"
      class="actions"
      @click="onSubmit"
    >
      Next
      <b-icon icon="arrow-right" class="back-arrow ml-1" />
    </b-button>

    <auth v-if="order" :order="order" />
  </b-container>
</template>

<script>
import Auth from '../Auth/Auth'
import { getPaymentTypes } from '../../apiService/apiService'

export default {
  name: 'CartSummary',
  data: () => ({
    paymentType: null,
    paymentTypes: [],
    order: null,
  }),

  components: { Auth },

  created() {
    this.onInit()
  },

  watch: {
    paymentType: function() {
      console.log(this.paymentType)
    },
  },

  methods: {
    async onInit() {
      const response = await getPaymentTypes()
      this.paymentTypes = response.data
      console.log(this.paymentTypes)
    },
    onSubmit() {
      this.order = {
        paymentTypeId: this.paymentType.id,
        productFilters: this.$store.getters.getCartItems,
      }
      console.log('onSubmit', this.order)
    },
  },

  computed: {
    totalPrice() {
      const products = this.$store.getters.getCartProducts
      var sum = 0
      products.forEach(product => {
        sum += product.variants[0].price
      })
      return sum
    },
  },
}
</script>

<style lang="scss">
.cart-summary {
  &-container {
    text-align: center;

    .dropdown-actions {
      width: 300px;
      height: 50px;
      padding: 0;
    }
    .actions {
      width: 300px;
      height: 50px;
    }
    .back-arrow {
      transition: transform 0.3s ease;
    }
    .actions:hover .back-arrow {
      transform: translate(5px, 0);
    }
  }
}
.total {
  font-size: 36px;
}
</style>
