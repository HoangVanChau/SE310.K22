import axios from 'axios';
import store from '@/store';
import { getToken } from '@/utils/auth';
import { environment } from '../environment/environment';
import { Message, MessageBox } from 'element-ui';
// import { removeToken } from './auth';
// create an axios instance
const service = axios.create({
  baseURL: environment.basePath, // api base_url
  headers: {
    'Content-Type': 'application/json;charset=UTF-8',
    'Access-Control-Allow-Origin': '*',
    Accept: '*/*'
  },
  timeout: 1000
});

// request interceptor
service.interceptors.request.use(
  config => {
    if (store.getters.token) {
      config.headers['Authorization'] = `Bearer ${getToken()}`;
      config.headers['Content-Type'] = `application/json`;
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
    const res = response;

    if (res.status !== 200) {
      Message({
        message: res.message,
        type: 'error',
        duration: 5 * 1000
      });
      // 50008:Illegal token; 50012:Other clients have logged in;  50014:Token expired;
      if (
        res.status === 500 ||
        res.status === 404 ||
        res.status === 400 ||
        res.status === 401
      ) {
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
              // removeToken();
              store.dispatch('LogOut');
              location.reload();
            }
          }
        );
      }
      return 'error';
    } else {
      return response.data;
    }
  },
  error => {
    console.log('err.response', error.response); // for debug
    if (error.response.status === 401) {
      MessageBox.confirm(
        'You have been logged out, you can cancel to stay on this page, or log in again',
        'Logout',
        {
          confirmButtonText: 'log out',
          cancelButtonText: 'cancel',
          type: 'warning',
          callback: () => {
            // removeToken();
            store.dispatch('LogOut');
            location.reload();
          }
        }
      );
    } else {
      const message = error.response.data
        ? error.response.data.message
        : error.response.statusText;

      Message({
        message: message,
        type: 'error',
        duration: 5 * 1000
      });
    }
    // return Promise.reject(error);
  }
);

export default service;
