import {
  getAllTeam,
  updateTeam,
  createTeam,
  addUserToTeam,
  changeLeader,
  getTeam,
  removeUserToTeam,
  deleteTeam
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
            changeLeader(data.teamId, data.newTeam.leaderId).then(res => {
              dispatch('GetAllTeam');
              resolve(res);
            });
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
    DeleteTeam({ commit, dispatch }, teamId) {
      return new Promise(resolve => {
        deleteTeam(teamId).then(res => {
          resolve(res);
        });
      }).catch(err => console.log(err));
    },
    GetTeamByID({ commit, dispatch }, teamId) {
      return new Promise(resolve => {
        getTeam(teamId)
          .then(res => {
            commit('SET_TEAM', res);
            resolve(res);
          })
          .catch(e => console.log(e));
      });
    },
    AddUserToTeam({ commit, dispatch }, data) {
      return new Promise(resolve => {
        addUserToTeam(data.teamId, data.userId)
          .then(res => {
            commit('SET_TEAM', res);
            resolve(res);
          })
          .catch(err => console.log(err));
      });
    },
    AddUsersToTeam({ commit, dispatch }, dataParam) {
      return new Promise(resolve => {
        dataParam.members.forEach(memberId => {
          addUserToTeam(dataParam.teamId, memberId).then(res => {
            console.log('AddUsersToTeam', res);
          });
        });
        resolve(true);
      });
    },
    RemoveUserToTeam({ commit, dispatch }, data) {
      return new Promise(resolve => {
        removeUserToTeam(data.teamId, data.userId)
          .then(res => {
            console.log('RemoveUserToTeam', res);
            resolve(res);
          })
          .catch(err => console.log(err));
      });
    },
    RemoveUsersToTeam({ commit, dispatch }, dataParam) {
      return new Promise(resolve => {
        dataParam.members.forEach(memberId => {
          removeUserToTeam(dataParam.teamId, memberId).then(res => {
            console.log('RemoveUsersToTeam', res);
          });
        });
        resolve(true);
      });
    }
  }
};

export default team;
