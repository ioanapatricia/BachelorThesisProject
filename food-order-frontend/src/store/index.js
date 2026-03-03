import Vue from 'vue'
import Vuex from 'vuex'

import actions from './actions.js'
import mutations from './mutations.js'
import getters from './getters.js'

Vue.use(Vuex)

export default new Vuex.Store({
  state: {
    categoriesWithProducts: [],
    products: [],
    cartItems: [],
    cartProducts: [],
    loggedUser: null,
    settings: {},
  },
  mutations,
  actions,
  getters,
  modules: {},
})
