import Vue from 'vue';
import Vuex from 'vuex';
import app from './modules/app';
import errorLog from './modules/errorLog';
import permission from './modules/permission';
import tagsView from './modules/tagsView';
import user from './modules/user';
import getters from './getters';
// import * as Cookies from 'js-cookie';
// import VuexPersistence from 'vuex-persist';

Vue.use(Vuex);

// const vuexCookie = new VuexPersistence({
//   supportCircular: true,
//   restoreState: (key, storage) => {
//     console.log('cooo', Cookies.getJSON(key));

//     return Cookies.getJSON(key);
//   },
//   saveState: (key, state, storage) =>
//     Cookies.set(key, state, {
//       expires: 3
//     }),
//   modules: ['user', 'tagsView', 'permission', 'errorLog', 'app']
// });

// const vuexLocal = new VuexPersistence({
//   storage: window.localStorage
// });

const store = new Vuex.Store({
  modules: {
    app,
    errorLog,
    permission,
    tagsView,
    user
  },
  getters,
  // plugins: [vuexLocal.plugin]
});

export default store;
