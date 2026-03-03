<template>
  <v-navigation-drawer
    app
    permanent
    expand-on-hover
    :mini-variant.sync="mini"
    dark
  >
    <v-list
      nav
      dense
      class="d-flex flex-column justify-space-between fill-height"
    >
      <div>
        <v-list-item link to="/products">
          <v-list-item-icon>
            <v-icon>mdi-food</v-icon>
          </v-list-item-icon>
          <v-list-item-title>Products</v-list-item-title>
        </v-list-item>
        <v-list-item link to="/orders">
          <v-list-item-icon>
            <v-icon>mdi-order-bool-ascending-variant</v-icon>
          </v-list-item-icon>
          <v-list-item-title>Orders</v-list-item-title>
        </v-list-item>
        <v-list-item link to="/categories">
          <v-list-item-icon>
            <v-icon>mdi-format-list-bulleted-type</v-icon>
          </v-list-item-icon>
          <v-list-item-title>Categories</v-list-item-title>
        </v-list-item>
        <v-list-item link to="/employees">
          <v-list-item-icon>
            <v-icon>mdi-account-group</v-icon>
          </v-list-item-icon>
          <v-list-item-title>Employees</v-list-item-title>
        </v-list-item>
        <v-list-item link to="/siteSettings">
          <v-list-item-icon>
            <v-icon>mdi-cog</v-icon>
          </v-list-item-icon>
          <v-list-item-title>Settings</v-list-item-title>
        </v-list-item>
      </div>
      <div>
        <v-divider class="mx-4"></v-divider>
        <v-list-item link @click="onLogout" class="mt-1">
          <v-list-item-icon>
            <v-icon>mdi-logout-variant</v-icon>
          </v-list-item-icon>
          <v-list-item-title>Logout</v-list-item-title>
        </v-list-item>
      </div>
    </v-list>
  </v-navigation-drawer>
</template>

<script>
export default {
  data: () => ({ mini: true }),
  methods: {
    onLogout() {
      localStorage.removeItem('loggedUser')
      this.$router.push(`/auth`)
    },
  },
}
</script>

<style></style>
