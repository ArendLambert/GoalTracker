// src/utils/jwt.js
import { jwtDecode } from 'jwt-decode'

export function getCookie(name) {
  const value = `; ${document.cookie}`;
  const parts = value.split(`; ${name}=`);
  if (parts.length === 2) return parts.pop().split(';').shift();
  return null;
}

export function decodeJwtToken() {
  const token = getCookie('goida');
  if (!token) {
    console.error('Токен не найден в куки goida');
    return null;
  }

  try {
    const decoded = jwtDecode(token);
    return {
      idUser: decoded.idUser,
      email: decoded.email,
    };
  } catch (error) {
    console.error('Ошибка декодирования JWT:', error);
    return null;
  }
}

export function deleteCookie(name) {
  document.cookie = `${name}=; expires=Thu, 01 Jan 1970 00:00:00 GMT; path=/`;
}