import VueJwtDecode from 'vue-jwt-decode'

export const isUserValid = user => {
  const token = VueJwtDecode.decode(user.token)

  return (
    token.role.toLowerCase().trim() == 'manager' ||
    token.role.toLowerCase().trim() == 'owner'
  )
}

export const isUserLoggedIn = () => {
  const user = getLoggedUser()
  if (!user) {
    return false
  }
  return isUserValid(user)
}

export const getLoggedUser = () => {
  return JSON.parse(localStorage.getItem('loggedUser'))
}
