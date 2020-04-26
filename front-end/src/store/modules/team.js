import {
  getAllTeam,
  updateTeam,
  createTeam,
  addUserToTeam
} from '../../api/team';

const team = {
  state: {
    teams: [],
    team: {}
  },
  mutations: {
    SET_TEAMS: (state, teams) => {
      state.teams = teams;
    },
    SET_TEAM: (state, team) => {
      state.team = team;
    }
  },
  actions: {
    GetAllTeam({ commit }) {
      return new Promise(resolve => {
        getAllTeam().then(response => {
          const items = response;
          commit('SET_TEAMS', items);
          resolve(items);
        });
      });
    },
    UpdateTeam({ commit, dispatch }, data) {
      return new Promise(resolve => {
        updateTeam(data.teamId, data.newTeam).then(res => {
          commit('SET_TEAM', res);
          dispatch('GetAllTeam');
          resolve(res);
        });
      });
    },
    CreateTeam({ commit, dispatch }, data) {
      return new Promise(resolve => {
        createTeam(data).then(res => {
          commit('SET_TEAM', res);
          dispatch('GetAllTeam');
          resolve(res);
        });
      });
    },
    AddUserToTeam({ commit, dispatch }, data) {
      return new Promise(resolve => {
        addUserToTeam(data.teamId, data.userId).then(res => {
          commit('SET_TEAM', res);
          dispatch('GetAllTeam');
          resolve(res);
        });
      });
    }
  }
};

export default team;
