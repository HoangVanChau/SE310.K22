import request from '@/utils/request'

export function fetchList(query) {
  return request({
    url: '/api/department',
    method: 'get',
    params: query
  })
}

export function fetchArticle(id) {
  return request({
    url: '/api/department',
    method: 'get',
    params: { id }
  })
}

export function fetchPv(pv) {
  return request({
    url: '/api/team',
    method: 'get',
    params: { pv }
  })
}

export function createArticle(data) {
  return request({
    url: '/api/department',
    method: 'post',
    data
  })
}

export function updateArticle(data) {
  return request({
    url: '/article/update',
    method: 'post',
    data
  })
}
