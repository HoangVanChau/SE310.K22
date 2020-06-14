import request from '@/utils/request';

export function readFile(file, name = 'file') {
  const params = {
    file
  };
  return request({
    url: '/api/attendances/excels',
    method: 'get',
    params
  });
}

export function getDataByIdAndDate(dataParam) {
  const params = {
    FromDate: dataParam.fromDate,
    ToDate: dataParam.toDate,
    EmployeeId: dataParam.employeeId
  };
  return request({
    url: '/api/attendances',
    method: 'get',
    params
  });
}
export function importJson(jsonString) {
  const data = JSON.parse(jsonString);
  return request({
    url: '/api/attendances',
    method: 'post',
    data
  });
}
export function getAttendances(id) {
  return request({
    url: '/api/attendances/'.concat(id),
    method: 'get'
  });
}
export function updateAttendances(id, jsonString) {
  const data = JSON.parse(jsonString);
  return request({
    url: '/api/attendances/'.concat(id),
    method: 'put',
    data
  });
}
export function deleteAttendances(id) {
  return request({
    url: '/api/attendances/'.concat(id),
    method: 'delete'
  });
}
// export function importAndInsert(file) {
//   const formData = new FormData();
//   formData.append('file', file);
//   return request({
//     url: '/api/attendances/excels',
//     method: 'post',
//     formData
//   });
// }
import axios from 'axios';
import { environment } from '../environment/environment';
import { Notification } from 'element-ui';
import { getToken } from '@/utils/auth';
export function importAndInsert(file) {
  const data = new FormData();
  data.append('file', file);
  const config = {
    headers: {
      'content-type': 'multipart/form-data',
      Authorization: `Bearer ${getToken()}`,
      // 'Access-Control-Allow-Origin': '*',
      Accept: '*/*'
    }
  };
  return axios
    .post(`${environment.basePath}/api/attendances/excels`, data, config)
    .catch(error => {
      const message = error.response.data
        ? error.response.data.message
        : error.response.statusText;
      console.log(error);

      Notification({
        message: message,
        type: 'error',
        duration: 5 * 1000
      });
    });
}
