import {
  getAllPosition,
  getPosition,
  createPosition,
  updatePosition
} from '../../api/position';

const position = {
  state: {
    positions: []
  },
  mutations: {
    SET_POSITIONS: (state, positions) => {
      state.positions = positions;
    }
  },
  actions: {
    GetAllPosition({ commit, state }) {
      return new Promise(resolve => {
        getAllPosition()
          .then(res => {
            commit('SET_POSITIONS', res);
            resolve(res);
          })
          .catch(e => console.log('getAllPosition :>> ', e));
      });
    },
    GetPosition({ commit, dispatch }, positionId) {
      return new Promise(resolve => {
        getPosition(positionId)
          .then(res => {
            resolve(res);
          })
          .catch(e => console.log('getPosition :>> ', e));
      });
    },
    CreatePosition({ commit, dispatch }, dataParam) {
      return new Promise(resolve => {
        createPosition(dataParam)
          .then(res => {
            console.log('createPosition', res);
            resolve(res);
          })
          .catch(e => console.log('createPosition :>> ', e));
      });
    },
    UpdatePosition({ commit, dispatch }, data) {
      return new Promise(resolve => {
        updatePosition(data.positionId, data.newPosition)
          .then(res => {
            resolve(res);
          })
          .catch(e => console.log('updatePosition :>> ', e));
      });
    }
  }
};
export default position;
