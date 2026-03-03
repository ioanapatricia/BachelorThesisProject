<template>
  <v-card class="ma-5" :loading="loading">
    <v-overlay :value="overlay">
      <v-progress-circular indeterminate size="64" />
    </v-overlay>
    <snack-bar :snackbar="snackbar" />

    <v-card-text>
      <p class="text-h5 grey--text">Settings</p>
      <v-form
        ref="form"
        v-model="valid"
        lazy-validation
        v-if="siteSettingsToUpdate"
      >
        <v-text-field
          v-model="siteSettingsToUpdate.phoneNumber"
          :rules="phoneNumberRules"
          label="Phone Number"
          prepend-icon="mdi-phone"
          required
          :disabled="viewOnlyMode"
        />

        <v-text-field
          v-model="siteSettingsToUpdate.schedule"
          :rules="scheduleRules"
          label="Schedule"
          prepend-icon="mdi-calendar"
          required
          :disabled="viewOnlyMode"
        />

        <v-text-field
          v-model="siteSettingsToUpdate.address"
          :rules="addressRules"
          label="Address"
          prepend-icon="mdi-map-marker"
          required
          :disabled="viewOnlyMode"
        />

        <v-text-field
          v-model="siteSettingsToUpdate.fax"
          :rules="faxRules"
          label="Fax"
          prepend-icon="mdi-fax"
          required
          :disabled="viewOnlyMode"
        />

        <v-text-field
          v-model="siteSettingsToUpdate.feedbackEmail"
          :rules="feedbackEmailRules"
          label="Feedback Email"
          prepend-icon="mdi-at"
          required
          :disabled="viewOnlyMode"
        />

        <v-text-field
          v-model="siteSettingsToUpdate.businessEmail"
          :rules="businessEmailRules"
          label="Business Email"
          prepend-icon="mdi-email"
          required
          :disabled="viewOnlyMode"
        />

        <v-text-field
          v-model="siteSettingsToUpdate.facebookUrl"
          :rules="facebookUrlRules"
          label="Facebook Url"
          prepend-icon="mdi-facebook"
          required
          :disabled="viewOnlyMode"
        />

        <v-text-field
          v-model="siteSettingsToUpdate.instagramUrl"
          :rules="instagramUrlRules"
          label="Instagram Url"
          prepend-icon="mdi-instagram"
          required
          :disabled="viewOnlyMode"
        />

        <v-row>
          <v-col>
            <v-btn
              :loading="loading"
              v-if="viewOnlyMode"
              :disabled="!valid"
              color="success"
              class="mr-4"
              @click="switchToEditMode"
            >
              Toggle Edit Mode
            </v-btn>

            <v-btn
              v-if="!viewOnlyMode"
              :disabled="!valid"
              color="success"
              class="mr-4"
              @click="onSubmit"
            >
              Submit
            </v-btn>
          </v-col>

          <v-col class="text-right">
            <v-btn
              v-if="!viewOnlyMode"
              color="error"
              class="mr-4"
              @click="switchToViewMode"
            >
              Cancel
            </v-btn>
            <v-btn
              v-if="viewOnlyMode && !loading"
              color="error"
              class="mr-4"
              to="/"
            >
              Back
            </v-btn>
          </v-col>
        </v-row>
      </v-form>
    </v-card-text>
  </v-card>
</template>

<script>
import SnackBar from '../common/SnackBar'
import {
  getSiteSettings,
  updateSiteSettings,
} from '../../apiService/apiService'
export default {
  name: 'SiteSettings',
  components: { SnackBar },
  data: () => ({
    snackbar: {
      text: '',
      color: '',
      show: false,
    },
    loading: false,
    overlay: false,
    viewOnlyMode: true,
    valid: true,
    siteSettingsToUpdate: {
      phoneNumber: '',
      schedule: '',
      address: '',
      fax: '',
      feedbackEmail: '',
      businessEmail: '',
      facebookUrl: '',
      instagramUrl: '',
    },
    phoneNumberRules: [
      v => !!v || 'Phone is required',
      v => (v && v.length >= 10) || 'Must be a valid Phone Number',
    ],
    scheduleRules: [v => !!v || 'Schedule is required'],
    addressRules: [v => !!v || 'Address is required'],
    faxRules: [v => !!v || 'Fax is required'],
    feedbackEmailRules: [
      v => !!v || 'Feedback Email is required',
      v => /\S+@\S+\.\S+/.test(v) || 'Email is not valid',
    ],
    businessEmailRules: [
      v => !!v || 'Business Email is required',
      v => /\S+@\S+\.\S+/.test(v) || 'Email is not valid',
    ],
    facebookUrlRules: [v => !!v || 'Facebook Url is required'],
    instagramUrlRules: [v => !!v || 'Instagram Url is required'],
  }),

  created() {
    this.initForm()
  },

  computed: {
    siteSettings() {
      return this.$store.getters.getSettings
    },
  },

  methods: {
    async initForm() {
      this.loading = true
      if (this.siteSettings == null) {
        await this.getAndSetSiteSettingsFromApi().then(() => {
          this.loading = false
        })
      } else {
        this.setSiteSettings(this.siteSettings)
      }
      this.loading = false
    },

    async getAndSetSiteSettingsFromApi() {
      const response = await getSiteSettings()
      this.$store.dispatch('setSiteSettingsAction', response.data)
      this.setSiteSettings(response.data)
    },

    setSiteSettings(settings) {
      this.siteSettingsToUpdate = JSON.parse(JSON.stringify(settings))
    },

    switchToEditMode() {
      this.viewOnlyMode = false
    },

    switchToViewMode() {
      this.viewOnlyMode = true
      this.setSiteSettings(this.siteSettings)
    },

    async onSubmit() {
      this.$refs.form.validate()
      this.overlay = true

      await updateSiteSettings(this.siteSettingsToUpdate)
        .then(response => {
          if (response.status >= 200 && response.status < 300) {
            this.initSnackbarSuccess()
            this.getAndSetSiteSettingsFromApi()
            this.viewOnlyMode = true
          }
        })
        .catch(error => {
          this.initSnackbarFailure(error.response.data)
        })
        .finally(() => {
          this.overlay = false
        })
    },

    isPristine() {
      return (
        JSON.stringify(this.siteSettings) ===
        JSON.stringify(this.siteSettingsToUpdate)
      )
    },

    initSnackbarSuccess() {
      this.snackbar = {
        text: 'Settings updated successfully!',
        color: 'success',
        show: true,
      }
    },

    initSnackbarFailure(errorMessage) {
      this.snackbar = {
        text: `Failed to update settings, reason: ${errorMessage}`,
        color: 'error',
        show: true,
      }
    },
  },
}
</script>
