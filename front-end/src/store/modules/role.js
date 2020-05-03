import { getAllRoles } from '../../api/role';

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
    }
  }
};
export default role;
