import request from '@/utils/request';

export function getAllUsers() {
  return request({
    url: '/api/users/getAllUser',
    method: 'get'
  });
}
export function getCurUser() {
  return request({
    url: '/api/users',
    method: 'get'
  });
}
export function getUser(userId) {
  return request({
    url: '/api/users/'.concat(userId),
    method: 'get'
  });
}
export function updateCurUser(data) {
  return request({
    url: '/api/users',
    method: 'patch',
    data
  });
}

export function createUser(data) {
  return request({
    url: '/api/users',
    method: 'post',
    data
  });
}

export function updateUser(userId, data) {
  return request({
    url: '/api/users'.concat(userId),
    method: 'patch',
    data
  });
}
