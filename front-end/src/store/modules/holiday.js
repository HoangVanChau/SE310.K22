import { getAll, deleteHoliday, addHoliday } from '../../api/holiday';

const holiday = {
  state: {
    holidays: []
  },
  mutations: {
    SET_HOLIDAYS: (state, holidays) => {
      state.holidays = holidays;
    }
  },
  actions: {
    GetAllHoliday({ commit }) {
      return new Promise(resolve => {
        getAll().then(result => {
          commit('SET_HOLIDAYS', result);
          resolve(result);
        });
      });
    },
    DeleteHoliday({ commit, state }, id) {
      return new Promise(resolve => {
        deleteHoliday(id).then(result => {
          const holidays = state.holidays.filter(item => item.id !== id);
          commit('SET_HOLIDAYS', holidays);
          resolve(result);
        });
      });
    },
    AddHoliday({ commit, state }, data) {
      return new Promise(resolve => {
        addHoliday(data).then(result => {
          var holidays = state.holidays;
          holidays.push(result);
          commit('SET_HOLIDAYS', holidays);
          resolve(result);
        });
      });
    }
  }
};
export default holiday;
