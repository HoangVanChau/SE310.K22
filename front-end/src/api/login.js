import request from '@/utils/request';

export function loginByUsername(email, password) {
  const data = {
    email,
    password
  };
  return request({
    url: '/api/auth/login',
    method: 'post',
    data
  });
}

export function getUserInfo(id) {
  console.log(id);

  return request({
    url: `/api/user/${id}`,
    method: 'get'
  });
}
