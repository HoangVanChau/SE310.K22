import request from '@/utils/request';

export function getProvinces() {
  return request({
    url: '/api/provinces',
    method: 'get'
  });
}
export function getDistricts(provinceId) {
  const params = {
    provinceId: provinceId
  };
  return request({
    url: '/api/districts',
    method: 'get',
    params
  });
}
export function getWards(districtId) {
  const params = {
    districtId: districtId
  };
  return request({
    url: '/api/wards',
    method: 'get',
    params
  });
}
