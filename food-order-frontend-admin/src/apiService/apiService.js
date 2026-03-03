import axios from 'axios'
import { getLoggedUser } from '../helpers/authExtensions'

export const getProducts = async () => {
  return await axios({
    method: 'get',
    url: 'http://localhost:5000/api/products',
  })
}

export const getProduct = async id => {
  return await axios({
    method: 'get',
    url: `http://localhost:5000/api/products/${id}`,
  })
}

export const createProduct = async product => {
  return await axios({
    method: 'post',
    url: `http://localhost:5000/api/products`,
    data: product,
    headers: { Authorization: 'Bearer ' + getLoggedUser().token },
  })
}

export const updateProduct = async (id, product) => {
  return await axios({
    method: 'put',
    url: `http://localhost:5000/api/products/${id}`,
    data: product,
    headers: { Authorization: 'Bearer ' + getLoggedUser().token },
  })
}

export const deleteProduct = async id => {
  return await axios({
    method: 'delete',
    url: `http://localhost:5000/api/products/${id}`,
    headers: { Authorization: 'Bearer ' + getLoggedUser().token },
  })
}

export const getOrders = async () => {
  return await axios({ 
    method: 'get', 
    url: `http://localhost:5000/api/orders`, 
    headers: { Authorization: 'Bearer ' + getLoggedUser().token }, 
  })
}

export const getOrder = async id => {
  return await axios({
    method: 'get',
    url: `http://localhost:5000/api/orders/${id}`,
    headers: { Authorization: 'Bearer ' + getLoggedUser().token },
  })
}

export const getCategories = async () => {
  return await axios({
    method: 'get',
    url: 'http://localhost:5000/api/categories',
  })
}

export const getCategory = async id => {
  return await axios({
    method: 'get',
    url: `http://localhost:5000/api/categories/${id}`,
  })
}

export const createCategory = async category => {
  return await axios({
    method: 'post',
    url: `http://localhost:5000/api/categories`,
    data: category,
    headers: { Authorization: 'Bearer ' + getLoggedUser().token },
  })
}

export const updateCategory = async (id, category) => {
  return await axios({
    method: 'put',
    url: `http://localhost:5000/api/categories/${id}`,
    data: category,
    headers: { Authorization: 'Bearer ' + getLoggedUser().token },
  })
}

export const deleteCategory = async id => {
  return await axios({
    method: 'delete',
    url: `http://localhost:5000/api/categories/${id}`,
    headers: { Authorization: 'Bearer ' + getLoggedUser().token },
  })
}

export const getSiteSettings = async () => {
  return await axios({
    method: 'get',
    url: `http://localhost:5000/api/settings`,
  })
}

export const getEmployees = async () => {
  return await axios({
    method: 'get',
    url: `http://localhost:5000/api/employees`,
    headers: { Authorization: 'Bearer ' + getLoggedUser().token },
  })
}

export const getEmployee = async (id) => {
  return await axios({
    method: 'get',
    url: `http://localhost:5000/api/employees/${id}`,
    headers: { Authorization: 'Bearer ' + getLoggedUser().token },
  })
}

export const createEmployee = async employee => {
  return await axios({
    method: 'post',
    url: `http://localhost:5000/api/employees`,
    data: employee,
    headers: { Authorization: 'Bearer ' + getLoggedUser().token },
  })
}

export const updateEmployee = async (id, employee) => {
  return await axios({
    method: 'put',
    url: `http://localhost:5000/api/employees/${id}`,
    data: employee,
    headers: { Authorization: 'Bearer ' + getLoggedUser().token },
  })
}

export const deleteEmployee = async id => {
  return await axios({
    method: 'delete',
    url: `http://localhost:5000/api/employees/${id}`,
    headers: { Authorization: 'Bearer ' + getLoggedUser().token },
  })
}

export const updateSiteSettings = async siteSettings => {
  return await axios({
    method: 'put',
    url: `http://localhost:5000/api/settings`,
    data: siteSettings,
    headers: { Authorization: 'Bearer ' + getLoggedUser().token },
  })
}

export const getWeightTypes = async () => {
  return await axios({
    method: 'get',
    url: 'http://localhost:5000/api/weightTypes',
  })
}

export const getRoles = async () => {
  return await axios({
    method: 'get',
    url: `http://localhost:5000/api/roles`,
    headers: { Authorization: 'Bearer ' + getLoggedUser().token },
  })
}

export const getImageForDisplayUrl = id => {
  return `http://localhost:5000/api/images/${id}`
}

export const getImageAsBase64String = async id => {
  return await axios({
    method: 'get',
    url: `http://localhost:5000/api/images/asbase64/${id}`,
  })
}

export const login = async loginData => {
  return await axios({
    method: 'post',
    url: `http://localhost:5000/api/authentication`,
    data: loginData,
  })
}
