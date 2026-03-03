<template>
  <b-container fluid>
    <b-row class="my-5">
      <b-col cols="4">
        <b-btn text class="back-button" to="/">
          <b-icon-arrow-left class="back-arrow" />
          Back to browse
        </b-btn>
      </b-col>
      <b-col cols="4" class="bambam">
        <h1>{{ text }}</h1>
      </b-col>
    </b-row>
  </b-container>
</template>

<script>
export default {
  name: 'CartHeader',
  props: ['text'],
}
</script>

<style scoped lang="scss">
.bambam {
  display: flex;
  justify-content: center;
  align-content: center;
  h1 {
    font-size: 36px;
  }
}
.back-button {
  background-color: rgba(0, 0, 0, 0);
  color: #7d7d7d;
  border: 0;
}
.back-arrow {
  transition: transform 0.3s ease;
}
.back-button:hover .back-arrow {
  transform: translate(-5px, 0);
}
</style>
