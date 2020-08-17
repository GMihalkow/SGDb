import Vue from 'vue';
import Router from 'vue-router';
import Home from './views/Home';
import Register from './views/identity/Register';
import Login from './views/identity/Login';
import ChangePassword from './views/identity/ChangePassword';
import CreatorsSearch from './views/admin/CreatorsSearch';
import GamesSearch from './views/admin/GamesSearch';
import PublishersSearch from './views/admin/PublishersSearch';
import GenresSearch from './views/admin/GenresSearch';
import FeaturedGames from './views/games/FeaturedGames';
import GameDetails from './views/games/GameDetails';
import UserViewedGamesHistory from './views/games/UserViewedGamesHistory';

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
      path: '/identity/changepassword',
      name: 'changePassword',
      component: ChangePassword,
      meta: { authenticate: true }
    },
    {
      path: '/admin/creators/search',
      name: 'creatorsSearch',
      component: CreatorsSearch,
      meta: { authenticate: true, authorize: [roles.Administrator] } 
    },
    {
      path: '/admin/games/search',
      name: 'gamesSearch',
      component: GamesSearch,
      meta: { authenticate: true, authorize: [roles.Administrator, roles.Creator] }
    },
    {
      path: '/admin/publishers/search',
      name: 'publishersSearch',
      component: PublishersSearch,
      meta: { authenticate: true, authorize: [roles.Administrator, roles.Creator] }
    },
    {
      path: '/admin/genres/search',
      name: 'genresSearch',
      component: GenresSearch,
      meta: { authenticate: true, authorize: [roles.Administrator, roles.Creator] }
    },
    {
      path: '/games/featured',
      name: 'featuredGames',
      component: FeaturedGames
    },
    {
      path: '/games/details/:id',
      name: 'gameDetails',
      component: GameDetails,
      meta: { authenticate: true } 
    },
    {
      path: '/games/userViewedGamesHistory',
      name: 'userViewedGamesHistory',
      component: UserViewedGamesHistory,
      meta: { authenticate: true }
    }
  ]
});

router.beforeEach(function(to, from, next) {
  const { authorize, authenticate } = to.meta;

  var isLoggedIn = authStore.getters.isLoggedIn;
  var role = authStore.getters.role;

  if ((to.fullPath === '/identity/login' || to.fullPath === '/identity/register') && isLoggedIn) {
    return next({ path: '/' });
  }

  if (authenticate && !isLoggedIn) {
    return next({ path: '/identity/login' });
  }

  if (authenticate && authorize && authorize.indexOf(role) < 0) {
    return next({ path: '/' }); 
  }

  next();
});

export default router;