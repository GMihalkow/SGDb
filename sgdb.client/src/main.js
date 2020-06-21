import Vue from 'vue';
import Vuelidate from 'vuelidate';
import App from './App';
import router from './router';
import store from './stores/auth-store';
import './css/global.css';
import 'primevue/resources/primevue.min.css';
import 'primeicons/primeicons.css';
import 'primevue/resources/themes/vela-blue/theme.css';

Vue.config.productionTip = false;

Vue.use(Vuelidate);

new Vue({
  router: router,
  store: store,
  render: h => h(App),
}).$mount('#app');