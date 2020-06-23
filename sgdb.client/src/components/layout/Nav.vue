<template>
    <div class="nav-container main-bg-dark">
        <nav class="nav">
            <router-link to="/">Home</router-link>
            <router-link to="/about">About</router-link>

            <a v-if="isLoggedIn" href="javascript:void(0)" v-on:click="test()">Test</a>

            <div class="p-fluid d-flex">
                <AutoComplete v-model="selectedGameName" :suggestions="gameNames" @complete="searchGame($event)" placeholder="Search for games by name..." field="name" />
                <Button icon="pi pi-search" class="p-button-primary p-button" />    
            </div>

            <router-link v-if="!isLoggedIn" to="/identity/login">Login</router-link>
            <router-link v-if="!isLoggedIn" to="/identity/register">Register</router-link>
            <a href="javascript:void(0)" v-if="isLoggedIn" v-on:click="logout()">Logout</a>
        </nav>
    </div>
</template>

<script>
    import AutoComplete from 'primevue/autocomplete';
    import Button from 'primevue/button';

    import baseApi from '../../api/base-api.js';
    
    export default {
        name: 'Nav',
        data() {
            return {
                selectedGameName: null,
                gameNames: []
            }
        },
        components: {
            Button: Button,
            AutoComplete: AutoComplete
        },
        methods: {
            async searchGame(event) {
            //     var result = (await baseApi.getAllForAutoComplete());

            //     console.log(result);

            //     this.gameNames = result.data.data.filter(function(g) { return g.name.toLowerCase().includes(event.query.toLowerCase()); /* IE11 support*/ });
            },
            logout() {
                this.$store.dispatch('logout');
            },
            test() {
                baseApi.get('https://localhost:5003/api/identity/test').then(function(res) {
                    console.log(res);
                }).catch(function(error) {
                    console.log(error);
                });
            }
        },
        computed: {
            isLoggedIn() {
                return this.$store.getters.isLoggedIn;
            }
        }
    }
</script>

<style scoped>
    .nav {
        display: flex;
        color: #fff;
        justify-content: center;
        flex-wrap: wrap;
        max-width: 60rem;
        margin: 0 auto;
    }

    .nav-container {
        width: 100%;
    }

    .p-button {
        margin: 0.6rem 0;
    }

    .p-autocomplete {
        display: block;
        margin: 0.6rem 0;
        min-width: 30rem;
    }

    .nav a {
        padding: 1rem 0.5rem;
        text-decoration: none;
        color: #fff;
        font-weight: bold;
        outline: none;
    }
</style>