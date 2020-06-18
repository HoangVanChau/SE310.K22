import request from '@/utils/request';

export function getAll() {
  return request({
    url: '/api/holidays',
    method: 'get'
  });
}
export function addHoliday(dataParam) {
  const data = {
    Date: dataParam.date,
    Description: dataParam.description,
  };

  return request({
    url: `/api/holidays`,
    method: 'post',
    data
  });
}
export function deleteHoliday(id) {
  return request({
    url: `/api/holidays/${id}`,
    method: 'delete'
  });
}
