import { upload } from '../../api/upload';
// import { environment } from '../../environment/environment';

const image = {
  state: {
    imageId: '',
    file: null
  },
  mutations: {
    SET_IMAGE_ID: (state, imageId) => {
      state.imageId = imageId;
    },
    SET_FILE: (state, file) => {
      state.file = file;
    }
  },
  actions: {
    UploadImage({ commit, dispatch }, file) {
      return new Promise(resolve => {
        upload(file).then(res => {
          const imageId = res;
          commit('SET_IMAGE_ID', imageId.data);
          console.log('UploadImage', imageId.data);
          // const link = `${environment.basePath}/api/images?id=${res}`;
          resolve(imageId.data);
        });
      });
    },
    PreviewImage({ commit, dispatch }, file) {
      return new Promise(resolve => {
        commit('SET_FILE', file);
        resolve(true);
      });
    },
    ResetFile({ commit, dispatch }) {
      return new Promise(resolve => {
        commit('SET_IMAGE_ID', '');
        commit('SET_FILE', null);
      });
    }
  }
};
export default image;
