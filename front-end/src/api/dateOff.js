import request from '@/utils/request';

export function getAllDateOffByUser(dataParam) {
  const params = {
    status: dataParam.status || null,
    userId: dataParam.userId || null,
    teamId: dataParam.teamId || null,
    fromDate: dataParam.fromDate || null,
    toDate: dataParam.toDate || null
  };
  return request({
    url: '/api/dateoffs',
    method: 'get',
    params
  });
}
export function getDateOffById(id) {
  return request({
    url: `/api/dateoffs/${id}`,
    method: 'get'
  });
}
export function requestDateOff(dataParam) {
  const data = {
    StartOff: dataParam.startOff || null,
    EndOff: dataParam.endOff || null,
    Date: dataParam.date || null,
    Reason: dataParam.reason || null
  };
  return request({
    url: '/api/dateoffs',
    method: 'post',
    data
  });
}
export function cancelDateOff(id) {
  return request({
    url: `/api/dateoffs/${id}`,
    method: 'delete'
  });
}
export function approveDateOff(id, dataParam) {
  const data = {
    IsApprove: dataParam.isApprove || false,
    // RejectReason: dataParam.rejectReason || ''
  };
  return request({
    url: `/api/dateoffs/${id}`,
    method: 'put',
    data
  });
}
