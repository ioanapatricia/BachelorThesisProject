<template>
  <v-snackbar
    v-model="snackbar.show"
    :color="snackbar.color"
    outlined
    dark
    transition="slide-y-transition"
  >
    {{ snackbar.text }}

    <template v-slot:action="{ attrs }">
      <v-btn
        :color="snackbar.color"
        text
        v-bind="attrs"
        @click="snackbar.show = false"
      >
        Close
      </v-btn>
    </template>
  </v-snackbar>
</template>

<script>
export default {
  props: ['snackbar'],
}
</script>
