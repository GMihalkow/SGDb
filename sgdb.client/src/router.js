import Vue from 'vue';
import Router from 'vue-router';
import Home from './views/Home';
import About from './views/About';
import Register from './views/Identity/Register';
import Login from './views/Identity/Login';

Vue.use(Router);

export default new Router({
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
    }
  ]
})