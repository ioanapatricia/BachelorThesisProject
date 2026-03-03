export default {
  setCategoriesWithProducts(state, payload) {
    state.categoriesWithProducts = payload
  },
  setProducts(state, payload) {
    state.products = payload
  },
  setSettings(state, payload) {
    state.settings = payload
  },
  pushSingleItemToCart(state, payload) {
    state.cartItems.push(payload)
  },

  deleteAllCartItems(state) {
    state.cartItems = []
  },

  setCartProducts(state, payload) {
    state.cartProducts = payload
  },
  setDeleteItemFromCart(state, payload) {
    state.cartItems = payload
  },
  setLoggedUser(state, payload) {
    state.loggedUser = payload
  },
}
