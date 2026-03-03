import axios from 'axios'
import store from '../store'

export const getImageForDisplayUrl = id => {
  return `http://localhost:5000/api/images/${id}`
}

export const getProducts = async () => {
  return await axios({
    method: 'get',
    url: 'http://localhost:5000/api/products',
  })
}

export const getCategoriesWithProducts = async () => {
  return await axios({
    method: 'get',
    url: 'http://localhost:5000/api/categories/products',
  })
}

export const getPaymentTypes = async () => {
  return await axios({
    method: 'get',
    url: 'http://localhost:5000/api/paymentTypes',
  })
}

export const login = async loginDetails => {
  return await axios({
    method: 'post',
    url: 'http://localhost:5000/api/authentication',
    data: loginDetails,
  })
}

export const register = async registerDetails => {
  return await axios({
    method: 'post',
    url: 'http://localhost:5000/api/registration',
    data: registerDetails,
  })
}

export const createOrder = async order => {
  return await axios({
    method: 'post',
    url: `http://localhost:5000/api/orders`,
    data: order,
    headers: { Authorization: 'Bearer ' + store.getters.getLoggedUser.token },
  })
}

export const getSettings = async () => {
  return await axios({
    method: 'get',
    url: 'http://localhost:5000/api/settings',
  })
}
