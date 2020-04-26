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
export function updateCurUser(data) {
  return request({
    url: '/api/users',
    method: 'patch',
    data
  });
}
