import {
  readFile,
  getDataByIdAndDate,
  importJson,
  getAttendances,
  updateAttendances,
  importAndInsert,
  deleteAttendances
} from '../../api/attendance';

const attendance = {
  state: {
    attendances: []
  },
  mutations: {
    SET_ATTENDANCES: (state, attendances) => {
      state.attendances = attendances;
    }
  },
  actions: {
    ReadExcel({ commit, state }, file) {
      return new Promise(resolve => {
        readFile(file).then(res => {
          commit('SET_ATTENDANCES', res);
          resolve(res);
        });
      });
    },
    GetDataByIdAndDate({ commit, state }, params) {
      return new Promise(resolve => {
        getDataByIdAndDate(params).then(res => {
          console.log('getDateByIdAnDate :>> ', res);
          resolve(res);
        });
      });
    },
    ImportJson({ commit, state }, jsonString) {
      return new Promise(resolve => {
        importJson(jsonString).then(res => {
          console.log('importJson :>> ', res);
          resolve(res);
        });
      });
    },
    GetAttendance({ commit, state }, id) {
      return new Promise(resolve => {
        getAttendances(id).then(res => {
          resolve(res);
        });
      });
    },
    UpdateAttendance({ commit, state }, params) {
      return new Promise(resolve => {
        updateAttendances(params.id, params.jsonString).then(res => {
          console.log('updateAttendances :>> ', res);
          resolve(res);
        });
      });
    },
    DeleteAttendances({ commit, dispatch }, id) {
      return new Promise(resolve => {
        deleteAttendances(id).then(res => {
          resolve(res);
        });
      });
    },
    ImportAndInsert({ commit, state }, file) {
      return new Promise(resolve => {
        importAndInsert(file).then(res => {
          console.log('importAndInsert :>> ', res);
          resolve(res);
        });
      });
    }
  }
};
export default attendance;
