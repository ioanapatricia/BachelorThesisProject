<template>
  <div class="commingSoonContainer">
    <b-container class="soon-container">
      <b-row>
        <b-col>
          <img
            :src="require('../assets/img/Budda_soon.png')"
            alt="Budda Soon Image"
            class="budda mx-auto"
          />
        </b-col>
      </b-row>
    </b-container>
  </div>
</template>

<script>
export default {
  name: 'CommingSoon',
}
</script>

<style lang="scss" scoped>
.commingSoonContainer {
  width: 100%;
  background-color: #fff;
}
.soon-container {
  .col {
    text-align: center;
  }
}
.budda {
  height: 250px;
  margin: 45px auto;
}
</style>
