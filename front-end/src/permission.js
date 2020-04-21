/* eslint-disable object-curly-spacing */
import router from './router';
import store from './store';
import { Message } from 'element-ui';
import NProgress from 'nprogress'; // progress bar
import 'nprogress/nprogress.css'; // progress bar style
import { getToken } from '@/utils/auth'; // getToken from cookie
import jwt from 'jsonwebtoken'

NProgress.configure({ showSpinner: false }); // NProgress Configuration

// permission judge function
function hasPermission(roles, permissionRoles) {
  if (roles.indexOf('admin') >= 0) return true; // admin permission passed directly
  if (!permissionRoles) return true;
  return roles.some(role => permissionRoles.indexOf(role) >= 0);
}

const whiteList = ['/login', '/auth-redirect']; // no redirect whitelist

router.beforeEach((to, from, next) => {
  NProgress.start(); // start progress bar
  if (getToken()) {
    // determine if there has token
    /* has token*/
    if (to.path === '/login') {
      next({ path: '/' });
      NProgress.done(); // if current page is dashboard will not trigger	afterEach hook, so manually handle it
    } else {
      if (store.getters.roles.length === 0) {
        console.log('@@@ userId', store.getters.userId);
        var token = getToken()
        var userId = jwt.decode(token).sub
        store
          .dispatch('GetUserInfo', userId)
          .then(res => {
            const roles = res.roles;
            // note: roles must be a array! such as: ['editor','develop']

            store
              .dispatch('GenerateRoutes', roles)
              .then(() => {
                // 根据roles权限生成可访问的路由表
                router.addRoutes(store.getters.addRouters); // 动态添加可访问路由表
                next({ ...to, replace: true }); // hack方法 确保addRoutes已完成 ,set the replace: true so the navigation will not leave a history record
              })
              .catch(e => console.log(e));
          })
          .catch(err => {
            store.dispatch('FedLogOut').then(() => {
              Message.error(err || 'Verification failed, please login again');
              next({ path: '/' });
            });
          });
      } else {
        // 没有动态改变权限的需求可直接next() 删除下方权限判断 ↓
        if (hasPermission(store.getters.roles, to.meta.roles)) {
          next();
        } else {
          next({ path: '/401', replace: true, query: { noGoBack: true } });
        }
        // 可删 ↑
      }
    }
  } else {
    /* has no token*/
    if (whiteList.indexOf(to.path) !== -1) {
      // 在免登录白名单，直接进入
      next();
    } else {
      next(`/login?redirect=${to.path}`); // 否则全部重定向到登录页
      NProgress.done(); // if current page is login will not trigger afterEach hook, so manually handle it
    }
  }
});

router.afterEach(() => {
  NProgress.done(); // finish progress bar
});
