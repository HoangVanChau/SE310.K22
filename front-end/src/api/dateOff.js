import request from '@/utils/request';

export function getAllDateOffByUser(dataParam) {
  const params = {
    status: dataParam.status,
    userId: dataParam.userId,
    teamId: dataParam.teamId,
    fromDate: dataParam.fromDate,
    toDate: dataParam.toDate
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
    StartOff: dataParam.startOff,
    EndOff: dataParam.endOff,
    Date: dataParam.date,
    Reason: dataParam.reason
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
    IsApprove: dataParam.isApprove,
    RejectReason: dataParam.rejectReason || '',
  };
  return request({
    url: `/api/dateoffs/${id}`,
    method: 'put',
    data
  });
}
