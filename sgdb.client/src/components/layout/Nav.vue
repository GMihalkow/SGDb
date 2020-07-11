<template>
    <div class="nav-container main-bg-dark">
        <nav class="nav">
            <router-link to="/">Home</router-link>
            <router-link to="/games/featured">Games</router-link>
            <router-link to="/about">About</router-link>

            <Menubar v-if="isLoggedIn" :model="adminMenuModel" />

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
    import Menubar from 'primevue/menubar';
    import baseApi from '../../api/base-api.js';
    import creatorsApi from '../../api/creators/creators-api';

    import { mapGetters } from 'vuex';
    import { roles } from '../../helpers/constants/roles';
    
    export default {
        name: 'Nav',
        data() {
            var _this = this;

            return {
                selectedGameName: null,
                gameNames: [],
                adminMenuModel: [
                {
                    label: 'Tools',
                    icon:'pi pi-fw pi-cog',
                    items: []
                }]
            }
        },
        components: {
            Button: Button,
            AutoComplete: AutoComplete,
            Menubar: Menubar
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
                creatorsApi.getAll().then(function(res) {
                    console.log(res);
                }).catch(function(err){ 
                    console.log(err);
                });
            }
        },
        computed: {
            ...mapGetters({'isLoggedIn': 'isLoggedIn'}),
            ...mapGetters({'role': 'role'}),
            ...mapGetters({'isUserAdmin': 'isUserAdmin'})
        },
        mounted() {
            var _this = this;

            var creatorsLinkObj = { label: 'Creators', visible: _this.isUserAdmin, to: '/admin/creators/search' };

            _this.adminMenuModel[0].items = [
                { label: '...', command:() => {} },
                { separator: true },
                { label: 'Creators', visible: _this.isUserAdmin, to: '/admin/creators/search' },
                { label: 'Games', visible: _this.isUserAdmin, to: '/admin/games/search' }
            ];
        },
        updated() {
            this.adminMenuModel[0].items = this.adminMenuModel[0].items || [];

            var separatorIndex = this.adminMenuModel[0].items.findIndex(function(menuObj) { return menuObj.separator; });

            var items = this.adminMenuModel[0].items.filter(function(menuObj, index) { return index <= separatorIndex; });

            items.push({ label: 'Creators', visible: this.isUserAdmin, to: '/admin/creators/search' });
            items.push({ label: 'Games', visible: this.isUserAdmin, to: '/admin/games/search' });

            this.adminMenuModel[0].items = items;
        }
    };
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

    .p-menubar {
        padding: 0;
        margin: 0.5rem;
    }

    .nav a {
        padding: 1rem 0.5rem;
        text-decoration: none;
        color: #fff;
        font-weight: bold;
        outline: none;
    }
</style>