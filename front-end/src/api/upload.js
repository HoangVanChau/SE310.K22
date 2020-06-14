import axios from 'axios';
import { environment } from '../environment/environment';
import { Notification } from 'element-ui';
import { getToken } from '@/utils/auth';

export function upload(file, name = 'file') {
  const formData = new FormData();
  formData.append(name, file);
  const config = {
    headers: {
      'content-type': 'multipart/form-data',
      Authorization: `Bearer ${getToken()}`,
      // 'Access-Control-Allow-Origin': '*',
      Accept: '*/*'
    }
  };
  return axios
    .post(`${environment.basePath}/api/images`, formData, config)
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
