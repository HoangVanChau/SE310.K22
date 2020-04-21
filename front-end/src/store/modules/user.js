import { loginByUsername, getUserInfo } from '@/api/login';
import { updateUser, deleteUser, addUser, uploadAvatar } from '@/api/user';
import { getToken, setToken, removeToken } from '@/utils/auth';
import jwt from 'jsonwebtoken';

const user = {
  state: {
    account: {},
    userId: '',
    code: '',
    token: getToken(),
    roles: [],
    newAvatar: {}
  },

  mutations: {
    SET_CODE: (state, code) => {
      state.code = code;
    },
    SET_TOKEN: (state, token) => {
      state.token = token;
    },
    SET_USERID: (state, id) => {
      state.userId = id;
    },
    SET_ROLES: (state, roles) => {
      state.roles = roles;
    },
    SET_ACCOUNT: (state, account) => {
      state.account = account;
    },
    SET_AVATAR: (state, filename) => {
      state.newAvatar = filename;
    }
  },

  actions: {
    LoginByUsername({ commit }, userInfo) {
      const email = userInfo.email.trim();
      return new Promise((resolve, reject) => {
        loginByUsername(email, userInfo.password)
          .then(response => {
            const data = response.data;
            const decode = jwt.decode(data.token);
            console.log('@@@ data', data);
            commit('SET_TOKEN', data.token);
            setToken(data.token);
            commit('SET_USERID', decode.sub);
            resolve();
          })
          .catch(error => {
            reject(error);
          });
      });
    },

    GetUserInfo({ commit, state }, id) {
      return new Promise((resolve, reject) => {
        getUserInfo(id)
          .then(response => {
            const user = response.data;
            commit('SET_ACCOUNT', user);
            commit('SET_ROLES', user.roles);
            commit('SET_AVATAR', user.avatar);
            commit('SET_USERID', id);
            resolve(user);
          })
          .catch(err => {
            reject(err);
          });
      });
    },

    // 第三方验证登录
    // LoginByThirdparty({ commit, state }, code) {
    //   return new Promise((resolve, reject) => {
    //     commit('SET_CODE', code)
    //     loginByThirdparty(state.status, state.email, state.code).then(response => {
    //       commit('SET_TOKEN', response.data.token)
    //       setToken(response.data.token)
    //       resolve()
    //     }).catch(error => {
    //       reject(error)
    //     })
    //   })
    // },

    // 登出
    LogOut({ commit, state }) {
      return new Promise((resolve, reject) => {
        commit('SET_TOKEN', '');
        commit('SET_ROLES', []);
        removeToken();
        resolve();
      });
    },

    // 前端 登出
    FedLogOut({ commit }) {
      return new Promise(resolve => {
        commit('SET_TOKEN', '');
        removeToken();
        resolve();
      });
    },

    // 动态修改权限
    ChangeRoles({ commit, dispatch }, id) {
      return new Promise(resolve => {
        commit('SET_USERID', id);
        getUserInfo(id).then(response => {
          const data = response.data;
          commit('SET_ROLES', data.roles);
          commit('SET_ACCOUNT', data);
          dispatch('GenerateRoutes', data); // 动态修改权限后 重绘侧边菜单
          resolve();
        });
      });
    },

    // update user
    UpdateUser({ commit, dispatch }, updatedUser) {
      console.log('updatedUser.file', updatedUser.file);

      return new Promise((resolve, reject) => {
        uploadAvatar(updatedUser.file).then(resp => {
          updateUser(updatedUser.id, updatedUser.data)
            .then(res => {
              if (res.code !== 200) return resolve(null);
              const user = { ...res.data, avatar: resp.url };
              commit('SET_ACCOUNT', user);
              commit('SET_ROLES', user.roles);
              resolve(user);
            })
            .catch(e => reject(e));
        });
      });
    },
    // update user
    DeleteUser({ commit, dispatch }, id) {
      return new Promise((resolve, reject) => {
        deleteUser(id)
          .then(res => {
            if (res.code !== 200) return resolve(false);
            const user = res.data;
            commit('SET_ACCOUNT', user);
            commit('SET_ROLES', user.roles);
            resolve(true);
          })
          .catch(e => reject(e));
      });
    },
    // update user
    AddUser({ commit, dispatch }, data) {
      return new Promise((resolve, reject) => {
        addUser(data)
          .then(res => {
            if (res.code !== 200) return resolve(null);
            const user = res.data;
            commit('SET_ACCOUNT', user);
            commit('SET_ROLES', user.roles);
            resolve(user);
          })
          .catch(e => reject(e));
      });
    },
    UpdateAvatar({ commit, dispatch }, file) {
      return new Promise(function(resolve, reject) {
        commit('SET_AVATAR', file);
        resolve(file);
        // uploadAvatar(file)
        //   .then(response => {
        //     resolve(response);
        //   })
        //   .catch(e => reject(e));
      });
    }
  }
};

export default user;
