import { getAllRoles, changeRole } from '../../api/role';

const role = {
  state: {
    roles: []
  },
  mutations: {
    SET_ROLES: (state, roles) => {
      state.roles = roles;
    }
  },
  actions: {
    GetAllRole({ commit, dispatch }) {
      return new Promise((resolve, reject) => {
        getAllRoles()
          .then(result => {
            commit('SET_ROLES', result);
            resolve(result);
          })
          .catch(err => console.error(err));
      });
    },
    ChangeRole({ commit, dispatch }, data) {
      return new Promise(resolve => {
        changeRole(data).then(res => {
          dispatch('GetUserInfo');
          resolve(res);
        });
      });
    }
  }
};
export default role;
