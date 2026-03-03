export default {
  getCategoriesWithProducts(store) {
    return store.categoriesWithProducts
  },
  getProducts(store) {
    return store.products
  },
  getCartItems(store) {
    return store.cartItems
  },
  getCartProducts(store) {
    return store.cartProducts
  },
  getLoggedUser(store) {
    return store.loggedUser
  },
  getSettings(store) {
    return store.settings
  },
}
