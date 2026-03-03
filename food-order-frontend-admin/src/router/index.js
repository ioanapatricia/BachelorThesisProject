import Vue from 'vue'
import VueRouter from 'vue-router'
import { isUserLoggedIn } from '../helpers/authExtensions'

import Home from '../views/Home.vue'
import Auth from '../components/authentication/Auth.vue'

import Products from '../views/Products.vue'
import CreateProduct from '../components/products/CreateProduct.vue'
import ViewEditProduct from '../components/products/ViewEditProduct.vue'

import Categories from '../components/categories/Categories.vue'
import ViewEditCategory from '../components/categories/ViewEditCategory.vue'
import CreateCategory from '../components/categories/CreateCategory.vue'

import Orders from '../components/orders/Orders.vue'
import ViewOrder from '../components/orders/ViewOrder.vue'

import Employees from '../components/employees/Employees.vue'
import CreateEmployee from '../components/employees/CreateEmployee.vue'
import ViewEditEmployee from '../components/employees/ViewEditEmployee.vue'

import SiteSettings from '../components/siteSettings/SiteSettings.vue'

Vue.use(VueRouter)

const routes = [
  {
    path: '/',
    name: 'Home',
    component: Home,
    beforeEnter(to, from, next) {
      if (isUserLoggedIn()) {
        next()
      } else {
        next({
          name: 'Auth',
        })
      }
    },
  },

  {
    path: '/products',
    name: 'Products',
    component: Products,
    beforeEnter(to, from, next) {
      if (isUserLoggedIn()) {
        next()
      } else {
        next({
          name: 'Auth',
        })
      }
    },
  },

  {
    path: '/auth',
    name: 'Auth',
    component: Auth,
    beforeEnter(to, from, next) {
      if (isUserLoggedIn()) {
        next({ name: 'Home' })
      } else {
        next()
      }
    },
  },

  {
    path: '/orders',
    name: 'Orders',
    component: Orders,
    beforeEnter(to, from, next) {
      if (isUserLoggedIn()) {
        next()
      } else {
        next({
          name: 'Auth',
        })
      }
    },
  },

  {
    path: '/categories',
    name: 'Categories',
    component: Categories,
    beforeEnter(to, from, next) {
      if (isUserLoggedIn()) {
        next()
      } else {
        next({
          name: 'Auth',
        })
      }
    },
  },

  {
    path: '/category/:id',
    name: 'ViewEditCategory',
    component: ViewEditCategory,
    beforeEnter(to, from, next) {
      if (isUserLoggedIn()) {
        next()
      } else {
        next({
          name: 'Auth',
        })
      }
    },
  },

  {
    path: '/createCategory',
    name: 'CreateCategory',
    component: CreateCategory,
    beforeEnter(to, from, next) {
      if (isUserLoggedIn()) {
        next()
      } else {
        next({
          name: 'Auth',
        })
      }
    },
  },

  {
    path: '/createProduct',
    name: 'CreateProduct',
    component: CreateProduct,
    beforeEnter(to, from, next) {
      if (isUserLoggedIn()) {
        next()
      } else {
        next({
          name: 'Auth',
        })
      }
    },
  },

  {
    path: '/viewEditProduct/:id',
    name: 'ViewEditProduct',
    component: ViewEditProduct,
    beforeEnter(to, from, next) {
      if (isUserLoggedIn()) {
        next()
      } else {
        next({
          name: 'Auth',
        })
      }
    },
  },

  {
    path: '/viewOrder/:id',
    name: 'ViewOrder',
    component: ViewOrder,
    beforeEnter(to, from, next) {
      if (isUserLoggedIn()) {
        next()
      } else {
        next({
          name: 'Auth',
        })
      }
    },
  },

  {
    path: '/employees',
    name: 'Employees',
    component: Employees,
    beforeEnter(to, from, next) {
      if (isUserLoggedIn()) {
        next()
      } else {
        next({
          name: 'Auth',
        })
      }
    },
  },

  {
    path: '/createEmployee',
    name: 'CreateEmployee',
    component: CreateEmployee,
    beforeEnter(to, from, next) {
      if (isUserLoggedIn()) {
        next()
      } else {
        next({
          name: 'Auth',
        })
      }
    },
  },

  {
    path: '/viewEditEmployee/:id',
    name: 'ViewEditEmployee',
    component: ViewEditEmployee,
    beforeEnter(to, from, next) {
      if (isUserLoggedIn()) {
        next()
      } else {
        next({
          name: 'Auth',
        })
      }
    },
  },

  {
    path: '/siteSettings',
    name: 'SiteSettings',
    component: SiteSettings,
    beforeEnter(to, from, next) {
      if (isUserLoggedIn()) {
        next()
      } else {
        next({
          name: 'Auth',
        })
      }
    },
  },

  {
    path: '/:pathMatch(.*)*',
    beforeEnter(to, from, next) {
      if (isUserLoggedIn()) {
        next({ name: 'Home' })
      } else {
        next({
          name: 'Auth',
        })
      }
    },
  },
]

const router = new VueRouter({
  mode: 'history',
  base: process.env.BASE_URL,
  routes,
})

export default router
