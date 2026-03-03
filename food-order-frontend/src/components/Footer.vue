<template>
  <b-container class="footer-component">
    <b-row class="footer-row">
      <b-col cols="8">
        <div class="footer-textbox">
          <div class="textbox-title">Influence Reality:</div>
          <b-link :href="'tel:' + settings ? settings.phoneNumber : ''">
            {{ settings ? settings.phoneNumber : '' }}
          </b-link>
        </div>

        <div class="footer-textbox">
          <div class="textbox-title">Address:</div>
          <div v-if="settings" class="textbox-data">{{ settings.address }}</div>
        </div>

        <div class="footer-textbox">
          <div class="textbox-title">Fax machine:</div>
          <div v-if="settings" class="textbox-data">{{ settings.fax }}</div>
        </div>

        <div class="footer-textbox">
          <div class="textbox-title">Feedback:</div>
          <div class="textbox-data">
            <b-link v-if="settings" :href="'mailto:' + settings.feedbackEmail">
              {{ settings.feedbackEmail }}
            </b-link>
          </div>
        </div>

        <div class="footer-textbox">
          <div class="textbox-title">
            For commercial proposals and cooperation:
          </div>
          <b-link v-if="settings" :href="'mailto:' + settings.businessEmail">
            {{ settings.businessEmail }}
          </b-link>
        </div>

        <b-row class="social">
          <b-col cols="6">
            <b-link
              v-if="settings"
              :href="settings.facebookUrl"
              target="_blank"
            >
              <b-icon-facebook />
              Facebook
            </b-link>
          </b-col>
          <b-col cols="6">
            <b-link
              v-if="settings"
              :href="settings.instagramUrl"
              target="_blank"
            >
              <b-icon-instagram />
              Instagram
            </b-link>
          </b-col>
        </b-row>
      </b-col>

      <b-col cols="4">
        <div class="copyright">
          <img
            :src="require('../assets/img/footer-logo.svg')"
            alt="Footer Logo"
          />
          All rights reserved BuddhaPizza TM
        </div>
      </b-col>
    </b-row>
  </b-container>
</template>

<script>
export default {
  name: 'Footer',
  computed: {
    settings() {
      return this.$store.getters.getSettings
    },
  },
}
</script>

<style lang="scss" scoped>
.footer {
  &-component {
    background-image: url(../assets/img/footer-vegetables.svg);
    background-position-x: 70%;
    background-position-y: 100%;
    background-size: 400px;
    background-repeat: no-repeat;
  }
  &-row {
    padding: 130px 0;

    .social {
      padding-top: 35px;
      font-size: 22px;

      svg {
        margin-right: 5px;
      }
    }
    .copyright {
      text-align: center;
      color: #615f5b;
      img {
        margin-bottom: 15px;
      }
    }
  }
  &-textbox {
    font-size: 22px;
    margin-bottom: 35px;

    .textbox {
      &-title {
        color: #615f5b;
        padding-bottom: 15px;
      }
    }
  }
}
a {
  text-decoration: none;
  color: #fff;
  &:hover {
    color: #615f5b;
  }
}
</style>
