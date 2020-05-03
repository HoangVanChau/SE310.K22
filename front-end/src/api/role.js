import request from '@/utils/request';

export function getAllRoles() {
  return request({
    url: '/api/roles',
    method: 'get'
  });
}
export function changeRole(data) {
  return request({
    url: '/api/roles',
    method: 'post',
    data
  });
}
