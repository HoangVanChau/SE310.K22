import request from '@/utils/request';

export function fetchListDepartment(query) {
  return request({
    url: '/api/department',
    method: 'get',
    params: query
  });
}

export function fetchDepartment(id) {
  return request({
    url: '/api/department/'.concat(id),
    method: 'get'
  });
}

export function createDepartment(data) {
  return request({
    url: '/api/department',
    method: 'post',
    data
  });
}

export function updateDepartment(id, data) {
  return request({
    url: '/api/department/'.concat(id),
    method: 'patch',
    data
  });
}
