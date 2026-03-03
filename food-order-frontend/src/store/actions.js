import {
  getCategoriesWithProducts,
  getProducts,
  getSettings,
} from '../apiService/apiService'

export default {
  async fetchCategoriesWithProducts(context) {
    const response = await getCategoriesWithProducts()
    context.commit('setCategoriesWithProducts', response.data)
  },

  async fetchProductsFromApi(context) {
    const response = await getProducts()
    context.commit('setProducts', response.data)
  },

  async fetchSettingsFromApi(context) {
    const response = await getSettings()
    context.commit('setSettings', response.data)
  },

  addItemToCart({ commit, dispatch }, payload) {
    commit('pushSingleItemToCart', payload)
    dispatch('setFilteredProducts')
  },

  deleteCartProduct({ commit, dispatch, getters }, payload) {
    const filteredCartItems = getters.getCartItems
    filteredCartItems.splice(
      filteredCartItems.findIndex(
        item =>
          item.productId == payload.productId &&
          item.variantId == payload.variantId,
      ),
      1,
    )

    commit('setDeleteItemFromCart', filteredCartItems)
    dispatch('setFilteredProducts')
  },

  deleteAllCartProducts({ commit, dispatch }) {
    commit('deleteAllCartItems')
    dispatch('setFilteredProducts')
  },

  setFilteredProducts({ commit, getters }) {
    const products = getters.getProducts
    const cartItems = getters.getCartItems

    let result = []
    cartItems.forEach(cartItem => {
      const findResult = products.find(
        product => product.id == cartItem.productId,
      )
      let correspondingItem = JSON.parse(JSON.stringify(findResult))

      correspondingItem.variants = correspondingItem.variants.filter(
        variant => variant.id == cartItem.variantId,
      )
      result.push(correspondingItem)
    })
    commit('setCartProducts', result)
  },
  login(context, payload) {
    context.commit('setLoggedUser', payload)
  },
}
