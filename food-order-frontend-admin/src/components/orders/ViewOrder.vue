<template>
  <v-card class="ma-5" v-if="order" :loading="loading">
    <v-card-text>
      <h1 class="mb-5">{{ order.user.firstName }}'s order</h1>
      <v-form ref="form">
        <v-text-field
          v-model="orderForDisplay.id"
          label="Id"
          prepend-icon="mdi-sim-outline"
          :disabled="true"
        />
        <v-text-field
          v-model="orderForDisplay.totalPrice"
          label="Total Price"
          prepend-icon="mdi-currency-usd"
          :disabled="true"
        />
        <v-text-field
          v-model="orderForDisplay.createdOn"
          label="Created on"
          prepend-icon="mdi-sort-calendar-descending"
          :disabled="true"
        />
        <v-text-field
          v-model="orderForDisplay.completedOn"
          label="Completed on"
          prepend-icon="mdi-sort-calendar-ascending"
          :disabled="true"
        />
        <v-text-field
          v-model="orderForDisplay.status.name"
          label="Status"
          prepend-icon="mdi-trophy-outline"
          :disabled="true"
        />

        <v-text-field
          v-model="orderForDisplay.paymentType.displayName"
          label="Payment type"
          prepend-icon="mdi-cash-multiple"
          :disabled="true"
        />

        <h2 class="mb-4">Customer info</h2>

        <v-text-field
          v-model="orderForDisplay.user.firstName"
          label="First Name"
          prepend-icon="mdi-alphabetical"
          :disabled="true"
        />

        <v-text-field
          v-model="orderForDisplay.user.lastName"
          label="Last Name"
          prepend-icon="mdi-alphabetical"
          :disabled="true"
        />

        <v-text-field
          v-model="orderForDisplay.user.userName"
          label="Username"
          prepend-icon="mdi-account-outline"
          :disabled="true"
        />

        <v-text-field
          v-model="orderForDisplay.user.email"
          label="Email"
          prepend-icon="mdi-at"
          :disabled="true"
        />

        <v-text-field
          v-model="orderForDisplay.user.phoneNumber"
          label="Phone Number"
          prepend-icon="mdi-phone-outline"
          :disabled="true"
        />

        <v-text-field
          v-model="orderForDisplay.addressForDisplay"
          label="Address"
          prepend-icon="mdi-map-marker-outline"
          :disabled="true"
        />

        <h2 class="mb-4">Products</h2>

        <v-card
          v-for="(product, index) in orderForDisplay.products"
          :key="index"
          class="ma-4"
        >
          <v-card-text>
            <v-text-field
              v-model="product.name"
              label="Name"
              prepend-icon="mdi-alphabetical"
              :disabled="true"
            />

            <v-text-field
              v-model="product.categoryName"
              label="Category"
              prepend-icon="mdi-shape-outline"
              :disabled="true"
            />

            <v-text-field
              v-model="product.salePercentage"
              label="Sale percentage"
              prepend-icon="mdi-percent-outline"
              :disabled="true"
            />

            <v-text-field
              v-model="product.weight"
              label="Weight"
              prepend-icon="mdi-weight-kilogram"
              :disabled="true"
            />

            <v-text-field
              v-model="product.price"
              label="Price"
              prepend-icon="mdi-currency-usd"
              :disabled="true"
            />
          </v-card-text>
        </v-card>

        <v-row>
          <v-col></v-col>
          <v-col class="text-right">
            <v-btn color="error" class="mr-4" to="/orders">
              Back
            </v-btn>
          </v-col>
        </v-row>
      </v-form>
    </v-card-text>
  </v-card>
</template>

<script>
import moment from 'moment'

import { getOrder } from '../../apiService/apiService'

export default {
  data: () => ({
    loading: true,
    order: null,
    orderForDisplay: null,
  }),

  created() {
    this.init()
  },

  methods: {
    async init() {
      const response = await getOrder(this.$route.params.id)
      this.order = response.data
      this.orderForDisplay = response.data
      this.processDates()
      this.processAddress()
      this.processSalePercentage()
      this.loading = false
    },

    processSalePercentage() {
      this.orderForDisplay.products.forEach(product => {
        product.salePercentage = product.salePercentage
          ? product.salePercentage
          : 'None'
      })
    },

    processDates() {
      this.orderForDisplay.createdOn = this.formatDate(this.order.createdOn)
      this.orderForDisplay.completedOn = this.formatDate(this.order.completedOn)
    },

    processAddress() {
      let address = ''

      if (this.order.address.city)
        address += ` ${this.order.address.county} city`

      if (this.order.address.county)
        address += `, ${this.order.address.county} county`

      if (this.order.address.sector)
        address += `, ${this.order.address.county} sector`

      if (this.order.address.street)
        address += `, ${this.order.address.street} street`

      if (this.order.address.streetNumber)
        address += `, nr. ${this.order.address.streetNumber}`

      if (this.order.address.entrance)
        address += `, ${this.order.address.entrance} entrance`

      if (this.order.address.building)
        address += `, ${this.order.address.building} building`

      if (this.order.address.floor)
        address += `, ${this.order.address.floor} floor`

      if (this.order.address.apartmentNumber)
        address += `, ${this.order.address.apartmentNumber} apartment number`

      this.orderForDisplay['addressForDisplay'] = address
    },

    formatDate(value) {
      if (value) {
        return moment(String(value)).format('hh:mm - DD.MM.YYYY')
      }
    },
  },
}
</script>

<style></style>
