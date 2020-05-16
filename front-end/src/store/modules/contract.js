import {
  getAllContract,
  getContract,
  createContract,
  updateContract
} from '../../api/contract';

const contact = {
  state: {
    contracts: [],
    fields: []
  },
  mutations: {
    SET_CONTRACTS: (state, contracts) => {
      state.contracts = contracts;
    },
    SET_FIELDS: (state, fields) => {
      state.fields = fields;
    }
  },
  actions: {
    GetAllContract({ commit, state }) {
      return new Promise(resolve => {
        getAllContract()
          .then(res => {
            commit('SET_CONTRACTS', res);
            const item = res[0];
            var fields = Object.keys(item);
            commit('SET_FIELDS', fields);
            var returned = {
              fields,
              data: res
            };
            resolve(returned);
          })
          .catch(e => console.log('getAllContract :>> ', e));
      });
    },
    GetContract({ commit, dispatch }, contactId) {
      return new Promise(resolve => {
        getContract(contactId)
          .then(res => {
            resolve(res);
          })
          .catch(e => console.log('getContract :>> ', e));
      });
    },
    CreateContract({ commit, dispatch }, dataParam) {
      return new Promise(resolve => {
        createContract(dataParam)
          .then(res => {
            resolve(res);
          })
          .catch(e => console.log('createContract :>> ', e));
      });
    },
    UpdateContract({ commit, dispatch }, data) {
      return new Promise(resolve => {
        updateContract(data.contractId, data.newContract)
          .then(res => {
            resolve(res);
          })
          .catch(e => console.log('updateContract :>> ', e));
      });
    }
  }
};
export default contact;
