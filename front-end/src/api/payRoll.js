import request from '@/utils/request';

export function getAllPayRoll() {
  return request({
    url: '/api/payrolls',
    method: 'get'
  });
}
export function getPayRollByUserId(userId) {
  return request({
    url: '/api/payrolls/' + userId,
    method: 'get'
  });
}
