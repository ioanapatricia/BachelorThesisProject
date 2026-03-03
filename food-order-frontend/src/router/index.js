import Vue from 'vue'
import VueRouter from 'vue-router'
import Home from '../views/Home.vue'
import PathNotFound from '../views/PathNotFound.vue'
import Cart from '../components/Orders/Cart.vue'
import OrderCompleted from '../components/Orders/OrderCompleted.vue'
import ErrorPage from '../views/Error.vue'

Vue.use(VueRouter)

const routes = [
  {
    path: '/',
    name: 'Home',
    component: Home,
    // component: () =>
    //   import(/* webpackChunkName: "about" */ '../views/About.vue'),
  },
  { path: '/cart', name: 'Cart', component: Cart },
  { path: '/orderCompleted', component: OrderCompleted },
  { path: '/error', component: ErrorPage },
  { path: '/:pathMatch(.*)*', component: PathNotFound },
]

const router = new VueRouter({
  mode: 'history',
  routes,
  scrollBehavior() {
    return { x: 0, y: 0 }
  },
})

export default router
