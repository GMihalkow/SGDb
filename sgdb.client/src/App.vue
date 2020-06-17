<template>
  <div id="app" class="main">
    <Nav/>
    <router-view/>
  </div>
</template>

<script>
import Nav from './components/layout/Nav.vue';

export default {
  name: 'App',
  components: {
    Nav: Nav
  }
}
</script>

<style>
  .main {
    height: 100%;
  }
</style>