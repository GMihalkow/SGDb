<template>
  <div id="app" class="main-container">
    <Nav/>
    <div class="main">
      <router-view/>
    </div>
    <Footer/>
  </div>
</template>

<script>
  import Nav from './components/layout/Nav';
  import Footer from './components/layout/Footer';

  export default {
    name: 'App',
    components: {
      Nav: Nav,
      Footer: Footer
    },
    beforeCreate: function() {
      this.$store.dispatch('setAuthHeader');
    }
  };
</script>

<style>
  .main {
    min-height: 100%;
    max-height: unset;
    margin: 0 auto;
  }

  .main-container {
    height: 100%;
  }
</style>