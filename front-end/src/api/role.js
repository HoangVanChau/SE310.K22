import request from '@/utils/request';

export function getAllRoles() {
  return request({
    url: '/api/roles',
    method: 'get'
  });
}
export function changeRole(dataParam) {
  const data = {
    UserId: dataParam.userId,
    NewRole: dataParam.newRole,
  }
  return request({
    url: '/api/roles',
    method: 'post',
    data
  });
}
