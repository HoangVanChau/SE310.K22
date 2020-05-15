import request from '@/utils/request';

export function getAllPosition() {
  return request({
    url: '/api/positions',
    method: 'get'
  });
}
export function getPosition(positionId) {
  return request({
    url: `/api/positions/${positionId}`,
    method: 'get'
  });
}
export function updatePosition(positionId, dataParam) {
  const data = {
    PositionName: dataParam.positionName,
    Description: dataParam.description,
    BaseMonthSalary: dataParam.baseMonthSalary,
    BaseHourSalary: dataParam.baseHourSalary,
    BaseOtSalaryPerHour: dataParam.baseOtSalaryPerHour,
    BaseDateOff: dataParam.baseDateOff,
    BaseLateMoney: dataParam.baseLateMoney
  };
  return request({
    url: `/api/positions/${positionId}`,
    method: 'patch',
    data
  });
}
export function createPosition(dataParam) {
  const data = {
    PositionName: dataParam.positionName,
    Description: dataParam.description,
    BaseMonthSalary: dataParam.baseMonthSalary,
    BaseHourSalary: dataParam.baseHourSalary,
    BaseOtSalaryPerHour: dataParam.baseOtSalaryPerHour,
    BaseDateOff: dataParam.baseDateOff,
    BaseLateMoney: dataParam.baseLateMoney
  };

  return request({
    url: `/api/positions`,
    method: 'post',
    data
  });
}
export function deletePosition(positionId) {
  return request({
    url: `/api/positions/${positionId}`,
    method: 'delete'
  });
}
