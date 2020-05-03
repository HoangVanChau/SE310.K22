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
    },
    DELETE_TEAM: (state, teamIndex) => {
      state.teams.splice(teamIndex, 1);
    }
  },
  actions: {
    GetAllTeam({ commit }) {
      return new Promise(resolve => {
        getAllTeam()
          .then(response => {
            const items = response;
            commit('SET_TEAMS', items);
            resolve(items);
          })
          .catch(err => console.log(err));
      });
    },
    UpdateTeam({ commit, dispatch }, data) {
      return new Promise(resolve => {
        updateTeam(data.teamId, data.newTeam)
          .then(res => {
            commit('SET_TEAM', res);
            dispatch('GetAllTeam');
            resolve(res);
          })
          .catch(err => console.log(err));
      });
    },
    CreateTeam({ commit, dispatch }, data) {
      return new Promise(resolve => {
        createTeam(data)
          .then(res => {
            commit('SET_TEAM', res);
            dispatch('GetAllTeam');
            resolve(res);
          })
          .catch(err => console.log(err));
      });
    },
    AddUserToTeam({ commit, dispatch }, data) {
      return new Promise(resolve => {
        addUserToTeam(data.teamId, data.userId)
          .then(res => {
            commit('SET_TEAM', res);
            dispatch('GetAllTeam');
            resolve(res);
          })
          .catch(err => console.log(err));
      });
    },
    DeleteSoftTeam({ commit, dispatch }, teamIndex) {
      return new Promise(resolve => {
        commit('DELETE_TEAM', teamIndex);
        resolve(this.state.teams);
      }).catch(err => console.log(err));
    }
  }
};

export default team;
