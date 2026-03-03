import Vue from 'vue'
import Vuex from 'vuex'

import {
  getProducts,
  getOrders,
  getCategories,
  getImageForDisplayUrl,
  getEmployees,
  getRoles,
} from '../apiService/apiService'

Vue.use(Vuex)

export default new Vuex.Store({
  state: {
    products: [],
    orders: [],
    categories: [],
    employees: [],
    roles: [],
    settings: null,
  },
  actions: {
    async setProductsActionWithApiData({ commit }) {
      const response = await getProducts()
      if (response.status == 200){
        commit('setProducts', response.data)
      } 
    },

    async setOrdersWithApiDataAction({ commit }) {
      const response = await getOrders()
      if (response.status == 200){
        commit('setOrders', response.data)
      } 
    },

    async setCategoriesWithApiDataAction({ commit }) {
      const response = await getCategories()
      if (response.data) {
        response.data.forEach(category => {
          category.logo['url'] = getImageForDisplayUrl(category.logo.id)
        })
      }
      commit('setCategories', response.data)
    },

    async setEmployeesWithApiDataAction({ commit }) {
      const response = await getEmployees()
      commit('setEmployees', response.data)
    },

    async setRolesWithApiDataAction({ commit }) {
      const response = await getRoles()
      commit('setRoles', response.data)
    },

    setLoggedUserAction(_, payload) {
      const loggedUser = JSON.stringify(payload)
      localStorage.setItem('loggedUser', loggedUser)
    },

    setSiteSettingsAction({ commit }, payload) {
      commit('setSiteSettings', payload)
    },
  },
  mutations: {
    setProducts(state, payload) {
      state.products = payload
    },

    setOrders(state, payload) {
      state.orders = payload
    },

    setCategories(state, payload) {
      state.categories = payload
    },

    setEmployees(state, payload) {
      state.employees = payload
    },

    setSiteSettings(state, payload) {
      state.settings = payload
    },

    setRoles(state, payload) {
      state.roles = payload
    },
  },
  getters: {
    getCategories(store) {
      return store.categories
    },

    getProducts(store) {
      return store.products
    },

    getOrders(store) {
      return store.orders
    },

    getEmployees(store) {
      return store.employees
    },

    getSettings(store) {
      return store.settings
    },

    getRoles(store) {
      return store.roles
    },
  },
  modules: {},
})
