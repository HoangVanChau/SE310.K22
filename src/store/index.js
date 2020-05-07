import Vue from 'vue';
import Vuex from 'vuex';
import app from './modules/app';
import errorLog from './modules/errorLog';
import permission from './modules/permission';
import tagsView from './modules/tagsView';
import user from './modules/user';
import team from './modules/team';
import role from './modules/role';
import address from './modules/address';
import image from './modules/upload';
import getters from './getters';

Vue.use(Vuex);

const store = new Vuex.Store({
  modules: {
    app,
    errorLog,
    permission,
    tagsView,
    user,
    team,
    role,
    address,
    image
  },
  getters
});

export default store;
