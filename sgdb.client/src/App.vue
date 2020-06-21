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
    }
  }
</script>

<style>
  .main {
    max-height: 100%;
    max-width: 60rem;
    margin: 0 auto;
  }

  .main-container {
    height: 100%;
  }
</style>