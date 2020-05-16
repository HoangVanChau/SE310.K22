import request from '@/utils/request';

export function getAllContract() {
  return request({
    url: '/api/contracts',
    method: 'get'
  });
}
export function getContract(contactId) {
  return request({
    url: `/api/contracts/${contactId}`,
    method: 'get'
  });
}
export function updateContract(contactId, dataParam) {
  const data = {
    ContractName: dataParam.contractName,
    UserId: dataParam.userId,
    TeamId: dataParam.teamId,
    PositionId: dataParam.positionId,
    StartDate: dataParam.startDate, // '2020-03-29T13:34:00.000'
    EndDate: dataParam.endDate,
    Active: true
  };
  return request({
    url: `/api/contracts/${contactId}`,
    method: 'patch',
    data
  });
}
export function createContract(dataParam) {
  const data = {
    ContractName: dataParam.contractName,
    UserId: dataParam.userId,
    TeamId: dataParam.teamId,
    PositionId: dataParam.positionId,
    StartDate: dataParam.startDate, // '2020-03-29T13:34:00.000'
    EndDate: dataParam.endDate,
    Active: dataParam.active
  };

  return request({
    url: `/api/contracts`,
    method: 'post',
    data
  });
}
export function deleteContract(contactId) {
  return request({
    url: `/api/contracts/${contactId}`,
    method: 'delete'
  });
}
