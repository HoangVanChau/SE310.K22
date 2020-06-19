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
import position from './modules/position';
import contract from './modules/contract';
import attendance from './modules/attendance';
import dateOff from './modules/dateOff';
import holiday from './modules/holiday';
import payRoll from './modules/payRoll';
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
    image,
    position,
    contract,
    attendance,
    dateOff,
    holiday,
    payRoll
  },
  getters
});

export default store;