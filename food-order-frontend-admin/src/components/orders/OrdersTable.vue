<template>
  <v-data-table
    :headers="headers"
    :items="orders"
    :loading="loading"
    :search="search"
    :items-per-page="15"
    sort-by="calories"
    class="elevation-1"
  >
    <template v-slot:top>
      <v-toolbar flat>
        <v-toolbar-title>Orders History</v-toolbar-title>
        <v-divider class="mx-4" inset vertical></v-divider>
        <v-spacer></v-spacer>
        <v-text-field
          v-model="search"
          append-icon="mdi-magnify"
          label="Search"
          single-line
          hide-details
        ></v-text-field>
      </v-toolbar>
    </template>

    <template v-slot:[`item.createdOn`]="{ item }">
      {{ formatDate(item.createdOn) }}
    </template>

    <template v-slot:[`item.completedOn`]="{ item }">
      {{ formatDate(item.completedOn) }}
    </template>

    <template v-slot:[`item.address`]="{ item }">
      {{
        item.address.city +
          ', ' +
          item.address.street +
          ' ' +
          item.address.streetNumber
      }}
    </template>

    <template v-slot:[`item.actions`]="{ item }">
      <v-icon small @click="onView(item.id)">
        mdi-eye
      </v-icon>
    </template>
  </v-data-table>
</template>

<script>
import moment from 'moment'

export default {
  data: () => ({
    search: '',
    loading: false,
    headers: [
      { text: 'Created', value: 'createdOn' },
      { text: 'Completed', value: 'completedOn' },
      { text: 'Status', value: 'status.name' },
      { text: 'Payment type', value: 'paymentType.displayName' },
      { text: 'Location', value: 'address' },
      { text: 'Email', value: 'user.email' },
      { text: 'Phone', value: 'user.phoneNumber' },
      { text: 'Products Count', value: 'products.length' },
      { text: 'Total Price', value: 'totalPrice' },
      { text: 'Actions', value: 'actions', sortable: false },
    ],
  }),

  created() {
    this.init()
  },

  computed: {
    orders() {
      return this.$store.getters.getOrders
    },
  },

  methods: {
    init() {
      if (this.orders.length > 0) return
      this.loading = true
      this.$store.dispatch('setOrdersWithApiDataAction').then(() => {
        this.loading = false
      })
    },
    formatDate(value) {
      if (value) {
        return moment(String(value)).format('hh:mm - DD.MM.YYYY')
      }
    },
    onView(orderId) {
      this.$router.push(`/viewOrder/${orderId}`)
    },
  },
}
</script>

<style></style>
