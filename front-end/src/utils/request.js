import axios from 'axios';
import store from '@/store';
import { getToken } from '@/utils/auth';
import { environment } from '../environment/environment';
import { Message, MessageBox } from 'element-ui';
// create an axios instance
const service = axios.create({
  baseURL: environment.basePath, // api base_url
  headers: {
    'Content-Type': 'application/json'
  },
  timeout: 1000
});

// request interceptor
service.interceptors.request.use(
  config => {
    // Do something before request is sent
    if (store.getters.token) {
      // 让每个请求携带token-- ['X-Token']为自定义key 请根据实际情况自行修改
      config.headers['X-Token'] = getToken();
    }
    return config;
  },
  error => {
    // Do something with request error
    console.log(error); // for debug
    Promise.reject(error);
  }
);

// response interceptor
service.interceptors.response.use(
  // response => response.data,
  /**
   * The following comment is to indicate the request status by customizing the code in the response
   * When the code returns the following situation, it means that there is a problem with the permissions. Log out and return to the login page
   * If you want to use xmlhttprequest to identify the status code, the logic can be written in the error below
   * The following codes are all examples, please modify them according to your own needs, if not, you can delete
   */
  response => {
    const res = response.data;
    if (res.code !== 200) {
      Message({
        message: res.message,
        type: 'error',
        duration: 5 * 1000
      });
      // 50008:Illegal token; 50012:Other clients have logged in;  50014:Token expired;
      if (res.code === 500 || res.code === 404 || res.code === 400) {
        // 请自行在引入 MessageBox
        // import { Message, MessageBox } from 'element-ui'
        MessageBox.confirm(
          'You have been logged out, you can cancel to stay on this page, or log in again',
          'Logout',
          {
            confirmButtonText: 'register',
            cancelButtonText: 'cancel',
            type: 'warning',
            callback: () => {
              location.reload();
            }
          }
        );
      }
      return Promise.reject('error');
    } else {
      return response.data;
    }
  },
  error => {
    console.log('err' + error); // for debug
    return Promise.reject(error);
  }
);

export default service;
