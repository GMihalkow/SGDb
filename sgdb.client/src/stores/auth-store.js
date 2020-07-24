import Vue from 'vue';
import Vuex from 'vuex';
import SecureLS from 'secure-ls';
import axios from 'axios';
import router from '../router';
import identityApi from '../api/identity/identity-api';
import { roles } from '../helpers/constants/roles';

Vue.use(Vuex);

const AUTHENTICATE = 'AUTHENTICATE';
const AUTHENTICATE_SUCCESS = 'AUTHENTICATE_SUCCESS';
const LOGOUT = 'LOGOUT';
const SETCREATORID = 'SETCREATORID';

var secureLs = new SecureLS({ encodingType: 'aes' });

const store = new Vuex.Store({
  state: {
    isLoggedIn: !!secureLs.get('auth_token'),
    role: secureLs.get('role'),
    isUserAdmin: !!secureLs.get('auth_token') && secureLs.get('role') == roles.Administrator,
    isUserCreator: !!secureLs.get('auth_token') && secureLs.get('role') == roles.Creator,
    creatorId: secureLs.get('creatorId'),
    pending: false
  },
  mutations: {
    // TODO [GM]: remove?
    [AUTHENTICATE] (state) {
      state.pending = true;
    },
    [AUTHENTICATE_SUCCESS] (state) {
      Vue.set(state, 'isLoggedIn', !!secureLs.get('auth_token'));
      
      var role = secureLs.get('role');

      Vue.set(state, 'role', secureLs.get('role'));
      
      var isUserAdmin = role === roles.Administrator;
      var isUserCreator = role === roles.Creator;

      Vue.set(state, 'isUserAdmin', !!secureLs.get('auth_token') && isUserAdmin);
      Vue.set(state, 'isUserCreator', !!secureLs.get('auth_token') && isUserCreator);

      state.pending = false;

      router.push('/');
    },
    [LOGOUT](state) {
      Vue.set(state, 'isLoggedIn', false);
      Vue.set(state, 'role', '');
      Vue.set(state, 'isUserAdmin', false);
      Vue.set(state, 'isUserCreator', false);
      
      axios.defaults.headers.common['Authorization'] = '';

      router.push('/');
    },
    [SETCREATORID](state) {
      Vue.set(state, 'creatorId', secureLs.get('creatorId'));
    }
  },
  actions: {
    authenticate({ commit }, { endpoint, creds, successCallback, errorCallback }) {
      commit(AUTHENTICATE); // TODO [GM]: show spinner

      return new Promise(function() {
        identityApi[endpoint](creds).then(function(res) {
          var data = res.data;

          if (data.succeeded) {
            var token = data.data.token;
            
            secureLs.set('auth_token', token);

            var role = data.data.role;
            secureLs.set('role', role);
            
            if (successCallback){
              successCallback();
            }

            commit(AUTHENTICATE_SUCCESS);
          }
        }).catch(function(error) {
          if (errorCallback) {
            errorCallback(error);
          }
        });
      });
    },
    logout({ commit }) {
      secureLs.remove('auth_token');
      secureLs.remove('role');

      commit(LOGOUT);
    },
    setAuthHeader({ commit }) {
      var token = secureLs.get('auth_token');

      if(token){
        axios.defaults.headers.common['Authorization'] = 'Bearer ' + token;
      }
    },
    setCreatorId({ commit }, { creatorId }) {
      secureLs.set('creatorId', creatorId);
      
      commit(SETCREATORID);
    }
  },
  getters: {
    isLoggedIn: state => state.isLoggedIn,
    role: state => state.role,
    isUserAdmin: state => state.isUserAdmin,
    isUserCreator: state => state.isUserCreator,
    creatorId: state => state.creatorId
  }
});  

export default store;