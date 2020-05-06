import { getProvinces, getDistricts, getWards } from '../../api/address';

const address = {
  state: {
    provinces: [],
    districts: [],
    wards: []
  },
  mutations: {
    SET_PROVINCES: (state, provinces) => {
      state.provinces = provinces;
    },
    SET_DISTRICTS: (state, districts) => {
      state.districts = districts;
    },
    SET_WARDS: (state, wards) => {
      state.wards = wards;
    }
  },
  actions: {
    GetProvinces({ commit, dispatch }) {
      return new Promise(resolve => {
        getProvinces().then(res => {
          commit('SET_PROVINCES', res);
          resolve(res);
        });
      });
    },
    GetDistricts({ commit, dispatch }, provinceId) {
      return new Promise(resolve => {
        getDistricts(provinceId).then(res => {
          commit('SET_DISTRICTS', res);
          resolve(res);
        });
      });
    },
    GetWards({ commit, dispatch }, districtId) {
      return new Promise(resolve => {
        getWards(districtId).then(res => {
          commit('SET_WARDS', res);
          resolve(res);
        });
      });
    }
  }
};
export default address;
