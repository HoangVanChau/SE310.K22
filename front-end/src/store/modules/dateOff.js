import {
  getAllDateOffByUser,
  requestDateOff,
  approveDateOff
} from '../../api/dateOff';

const contact = {
  state: {
    dateOffs: [],
    allDateOffs: []
  },
  mutations: {
    SET_DATE_OFF: (state, dateOffs) => {
      state.dateOffs = dateOffs;
    },
    SET_ALL: (state, lstDateOff) => {
      state.allDateOffs = lstDateOff;
    }
  },
  actions: {
    ResetDateOffs({ commit }) {
      return new Promise(resolve => {
        commit('SET_DATE_OFF', []);
      });
    },
    GetDateOffsByUser({ commit }, params) {
      return new Promise(resolve => {
        getAllDateOffByUser(params).then(result => {
          commit('SET_DATE_OFF', result);
          resolve(result);
        });
      });
    },
    GetAll({ commit }) {
      return new Promise(resolve => {
        getAllDateOffByUser(null).then(result => {
          commit('SET_ALL', result);
          resolve(result);
        });
      });
    },
    SubmitDateOff({ commit }, params) {
      return new Promise(resolve => {
        requestDateOff(params).then(result => {
          // eslint-disable-next-line no-undef
          // const map = _.keyBy(result, item => item.userId);
          resolve(result);
        });
      });
    },
    ApproveDateOff({ commit }, params) {
      return new Promise(resolve => {
        approveDateOff(params.id, params.dataParam).then(result => {
          resolve(result);
        });
      });
    }
  }
};
export default contact;
