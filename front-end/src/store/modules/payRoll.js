import { getAllPayRoll, getPayRollByUserId } from '../../api/payRoll';

const payRoll = {
  state: {
    payRolls: [],
    curPayRoll: {}
  },
  mutations: {
    SET_PAYROLLS: (state, payRolls) => {
      state.payRolls = payRolls;
    },
    SET_PAYROLL: (state, curPayRoll) => {
      state.curPayRoll = curPayRoll;
    }
  },
  actions: {
    GetAllPayRolls({ commit, dispatch }) {
      return new Promise(resolve => {
        getAllPayRoll().then(result => {
          commit('SET_PAYROLLS', result);
          resolve(result);
        });
      });
    },
    GetPayRollByUser({ commit }, userId) {
      return new Promise(resolve => {
        getPayRollByUserId(userId).then(result => {
          commit('SET_PAYROLL', result);
          resolve(result);
        });
      });
    }
  }
};
export default payRoll;
