import { loginByUsername } from '@/api/login';
import { getToken, setToken, removeToken } from '@/utils/auth';
import { getCurUser, updateCurUser, getAllUsers } from '../../api/user';

const user = {
  state: {
    curUser: {},
    token: getToken(),
    roles: [],
    setting: {
      articlePlatform: []
    },
    users: []
  },

  mutations: {
    SET_TOKEN: (state, token) => {
      state.token = token;
    },
    SET_INTRODUCTION: (state, introduction) => {
      state.introduction = introduction;
    },
    SET_SETTING: (state, setting) => {
      state.setting = setting;
    },
    SET_USER: (state, user) => {
      state.curUser = user;
    },
    SET_ROLES: (state, roles) => {
      state.roles = roles;
    },
    SET_USERS: (state, users) => {
      state.users = users;
    }
  },

  actions: {
    // 用户名登录
    LoginByUsername({ commit }, userInfo) {
      const username = userInfo.username.trim();
      return new Promise((resolve, reject) => {
        loginByUsername(username, userInfo.password)
          .then(response => {
            const data = response;
            commit('SET_TOKEN', data.accessToken);
            setToken(data.accessToken);
            resolve();
          })
          .catch(error => {
            reject(error);
          });
      });
    },

    GetUserInfo({ commit, state }) {
      return new Promise(resolve => {
        getCurUser().then(user => {
          commit('SET_USER', user);
          commit('SET_ROLES', [user.role]);
          resolve(user);
        });
      });
    },
    UpdateCurUser({ commit, state }, updatedUser) {
      return new Promise(resolve => {
        updateCurUser(updatedUser).then(user => {
          commit('SET_USER', user);
          commit('SET_ROLES', [user.role]);
          resolve(user);
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
        console.log('@@@ state.token', state.token);
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
    ChangeRoles({ commit, dispatch }, role) {
      return new Promise(resolve => {
        commit('SET_TOKEN', role);
        setToken(role);
        getCurUser().then(response => {
          const data = response.data;
          commit('SET_ROLES', data.roles);
          // commit('SET_NAME', data.name);
          // commit('SET_AVATAR', data.avatar);
          // commit('SET_INTRODUCTION', data.introduction);
          dispatch('GenerateRoutes', data); // 动态修改权限后 重绘侧边菜单
          resolve();
        });
      });
    },
    GetAllUser({ commit, dispatch }) {
      return new Promise(resolve => {
        getAllUsers().then(res => {
          commit('SET_USERS', res);
          resolve(res);
        });
      });
    }
  }
};

export default user;
