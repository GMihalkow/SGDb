import Vue from 'vue';
import App from './App.vue';
import router from './router';
import './css/global.css';
import '../node_modules/primeicons/primeicons.css';
import '../node_modules/primevue/resources/primevue.min.css';
import '../node_modules/primevue/resources/themes/vela-blue/theme.css';

Vue.config.productionTip = false;

new Vue({
  router: router,
  render: h => h(App),
}).$mount('#app');