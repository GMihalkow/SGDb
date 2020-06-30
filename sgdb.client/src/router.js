import Vue from 'vue';
import Router from 'vue-router';
import Home from './views/Home';
import About from './views/About';
import Register from './views/identity/Register';
import Login from './views/identity/Login';
import CreatorsSearch from './views/admin/CreatorsSearch';

import authStore from './stores/auth-store';
import { roles } from './helpers/constants/roles';

Vue.use(Router);

var router = new Router({
  mode: 'history',
  routes: [
    {
      path: '/',
      name: 'home',
      component: Home
    },
    {
      path: '/about',
      name: 'about',
      component: About
    },
    {
      path: '/identity/register',
      name: 'register',
      component: Register
    },
    {
      path: '/identity/login',
      name: 'login',
      component: Login
    },
    {
      path: '/admin/creators/search',
      name: 'creatorsSearch',
      component: CreatorsSearch,
      meta: { authenticate: true, authorize: [roles.Administrator] } 
    },
  ]
});

// TODO [GM]: add authorization
router.beforeEach(function(to, from, next) {
  const { authorize, authenticate } = to.meta;

  var isLoggedIn = authStore.getters.isLoggedIn;
  var role = authStore.getters.role;

  if (authenticate && !isLoggedIn) {
    return next({ path: '/identity/login' });
  }

  if (authenticate && authorize && authorize.indexOf(role) < 0) {
    return next({ path: '/' }); 
  }

  next();
});

export default router;