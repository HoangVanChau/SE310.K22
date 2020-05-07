import axios from 'axios';
import { environment } from '../environment/environment';

export function upload(file, name = 'file') {
  const formData = new FormData();
  formData.append(name, file);
  const config = {
    headers: {
      'content-type': 'multipart/form-data'
    }
  };
  return axios.post(`${environment.basePath}/api/images`, formData, config);
}
