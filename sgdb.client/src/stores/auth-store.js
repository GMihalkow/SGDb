import Vue from 'vue';
import Vuex from 'vuex';
import axios from 'axios';
import router from '../router';
import authApi from '../api/auth-api';

Vue.use(Vuex);

const AUTHENTICATE = 'AUTHENTICATE';
const AUTHENTICATE_SUCCESS = 'AUTHENTICATE_SUCCESS';
const LOGOUT = 'LOGOUT';

const store = new Vuex.Store({
  state: {
    isLoggedIn: !!localStorage.getItem('auth_token'),
    pending: false
  },
  mutations: {
    [AUTHENTICATE] (state) {
      state.pending = true;
    },
    [AUTHENTICATE_SUCCESS] (state) {
      state.isLoggedIn = true;
      state.pending = false;

      router.push('/');
    },
    [LOGOUT](state) {
      state.isLoggedIn = false;
      axios.defaults.headers.common['Authorization'] = '';

      router.push('/');
    }
  },
  actions: {
    login({ commit }, { creds, errorCallback }) {
      commit(AUTHENTICATE); // TODO [GM]: show spinner

      return new Promise(resolve => {
        authApi.login(creds).then(function(res) {
            var data = res.data;

            if (data.succeeded) {
                var token = data.data;
                
                localStorage.setItem('auth_token', token);

                axios.defaults.headers.common['Authorization'] = 'Bearer ' + token;
                
                commit(AUTHENTICATE_SUCCESS);
            }
        }).catch(function(error) {
            if (errorCallback) {
              errorCallback(error);
            }
        });
      });
    },
    register({ commit }, { creds, errorCallback }) {
      commit(AUTHENTICATE); // TODO [GM]: show spinner

      return new Promise(resolve => {
        authApi.register(creds).then(function(res) {
            var data = res.data;

            if (data.succeeded) {
                var token = data.data;
                
                localStorage.setItem('auth_token', token);

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
      localStorage.removeItem('auth_token');

      commit(LOGOUT);
    }
  },
  getters: {
    isLoggedIn: function(state) {
      return state.isLoggedIn;
    }
  }
});  

export default store;